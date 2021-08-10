using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform viewPoint;
    [SerializeField] float mouseSensetivity = 1f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] CharacterController characterController;

    float verticleRotStore;
    Vector2 mouseInput;
    Vector3 moveInput, moveDirection;

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

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveDirection = ((transform.forward * moveInput.z) + (transform.right * moveInput.x)).normalized;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}