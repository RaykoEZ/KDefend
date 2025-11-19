using UnityEngine;
using UnityEngine.Events;
// handles summoned object behaviours on spawn & on kill/despawn
public class SummonedEntity : MonoBehaviour 
{
    [SerializeField] UnityEvent m_init = default;
    [SerializeField] UnityEvent m_onDespawn = default;
    void Start()
    {
        Init();
    }
    public virtual void Init() 
    {
        m_init?.Invoke();
    }
    public virtual void Despawn() 
    {
        m_onDespawn?.Invoke();
        Destroy(gameObject);
    }
}
