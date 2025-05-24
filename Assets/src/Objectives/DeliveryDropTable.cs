using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "deli_", menuName = "New Delivery Droptable", order = 0)]
public class DeliveryDropTable : ScriptableObject
{
    [SerializeField] List<DeliveryDetail> m_dropList = default;
    public List<DeliveryDetail> DropList { get => m_dropList; }
    public DeliveryDetail Find(string title) 
    {
        return m_dropList.Find((i) => i.Title == title);
    }
    public List<DeliveryDetail> Find(List<string> titles)
    {
        List<DeliveryDetail> ret = new List<DeliveryDetail>();
        foreach (var item in titles)
        {
            ret.Add(Find(item));
        }
        return ret;
    }
}
