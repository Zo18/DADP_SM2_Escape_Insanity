using UnityEngine;

public class SlowDrift : MonoBehaviour
{
    public float zoomAmount = 0.05f;
    public float speed = 0.05f;
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.localScale = startScale * (1f + t * zoomAmount);
    }
}