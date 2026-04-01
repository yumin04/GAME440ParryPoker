
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Player1HealthText;
    [SerializeField] private TextMeshProUGUI Player2HealthText;
    [SerializeField] private GameObject Player1HealthBar;
    [SerializeField] private GameObject Player2HealthBar;
    [SerializeField] private Sprite redHealthBar;
    [SerializeField] private Sprite greenHealthBar;
    private Image Player1HealthBarImage;
    private Image Player2HealthBarImage;
    
    public void Init(bool isPlayer1)
    {

        Player1HealthBarImage = Player1HealthBar.GetComponent<Image>();
        Player2HealthBarImage = Player2HealthBar.GetComponent<Image>();
        gameObject.SetActive(true);
        if (isPlayer1)
        {
            Player1HealthText.color = Color.black;
            Player1HealthBarImage.sprite = greenHealthBar;
            Player2HealthText.color = Color.red;
            Player2HealthBarImage.sprite = redHealthBar;
        }
        else
        {
            Player1HealthText.color = Color.red;
            Player1HealthBarImage.sprite = redHealthBar;
            Player2HealthText.color = Color.black;
            Player2HealthBarImage.sprite = greenHealthBar;
        }
    }

    public void HideHealthDisplay()
    {
        gameObject.SetActive(false);
    }
    public void SetPlayer1Health(int player1Health)
    {
        Player1HealthText.text = player1Health.ToString() +" / 100 HP";
        Player1HealthBar.transform.localScale = new Vector3(player1Health / 100f, 1, 1);
    }

    public void SetPlayer2Health(int player2Health)
    {
        Player2HealthText.text = player2Health.ToString() +" / 100 HP";
        Player2HealthBar.transform.localScale = new Vector3(player2Health / 100f, 1, 1);
    }
}