using UnityEngine;

public class DeliveryDestination : MonoBehaviour 
{
    [SerializeField] DeliveryPrompter m_prompter = default;
    void OnTriggerEnter2D(Collider2D collision)
    {
        bool playerCheck = (collision.attachedRigidbody.TryGetComponent(out Player entering));
        if (playerCheck && entering)
        {
            m_prompter?.OnDeliveryComplete(entering.CurrentlyDelivering);
            entering.CurrentlyDelivering = DeliveryDetail.None;
        }
    }

}
