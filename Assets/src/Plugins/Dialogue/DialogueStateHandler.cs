using UnityEngine;
using PixelCrushers.DialogueSystem;
// Change a dialogue trigger's conversation pointer
// to trigger a different dialogue tree later
public class DialogueStateHandler : MonoBehaviour 
{
    [SerializeField, ConversationPopup(true)] string m_defaultConversation = default;
    [SerializeField] DialogueDatabase m_database = default;
    [SerializeField] DialogueSystemTrigger m_trigger = default;
    public void SetToDefault() 
    {
        m_trigger.conversation = m_defaultConversation;
    }
    public void ChangeConversation(string conversationTitle) 
    {
        var check = m_database.GetConversation(conversationTitle);
        if (check != null) 
        {
            m_trigger.conversation = conversationTitle;
        }
    }
}
/**/