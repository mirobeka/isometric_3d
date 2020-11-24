using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class SubtitleTrackMixer : PlayableBehaviour
{

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI tmpText = playerData as TextMeshProUGUI;
        string currentText = "";
        float currentAlpha = 0f;

        if (!tmpText){return;}

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0f)
            {
                ScriptPlayable<SubtitleBehaviour> inputPlayable;
                inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                SubtitleBehaviour input = inputPlayable.GetBehaviour();

                currentText = input.subtitleText;
                currentAlpha = inputWeight;
            }
        }


        tmpText.text = currentText;
        tmpText.color = new Color(1, 1, 1, currentAlpha);
    }
}
