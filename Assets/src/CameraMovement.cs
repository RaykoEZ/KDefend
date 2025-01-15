using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] Bounds m_movementBounds = default;
    public RectTransform Player;
    public float damping;
    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        Vector3 movePosition = new Vector3(
            Mathf.Clamp(Player.position.x, -m_movementBounds.extents.x, m_movementBounds.extents.x),
            Mathf.Clamp(Player.position.y, -m_movementBounds.extents.y, m_movementBounds.extents.y),
            transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }
}
