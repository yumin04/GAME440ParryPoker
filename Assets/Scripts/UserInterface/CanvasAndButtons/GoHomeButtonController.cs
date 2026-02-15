using GenericHelpers;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.CanvasAndButtons {
	// Makes the "Go Home" button hidden until the game is over. When game is done, we show it so you can go back to main menu.
	public class GoHomeButtonController : MonoBehaviour {
		[SerializeField] private GameObject goHomeButton;

		[Header("Button design (used when button is created at runtime)")]
		[SerializeField] private Color buttonColor = new(0.2f, 0.5f, 0.9f);
		[SerializeField] private Vector2 buttonSize = new(220f, 50f);
		[SerializeField] [Range(0f, 1f)] private float buttonPositionY = 0.1f;
		[SerializeField] private Sprite buttonSprite;
		[SerializeField] private string buttonText = "Go Home";
		[SerializeField] private Color textColor = Color.white;
		[SerializeField] private int fontSize = 22;
		[SerializeField] private Font textFont;

		private void Awake() {
			if (!goHomeButton)
				CreateGoHomeButton();

			// this should be valid here
			goHomeButton.SetActive(false);
		}

		// Hook this up to your button's OnClick in the inspector. It just loads the main menu.
		public void OnGoHomeClicked() {
			SceneLoader.Instance?.LoadMainMenuScene();
		}

		private void CreateGoHomeButton() {
			var canvasGo = new GameObject("GoHomeCanvas");
			var canvas = canvasGo.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvasGo.AddComponent<CanvasScaler>();
			canvasGo.AddComponent<GraphicRaycaster>();

			var buttonGo = new GameObject("GoHomeButton");
			buttonGo.transform.SetParent(canvasGo.transform, false);

			var rect = buttonGo.AddComponent<RectTransform>();
			rect.anchorMin = new Vector2(0.5f, buttonPositionY);
			rect.anchorMax = new Vector2(0.5f, buttonPositionY);
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.sizeDelta = buttonSize;
			rect.anchoredPosition = Vector2.zero;

			var image = buttonGo.AddComponent<Image>();
			image.color = buttonColor;
			if (buttonSprite) image.sprite = buttonSprite;

			var button = buttonGo.AddComponent<Button>();
			button.targetGraphic = image;
			button.onClick.AddListener(OnGoHomeClicked);

			var textGo = new GameObject("Text");
			textGo.transform.SetParent(buttonGo.transform, false);
			var textRect = textGo.AddComponent<RectTransform>();
			textRect.anchorMin = Vector2.zero;
			textRect.anchorMax = Vector2.one;
			textRect.offsetMin = Vector2.zero;
			textRect.offsetMax = Vector2.zero;
			var text = textGo.AddComponent<Text>();
			text.text = buttonText;
			text.alignment = TextAnchor.MiddleCenter;
			text.color = textColor;
			text.fontSize = fontSize;
			if (textFont) text.font = textFont;

			goHomeButton = buttonGo;
		}

		private void OnEnable() {
			GameEvents.OnRoundEnd += ShowGoHomeButton;
		}

		private void OnDisable() {
			GameEvents.OnRoundEnd -= ShowGoHomeButton;
		}

		// When the round ends, show the button so the player can go home
		private void ShowGoHomeButton() {
			goHomeButton?.SetActive(true);
		}
	}
}