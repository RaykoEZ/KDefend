using UnityEngine;
// handles item drop upon defeating an enemy
public class DropItemOnDefeat : MonoBehaviour 
{
    [Range(0f, 1f)]
    [SerializeField] float m_dropRate = default;
    [SerializeField] ItemDropList m_dropList = default;
    public void TryDropItem() 
    {
        // drop check
        float rand = Random.Range(0f, 1f);
        if (rand > m_dropRate) return;
        // get drops from drop list
        Item drop = m_dropList.GetWeightedDrop();
        // spawn
        GameUtil.SpawnObject(drop, transform.position, transform.parent.parent);
    }
}
