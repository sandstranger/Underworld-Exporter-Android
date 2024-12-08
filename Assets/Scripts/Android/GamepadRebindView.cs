using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class GamepadRebindView : View<GamepadRebindViewPresenter>
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetAllButton;
        [SerializeField] private Button _closeOverlayButton;
        [SerializeField] private GameObject _rebindOverlay;

        protected override void OnViewInitialized()
        {
            _backButton.onClick.AddListener(OnBackButtonPressed);
            _resetAllButton.onClick.AddListener(InputManager.ResetAllOverrides);
            _closeOverlayButton.onClick.AddListener(() => _rebindOverlay.SetActive(false));
        }
    }
}