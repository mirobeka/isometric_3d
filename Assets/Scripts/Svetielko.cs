using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Svetielko : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;
    public bool svieti = false;

    private Light svetlo;
    private Renderer rndr;

    // Start is called before the first frame update
    void Start()
    {
        rndr = GetComponentInChildren<Renderer>();
        svetlo = GetComponentInChildren<Light>();
        StartCoroutine(AutoVypinac());
    }

    IEnumerator AutoVypinac() 
    {
        yield return new WaitForSeconds(2);
        svieti = false;
        StartCoroutine(AutoVypinac());
    }

    // Update is called once per frame
    void Update()
    {
        if(svieti){
            Zapni();
        }else{
            Vypni();
        }
    }

    public void Zapni(){
        rndr.material = onMaterial;
        svetlo.enabled = true;
    }

    public void Vypni(){
        rndr.material = offMaterial;
        svetlo.enabled = false;
    }

    private void OnTriggerStay(Collider other){
        if (other.tag == "Interactable"){
        Debug.Log("Stay: Zapni");
            svieti = true;
        }

    }

}
