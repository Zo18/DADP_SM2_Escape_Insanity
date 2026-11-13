using UnityEngine;

public class KeyCard : Interactable
{
    public override void Interact()
    {
        ObjectiveManager.I.GotKeycard();
        Destroy(gameObject);
    }
}