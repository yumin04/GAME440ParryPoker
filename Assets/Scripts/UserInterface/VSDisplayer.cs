using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Android;

public class VSDisplayer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI player1You;
    [SerializeField] private TextMeshProUGUI player2You;
    private float cameraTransitionTime = 4f;
    public void Init(bool isPlayer1)
    {
        gameObject.SetActive(true);

        StartCoroutine(ShowRoutine(isPlayer1));
    }

    private IEnumerator ShowRoutine(bool isPlayer1)
    {
        
        if (isPlayer1)
        {
            player1You.faceColor = Color.antiqueWhite;
            player2You.faceColor = new Color32(0xFF, 0x31, 0x31, 255);
        }
        else
        {
            player1You.faceColor = new Color32(0xFF, 0x31, 0x31, 255);
            player2You.faceColor = Color.antiqueWhite;
        }
        
        DisableCoroutine(cameraTransitionTime);
        yield return null;
    }

    private void DisableCoroutine(float disableTime)
    {
        StartCoroutine(DisablePanel(disableTime));
    }

    private IEnumerator DisablePanel(float disableTime)
    {
        // TODO: animate the vs display
        yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        // TODO: Refactor
        // So this thing does not need to know GAME
        Game.Instance.StartGame();
    }
}