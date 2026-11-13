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
    public FPController playerMovement;

    private bool hasLost;
    public bool HasLost => hasLost;

    [Header("Fallback Sprint Input")]
    public InputAction tempSprintAction;

    [Header("Vision Feedback (optional, wire up later)")]
    public Image visionOverlay;

    private bool IsSprintingNow()
    {
        if (playerMovement != null) return playerMovement.IsSprinting;
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
        if (hasLost) return;

        if (IsSprintingNow())
        {
            currentSanity -= drainRatePerSecond * Time.deltaTime;
        }
        else
        {
            currentSanity += regenRatePerSecond * Time.deltaTime;
        }

        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
        if (currentSanity <= 0f)
        {
            currentSanity = 0f;
            hasLost = true;
        }

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
