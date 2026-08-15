using UnityEngine.Playables;
using UnityEngine.Rendering;

public class PostProcessMixer : PlayableBehaviour 
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Volume volume = playerData as Volume;
        if (volume == null)
        {
            return;
        }
        float currentWeight = 0f;
        VolumeProfile currentProfile = null;
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);
            if (weight > 0f)
            {
                var intputPlayable = (ScriptPlayable<PostProcessBehaviour>)playable.GetInput(i);
                PostProcessBehaviour input = intputPlayable.GetBehaviour();
                currentWeight = weight * input.VolumeWeight;
                currentProfile = input.PostprocessSetting;
            }
        }
        volume.weight = currentWeight;
        volume.profile = currentProfile;       
    }
}