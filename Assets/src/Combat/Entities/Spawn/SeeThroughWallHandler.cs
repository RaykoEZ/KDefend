using UnityEngine;
// use to enable attached object be seen through objects upon entering behind the object
public class SeeThroughWallHandler : MonoBehaviour 
{
    protected static int s_playerPosId = Shader.PropertyToID("_playerPosition");
    protected static int s_sizeId = Shader.PropertyToID("_size");
    protected static int s_opacityId = Shader.PropertyToID("_opacity");
    protected static int s_smoothId = Shader.PropertyToID("_smoothing");
    [SerializeField] Material m_seeThroughMaterial = default;
    [SerializeField] LayerMask m_raycastChecks = default;
    void Update()
    {
        // check if we are behind a structure
        var hit = Physics2D.OverlapPoint(transform.position, m_raycastChecks);
        if (hit != null) 
        {
            // activate & set player position in camera object, and assign see through position
            m_seeThroughMaterial.SetFloat(s_sizeId, 1f);
            var view = Camera.main.WorldToViewportPoint(transform.position);
            m_seeThroughMaterial.SetVector(s_playerPosId, view);
        }
        else 
        {
            m_seeThroughMaterial.SetFloat(s_sizeId, 0f);
        }
    }
}
