using UnityEngine;
using UnityEngine.UI;

public class EndGamePlaceholder : Singleton<EndGamePlaceholder>
{
    [SerializeField] private Button EndGameButton;

    public void Start()
    {
        EndGameButton.onClick.AddListener(EndGame);
    }

    private void EndGame()
    {
        Game.Instance.EndGame();
    }
}