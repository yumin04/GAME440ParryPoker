using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TrailerInput : Singleton<TrailerInput>
{
    [Header("UI")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button goButton;

    [Header("Input")]
    [SerializeField] private InputAction escAction;

    private List<string> scenes;
    private string selectedScene;

    private void OnEnable()
    {
        escAction.Enable();
        escAction.performed += OnEscPressed;
    }

    private void OnDisable()
    {
        escAction.performed -= OnEscPressed;
        escAction.Disable();
    }

    private void Start()
    {
        scenes = GetTrailerScenes();

        dropdown.ClearOptions();
        dropdown.AddOptions(scenes);

        dropdown.onValueChanged.AddListener(OnDropdownChanged);

        if (scenes.Count > 0)
        {
            selectedScene = scenes[0];
        }

        goButton.onClick.AddListener(OnClickGo);
    }

    private void OnEscPressed(InputAction.CallbackContext ctx)
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
    }

    private void OnDropdownChanged(int index)
    {
        selectedScene = scenes[index];
    }

    private void OnClickGo()
    {
        if (!string.IsNullOrEmpty(selectedScene))
        {
            StartCoroutine(LoadWithDelay(selectedScene));
            uiPanel.SetActive(!uiPanel.activeSelf);
        }
    }

    private IEnumerator LoadWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }

    // =========================
    // Scene Logic
    // =========================

    public List<string> GetTrailerScenes()
    {
        List<string> scenes = new List<string>();

        int count = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);

            if (path.Contains("Scenes/Trailers"))
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                scenes.Add(name);
            }
        }

        return scenes;
    }
}