using UnityEngine;

public class CardReader : Interactable
{
    public override void Interact()
    {
        if (ObjectiveManager.I.hasKeycard && !ObjectiveManager.I.cardReaderDone)
            ObjectiveManager.I.UseCardReader();
    }
}