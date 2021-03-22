using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mu : MonoBehaviour
{
    public bool sliceNow = false;

    private GameObject victim;
    private GameObject plane;

    void Start(){
        victim = transform.Find("Wall").gameObject;
        plane = transform.Find("CutPlane").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(sliceNow){
            DoSliceObject();
            sliceNow = false;
        }
    }

    void DoSliceObject(){

        Vector3 normal = plane.transform.up;

        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(victim.transform.localToWorldMatrix.transpose * normal)).normalized;
        Debug.Log(transformedNormal);

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = victim.gameObject.transform.InverseTransformPoint(plane.transform.position);

        Plane _plane = new Plane();

        _plane.SetNormalAndPosition(
                transformedNormal,
                transformedStartingPoint);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            _plane = _plane.flipped;
        }

        GameObject slice = Slicer.Slice(_plane, victim);
        Destroy(victim.gameObject);
    }
}
