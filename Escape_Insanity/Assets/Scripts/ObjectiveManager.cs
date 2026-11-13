using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager I;
    public TMP_Text hudText;
    public int fusesNeeded = 3;
    public bool hasKeycard, cardReaderDone, fuseBoxDone;
    int fuses;

    void Awake() { I = this; Refresh(); }
    public void GotKeycard() { hasKeycard = true; Refresh(); }
    public void UseCardReader() { cardReaderDone = true; Refresh(); }
    public void GotFuse() { fuses++; Refresh(); }
    public void UseFuseBox() { fuseBoxDone = true; Refresh(); }
    public bool FusesComplete => fuses >= fusesNeeded;

    void Refresh()
    {
        if (!hasKeycard) hudText.text = "Find the keycard";
        else if (!cardReaderDone) hudText.text = "Use the card reader";
        else if (!FusesComplete) hudText.text = "Find the fuses (" + fuses + "/" + fusesNeeded + ")";
        else if (!fuseBoxDone) hudText.text = "Put the fuses in the fuse box";
        else hudText.text = "Escape through the exit door";
    }
}