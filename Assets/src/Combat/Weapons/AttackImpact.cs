using UnityEngine;
using UnityEngine.Playables;
// plays hit impact sequence 
public class AttackImpact : MonoBehaviour 
{
    [SerializeField] PlayableDirector m_impactSequence = default;
    public void TriggerAtWorldPosition() 
    {
        m_impactSequence?.Play();
    }
}
