using System;
using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class UltimaUnderworldApplication : MonoBehaviour
    {
        public static event Action<string> OnGamePathSet;
        public static event Action<string> OnMusicPathSet;
        public static PathMode PathMode;
        
        private void Start()
        {
            AndroidUtils.RequestManageAllFilesAccess();
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
            GameModel.CurrentModel.Save();
        }

        private void OnDisable()
        {
            GameModel.CurrentModel.Save();
        }
    }

    public enum PathMode
    {
        BasePath,
        Music
    }
}
