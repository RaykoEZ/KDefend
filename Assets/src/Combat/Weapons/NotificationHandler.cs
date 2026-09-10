using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Curry.Events;
using UnityEngine.Playables;
// Use this when triggering an event, if "notify" is included in payload dictionary, display it on the message HUD.
public class NotificationHandler : MonoBehaviour 
{
    [SerializeField] List<GameEventTriggerType> m_listenToEventTypes = default;
    [SerializeField] TextMeshProUGUI m_textField = default;
    [SerializeField] PlayableAsset m_toPlay = default;
    [SerializeField] PlayableDirector m_sequencer = default;
    void OnEnable() 
    {
        foreach (var item in m_listenToEventTypes)
        {
            KDEventHandler.ListenToGlobal(item, OnNotification);
        }

    }
    void OnDisable()
    {
        foreach (var item in m_listenToEventTypes)
        {
            KDEventHandler.UnlistenFromGlobal(item, OnNotification);
        }
    }
    public void Notify(string message) 
    {
        if (message == null) return;
        m_textField.text = message;
        GameUtil.PlaySequence(m_sequencer, m_toPlay);
    }
    void OnNotification(object sender, KDEventInfo args) 
    {
        if (args.Payload == null) return;
        if (args.Payload.TryGetValue("notify", out object result) && result is string display)
        {
            m_textField.text = display;
            GameUtil.PlaySequence(m_sequencer, m_toPlay);
        }
    }
}
