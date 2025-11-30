using System.Collections;
using System.Collections.Generic;
using Curry.UI;
using UnityEngine;
using UnityEngine.Events;
public class RegenerateCounter : MonoBehaviour 
{
    [SerializeField] List<ResourceDisplayHandler> m_counters = default;
    [SerializeField] UnityEvent m_onUseCounter = default;
    Queue<ResourceDisplayHandler> m_free = new Queue<ResourceDisplayHandler>();
    public bool IsFree => m_free.Count > 0;
    void Start() 
    {
        // init all counters
        foreach (ResourceDisplayHandler handler in m_counters) 
        { 
            m_free.Enqueue(handler);
        }
    }
    public void TryUseCounter() 
    {
        if (m_free.TryDequeue(out var item) && item != null)
        {
            item.SetCurrentValue(0f, instant: true);
            m_onUseCounter?.Invoke();
            StartCoroutine(Regenerate(item));
        }
    }
    protected IEnumerator Regenerate(ResourceDisplayHandler toRegen) 
    {
        toRegen.SetCurrentValue(toRegen.Max, instant: false);
        yield return new WaitUntil(() => toRegen.IsAtMax);
        yield return new WaitForSeconds(0.1f);
        m_free.Enqueue(toRegen);
    }
}
