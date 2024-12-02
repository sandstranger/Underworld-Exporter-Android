using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

namespace UnderworldExporter.Game
{
    sealed class VirtualMouseUI : MonoBehaviour
    {
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private VirtualMouseInput virtualMouseInput;

        private void Start()
        {
            UpdateLocalScale();
            InputManager.OnKeyRebindStarted += OnKeyRebindStarted;

            InputManager.OnInputTypeChanged += UpdateVisibility;
            UpdateVisibility(InputManager.CurrentInputType);
        }

        private void OnDestroy()
        {
            InputManager.OnKeyRebindStarted -= OnKeyRebindStarted;
            InputManager.OnInputTypeChanged -= UpdateVisibility;
        }
        
        private void Update()
        {
#if UNITY_EDITOR            
            UpdateLocalScale();
#endif            
        }

        private void LateUpdate()
        {
            Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
            virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
            virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0f, Screen.height);
            InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
        }
        
        private void UpdateLocalScale() => transform.localScale = Vector3.one * (1f / canvasRectTransform.localScale.x);

        private void UpdateVisibility(InputManager.InputType inputType) => gameObject.SetActive(inputType == InputManager.InputType.Gamepad);

        private async void OnKeyRebindStarted(bool isRebindActive)
        {
            if (isRebindActive)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(0.1f));
                UpdateVisibility(InputManager.CurrentInputType);
            }
        }
    }
}