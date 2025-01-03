using TMPro;
using UnityEngine;

public class Test_StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_testDisplay = default;
    [SerializeField] BaseEntity m_toTest = default;
    void Update()
    {
        m_testDisplay.text = m_toTest.CurrentStats.Health.ToString();
    }
}
