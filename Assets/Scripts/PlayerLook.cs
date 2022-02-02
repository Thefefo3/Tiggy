using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{


    public Animator anim;
    [Header("References")]
    [SerializeField] WallRunning wallRunning;

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam;
    [SerializeField] Transform body;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        MyInput();

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRunning.tilt);
        body.transform.localRotation = Quaternion.Euler(0, yRotation, wallRunning.tilt);

        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");


        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        



        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
