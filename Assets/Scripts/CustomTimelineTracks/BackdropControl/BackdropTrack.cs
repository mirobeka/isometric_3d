using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;

[TrackBindingType(typeof(Image))]
[TrackClipType(typeof(BackdropClip))]
public class BackdropTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount){
        return ScriptPlayable<BackdropTrackMixer>.Create(graph, inputCount);
    }
}
