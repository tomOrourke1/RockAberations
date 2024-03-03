using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerArmGrabber : MonoBehaviour
{

    [SerializeField] PlayerController player;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Camera cam;


    [SerializeField] LayerMask layermask;


    Quaternion lastRotation;


    GameObject sphere;
    bool grabbing = false;


    Vector3 directionToPlayer;

    ITakeVelocity playerVelAccepter;


    public bool Grabing => grabbing;
    public Vector3 SpherePos => sphere.transform.position;


    public event Action OnGrabStart;
    public event Action OnGrabStop;


    private void Awake()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 0.1f;
        sphere.transform.position = Vector3.one * -100f;
        sphere.GetComponent<Renderer>().material.color = Color.black;


        playerVelAccepter = player.gameObject.GetComponent<ITakeVelocity>();


    }
    private void Start()
    {

        InputManager.Instance.Actions.Grab.started += GrabStart;
        InputManager.Instance.Actions.Grab.canceled += GrabStop;
    }



    private void GrabStop(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnGrabStop?.Invoke();

        grabbing = false;
        sphere.transform.position = Vector3.up * -100f;
        player.AcceptVel = false;

    }

    private void GrabStart(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        bool doHit = Physics.Raycast(ray, out RaycastHit hit, 10f, layermask);

        if (doHit)
        {
            OnGrabStart?.Invoke();

            sphere.transform.position = hit.point;
            grabbing = true;
            player.AcceptVel = true;

            lastRotation = cameraPivot.rotation;

            directionToPlayer = (player.transform.position - sphere.transform.position);
        }
           

    }

    private void Update()
    {

        if (grabbing)
        {
            var rot = cameraPivot.rotation;

            var agnleDelta = Quaternion.Angle(rot, lastRotation);

            Vector3 vel = Vector3.zero;


            var right = Vector3.Cross(cam.transform.forward, directionToPlayer.normalized);

            Debug.DrawLine(player.transform.position, player.transform.position + right, Color.red);

            directionToPlayer = Quaternion.AngleAxis(agnleDelta, right) * directionToPlayer;


            var toPoint = directionToPlayer + sphere.transform.position;

            Debug.DrawLine(sphere.transform.position, toPoint);

            vel = toPoint - player.transform.position;

          //  playerVelAccepter.TakeVelocity(vel);
            playerVelAccepter.TakePosition(toPoint);
        }
        else
        {

        }


        lastRotation = cameraPivot.rotation;
    }




}
