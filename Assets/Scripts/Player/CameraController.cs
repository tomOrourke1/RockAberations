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





        Vector3 eulerAngles = new Vector3(pitchAngle, yawAgnle, 0);





        cameraOffset.localRotation = Quaternion.Euler(eulerAngles);
    }



}
