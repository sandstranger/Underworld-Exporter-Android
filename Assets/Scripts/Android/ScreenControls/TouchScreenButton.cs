using UnityEngine;
using UnityEngine.EventSystems;

namespace UnderworldExporter.Game
{
    public sealed class TouchScreenButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] 
        private KeyCode _keyCode;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            ScreenControlsManager.OnKeyDown(_keyCode);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ScreenControlsManager.OnKeyUp(_keyCode);
        }
    }
}