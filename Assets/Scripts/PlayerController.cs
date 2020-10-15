using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    public float moveSpeed = 2f;
    public float turnSmoothTime = 0.05f;
    // this is here because of weird blender export and rotations
    // public float defaultRotationX = 0f;
    public Animator animator;

    float turnSmoothVelocity;

    // isometric direction vectors
    private Vector3 forwardDirection = new Vector3(0.6f, 0f, 0.6f);
    private Vector3 rightDirection = new Vector3(0.6f, 0f, -0.6f);

    // Update is called once per frame
    void Update()
    {
        float speed = Move();
        if (speed >= 0.1f)
        {
            // Set the animator - is moving to True so we have moving animation
            animator.SetBool("IsMoving", true);
        }
        else
        {
            // Set the animator - is moving to False so we have idle animation
            animator.SetBool("IsMoving", false);
        }
    }

    float Move()
    {
        // get the horizontal and vertical buttons
        // use right and forward direction vectors to fix movement in
        // isometric view
        Vector3 horizontal = rightDirection * Input.GetAxisRaw("Horizontal");
        Vector3 vertical = forwardDirection * Input.GetAxisRaw("Vertical");
        // create direction vector
        Vector3 direction = Vector3.Normalize(horizontal + vertical);

        if (direction.magnitude >= 0.1f)
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

        // return speed for other purposes
        return direction.magnitude;
    }
}
