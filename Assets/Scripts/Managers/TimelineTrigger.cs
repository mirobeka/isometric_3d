using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TimelineTrigger : MonoBehaviour
{
    public TimelineAsset scene;
    public bool isEnabled = false;
    public bool wasTriggered = false;

	void Start () {
        DisableTrigger();
	}

    private void OnTriggerEnter(Collider other){
        //check if colliding object is player
        if (other.tag == "Player"){
            PlayTimelineAsset();
            // disable further triggers
            DisableTrigger();
            wasTriggered = true;
        }
    }

    public void PlayTimelineAsset(){
        if (scene != null)
            GameManager.Instance.SetTimelinePlayable(scene);
    }

    public void EnableTrigger(){
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        isEnabled = true;
    }

    public void DisableTrigger(){
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        isEnabled = false;
    }


}

