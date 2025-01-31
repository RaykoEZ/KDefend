using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Contains inventory content and methods to access/modify
public class InventoryManager : MonoBehaviour 
{
    [SerializeField] UnityEvent<Item> m_onObtainItem = default;
    protected Inventory<Item> m_heldItems = new Inventory<Item>();
    protected List<Item> HeldItems => m_heldItems.ItemList;
    public InventoryState GetState() 
    {
        return m_heldItems.GetState();
    }
    public void ObtainItem(Item obtained) 
    {
        m_heldItems.Add(obtained);
        m_onObtainItem?.Invoke(obtained);
    }
    public IEnumerator OnTriggerItemEffects(KDefenderEventContext e) 
    {
        foreach (var item in HeldItems)
        {
            TryUdateCollectible(e, item);
            yield return new WaitForEndOfFrame();
        }
    }
    protected void TryUdateCollectible(KDefenderEventContext e, Item toUse)
    {
        if (m_heldItems.TryGetValue(toUse, out Item result))
        {
            result?.Update(e);
        }
    }
}
