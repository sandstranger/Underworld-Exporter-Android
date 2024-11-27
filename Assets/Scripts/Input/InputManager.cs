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
        private const string VirtualMouseId = "VirtualMouse";

        public const int FakeMouseButtonId = 2980;
        public const int LeftMouseButtonId = 0;
        public const int RightMouseButtonId = 1;

        public static event Action<InputType> OnInputTypeChanged;

#if UNITY_EDITOR
        public static InputType CurrentInputType { get; private set; } = InputType.KeyboardMouse;
#else
        public static InputType CurrentInputType { get; private set; } = InputType.Touch;
#endif

        public static Vector2 Move => _instance._moveAction.ReadValue<Vector2>();

        public static Vector2 Look => _instance._lookAction.ReadValue<Vector2>();

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
                    case InputType.Unknown:
                        return Vector2.zero;
                        break;
                }

                return default;
            }
        }

        private static InputManager _instance;
        private static readonly Dictionary<KeyCode, InputAction> _actions = new();
        private static Vector2 _lastTouchPosition = Vector2.zero;

        [SerializeField] private Transform _virtualMouse;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _lookAction;

        public enum InputType
        {
            KeyboardMouse,
            Gamepad,
            Touch,
            Unknown
        }

        private void Awake()
        {
            _instance = this;
            UpdateDevices();
            EnableEnhancedTouchSupport();
            InputSystem.onDeviceChange += OnDeviceChanged;

            _moveAction = InputSystem.actions.FindAction("Move");
            _lookAction = InputSystem.actions.FindAction("Look");

            if (_actions.Count == 0)
            {
                _actions[KeyCode.Space] = InputSystem.actions.FindAction("Jump");
                _actions[KeyCode.Mouse0] = InputSystem.actions.FindAction("LeftMouseClick");
                _actions[KeyCode.Mouse1] = InputSystem.actions.FindAction("RightMouseClick");
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

        private void OnDestroy()
        {
            _instance = null;
            DisableEnhancedTouchSupport();
            InputSystem.onDeviceChange -= OnDeviceChanged;
            StopAllCoroutines();
        }

        public static bool IsPressed(KeyCode keyCode)
        {
            return _actions.TryGetValue(keyCode, out var result) && (result.IsPressed());
        }

        public static bool OnKeyDown(KeyCode keyCode)
        {
            return _actions.TryGetValue(keyCode, out var result) &&
                   (result.IsPressed() && result.WasPressedThisFrame());
        }

        public static bool OnKeyUp(KeyCode keyCode)
        {
            return _actions.TryGetValue(keyCode, out var result) && (result.WasReleasedThisFrame());
        }

        private void EnableEnhancedTouchSupport()
        {
#if !UNITY_EDITOR
            EnhancedTouchSupport.Enable();
#endif
        }

        private void DisableEnhancedTouchSupport()
        {
#if !UNITY_EDITOR
            EnhancedTouchSupport.Disable();
#endif
        }

        private static void UpdateDevices()
        {
            var devices = InputSystem.devices;
            var isGamePadActive = devices.Any(device => device is Gamepad or UnityEngine.InputSystem.Joystick);

            if (isGamePadActive)
            {
                OnDeviceChanged(InputType.Gamepad);
                return;
            }

#if UNITY_EDITOR
            var isKeyboardActive = devices.Any(device => device is Mouse);

            if (isKeyboardActive)
            {
                OnDeviceChanged(InputType.KeyboardMouse);
            }
            
#else
            var isTouchActive = devices.Any(device => device is Touchscreen);

            if (isTouchActive)
            {
                OnDeviceChanged(InputType.Touch);
                return;
            }

           var isKeyboardActive = devices.Any(device => device is Mouse && !device.displayName.Contains(VirtualMouseId));

            if (isKeyboardActive)
            {
                OnDeviceChanged(InputType.KeyboardMouse);
            }
#endif
        }

        private static void OnDeviceChanged(InputType inputType)
        {
            if (CurrentInputType != inputType)
            {
                CurrentInputType = inputType;
                OnInputTypeChanged?.Invoke(inputType);
            }
        }

        private static void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            InputType currentInputType = CurrentInputType;

            if (device.displayName.Contains("VirtualMouse"))
            {
                return;
            }

            if (change == InputDeviceChange.Added)
            {
                switch (device)
                {
                    case Gamepad:
                        currentInputType = InputType.Gamepad;
                        break;
                    case Mouse:
                        currentInputType = InputType.KeyboardMouse;
                        break;
                }
            }
            else if (change == InputDeviceChange.Removed)
            {
                switch (device)
                {
#if UNITY_EDITOR
                    case UnityEngine.InputSystem.Joystick:
                    case Gamepad:
                        currentInputType = InputSystem.devices.Any(currentDevice => currentDevice is Mouse or Keyboard)
                            ? InputType.KeyboardMouse
                            : InputType.Unknown;
                        break;
                    case Mouse:
                    case Keyboard:
                        currentInputType =
                            InputSystem.devices.Any(currentDevice =>
                                currentDevice is Gamepad or UnityEngine.InputSystem.Joystick)
                                ? InputType.Gamepad
                                : InputType.Unknown;
                        break;
#else
                        case UnityEngine.InputSystem.Joystick:
                        case Gamepad:
                            currentInputType =
 InputSystem.devices.Any(currentDevice => currentDevice is Touchscreen) ? InputType.Touch : InputType.KeyboardMouse;
                            break;
                        case Mouse:
                        case Keyboard: 
                            currentInputType =
 InputSystem.devices.Any(currentDevice => currentDevice is Gamepad or UnityEngine.InputSystem.Joystick) ? InputType.Gamepad : InputType.Touch;
                            break;
#endif
                }

            }
            
            OnDeviceChanged(currentInputType);
        }
    }
}