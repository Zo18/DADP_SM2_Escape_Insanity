using UnityEngine;
using UnityEngine.InputSystem;

public class LeanPeek : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the player's camera Transform here (the same one used for mouse look).")]
    public Transform cameraTransform;

    [Header("Lean Settings")]
    public float leanAngle = 15f;   // how far the camera tilts (roll) when leaning
    public float leanOffset = 0.5f; // how far sideways the camera shifts when leaning
    public float leanSpeed = 8f;    // how quickly the lean blends in/out

    [Header("Temporary Input (swap to shared Input Actions asset later)")]
    public InputAction leanLeftAction;  // bind to Q for now
    public InputAction leanRightAction; // bind to E for now

    private float currentLean = 0f; // -1 = full left, 1 = full right
    private float targetLean = 0f;

    private void OnEnable()
    {
        leanLeftAction?.Enable();
        leanRightAction?.Enable();
    }

    private void OnDisable()
    {
        leanLeftAction?.Disable();
        leanRightAction?.Disable();
    }

    private void Update()
    {
        bool leaningLeft = leanLeftAction != null && leanLeftAction.IsPressed();
        bool leaningRight = leanRightAction != null && leanRightAction.IsPressed();

        if (leaningLeft && !leaningRight)
            targetLean = -1f;
        else if (leaningRight && !leaningLeft)
            targetLean = 1f;
        else
            targetLean = 0f;

        currentLean = Mathf.Lerp(currentLean, targetLean, leanSpeed * Time.deltaTime);
    }

    // LateUpdate runs AFTER the look script's Update, so this applies
    // on top of whatever look rotation was set that frame, instead of
    // fighting over who writes to cameraTransform last.
    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 pos = cameraTransform.localPosition;
        pos.x = currentLean * leanOffset;
        cameraTransform.localPosition = pos;

        Quaternion tilt = Quaternion.Euler(0f, 0f, -currentLean * leanAngle);
        cameraTransform.localRotation = cameraTransform.localRotation * tilt;
    }
}