using Curry.Explore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Display 3 item choices for player 
public class ShopUI : HideableUI 
{
    [SerializeField] TextMeshProUGUI m_description = default;
    [SerializeField] Image m_bg = default;
    [SerializeField] Image m_itemArt = default;
    ItemAsset m_currentItemRef;
    public delegate void ItemOptionEvent(ItemAsset itemAsset);
    public ItemOptionEvent OnChosen;
    public void Init(ItemAsset item)
    {
        m_currentItemRef = item;
        m_description.text = item.Asset.Property.Description;
        m_bg.sprite = item.CardBack;
        m_itemArt.sprite = item.CardArt;
    }
    
    public void OnItemSelect()
    {
        // player obtains perk or item
        OnChosen?.Invoke(m_currentItemRef);
    }
}