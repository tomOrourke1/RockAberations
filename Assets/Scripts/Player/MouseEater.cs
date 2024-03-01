using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEater : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
