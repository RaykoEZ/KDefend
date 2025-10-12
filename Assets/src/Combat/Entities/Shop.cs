using Curry.Explore;
using System;
using System.Collections.Generic;
using UnityEngine;
// handles specific item selections to trade Pts for every boss cycle
public class Shop : Building 
{
    [SerializeField] ShopPoolUpdater m_updater = default;
    [SerializeField] ShopOptionsHandler m_optionHandler = default;
    // on interact, update item pool
    // activate this when user chooses interact option (e.g. E button)
    protected override void Interact_Internal()
    {
        // TODO: DIRTY flag when item added, so we don't call this line every time
        List<ItemAsset> options = m_updater.GetOptionAssetPool();
        m_optionHandler?.UpdateShopOptions(options);
    }
}
