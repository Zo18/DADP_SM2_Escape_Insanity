using UnityEngine;

// Needs the keycard AND power (fuse box done). Opens the exit.
public class CardReader : Interactable
{
    public override void Interact()
    {
        if (ObjectiveManager.I.hasKeycard && ObjectiveManager.I.fuseBoxDone && !ObjectiveManager.I.cardReaderDone)
            ObjectiveManager.I.UseCardReader();
    }
}