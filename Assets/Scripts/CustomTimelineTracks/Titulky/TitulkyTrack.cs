using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;

[TrackBindingType(typeof(TextMeshProUGUI))]
[TrackClipType(typeof(TitulkyClip))]
public class TitulkyTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount){
        return ScriptPlayable<TitulkyTrackMixer>.Create(graph, inputCount);
    }
}
