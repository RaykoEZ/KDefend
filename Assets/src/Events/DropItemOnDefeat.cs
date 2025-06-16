using UnityEngine;
// handles item drop upon defeating an enemy
public class DropItemOnDefeat : MonoBehaviour 
{
    [SerializeField] ItemDropList m_dropList = default;
    public void DropItem() 
    {
        // get drops from drop list
        Item drop = m_dropList.GetWeightedDrop();
        // spawn
        GameUtil.SpawnObject(drop, transform.position, transform.parent.parent);
    }
}
