using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    sealed class Joystick : MonoBehaviour
    {
        [SerializeField]
        private zFrame.UI.Joystick _joystick;
        [SerializeField]
        private RectTransform _joystickKnob;

        private void Start()
        {
            _joystick.maxRadius = _joystickKnob.rect.width;
        }
    }
}