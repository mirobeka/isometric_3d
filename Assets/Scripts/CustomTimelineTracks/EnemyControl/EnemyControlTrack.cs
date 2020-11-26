using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackBindingType(typeof(GameObject))]
[TrackClipType(typeof(EnemyControlClip))]
public class EnemyControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount){
        return ScriptPlayable<EnemyControlTrackMixer>.Create(graph, inputCount);
    }
}

