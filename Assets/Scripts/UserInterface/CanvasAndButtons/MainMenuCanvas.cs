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

    [Header("Main Menu Visuals")]
    [Tooltip("Optional extra visuals to hide/show with the main menu (ex: TitleLogo).")]
    [SerializeField] private GameObject[] mainMenuVisualObjects;
    [Tooltip("Optional smoke particle system shown on the main menu.")]
    [SerializeField] private ParticleSystem menuSmoke;
    
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
        TryAutoWireOptionalVisuals();
        mainMenuPanel.SetActive(true);
        hostClientPanel.SetActive(false);
        SetMainMenuState(true);
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
        SetMainMenuState(false);
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
        SetMainMenuState(true);
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

    private void SetMainMenuState(bool showMainMenu)
    {
        mainMenuPanel.SetActive(showMainMenu);
        hostClientPanel.SetActive(!showMainMenu);

        if (mainMenuVisualObjects != null)
        {
            foreach (GameObject visual in mainMenuVisualObjects)
            {
                if (visual != null)
                {
                    visual.SetActive(showMainMenu);
                }
            }
        }

        if (menuSmoke != null)
        {
            GameObject smokeObject = menuSmoke.gameObject;
            smokeObject.SetActive(showMainMenu);

            if (showMainMenu)
            {
                // Reset and replay so smoke always looks correct after returning.
                menuSmoke.Clear(true);
                menuSmoke.Play(true);
            }
            else
            {
                menuSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    private void TryAutoWireOptionalVisuals()
    {
        if (menuSmoke == null)
        {
            GameObject smokeObject = GameObject.Find("Smoke");
            if (smokeObject != null)
            {
                menuSmoke = smokeObject.GetComponent<ParticleSystem>();
            }
        }

        if (mainMenuVisualObjects == null || mainMenuVisualObjects.Length == 0)
        {
            GameObject titleLogo = GameObject.Find("TitleLogo");
            if (titleLogo != null)
            {
                mainMenuVisualObjects = new[] { titleLogo };
            }
        }
    }
}