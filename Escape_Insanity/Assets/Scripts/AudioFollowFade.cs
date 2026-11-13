using UnityEngine;

// Fades this AudioSource in and out by following the black FadeOverlay.
// When the screen is black the music is silent; when the screen is clear, it plays at Max Volume.
// No changes to IntroSequence are needed: it already fades the overlay in and out.
[RequireComponent(typeof(AudioSource))]
public class AudioFollowFade : MonoBehaviour
{
    [SerializeField] CanvasGroup fadeGroup;                  // drag FadeOverlay here
    [SerializeField, Range(0f, 1f)] float maxVolume = 0.5f;  // the volume while the screen is clear

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = fadeGroup != null ? 0f : maxVolume;
    }

    void Update()
    {
        if (fadeGroup == null) return;
        source.volume = maxVolume * (1f - Mathf.Clamp01(fadeGroup.alpha));
    }
}