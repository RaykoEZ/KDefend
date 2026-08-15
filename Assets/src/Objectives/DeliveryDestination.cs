using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Curry.Explore;
public class DeliveryDestination : HideableUI 
{
    [SerializeField] UnityEvent<DeliveryDetail> m_onPackageReceived = default;
    HashSet<DeliveryDetail> m_toExpect = new HashSet<DeliveryDetail>();
    bool m_showing = false;
    void Update()
    {
        // if there are packages to expect, highlight area in animator
        if (m_toExpect.Count == 0)
        {
            Hide();
        }
        else if(!m_showing)
        {
            m_showing = true;
            Show();
        }
    }
    public void ExpectPackage(DeliveryDetail toExpect) 
    {
        m_toExpect.Add(toExpect);
    }
    void ReceivePackage(DeliveryDetail received) 
    {
        m_toExpect.Remove(received);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // receive package upon entering destination range
        bool playerCheck = (collision.attachedRigidbody.TryGetComponent(out Player entering));
        if (!playerCheck) return;
        DeliveryDetail package = entering.CurrentlyDelivering;
        if (m_toExpect.Contains(package))
        {
            m_onPackageReceived?.Invoke(package);
            ReceivePackage(package);
            entering.CurrentlyDelivering = DeliveryDetail.None;
        }
    }
}
