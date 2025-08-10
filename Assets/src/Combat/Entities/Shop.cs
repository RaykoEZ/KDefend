using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// handles specific item selections to trade Pts for every boss cycle
public class Shop : Building 
{
    [SerializeField] ShopPoolUpdater m_updater = default;
    [SerializeField] ItemAssetLookup m_default = default;
    [SerializeField] List<ShopUI> m_optionUI = default;
    List<ItemAsset> m_currentList = new List<ItemAsset>();
    void OnEnable()
    {
        HideAllOptons();
    }
    void OnDisable()
    {
        HideAllOptons();
    }
    void UpdatePool() 
    {
        // set to default, add bonus pools from updater
        m_currentList = m_default.Assets;
        m_currentList.AddRange(m_updater.AddedPool);
    }
    // on interact, look at player inventory, change drop list depending on 
    // all obtained items
    // activate this when user chooses interact option (e.g. E button)
    protected override void Interact_Internal()
    {
        // get 3 random items to choose from
        // set to default if we have nothing
        UpdatePool();
        List<ItemAsset> options = SamplingUtil.SampleFromList(m_currentList, 3, uniqueResults: true);
        for (int i = 0; i < m_optionUI.Count; ++ i) 
        {
            m_optionUI[i]?.Init(options[i]);
        }
        StartCoroutine(ShowOptions());
    }
    public void OnPlayerChosen(ItemAsset chosen) 
    {
        HideAllOptons();
        // update player inventory, setup item effects & trigger events for obtaining the item

    }
    public void HideAllOptons() 
    {
        foreach (var item in m_optionUI) 
        {
            item.OnChosen -= OnPlayerChosen;
            item.Hide();
        }
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
