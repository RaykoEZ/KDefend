using Curry.Explore;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Display 3 item choices for player 
public class OptionUI : HideableUI 
{
    [SerializeField] TextMeshProUGUI m_name = default;
    [SerializeField] TextMeshProUGUI m_description = default;
    [SerializeField] Image m_bg = default;
    [SerializeField] Image m_itemArt = default;
    Item m_currentItemRef;
    public delegate void ItemOptionEvent(Item itemAsset);
    public ItemOptionEvent OnChosen;
    public void Init(ItemAsset item)
    {
        m_currentItemRef = item.PrefabRef;
        m_name.text = item.PrefabRef.Property.Name;
        m_description.text = item.PrefabRef.Property.Description;
        m_bg.sprite = item.CardBack;
        m_itemArt.sprite = item.CardArt;
    }
    public void Init(Item item)
    {
        m_currentItemRef = item;
        m_name.text = item.Property.Name;
        m_description.text = item.Property.Description;
        m_itemArt.sprite = item.CardArt.sprite;
    }
    public void OnItemSelect()
    {
        // player obtains perk or item
        OnChosen?.Invoke(m_currentItemRef);
    }
}