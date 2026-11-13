using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Fades the main menu to black, then loads the next scene.
// Hook the Play button's On Click to MenuSceneFader.PlayGame().
public class MenuSceneFader : MonoBehaviour
{
    [SerializeField] CanvasGroup fadeGroup;           // full-screen black Image with a Canvas Group
    [SerializeField] string sceneToLoad = "IntroScene";
    [SerializeField] float fadeTime = 1f;

    bool loading;

    void Start()
    {
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;   // so the overlay never blocks the buttons
        }
    }

    public void PlayGame()
    {
        if (loading) return;                    // ignore double clicks
        loading = true;
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        if (fadeGroup != null)
        {
            fadeGroup.blocksRaycasts = true;    // block further clicks during the fade
            float t = 0f;
            while (t < fadeTime)
            {
                t += Time.unscaledDeltaTime;
                fadeGroup.alpha = Mathf.Clamp01(t / fadeTime);
                yield return null;
            }
            fadeGroup.alpha = 1f;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}