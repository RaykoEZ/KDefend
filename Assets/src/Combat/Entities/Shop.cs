using Curry.Explore;
using System;
using System.Collections.Generic;
using UnityEngine;
// handles specific item selections to trade Pts for every boss cycle
public class Shop : Building 
{
    [SerializeField] ShopPoolUpdater m_updater = default;
    [SerializeField] ShopOptionsHandler m_optionHandler = default;
    [SerializeField] HideableUI m_display = default;
    bool m_isOn = false;
    // on interact, update item pool
    // activate this when user chooses interact option (e.g. E button)
    protected override void Interact_Internal()
    {
        if (m_isOn) return;
        m_isOn = true;
        // TODO: DIRTY flag when item added, so we don't call this line every time
        List<ItemAsset> options = m_updater.GetOptionAssetPool();
        m_optionHandler?.UpdateShopOptions(options);
        m_display?.Show();
    }
}
