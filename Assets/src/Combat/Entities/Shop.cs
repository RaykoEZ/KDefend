using Curry.Events;
using Curry.Explore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// handles specific item selections to trade Pts for every boss cycle
[RequireComponent(typeof(HideableUI))]
public class Shop : Building 
{
    [SerializeField] HideableUI m_ui = default;
    [SerializeField] ShopPoolUpdater m_updater = default;
    [SerializeField] List<ShopUI> m_optionUI = default;
    void OnEnable()
    {
        HideAllOptons();
    }
    void OnDisable()
    {
        HideAllOptons();
    }
    // on interact, look at player inventory, change drop list depending on 
    // all obtained items
    // activate this when user chooses interact option (e.g. E button)
    protected override void Interact_Internal()
    {
        // get 3 random items to choose from
        // set to default if we have nothing
        m_updater?.UpdatePool();
        List<ItemAsset> options = m_updater.GetOptionAssetPool();
        options = SamplingUtil.SampleFromList(options, 3, uniqueResults: true);
        for (int i = 0; i < m_optionUI.Count; ++ i) 
        {
            m_optionUI[i]?.Init(options[i]);
        }
        m_ui?.Show();
        StartCoroutine(ShowOptions());
    }
    public void OnPlayerChosen(ItemAsset chosen) 
    {
        HideAllOptons();
        // update player inventory, setup item effects & trigger events for obtaining the item
        KDEventInfo args = new KDEventInfo(KD_StaticEventFlags.None, GameEventTriggerType.ItemObtained,
            new Dictionary<string, object> { { "item", chosen.Asset } });
        InternalEventHandler.TriggerGlobalEvent(this, args);
    }
    public void HideAllOptons() 
    {
        foreach (var item in m_optionUI) 
        {
            item.OnChosen -= OnPlayerChosen;
            item.Hide();
        }
        m_ui?.Hide();
    }
    IEnumerator ShowOptions() 
    { 
        foreach(var item in m_optionUI) 
        {
            item.Show();
            item.OnChosen += OnPlayerChosen;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
