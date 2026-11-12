using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float hoverScale = 1.1f;
    public float pressScale = 0.95f;
    public float speed = 12f;

    [Header("Optional sounds")]
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    private Vector3 baseScale;
    private Vector3 targetScale;
    private bool isHovering;

    void Awake()
    {
        baseScale = transform.localScale;
        targetScale = baseScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale, targetScale, Time.unscaledDeltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData e)
    {
        isHovering = true;
        targetScale = baseScale * hoverScale;
        PlaySound(hoverClip);
    }

    public void OnPointerExit(PointerEventData e)
    {
        isHovering = false;
        targetScale = baseScale;
    }

    public void OnPointerDown(PointerEventData e)
    {
        targetScale = baseScale * pressScale;
        PlaySound(clickClip);
    }

    public void OnPointerUp(PointerEventData e)
    {
        targetScale = isHovering ? baseScale * hoverScale : baseScale;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}