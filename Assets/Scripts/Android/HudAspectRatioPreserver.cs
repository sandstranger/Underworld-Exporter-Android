using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class HudAspectRatioPreserver : MonoBehaviour
    {
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
            SettingsView.OnViewClosed += UpdateAspectRatio;
            UpdateAspectRatio();
        }
        
        private void UpdateAspectRatio()
        {
            bool preserveHudAspectRatio = GameModel.CurrentModel.PreferOriginalHud;

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