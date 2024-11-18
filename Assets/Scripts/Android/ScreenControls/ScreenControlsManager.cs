using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class ScreenControlsManager : MonoBehaviour
    {
        private const string HideScreenControlsKey = "hide_screen_controls";

        public static ScreenControlsManager Instance { get; private set; }

        public static bool HideScreenControls
        {
            get => PlayerPrefsExtensions.GetBool(HideScreenControlsKey);
            set => PlayerPrefsExtensions.SetBool(HideScreenControlsKey, value);
        }

        public bool IsHidden { get; private set; }

        private readonly Dictionary<KeyCode, bool> _keys = new();
        
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
            Instance = this;
            _rootPanel.SetActive(!HideScreenControls);
            _showExtraBtnsButton.onClick.AddListener(() => _extraButtonsHolder.SetActive(!_extraButtonsHolder.activeSelf));
            _hideAllScreenControlsBtn.onClick.AddListener(() =>
            {
                IsHidden = !_allBtnsHolder.activeSelf;
                _allBtnsHolder.SetActive(!_allBtnsHolder.activeSelf);
            });
        }

        public void OnKeyDown(KeyCode keyCode) => _keys[keyCode] = true;

        public void OnKeyUp(KeyCode keyCode) => _keys[keyCode] = false;

        public bool IsKeyPressed(KeyCode keyCode) => _keys.TryGetValue(keyCode, out var pressed) && pressed;
    }
}