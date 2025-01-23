using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class PostProcessClip : PlayableAsset
{
    public float VolumeWeight;
    public VolumeProfile PostprocessSetting;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PostProcessBehaviour>.Create(graph);
        PostProcessBehaviour ret = playable.GetBehaviour();
        ret.VolumeWeight = VolumeWeight;
        ret.PostprocessSetting = PostprocessSetting;
        return playable;
    }
}
