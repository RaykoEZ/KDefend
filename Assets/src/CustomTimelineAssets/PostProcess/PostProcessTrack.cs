using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
[TrackBindingType(typeof(Volume))]
[TrackClipType(typeof(PostProcessClip))]
public class PostProcessTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<PostProcessMixer>.Create(graph, inputCount);
    }
}
