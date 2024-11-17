using UnityEngine;

namespace UnderworldExporter.Game
{
    sealed class UltimaUnderworldApplication : MonoBehaviour
    {
        private void Start()
        {
            AndroidUtils.CopyConfigFiles();
            AndroidUtils.RequestManageAllFilesAccess();
        }
    }
}
