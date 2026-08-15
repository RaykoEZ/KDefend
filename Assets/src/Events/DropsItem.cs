using UnityEngine;
// handles item drop, usually for upon defeating an enemy
public class DropsItem : MonoBehaviour 
{
    // -1 for unlimited
    [SerializeField] int m_dropLimit = default;
    [Range(0f, 1f)]
    [SerializeField] float m_dropRate = default;
    [SerializeField] ItemDropList m_dropList = default;
    protected delegate void OnDrop(Item drop);
    protected event OnDrop OnItemDropped;
    protected int m_numDropped = 0;
    public virtual void TryDropItem() 
    {
        if (m_numDropped >= m_dropLimit && m_dropLimit >= 0) return;
        m_numDropped++;
        // drop check
        float rand = Random.Range(0f, 1f);
        if (rand > m_dropRate)
        {
            OnDropFail();
            return; 
        }
        // get drops from drop list
        ItemAsset drop = m_dropList.GetWeightedDrop();
        // spawn
        var instance = Item.SpawnItem(drop, transform.parent, transform.localPosition);
        //set instance child order behind the dropper, to display on top of dropper
        instance.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        OnItemDropped?.Invoke(instance);
    }
    // When a drop rate check fails
    protected virtual void OnDropFail() 
    {    
    }
}
