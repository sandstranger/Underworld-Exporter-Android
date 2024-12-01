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
        private TMP_Text _musicPathText;

        [SerializeField]
        private Button _startGameButton;

        [SerializeField] 
        private Button _setPathToGameButton;

        [SerializeField] 
        private Button _setPathToMusicButton;

        [SerializeField] 
        private Button _showSettingsViewButton;
        
        [SerializeField]
        private Button _exitGameButton;

        [SerializeField] 
        private SettingsView _settingsView;
        
        private AndroidRootViewController _rootViewController;
        
        private void Awake()
        {
            _rootViewController = new AndroidRootViewController(this);
            _exitGameButton.onClick.AddListener(_rootViewController.OnExitButtonClicked);
            _startGameButton.onClick.AddListener(_rootViewController.OnStartGameButtonClicked);
            _setPathToGameButton.onClick.AddListener(_rootViewController.OnSetGamePathButtonClicked);
            _setPathToMusicButton.onClick.AddListener(_rootViewController.OnSetMusicPathButtonClicked);
            _showSettingsViewButton.onClick.AddListener(() => _settingsView.gameObject.SetActive(true));
            UpdateGamePath(_rootViewController.BasePath);
            UpdateMusicPath(_rootViewController.MusicPath);
        }

        public void UpdateGamePath(string gamePath)
        {
            _gamePathText.text = gamePath;
            _startGameButton.interactable = !string.IsNullOrEmpty(gamePath);
        }

        public void UpdateMusicPath(string musicPath) => _musicPathText.text = musicPath;
    }
}