using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class BoardingScript : MonoBehaviour
{
    public TimelineTrigger trigger;
    public Elektricka autobus;

    public TimelineAsset nastupila;
    public TimelineAsset nenastupila;
    private bool oneShot = false;


    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponentInChildren<TimelineTrigger>();
        autobus = GetComponent<Elektricka>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!autobus.moving && !trigger.isEnabled && !trigger.wasTriggered){
            trigger.scene = nastupila;
            trigger.EnableTrigger();
            oneShot = true;
        }else if(autobus.moving && trigger.isEnabled){
            trigger.DisableTrigger();
        }

        // ak NEnastupila tak spusti druhú alternatívu
        if (autobus.moving && !trigger.wasTriggered && oneShot){
            Debug.Log("Nenastupila!!!!!!!");
            trigger.scene = nenastupila;
            trigger.PlayTimelineAsset();
            oneShot = false;
        }
    }
}
