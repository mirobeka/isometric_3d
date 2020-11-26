using UnityEngine;

public class Pickup : Interactable
{
    public Transform pickUpDestination;
    private bool carying = false;

    public override void Interact(){
        if (carying){
            // DROP OBJECT
            GetComponent<Rigidbody>().isKinematic=false;
            // GetComponent<Rigidbody>().useGravity=true;
            // GetComponent<Rigidbody>().freezeRotation=false;
            transform.parent = null;
            carying = false;
        }else{
            // PICK UP OBJECT
            GetComponent<Rigidbody>().isKinematic=true;
            // GetComponent<Rigidbody>().useGravity=false;
            // GetComponent<Rigidbody>().freezeRotation=true;

            // set rotation to default
            transform.rotation = Quaternion.identity;

            // set position to pickUpDestination
            transform.position = pickUpDestination.position;
            transform.parent = GameObject.Find("Player").transform;
            carying = true;
        }
    }
}
