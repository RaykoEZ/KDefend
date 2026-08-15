using System.Collections;
using UnityEngine;

public class SupplyDrop : DropsItem 
{
    [SerializeField] bool m_dropOnStart = default;
    // -1 means no limit
    [Range(-1, 999)]
    [SerializeField] int m_numDropLimit = default;
    [SerializeField] RoutineCaller m_routineCaller = default;
    void OnEnable()
    {
        OnItemDropped += OnItemDrop;
    }
    void OnDisable()
    {
        OnItemDropped -= OnItemDrop;
    }
    void Start()
    {
        if (m_dropOnStart) 
        {
            Begin();
        }
    }
    // update sropped supply reference
    void OnItemDrop(Item drop) 
    {
        if (drop == null) return;
        m_routineCaller.ResetTimeInterval();
        m_numDropped++;
        drop.OnItemPickup += OnPickup;
        // Stop Coroutine for schedule drop
        Stop();
    }
    // begin routine tries
    void OnPickup(Item drop) 
    {
        drop.OnItemPickup -= OnPickup;
        Begin();
    }
    // when drop fails, increase next drop check speed
    protected override void OnDropFail()
    {
        m_routineCaller.TimeInterval *= 0.6f;
    }
    public void Begin() 
    {
        m_routineCaller.StartRoutine(SupplyDrop_Internal());
    }
    public void Stop() 
    {
        m_routineCaller.StopRoutine();
    }
    IEnumerator SupplyDrop_Internal() 
    {
        // check for drop limit and whether a supply was already dropped and unclaimed
        if (m_numDropLimit < 0 || m_numDropped < m_numDropLimit) 
        {
            TryDropItem();
        }
        yield return null;
    }
}