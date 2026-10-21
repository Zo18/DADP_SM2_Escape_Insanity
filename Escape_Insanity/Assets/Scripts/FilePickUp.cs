using UnityEngine;

public class FilePickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.I.Collect();
        Destroy(gameObject);
    }
}