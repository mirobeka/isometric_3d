using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vytah : MonoBehaviour
{
    public float vSpeed = 1f;

    private bool navmeshAgentDisabled = false;
    // Update is called once per frame
    void Update()
    {
        if(!navmeshAgentDisabled){
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            navmeshAgentDisabled = true;
        }

        transform.position = transform.position + new Vector3(0f, vSpeed * Time.deltaTime, 0f);
    }
}
