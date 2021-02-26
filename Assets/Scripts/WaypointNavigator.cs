using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Waypoint currentWaypoint;

    void Awake(){
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(currentWaypoint.GetPosition());
        
    }

    // Update is called once per frame
    void Update()
    {
        if(reachedDestination()){
            currentWaypoint = currentWaypoint.nextWaypoint;
            agent.SetDestination(currentWaypoint.GetPosition());
        }
        
    }

    bool reachedDestination(){
        if (!agent.pathPending){
            if (agent.remainingDistance <= agent.stoppingDistance){
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f){
                    return true;
                }
            }
        }
        return false;
    }
}
