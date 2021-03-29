using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestination : MonoBehaviour
{
    public Transform finalPosition;
    public ItemManager itemManager;
    public Material placeholderMat;

    // Start is called before the first frame update
    void Start()
    {
        finalPosition = transform.Find("FinalPosition");
    }

    public void SetMesh(GameObject newVisualPrefab){
        GameObject origVisual = transform.Find("Visual").gameObject;
        Destroy(origVisual);

        GameObject newVisual = Instantiate(newVisualPrefab, this.transform);
        Renderer[] rs = newVisual.GetComponentsInChildren<Renderer>();

        // special material to all meshes in children
        foreach(Renderer r in rs){
            r.material = placeholderMat;
        }

        Rigidbody[] rbs = newVisual.GetComponentsInChildren<Rigidbody>();

        // special material to all meshes in children
        foreach(Rigidbody rb in rbs){
            Destroy(rb);
        }
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
