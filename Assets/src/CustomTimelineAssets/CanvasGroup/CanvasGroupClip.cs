using UnityEngine;
using UnityEngine.Playables;

public class CanvasGroupClip : PlayableAsset
{
    [Range(0f, 1f)]
    public float Alpha;
    public bool Interactable;
    public bool BlockRaycast;
    public bool IgnoreParentGroup;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CanvasGroupBehaviour>.Create(graph);
        CanvasGroupBehaviour ret = playable.GetBehaviour();
        ret.Alpha = Alpha;
        ret.Interactable = Interactable;
        ret.BlockRaycast = BlockRaycast;
        ret.IgnoreParentGroup = IgnoreParentGroup;
        return playable;
    }
}
