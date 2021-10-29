using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Player")]
    float playerHeight = 2f;

    [Header("Jumping")]
    public float jumpForce = 9f;
    public float maxJumps = 2;
    public float jumpsLeft = 0;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sprintSpeed = 9f;
    [SerializeField] float acceleration = 15f;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.1f;
    public bool isGrounded { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    float horizotalMovement;
    float verticalMovement;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }


    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        MyInput();
        ControlDrag();
        ControlSpeed();

        //Jump
        if(Input.GetKeyDown(jumpKey) && isGrounded || Input.GetKeyDown(jumpKey) && jumpsLeft > 0)
        {
            Jump();
        }
        if (isGrounded)
        {
            jumpsLeft = maxJumps - 1;
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void Jump()
    {
        if (isGrounded || jumpsLeft > 0)
        {
            jumpsLeft = jumpsLeft - 1;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void MyInput()
    {
        horizotalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizotalMovement;

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
       
    }
}
