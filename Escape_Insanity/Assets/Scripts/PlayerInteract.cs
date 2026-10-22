using UnityEngine;
using UnityEngine.InputSystem;

// Looks where the camera points; press E to use whatever is there.
public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    public float reach = 3f;
    public float aimRadius = 0.3f; // bigger = easier to target

    void Update()
    {
        FilePickup target = null;
        if (Physics.SphereCast(cam.transform.position, aimRadius, cam.transform.forward, out RaycastHit hit, reach))
            target = hit.collider.GetComponentInParent<FilePickup>();

        GameManager.Instance.ShowPrompt(target != null);

        if (target != null && Keyboard.current.eKey.wasPressedThisFrame)
            target.PickUp();
    }
}