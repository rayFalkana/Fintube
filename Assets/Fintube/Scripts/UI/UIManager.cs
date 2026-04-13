using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image displayImage;

    [SerializeField] private EventToggleGroup toggleGroup;
    [SerializeField] private GameObject parentMode;

    [SerializeField] private GameObject panelUI;
    [SerializeField] private CanvasGroup panelUI2;

    [SerializeField] private Button btnTurnOnUI;
    [SerializeField] private Button btnTurnOffUI;

    [SerializeField] private GameObject panelUIKeyBinding;
    [SerializeField] private Text textKeyBinding;
    [SerializeField] private Button btnTurnOffPanelUIKeyBinding;

    [SerializeField] private GameObject panelUIDisplaySetting;
    [SerializeField] private Button btnShowPanelUIDisplaySetting;

    [SerializeField] private GameObject triggerGIFLimit;

    private TypeMode newHotkey;

    private const int maxResolutionToResizeDisplayedImage = 1000;

    public delegate void _delegate(string toggle);

    FintubeManager FM;

    #region Unity
    public void StartUI(FintubeManager fintube)
    {
        FM = fintube;
        toggleGroup.StartToggleGroup();
        btnTurnOffPanelUIKeyBinding.onClick.AddListener(()=> { ActivateHotkeyUI(); });
        btnShowPanelUIDisplaySetting.onClick.AddListener(() => { ShowDisplaySetting(); });
        InitStreamerMode();
    }

    private void Update()
    {
        if (panelUIKeyBinding.activeSelf)
        {
            textKeyBinding.text = newHotkey.Hotkey;
        }
    }
    #endregion

    #region ToggleGroup
    public void EnableToggle() { toggleGroup.enabled = true; }
    public Toggle AddToggle(GameObject toggle) {
        toggle.transform.SetParent(toggleGroup.transform);
        toggle.transform.localScale = Vector3.one;
        Toggle temp = toggle.GetComponent<Toggle>();
        toggleGroup.Add(temp);
        return temp;
    }
    public Transform ParentModes() { return parentMode.transform; }
    public void AddListenerToToggle(_delegate call)
    {
        toggleGroup.onActiveToggleChanged.AddListener(x => call(x.name));
    }
    public static string GetNameOfActiveToggle(Toggle toggle)
    {
        return toggle.name;
    }
    public void ChangeToggleViaName(string name)
    {
        toggleGroup.ChangeToggleViaName(name);
    }
    #endregion

    #region displayImage
    public void TriggerGIFLimit() { triggerGIFLimit.SetActive(true); }
    public void ShowDisplaySetting() {
        if (panelUIDisplaySetting.activeSelf)
        {
            panelUIDisplaySetting.SetActive(false);
        }
        else
        {
            panelUIDisplaySetting.SetActive(true);
        }
    }
    public void OffDisplay() { displayImage.gameObject.SetActive(false); }
    public void SetImageInDisplay(Sprite sprite) {
        FM.modeController.AnimInputStopPlaying();
        displayImage.gameObject.SetActive(true);
        displayImage.sprite = sprite;
    }
    public void SetImageInDisplay(List<AnimSprite> sprite, AnimationInputController animInput) {
        animInput.PlayAnimation(sprite);
        displayImage.gameObject.SetActive(false);
    }
    public void SetImageInDisplay(List<AnimSprite> sprite, int [] ID)
    {
        FM.modeController.AnimInputPlayAnimation(sprite, ID);
        displayImage.gameObject.SetActive(false);
    }
    public void ResizeImage(int _value)
    {
        int currentWidth = displayImage.sprite.texture.width;
        int currentHeight = displayImage.sprite.texture.height;

        float aspecRatio = (float)currentWidth / currentHeight;

        int newWidth, newHeight;

        if (aspecRatio > 1f)
        {
            newWidth = Mathf.Min(currentWidth, _value);
            newHeight = Mathf.RoundToInt(newWidth / aspecRatio);
        }
        else
        {
            newHeight = Mathf.Min(currentHeight, _value);
            newWidth = Mathf.RoundToInt(newHeight * aspecRatio);
        }

        displayImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }
    public void ResizeImage() { ResizeImage(maxResolutionToResizeDisplayedImage); }

    #endregion

    #region streamerMode(AKA)HideUImode
    private void InitStreamerMode()
    {
        btnTurnOffUI.onClick.AddListener(() => {
            btnTurnOnUI.gameObject.SetActive(true);
            HideInsteadOfDeactivate(true);
            panelUI.SetActive(false);
        });

        btnTurnOnUI.onClick.AddListener(() => {
            HideInsteadOfDeactivate(false);
            panelUI.SetActive(true);
            btnTurnOnUI.gameObject.SetActive(false);
        });
    }
    private void HideInsteadOfDeactivate(bool hide)
    {
        if (hide)
        {
            panelUI2.interactable = false;
            panelUI2.alpha = 0;
        }
        else
        {
            panelUI2.interactable = true;
            panelUI2.alpha = 1;
        }
    }
    #endregion

    #region Keyboard
    public bool PanelAssignHotkey
    {
        get { return panelUIKeyBinding.activeSelf; }
    }
    public void ActivateHotkeyUI(TypeMode typeMode=null)
    {
        if (panelUIKeyBinding.activeSelf)
        {
            panelUIKeyBinding.SetActive(false);
            return;
        }
        else
        {
            panelUIKeyBinding.SetActive(true);
        }
        newHotkey = typeMode;
    }
    public void InputNewHotkey(string newHotkey)
    {
        this.newHotkey.Hotkey = newHotkey;
    }
    #endregion
}
