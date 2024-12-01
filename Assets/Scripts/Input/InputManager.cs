using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#if !UNITY_EDITOR
using UnityEngine.InputSystem.EnhancedTouch;
#endif
namespace UnderworldExporter.Game
{
    public sealed class InputManager : MonoBehaviour
    {
        public static readonly TimeSpan TimeForEmulateRightMouseBtnClicks = TimeSpan.FromSeconds(1);
        public const int FakeMouseButtonId = 2980;
        public const int LeftMouseButtonId = 0;
        public const int RightMouseButtonId = 1;

        public static event Action<InputType> OnInputTypeChanged;

        public static event Action<bool> OnKeyRebindStarted
        {
            add
            {
                if (_instance._keyRebindListener != null)
                {
                    _instance._keyRebindListener.OnEnableEvent += value;
                }
            }
            remove
            {
                if (_instance._keyRebindListener != null)
                {
                    _instance._keyRebindListener.OnEnableEvent -= value;
                }
            }
        }

        private const string EnableGyroscopePrefsKey = "enable_gyroscope";
        private const string TouchSchemeName = "Touch";
        private const string KeyboardMouseSchemeName = "KeyboardMouse";
        private const string GamepadSchemeName = "Gamepad";
        
#if UNITY_EDITOR
        public static InputType CurrentInputType { get; private set; } = InputType.KeyboardMouse;
#else
        public static InputType CurrentInputType { get; private set; } = InputType.Touch;
#endif
        
        public static bool EnableGyroscope
        {
            get
            {
                if (!_enableGyroscope.HasValue)
                {
                    _enableGyroscope = PlayerPrefsExtensions.GetBool(EnableGyroscopePrefsKey, false);
                }

                return _enableGyroscope.Value;
            }

            set
            {
                _enableGyroscope = value;
                PlayerPrefsExtensions.SetBool(EnableGyroscopePrefsKey, value);
            }
        }
        
        public static bool IsTouchActive
        {
            get
            {
#if UNITY_EDITOR
                return !ScreenControlsManager.HideScreenControls;
#else
                return InputManager.CurrentInputType == InputType.Touch;
#endif
            }
        }
        
        public static Vector2 Move => _instance._moveAction.ReadValue<Vector2>();

        public static Vector2 Look
        {
            get
            {
#if UNITY_EDITOR
                if (!ScreenControlsManager.HideScreenControls)
                {
                    return _instance._touchCamera.CurrentTouchDelta;
                }
                else
                {
                    return _instance._lookAction.ReadValue<Vector2>();
                }
#else                
                var gyro = UnityEngine.InputSystem.Gyroscope.current;
                
                if (EnableGyroscope && gyro !=null )
                {
                    var result = gyro.angularVelocity.ReadValue();
                    return new Vector2(result.y, result.x);
                }
                else if (CurrentInputType == InputType.Touch)
                {
                    return _instance._touchCamera.CurrentTouchDelta;
                }
                else
                {
                    return _instance._lookAction.ReadValue<Vector2>();
                }
#endif
            }
        }

        public static Vector2 MousePosition
        {
            get
            {
                switch (CurrentInputType)
                {
                    case InputType.Gamepad:
                        return _instance._virtualMouse.position;
                    case InputType.KeyboardMouse:
                        return Mouse.current.position.ReadValue();
                    case InputType.Touch:
                        var activeTouchesArray = Touch.activeTouches;
                        if (activeTouchesArray.Count > 0)
                        {
                            _lastTouchPosition = activeTouchesArray[0].screenPosition;
                            return _lastTouchPosition;
                        }

                        return _lastTouchPosition;
                }

                return default;
            }
        }

        private static bool? _enableGyroscope;
        private static InputManager _instance;
        private static readonly Dictionary<KeyCode, InputAction> _actions = new();
        private static Vector2 _lastTouchPosition = Vector2.zero;

        [SerializeField] private TouchCamera _touchCamera;
        [SerializeField] private GameobjectEventsProvider _keyRebindListener;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private InputActionAsset _playerInputActionAsset;
        [SerializeField] private Transform _virtualMouse;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _lookAction;

        public enum InputType
        {
            KeyboardMouse,
            Gamepad,
            Touch
        }

