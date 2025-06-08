using UnityEngine;
using Curry.Game;
using System.Collections.Generic;
using System.Collections;

// updates delivery objective container
public class DeliveryManager : ObjectiveManager<DeliveryDetail>
{
    [SerializeField] DeliveryDropTable m_deliveryList = default;
    [SerializeField] DeliveryPrompter m_prompt = default;
    Coroutine m_deliveryRespawn;
    public override void Init(List<DeliveryDetail> completedObjectives, List<DeliveryDetail> newObjectives)
    {
        DeliveryObjective comp;
        m_prompt.DeliveryReceive -= OnDeliveryComplete;
        m_prompt.DeliveryReceive += OnDeliveryComplete;
        foreach (var objectiveTitle in completedObjectives)
        {
            comp = NewObjective(objectiveTitle);
            m_completed.Add(comp);
        }
        // instantiate objectives from detail provided
        foreach (var objective in newObjectives)
        {
            // prompt active tasks
            ActivateDelivery(objective);
        }
    }
    // New delivery active, spawn box in origin
    public void ActivateNewDelivery(string title)
    {
        DeliveryDetail detail = m_deliveryList.Find(title);
        ActivateDelivery(detail);
    }
    public void ActivateDelivery(DeliveryDetail objective) 
    {
        var result = NewObjective(objective);
        NewActiveObjective(result);
        Debug.Log($"Activate new delivery: {objective.Title}");
        m_prompt?.NewDelivery(objective);
    }
    // find objective the player finished, log the update
    void OnDeliveryComplete(DeliveryDetail detail)
    {
        var objective = GetByTitle(detail.Title);
        if (objective == null) return;
        OnObjectiveComplete(objective);
        if (m_deliveryRespawn != null && m_prompt.IsFull) 
        {
            m_deliveryRespawn = StartCoroutine(RespawnDelivery());
        }
    }
    IEnumerator RespawnDelivery() 
    {
        float rand = Random.Range(180f, 300f);
        yield return new WaitForSeconds(rand);
        // get a delivery detail and spawn item
        DeliveryDetail randomDrop = m_deliveryList.Random();
        ActivateDelivery(randomDrop);
        m_deliveryRespawn = null;
    }
    DeliveryObjective NewObjective(DeliveryDetail objective) 
    {
        DeliveryObjective ret = new DeliveryObjective();
        ret?.Init();
        ret.Setup(objective);
        return ret;
    }
}