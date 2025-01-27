using UnityEngine;
// handles item effect activation triggers
public class ItemEventHandler : MonoBehaviour 
{
    [SerializeField] InventoryManager m_inventory = default;
    public void OnItemEvent(KDefenderEventContext e)
    {
        // trigger effect activation attempt
        StartCoroutine(m_inventory.OnTriggerItemEffects(e));
    }
}
