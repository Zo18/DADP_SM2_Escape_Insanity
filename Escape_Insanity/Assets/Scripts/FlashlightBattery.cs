using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightBattery : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the flashlight's Light component here (should be a Spot Light).")]
    public Light flashlight;

    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float currentBattery;
    public float drainRatePerSecond = 5f; // only drains while flashlight is ON

    [Header("Low Battery Feedback")]
    public float lowBatteryThreshold = 20f; // below this, flicker starts
    public float flickerIntensityMin = 0.3f;
    public float flickerSpeed = 8f;

    [Header("Temporary Input (swap to shared Input Actions asset later)")]
    public InputAction toggleFlashlightAction; // bind to F for now

    private bool isOn = false;
    private float baseIntensity;

    private void OnEnable()
    {
        toggleFlashlightAction?.Enable();
        if (toggleFlashlightAction != null)
            toggleFlashlightAction.performed += OnTogglePerformed;
    }

    private void OnDisable()
    {
        if (toggleFlashlightAction != null)
            toggleFlashlightAction.performed -= OnTogglePerformed;
        toggleFlashlightAction?.Disable();
    }

    private void Start()
    {
        currentBattery = maxBattery;
        if (flashlight != null)
        {
            baseIntensity = flashlight.intensity;
            flashlight.enabled = false;
        }
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        // Don't allow turning on a dead battery
        if (!isOn && currentBattery <= 0f) return;

        isOn = !isOn;
        if (flashlight != null)
            flashlight.enabled = isOn;
    }

    private void Update()
    {
        if (flashlight == null) return;

        if (isOn)
        {
            currentBattery -= drainRatePerSecond * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

            if (currentBattery <= 0f)
            {
                // Battery died mid-use, force it off
                isOn = false;
                flashlight.enabled = false;
                return;
            }

            if (currentBattery <= lowBatteryThreshold)
            {
                // Flicker effect: intensity wobbles as battery gets low
                float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
                flashlight.intensity = Mathf.Lerp(flickerIntensityMin, baseIntensity, flicker);
            }
            else
            {
                flashlight.intensity = baseIntensity;
            }
        }
    }

    // Call this from a future battery pickup script, e.g. batteryScript.AddBattery(30f)
    public void AddBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0f, maxBattery);
    }
}