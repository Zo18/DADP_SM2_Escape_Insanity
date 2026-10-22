using UnityEngine;

// Invisible trigger at the exit. Only lets you win once all files are found.
public class ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.AllFilesFound)
            GameManager.Instance.Win();
    }
}