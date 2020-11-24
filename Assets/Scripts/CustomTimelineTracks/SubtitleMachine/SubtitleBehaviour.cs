using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class SubtitleBehaviour : PlayableBehaviour
{
    public string subtitleText;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI tmpText = playerData as TextMeshProUGUI;
        tmpText.text = subtitleText;
        tmpText.color = new Color(1, 1, 1, info.weight);
}
}

