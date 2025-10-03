using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopPool", menuName = "Jams/Item/Shop Pool", order = 1)]
public class ShopPool : ScriptableObject
{
    [SerializeField] ItemAssetLookup m_itemPoolLookup = default;

    [SerializeField] List<ShopItemState> m_defaultPoolStates = default;
    public ItemAssetLookup ItemPoolLookup => m_itemPoolLookup;
    public List<ShopItemState> DefaultPoolStates => m_defaultPoolStates;
}