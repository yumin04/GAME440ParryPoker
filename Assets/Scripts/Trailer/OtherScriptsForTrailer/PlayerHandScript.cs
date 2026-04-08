using System.Collections;
using System.Collections.Generic;
using SOFile;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHandscript : MonoBehaviour
{
    
    [Header("Other Objects")] 
    // [SerializeField] private TrailerAttackCard trailerAttackCard;
    [SerializeField] private TrailerObjectInstantiator trailerObjectInstantiator;
    public static int[] player1Cards = new[] { 7, 10 }; // 8, 9, 11
    public static int[] player2Cards = new[] {1, 14}; // 27, 40
    public static int[] roundCards = new[] { 9, 50, 27, 26, 40, 13, 36, 19, 8, 11};
    [SerializeField] private int subRoundNumber;
    private CardDataSO cardData;
    
    
    [SerializeField] private GameObject player1Camera;
    [SerializeField] private GameObject player2Camera;

     private void Start()
     {
         InitializePlayersHand();
         StartCoroutine(PlayCameraSequence());
     }

     private void InitializePlayersHand()
     {
         trailerObjectInstantiator.AddMultipleCardsToPlayer1(player1Cards);
         trailerObjectInstantiator.AddMultipleCardsToPlayer2(player2Cards);
     }

     private IEnumerator PlayCameraSequence()
     {
         player1Camera.SetActive(true);
         player2Camera.SetActive(false);
         HashSet<int> p1Target = new HashSet<int> { 0, 1, 5, 8, 9 };
         HashSet<int> p2Target = new HashSet<int> { 2, 4, 7, 3, 6 };
         yield return new WaitForSeconds(1.5f);
         
         for (int i = 0; i < roundCards.Length; i++)
         {
             

             if (p1Target.Contains(i))
             {
                 trailerObjectInstantiator.AddCardToPlayer1(roundCards[i]);
                 yield return new WaitForSeconds(1.5f);                 
             }
             
             if (p2Target.Contains(i))
             {
                 trailerObjectInstantiator.AddCardToPlayer2(roundCards[i]);
                 yield return new WaitForSeconds(1.5f);
             }
         }
         yield return null;
         trailerObjectInstantiator.DestroyAllPlayerHand();
         player1Camera.SetActive(false);
         player2Camera.SetActive(true);
         InitializePlayersHand();
         yield return new WaitForSeconds(1.5f);
         for (int i = 0; i < roundCards.Length; i++)
         {
             if (p1Target.Contains(i))
             {
                 trailerObjectInstantiator.AddCardToPlayer1(roundCards[i]);
                 yield return new WaitForSeconds(1.5f);                 
             }
             
             if (p2Target.Contains(i))
             {
                 trailerObjectInstantiator.AddCardToPlayer2(roundCards[i]);
                 yield return new WaitForSeconds(1.5f);
             }
         }
     }

     
     

}