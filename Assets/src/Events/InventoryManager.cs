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
    public IEnumerator OnTriggerItemEffects(GameEventContext e) 
    {
        foreach (var item in HeldItems)
        {
            TryActivateItem(e, item);
            yield return new WaitForEndOfFrame();
        }
    }
    protected void TryActivateItem(GameEventContext e, Item toUse)
    {
        if (m_heldItems.TryGetValue(toUse, out Item result))
        {
            result?.Activate(e);
        }
    }
}
