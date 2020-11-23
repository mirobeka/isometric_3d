using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TimeMachineBehaviour : PlayableBehaviour
{
	public TimeMachineAction action;
	public Condition condition;
	public string markerToJumpTo, markerLabel;
	public float timeToJumpTo;
    public GameObject enemy;

	[HideInInspector]
	public bool clipExecuted = false; //the user shouldn't author this, the Mixer does

	public bool ConditionMet()
	{
        
		switch(condition)
		{
			case Condition.Always:
				return true;
				
			case Condition.WalkingToDestination:
				if(enemy != null)
				{
					return walkingToDestination();
				}
				else
				{
					return false;
				}

			case Condition.Never:
			default:
				return false;
		}
	}

    private bool walkingToDestination(){
        // is enemy at the destination ?
        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }
        return true;
    }

	public enum TimeMachineAction
	{
		Marker,
		JumpToTime,
		JumpToMarker,
		Pause,
	}

	public enum Condition
	{
		Always,
		Never,
		WalkingToDestination,
	}
}
