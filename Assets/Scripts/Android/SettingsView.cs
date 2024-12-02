using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameModel;

namespace UnderworldExporter.Game
{
    public sealed class SettingsView : MonoBehaviour
    {
        [SerializeField] 
        private ScreenControlsConfigurator _screenControlsConfigurator;
        
        [SerializeField] 
        private Toggle _enableGyroscopeToggle;
        
        [SerializeField] 
        private Toggle _hideScreenControlsToggle;

        [SerializeField] 
        private Toggle _prserveHudAspectRatioToggle;
         
        [SerializeField] 
        private TMP_InputField _maxFpsInputField;
        [SerializeField] 
        private Button _configureScreenControlsButton;

        [SerializeField] 
        private Toggle _fullscreenTouchCameraToggle;

        [SerializeField] 
        private Toggle _invertXAxisToggle;

        [SerializeField] 
        private Toggle _invertYAxisToggle;

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _showGamepadRebindViewButton;
        [SerializeField] private GamepadRebindView _gamepadRebindView;

        [SerializeField] private TMP_InputField _defaultLightLevelInput;
        [SerializeField] private Toggle _speakableNpcToggle;

        [SerializeField] private TMP_InputField _mouseXSenstivityInputField;
        [SerializeField] private TMP_InputField _mouseYSenstivityInputField;
        
        [SerializeField] private TMP_InputField _gamepadXSenstivityInputField;
        [SerializeField] private TMP_InputField _gamepadYSenstivityInputField;
        
        [SerializeField] private TMP_InputField _gamepadMouseSenstivityInputField;
        
        [SerializeField] private TMP_InputField _touchXSenstivityInputField;
        [SerializeField] private TMP_InputField _touchYSenstivityInputField;
        
        [SerializeField] private TMP_InputField _gyroXSenstivityInputField;
        [SerializeField] private TMP_InputField _gyroYSenstivityInputField;
        
        [SerializeField] private TMP_InputField _fov;

        [SerializeField] private Toggle _infiniteMana;
        [SerializeField] private Toggle _godModToggle;
        [SerializeField] private Toggle _contextUIEnabledToggle;

        [SerializeField] private Toggle _autoEatToggle;
        [SerializeField] private Toggle _autoKeyUseToggle;
        
