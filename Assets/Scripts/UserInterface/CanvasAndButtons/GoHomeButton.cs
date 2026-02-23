using UnityEngine;
using UnityEngine.UI;

// TODO: 
namespace UserInterface.CanvasAndButtons
{
    // Makes the "Go Home" button hidden until the game is over. When game is done, we show it so you can go back to main menu.
    public class GoHomeButton : MonoBehaviour
    {
        [SerializeField] private GameObject goHomeButton;
        
        
        // TODO: Delete all this and start refactoring
        [Header("Button design (used when button is created at runtime)")]
        [SerializeField] private Color buttonColor = new Color(0.2f, 0.5f, 0.9f);
        [SerializeField] private Vector2 buttonSize = new Vector2(220f, 50f);
        [SerializeField] [Range(0f, 1f)] private float buttonPositionY = 0.1f;
        [SerializeField] private Sprite buttonSprite;
        [SerializeField] private string buttonText = "Go Home";
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private int fontSize = 22;
        [SerializeField] private Font textFont;

        void Awake()
        {
            if (goHomeButton == null)
                goHomeButton = CreateGoHomeButton();

            if (goHomeButton != null)
                goHomeButton.SetActive(false);
        }

        GameObject CreateGoHomeButton()
        {
            var canvasGO = new GameObject("GoHomeCanvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            var btnGO = new GameObject("GoHomeButton");
            btnGO.transform.SetParent(canvasGO.transform, false);

            var rect = btnGO.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, buttonPositionY);
            rect.anchorMax = new Vector2(0.5f, buttonPositionY);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = buttonSize;
            rect.anchoredPosition = Vector2.zero;

            var image = btnGO.AddComponent<Image>();
            image.color = buttonColor;
            if (buttonSprite != null) image.sprite = buttonSprite;

            var btn = btnGO.AddComponent<Button>();
            btn.targetGraphic = image;
            btn.onClick.AddListener(OnGoHomeClicked);

            var textGO = new GameObject("Text");
            textGO.transform.SetParent(btnGO.transform, false);
            var textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            var text = textGO.AddComponent<Text>();
            text.text = buttonText;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = textColor;
            text.fontSize = fontSize;
            if (textFont != null) text.font = textFont;

            return btnGO;
        }

        void OnEnable()
        {
            GameEvents.OnRoundEnd += ShowGoHomeButton;
        }

        void OnDisable()
        {
            GameEvents.OnRoundEnd -= ShowGoHomeButton;
        }

        // When the round ends, show the button so the player can go home
        void ShowGoHomeButton()
        {
            if (goHomeButton != null)
                goHomeButton.SetActive(true);
        }

        // Hook this up to your button's OnClick in the inspector. It just loads the main menu.
        public void OnGoHomeClicked()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadMainMenuScene();
        }
    }
}
