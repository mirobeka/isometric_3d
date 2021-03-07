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
    public bool moving = true;
    private AudioSource movingSound;
    private AudioSource bellSound;
    private AudioSource idleSound;

    void Awake(){
        startPosition = transform.position;
        moving = true;
        movingSound = transform.Find("Sounds/MovingSound").GetComponent<AudioSource>();
        var idleSoundObject = transform.Find("Sounds/IdleSound");
        if (idleSoundObject != null){
            idleSound = idleSoundObject.GetComponent<AudioSource>();
        }
        bellSound = transform.Find("Sounds/BellSound").GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float currentDistance = Vector3.Distance(startPosition, transform.position);

        if ( moving && (currentDistance >= travelDistance)){
            // time to stop
            moving = false;
            StartCoroutine(waitBeforeMovingAgain());
        }

        if (moving){
            // fade in moving until full volume
            float movingVolume = Mathf.Lerp(movingSound.volume, 1f, Time.deltaTime );
            movingSound.volume = movingVolume;

            // fade out idle until zero volume
            if (idleSound != null){
                float idleVolume = Mathf.Lerp(idleSound.volume, 0f, 2f*Time.deltaTime );
                idleSound.volume = idleVolume;
            }

            Vector3 pos = transform.position + directionVector * Time.deltaTime;
            transform.position = pos;
        }else{
            //fade moving sound to 0
            float volume = Mathf.Lerp(movingSound.volume, 0f, 2f*Time.deltaTime );
            movingSound.volume = volume;

            // fade in idle to 1
            if (idleSound != null){
                float idleVolume = Mathf.Lerp(idleSound.volume, 1f, Time.deltaTime );
                idleSound.volume = idleVolume;
            }
        }

    }
    

    private IEnumerator waitBeforeMovingAgain()
    {
        // počkaj na zastavke
        yield return new WaitForSeconds(stopWait);
        bellSound.Play();
        yield return new WaitForSeconds(2f);

        startPosition = transform.position;
        moving = true;
    }
}
