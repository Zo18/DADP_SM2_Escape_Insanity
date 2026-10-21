using UnityEngine;

public class ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.I.AllFound) GameManager.I.Show("You escaped.");
        else GameManager.I.Show("I can't leave yet... find the files.");
    }
}