using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BikeController : MonoBehaviour
{
    public float bikeSpeed = 2f;
    public float bikeSteering = 4f;
    public Vector3 scrollSpeed = new Vector3(4f, 0f, 0f);

    private InputMaster controls = null;
    private Vector2 moveVector = Vector2.zero;
    private CharacterController controller;

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
