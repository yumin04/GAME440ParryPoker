using Unity.Netcode;
using UnityEngine;

public class Result : MonoBehaviour
{
    public void Start()
    {
        GameStateHandler.ChangeState(new ResultState());
    }
}