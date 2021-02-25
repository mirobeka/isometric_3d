using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elektricka : MonoBehaviour
{
    public float stopWait = 10;
    public float travelDistance = 70f;
    // pohybuj sa iba rovno po x osi
    public Vector3 directionVector = new Vector3(-4f,0f,0f);

    private Vector3 startPosition;
    private bool moving = true;

    void Awake(){
        startPosition = transform.position;
        moving = true;
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float currentDistance = Vector3.Distance(startPosition, transform.position);


        if ( moving && (currentDistance >= travelDistance)){
            // time to stop
            Debug.Log("set moving to false!");
            moving = false;
            StartCoroutine(waitBeforeMovingAgain());
        }

        if (moving){
            Vector3 pos = transform.position + directionVector * Time.deltaTime;
            transform.position = pos;
        }

    }

    private IEnumerator waitBeforeMovingAgain()
    {
        Debug.Log("wait!!");
        // počkaj na zastavke
        yield return new WaitForSeconds(stopWait);
        // TODO: zazvon

        Debug.Log("Start moving again");
        startPosition = transform.position;
        moving = true;
    }
}
