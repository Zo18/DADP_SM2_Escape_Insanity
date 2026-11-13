using UnityEngine;

public class ExitDoor : Interactable
{
    public GameObject winPanel;

    public override void Interact()
    {
        if (!ObjectiveManager.I.fuseBoxDone) return;
        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}