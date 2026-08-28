using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TriggerDialogue : MonoBehaviour 
{
    [SerializeField] string m_conversationTitle = default;
    public void Trigger() 
    { 
        DialogueManager.StartConversation(m_conversationTitle);
    }
}
