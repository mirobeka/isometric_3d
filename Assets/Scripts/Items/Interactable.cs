using UnityEngine;
using cakeslice;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(){
        Debug.Log("Interacting with " + transform.name);
    }

    public virtual void Focus(){
        Outline[] outlines = GetComponentsInChildren<Outline>();
        foreach(Outline outline in outlines){
            outline.eraseRenderer = false;
        }
    }

    public virtual void DeFocus(){
        Outline[] outlines = GetComponentsInChildren<Outline>();
        foreach(Outline outline in outlines){
            outline.eraseRenderer = true;
        }
    }

}
