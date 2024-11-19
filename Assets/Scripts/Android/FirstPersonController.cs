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
                // Get the input vector from keyboard or analog stick
                Vector3 directionVector;
                directionVector = new Vector3(Input.GetAxis("Horizontal"), YDefaultPosition, Input.GetAxis("Vertical"));
                _inputController.MoveCharacter(directionVector);
                
                _inputController.Jump(Input.GetButton("Jump"));
                return;
            }
            
            _inputController.Jump(ScreenControlsManager.IsKeyPressed(KeyCode.Space));
        }
    }
}