        private void Awake()
        {
            _maxFpsInputField.text = CurrentModel.MaxFps.ToString();
            _invertXAxisToggle.isOn = CurrentModel.InvertXAxix;
            _invertYAxisToggle.isOn = CurrentModel.InvertYAxix;
            _enableGyroscopeToggle.isOn = CurrentModel.EnableGyroscope;
            _hideScreenControlsToggle.isOn = CurrentModel.HideScreenControls;
            _prserveHudAspectRatioToggle.isOn = CurrentModel.PreferOriginalHud;
            _fullscreenTouchCameraToggle.isOn = GameModel.CurrentModel.PreferFullScreenTouchCameraInMouseMode;
            
            _defaultLightLevelInput.text = CurrentModel.DefaultLightLevel.ToString(CultureInfo.InvariantCulture);
            _speakableNpcToggle.isOn = CurrentModel.SpeakableNpc;

            _mouseXSenstivityInputField.text = CurrentModel.MouseXSensitivity.ToString(CultureInfo.InvariantCulture);
            _mouseYSenstivityInputField.text = CurrentModel.MouseYSensitivity.ToString(CultureInfo.InvariantCulture);

            _touchXSenstivityInputField.text = CurrentModel.TouchXSensitivity.ToString(CultureInfo.InvariantCulture);
            _touchYSenstivityInputField.text = CurrentModel.TouchYSensitivity.ToString(CultureInfo.InvariantCulture);

            _gyroXSenstivityInputField.text = CurrentModel.GyroXSensitivity.ToString(CultureInfo.InvariantCulture);
            _gyroYSenstivityInputField.text = CurrentModel.GyroYSensitivity.ToString(CultureInfo.InvariantCulture);

            _gamepadXSenstivityInputField.text = CurrentModel.GamepadXSensitivity.ToString(CultureInfo.InvariantCulture);
            _gamepadYSenstivityInputField.text = CurrentModel.GamepadYSensitivity.ToString(CultureInfo.InvariantCulture);
            _gamepadMouseSenstivityInputField.text = CurrentModel.GamepadMouseEmulationSensitivity.ToString(CultureInfo.InvariantCulture);

            _fov.text = CurrentModel.FOV.ToString(CultureInfo.InvariantCulture);

            _infiniteMana.isOn = CurrentModel.InfiniteMana;
            _godModToggle.isOn = CurrentModel.GodMode;

            _contextUIEnabledToggle.isOn = CurrentModel.ContextUIEnabled;
            _autoEatToggle.isOn = CurrentModel.AutoEat;
            _autoKeyUseToggle.isOn = CurrentModel.AutoKeyUse;
            
            _configureScreenControlsButton.onClick.AddListener(() => _screenControlsConfigurator.Show());
            _backButton.onClick.AddListener(() => gameObject.SetActive(false));
            _showGamepadRebindViewButton.onClick.AddListener(()=>_gamepadRebindView.gameObject.SetActive(true));

            _maxFpsInputField.onValueChanged.AddListener(newValue =>
            {
                if (int.TryParse(newValue, out var maxFpsValue))
                {
                    CurrentModel.MaxFps = maxFpsValue;
                }
            });

            _defaultLightLevelInput.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.DefaultLightLevel = parsedValue;
                }
            });
            
            _mouseXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.MouseXSensitivity = parsedValue;
                }
            });

            _mouseYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.MouseYSensitivity = parsedValue;
                }
            });
            
            _touchXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.TouchXSensitivity = parsedValue;
                }
            });
            
            _touchYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.TouchYSensitivity = parsedValue;
                }
            });
            
            _gyroXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.GyroXSensitivity = parsedValue;
                }
            });
            
            _gyroYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.GyroYSensitivity = parsedValue;
                }
            });
            
            _gamepadXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.GamepadXSensitivity = parsedValue;
                }
            });
            
            _gamepadYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.GamepadYSensitivity = parsedValue;
                }
            });

            _gamepadMouseSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.GamepadMouseEmulationSensitivity = parsedValue;
                }
            });
            
            _fov.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, out var parsedValue))
                {
                    CurrentModel.FOV = parsedValue;
                }
            });
            
            _speakableNpcToggle.onValueChanged.AddListener(isOn => CurrentModel.SpeakableNpc = isOn);
            _infiniteMana.onValueChanged.AddListener(isOn => CurrentModel.InfiniteMana = isOn);
            _godModToggle.onValueChanged.AddListener(isOn => CurrentModel.GodMode = isOn);
            _autoEatToggle.onValueChanged.AddListener(isOn => CurrentModel.AutoEat = isOn);
            _autoKeyUseToggle.onValueChanged.AddListener(isOn => CurrentModel.AutoKeyUse = isOn);
            _contextUIEnabledToggle.onValueChanged.AddListener(isOn => CurrentModel.ContextUIEnabled = isOn);

            _enableGyroscopeToggle.onValueChanged.AddListener(isOn => CurrentModel.EnableGyroscope = isOn);
            _invertXAxisToggle.onValueChanged.AddListener(isOn => CurrentModel.InvertXAxix = isOn);
            _invertYAxisToggle.onValueChanged.AddListener(isOn => CurrentModel.InvertYAxix = isOn);
            _hideScreenControlsToggle.onValueChanged.AddListener(isOn => CurrentModel.HideScreenControls = isOn);
            _prserveHudAspectRatioToggle.onValueChanged.AddListener(isOn => CurrentModel.PreferOriginalHud = isOn);
            _fullscreenTouchCameraToggle.onValueChanged.AddListener(isOn => CurrentModel.PreferFullScreenTouchCameraInMouseMode = isOn);
        }

        private void OnDisable() => CurrentModel.Save();
    }
}