using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using MText;

public class SubtitleTrackMixer : PlayableBehaviour
{

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        GameObject subsObject = playerData as GameObject;
        string currentText = "";
        float currentAlpha = 0f;

        if (!subsObject){return;}

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
            }        }
        UpdateText(subsObject, currentText, currentAlpha);
    }

    public void UpdateText(GameObject subsObject, string newText, float currentAlpha)
    {
        Modular3DText[] modularTexts = subsObject.GetComponentsInChildren<Modular3DText>();
        foreach (Modular3DText modularText in modularTexts)
        {
            modularText.UpdateText(newText);
        }
        
    }
}
