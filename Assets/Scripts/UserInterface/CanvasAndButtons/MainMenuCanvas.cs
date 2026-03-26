using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour {
	// Don't change it, made this so we do not need to hook up to UI again.
	// To make things scalable, this is better (even though this is TOTALLY not scalable)
	[Header("Panels")]
	[SerializeField] private GameObject mainMenuPanel;
	[SerializeField] private GameObject hostClientPanel;
	[SerializeField] private GameObject clientAddressPanel;

	[Space]

	[Header("Main Menu Buttons")]
	[SerializeField] private Button startButton;
	[SerializeField] private Button tutorialButton;
	[SerializeField] private Button exitButton;

	[Header("Host Client Buttons")]
	[FormerlySerializedAs("backButton")]
	[SerializeField] private Button backButtonHostClient;
	[SerializeField] private Button hostButton;
	[SerializeField] private Button clientButton;

	[Header("Client Address Panels")]
	[SerializeField] private Button backButtonClientAddress;
	[SerializeField] private Button clientStartButton;
	[SerializeField] private TMP_InputField addressInputField;

	[Header("Audio")] [SerializeField] private AudioSource musicSource;
	[Tooltip("Looping menu music. Asset: Assets/Audio/jazzforgame84.mp3")]
	[SerializeField] private AudioClip mainMenuMusic;

	public void Awake() {
		Debug.Log("MainMenuCanvas Awake");
		mainMenuPanel.SetActive(true);
		hostClientPanel.SetActive(false);
	}

	public void Start() {
		startButton.onClick.AddListener(OnStartClicked);
		tutorialButton.onClick.AddListener(OnTutorialClicked);
		exitButton.onClick.AddListener(OnExitClicked);

		backButtonHostClient.onClick.AddListener(OnBackHostClientClicked);
		hostButton.onClick.AddListener(OnHostClicked);
		clientButton.onClick.AddListener(OnClientClicked);

		backButtonClientAddress.onClick.AddListener(OnBackClientAddressClicked);
		clientStartButton.onClick.AddListener(OnClientStartClicked);

		StartMainMenuMusic();
	}

	private void StartMainMenuMusic() {
		if (!mainMenuMusic) mainMenuMusic = Resources.Load<AudioClip>("Audio/jazzforgame84");
		if (!musicSource || !mainMenuMusic) return;
		musicSource.clip = mainMenuMusic;
		musicSource.loop = true;
		musicSource.volume = 4f;
		musicSource.mute = false;
		musicSource.Play();
	}

	private void PlayButtonSound() {
		if (musicSource) musicSource.Stop();
	}

	// mainMenuPanel Interactions
	private void OnStartClicked() {
		PlayButtonSound();
		Debug.Log("OnStartClicked");
		hostClientPanel.SetActive(true);
	}

	private void OnTutorialClicked() {
		PlayButtonSound();
		Debug.Log("OnTutorialClicked");
	}

	private void OnExitClicked() {
		PlayButtonSound();
		Debug.Log("OnExitClicked");
		EndApplication.QuitApplication();
	}

	// HostClientPanel Interactions
	private void OnBackHostClientClicked() {
		PlayButtonSound();
		Debug.Log("OnBackHostClientClicked");
		hostClientPanel.SetActive(false);
		StartMainMenuMusic();
	}

	private void OnHostClicked() {
		PlayButtonSound();
		if (!NetworkManager.Singleton) return;

		HostClientManager.Instance.StartHost();
		Debug.Log("StartHost");
	}

	private void OnClientClicked() {
		PlayButtonSound();
		Debug.Log("OnClientClicked");
		clientAddressPanel.SetActive(true);
		hostClientPanel.SetActive(false);
	}

	private void OnBackClientAddressClicked() {
		PlayButtonSound();
		Debug.Log("OnBackClientAddressClicked");
		clientAddressPanel.SetActive(false);
		hostClientPanel.SetActive(true);
	}

	private void OnClientStartClicked() {
		PlayButtonSound();
		if (!NetworkManager.Singleton) return;

		var address = addressInputField.text;
		if (address.Length > 0)
			HostClientManager.Instance.StartClient(address);
		else
			HostClientManager.Instance.StartClient();

		Debug.Log("StartClient");
	}
}
