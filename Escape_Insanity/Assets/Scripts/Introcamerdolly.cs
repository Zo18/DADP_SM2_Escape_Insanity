using UnityEngine;

public class IntroCameraDolly : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform lookTarget;      // optional: camera keeps facing this (e.g. the asylum door)
    [SerializeField] float duration = 35f;      // roughly the length of your intro text

    [Header("Handheld Sway")]
    [SerializeField] float shakeAmount = 0.04f;
    [SerializeField] float shakeSpeed = 0.7f;

    float timer;

    void Start()
    {
        if (startPoint != null)
            transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
    }

    void Update()
    {
        if (startPoint == null || endPoint == null) return;

        timer += Time.unscaledDeltaTime;
        float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(timer / duration));

        Vector3 pos = Vector3.Lerp(startPoint.position, endPoint.position, t);
        Quaternion rot = Quaternion.Slerp(startPoint.rotation, endPoint.rotation, t);

        if (lookTarget != null)
            rot = Quaternion.LookRotation(lookTarget.position - pos);

        // Perlin noise gives a smooth, unsettling handheld drift
        float n = timer * shakeSpeed;
        Vector3 offset = new Vector3(
            Mathf.PerlinNoise(n, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, n + 10f) - 0.5f,
            0f) * shakeAmount * 2f;

        transform.SetPositionAndRotation(pos + offset, rot);
    }
}