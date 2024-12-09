using System.Globalization;
using UnityEngine;

namespace UnderworldExporter.Game
{
    [RequireComponent(typeof(FPSInputControllerC))]
    public sealed class FirstPersonController : MonoBehaviour
    {
        private float YDefaultPosition => !_character.NoClipEnabled ? 0.5f : 0.0f;
        private bool _joyStickMoved = false;

        [SerializeField] 
        private UWCharacter _character;
        [SerializeField]
        private FPSInputControllerC _inputController;

        public void OnJoystickMoved()
        {
            _joyStickMoved = true;
        }
        
        public void OnJoystickMoved(Vector2 direction)
        {
#if UNITY_EDITOR            
            if (!_joyStickMoved)
            {
                return;
            }
#else            
            if (!_joyStickMoved || !InputManager.IsTouchActive )
            {
                return;
            }
#endif
            Vector3 directionVector = direction.magnitude == 0
                ? new Vector3(0, YDefaultPosition, 0)
                : new Vector3(direction.x, YDefaultPosition, direction.y);

            _inputController.MoveCharacter(directionVector);
        }

        private void Update()
        {
            if (!InputManager.IsTouchActive)
            {
                Vector2 move = InputManager.Move;
                // Get the input vector from keyboard or analog stick
                _inputController.MoveCharacter(new Vector3(move.x, YDefaultPosition, move.y));
            }
            _inputController.Jump(InputManager.IsPressed(KeyCode.Space) || ScreenControlsManager.IsKeyPressed(KeyCode.Space));
        }
    }
}