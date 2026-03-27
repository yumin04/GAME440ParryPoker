using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UserInterface : Singleton<UserInterface>
{
    [SerializeField] private VSDisplayer vsDisplayer;
    [SerializeField] private HealthDisplay healthDisplay;

    public void DisplayVS(ulong serverClientId)
    {
        ulong myId = NetworkManager.Singleton.LocalClientId;

        bool isPlayer1 = (myId == serverClientId);

        vsDisplayer.Init(isPlayer1);
    }

    public void DisplayHealth(ulong serverClientId)
    {
        ulong myId = NetworkManager.Singleton.LocalClientId;
        bool isPlayer1 = (myId == serverClientId);
        healthDisplay.Init(isPlayer1);
    }

    public void ChangePlayer1Health(int health)
    {
        healthDisplay.SetPlayer1Health(health);
    }
    public void ChangePlayer2Health(int health)
    {
        healthDisplay.SetPlayer2Health(health);
    }
    
    public void DisplayRoundNumber(int roundNumber)
    {
        
    }

    public void DisplaySubroundNumber(int subroundNumber)
    {
        
    }

    public void Player2Win()
    {
        // TODO: Winner Display
    }
    public void Player1Win()
    {
        // TODO: Winner Display
    }
}