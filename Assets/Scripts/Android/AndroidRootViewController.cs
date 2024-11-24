using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnderworldExporter.Game
{
    public sealed class AndroidRootViewController
    {
        private const string SlashSymbol = "/";
        private const string BaseGamePathKey = "game_path";
        private const string MusicPathKey = "music_path";
        private const string MaxFpsKey = "max_fps";
        private const int MaxFpsDefaultValue = 60;
        
        private readonly AndroidRootView _view;

        public int MaxFps
        {
            get => PlayerPrefs.GetInt(MaxFpsKey, MaxFpsDefaultValue);
            set => PlayerPrefs.SetInt(MaxFpsKey, value);
        }
        
        public string BasePath
        {
            get => PlayerPrefs.GetString(BaseGamePathKey, string.Empty);
            private set
            {
                PlayerPrefs.SetString(BaseGamePathKey, value);
                PlayerPrefs.Save();
            }
        }

        public string MusicPath
        {
            get => PlayerPrefs.GetString(MusicPathKey, string.Empty);
            private set
            {
                PlayerPrefs.SetString(MusicPathKey, value);
                PlayerPrefs.Save();
            }
        }

        public AndroidRootViewController(AndroidRootView view)
        {
            _view = view;
            Loader.BasePath = BasePath;
            MusicController.UW1Path = MusicPath;
            
            UltimaUnderworldApplication.OnGamePathSet += gamePath =>
            {
                BasePath = Loader.BasePath = gamePath + SlashSymbol;
                AndroidUtils.CopyConfigFiles(Loader.BasePath);
                view.UpdateGamePath(Loader.BasePath) ;
            };

            UltimaUnderworldApplication.OnMusicPathSet += musicPath =>
            {
                MusicPath = MusicController.UW1Path = musicPath + SlashSymbol;
                view.UpdateMusicPath(MusicController.UW1Path) ;
            };
        }

        public void OnSetGamePathButtonClicked()
        {
#if UNITY_EDITOR
            Loader.BasePath = BasePath = EditorUtility
                .OpenFolderPanel( "Choose game folder", Application.dataPath, "UW1" ) + SlashSymbol;

            _view.UpdateGamePath(Loader.BasePath);
#else
            UltimaUnderworldApplication.PathMode = PathMode.BasePath;
            DirectoryPicker.PickDirectory();
#endif
        }

        public void OnSetMusicPathButtonClicked()
        {
#if UNITY_EDITOR
            MusicController.UW1Path = MusicPath = EditorUtility
                .OpenFolderPanel( "Choose music folder", Application.dataPath, "UW1_Music" ) + SlashSymbol;

            _view.UpdateMusicPath(MusicController.UW1Path);
#else
            UltimaUnderworldApplication.PathMode = PathMode.Music;
            DirectoryPicker.PickDirectory();
#endif
        }
        
        public void OnStartGameButtonClicked()
        {
            if (!string.IsNullOrEmpty(Loader.BasePath))
            {
                Application.targetFrameRate = MaxFps;
                SceneManager.LoadScene(1);
            }
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}