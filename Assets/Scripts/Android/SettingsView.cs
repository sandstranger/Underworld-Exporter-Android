using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class SettingsView : MonoBehaviour
    {
        [SerializeField] 
        private ScreenControlsConfigurator _screenControlsConfigurator;

        [SerializeField] 
        private Toggle _hideScreenControlsToggle;

        [SerializeField] 
        private Toggle _prserveHudAspectRatioToggle;
         
        [SerializeField] 
        private TMP_InputField _maxFpsInputField;
        [SerializeField] 
        private Button _configureScreenControlsButton;

        [SerializeField] 
        private Toggle _fullscreenTouchCameraToggle;

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _showGamepadRebindViewButton;
        [SerializeField] private GamepadRebindView _gamepadRebindView;
        private void Awake()
        {
            _maxFpsInputField.text = Loader.MaxFps.ToString();
            _hideScreenControlsToggle.isOn = ScreenControlsManager.HideScreenControls;
            _prserveHudAspectRatioToggle.isOn = HudAspectRatioPreserver.PreserveHudAspectRatio;
            _fullscreenTouchCameraToggle.isOn = CanvasSortOrderChanger.ChangeSortingOrder;
         
            _configureScreenControlsButton.onClick.AddListener(() => _screenControlsConfigurator.Show());
            _backButton.onClick.AddListener(() => gameObject.SetActive(false));
            _showGamepadRebindViewButton.onClick.AddListener(()=>_gamepadRebindView.gameObject.SetActive(true));

            _maxFpsInputField.onValueChanged.AddListener(newValue =>
            {
                if (int.TryParse(newValue, out var maxFpsValue))
                {
                    Loader.MaxFps = maxFpsValue;
                }
            });
            _hideScreenControlsToggle.onValueChanged.AddListener(isOn => ScreenControlsManager.HideScreenControls = isOn);
            _prserveHudAspectRatioToggle.onValueChanged.AddListener(isOn => HudAspectRatioPreserver.PreserveHudAspectRatio = isOn);
            _fullscreenTouchCameraToggle.onValueChanged.AddListener(isOn => CanvasSortOrderChanger.ChangeSortingOrder = isOn);
        }
    }
}