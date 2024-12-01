using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class GamepadRebindView : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetAllButton;
        [SerializeField] private Button _closeOverlayButton;
        [SerializeField] private GameObject _rebindOverlay;
        private void Awake()
        {
            _backButton.onClick.AddListener(() => gameObject.SetActive(false));
            _resetAllButton.onClick.AddListener(InputManager.ResetAllOverrides);
            _closeOverlayButton.onClick.AddListener(() => _rebindOverlay.SetActive(false));
        }
    }
}