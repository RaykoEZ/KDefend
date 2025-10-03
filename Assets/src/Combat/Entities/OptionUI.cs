using Curry.Explore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Display 3 item choices for player 
public class OptionUI : HideableUI 
{
    [SerializeField] protected TextMeshProUGUI m_name = default;
    [SerializeField] protected TextMeshProUGUI m_description = default;
    [SerializeField] protected Image m_bg = default;
    [SerializeField] protected Image m_itemArt = default;
    protected ItemAsset m_currentItemRef;
    public delegate void ItemOptionEvent(OptionUI itemAsset);
    public ItemOptionEvent OnChosen;
    public ItemAsset CurrentItemRef { get => m_currentItemRef; set => m_currentItemRef = value; }
    public virtual void Init(ItemAsset item)
    {
        m_currentItemRef = item;
        m_name.text = item.PrefabRef.Property.Name;
        m_description.text = item.PrefabRef.Property.Description;
        m_bg.sprite = item.CardBack;
        m_itemArt.sprite = item.CardArt;
    }
    public virtual void OnItemSelect()
    {
        // player obtains perk or item
        OnChosen?.Invoke(this);
    }
}