using UnityEngine;

public class Pickup : Interactable
{
    public Transform _pickUpDestination;
    private bool carying = false;

    public override void Interact(){
        if (carying){
            // DROP OBJECT
            gameObject.AddComponent<Rigidbody>();

            transform.parent = null;
            carying = false;
        }else{
            // PICK UP OBJECT
            Destroy(GetComponent<Rigidbody>());

            // set position to pickUpDestination
            transform.position = _pickUpDestination.position;
            transform.rotation = _pickUpDestination.rotation;
            transform.parent = _pickUpDestination.transform;
            carying = true;
        }
    }
}
