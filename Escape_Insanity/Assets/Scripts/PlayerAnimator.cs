using UnityEngine;

// Plays the walk animation only while the player is moving.
// When standing still it either freezes mid-step, or resets to a neutral pose.
// While hiding, it plays the idle animation instead.
public class PlayerAnimator : MonoBehaviour
{
    public CharacterController controller;   // on Player
    public Animator animator;                // on PC_Patient
    public float walkSpeed = 5f;             // match FPController's Move Speed
    public bool resetToStandingPose = true;  // on = neutral pose, off = freeze mid-step

    bool wasMoving;
    bool isHiding;

    void Update()
    {
        if (isHiding) return;                  // while hiding, Idle plays and walking logic is off

        Vector3 velocity = controller.velocity;
        velocity.y = 0f;                       // ignore falling and jumping
        float speed = velocity.magnitude;
        bool moving = speed > 0.1f;

        if (moving)
        {
            animator.speed = speed / walkSpeed;   // faster when sprinting
        }
        else
        {
            animator.speed = 0f;                  // pause the animation

            if (resetToStandingPose && wasMoving)
                animator.Play("PC_Walk", 0, 0f);  // back to the first frame
        }

        wasMoving = moving;
    }

    // Called by the hiding system when the player enters or leaves a hiding spot
    public void SetHiding(bool hiding)
    {
        isHiding = hiding;
        animator.speed = 1f;
        animator.Play(hiding ? "PC_Idle" : "PC_Walk", 0, 0f);
    }
}