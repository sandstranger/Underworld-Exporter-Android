using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    sealed class Joystick : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent OnJoystickStartedToMove;

        [SerializeField]
        private zFrame.UI.Joystick _joystick;
        [SerializeField]
        private RectTransform _joystickKnob;

        private void Start()
        {
            _joystick.maxRadius = _joystickKnob.rect.width;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnJoystickStartedToMove.Invoke();
        }
    }
}