using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elektricka : MonoBehaviour
{
    public List<Vector3> stops;
    public float speed = 0.1f;
    public float stopWait = 10;

    private int currentStop = 0;
    private bool moving = true;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving && (transform.position == stops[currentStop])){
            moving = false;
            StartCoroutine(selectNextStop());
        }

        transform.position = Vector3.MoveTowards(transform.position, stops[currentStop], speed);
    }

    private IEnumerator selectNextStop()
    {
        // počkaj na zastavke
        yield return new WaitForSeconds(stopWait);
        // TODO: zazvon

        if (currentStop+1 < stops.Count){
            // nastav ďalšiu zastávku ak ju máš
            currentStop += 1;
            moving = true;
        }
    }
}
