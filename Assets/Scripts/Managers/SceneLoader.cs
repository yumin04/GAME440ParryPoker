using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class SceneLoader : Singleton<SceneLoader>
{
    private enum SceneType
    {
        MainMenuScene,
        RoundScene,
        ResultScene,
    }

    // =========================
    // Public Scene APIs
    // =========================

    public void LoadMainMenuScene()
    {
        Load(SceneType.MainMenuScene);
    }

    public void LoadResultScene()
    {
        Load(SceneType.ResultScene);
    }

    public void LoadRoundScene()
    {
        Load(SceneType.RoundScene);
    }

    // =========================
    // Internal Loader
    // =========================

    private void Load(SceneType sceneType)
    {
        string sceneName = sceneType.ToString();

        // Going back to main menu: shut down network so we can start a new Host/Client next time
        if (sceneType == SceneType.MainMenuScene && NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
        }

        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(
                sceneName,
                LoadSceneMode.Single
            );
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}