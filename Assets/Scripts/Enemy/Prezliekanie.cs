using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prezliekanie : MonoBehaviour
{

    public Material nakedMaterial;

    public void GetNaked(){
        SkinnedMeshRenderer r = GetComponentInChildren<SkinnedMeshRenderer>();
        r.material = nakedMaterial;
    }

}
