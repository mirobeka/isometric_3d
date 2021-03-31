using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestination : MonoBehaviour
{
    public ItemManager itemManager;
    public Material placeholderMat;


    public void SetMesh(GameObject newVisualPrefab){
        GameObject origVisual = transform.Find("Visual").gameObject;
        Destroy(origVisual);

        GameObject newVisual = Instantiate(newVisualPrefab, this.transform);
        // vymaz tag
        newVisual.tag = "Untagged";
        newVisual.transform.position = this.transform.position;
        newVisual.transform.rotation = this.transform.rotation;

        Renderer[] rs = newVisual.GetComponentsInChildren<Renderer>();

        // special material to all meshes in children
        foreach(Renderer r in rs){
            r.material = placeholderMat;
        }

        // zbav sa niektorých vlastností tohto predmetu
        Rigidbody[] rbs = newVisual.GetComponentsInChildren<Rigidbody>();
        // special material to all meshes in children
        foreach(Rigidbody rb in rbs){
            Destroy(rb);
        }

        BoxCollider[] colliders = newVisual.GetComponentsInChildren<BoxCollider>();
        // special material to all meshes in children
        foreach(BoxCollider collider in colliders){
            Destroy(collider);
        }

        Pickup[] pus = newVisual.GetComponentsInChildren<Pickup>();
        // special material to all meshes in children
        foreach(Pickup pu in pus){
            Destroy(pu);
        }
    }


    private void OnTriggerEnter(Collider other){
        if (other.tag == "Interactable"){
            Debug.Log("Prisiel do kontaktu");
            other.gameObject.transform.position = transform.position;
            other.gameObject.transform.rotation = transform.rotation;

            //
            itemManager.ItemInPosition();
            Destroy(gameObject);
        }
    }

}
