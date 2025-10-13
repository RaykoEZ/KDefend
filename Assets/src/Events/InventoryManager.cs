using Curry.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Contains inventory content and methods to access/modify
public class InventoryManager : MonoBehaviour 
{
    [SerializeField] Player m_player = default;
    [SerializeField] ItemAssetLookup m_mainCollection = default;
    [SerializeField] UnityEvent<Item> m_onObtainItem = default;
    protected Inventory<Item> m_heldItems = new Inventory<Item>();
    public List<Item> HeldItems => m_heldItems.ItemList;
    public List<ItemProperty> GetItemPropeties() 
    { 
        List<ItemProperty> ret = new List<ItemProperty>();
        foreach (var item in HeldItems)
        {
            ret.Add(item.Property);
        }
        return ret;
    }
    // listen to obtaining item/perks
    void OnEnable()
    {
        InternalEventHandler.ListenToGlobal(
            GameEventTriggerType.ItemObtained, OnObtainItem);
    }
    void OnDisable()
    {
        InternalEventHandler.UnlistenFromGlobal(
    GameEventTriggerType.ItemObtained, OnObtainItem);
    }
    public void Init(List<ItemProperty> props)
    {
        Item toAdd;
        Item instance;
        foreach (var property in props)
        {
            toAdd = m_mainCollection.GetItemByProperty(property).PrefabRef;
            // instantiate item
            instance = Instantiate(toAdd, m_player.transform);
            (instance as Collectible)?.Init(m_player);
            m_heldItems.Add(instance);
        }
    }
    public void ObtainItem(Item obtained) 
    {
        if (obtained is Collectible)
        {
            m_heldItems.Add(obtained);
            m_onObtainItem?.Invoke(obtained);
        }
        obtained?.OnPickup(m_player);
    }
    public void UseItem(int itemIndex) 
    {
        if (HeldItems.Count == 0 || 
            itemIndex < 0 || 
            itemIndex >= HeldItems.Count) return;
        HeldItems[itemIndex]?.UseItem();
    }
    protected void OnObtainItem(object sender, KDEventInfo args) 
    {
        if (args == null || args.Payload == null) return;
        if (args.Payload.TryGetValue("item", out object result) && result is ItemAsset prefab) 
        {
            Item instance = Item.SpawnItem(prefab, m_player.transform, Vector2.zero);
            ObtainItem(instance);
        }
    }
}
