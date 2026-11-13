using UnityEngine;
using UnityEngine.InputSystem;

// Combined version: works with Zoe's file pickups AND Lilly's Interactables
// (fuses, fuse box, keycard, card reader, exit door).
// Look at something and press E (or X / Square on a controller) to use it.
public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    public float reach = 3f;
    public float aimRadius = 0.3f; // bigger = easier to target

    void Update()
    {
        FilePickup file = null;
        Interactable thing = null;

        if (Physics.SphereCast(cam.transform.position, aimRadius, cam.transform.forward, out RaycastHit hit, reach))
        {
            file = hit.collider.GetComponentInParent<FilePickup>();
            thing = hit.collider.GetComponentInParent<Interactable>();
        }

        bool hasTarget = file != null || thing != null;
        if (GameManager.Instance != null) GameManager.Instance.ShowPrompt(hasTarget);

        bool pressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                    || (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame);

        if (!pressed) return;
        if (file != null) file.PickUp();
        else if (thing != null) thing.Interact();
    }
}