using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmGrabber : MonoBehaviour
{

    [SerializeField] PlayerController player;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Camera cam;


    Quaternion lastRotation;


    GameObject sphere;
    bool grabbing = false;


    Vector3 directionToPlayer;

    ITakeVelocity playerVelAccepter;

    private void Awake()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 0.1f;
        sphere.transform.position = Vector3.one * -100f;
        sphere.GetComponent<Renderer>().material.color = Color.black;


        player.gameObject.GetComponent<ITakeVelocity>();


        InputManager.Instance.Actions.Grab.started += GrabStart;
        InputManager.Instance.Actions.Grab.canceled += GrabStop;
    }

    private void OnDisable()
    {
        sphere.transform.position = Vector3.up * -100f;
    }

    private void GrabStop(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        grabbing = false;
        sphere.transform.position = Vector3.up * -100f;
        player.AcceptVel = false;

    }

    private void GrabStart(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var ray = cam.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0));

        bool doHit = Physics.Raycast(ray, out RaycastHit hit, 10f);

        if (doHit)
        {
            sphere.transform.position = hit.point;
            grabbing = true;
            player.AcceptVel = true;

            lastRotation = cameraPivot.rotation;

            directionToPlayer = (player.gameObject.transform.position - sphere.transform.position).normalized;
        }
           

    }

    private void Update()
    {

        if (grabbing)
        {





        }
        else
        {

        }


        lastRotation = cameraPivot.rotation;
    }




}
