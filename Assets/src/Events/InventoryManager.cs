using Curry.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Contains inventory content and methods to access/modify
public class InventoryManager : MonoBehaviour 
{
    [SerializeField] Player m_player = default;
    [SerializeField] ItemAssetLookup m_mainCollection = default;
    protected Inventory<Item> m_heldItems = new Inventory<Item>();
    public List<Item> HeldItems => m_heldItems.ItemList;
    public List<ItemState> GetItemPropeties() 
    { 
        List<ItemState> ret = new List<ItemState>();
        foreach (var item in HeldItems)
        {
            ItemState state = new ItemState(item.Property, item.CurrentStack);
            ret.Add(state);
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
    public void Init(List<ItemState> props)
    {
        Item toAdd;
        Item instance;
        foreach (var itemState in props)
        {
            toAdd = m_mainCollection.GetItemByProperty(itemState.Property).PrefabRef;
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
    public void UseItem(string itemId) 
    {
        if (HeldItems.Count == 0 ||
        string.IsNullOrWhiteSpace(itemId)) return;
        HeldItems.Find((x)=>x.Property.Id == itemId)?.UseItem();
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
