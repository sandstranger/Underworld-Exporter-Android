using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderworldExporter.Game
{
    sealed class UltimaUnderworldApplication : MonoBehaviour
    {
        private void Start()
        {
            AndroidUtils.RequestManageAllFilesAccess();
        }
    }
}
