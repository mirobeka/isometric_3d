using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class TitulkyTrackMixer : PlayableBehaviour
{

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI titulky = playerData as TextMeshProUGUI;
        string currentText = "";
        float currentAlpha = 0f;

        if (!titulky){return;}

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0f)
            {
                ScriptPlayable<TitulkyBehaviour> inputPlayable;
                inputPlayable = (ScriptPlayable<TitulkyBehaviour>)playable.GetInput(i);
                TitulkyBehaviour input = inputPlayable.GetBehaviour();

                currentText = input.titulkyText;
                currentAlpha = inputWeight;
            }
        }
        titulky.text = currentText;
        titulky.color = new Color(1, 1, 1, currentAlpha);
    }
}
