using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BikeController : MonoBehaviour
{
    public float bikeSpeed = 2f;
    public float bikeSteering = 4f;
    public Vector3 scrollSpeed = new Vector3(4f, 0f, 0f);
    public float pushPower = 2.0f;

    private InputMaster controls = null;
    private Vector2 moveVector = Vector2.zero;
    private CharacterController controller;
    private float gravity = -9.81f;
    private float vSpeed = 0f;

    void Awake()
    {
        controls = new InputMaster();
        controls.Gameplay.Move.performed += ctx => moveVector = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveVector = Vector2.zero;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // rovno pripočítaj rýchlosť vytáčania aj rýchlosť motorky
        Vector3 horizontal = -transform.forward * moveVector.y * bikeSpeed;
        Vector3 vertical = -transform.right * moveVector.x * bikeSteering;
        Vector3 direction = horizontal + vertical;

        // pridaj scroll speed vektor
        direction = direction + scrollSpeed;

        // posúvaj hráčika
        controller.Move(direction * Time.deltaTime);

        // gravity
        vSpeed += gravity * Time.deltaTime;
        if (controller.isGrounded)
            vSpeed = 0f;
        Vector3 velocity = new Vector3(0f, vSpeed, 0f);
        controller.Move(velocity * Time.deltaTime);
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

    public void EnableControls(){
        controls.Gameplay.Enable();
    }

    public void DisableControls(){
        controls.Gameplay.Disable();
    }


    void OnEnable(){
        EnableControls();
    }

    void OnDisable(){
        DisableControls();
    }
}
