using UnityEngine;
using Curry.Explore;
public delegate void OnDeliveryUpdate(DeliveryDetail detail);
public class DeliveryBox : HideableUI
{
    public event OnDeliveryUpdate OnDeliveryBegin;
    DeliveryDetail m_currentRef;
    public void Init(DeliveryDetail detail)
    {
        m_currentRef = detail;
        Show();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        bool check = (collision.attachedRigidbody.TryGetComponent(out Player entering));
        if (check) 
        {
            OnDeliveryBegin?.Invoke(m_currentRef);
            Hide();
            Destroy(gameObject);
        }
    }
}
