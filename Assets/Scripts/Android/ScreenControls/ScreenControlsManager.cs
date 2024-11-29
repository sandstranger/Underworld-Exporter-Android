using UnityEngine;
using UnityEngine.UI;

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
            _rootPanel.SetActive(!HideScreenControls);
            _showExtraBtnsButton.onClick.AddListener(() => _extraButtonsHolder.SetActive(!_extraButtonsHolder.activeSelf));
            _hideAllScreenControlsBtn.onClick.AddListener(() => _allBtnsHolder.SetActive(!_allBtnsHolder.activeSelf));
        }
    }
}