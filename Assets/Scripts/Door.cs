using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnimator;
    public GameObject doorTrigger;
    // Start is called before the first frame update

    void Awake(){
        doorAnimator = GetComponentInChildren<Animator>();
    }

    void Start(){
        doorTrigger = transform.Find("OpenDoorTrigger").gameObject;
        DoorTrigger trigger = doorTrigger.GetComponent<DoorTrigger>();
        trigger.doorRef = this;
    }

    public virtual void OpenDoor(string sideToOpen){
        doorAnimator.Play(sideToOpen);
        StartCoroutine(CloseDoorWithDelay(5));

    }

    public virtual IEnumerator CloseDoorWithDelay(int delay){
        yield return new WaitForSeconds(delay);
        doorAnimator.Play("CloseSide1");
        doorTrigger.SetActive(true);
    } 
}
