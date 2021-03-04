using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("IsRiding", true);
    }
}
