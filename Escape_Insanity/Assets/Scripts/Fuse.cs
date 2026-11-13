using UnityEngine;

// Can be picked up at any time, in any order.
public class Fuse : Interactable
{
    public override void Interact()
    {
        ObjectiveManager.I.GotFuse();
        Destroy(gameObject);
    }
}