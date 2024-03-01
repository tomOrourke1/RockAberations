using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] Rigidbody rb;


    [Header("Movement Values")]
    [SerializeField] float speed;






    [Header("Collision Values")]
    [SerializeField] float maxGroundAngle = 45f;


    // Collision Info
    int contactCount;
    Vector3 groundNormal;
    Vector3 steepNormal;
    bool isGrounded;
    float maxGroundDot;




    private void OnValidate()
    {
        maxGroundDot = Mathf.Acos(maxGroundAngle);
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





        rb.velocity = (inp * speed) + (Vector3.up * ySpeed);
    }
    void PostMove()
    {
        // clear conditions
        contactCount = 0;
        groundNormal = steepNormal = Vector3.zero;
        isGrounded = false;
    }



    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }



    void EvaluateCollision(Collision coll)
    {
        foreach (var c in coll.contacts)
        {
            
        }
    }


}
