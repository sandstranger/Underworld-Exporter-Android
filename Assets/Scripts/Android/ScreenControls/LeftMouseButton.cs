using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class LeftMouseButton : MonoBehaviour
    {
        private readonly KeyCode _keyCode = KeyCode.Mouse0;
        
        public void OnPointerDown()
        {
            ScreenControlsManager.OnKeyDown(_keyCode);
        }

        public void OnPointerUp()
        {
            ScreenControlsManager.OnKeyUp(_keyCode);
        }
    }
}