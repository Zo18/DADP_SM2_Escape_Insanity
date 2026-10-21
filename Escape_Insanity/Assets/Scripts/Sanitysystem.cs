using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity;
    public float drainRatePerSecond = 8f;
    public float regenRatePerSecond = 4f;

    // NOTE: Once Person A's FPController exists on this branch,
    // add this back in and swap IsSprintingNow() to use it:
    // public FPController playerMovement;

    [Header("Temporary Input (remove once real sprint exists)")]
    public InputAction tempSprintAction;

    [Header("Vision Feedback (optional, wire up later)")]
    public Image visionOverlay;

    private bool IsSprintingNow()
    {
        return tempSprintAction != null && tempSprintAction.IsPressed();
    }

    private void OnEnable()
    {
        tempSprintAction?.Enable();
    }

    private void OnDisable()
    {
        tempSprintAction?.Disable();
    }

    private void Start()
    {
        currentSanity = maxSanity;
    }

    private void Update()
    {
        if (IsSprintingNow())
        {
            currentSanity -= drainRatePerSecond * Time.deltaTime;
        }
        else
        {
            currentSanity += regenRatePerSecond * Time.deltaTime;
        }

        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        UpdateVisionFeedback();
    }

    private void UpdateVisionFeedback()
    {
        if (visionOverlay == null) return;

        float fearAmount = 1f - (currentSanity / maxSanity);
        Color c = visionOverlay.color;
        c.a = fearAmount * 0.6f;
        visionOverlay.color = c;
    }
}