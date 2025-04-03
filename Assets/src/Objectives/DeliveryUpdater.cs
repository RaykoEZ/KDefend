using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
// updates delivery objectives
public class DeliveryUpdater : MonoBehaviour 
{
    [SerializeField] ObjectiveManager m_objective = default;
    [SerializeField] List<DeliveryDropTable> m_levelDeliverList = default;
    public void AddDelivery(List<DeliveryDetail> details) 
    {
        foreach (var item in details)
        {
            DeliveryObjective newObj = new DeliveryObjective();
            newObj.Setup(item);
            m_objective?.NewActiveObjective(newObj);
        }
    }
    public void AddFromLevel(int levelIndex) 
    {
        if (levelIndex >= m_levelDeliverList.Count) return;
        AddDelivery(m_levelDeliverList[levelIndex].DropList);
    }
}