using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UserInterface : Singleton<UserInterface>
{
    [SerializeField] private VSDisplayer vsDisplayer;
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private SubRoundIndicator subRoundIndicator;
    [SerializeField] private RoundIndicator roundIndicator;


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

    public void HideHealthDisplay()
    {
        healthDisplay.HideHealthDisplay();
    }
    
    public void ChangePlayer1Health(int health)
    {
        healthDisplay.SetPlayer1Health(health);
    }
    public void ChangePlayer2Health(int health)
    {
        healthDisplay.SetPlayer2Health(health);
    }


    public void EnableSubRoundNumber() => subRoundIndicator.EnableAllSubRound();

    public void DisableSubRoundNumber() => subRoundIndicator.DisableSubRoundNumber();

    public void EnableRoundNumber() => roundIndicator.EnableRoundNumber();


    public void DisableRoundNumber() => roundIndicator.DisableRoundNumber();

    public void ChangeSubRoundNumber(int subroundNumber)
    {
        subRoundIndicator.DisableSubRound(subroundNumber);
    }

    public void ChangeRoundNumber(int roundNumber)
    {
        roundIndicator.ChangeRoundText(roundNumber);

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