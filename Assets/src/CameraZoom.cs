using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour 
{
    [SerializeField] List<Camera> m_affectingCameras = new List<Camera>();
    [SerializeField] float m_zoomTime = default;
    [SerializeField] float m_normalZoom = default;
    [SerializeField] float m_zoomInValue = default;
    [SerializeField] float m_zoomOutValue = default;
    float m_currentZoom = 0f;
    public void ZoomIn() 
    {
        StartCoroutine(ChangeZoom_Internal(m_zoomInValue));
    }
    public void ZoomOut() 
    {
        StartCoroutine(ChangeZoom_Internal(m_zoomOutValue));
    }
    public void ResetZoom()
    {
        StartCoroutine(ChangeZoom_Internal(m_normalZoom));
    }
    IEnumerator ChangeZoom_Internal(float target) 
    {
        float newZoom; 
        float t = 0f;
        m_currentZoom = m_affectingCameras[0].orthographicSize;
        while (t < m_zoomTime) 
        {
            newZoom = Mathf.Lerp(m_currentZoom, target, t / m_zoomTime);
            foreach (var cam in m_affectingCameras)
            {
                cam.orthographicSize = newZoom;
            }
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
    }
}
