using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoniecTrate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        Debug.Log("triggered");
        //pozri ci je to elektricka
        if (other.tag == "Elektricka"){
            Destroy(other.gameObject);
        }
    }
}
