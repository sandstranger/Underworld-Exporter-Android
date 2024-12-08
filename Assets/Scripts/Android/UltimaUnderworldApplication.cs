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

        private void Start()
        {
            AndroidUtils.RequestManageAllFilesAccess();
        }
    }

    public enum PathMode
    {
        BasePath,
        Music
    }
}