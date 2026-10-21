using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public int total = 3;
    public TextMeshProUGUI hud;
    int found;

    public bool AllFound => found >= total;

    void Awake()
    {
        I = this;
        if (hud == null) hud = FindFirstObjectByType<TextMeshProUGUI>();
        Refresh("");
    }

    public void Collect() { found++; Refresh(""); }
    public void Show(string msg) { Refresh(msg); }

    void Refresh(string msg)
    {
        if (hud == null) return;
        hud.text = "Patient files: " + found + "/" + total + (msg == "" ? "" : "\n" + msg);
    }
}