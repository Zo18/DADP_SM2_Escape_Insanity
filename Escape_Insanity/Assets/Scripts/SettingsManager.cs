using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public string menuScene = "MainMenu";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        SetVolume(volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;          // controls every sound in the game
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void OnBackPressed()
    {
        SceneManager.LoadScene(menuScene);
    }
}