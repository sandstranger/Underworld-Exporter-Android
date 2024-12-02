using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnderworldExporter.Game
{
    public sealed class AndroidRootViewController
    {
        private const string SlashSymbol = "/";
        private readonly AndroidRootView _view;

        public AndroidRootViewController(AndroidRootView view)
        {
            _view = view;

            UltimaUnderworldApplication.OnGamePathSet += gamePath =>
            {
                GameModel.CurrentModel.BasePath = gamePath + SlashSymbol;
                view.UpdateGamePath(GameModel.CurrentModel.BasePath) ;
            };

            UltimaUnderworldApplication.OnMusicPathSet += musicPath =>
            {
                GameModel.CurrentModel.UW1SoundBank = musicPath + SlashSymbol;
                view.UpdateMusicPath(GameModel.CurrentModel.UW1SoundBank) ;
            };
        }

        public void OnSetGamePathButtonClicked()
        {
#if UNITY_EDITOR
            GameModel.CurrentModel.BasePath = EditorUtility
                .OpenFolderPanel( "Choose game folder", Application.dataPath, "UW1" ) + SlashSymbol;

            _view.UpdateGamePath(GameModel.CurrentModel.BasePath);
#else
            UltimaUnderworldApplication.PathMode = PathMode.BasePath;
            DirectoryPicker.PickDirectory();
#endif
        }

        public void OnSetMusicPathButtonClicked()
        {
#if UNITY_EDITOR
            GameModel.CurrentModel.UW1SoundBank = EditorUtility
                .OpenFolderPanel( "Choose music folder", Application.dataPath, "UW1_Music" ) + SlashSymbol;

            _view.UpdateMusicPath(GameModel.CurrentModel.UW1SoundBank);
#else
            UltimaUnderworldApplication.PathMode = PathMode.Music;
            DirectoryPicker.PickDirectory();
#endif
        }
        
        public void OnStartGameButtonClicked()
        {
            if (!string.IsNullOrEmpty(GameModel.CurrentModel.BasePath))
            {
                Application.targetFrameRate = GameModel.CurrentModel.MaxFps;
                SceneManager.LoadScene(1);
            }
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}