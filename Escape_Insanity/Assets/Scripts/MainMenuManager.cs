using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene names")]
    public string gameScene = "LevelDesign";
    public string settingsScene = "Settings";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;   // so you can click the buttons
        Cursor.visible = true;
    }

    public void OnNewGamePressed()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OnSettingsPressed()
    {
        SceneManager.LoadScene(settingsScene);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}