using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public string side;
    public Door doorRef;

    private void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            doorRef.OpenDoor(side);
            gameObject.SetActive(false);
        }
    }

}
