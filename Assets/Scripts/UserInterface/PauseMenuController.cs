using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    private static readonly Color LabelGold = new Color(1f, 0.93f, 0.72f);
    private static Sprite _whiteSprite;

    private PauseMenuStyle _style;
    private Canvas _canvas;
    private GameObject _panel;
    private TextMeshProUGUI _qualityLabel;
    private Toggle _fullscreenToggle;
    private bool _open;

    private void Awake()
    {
        _style = Resources.Load<PauseMenuStyle>("PauseMenuStyle");
        BuildUi();
        ApplyLoadedFullscreenPref();
        SetOpen(false);
    }

    private void ApplyLoadedFullscreenPref()
    {
        if (PlayerPrefs.HasKey("pp_fullscreen"))
            Screen.fullScreen = PlayerPrefs.GetInt("pp_fullscreen") != 0;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;
        if (!Keyboard.current.pKey.wasPressedThisFrame) return;
        SetOpen(!_open);
    }

    private void BuildUi()
    {
        var canvasGo = new GameObject("SettingsCanvas");
        canvasGo.transform.SetParent(transform, false);
        _canvas = canvasGo.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 200;
        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        canvasGo.AddComponent<GraphicRaycaster>();

        var dimRt = FullStretch(canvasGo.transform, "Dim");
        var dim = dimRt.gameObject.AddComponent<Image>();
        dim.color = new Color(0f, 0f, 0f, 0.78f);
        dim.raycastTarget = true;

        var center = new GameObject("Center");
        var centerRt = center.AddComponent<RectTransform>();
        centerRt.SetParent(canvasGo.transform, false);
        centerRt.anchorMin = new Vector2(0.5f, 0.5f);
        centerRt.anchorMax = new Vector2(0.5f, 0.5f);
        centerRt.pivot = new Vector2(0.5f, 0.5f);
        centerRt.sizeDelta = new Vector2(480f, 520f);

        _panel = new GameObject("SettingsPanel");
        var panelRt = _panel.AddComponent<RectTransform>();
        panelRt.SetParent(centerRt, false);
        Stretch(panelRt);
        var v = _panel.AddComponent<VerticalLayoutGroup>();
        v.spacing = 18f;
        v.padding = new RectOffset(24, 24, 24, 24);
        v.childAlignment = TextAnchor.UpperCenter;
        v.childControlHeight = true;
        v.childControlWidth = true;
        v.childForceExpandHeight = false;
        v.childForceExpandWidth = true;

        AddTitle(_panel.transform, "Settings");
        AddFullscreenRow(_panel.transform);
        AddQualityRow(_panel.transform);
        AddMenuButton(_panel.transform, "Main menu", GoMainMenu);
        AddMenuButton(_panel.transform, "Close", () => SetOpen(false));
    }

    private void SetOpen(bool open)
    {
        _open = open;
        if (_canvas) _canvas.enabled = open;
        if (open && _fullscreenToggle) _fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
        if (open) RefreshQualityLabel();
    }

    private void GoMainMenu()
    {
        SetOpen(false);
        Time.timeScale = 1f;
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            NetworkManager.Singleton.Shutdown();
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadMainMenuScene();
        else
            SceneManager.LoadScene("MainMenuScene");
    }

    private void AddTitle(Transform parent, string text)
    {
        var go = new GameObject("Title");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        var le = go.AddComponent<LayoutElement>();
        le.minHeight = 56f;
        le.preferredHeight = 56f;
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.font = GetFont();
        tmp.fontSize = 46f;
        tmp.fontWeight = FontWeight.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = LabelGold;
        tmp.raycastTarget = false;
    }

    private void AddMenuButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick)
    {
        var row = new GameObject("Row_" + label);
        var rt = row.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        var le = row.AddComponent<LayoutElement>();
        le.minHeight = 68f;
        le.preferredHeight = 72f;

        var capsule = new GameObject("Capsule");
        var capRt = capsule.AddComponent<RectTransform>();
        capRt.SetParent(rt, false);
        Stretch(capRt);
        var img = capsule.AddComponent<Image>();
        if (_style != null && _style.buttonSprite != null)
        {
            img.sprite = _style.buttonSprite;
            img.type = Image.Type.Sliced;
            img.material = _style.buttonMaterial;
        }
        else
        {
            img.color = new Color(0.15f, 0.15f, 0.18f, 0.95f);
        }

        img.raycastTarget = true;

        var btn = row.AddComponent<Button>();
        btn.targetGraphic = img;
        var cb = btn.colors;
        cb.normalColor = Color.white;
        cb.highlightedColor = new Color(0.78f, 0.76f, 0.82f);
        cb.pressedColor = new Color(0.55f, 0.52f, 0.6f);
        cb.selectedColor = new Color(0.96f, 0.96f, 0.96f);
        cb.fadeDuration = 0.1f;
        btn.colors = cb;
        btn.onClick.AddListener(onClick);

        var textGo = new GameObject("Text");
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.SetParent(rt, false);
        Stretch(textRt);
        var tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.font = GetFont();
        tmp.fontSize = 42f;
        tmp.fontWeight = FontWeight.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = LabelGold;
        tmp.raycastTarget = false;
    }

    private void AddFullscreenRow(Transform parent)
    {
        var row = new GameObject("FullscreenRow");
        var rt = row.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        var le = row.AddComponent<LayoutElement>();
        le.minHeight = 44f;
        le.preferredHeight = 44f;
        var h = row.AddComponent<HorizontalLayoutGroup>();
        h.childAlignment = TextAnchor.MiddleLeft;
        h.spacing = 12f;
        h.childControlWidth = false;
        h.childControlHeight = true;
        h.childForceExpandWidth = false;
        h.childForceExpandHeight = false;
        h.padding = new RectOffset(8, 8, 0, 0);

        var labelGo = new GameObject("Label");
        var labelRt = labelGo.AddComponent<RectTransform>();
        labelRt.SetParent(rt, false);
        var labelLe = labelGo.AddComponent<LayoutElement>();
        labelLe.flexibleWidth = 1f;
        labelLe.minWidth = 200f;
        var labelTmp = labelGo.AddComponent<TextMeshProUGUI>();
        labelTmp.text = "Fullscreen";
        labelTmp.font = GetFont();
        labelTmp.fontSize = 32f;
        labelTmp.fontWeight = FontWeight.Bold;
        labelTmp.color = LabelGold;
        labelTmp.raycastTarget = false;

        var toggleGo = new GameObject("Toggle");
        var toggleRt = toggleGo.AddComponent<RectTransform>();
        toggleRt.SetParent(rt, false);
        var toggleLe = toggleGo.AddComponent<LayoutElement>();
        toggleLe.preferredWidth = 48f;
        toggleLe.preferredHeight = 32f;
        _fullscreenToggle = toggleGo.AddComponent<Toggle>();
        var bg = new GameObject("Background");
        var bgRt = bg.AddComponent<RectTransform>();
        bgRt.SetParent(toggleRt, false);
        Stretch(bgRt);
        var bgImg = bg.AddComponent<Image>();
        bgImg.sprite = WhiteSprite();
        bgImg.color = new Color(0.2f, 0.2f, 0.25f, 1f);
        var check = new GameObject("Checkmark");
        var checkRt = check.AddComponent<RectTransform>();
        checkRt.SetParent(bgRt, false);
        checkRt.anchorMin = new Vector2(0.12f, 0.12f);
        checkRt.anchorMax = new Vector2(0.88f, 0.88f);
        checkRt.offsetMin = Vector2.zero;
        checkRt.offsetMax = Vector2.zero;
        var checkImg = check.AddComponent<Image>();
        checkImg.sprite = WhiteSprite();
        checkImg.color = LabelGold;
        _fullscreenToggle.targetGraphic = bgImg;
        _fullscreenToggle.graphic = checkImg;
        _fullscreenToggle.isOn = Screen.fullScreen;
        _fullscreenToggle.onValueChanged.AddListener(on =>
        {
            Screen.fullScreen = on;
            PlayerPrefs.SetInt("pp_fullscreen", on ? 1 : 0);
            PlayerPrefs.Save();
        });
    }

    private void AddQualityRow(Transform parent)
    {
        var row = new GameObject("QualityRow");
        var rt = row.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        var le = row.AddComponent<LayoutElement>();
        le.minHeight = 72f;
        le.preferredHeight = 72f;

        var capsule = new GameObject("Capsule");
        var capRt = capsule.AddComponent<RectTransform>();
        capRt.SetParent(rt, false);
        Stretch(capRt);
        var img = capsule.AddComponent<Image>();
        if (_style != null && _style.buttonSprite != null)
        {
            img.sprite = _style.buttonSprite;
            img.type = Image.Type.Sliced;
            img.material = _style.buttonMaterial;
        }
        else
        {
            img.color = new Color(0.15f, 0.15f, 0.18f, 0.95f);
        }

        img.raycastTarget = true;

        var btn = row.AddComponent<Button>();
        btn.targetGraphic = img;
        var cb = btn.colors;
        cb.normalColor = Color.white;
        cb.highlightedColor = new Color(0.78f, 0.76f, 0.82f);
        cb.pressedColor = new Color(0.55f, 0.52f, 0.6f);
        btn.colors = cb;
        btn.onClick.AddListener(CycleQuality);

        var textGo = new GameObject("Text");
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.SetParent(rt, false);
        Stretch(textRt);
        _qualityLabel = textGo.AddComponent<TextMeshProUGUI>();
        _qualityLabel.font = GetFont();
        _qualityLabel.fontSize = 34f;
        _qualityLabel.fontWeight = FontWeight.Bold;
        _qualityLabel.alignment = TextAlignmentOptions.Center;
        _qualityLabel.color = LabelGold;
        _qualityLabel.raycastTarget = false;
        RefreshQualityLabel();
    }

    private void CycleQuality()
    {
        var names = QualitySettings.names;
        if (names == null || names.Length == 0) return;
        int i = (QualitySettings.GetQualityLevel() + 1) % names.Length;
        QualitySettings.SetQualityLevel(i);
        RefreshQualityLabel();
    }

    private void RefreshQualityLabel()
    {
        if (!_qualityLabel) return;
        var names = QualitySettings.names;
        int q = QualitySettings.GetQualityLevel();
        string name = names != null && q >= 0 && q < names.Length ? names[q] : "?";
        _qualityLabel.text = "Graphics: " + name;
    }

    private TMP_FontAsset GetFont()
    {
        if (_style != null && _style.labelFont != null) return _style.labelFont;
        return Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
    }

    private static RectTransform FullStretch(Transform parent, string name)
    {
        var go = new GameObject(name);
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        Stretch(rt);
        return rt;
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;
    }

    private static Sprite WhiteSprite()
    {
        if (_whiteSprite != null) return _whiteSprite;
        var t = Texture2D.whiteTexture;
        _whiteSprite = Sprite.Create(t, new Rect(0f, 0f, t.width, t.height), new Vector2(0.5f, 0.5f), 100f);
        return _whiteSprite;
    }
}
