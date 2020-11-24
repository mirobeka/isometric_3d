using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TimelineTrigger : MonoBehaviour
{
    public TimelineAsset scene;

	void Start () {
        GetComponent<MeshRenderer>().enabled = false;
	}

    private void OnTriggerEnter(Collider other){
        //check if colliding object is player
        if (other.tag == "Player"){
            PlayTimelineAsset();
            // disable further triggers
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void PlayTimelineAsset(){
        GameManager.Instance.SetTimelinePlayable(scene);
    }

}

