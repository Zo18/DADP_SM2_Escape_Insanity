using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuFlicker : MonoBehaviour
{
    public float baseAlpha = 0.15f;
    public float maxAlpha = 0.6f;
    public float minWait = 2f;
    public float maxWait = 6f;

    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    void Start()
    {
        SetAlpha(baseAlpha);
        StartCoroutine(FlickerLoop());
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            int flickers = Random.Range(2, 5);
            for (int i = 0; i < flickers; i++)
            {
                SetAlpha(Random.Range(baseAlpha + 0.2f, maxAlpha));
                yield return new WaitForSeconds(Random.Range(0.03f, 0.1f));
                SetAlpha(baseAlpha);
                yield return new WaitForSeconds(Random.Range(0.03f, 0.12f));
            }
        }
    }

    void SetAlpha(float a)
    {
        Color c = img.color;
        c.a = a;
        img.color = c;
    }
}