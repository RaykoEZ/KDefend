using Curry.Events;
using Curry.Explore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles item option UI displays
public class ItemOptionHandler : HideableUI
{
    [SerializeField] protected List<OptionUI> m_optionUI = default;
    void OnEnable()
    {
        Hide();
        Init();
    }
    void OnDisable()
    {
        Shutdown();
        Hide();
    }
    protected void Init()
    {
        KDEventHandler.ListenToGlobal(GameEventTriggerType.ItemOption, OnShowOptions);
        // setup callbacks
        foreach (var option in m_optionUI)
        {
            InitOption(option);
        }
    }
    protected void Shutdown() 
    {
        KDEventHandler.UnlistenFromGlobal(GameEventTriggerType.ItemOption, OnShowOptions);
        foreach (var option in m_optionUI)
        {
            ShutdownOption(option);
        }
    }
    protected virtual void InitOption(OptionUI option) 
    {
        // setup callbacks
        option.OnChosen += OnPlayerChosen;     
    }
    protected virtual void ShutdownOption(OptionUI option) 
    {     
        option.OnChosen -= OnPlayerChosen;       
    }
    protected virtual void OnShowOptions(object sender, KDEventInfo args)
    {
        if (args.Payload == null) return;
        if (args.Payload.TryGetValue("options", out object result) && result is List<ItemAsset> list)
        {
            ShowOptions(list);
        }
    }
    protected virtual void ShowOptions(List<ItemAsset> options)
    {
        int size = Mathf.Min(options.Count, m_optionUI.Count);
        // setup each option callbacks
        for (int i = 0; i < size; ++i)
        {
            m_optionUI[i]?.Init(options[i]);
        }
        KDEventUtil.PauseGame(this, false);
        Show();
        StartCoroutine(ShowOptions_Internal());
    }
    protected virtual void OnPlayerChosen(OptionUI chosen)
    {
        Hide();
        KDEventUtil.PauseGame(this, true);
        // update player inventory, setup item effects & trigger events for obtaining the item
        KDEventUtil.ObtainItemEvent(this, chosen.CurrentItemRef);
    }
    protected IEnumerator ShowOptions_Internal()
    {
        foreach (var item in m_optionUI)
        {
            item.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
