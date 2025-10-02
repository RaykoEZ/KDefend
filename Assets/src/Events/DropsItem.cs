using UnityEngine;
// handles item drop, usually for upon defeating an enemy
public class DropsItem : MonoBehaviour 
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
        ItemAsset drop = m_dropList.GetWeightedDrop();
        // spawn
        Item.SpawnItem(drop, transform.parent.parent, transform.position);
    }
}