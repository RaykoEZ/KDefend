using UnityEngine;
using UnityEngine.Playables;

public class SubtitleClip : PlayableAsset 
{
    public Color TextColour;
    [TextArea(5, 25)]
    public string SubtitleText;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);
        SubtitleBehaviour ret = playable.GetBehaviour();
        ret.SubtitleText = SubtitleText;
        ret.TextColour = TextColour;
        return playable;
    }
}
