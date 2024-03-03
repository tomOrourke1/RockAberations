using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerSplineController : MonoBehaviour
{

    [SerializeField] SplineContainer spline;
    [SerializeField] PlayerArmGrabber arm;
    [SerializeField] Material armMat;

    [SerializeField] int controlNodeIndex = 2;

    MeshRenderer r;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = new Mesh();
        r = GetComponent<MeshRenderer>();
        r.material = armMat;
    }






    private void LateUpdate()
    {
        if(arm.Grabing)
        {
            r.enabled = true;
            spline.enabled = true;

            var s = spline[0][controlNodeIndex];

            var dir = arm.SpherePos - transform.position;


            float angle = Vector3.SignedAngle(gameObject.transform.forward, Vector3.forward, Vector3.up);

            dir = Quaternion.AngleAxis(angle, Vector3.up) * (dir);

            s.Position = dir;

            spline[0].SetKnot(controlNodeIndex, s);            
        }
        else
        {
            spline.enabled = false;
            r.enabled = false;

        }
    }


}
