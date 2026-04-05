using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UserInterface : Singleton<UserInterface>
{
    [SerializeField] private VSDisplayer vsDisplayer;
    [SerializeField] private HealthDisplay healthDisplay;
    [SerializeField] private SubRoundIndicator subRoundIndicator;
    [SerializeField] private RoundIndicator roundIndicator;

    private bool isPlayer1;

    public void Init(bool isPlayer1)
    {
        this.isPlayer1 = isPlayer1;
    }
    
    public void DisplayVS()
    {
        vsDisplayer.Init(isPlayer1);
    }

    public void DisplayHealth()
    {
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

    public void DisableDisplayVS()
    {
        vsDisplayer.InstantDisableShowRoutine();
    }
}