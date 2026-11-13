using UnityEngine;

public class Fuse : Interactable
{
    public override void Interact()
    {
        ObjectiveManager.I.GotFuse();
        Destroy(gameObject);
    }
}