using System;
using System.IO;
using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class UltimaUnderworldApplication : MonoBehaviour
    {
        public static event Action<string> OnGamePathSet;
        public static event Action<string> OnMusicPathSet;
        public static PathMode PathMode;

        [SerializeField] private Transform _viewsHolder;
        
        private Navigator _navigator;

        private void Start()
        {
            AndroidUtils.RequestManageAllFilesAccess();
 
            _navigator = new Navigator(_viewsHolder);
            _navigator.PushView<RootView>();
        }

        public void SetGamePath(string gamePath)
        {
            if (PathMode == PathMode.Music)
            {
                OnMusicPathSet?.Invoke(gamePath);
            }
            else
            {
                OnGamePathSet?.Invoke(gamePath);
            }
        }

        private void OnDestroy()
        {
            InitLog();
            GameModel.CurrentModel.Save();
        }

        private void OnDisable()
        {
            GameModel.CurrentModel.Save();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemInit() => InitLog();
        
        private sealed class LogHandler : ILogHandler, IDisposable
        {
            public static bool _loggingWasInit = false;
            private readonly StreamWriter streamWriter;

            public LogHandler()
            {
                var dataPath = GameModel.CurrentModel.BasePath;
                
                string filePath = Path.Combine(dataPath, "Player.log");

                string errorMessage = null;
                try
                {
                    if (File.Exists(filePath))
                    {
                        string prevPath = Path.Combine(dataPath, "Player-prev.log");
                        File.Delete(prevPath);
                        File.Move(filePath, prevPath);
                    }
                }
                catch (Exception e)
                {
                    errorMessage = $"Could not preserve previous log: {e.Message}";
                }

                streamWriter = File.CreateText(filePath);
                streamWriter.AutoFlush = true;

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    streamWriter.WriteLine(errorMessage);
                }
            }

            public void LogException(Exception exception, UnityEngine.Object context)
            {
                streamWriter.WriteLine(exception.ToString());
            }

            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                string prefix = "";
                switch (logType)
                {
                    case LogType.Error:
                        prefix = "[Error] ";
                        break;

                    case LogType.Warning:
                        prefix = "[Warning] ";
                        break;

                    case LogType.Assert:
                        prefix = "[Assert] ";
                        break;

                    case LogType.Exception:
                        prefix = "[Exception] ";
                        break;
                }

                streamWriter.WriteLine(prefix + string.Format(format, args));
            }

            public void Dispose()
            {
                streamWriter.Close();
            }
        }

        static void InitLog()
        {
            if (Application.isPlaying && Application.installMode != ApplicationInstallMode.Editor && 
                !string.IsNullOrEmpty(GameModel.CurrentModel.BasePath) && !LogHandler._loggingWasInit)
            {
                return;
                LogHandler._loggingWasInit = true;
                Debug.unityLogger.logHandler = new LogHandler();
            }
        }
    }

    public enum PathMode
    {
        BasePath,
        Music
    }
}