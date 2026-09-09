using UnityEngine;
// drop a designated item
public class DropItem_Single : MonoBehaviour 
{
    [SerializeField] ItemAsset m_dropAssetRef = default;
    public void DropItem() 
    {
        // spawn
        var instance = Item.SpawnItem(m_dropAssetRef, transform.parent, transform.localPosition);
        //set instance child order behind the dropper, to display on top of dropper
        instance.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
    }
    public void DropItem(Transform spawnAt) 
    {
        // spawn
        var instance = Item.SpawnItem(m_dropAssetRef, spawnAt.parent, spawnAt.localPosition);
        //set instance child order behind the dropper, to display on top of dropper
        instance.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
    }
}