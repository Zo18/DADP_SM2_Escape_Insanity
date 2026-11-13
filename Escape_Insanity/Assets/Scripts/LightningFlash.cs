using System.Collections;
using UnityEngine;

// Randomly flashes a light like lightning, then plays thunder after a short delay.
// Put this on an empty object and assign the scene's Directional Light.
public class LightningFlash : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] Light flashLight;                       // usually the Directional Light
    [SerializeField] float flashIntensity = 2.5f;
    [SerializeField] Color flashColor = new Color(0.8f, 0.87f, 1f);

    [Header("Timing")]
    [SerializeField] float firstFlashDelay = 4f;
    [SerializeField] float minDelay = 6f;
    [SerializeField] float maxDelay = 14f;

    [Header("Thunder (optional)")]
    [SerializeField] AudioSource thunderSource;
    [SerializeField] AudioClip[] thunderClips;
    [SerializeField] float thunderDelayMin = 0.5f;           // thunder arrives after the flash
    [SerializeField] float thunderDelayMax = 2.5f;

    float baseIntensity;
    Color baseColor;

    void Awake()
    {
        if (flashLight == null) return;
        baseIntensity = flashLight.intensity;
        baseColor = flashLight.color;
    }

    void OnEnable()
    {
        StartCoroutine(LightningLoop());
    }

    void OnDisable()
    {
        RestoreLight();
    }

    IEnumerator LightningLoop()
    {
        yield return new WaitForSecondsRealtime(firstFlashDelay);

        while (true)
        {
            yield return FlashRoutine();
            yield return new WaitForSecondsRealtime(Random.Range(minDelay, maxDelay));
        }
    }

    // Call this from another script if you want a flash at a specific moment (e.g. on a certain line)
    public void Strike()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        if (thunderSource != null && thunderClips != null && thunderClips.Length > 0)
            StartCoroutine(ThunderRoutine());

        if (flashLight == null) yield break;

        int pulses = Random.Range(2, 5);
        for (int i = 0; i < pulses; i++)
        {
            flashLight.color = flashColor;
            flashLight.intensity = flashIntensity * Random.Range(0.6f, 1f);
            yield return new WaitForSecondsRealtime(Random.Range(0.04f, 0.1f));

            RestoreLight();
            yield return new WaitForSecondsRealtime(Random.Range(0.04f, 0.12f));
        }

        // One final, longer flash
        flashLight.color = flashColor;
        flashLight.intensity = flashIntensity;
        yield return new WaitForSecondsRealtime(Random.Range(0.12f, 0.25f));
        RestoreLight();
    }

    IEnumerator ThunderRoutine()
    {
        yield return new WaitForSecondsRealtime(Random.Range(thunderDelayMin, thunderDelayMax));
        AudioClip clip = thunderClips[Random.Range(0, thunderClips.Length)];
        if (clip != null) thunderSource.PlayOneShot(clip);
    }

    void RestoreLight()
    {
        if (flashLight == null) return;
        flashLight.intensity = baseIntensity;
        flashLight.color = baseColor;
    }
}