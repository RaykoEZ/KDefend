using UnityEngine;
using UnityEngine.Playables;

public class AttackImpact : MonoBehaviour 
{
    [SerializeField] PlayableDirector m_impactSequence = default;
    public void TriggerAtWorldPosition(BaseEntity hit) 
    {
        transform.position = hit.transform.position;
        m_impactSequence?.Play();
    }
}
