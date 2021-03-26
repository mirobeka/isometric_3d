using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDoor : Door
{

    public override void OpenDoor(string sideToOpen){
        doorAnimator.Play("LiftOpen");
        StartCoroutine(CloseDoorWithDelay(5));
    }

    public override IEnumerator CloseDoorWithDelay(int delay){
        yield return new WaitForSeconds(delay);
        doorAnimator.Play("LiftClose");
        doorTrigger.SetActive(true);
    } 

}
