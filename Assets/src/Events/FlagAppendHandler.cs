using UnityEngine;

public class FlagAppendHandler : MonoBehaviour 
{
    [SerializeField] KD_StaticEventFlags m_appendThisFlag = default;
    [SerializeField] StaticFlagEventHandler m_eventHandler = default;
    public void Append() 
    {
        m_eventHandler?.AppendFlag(m_appendThisFlag);
    }
}
