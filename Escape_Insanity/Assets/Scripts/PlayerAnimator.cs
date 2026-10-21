using UnityEngine;

// Plays the walk animation only while the player is moving.
// While hiding, it plays the idle animation instead.
public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;                // on PC_Patient
    public float walkSpeed = 5f;             // match FPController's Move Speed
    public bool resetToStandingPose = true;  // on = neutral pose, off = freeze mid-step

    Vector3 lastPos;
    bool wasMoving;
    bool isHiding;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        if (isHiding) return;

        Vector3 delta = transform.position - lastPos;
        delta.y = 0f;                                   // ignore falling
        float speed = delta.magnitude / Time.deltaTime; // real speed this frame
        lastPos = transform.position;

        bool moving = speed > 0.1f;

        if (moving)
            animator.speed = speed / walkSpeed;
        else
        {
            animator.speed = 0f;
            if (resetToStandingPose && wasMoving)
                animator.Play("PC_Walk", 0, 0f);
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