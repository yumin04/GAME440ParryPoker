
using UnityEngine;

public class ResultState : IGameState
{
    public void OnEnter()
    {
        Debug.Log("OnEnter ResultState");
        GameEvents.OnLoadResultScene?.Invoke();
    }

    public void OnExit()
    {
        
    }
}