        private void Awake()
        {
            _instance = this;
            EnableGyroscopeSupport();
            EnableEnhancedTouchSupport();

            _moveAction = InputSystem.actions.FindAction("Move");
            _lookAction = InputSystem.actions.FindAction("Look");

            if (_actions.Count == 0)
            {
                _actions[KeyCode.Space] = InputSystem.actions.FindAction("Jump");
                _actions[KeyCode.Mouse0] = InputSystem.actions.FindAction("LeftMouseClick");
                _actions[KeyCode.Mouse1] = InputSystem.actions.FindAction("RightMouseClick");
                _actions[KeyCode.Mouse2] = InputSystem.actions.FindAction("MiddleMouseClick");
                _actions[KeyCode.F1] = InputSystem.actions.FindAction("F1");
                _actions[KeyCode.F2] = InputSystem.actions.FindAction("F2");
                _actions[KeyCode.F3] = InputSystem.actions.FindAction("F3");
                _actions[KeyCode.F4] = InputSystem.actions.FindAction("F4");
                _actions[KeyCode.F5] = InputSystem.actions.FindAction("F5");
                _actions[KeyCode.F6] = InputSystem.actions.FindAction("F6");
                _actions[KeyCode.Escape] = InputSystem.actions.FindAction("Escape");
                _actions[KeyCode.Q] = InputSystem.actions.FindAction("CastSpell");
                _actions[KeyCode.T] = InputSystem.actions.FindAction("TrackSkill");
                _actions[KeyCode.R] = InputSystem.actions.FindAction("FlyUp");
                _actions[KeyCode.V] = InputSystem.actions.FindAction("FlyDown");
                _actions[KeyCode.E] = InputSystem.actions.FindAction("ToggleMouseLook");
                _actions[KeyCode.Alpha0] = InputSystem.actions.FindAction("0");
                _actions[KeyCode.Alpha1] = InputSystem.actions.FindAction("1");
                _actions[KeyCode.Alpha2] = InputSystem.actions.FindAction("2");
                _actions[KeyCode.Alpha3] = InputSystem.actions.FindAction("3");
                _actions[KeyCode.Alpha4] = InputSystem.actions.FindAction("4");
                _actions[KeyCode.Alpha5] = InputSystem.actions.FindAction("5");
                _actions[KeyCode.Alpha6] = InputSystem.actions.FindAction("6");
                _actions[KeyCode.Alpha7] = InputSystem.actions.FindAction("7");
                _actions[KeyCode.Alpha8] = InputSystem.actions.FindAction("8");
                _actions[KeyCode.Alpha9] = InputSystem.actions.FindAction("9");
            }
        }

        private void Update()
        {
            UpdateDevices(_playerInput.currentControlScheme);
        }
        
        private void OnDestroy()
        {
            _instance = null;
            DisableGyroscopeSupport();
            DisableEnhancedTouchSupport();
        }

        public static void ResetAllOverrides()
        {
            if (_instance._playerInputActionAsset != null)
            {
                foreach (var inputAction in _instance._playerInputActionAsset)
                {
                    inputAction.RemoveAllBindingOverrides();
                }
            }
        }
        
        public static bool IsPressed(KeyCode keyCode)
        {
            return _actions.TryGetValue(keyCode, out var result) && (result.IsPressed());
        }

        public static bool OnKeyDown(KeyCode keyCode)
        {
            return _actions.TryGetValue(keyCode, out var result) && result.WasPressedThisFrame();
        }

        public static bool OnKeyUp(KeyCode keyCode)
        {
            return _actions.TryGetValue(keyCode, out var result) && (result.WasReleasedThisFrame());
        }

        private static void EnableGyroscopeSupport()
        {
            var gyro = UnityEngine.InputSystem.Gyroscope.current;

            if (EnableGyroscope && gyro != null)
            {
                InputSystem.EnableDevice(gyro);
            }
        }

        private static void DisableGyroscopeSupport()
        {
            var gyro = UnityEngine.InputSystem.Gyroscope.current;

            if (gyro != null)
            {
                InputSystem.DisableDevice(gyro);
            }
        }
        
        private static void EnableEnhancedTouchSupport()
        {
#if !UNITY_EDITOR
            EnhancedTouchSupport.Enable();
#endif
        }

        private static void DisableEnhancedTouchSupport()
        {
#if !UNITY_EDITOR
            EnhancedTouchSupport.Disable();
#endif
        }

        private static void UpdateDevices(string currentSchemeName)
        {
            var currentInputType = CurrentInputType;
            
            switch (currentSchemeName)
            {
                case TouchSchemeName:
                    currentInputType = InputType.Touch;
                    break;
                case KeyboardMouseSchemeName:
                    currentInputType = InputType.KeyboardMouse;
                    break;
                case GamepadSchemeName:
                    currentInputType = InputType.Gamepad;
                    break;
            }
            
            OnDeviceChanged(currentInputType);
        }

        private static void OnDeviceChanged(InputType inputType)
        {
            if (CurrentInputType != inputType)
            {
                CurrentInputType = inputType;
                OnInputTypeChanged?.Invoke(inputType);
            }
        }
    }
}