using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestination : MonoBehaviour
{
    public Transform finalPosition;
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
        }
    }


}
