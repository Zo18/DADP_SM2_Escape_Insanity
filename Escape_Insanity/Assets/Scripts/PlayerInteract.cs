using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    public float range = 3f;

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                Interactable i = hit.collider.GetComponentInParent<Interactable>();
                if (i != null) i.Interact();
            }
        }
    }
}