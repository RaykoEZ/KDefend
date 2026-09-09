using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TriggerDialogue : MonoBehaviour 
{
    [SerializeField, ConversationPopup(true)] string m_conversationTitle = default;
    public void Trigger() 
    { 
        DialogueManager.StartConversation(m_conversationTitle);
    }
}
