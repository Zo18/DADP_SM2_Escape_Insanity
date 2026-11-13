using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    [System.Serializable]
    public class Beat
    {
        [TextArea(2, 4)] public string text;
        public float holdTime = 2f;     // seconds to wait after the text finishes typing
        public Sprite image;            // optional: changes the background for this line
        public AudioClip voice;         // optional: nurse voice line
    }

    [Header("Scene References")]
    [SerializeField] TMP_Text lineText;
    [SerializeField] Image backgroundImage;      // optional
    [SerializeField] CanvasGroup fadeGroup;      // full-screen black Image with a CanvasGroup
    [SerializeField] AudioSource voiceSource;    // optional

    [Header("Settings")]
    [SerializeField] string nextSceneName = "GameScene";
    [SerializeField] float typeDelay = 0.04f;
    [SerializeField] float fadeTime = 1.2f;

    [Header("Story Lines")]
    [SerializeField]
    Beat[] beats = new Beat[]
    {
        new Beat { text = "You shouldn't have come here. But since you did... listen carefully.", holdTime = 2.5f },
        new Beat { text = "This place was closed long ago. The people who ran it are gone. He isn't.", holdTime = 2.5f },
        new Beat { text = "He was a patient. They called it treatment. It was something else.", holdTime = 2.5f },
        new Beat { text = "He died here, and something answered. Now he walks these halls looking for someone to blame.", holdTime = 3f },
        new Beat { text = "I can't stop him. I can only show you the way.", holdTime = 2.5f },
        new Beat { text = "Find the exit before he finds you. There are only two endings in this place.", holdTime = 3.5f },
    };

    bool ending;

    void Start()
    {
        lineText.text = "";
        if (fadeGroup != null) fadeGroup.alpha = 1f;
        StartCoroutine(RunIntro());
    }

    IEnumerator RunIntro()
    {
        yield return Fade(1f, 0f);

        foreach (Beat beat in beats)
        {
            if (backgroundImage != null && beat.image != null)
                backgroundImage.sprite = beat.image;

            if (voiceSource != null && beat.voice != null)
                voiceSource.PlayOneShot(beat.voice);

            // Typewriter effect
            lineText.text = beat.text;
            lineText.maxVisibleCharacters = 0;
            int total = beat.text.Length;
            for (int i = 1; i <= total; i++)
            {
                lineText.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(typeDelay);
            }

            yield return new WaitForSecondsRealtime(beat.holdTime);

            lineText.text = "";
            yield return new WaitForSecondsRealtime(0.5f);
        }

        yield return EndIntro();
    }

    IEnumerator EndIntro()
    {
        if (ending) yield break;
        ending = true;
        yield return Fade(0f, 1f);
        SceneManager.LoadScene("LevelDesignScene");
    }

    // Hook this up to the Skip button's OnClick
    public void Skip()
    {
        if (ending) return;
        StopAllCoroutines();
        StartCoroutine(EndIntro());
    }

    IEnumerator Fade(float from, float to)
    {
        if (fadeGroup == null) yield break;
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }
        fadeGroup.alpha = to;
    }
}