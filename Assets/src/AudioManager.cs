using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
[Serializable]
public struct AudioDetail 
{
    public string Name;
    public AudioClip Clip;
    public bool Loop;
    public float Volume;
    public float Pitch;
}
[Serializable]
public class AudioItem
{
    public AudioDetail Detail;
    [HideInInspector]
    public AudioSource Source;
    public AudioMixerGroup Output;

}
public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioItem> m_audioList = default;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var a in m_audioList)
        {
            a.Source = gameObject.AddComponent<AudioSource>();
            a.Source.outputAudioMixerGroup = a.Output;
            a.Source.clip = a.Detail.Clip;
            a.Source.loop = a.Detail.Loop;
            a.Source.volume = a.Detail.Volume;
            a.Source.pitch = a.Detail.Pitch;
        }
    }
    // add a new source on a specified parent and play the sound
    public static void PlayAt(Transform audioParent, AudioDetail toAdd) 
    {
        if (audioParent == null) return;
        AudioSource source;
        // get existing audio source to modify, if no audio source on parent, make a new one
        if (!audioParent.TryGetComponent(out source)) 
        {
            source = audioParent.gameObject.AddComponent<AudioSource>();
        }
        source.clip = toAdd.Clip;
        source.loop = toAdd.Loop;
        source.volume = toAdd.Volume;
        source.pitch = toAdd.Pitch;
        source.Play();
    }
    public bool Play(string audioName)
    {
        var result = m_audioList.Find((audio) => audio.Detail.Name == audioName);
        result?.Source?.Play();
        if (result == null) Debug.LogWarning($"Audio: {audioName} not found, cannot play audio");
        return result != null;
    }
    public bool Stop(string audioName)
    {
        var result = m_audioList.Find((audio) => audio.Detail.Name == audioName);
        result?.Source?.Stop();
        if (result == null) Debug.LogWarning($"Audio: {audioName} not found, cannot stop audio");
        return result != null;
    }
}
