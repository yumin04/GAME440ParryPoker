
using TMPro;
using UnityEngine;


public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Player1HealthText;
    [SerializeField] private TextMeshProUGUI Player2HealthText;
    
    public void Init(bool isPlayer1)
    {
        gameObject.SetActive(true);
        if (isPlayer1)
        {
            Player1HealthText.color = Color.blue;
            Player2HealthText.color = Color.black;
        }
        else
        {
            Player1HealthText.color = Color.black;
            Player2HealthText.color = Color.blue;
        }
    }
    
    public void SetPlayer1Health(int player1Health)
    {
        Player1HealthText.text = "Player1: "+ player1Health.ToString() +" HP";
    }

    public void SetPlayer2Health(int player2Health)
    {
        Player2HealthText.text = "Player2: " + player2Health.ToString() +" HP";
    }
}