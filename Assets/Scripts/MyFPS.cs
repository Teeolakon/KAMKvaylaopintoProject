using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFPS : MonoBehaviour
{

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 3f;
    public float lookSpeed = 2f;
    public Transform cameraTransform;

    private CharacterController playerController;
    private bool isJumping;
    private Vector3 playerVel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        //Player movement
        float movementSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        playerController.Move(move * movementSpeed * Time.deltaTime);
        
        //Player jump
        if(playerController.isGrounded)
        {
            playerVel.y = 0f;
            isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            playerVel.y += Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            isJumping = true;
        }

        playerVel.y += Physics.gravity.y * Time.deltaTime;
        playerController.Move(playerVel * Time.deltaTime);


        //Player look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
        transform.Rotate(Vector3.up * mouseX);

        //Camera look
        Vector3 currentRotation = cameraTransform.rotation.eulerAngles;
        float desiredRotationX = currentRotation.x - mouseY;

        if (desiredRotationX > 180)
            desiredRotationX -= 360;

        desiredRotationX = Mathf.Clamp(desiredRotationX, -90f, 90f);
        cameraTransform.rotation = Quaternion.Euler(desiredRotationX, currentRotation.y, currentRotation.z);

    }
}
