using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// changes shop item pool when player obtains/triggers a game event
public class ItemPoolUpdater : MonoBehaviour
{
    [SerializeField] Shop m_shop = default;
    InternalEventHandler m_event = new InternalEventHandler();
    void OnEnable()
    {
        // Listen to events for specific effects
    }
}
// handles specific item selections to trade Pts for every boss cycle
public class Shop : Building 
{
    [SerializeField] ItemAssetLookup m_default = default;
    [SerializeField] List<ShopUI> m_optionUI = default;
    List<ItemAsset> m_currentList = new List<ItemAsset>();
    void OnEnable()
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
        if (m_currentList == null) 
        {
            m_currentList.AddRange(m_default.Assets);
        }
        List<ItemAsset> options = SamplingUtil.SampleFromList(m_currentList, 3, uniqueResults: true);
        for (int i = 0; i < m_optionUI.Count; ++ i) 
        {
            m_optionUI[i]?.Init(options[i]);
        }
        StartCoroutine(ShowOptions());
    }
    public void OnPlayerChosen(ItemAsset chosen) 
    { 
    
    }
    public void HideAllOptons() 
    {
        foreach (var item in m_optionUI) 
        {
            item.Hide();
        }
    }
    IEnumerator ShowOptions() 
    { 
        foreach(var item in m_optionUI) 
        {
            item.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
