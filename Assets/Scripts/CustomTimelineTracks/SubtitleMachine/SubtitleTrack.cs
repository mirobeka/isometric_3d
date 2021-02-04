using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackBindingType(typeof(GameObject))]
[TrackClipType(typeof(SubtitleClip))]
[TrackClipType(typeof(SubtitleNameClip))]
public class SubtitleTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount){
        return ScriptPlayable<SubtitleTrackMixer>.Create(graph, inputCount);
    }
}

