using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnderworldExporter.Game
{
    public sealed class AndroidRootViewController
    {
        private const string SlashSymbol = "/";
        private const string BaseGamePathKey = "game_path";
        
        private readonly AndroidRootView _view;

        public string BasePath
        {
            get => PlayerPrefs.GetString(BaseGamePathKey, string.Empty);
            set => PlayerPrefs.SetString(BaseGamePathKey, value);
        }
        
        public AndroidRootViewController(AndroidRootView view)
        {
            _view = view;
            Loader.BasePath = BasePath;
            
            UltimaUnderworldApplication.OnGamePathSet += gamePath =>
            {
                BasePath = Loader.BasePath = gamePath + SlashSymbol;
                view.UpdateGamePath(Loader.BasePath) ;
            };
        }

        public void OnSetGamePathButtonClicked()
        {
#if UNITY_EDITOR
            Loader.BasePath = BasePath = EditorUtility
                .OpenFolderPanel( "Choose game folder", Application.dataPath, "UW1" ) + SlashSymbol;

            _view.UpdateGamePath(Loader.BasePath);
#else
            DirectoryPicker.PickDirectory();
#endif
        }
        
        public void OnStartGameButtonClicked()
        {
            if (!string.IsNullOrEmpty(Loader.BasePath))
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