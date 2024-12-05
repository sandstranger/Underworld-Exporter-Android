using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class RootView : View<RootViewPresenter>
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

        protected override void OnViewInitialized()
        {
            _exitGameButton.onClick.AddListener(Presenter.OnExitButtonClicked);
            _startGameButton.onClick.AddListener(Presenter.OnStartGameButtonClicked);
            _setPathToGameButton.onClick.AddListener(Presenter.OnSetGamePathButtonClicked);
            _setPathToMusicButton.onClick.AddListener(Presenter.OnSetMusicPathButtonClicked);
            _showSettingsViewButton.onClick.AddListener(() => Navigator.PushView<SettingsView>());
            UpdateGamePath(GameModel.CurrentModel.BasePath);
            UpdateMusicPath(GameModel.CurrentModel.UW1SoundBank);
        }

        public void UpdateGamePath(string gamePath)
        {
            _gamePathText.text = gamePath;
            _startGameButton.interactable = !string.IsNullOrEmpty(gamePath);
        }

        protected override void OnBackButtonPressed()
        {
            Presenter.OnExitButtonClicked();
        }

        public void UpdateMusicPath(string musicPath) => _musicPathText.text = musicPath;
    }
}