using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusInteractable : MonoBehaviour
{
    public PlayerController playerController;
    public Transform pickUpDestination;

    // keep track of objects in focus collider box
    public SortedList<float, GameObject> itemsInFocus = new SortedList<float, GameObject>();

    void Awake(){
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider item){
        if (item.tag == "Interactable"){
            // nastav pickupdestination
            Pickup pick = item.gameObject.GetComponent<Pickup>();
            if (pick != null){
                pick._pickUpDestination = pickUpDestination;
            }
            // get distance
            float distance = Vector3.Distance(item.gameObject.transform.position, transform.position);

            // if this object is already in list, update key 
            if (itemsInFocus.ContainsValue(item.gameObject)){
                int idx = itemsInFocus.IndexOfValue(item.gameObject);
                itemsInFocus.RemoveAt(idx);
            }
            itemsInFocus.Add(distance, item.gameObject);

            // now focus on the closest item
            ReFocusItem();
        }
    }

    private void OnTriggerStay(Collider item){
        if (item.tag == "Interactable"){
            float distance = Vector3.Distance(item.gameObject.transform.position, transform.position);
            if (itemsInFocus.ContainsValue(item.gameObject)){
                int idx = itemsInFocus.IndexOfValue(item.gameObject);
                itemsInFocus.RemoveAt(idx);
            }
            itemsInFocus.Add(distance, item.gameObject);
            ReFocusItem();
        }
    }

    private void OnTriggerExit(Collider item){
        if (item.tag == "Interactable"){
            if (itemsInFocus.ContainsValue(item.gameObject)){
                // if the item is in the list, remove it
                int idx = itemsInFocus.IndexOfValue(item.gameObject);
                itemsInFocus.RemoveAt(idx);
                // disable highlight - just to be shure
                item.gameObject.GetComponent<Interactable>().DeFocus();
            }else{
                Debug.Log("This situation should not happen!");
            }
            ReFocusItem();
        }
    }

    private void ReFocusItem(){
        // if player is already carrying some object, don't focus anything else
        if (playerController.carryingObject)
            return;

        // get the closest object
        if (itemsInFocus.Count == 0){
            playerController.focusObject = null;
            return;
        }

        GameObject focusObject = itemsInFocus.Values[itemsInFocus.Count -1 ];
        // nothing to focus
        if (focusObject == null){
            return;
        }

        if (playerController.focusObject != focusObject){
            // we only want single item highlighted, so disable hightlight on current
            if (playerController.focusObject != null)
                playerController.focusObject.GetComponent<Interactable>().DeFocus();
            // highlight the object
            focusObject.GetComponent<Interactable>().Focus();
            // set focus object to player controller
            playerController.focusObject = focusObject;
        }
    }

}
