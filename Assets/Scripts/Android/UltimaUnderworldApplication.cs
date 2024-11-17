using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class UltimaUnderworldApplication : MonoBehaviour
    {
        private void Start()
        {
            AndroidUtils.CopyConfigFiles();
            AndroidUtils.RequestManageAllFilesAccess();
        }

        public void SetGamePath(string gamePath) => Loader.BasePath = gamePath;
    }
}
