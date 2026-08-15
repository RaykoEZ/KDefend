using UnityEngine;
using UnityEngine.Playables;

public class CanvasGroupMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        CanvasGroup group = playerData as CanvasGroup;
        if (group == null)
        {
            return;
        }
        float Alpha = 0f;
        bool Interactable = false;
        bool BlockRaycast = false;
        bool IgnoreParentGroup = false;
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);
            if (weight > 0f)
            {
                var intputPlayable = (ScriptPlayable<CanvasGroupBehaviour>)playable.GetInput(i);
                CanvasGroupBehaviour input = intputPlayable.GetBehaviour();
                Alpha = weight * input.Alpha;
                Interactable = input.Interactable;
                BlockRaycast = input.BlockRaycast;
                IgnoreParentGroup = input.IgnoreParentGroup;
            }
        }
        group.alpha = Alpha;
        group.interactable = Interactable;
        group.blocksRaycasts = BlockRaycast;
        group.ignoreParentGroups = IgnoreParentGroup;
    }
}
