using UnityEngine.EventSystems;

namespace UnderworldExporter.Game
{
    static class InputExtensions
    {
        public static int GetPointerId(this PointerEventData pointer) => (int) pointer.button;
    }
}