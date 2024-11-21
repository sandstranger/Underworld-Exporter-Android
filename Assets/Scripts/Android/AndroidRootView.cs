using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class AndroidRootView : MonoBehaviour
    {
        [SerializeField] 
        private ScreenControlsConfigurator _screenControlsConfigurator;
        [SerializeField] 
        private TMP_Text _gamePathText;

        [SerializeField] 
        private Toggle _hideScreenControlsToggle;

        [SerializeField] 
        private Toggle _prserveHudAspectRatioToggle;
         
        [SerializeField] 
        private TMP_InputField _maxFpsInputField;
        
        [SerializeField]
        private Button _startGameButton;

        [SerializeField] 
        private Button _setPathToGameButton;

        [SerializeField]
        private Button _exitGameButton;

        [SerializeField] 
        private Button _configureScreenControlsButton;

        [SerializeField] 
        private Toggle _fullscreenTouchCameraToggle;
        
        private AndroidRootViewController _rootViewController;
        
        private void Awake()
        {
            _rootViewController = new AndroidRootViewController(this);
            
            _maxFpsInputField.text = _rootViewController.MaxFps.ToString();
            _hideScreenControlsToggle.isOn = ScreenControlsManager.HideScreenControls;
            _prserveHudAspectRatioToggle.isOn = HudAspectRatioPreserver.PreserveHudAspectRatio;
            _fullscreenTouchCameraToggle.isOn = CanvasSortOrderChanger.ChangeSortingOrder;

            _exitGameButton.onClick.AddListener(_rootViewController.OnExitButtonClicked);
            _startGameButton.onClick.AddListener(_rootViewController.OnStartGameButtonClicked);
            _setPathToGameButton.onClick.AddListener(_rootViewController.OnSetGamePathButtonClicked);
            _configureScreenControlsButton.onClick.AddListener(() => _screenControlsConfigurator.Show());
            _maxFpsInputField.onValueChanged.AddListener(newValue =>
            {
                if (int.TryParse(newValue, out var maxFpsValue))
                {
                    _rootViewController.MaxFps = maxFpsValue;
                }
            });
            _hideScreenControlsToggle.onValueChanged.AddListener(isOn => ScreenControlsManager.HideScreenControls = isOn);
            _prserveHudAspectRatioToggle.onValueChanged.AddListener(isOn => HudAspectRatioPreserver.PreserveHudAspectRatio = isOn);
            _fullscreenTouchCameraToggle.onValueChanged.AddListener(isOn => CanvasSortOrderChanger.ChangeSortingOrder = isOn);
            UpdateGamePath(_rootViewController.BasePath);
        }

        public void UpdateGamePath(string gamePath)
        {
            _gamePathText.text = gamePath;
            _startGameButton.interactable = !string.IsNullOrEmpty(gamePath);
        }
    }
}