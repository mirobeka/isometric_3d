using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class EnemyControlTrackMixer : PlayableBehaviour
{

	private GameObject enemy;
	private bool firstFrameHappened = false;
	private Vector3 defaultPosition, newPosition, finalPosition, previousInputFinalPosition;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        
        enemy = playerData as GameObject;

        if (enemy == null)
            return;

		if (!firstFrameHappened)
		{
			defaultPosition = enemy.transform.position;
			firstFrameHappened = true;
		}

		//different behaviour depending if Unity is in Play mode or not,
		//because NavMeshAgent is not available in Edit mode
		if(Application.isPlaying)
		{
			ProcessPlayModeFrame(playable);
		}
		else
		{
			ProcessEditModeFrame(playable);
		}

    }

	//Happens every frame in Edit mode.
	//Uses transform.position of the enemy to approximate what they would do in Play mode with the NavMeshAgent
	private void ProcessEditModeFrame(Playable playable)
	{
		previousInputFinalPosition = defaultPosition;
		int inputCount = playable.GetInputCount();

		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);

			ScriptPlayable<EnemyControlBehaviour> inputPlayable;
            inputPlayable = (ScriptPlayable<EnemyControlBehaviour>)playable.GetInput(i);
			EnemyControlBehaviour input = inputPlayable.GetBehaviour();

			//final position je to isté ako targetposition
			finalPosition = input.targetPosition;

			if(inputWeight > 0f)
			{
				double progress = inputPlayable.GetTime()/inputPlayable.GetDuration();
				newPosition= Vector3.Lerp(previousInputFinalPosition, finalPosition, (float)progress);
				enemy.transform.position = newPosition;
				continue;
			}
			else
			{
				previousInputFinalPosition = finalPosition; //cached to act as initial position for the next input
			}
		}
	}

	//Happens in Play mode
	//Uses the NavMeshAgent to control the units, delegating their movement and animations to the AI
	private void ProcessPlayModeFrame(Playable playable)
	{

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0f)
            {
                ScriptPlayable<EnemyControlBehaviour> inputPlayable;
                inputPlayable = (ScriptPlayable<EnemyControlBehaviour>)playable.GetInput(i);
                EnemyControlBehaviour input = inputPlayable.GetBehaviour();

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                agent.SetDestination(input.targetPosition);
            }
        }
	}

	public override void OnPlayableDestroy(Playable playable)
	{
		if(!Application.isPlaying)
		{
			firstFrameHappened = false;
			
			if (enemy == null)
				return;
			
			enemy.transform.position = defaultPosition;
		}
	}
}
