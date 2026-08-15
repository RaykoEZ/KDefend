using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleMixer: PlayableBehaviour 
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI text = playerData as TextMeshProUGUI;

        Color currentColor = Color.white;
        string currenteText = "";
        if (text == null) 
        { 
            return; 
        }
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);
            if (weight > 0f) 
            {
                var intputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                SubtitleBehaviour input = intputPlayable.GetBehaviour();
                currenteText = input.SubtitleText;
                currentColor = input.TextColour;
                currentColor.a = weight;
            }
        }
        text.text = currenteText;
        text.color = currentColor;
    }
}