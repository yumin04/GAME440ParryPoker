using UnityEngine;

public class CardPriorityPosition : MonoBehaviour
{
    private void Awake()
    {
        GameParameters.CardPriorityPosition = transform;
    }

    private void OnDestroy()
    {
        if (GameParameters.CardPriorityPosition == transform)
            GameParameters.CardPriorityPosition = null;
    }
}