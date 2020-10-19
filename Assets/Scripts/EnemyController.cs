using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public CharacterController controller;
    public float moveSpeed = 1.8f;
    public float turnSmoothTime = 0.05f;
    public Animator animator;

    private float turnSmoothVelocity;

    void Update()
    {

        float distanceToPlayer = Move();
        // Set the animator - is moving to True so we have moving animation
        animator.SetBool("IsWalking",  (distanceToPlayer >= 2.0f));
    }

    float Move()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);

        if (distanceToPlayer >= 2.0f)
        {
            // character rotation smooting
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            // I've figured out how to export blender -> unity correctly so
            // not using this hack anymore. But I'll keep it here
            //
            //   transform.rotation = Quaternion.Euler(defaultRotationX, angle, 0f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // use character controller for movement
            controller.Move(direction * moveSpeed * Time.deltaTime);
        }
        return distanceToPlayer;
    }
}
