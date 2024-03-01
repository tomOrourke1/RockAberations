using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cameraOffset;


    [Header("Camera Values")]
    [SerializeField] float camSpeed;



    float yawAgnle;
    float pitchAngle;


    private void LateUpdate()
    {
        var inp = InputManager.Instance.Actions.MouseDelta.ReadValue<Vector2>();


        pitchAngle -= inp.y * camSpeed * Time.deltaTime;
        yawAgnle += inp.x * camSpeed * Time.deltaTime;

        pitchAngle = Mathf.Clamp(pitchAngle, -80, 80);

        yawAgnle = yawAgnle >= 360 ? yawAgnle - 360 : yawAgnle < 0 ? yawAgnle + 360 : yawAgnle;


        Vector3 eulerAngles = new Vector3(pitchAngle, yawAgnle, 0);

        cameraOffset.localRotation = Quaternion.Euler(eulerAngles);
    }



}
