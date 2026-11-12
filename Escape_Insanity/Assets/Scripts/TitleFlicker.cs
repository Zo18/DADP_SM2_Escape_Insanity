using System.Collections;
using UnityEngine;
using TMPro;

public class TitleFlicker : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] float minDelay = 2f;
    [SerializeField] float maxDelay = 6f;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip buzzClip;

    Color baseColor;

    void Awake()
    {
        if (title == null) title = GetComponent<TMP_Text>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        baseColor = title.color;
    }

    void OnEnable() => StartCoroutine(FlickerLoop());

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(minDelay, maxDelay));

            // Play the buzz at the start of each flicker burst
            if (audioSource != null && buzzClip != null)
                audioSource.PlayOneShot(buzzClip);

            int flickers = Random.Range(3, 7);
            for (int i = 0; i < flickers; i++)
            {
                title.alpha = Random.Range(0.1f, 0.6f);
                yield return new WaitForSecondsRealtime(Random.Range(0.03f, 0.09f));
                title.alpha = 1f;
                yield return new WaitForSecondsRealtime(Random.Range(0.03f, 0.12f));
            }
            title.color = baseColor;
        }
    }
}