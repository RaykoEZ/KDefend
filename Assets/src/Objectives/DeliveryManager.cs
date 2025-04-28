using UnityEngine;
using Curry.Game;
using System.Collections.Generic;

// updates delivery objective container
public class DeliveryManager : ObjectiveManager<DeliveryDetail>
{
    [SerializeField] List<DeliveryDropTable> m_levelDeliverList = default;
    public void AddDelivery(List<DeliveryDetail> details) 
    {
        foreach (var item in details)
        {
            DeliveryObjective newObj = new DeliveryObjective();
            newObj.Setup(item);
            NewActiveObjective(newObj);
        }
    }
    public void AddFromLevel(int levelIndex) 
    {
        if (levelIndex >= m_levelDeliverList.Count) return;
        AddDelivery(m_levelDeliverList[levelIndex].DropList);
    }

    public override void Init(List<DeliveryDetail> completedObjectives, List<DeliveryDetail> newObjectives)
    {
        // instantiate objectives from detail provided
        foreach (DeliveryDetail objective in newObjectives)
        {
            DeliveryObjective newObj = new DeliveryObjective();
            newObj.Init();
            newObj.Setup(objective);
            NewActiveObjective(newObj);
        }
        foreach (DeliveryDetail objective in completedObjectives)
        {
            DeliveryObjective comp = new DeliveryObjective();
            comp.Init();
            comp.Setup(objective);
            OnObjectiveComplete(comp);
        }
    }
}