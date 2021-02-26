using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{

    public GameObject personPrefab;
    public int personsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(Spawn()); 
    }

    IEnumerator Spawn(){
        int count = 0;
        while (count < personsToSpawn){
            GameObject obj = Instantiate(personPrefab);
            UnityEngine.AI.NavMeshAgent agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.speed = Random.Range(2.5f, 4.5f);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            Waypoint waypoint = child.GetComponent<Waypoint>();
            obj.GetComponent<WaypointNavigator>().currentWaypoint = waypoint; 
            obj.transform.position = waypoint.GetPosition();
            
            yield return new WaitForEndOfFrame();

            count++;

        }
    }

}
