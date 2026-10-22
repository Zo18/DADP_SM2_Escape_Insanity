using UnityEngine;

// Put on each Paper Stack. Holds the story text shown when picked up.
public class FilePickup : MonoBehaviour
{
    [TextArea(3, 6)] public string noteText = "Patient 07 - Day 41. Dosage increased again...";

    public void PickUp()
    {
        GameManager.Instance.CollectFile(noteText);
        Destroy(gameObject);
    }
}