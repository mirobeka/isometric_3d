using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public UnityEngine.AI.NavMeshAgent agent;

    public bool standToSide = false;
    public float distance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (standToSide){
            // ktorá strana je bližšie?
            Vector3 rightSide = player.transform.position + player.transform.right * distance;
            Vector3 leftSide = player.transform.position + -player.transform.right * distance;

            float distanceToRightSide = Vector3.Distance(transform.position, rightSide);
            float distanceToLeftSide = Vector3.Distance(transform.position, leftSide);

            Vector3 dest = distanceToLeftSide <= distanceToRightSide ? leftSide : rightSide;

            agent.SetDestination(dest);
            // musíš presne chodiť
            SetAgentStoppingDistance(0f);
            float distanceToTarget = Vector3.Distance(dest, transform.position);
            if(distanceToTarget <= 1){
                // ak už si na svojom mieste, otoč sa
                transform.rotation =  Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime);
            }
        }else{
            SetAgentStoppingDistance(1.5f);
            agent.SetDestination(player.transform.position);
        }
    }

    void SetAgentStoppingDistance(float newDistance){
        if (agent.stoppingDistance == newDistance){
            return;
        }
        agent.stoppingDistance = newDistance;
    }
}
