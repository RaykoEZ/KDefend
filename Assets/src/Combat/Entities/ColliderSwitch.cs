using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class ColliderSwitch: MonoBehaviour 
{
    [SerializeField] List<Collider2D> m_toToggle = default;
    public void Toggle() 
    {
        foreach (var item in m_toToggle)
        {
            item.enabled = !item.enabled;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.gameObject.TryGetComponent<Player>(out _)) 
        {
            Toggle();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody.gameObject.TryGetComponent<Player>(out _))
        {
            Toggle();
        }
    }
}
