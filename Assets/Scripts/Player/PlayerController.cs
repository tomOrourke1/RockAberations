using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerAnimator anim;
    [SerializeField] ModelRotator rotator;

    [Header("Movement Values")]
    [SerializeField] float speed;
    [SerializeField] float jumpValue;
    [SerializeField] float gravityAmount;
    [SerializeField] float accel = 10f;





    [Header("Collision Values")]
    [SerializeField] float maxGroundAngle = 45f;


    // Collision Info
    int contactCount;
    int groundCount;
    int steepCount;
    Vector3 groundNormal;
    Vector3 steepNormal;
    float maxGroundDot;


    // properties
    bool Grounded => groundCount > 0;
    bool Steep => steepCount > 0;

    Vector3 GroundNormal => groundNormal.normalized;
    Vector3 SteepNormal => steepNormal.normalized;


    Vector3 lastHeading = Vector3.forward;

    public Vector3 Heading => lastHeading;

    private void OnValidate()
    {
        maxGroundDot = Mathf.Acos(maxGroundAngle);
    }

    private void Awake()
    {
        maxGroundDot = Mathf.Acos(Mathf.Deg2Rad * maxGroundAngle);
    }



    private void FixedUpdate()
    {
        PreMove();
        Move();
        PostMove();
    }



    void PreMove()
    {
        // determine movement conditions / state


    }

    void Move()
    {
        // move given those conditions
        var inp = InputManager.Instance.PlannarInput;

        var vel = rb.velocity;
        float ySpeed = vel.y;
        vel.y = 0;



        var desVel = inp * speed;


        vel = Vector3.MoveTowards(vel, desVel, Time.deltaTime * accel);

        if (Grounded)
        {
            if(InputManager.Instance.Actions.Jump.WasPressedThisFrame())
            {
                ySpeed += jumpValue;
                anim.OnJump();
            }


            anim.SetMovement(vel.magnitude / speed);

        }
        else
        {
            ySpeed -= Time.deltaTime * gravityAmount;

        }

        anim.SetGrounded(Grounded);

        rb.velocity = (vel) + (Vector3.up * ySpeed);


        lastHeading = vel.normalized.magnitude > 0 ? vel.normalized : lastHeading;

        if (!(vel.normalized.magnitude > 0))
        {
            rotator.ResetInternals();
        }

        Debug.DrawLine(transform.position, transform.position + lastHeading, Color.yellow);


    }
    void PostMove()
    {
        // clear conditions
        contactCount = groundCount = steepCount = 0;
        groundNormal = steepNormal = Vector3.zero;
    }



    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }



    void EvaluateCollision(Collision coll)
    {
        foreach (var c in coll.contacts)
        {
            if (Vector3.Dot(c.normal, Vector3.up) >= maxGroundDot)
            {
                groundNormal += c.normal;
                groundCount++;
            }
            else
            {
                steepNormal += c.normal;
                steepCount++;
            }

            contactCount++;
        }
    }


}
