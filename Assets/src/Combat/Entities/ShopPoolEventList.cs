using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopPoolEvent", menuName = "Jams/Event/Shop Pool Update", order = 1)]
public class ShopPoolEventList : ScriptableObject 
{
    [SerializeField] List<UpdateShopPool> m_events = default;
    public List<UpdateShopPool> GetEvents() 
    { 
        List<UpdateShopPool> ret = new List<UpdateShopPool>();
        foreach (UpdateShopPool s in m_events) 
        {
            ret.Add(new UpdateShopPool(s));
        }
        return ret;
    }
}