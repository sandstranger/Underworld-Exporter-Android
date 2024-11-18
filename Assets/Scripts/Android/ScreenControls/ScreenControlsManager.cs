using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class ScreenControlsManager : MonoBehaviour
    {
        private const string HideScreenControlsKey = "hide_screen_controls";
        
        public static bool HideScreenControls
        {
            get => PlayerPrefsExtensions.GetBool(HideScreenControlsKey);
            set => PlayerPrefsExtensions.SetBool(HideScreenControlsKey, value);
        }
    }
}