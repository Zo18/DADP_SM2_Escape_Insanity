using UnityEngine;
 
// Invisible trigger behind the exit barrier. Walking into it wins once the exit is open.
public class ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ObjectiveManager.I.cardReaderDone)
            GameManager.Instance.Win();
    }
}
 