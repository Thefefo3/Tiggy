using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{

    public FovController fovController;

    public WallRunning wallRunning;

    public Animator anim;

    float rotationFactorPerFrame=1.0f;

    PhotonView PV;

    [SerializeField] CapsuleCollider collider;
    [SerializeField] Transform orientation;

    [Header("Player")]
    float playerHeight = 2f;

    [Header("Jumping")]
    public float jumpForce = 9f;
    public float maxJumps = 2;
    public float jumpsLeft = 0;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.5f;

    [Header("Movement")]
    [SerializeField]public float moveSpeed = 14f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField]public float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 14f;
    [SerializeField] public float sprintSpeed = 14f;
    [SerializeField] float acceleration = 15f;

    [Header("Sliding")]
    [SerializeField] float slidingSpeed = 18f;
    [SerializeField] float slideTime = 1f;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;


    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.1f;

    [SerializeField] Transform cameraPosition;
    
    //States
    public bool isGrounded { get; private set; }


    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    float horizotalMovement;
    float verticalMovement;

    Rigidbody rb;
    private float originalHeight; //player original height
    private float heightReduction = 0.3f;

    RaycastHit slopeHit;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
        else
        {
            this.tag = "LocalPlayer";
        }


        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        originalHeight = collider.height;
        collider.center = new Vector3(0, 0, 0);

    }


    private void Update()
    {

        if (!PV.IsMine)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //handleRotation(); 
        MyInput();
        ControlDrag();
        ControlSpeed();
        ControlFOV();
        ControlFall();

        //Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded || Input.GetKeyDown(jumpKey) && jumpsLeft > 0)
        {
           
            Jump();
            
        }
        if (isGrounded)
        {
            //this.anim.SetBool("jump", false);
            jumpsLeft = maxJumps - 1;
         
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        //fovController.setFovToSpeed;
    }



    void ControlFall()
    {
        if (!wallRunning.getIsWallRunning())
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetKeyDown(jumpKey))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
       
    }

    void Jump()
    {
        if (isGrounded || jumpsLeft > 0)
        {
            
            jumpsLeft = jumpsLeft - 1;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            this.anim.Play("Jump");
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
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


    void MyInput()
    {
        horizotalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizotalMovement;
        this.anim.SetFloat("vertical", verticalMovement);
        this.anim.SetFloat("horizontal", horizotalMovement);


    }

    /*
    void handleRotation()
    {
        Vector3 positionToLookAt;
        
        positionToLookAt.x = moveDirection.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = moveDirection.z;

        Quaternion currentRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

    }
    */

    void ControlSpeed()
    {
        
        moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
  
        
    }

    void ControlFOV() 
    {
        if (rb.velocity.magnitude == 0)
        {
            fovController.resetFov();
        }
        else
        {
            fovController.setRunFov();
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
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        //Movement
        MovePlayer();
        ControlDrag();
        MyInput();
     
       
      
        

        //Sliding

    }
}
