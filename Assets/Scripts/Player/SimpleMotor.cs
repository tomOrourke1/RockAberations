using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMotor : MonoBehaviour
{
    int maxBounces = 5;
    float skinWidth = 0.015f;
    float maxSlopeAngle = 55;

    Bounds bounds;
    [SerializeField] SphereCollider collider;

    bool isGrounded = false;
    [SerializeField]
    LayerMask layerMask;

    [Space]
    [SerializeField] Vector3 colliderOffset;


    [SerializeField]
    Vector3 upNormal = Vector3.up;

    Vector3 point1;
    Vector3 point2;


    Vector3 lastHitNormal = Vector3.zero;

    RaycastHit lastHit;

    public Vector3 Normal => lastHitNormal;
    public Vector3 UpDir => upNormal;


    float Radius => collider.bounds.extents.x;
    float BRadius => bounds.extents.x;

    public bool Grounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }

    public RaycastHit LastHit => lastHit;

    

    private void Update()
    {
        bounds = collider.bounds;
        bounds.Expand(-2 * skinWidth);
    }

    private void OnDrawGizmos()
    {
        //    Gizmos.DrawWireSphere(point1, collider.bounds.extents.x);
        //    Gizmos.DrawWireSphere(point2, collider.bounds.extents.x);


        //     Gizmos.color = Color.red;
        //    Gizmos.DrawLine(lastHit.point, lastHit.point + lastHitNormal);
    }


    public Vector3 Move(Vector3 velocity, float deltaTime)
    {
        Vector3 vel = velocity * deltaTime;
        Vector3 delta = CollideAndSlideMK3(vel, transform.position, 0, vel);
        transform.position += delta;

        velocity = ProjectAndScale(velocity, lastHitNormal);

        return velocity;
    }




    private Vector3 CollideAndSlideMK3(Vector3 vel, Vector3 pos, int depth, Vector3 velInit)
    {
        if (depth >= maxBounces)
        {
            return Vector3.zero;
        }

        float dist = vel.magnitude + skinWidth;
        RaycastHit hit;

        bool doHit = Physics.SphereCast(pos, BRadius, vel.normalized, out hit, dist, layerMask);
        //bool doHit = Physics.CapsuleCast(point1, point2, BRadius, vel.normalized, out hit, dist, layerMask);

        // bool doHit = Physics.SphereCast(pos + colliderOffset, bounds.extents.x, vel.normalized, out hit, dist, layerMask);

        if (doHit)
        {
            lastHitNormal = hit.normal;
            lastHit = hit;


            Vector3 snapToSurface = vel.normalized * (hit.distance - skinWidth);
            Vector3 leftover = vel - snapToSurface;

            // needs to check opositite of down against the normal
            float angle = Vector3.Angle(upNormal, hit.normal);

            if (snapToSurface.magnitude <= skinWidth)
            {
                snapToSurface = Vector3.zero;
            }

            //normal gound / slope
            if (angle <= maxSlopeAngle)
            {
                leftover = ProjectAndScale(leftover, hit.normal);
            }
            // wall or steep slope
            else
            {
                // this used to have removed the y axis
                float scale = 1 - Vector3.Dot(
                    hit.normal.normalized/*new Vector3(hit.normal.x, 0, hit.normal.z).normalized*/,
                    -velInit.normalized/*-new Vector3(velInit.x, 0, velInit.z).normalized*/
                    );


                if (isGrounded)
                {
                    // this also used to have y of 0
                    leftover = ProjectAndScale(
                        leftover/*new Vector3(leftover.x, 0, leftover.z)*/,
                        hit.normal /*new Vector3(hit.normal.x, 0, hit.normal.z)*/
                        );
                    leftover *= scale;
                }
                else
                {
                    leftover = ProjectAndScale(leftover, hit.normal) * scale;
                }
            }

            return snapToSurface + CollideAndSlideMK3(leftover, pos + snapToSurface, depth + 1, velInit);

        }
        else // doesn't hit anything
        {
            lastHit = default;
            isGrounded = false;
            lastHitNormal = Vector3.zero;

        }

        return vel;
    }



    public void CheckGrounded()
    {
        RaycastHit hit;
        bool hihi = Physics.Raycast(point1, Vector3.down, out hit, Radius + skinWidth);
        //bool doHit = Physics.CapsuleCast(point1, point2, BRadius, Vector3.down, out hit, 0.0001f, layerMask);
        isGrounded = hihi;
    }

    Vector3 ProjectAndScale(Vector3 p1, Vector3 p2)
    {
        float mag = p1.magnitude;
        p1 = Vector3.ProjectOnPlane(p1, p2).normalized;
        return (p1 * mag);
    }


    /// <summary>
    /// set the gravity direction
    /// will normalize internally
    /// </summary>
    /// <param name="gravNormal"></param>
    public void SetUpDirection(Vector3 gravNormal)
    {
        upNormal = gravNormal.normalized;
    }





}
