using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class ScreenControlsManager : MonoBehaviour
    {
        private static readonly Dictionary<KeyCode, bool> _keys = new();
        
        [SerializeField] 
        private Button _showExtraBtnsButton;
        
        [SerializeField] 
        private Button _hideAllScreenControlsBtn;
        
        [SerializeField] 
        private GameObject _rootPanel;

        [SerializeField] 
        private GameObject _allBtnsHolder;
        
        [SerializeField] 
        private GameObject _extraButtonsHolder;
        
        private void Awake()
        {
            NavigatorHolder.OnRootViewClosed += UpdateVisibility;
            UpdateVisibility();
            _showExtraBtnsButton.onClick.AddListener(() => _extraButtonsHolder.SetActive(!_extraButtonsHolder.activeSelf));
            _hideAllScreenControlsBtn.onClick.AddListener(() => _allBtnsHolder.SetActive(!_allBtnsHolder.activeSelf));
        }

        public static void OnKeyDown(KeyCode keyCode) => _keys[keyCode] = true;

        public static void OnKeyUp(KeyCode keyCode) => _keys[keyCode] = false;

        public static bool IsKeyPressed(KeyCode keyCode) => _keys.TryGetValue(keyCode, out var pressed) && pressed;

        private void UpdateVisibility()
        {
            _rootPanel.SetActive(!GameModel.CurrentModel.HideScreenControls);
        }
    }
}