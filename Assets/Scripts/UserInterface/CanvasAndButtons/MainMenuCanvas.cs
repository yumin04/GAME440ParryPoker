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

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [Tooltip("Looping menu music. Asset: Assets/Audio/jazzforgame84.mp3")]
    [SerializeField] private AudioClip mainMenuMusic;

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

        StartMainMenuMusic();
    }

    private void StartMainMenuMusic()
    {
        if (mainMenuMusic == null) mainMenuMusic = Resources.Load<AudioClip>("Audio/jazzforgame84");
        if (musicSource != null && mainMenuMusic != null)
        {
            musicSource.clip = mainMenuMusic;
            musicSource.loop = true;
            musicSource.volume = 4f;
            musicSource.mute = false;
            musicSource.Play();
        }
    }

    private void PlayButtonSound()
    {
        if (musicSource != null) musicSource.Stop();
    }
    
    // mainMenuPanel Interactions
    private void OnStartClicked()
    {
        PlayButtonSound();
        Debug.Log("OnStartClicked");
        hostClientPanel.SetActive(true);
    }
    private void OnTutorialClicked()
    {
        PlayButtonSound();
        Debug.Log("OnTutorialClicked");
    }
    private void OnExitClicked()
    {
        PlayButtonSound();
        Debug.Log("OnExitClicked");
        EndApplication.QuitApplication();
    }
    
    // HostClientPanel Interactions
    private void OnBackClicked()
    {
        PlayButtonSound();
        Debug.Log("OnBackClicked");
        hostClientPanel.SetActive(false);
        StartMainMenuMusic();
    }

    private void OnHostClicked()
    {
        PlayButtonSound();
        if (NetworkManager.Singleton == null) return;

        HostClientManager.Instance.StartHost();
        Debug.Log("StartHost");
    }

    private void OnClientClicked()
    {
        PlayButtonSound();
        if (NetworkManager.Singleton == null) return;

        HostClientManager.Instance.StartClient();
        Debug.Log("StartClient");
    }
}