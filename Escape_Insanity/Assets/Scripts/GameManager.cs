using UnityEngine;
using TMPro;

// Tracks files, updates the HUD, shows notes, and opens the exit.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int filesNeeded = 3;
    public TMP_Text fileCounter;     // "Files 0/3"
    public TMP_Text objectiveText;   // what to do next
    public GameObject interactPrompt;// "Press E"
    public GameObject notePanel;     // panel with the note
    public TMP_Text noteText;
    public GameObject winPanel;

    int files;
    public bool AllFilesFound => files >= filesNeeded;

    void Awake() { Instance = this; }

    void Start()
    {
        notePanel.SetActive(false);
        winPanel.SetActive(false);
        interactPrompt.SetActive(false);
        UpdateHUD();
    }

    void Update()
    {
        // close the note with E or Escape
        if (notePanel.activeSelf && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
            notePanel.SetActive(false);
    }

    public void ShowPrompt(bool show) { interactPrompt.SetActive(show && !notePanel.activeSelf); }

    public void CollectFile(string text)
    {
        files++;
        noteText.text = text;
        notePanel.SetActive(true);
        CancelInvoke(nameof(HideNote));
        Invoke(nameof(HideNote), 5f);   // note disappears after 5 seconds
        UpdateHUD();
    }

    void HideNote() { notePanel.SetActive(false); }

    void UpdateHUD()
    {
        fileCounter.text = "Files " + files + "/" + filesNeeded;
        objectiveText.text = AllFilesFound
            ? "Get back to reception and escape"
            : "Find the patient files";
    }

    public void Win()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}