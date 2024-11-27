using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnderworldExporter.Game
{
    [RequireComponent(typeof(TouchScreenItemRepositioner))]
    sealed class DragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField]
        private ScreenControlsConfigurator controlsConfigurator;

        [FormerlySerializedAs("_buttonPositionHelper")] [SerializeField]
        private TouchScreenItemRepositioner touchScreenItemRepositioner;

        public void OnDrag(PointerEventData eventData) =>
            touchScreenItemRepositioner.Position = InputManager.MousePosition;

        public void OnPointerDown(PointerEventData eventData) =>
            controlsConfigurator.CurrentButton = touchScreenItemRepositioner;
    }
}