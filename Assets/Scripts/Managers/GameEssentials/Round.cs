using UnityEngine;

public class Round : MonoBehaviour
{
    public void Start()
    {
        GameEvents.OnRoundStart?.Invoke();
    }
}