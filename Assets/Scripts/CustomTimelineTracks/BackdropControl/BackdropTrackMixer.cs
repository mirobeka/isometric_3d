using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BackdropTrackMixer : PlayableBehaviour
{

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Image backdrop = playerData as Image;
        float currentAlpha = 0f;

        if (!backdrop){return;}

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0f)
            {
                currentAlpha = inputWeight;
            }
        }
        backdrop.color = new Color(0, 0, 0, currentAlpha);
    }
}
