using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardingScript : MonoBehaviour
{
    public TimelineTrigger trigger;
    public Elektricka autobus;


    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponentInChildren<TimelineTrigger>();
        autobus = GetComponent<Elektricka>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!autobus.moving && !trigger.isEnabled){
            trigger.EnableTrigger();
        }else if( autobus.moving && trigger.isEnabled){
            trigger.DisableTrigger();
        }
    }
}
