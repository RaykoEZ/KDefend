using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class AttackRotation : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 direction = mousePosition - transform.position;

        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
