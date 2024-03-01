using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;


    [Header("Dependenceis")]
    [SerializeField] Camera cam;

    GameActions input;




    public GameActions.PlayerActions Actions => input.Player;

    public Vector3 PlannarInput
    {
        get
        {
            var ff = cam.transform.forward;
            var rr = cam.transform.right;
            ff.y = 0;
            rr.y = 0;
            ff.Normalize();
            rr.Normalize();

            var inp = input.Player.MoveAxis.ReadValue<Vector2>();


            return Vector3.ClampMagnitude(inp.x * rr + inp.y * ff, 1.0f);
        }
    }





    private void Awake()
    {
        Instance = this;

        input = new GameActions();
        input.Enable();


    }




}
