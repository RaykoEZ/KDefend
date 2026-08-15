using UnityEngine;
using System.Collections.Generic;
public class ShopOptionsHandler : ItemOptionHandler
{
    [SerializeField] protected BuyItem m_buyItem = default;
    // for instantiating each shop item options
    [SerializeField] protected ShopOptionUI m_optionUIPrefab = default;
    public void UpdateShopOptions(List<ItemAsset> items) 
    {
        // clear listeners to set new ones to avoid duplicate setups
        Shutdown();
        ShopOptionUI instance;
        for (int i = 0; i < items.Count; i++) 
        {
            if (i >= m_optionUI.Count) 
            {
                instance = GameUtil.SpawnObject(m_optionUIPrefab, Vector3.zero, transform);
                m_optionUI.Add(instance);
            }
            m_optionUI[i].Init(items[i]);
        }
        Init();
        Show();
        StartCoroutine(ShowOptions_Internal());
    }
    protected override void OnPlayerChosen(OptionUI chosen)
    {
        if (chosen is ShopOptionUI shopOption) 
        {
            // update player inventory, setup item effects & trigger events for obtaining the item
            m_buyItem?.OnItemChosen(shopOption);
        }
    }
    protected void OnUnChosen(OptionUI _) 
    {
        m_buyItem?.CancelChoice();
    }
    protected override void InitOption(OptionUI option)
    {
        base.InitOption(option);
        (option as ShopOptionUI).OnUnchosen += OnUnChosen;
    }
    protected override void ShutdownOption(OptionUI option)
    {
        base.ShutdownOption(option);
        (option as ShopOptionUI).OnUnchosen -= OnUnChosen;    
    }
}
