using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSight2 : MonoBehaviour
{

    public float transparencyRadius = 5f;
    public GameObject player;
    public LayerMask layerToFade;

    private float distanceToPlayer;
    private float castDistance;

    void Start()
    {
        // set distance to player. Our camera has fixed distance to player
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        // try to estimate capsule cast distance, so it doesn't change object
        // behind the player
        castDistance = distanceToPlayer - transparencyRadius - 1f;
    }

    void Update()
    {
        RaycastHit[] hits; // you can also use CapsuleCastAll() 
        // TODO: setup your layermask it improve performance and filter your hits. 
        Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);

        //hits = Physics.RaycastAll(transform.position, direction, distanceToPlayer);
        hits = Physics.CapsuleCastAll(transform.position, transform.position, transparencyRadius, direction, distanceToPlayer-transparencyRadius, layerToFade);

        foreach (RaycastHit hit in hits)
        {
            AutoFold af = hit.collider.GetComponent<AutoFold>();
            if (af == null)
                continue;
            // no autofold attached? go to next hit 
            // TODO: maybe implement here a check for GOs that should not be affected like the player
            af.BeFolded(); // get called every frame to reset the falloff
        }
    }
}

