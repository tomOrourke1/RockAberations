using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] PlayerAnimator anim;



    [SerializeField] float rotSpeed = 20.0f;
    Vector3 internalDirection = Vector3.forward;


    public Vector3 Direction => internalDirection;


    private void LateUpdate()
    {

        // get current right based off of playerHeading
        Vector3 right = Vector3.Cross(player.Heading, Vector3.up);


        float dotright = Vector3.Dot(internalDirection, right);
        float dotForward = Vector3.Dot(internalDirection, player.Heading);

   //     Debug.Log("right val : " + dotright);

     //   Debug.DrawLine(transform.position, transform.position + internalDirection, Color.red);



        if(dotForward <= -0.3f)
        {
            internalDirection = player.Heading;
        }


        anim.SetDirection(GetDirectionValue(dotright, dotForward));

        internalDirection = Vector3.RotateTowards(internalDirection, player.Heading, Mathf.Deg2Rad * rotSpeed * Time.deltaTime, 0);




        transform.localRotation = Quaternion.LookRotation(player.Heading, Vector3.up);
    }

    public void ResetInternals()
    {
        internalDirection = player.Heading;
    }


    float GetDirectionValue(float right, float forward)
    {
        if(forward < 0)
        {
            return right >= 0 ? 1.0f : -1.0f;
        }
        else
        {
            return right;
        }

    }
}
