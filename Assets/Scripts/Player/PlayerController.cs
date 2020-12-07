using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    public float walkSpeed = 2f;
    public float runSpeed = 4.5f;
    public float turnSmoothTime = 0.05f;
    public float pushPower = 2.0f;
    // this is here because of weird blender export and rotations
    // public float defaultRotationX = 0f;
    public Animator animator;

    float turnSmoothVelocity;

    // isometric direction vectors
    // these needs to be added to controls direction vector
    private Vector3 forwardDirection = new Vector3(0.6f, 0f, 0.6f);
    private Vector3 rightDirection = new Vector3(0.6f, 0f, -0.6f);

    // using this for gravity
    private float vSpeed = 0f;
    private float gravity = -9.81f;

    // controls
    private InputMaster controls = null;
    private Vector2 moveVector = Vector2.zero;
    private bool isRunning = false;

    // Interactions
    public GameObject focusObject = null;
    public bool carryingObject = false;

    void Awake(){
        controls = new InputMaster();

        // define all controls / interaction / etc
        controls.Gameplay.Move.performed += ctx => moveVector = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveVector = Vector2.zero;

        controls.Gameplay.Run.performed += ctx => isRunning = true;
        controls.Gameplay.Run.canceled += ctx => isRunning = false;

        controls.Gameplay.Interact.performed += ctx => StartInteract();
        // this needs to be handled better way. This is buggy
        // controls.Gameplay.Interact.performed += ctx => StopInteract();
    }

    // Update is called once per frame
    void Update()
    {
        float velocity = Move();
        // Set the animator
        bool isWalking = velocity >= 0.1f;

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsRunning", isRunning);

    }

    float Move()
    {
        // get the horizontal and vertical buttons
        // use right and forward direction vectors to fix movement in
        // isometric view
        Vector3 horizontal = rightDirection * moveVector.x;
        Vector3 vertical = forwardDirection * moveVector.y;

        // check if player is running
        float moveSpeed = isRunning ? runSpeed : walkSpeed;

        // create direction vector
        // Vector3 direction = Vector3.Normalize(horizontal + vertical);
        // try not to normalize the direction vector (better for controller)
        Vector3 direction = horizontal + vertical;
        Vector3 speed = new Vector3(0f, 0f, 0f);

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
            speed = direction * moveSpeed;
            controller.Move(speed * Time.deltaTime);
        }

        // gravity
        vSpeed += gravity * Time.deltaTime;
        if (controller.isGrounded)
            vSpeed = 0f;
        Vector3 velocity = new Vector3(0f, vSpeed, 0f);
        controller.Move(velocity * Time.deltaTime);

        // return velocity for other purposes
        return speed.magnitude;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // ITERACT WITH RIGIDBODY
        Rigidbody body = hit.collider.attachedRigidbody;
        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }

    void StartInteract(){
        if (focusObject != null){
            // check if we are interacting with pickupable object
            if (focusObject.GetComponent<Pickup>() != null){
                focusObject.GetComponent<Interactable>().Interact();
                // TODO: is this assumption dangerous?
                // assuming that interaction with this object is to drop it
                carryingObject = !carryingObject;
            }else{
                // otherwise just interact with the bloody thing!
                focusObject.GetComponent<Interactable>().Interact();
            }

        }else{
            Debug.Log("Nothing in focus");
        }
    }

    void StopInteract(){
        if (focusObject != null){
            Debug.Log("Stopping interaction with " + focusObject.name);
        }else{
            Debug.Log("Nothing in focus");
        }
    }

    void OnEnable(){
        controls.Gameplay.Enable();
    }

    void OnDisable(){
        controls.Gameplay.Disable();
    }
}
