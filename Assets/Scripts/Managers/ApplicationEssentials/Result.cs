using GameStates;
using GameStates.States;
using Unity.Netcode;
using UnityEngine;

public class Result : MonoBehaviour
{
    public void Start()
    {
        GameStateHandler.ChangeState(new ResultState());
    }
}