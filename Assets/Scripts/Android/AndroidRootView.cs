using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class AndroidRootView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _gamePathText;

        [SerializeField] 
        private TMP_InputField _maxFpsInputField;
        
        [SerializeField]
        private Button _startGameButton;

        [SerializeField] 
        private Button _setPathToGameButton;

        [SerializeField]
        private Button _exitGameButton;

        private AndroidRootViewController _rootViewController;
        
        private void Awake()
        {
            _rootViewController = new AndroidRootViewController(this);
            _maxFpsInputField.text = _rootViewController.MaxFps.ToString();
            _exitGameButton.onClick.AddListener(_rootViewController.OnExitButtonClicked);
            _startGameButton.onClick.AddListener(_rootViewController.OnStartGameButtonClicked);
            _setPathToGameButton.onClick.AddListener(_rootViewController.OnSetGamePathButtonClicked);
            _maxFpsInputField.onValueChanged.AddListener(newValue =>
            {
                if (int.TryParse(newValue, out var maxFpsValue))
                {
                    _rootViewController.MaxFps = maxFpsValue;
                }
            });
            UpdateGamePath(_rootViewController.BasePath);
        }

        public void UpdateGamePath(string gamePath)
        {
            _gamePathText.text = gamePath;
            _startGameButton.interactable = !string.IsNullOrEmpty(gamePath);
        }
    }
}