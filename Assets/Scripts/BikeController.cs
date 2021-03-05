using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BikeController : MonoBehaviour
{
    public float speed = 2f;

    private InputMaster controls = null;
    private Vector2 moveVector = Vector2.zero;
    private CharacterController controller;
    private Vector3 scrollSpeed = new Vector3(4f, 0f, 0f);

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
        //transform.position = transform.position - transform.forward * Time.deltaTime * speed;
        Vector3 horizontal = -transform.forward * moveVector.y;
        Vector3 vertical = -transform.right * moveVector.x;
        Vector3 direction = horizontal + vertical;

        // pridaj rýchlosť
        direction *= speed;

        // pridaj scroll speed vektor
        direction += scrollSpeed;

        // posúvaj hráčika
        controller.Move(direction * Time.deltaTime);
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
