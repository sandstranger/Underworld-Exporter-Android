using System;
using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class GameobjectEventsProvider : MonoBehaviour
    {
        public event Action<bool> OnEnableEvent;

        private void OnEnable() => OnEnableEvent?.Invoke(true);

        private void OnDisable() => OnEnableEvent?.Invoke(false);

    }
}