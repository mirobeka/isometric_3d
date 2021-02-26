using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Waypoint currentWaypoint;
    public int direction;

    void Awake(){
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
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

            bool shouldBranch = shouldTakeBranch();
            if(shouldBranch){
                currentWaypoint = currentWaypoint.branches[
                    Random.Range(0, currentWaypoint.branches.Count -1)];
            }

            if (direction == 0){

                if (currentWaypoint.nextWaypoint != null){
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }else{
                    currentWaypoint = currentWaypoint.previousWaypoint;
                    direction = 1;
                }
            }else if (direction == 1){
                if (currentWaypoint.previousWaypoint != null){
                    currentWaypoint = currentWaypoint.previousWaypoint;
                }else{
                    currentWaypoint = currentWaypoint.nextWaypoint;
                    direction = 0;
                }
            }
            agent.SetDestination(currentWaypoint.GetPosition());
        }
        
    }
    
    bool shouldTakeBranch(){
        if(currentWaypoint.branches != null && currentWaypoint.branches.Count > 0){
            return Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
        }
        return false;
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
