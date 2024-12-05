using UnityEngine;

namespace UnderworldExporter.Game
{
    internal static class Extensions
    {
        public static bool IsLastSibling(this Transform t)
        {
            if(t != null && t.parent != null)
            {
                return t.GetSiblingIndex() == (t.parent.childCount - 1);
            }
            return true;
        }
    }
}