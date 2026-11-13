using UnityEngine;

// Works once all fuses have been collected.
public class FuseBox : Interactable
{
    public override void Interact()
    {
        if (ObjectiveManager.I.FusesComplete && !ObjectiveManager.I.fuseBoxDone)
            ObjectiveManager.I.UseFuseBox();
    }
}