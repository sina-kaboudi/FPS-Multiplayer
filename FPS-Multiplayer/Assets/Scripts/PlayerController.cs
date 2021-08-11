using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform viewPoint;
    [SerializeField] float mouseSensetivity = 1f;
    [SerializeField] float walkSpeed = 5f, runSpeed = 8f;
    [SerializeField] CharacterController characterController;
    [SerializeField] float jumpForce = 12f, gravityModifier = 2.5f;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask groundLayers;

    float verticleRotStore;
    Vector2 mouseInput;
    Vector3 moveInput, movement;
    float currentMoveSpeed;
    Camera cam;
    bool isGrounded;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensetivity;

        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + mouseInput.x,
            transform.rotation.eulerAngles.z);

        verticleRotStore += mouseInput.y;
        verticleRotStore = Mathf.Clamp(verticleRotStore, -60f, 60);
    
        viewPoint.rotation = Quaternion.Euler(
            -verticleRotStore,
            viewPoint.rotation.eulerAngles.y,
            viewPoint.rotation.eulerAngles.z);


        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = runSpeed;
        }
        else
        {
            currentMoveSpeed = walkSpeed;
        }

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        float yVelocity = movement.y;
        movement = ((transform.forward * moveInput.z) + (transform.right * moveInput.x)).normalized * currentMoveSpeed;
        movement.y = yVelocity;
        if (characterController.isGrounded)
        {
            movement.y = 0;
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.35f, groundLayers);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            movement.y = jumpForce;
        }

        movement.y += Physics.gravity.y * Time.deltaTime * gravityModifier;

        characterController.Move(movement * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void LateUpdate()
    {
        cam.transform.position = viewPoint.transform.position;
        cam.transform.rotation = viewPoint.transform.rotation;
    }
}