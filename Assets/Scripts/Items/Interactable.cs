using UnityEngine;
using cakeslice;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(){
        Debug.Log("Interacting with " + transform.name);
    }

    public virtual void Focus(){
        Outline outline = GetComponent<Outline>();
        outline.eraseRenderer = false;
    }

    public virtual void DeFocus(){
        Outline outline = GetComponent<Outline>();
        outline.eraseRenderer = true;
    }

}
