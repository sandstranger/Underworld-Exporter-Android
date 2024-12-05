using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    sealed class ScreenControlsConfigurator : View<ScreenControlsConfiguratorPresenter>
    {
        public static event Action ResetToDefaults;

        public TouchScreenItemRepositioner CurrentButton
        {
            get => _currentButton;
            set
            {
                _currentButton = value;

                if (value != null)
                {
                    _currentButtonNameText.text = value.ButtonId;
                }
            }
        }

        private TouchScreenItemRepositioner _currentButton;

        [SerializeField]
        private TMP_Text _currentButtonNameText;
        [SerializeField]
        private Button _alphaPlusBtn;
        [SerializeField]
        private Button _alphaMinusBtn;
        [SerializeField]
        private Button _sizePlusBtn;
        [SerializeField]
        private Button _sizeMinusBtn;
        [SerializeField]
        private Button _resetToDefaultsBtn;
        [SerializeField]
        private Button _backBtn;
        [SerializeField]
        private GameObject _rootPanel;

        [SerializeField]
        private float _changeSizeValue = 20.0f;
        [SerializeField]
        private float _changeAlphaValue = 0.1f;

        private void Awake()
        {
            _alphaPlusBtn.onClick.AddListener(OnAlphaPlusBtnClicked);
            _alphaMinusBtn.onClick.AddListener(OnAlphaMinusBtnClicked);
            _sizePlusBtn.onClick.AddListener(OnSizePlusBtnClicked);
            _sizeMinusBtn.onClick.AddListener(OnSizeMinusBtnClicked);
            _resetToDefaultsBtn.onClick.AddListener(() => ResetToDefaults?.Invoke());
            _backBtn.onClick.AddListener(Close);
        }

        private void Close()
        {
            CurrentButton = null;
            _currentButtonNameText.text = string.Empty;
            base.OnBackButtonPressed();
        }

        private void OnAlphaPlusBtnClicked()
        {
            if (CurrentButton == null)
            {
                return;
            }

            if ( !Mathf.Approximately(CurrentButton.Alpha,1.0f))
            {
                CurrentButton.Alpha += _changeAlphaValue;
            }
        }

        private void OnAlphaMinusBtnClicked()
        {
            if (CurrentButton == null)
            {
                return;
            }

            if ( !Mathf.Approximately(CurrentButton.Alpha,0.0f))
            {
                CurrentButton.Alpha -= _changeAlphaValue;
            }
        }

        private void OnSizePlusBtnClicked()
        {
            if (CurrentButton == null)
            {
                return;
            }

            CurrentButton.Size = new Vector2( CurrentButton.Size.x + _changeSizeValue, CurrentButton.Size.y + _changeSizeValue);
        }

        private void OnSizeMinusBtnClicked()
        {
            if (CurrentButton == null)
            {
                return;
            }

            CurrentButton.Size = new Vector2( CurrentButton.Size.x - _changeSizeValue, CurrentButton.Size.y - _changeSizeValue);
        }
    }
}