using System;
using System.Globalization;
using UnityEngine;

namespace UnderworldExporter.Game
{
    [RequireComponent(typeof(FPSInputControllerC))]
    public sealed class FirstPersonController : MonoBehaviour
    {
        private const float YDefaultPosition = 0.5f;

        private bool _hideScreenControls;
        private bool _joyStickMoved = false;

        [SerializeField]
        private FPSInputControllerC _inputController;

        private void Awake()
        {
            _hideScreenControls = ScreenControlsManager.HideScreenControls;
        }
        
        public void OnJoystickMoved()
        {
            _joyStickMoved = true;
        }
        
        public void OnJoystickMoved(Vector2 direction)
        {
            if (!_joyStickMoved)
            {
                return;
            }

            Vector3 directionVector = direction.magnitude == 0
                ? new Vector3(0, YDefaultPosition, 0)
                : new Vector3(direction.x, YDefaultPosition, direction.y);

            _inputController.MoveCharacter(directionVector);
        }

        private void Update()
        {
            if (_hideScreenControls)
            {
                Vector2 move = InputManager.Move;
                // Get the input vector from keyboard or analog stick
                _inputController.MoveCharacter(new Vector3(move.x, YDefaultPosition, move.y));
            }

            _inputController.Jump(InputManager.IsPressed(KeyCode.Space));
        }
    }
}