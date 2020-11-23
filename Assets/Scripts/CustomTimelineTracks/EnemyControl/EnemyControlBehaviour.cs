using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EnemyControlBehaviour : PlayableBehaviour
{
    public GameObject enemy;
    public Vector3 targetPosition;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (enemy != null && Application.isPlaying)
        {
            UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.SetDestination(targetPosition);
        }
    }

}

// public class LightControlBehaviour : PlayableBehaviour
// {
//    public Light light = null;
//    public Color color = Color.white;
//    public float intensity = 1f;
//   
//     public override void ProcessFrame(Playable playable, FrameData info, object playerData)
//    {
//        if (light != null)
//        {
//            light.color = color;
//            light.intensity = intensity;
//        }
//    }
// }
