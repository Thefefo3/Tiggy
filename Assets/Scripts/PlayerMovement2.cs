using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[AddComponentMenu("Character/Controller")]
public class PlayerMovement2 : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float runSpeed = 10f;

    public float gravity = 13f;

    public float jumpHeight = 2f;
    public int maxJumps = 2;

    public float airAccelerator = 0.15f;
    public float groundAccelerator = 0.6f;
    public bool controllerAble = true;

    float speed;
    float acceleration;
    int jumpsDone = 0;
    float jumpedYPos;
    float landedYPos;
    bool lastGrounded;
    float nextFootstep;
    bool moving;
    bool onSlippyMat;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerAble)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (IsGrounded())
                {
                    jumpsDone = 0;
                    jumpedYPos = transform.position.y;
                    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, CalculateJumpVerticalSpeed(), GetComponent<Rigidbody>().velocity.z);
                }
                else
                {
                    if (jumpsDone < maxJumps - 1)
                    {
                        jumpsDone++;
                        jumpedYPos = transform.position.y;
                        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, CalculateJumpVerticalSpeed(), GetComponent<Rigidbody>().velocity.z);
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();

        if (Input.GetButton("Run"))
        {
            speed = runSpeed;

            capsule.height = Mathf.Lerp(capsule.height, 2f, Time.deltaTime * 10f);
        }
        else
        {
            speed = walkSpeed;
        }

        moving = Input.GetButton("Horizontal") || Input.GetButton("Vertical");

        if (IsGrounded()) 
        {
            if (controllerAble == true)
            {
                acceleration = groundAccelerator;
            }
        }
        else
        {
            if (controllerAble == true)
            {
                acceleration = airAccelerator;
            }
        }

        Vector3 targetVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity.normalized) * speed;
        if (controllerAble == false)
        {
            targetVelocity = Vector3.zero;
        }
        else
        {
            //Limit the velocity if its higher than usual, this happens when walking diagonally.
            if (targetVelocity.sqrMagnitude > (speed * speed))
            {
                targetVelocity = targetVelocity.normalized * speed;
            }
        }

        Vector3 velocityChange = (targetVelocity - GetComponent<Rigidbody>().velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -acceleration, acceleration);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -acceleration, acceleration);
        velocityChange.y = 0;

        GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));


    }


    public bool IsGrounded()
    {
        //All done for you perfectly, you can resize your player and you will still be able to use it.
        float castRadius = transform.localScale.x / 2 - 0.1f;
        float castDistance = transform.localScale.x + 0.1f;

        //Casts 5 raycasts from different parts of the player, for accuracy.
        Vector3 leftCast = new Vector3(transform.position.x - castRadius, transform.position.y, transform.position.z);
        Vector3 rightCast = new Vector3(transform.position.x + castRadius, transform.position.y, transform.position.z);
        Vector3 frontCast = new Vector3(transform.position.x, transform.position.y, transform.position.z + castRadius);
        Vector3 backCast = new Vector3(transform.position.x, transform.position.y, transform.position.z - castRadius);
        Vector3 centerCast = transform.position;

        return Physics.Raycast(leftCast, -transform.up, castDistance) || Physics.Raycast(rightCast, -transform.up, castDistance) ||
            Physics.Raycast(frontCast, -transform.up, castDistance) || Physics.Raycast(backCast, -transform.up, castDistance) ||
                Physics.Raycast(centerCast, -transform.up, castDistance);
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}
