using UnityEngine;

public class PlayerObjectSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    
    [Header("Poses")]
    public GameObject cardHold;
    public GameObject handLay;
    public GameObject cardCatch;
    public GameObject frogDeath;
    public GameObject frogEvilSmile;
    public GameObject frogGrab;
    public GameObject frogHit;
    public GameObject frogIdle;
    public GameObject frogShoot;
    public GameObject frogVictory;
    
    // 핵심 함수
    public GameObject SwapPose(GameObject posePrefab, Transform targetTransform)
    {
        GameObject obj = Instantiate(posePrefab, targetTransform.position,  targetTransform.rotation);

        // obj.transform.position = targetTransform.position;
        // obj.transform.rotation = targetTransform.rotation;
        // obj.transform.localScale = targetTransform.localScale;
        return obj;
    }

    public void DeactivatePlayer1()
    {
        player1.SetActive(false);
    }
    public void DeactivatePlayer2()
    {
        player2.SetActive(false);
    }
    public void ActivatePlayer1()
    {
        player1.SetActive(true);
    }
    public void ActivatePlayer2()
    {
        player2.SetActive(true);
    }
}