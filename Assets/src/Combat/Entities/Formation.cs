using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public delegate void FormationUpdate();
public abstract class Formation : MonoBehaviour
{
    [SerializeField] UnityEvent m_onFormationInit = default;
    [SerializeField] UnityEvent m_onFormationEnd = default;
    [SerializeField] protected List<Transform> m_members = default;
    bool m_randomizeNextPositon = false;
    public event FormationUpdate OnFormationEnd;
    public virtual bool RandomizeFormationPosition
    { get => m_randomizeNextPositon; set => m_randomizeNextPositon = value; }
    public abstract bool TryGetFormationPosition(BaseEntity chaseTarget, out Vector3 warpPosition);
    void Start() 
    {
        m_onFormationInit?.Invoke();
    }
    public void EndFormation() 
    {
        OnFormationEnd?.Invoke();
        m_onFormationEnd?.Invoke();
    } 
}
