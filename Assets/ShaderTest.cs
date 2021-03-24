using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    public bool fade;
    private Material mat;

    void Start(){
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if(fade){
            mat.SetFloat("_PlanePosition", 1.3f);
        }else{
            mat.SetFloat("_PlanePosition", 10f);
        }
    }
}
