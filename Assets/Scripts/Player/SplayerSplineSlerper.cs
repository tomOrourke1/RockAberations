using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplayerSplineSlerper : MonoBehaviour
{
    [SerializeField] SplineExtrude splineEx;
    [SerializeField] PlayerArmGrabber armGrabber;


    [SerializeField] float lickSpeedOut;
    [SerializeField] float lickSpeedIn;
    float speed;

    bool destination;
    float direction;


    bool licking = false;
    float tonguePoint;


    MeshRenderer r;


    private void Awake()
    {
        r = GetComponent<MeshRenderer>();

        armGrabber.OnGrabStart += StartLick;
        armGrabber.OnGrabStop += StopLick;
    }


    private void OnDisable()
    {
        armGrabber.OnGrabStart -= StartLick;
        armGrabber.OnGrabStop -= StopLick;

    }

    private void LateUpdate()
    {
        if(licking)
        {
            tonguePoint = tonguePoint + (direction * speed * Time.deltaTime);

            tonguePoint = Mathf.Clamp01(tonguePoint);

            splineEx.Range = new Vector2(0,tonguePoint);


            if (EvaluateEndPoint(tonguePoint, destination, out Vector2 newPoint))
            {
                licking = false;
                splineEx.Range = newPoint;

                if(newPoint.y == 0)
                {
                    r.enabled = false;
                }
            }
        }
    }



    public void StartLick()
    {
        licking = false;
        tonguePoint = 1;
        direction = 1.0f;
        destination = true;

        splineEx.Range = new Vector2(0, tonguePoint);


        speed = lickSpeedOut;
    }


    public void StopLick()
    {
        licking = true;
        tonguePoint = 1;
        direction = -1.0f;
        destination = false;

        speed = lickSpeedIn;
    }


    bool EvaluateEndPoint(float tonguePoint, bool destination, out Vector2 newPoint)
    {
        if (destination)
        {
            newPoint.x = 0;
            newPoint.y = 1;
            return tonguePoint >= 1;
        }
        else
        {
            newPoint.x = 0;
            newPoint.y = 0;
            return tonguePoint <= 0;
        }
    }

    
}
