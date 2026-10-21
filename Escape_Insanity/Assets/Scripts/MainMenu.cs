using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Play clicked");
        SceneManager.LoadScene("LevelDesign");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}