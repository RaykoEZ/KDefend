using Curry.Events;
using Curry.Explore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// handles item option UI displays
public class ItemOptionHandler : HideableUI
{
    [SerializeField] List<OptionUI> m_optionUI = default;
    void OnEnable()
    {
        Hide();
        InternalEventHandler.ListenToGlobal(GameEventTriggerType.ItemOption, OnObtainItem);
        // setup callbacks
        foreach (var option in m_optionUI) 
        {
            option.OnChosen += OnPlayerChosen;
        }
    }
    void OnDisable()
    {
        InternalEventHandler.UnlistenFromGlobal(GameEventTriggerType.ItemOption, OnObtainItem);
        foreach (var option in m_optionUI)
        {
            option.OnChosen -= OnPlayerChosen;
        }
        Hide();
    }
    protected void OnObtainItem(object sender, KDEventInfo args)
    {
        if (args.Payload == null) return;
        if (args.Payload.TryGetValue("options", out object result) && result is List<Item> list)
        {
            ShowOptions(list);
        }
    }
    public void ShowOptions(List<ItemAsset> options)
    {
        int size = Mathf.Min(options.Count, m_optionUI.Count);
        // setup each option callbacks
        for (int i = 0; i < size; ++i)
        {
            m_optionUI[i]?.Init(options[i]);
        }
        KDEventUtil.PauseGame(this, false);
        Show();
        StartCoroutine(ShowOptions());
    }
    public void ShowOptions(List<Item> options)
    {
        int size = Mathf.Min(options.Count, m_optionUI.Count);
        // setup each option content
        for (int i = 0; i < size; ++i)
        {
            m_optionUI[i]?.Init(options[i]);
        }
        KDEventUtil.PauseGame(this, false);
        Show();
        StartCoroutine(ShowOptions());
    }
    protected void OnPlayerChosen(Item chosen)
    {
        Hide();
        KDEventUtil.PauseGame(this, true);
        // update player inventory, setup item effects & trigger events for obtaining the item
        KDEventInfo args = new KDEventInfo(KD_StaticEventFlags.None, GameEventTriggerType.ItemObtained,
            new Dictionary<string, object> { { "item", chosen } });
        InternalEventHandler.TriggerGlobalEvent(this, args);
    }
    IEnumerator ShowOptions()
    {
        foreach (var item in m_optionUI)
        {
            item.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
