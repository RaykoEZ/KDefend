using UnityEngine;

public class  ShopOptionsHandler : ItemOptionHandler
{
    [SerializeField] protected BuyItem m_buyItem = default;
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
