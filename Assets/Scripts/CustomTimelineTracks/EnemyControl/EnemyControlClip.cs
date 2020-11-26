using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EnemyControlClip : PlayableAsset
{
   public Vector3 targetPosition;

   public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
   {
       var playable = ScriptPlayable<EnemyControlBehaviour>.Create(graph);
      
       var enemyControlBehaviour = playable.GetBehaviour();
       enemyControlBehaviour.targetPosition = targetPosition;

       return playable;   
   }
}
