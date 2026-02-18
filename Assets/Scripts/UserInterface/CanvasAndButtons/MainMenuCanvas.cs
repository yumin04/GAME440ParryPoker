using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuCanvas : MonoBehaviour
{
    // Don't change it, made this so we do not need to hook up to UI again.
    // To make things scalable, this is better
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject hostClientPanel;
    [Space]
    
    [Header("Main Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button exitButton;

    [Header("Host Client Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    public void Awake()
    {
        Debug.Log("MainMenuCanvas Awake");
        mainMenuPanel.SetActive(true);
        hostClientPanel.SetActive(false);
    }
    
    public void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);
        tutorialButton.onClick.AddListener(OnTutorialClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        
        backButton.onClick.AddListener(OnBackClicked);
        hostButton.onClick.AddListener(OnHostClicked);
        clientButton.onClick.AddListener(OnClientClicked);
    }
    
    // mainMenuPanel Interactions
    private void OnStartClicked()
    {
        Debug.Log("OnStartClicked");
        // make the "Host Client Panel" Pop Up
        hostClientPanel.SetActive(true);
    }
    private void OnTutorialClicked()
    {
        Debug.Log("OnTutorialClicked");
        // After all game is made
        // Doesn't need to be Tutorial?
    }
    private void OnExitClicked()
    {
        Debug.Log("OnExitClicked");
        EndApplication.QuitApplication();
    }
    
    // HostClientPanel Interactions
    private void OnBackClicked()
    {
        Debug.Log("OnBackClicked");
        hostClientPanel.SetActive(false);
    }

    private void OnHostClicked()
    {
        if (NetworkManager.Singleton == null) return;

        HostClientManager.Instance.StartHost();
        Debug.Log("StartHost");
        // Move On To Next Scene
    }

    private void OnClientClicked()
    {
        if (NetworkManager.Singleton == null) return;

        HostClientManager.Instance.StartClient();
        Debug.Log("StartClient");
        // Move On To Next Scene
    }
}