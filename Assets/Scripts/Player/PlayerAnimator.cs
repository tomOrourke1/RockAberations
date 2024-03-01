using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;


    static string Speed = "Speed";
    static string Dir = "Dir";
    static string Jump = "Jump";
    static string Grounded = "Grounded";
    static string Sliding = "Sliding";
    static string Dive = "Dive";







    public void SetMovement(float speed)
    {
        speed = Mathf.Clamp01(speed);

        animator.SetFloat(Speed, speed);
    }
    public void SetDirection(float direct)
    {
        direct = Mathf.Clamp(direct, -1, 1);

        animator.SetFloat(Dir, direct);
    }


    public void SetGrounded(bool ground)
    {
        animator.SetBool(Grounded, ground);
    }
    public void SetSliding(bool slide)
    {
        animator.SetBool(Sliding, slide);
    }


    public void OnJump()
    {
        animator.SetTrigger(Jump);
    }

    public void OnDive()
    {
        animator.SetTrigger(Dive);
    }



}
