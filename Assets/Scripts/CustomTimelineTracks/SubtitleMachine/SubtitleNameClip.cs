using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleNameClip : PlayableAsset
{
    public string playerName;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

        SubtitleBehaviour subtitleBehaviour = playable.GetBehaviour();
        subtitleBehaviour.subtitleText = playerName;

        return playable;
    }
}


