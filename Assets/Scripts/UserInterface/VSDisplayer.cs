using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Android;

public class VSDisplayer : MonoBehaviour
{

    [SerializeField] private GameObject player1You;
    [SerializeField] private GameObject player2You;
    public void Init(bool isPlayer1)
    {
        gameObject.SetActive(true);

        StartCoroutine(ShowRoutine(isPlayer1));
    }

    private IEnumerator ShowRoutine(bool isPlayer1)
    {
        
        if (isPlayer1)
        {
            player1You.SetActive(true);
            player2You.SetActive(false);
        }
        else
        {
            player1You.SetActive(false);
            player2You.SetActive(true);
        }

        
        // TODO: animate the vs display
        yield return new WaitForSeconds(2f);
        
        Debug.Log("[DEBUG] After Show Routine");
        gameObject.SetActive(false);
        
    }

    public void OnDisable()
    {
        // TODO: Refactor
        // So this thing does not need to know GAME
        Game.Instance.StartGame();
    }
}