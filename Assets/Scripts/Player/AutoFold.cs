using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFold : MonoBehaviour {

    public float fadeRate = 0.5f;
    public Renderer rend;

    public float cutPlanePosition = 1.3f;
    public float normalPlanePosition = 10f;

    private float _fade; //malá pomocná premenná
    private bool shouldBeFolded = false;
    private MeshCollider _collider;

    public void BeFolded() {
        Fold();
        shouldBeFolded = true;
        _fade = 0f;
    }

    private void Start() {
        rend = GetComponent<Renderer>();
        if (rend == null){
            rend = GetComponentInChildren<Renderer>();
            _collider = GetComponentInChildren<MeshCollider>();
            CopyColiderFromChild();
        }
        _fade = 1f;
        shouldBeFolded = false;

        // unfold by default
        UnFold();
    }

    // Update is called once per frame
    private void Update() {
        if (_fade >= 1 && !shouldBeFolded){
            // unfold
            UnFold();
        }else if(_fade < 1 && !shouldBeFolded) {
            _fade = Mathf.Min(_fade + fadeRate*Time.deltaTime, 1f);
        }
        shouldBeFolded = false;
    }

    void Fold(){
        rend.material.SetFloat("_PlanePosition", cutPlanePosition);
    }

    void UnFold(){
        rend.material.SetFloat("_PlanePosition", normalPlanePosition);
    }

    void CopyColiderFromChild(){
        MeshCollider wallCollider = gameObject.AddComponent<MeshCollider>();
        wallCollider.sharedMesh = _collider.sharedMesh;
    }

}

