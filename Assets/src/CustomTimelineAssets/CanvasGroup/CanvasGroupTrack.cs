using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[TrackBindingType(typeof(CanvasGroup))]
[TrackClipType(typeof(CanvasGroupClip))]
public class CanvasGroupTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<CanvasGroupMixer>.Create(graph, inputCount);
    }
}