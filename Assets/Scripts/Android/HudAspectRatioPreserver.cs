using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class HudAspectRatioPreserver : MonoBehaviour
    {
        private const string PreserveHudAspectRationKey = "preserve_hud_aspect_ratio";
        
        public static bool PreserveHudAspectRatio
        {
            get => PlayerPrefsExtensions.GetBool(PreserveHudAspectRationKey, false);
            set => PlayerPrefsExtensions.SetBool(PreserveHudAspectRationKey, value);
        }
        
        [SerializeField] 
        private RectTransform _hudRoot;

        [SerializeField] 
        private GameObject[] _blackSprites;

        [SerializeField] 
        private int _originalHudWidth = 320;
        [SerializeField] 
        private int _originalHudHeight = 240;

        private void Start()
        {
            bool preserveHudAspectRatio = PreserveHudAspectRatio;

            foreach (var blackSprite in _blackSprites)
            {
                blackSprite.SetActive(preserveHudAspectRatio);
            }

            if (preserveHudAspectRatio)
            {
                _hudRoot.anchorMax = _hudRoot.anchorMin = Vector2.one * 0.5f;
                _hudRoot.sizeDelta = new Vector2(_originalHudWidth, _originalHudHeight);
            }
        }
    }
}