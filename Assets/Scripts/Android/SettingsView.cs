using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static GameModel;

namespace UnderworldExporter.Game
{
    public sealed class SettingsView : View<SettingsViewPresenter>
    {
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
        private Toggle _invertGyroscopeXAxisToggle;

        [SerializeField] 
        private Toggle _invertGyroscopeYAxisToggle;

        [SerializeField] 
        private Toggle _invertCameraXAxisToggle;

        [SerializeField] 
        private Toggle _invertCameraYAxisToggle;

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _showGamepadRebindViewButton;
 
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

        [SerializeField] private TMP_InputField _playerSwimSpeed;

        [SerializeField] private Toggle _infiniteMana;
        [SerializeField] private Toggle _godModToggle;
        [SerializeField] private Toggle _contextUIEnabledToggle;

        [SerializeField] private Toggle _autoEatToggle;
        [SerializeField] private Toggle _autoKeyUseToggle;
        [SerializeField] private Toggle _showFpsToggle;

        [SerializeField] private TMP_InputField _playerSpeed;

        [SerializeField] private Toggle _haptickFeedbackToggle;
        [SerializeField] private TMP_Dropdown _logLevelDropdown;

        protected override void OnViewInitialized()
        {
            _logLevelDropdown.value = (int)CurrentModel.LogLevel;
            _playerSpeed.text = CurrentModel.PlayerSpeed.ToString(CultureInfo.InvariantCulture);
            _playerSwimSpeed.text = CurrentModel.PlayerSwimSpeed.ToString(CultureInfo.InvariantCulture);
            _maxFpsInputField.text = CurrentModel.MaxFps.ToString();
            _haptickFeedbackToggle.isOn = CurrentModel.EnableHaptickFeedback;
            _invertGyroscopeXAxisToggle.isOn = CurrentModel.InvertGyroXAxis;
            _invertGyroscopeYAxisToggle.isOn = CurrentModel.InvertGyroYAxis;
            _invertCameraXAxisToggle.isOn = CurrentModel.InvertXAxis;
            _invertCameraYAxisToggle.isOn = CurrentModel.InvertYAxis;
            _enableGyroscopeToggle.isOn = CurrentModel.EnableGyroscope;
            _hideScreenControlsToggle.isOn = CurrentModel.HideScreenControls;
            _prserveHudAspectRatioToggle.isOn = CurrentModel.PreferOriginalHud;
            _fullscreenTouchCameraToggle.isOn = GameModel.CurrentModel.PreferFullScreenTouchCameraInMouseMode;
            _showFpsToggle.isOn = CurrentModel.ShowFps;

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

            _configureScreenControlsButton.onClick.AddListener(() => Navigator.PushView<ScreenControlsConfigurator>());
            _backButton.onClick.AddListener(() => base.OnBackButtonPressed() );
            _showGamepadRebindViewButton.onClick.AddListener(() => Navigator.PushView<GamepadRebindView>());

            _logLevelDropdown.onValueChanged.AddListener(newValue =>
            {
                CurrentModel.LogLevel = (LogLevel)newValue;
                Logger.Instance.InitLog();
            });
            
            _maxFpsInputField.onValueChanged.AddListener(newValue =>
            {
                if (int.TryParse(newValue, out var maxFpsValue))
                {
                    CurrentModel.MaxFps = maxFpsValue;
                }
            });

            _playerSpeed.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var playerSpeed))
                {
                    CurrentModel.PlayerSpeed = playerSpeed;
                }
            });

            _playerSwimSpeed.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var playerSpeed))
                {
                    CurrentModel.PlayerSwimSpeed = playerSpeed;
                }
            });

            _defaultLightLevelInput.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.DefaultLightLevel = parsedValue;
                }
            });
            
            _mouseXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.MouseXSensitivity = parsedValue;
                }
            });

            _mouseYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.MouseYSensitivity = parsedValue;
                }
            });
            
            _touchXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.TouchXSensitivity = parsedValue;
                }
            });
            
            _touchYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.TouchYSensitivity = parsedValue;
                }
            });
            
            _gyroXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.GyroXSensitivity = parsedValue;
                }
            });
            
            _gyroYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.GyroYSensitivity = parsedValue;
                }
            });
            
            _gamepadXSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.GamepadXSensitivity = parsedValue;
                }
            });
            
            _gamepadYSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.GamepadYSensitivity = parsedValue;
                }
            });

            _gamepadMouseSenstivityInputField.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.GamepadMouseEmulationSensitivity = parsedValue;
                }
            });
            
            _fov.onValueChanged.AddListener(newValue =>
            {
                if (float.TryParse(newValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    CurrentModel.FOV = parsedValue;
                }
            });
            
            _haptickFeedbackToggle.onValueChanged.AddListener(isOn => CurrentModel.EnableHaptickFeedback = isOn);
            _showFpsToggle.onValueChanged.AddListener(isOn => CurrentModel.ShowFps = isOn);
            _speakableNpcToggle.onValueChanged.AddListener(isOn => CurrentModel.SpeakableNpc = isOn);
            _infiniteMana.onValueChanged.AddListener(isOn => CurrentModel.InfiniteMana = isOn);
            _godModToggle.onValueChanged.AddListener(isOn => CurrentModel.GodMode = isOn);
            _autoEatToggle.onValueChanged.AddListener(isOn => CurrentModel.AutoEat = isOn);
            _autoKeyUseToggle.onValueChanged.AddListener(isOn => CurrentModel.AutoKeyUse = isOn);
            _contextUIEnabledToggle.onValueChanged.AddListener(isOn => CurrentModel.ContextUIEnabled = isOn);

            _enableGyroscopeToggle.onValueChanged.AddListener(isOn => CurrentModel.EnableGyroscope = isOn);
            
            _invertGyroscopeXAxisToggle.onValueChanged.AddListener(isOn => CurrentModel.InvertGyroXAxis = isOn);
            _invertGyroscopeYAxisToggle.onValueChanged.AddListener(isOn => CurrentModel.InvertGyroYAxis = isOn);

            _invertCameraXAxisToggle.onValueChanged.AddListener(isOn => CurrentModel.InvertXAxis = isOn);
            _invertCameraYAxisToggle.onValueChanged.AddListener(isOn => CurrentModel.InvertYAxis = isOn);

            _hideScreenControlsToggle.onValueChanged.AddListener(isOn => CurrentModel.HideScreenControls = isOn);
            _prserveHudAspectRatioToggle.onValueChanged.AddListener(isOn => CurrentModel.PreferOriginalHud = isOn);
            _fullscreenTouchCameraToggle.onValueChanged.AddListener(isOn => CurrentModel.PreferFullScreenTouchCameraInMouseMode = isOn);
        }

        protected override void OnViewDestroyed()
        {
            CurrentModel.Save();
        }
    }
}