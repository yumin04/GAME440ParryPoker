
using TMPro;
using UnityEngine;

public class RoundIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RoundIndicatorText;
    
    public void EnableRoundNumber()
    {
        gameObject.SetActive(true);
    }

    public void DisableRoundNumber()
    {
        gameObject.SetActive(false);
    }

    public void ChangeRoundText(int roundNumber)
    {
        RoundIndicatorText.text = "ROUND " + roundNumber.ToString();
    }
}
