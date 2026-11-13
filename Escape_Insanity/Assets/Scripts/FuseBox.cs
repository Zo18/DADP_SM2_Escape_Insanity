using UnityEngine;

public class FuseBox : Interactable
{
    public override void Interact()
    {
        if (ObjectiveManager.I.cardReaderDone && ObjectiveManager.I.FusesComplete && !ObjectiveManager.I.fuseBoxDone)
            ObjectiveManager.I.UseFuseBox();
    }
}