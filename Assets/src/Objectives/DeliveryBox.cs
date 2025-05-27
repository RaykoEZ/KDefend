using UnityEngine;
using Curry.Explore;
public class DeliveryBox : HideableUI
{
    public event OnDeliveryUpdate OnDeliveryBegin;
    public event OnDeliveryUpdate OnDeliveryReceive;
    DeliveryDetail m_currentRef;
    public DeliveryDetail Detail => m_currentRef;

    public void Init(DeliveryDetail detail)
    {
        m_currentRef = detail;
    }
    void OnEnable()
    {
        Show();
    }
    void OnDisable()
    {
        Hide();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        bool playerCheck = (collision.attachedRigidbody.TryGetComponent(out Player entering));
        if (playerCheck) 
        {
            Debug.Log("Delivery pack obtained");
            // set player delivery state
            entering.CurrentlyDelivering = Detail;
            OnDeliveryBegin?.Invoke(Detail);
            Hide();
            Destroy(gameObject);
        }
    }
}
