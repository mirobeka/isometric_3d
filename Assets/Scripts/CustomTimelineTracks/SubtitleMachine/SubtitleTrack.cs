using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;

[TrackBindingType(typeof(TextMeshProUGUI))]
[TrackClipType(typeof(SubtitleClip))]
[TrackClipType(typeof(SubtitleNameClip))]
public class SubtitleTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount){
        return ScriptPlayable<SubtitleTrackMixer>.Create(graph, inputCount);
    }
}

