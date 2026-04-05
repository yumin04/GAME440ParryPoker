
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Player1HealthText;
    [SerializeField] protected TextMeshProUGUI Player2HealthText;
    [SerializeField] protected GameObject Player1HealthBar;
    [SerializeField] protected GameObject Player2HealthBar;
    [SerializeField] protected Sprite redHealthBar;
    [SerializeField] protected Sprite greenHealthBar;
    protected Image Player1HealthBarImage;
    protected Image Player2HealthBarImage;
    
    public virtual void Init(bool isPlayer1)
    {

        Player1HealthBarImage = Player1HealthBar.GetComponent<Image>();
        Player2HealthBarImage = Player2HealthBar.GetComponent<Image>();
        gameObject.SetActive(true);
        if (isPlayer1)
        {
            Player1HealthText.color = Color.black;
            Player1HealthBarImage.sprite = greenHealthBar;
            Player2HealthText.color = new Color32(0xFF, 0x31, 0x31, 255);
            Player2HealthBarImage.sprite = redHealthBar;
        }
        else
        {

            Player1HealthText.color = new Color32(0xFF, 0x31, 0x31, 255);
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