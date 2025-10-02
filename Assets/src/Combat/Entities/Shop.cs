using Curry.Explore;
using System;
using System.Collections.Generic;
using UnityEngine;
// handles specific item selections to trade Pts for every boss cycle
public class Shop : Building 
{
    [SerializeField] ShopPoolUpdater m_updater = default;
    [SerializeField] ItemOptionHandler m_optionHandler = default;
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
        m_optionHandler?.ShowOptions(options);
    }
}
