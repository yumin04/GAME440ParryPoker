using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Round : MonoBehaviour
{
    public void Start()
    {
        GameEvents.OnRoundStart?.Invoke();
    }

    void Update()
    {
        // Press G to end the game. In multiplayer we tell everyone; in solo we just do it locally.
        if (Keyboard.current == null || !Keyboard.current.gKey.wasPressedThisFrame)
            return;

        var net = NetworkManager.Singleton;
        if (net != null && net.IsListening)
        {
            var sync = FindObjectOfType<GameOverSync>();
            if (sync != null)
                sync.RequestEndGameServerRpc();
        }
        else
        {
            GameEvents.OnRoundEnd?.Invoke();
        }
    }
}