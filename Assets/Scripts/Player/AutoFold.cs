using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFold : MonoBehaviour {

    public float fadeRate = 0.5f;

    private GameObject normalObject;
    private GameObject foldedObject;
    private float fade;
    private bool shouldBeFolded = false;

    public void BeFolded() {
        Fold();
        shouldBeFolded = true;
        fade = 0f;
    }

    private void Start() {
        normalObject = transform.Find("FullSize").gameObject;
        Transform foldedTransform = transform.Find("CutSize");
        if(foldedTransform != null){
            foldedObject = foldedTransform.gameObject;
            Debug.Log("found folded version");
        }

        CopyColiderFromChild();

        // unfold by default
        UnFold();
    }

    // Update is called once per frame
    private void Update() {
        if (fade >= 1 && !shouldBeFolded){
            // unfold
            UnFold();
        }

        if (!shouldBeFolded) {
            fade = Mathf.Min(fade + fadeRate*Time.deltaTime, 1f);
        }
        shouldBeFolded = false;
    }

    void Fold(){
        normalObject.SetActive(false);
        if (foldedObject != null){
            foldedObject.SetActive(true);
        }
    }

    void UnFold(){
        normalObject.SetActive(true);
        if(foldedObject != null){
            foldedObject.SetActive(false);
        }
    }

    void CopyColiderFromChild(){
        MeshCollider wallCollider = gameObject.AddComponent<MeshCollider>();
        MeshCollider fullSizeCollider = normalObject.GetComponent<MeshCollider>();
        wallCollider.sharedMesh = fullSizeCollider.sharedMesh;
    }
}

