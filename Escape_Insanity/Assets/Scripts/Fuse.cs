using UnityEngine;

public class Fuse : Interactable
{
    public override void Interact()
    {
        if (!ObjectiveManager.I.cardReaderDone) return;
        ObjectiveManager.I.GotFuse();
        Destroy(gameObject);
    }
}