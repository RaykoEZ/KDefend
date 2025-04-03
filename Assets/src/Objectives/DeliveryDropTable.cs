using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "deli_", menuName = "New Delivery Droptable", order = 0)]
public class DeliveryDropTable : ScriptableObject
{
    [SerializeField] List<DeliveryDetail> m_dropList = default;
    public List<DeliveryDetail> DropList { get => m_dropList; }
}
