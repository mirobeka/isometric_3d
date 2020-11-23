using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EnemyControlAsset : PlayableAsset
{
   public ExposedReference<GameObject> enemy;
   public Vector3 targetPosition;

   public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
   {
       var playable = ScriptPlayable<EnemyControlBehaviour>.Create(graph);
      
       var enemyControlBehaviour = playable.GetBehaviour();
       enemyControlBehaviour.enemy = enemy.Resolve(graph.GetResolver());
       enemyControlBehaviour.targetPosition = targetPosition;

       return playable;   
   }
}
