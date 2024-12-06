using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnderworldExporter.Game
{
    [Preserve]
    public sealed class RootViewPresenter : Presenter<RootView>
    {
        private const string SlashSymbol = "/";

        public RootViewPresenter(IView view) : base(view)
        {
            UltimaUnderworldApplication.OnGamePathSet += gamePath =>
            {
                GameModel.CurrentModel.BasePath = gamePath + SlashSymbol;
                View.UpdateGamePath(GameModel.CurrentModel.BasePath);
            };

            UltimaUnderworldApplication.OnMusicPathSet += musicPath =>
            {
                GameModel.CurrentModel.UW1SoundBank = musicPath + SlashSymbol;
                View.UpdateMusicPath(GameModel.CurrentModel.UW1SoundBank);
            };
        }

        public void OnSetGamePathButtonClicked()
        {
#if UNITY_EDITOR
            GameModel.CurrentModel.BasePath = EditorUtility
                .OpenFolderPanel( "Choose game folder", Application.dataPath, "UW1" ) + SlashSymbol;

            View.UpdateGamePath(GameModel.CurrentModel.BasePath);
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

            View.UpdateMusicPath(GameModel.CurrentModel.UW1SoundBank);
#else
            UltimaUnderworldApplication.PathMode = PathMode.Music;
            DirectoryPicker.PickDirectory();
#endif
        }
        
        public void OnStartGameButtonClicked()
        {
            if (!string.IsNullOrEmpty(GameModel.CurrentModel.BasePath))
            {
                SceneManager.LoadScene(1);
            }
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}