using System;
using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class UltimaUnderworldApplication : MonoBehaviour
    {
        public static event Action<string> OnGamePathSet;
        
        private void Start()
        {
            AndroidUtils.RequestManageAllFilesAccess();
        }

        public void SetGamePath(string gamePath)
        {
            OnGamePathSet?.Invoke(gamePath);
        }
    }
}
