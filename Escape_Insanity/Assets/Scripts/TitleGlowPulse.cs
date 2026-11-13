using UnityEngine;
using TMPro;

public class TitleGlowPulse : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] float speed = 1.5f;
    [SerializeField] float minGlow = 0.2f;
    [SerializeField] float maxGlow = 0.7f;

    Material mat;

    void Awake()
    {
        if (title == null) title = GetComponent<TMP_Text>();
        mat = title.fontMaterial; // creates an instance, so other text isn't affected
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.unscaledTime * speed) + 1f) * 0.5f;
        mat.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.Lerp(minGlow, maxGlow, t));
    }
}