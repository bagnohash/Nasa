using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_movement : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 7.0f;
    public float gravity = 20.0f;
    public float sensitivity = 3.0f;
    public float minY = -90.0f;
    public float maxY = 90.0f;
    private bool isCrouching = false;
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;

    private CharacterController controller;
    private Vector3 movementDirection = Vector3.zero;
    private float rotation = 0.0f;
    private float cameraRotation = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
	Cursor.visible=false;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Poruszanie
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 movement = transform.forward * vertical * speed + transform.right * horizontal * speed;

        //skok
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                movementDirection.y = jumpForce;
            }
        }

        //kucanie
        if (isCrouching==false)     //usuwa warna
        {
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = crouchingHeight;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            controller.height = standingHeight;
        }

        movementDirection.y -= gravity * Time.deltaTime;
        movement += movementDirection;
        controller.Move(movement * Time.deltaTime);

        // Rotacja postaci za pomocą myszy
        rotation += Input.GetAxis("Mouse X") * sensitivity;
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        // Poruszanie kamery za pomocą myszy
        cameraRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        cameraRotation = Mathf.Clamp(cameraRotation, minY, maxY);
        Camera.main.transform.localRotation = Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
    }
}