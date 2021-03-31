using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            float force = collision.relativeVelocity.magnitude;
            collision.collider.GetComponent<PlayerHealth>().TakeDamage(force);
        }
    }


}
