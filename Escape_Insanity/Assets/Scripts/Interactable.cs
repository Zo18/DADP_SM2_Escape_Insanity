using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string prompt = "Interact";
    public abstract void Interact();
}