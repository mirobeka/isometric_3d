using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestination : MonoBehaviour
{
    public int itemId;
    public Transform finalPosition;
    public ItemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
        finalPosition = transform.Find("FinalPosition");
    }


    private void OnTriggerEnter(Collider other){
        if (other.tag == "Interactable"){
            Debug.Log("Prisiel do kontaktu");
            other.gameObject.transform.position = finalPosition.position;
            other.gameObject.transform.rotation = finalPosition.rotation;

            //
            itemManager.ItemInPosition();
            Destroy(gameObject);
        }
    }

}
