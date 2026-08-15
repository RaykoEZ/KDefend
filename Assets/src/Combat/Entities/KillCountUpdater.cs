using TMPro;
using UnityEngine;

public class KillCountUpdater : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI m_toUpdate = default;
    public void UpdateCount(int numKills) 
    {
        m_toUpdate.text = numKills.ToString();
    }
}
