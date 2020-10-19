using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSight : MonoBehaviour
{

    public Material transparentMaterial = null;
    public float fadeInTimeout = 0.6f;
    public float fadeOutTimeout = 0.2f;
    public float targetTransparency = 0.3f;
    public float transparencyRadius = 5f;
    public GameObject player;

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

        int layerMask = 1 << 1;

        //hits = Physics.RaycastAll(transform.position, direction, distanceToPlayer);
        hits = Physics.CapsuleCastAll(transform.position, transform.position, transparencyRadius, direction, distanceToPlayer-transparencyRadius, layerMask);

        foreach (RaycastHit hit in hits)
        {
            Renderer R = hit.collider.GetComponent<Renderer>();
            if (R == null)
                continue;
            // no renderer attached? go to next hit 
            // TODO: maybe implement here a check for GOs that should not be affected like the player
            AutoTransparent AT = R.GetComponent<AutoTransparent>();

            if (AT == null) // if no script is attached, attach one
            {
                AT = R.gameObject.AddComponent<AutoTransparent>();
                AT.TransparentMaterial = transparentMaterial;
                AT.FadeInTimeout = fadeInTimeout;
                AT.FadeOutTimeout = fadeOutTimeout;
                AT.TargetTransparency = targetTransparency;
            }
            AT.BeTransparent(); // get called every frame to reset the falloff
        }
    }
}

