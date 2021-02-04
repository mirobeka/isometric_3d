using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RemoveShadow : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        MeshRenderer mRenderer = this.GetComponent<MeshRenderer>();
        mRenderer.receiveShadows = false;
        mRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
    }
}
