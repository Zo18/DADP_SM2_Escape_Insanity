using UnityEngine;
using TMPro;

// Escape objective: 3 fuses -> fuse box -> keycard -> card reader -> exit opens.
// Creates itself if missing. If Hud Text is empty, uses the GameManager's objective text.
public class ObjectiveManager : MonoBehaviour
{
    static ObjectiveManager instance;
    public static ObjectiveManager I
    {
        get
        {
            if (instance == null) instance = FindFirstObjectByType<ObjectiveManager>();
            if (instance == null) instance = new GameObject("ObjectiveManager").AddComponent<ObjectiveManager>();
            return instance;
        }
    }

    public TMP_Text hudText;
    public GameObject exitBarrier;   // the black block at the exit; hidden when the card reader is used
    public int fusesNeeded = 3;
    public bool hasKeycard, cardReaderDone, fuseBoxDone;
    int fuses;

    void Awake() { instance = this; }
    void Start() { Refresh(); }

    public void GotFuse() { fuses++; Refresh(); }
    public void UseFuseBox() { fuseBoxDone = true; Refresh(); }
    public void GotKeycard() { hasKeycard = true; Refresh(); }
    public void UseCardReader()
    {
        cardReaderDone = true;
        if (exitBarrier) exitBarrier.SetActive(false);
        Refresh();
    }
    public bool FusesComplete => fuses >= fusesNeeded;

    public void Refresh()
    {
        TMP_Text t = hudText;
        if (t == null && GameManager.Instance != null) t = GameManager.Instance.objectiveText;
        if (t == null) return;

        if (!FusesComplete) t.text = "Find the fuses (" + fuses + "/" + fusesNeeded + ")";
        else if (!fuseBoxDone) t.text = "Put the fuses in the fuse box";
        else if (!hasKeycard) t.text = "Find the keycard";
        else if (!cardReaderDone) t.text = "Use the keycard on the card reader";
        else t.text = "The exit is open - escape!";
    }
}