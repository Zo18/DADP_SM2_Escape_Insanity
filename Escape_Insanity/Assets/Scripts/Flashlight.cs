using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;          // drag the Spot Light here
    public AudioSource clickSound;    // optional, leave empty for now
    public bool startsOn = false;

    void Start()
    {
        flashlight.enabled = startsOn;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            flashlight.enabled = !flashlight.enabled;
            if (clickSound != null) clickSound.Play();
        }
    }
}