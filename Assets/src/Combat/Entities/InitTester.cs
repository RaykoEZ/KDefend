using UnityEngine;
// FOR TESTING: set character stats and position
public class InitTester : MonoBehaviour 
{
    [SerializeField] protected EntityProperty m_initStats = default;
    [SerializeField] protected Transform m_location = default;
    [SerializeField] protected BaseEntity m_initTarget = default;
    [SerializeField] protected NpcMovement m_movement = default;
    void Start()
    {
        EntityState toSet = new EntityState {
            Position = m_location.position,
            Property = m_initStats };
        m_initTarget.Init(toSet);
        m_movement?.Init(m_initTarget);
    }
}
