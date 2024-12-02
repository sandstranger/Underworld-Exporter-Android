using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class CanvasSortOrderChanger : MonoBehaviour
    {
        private const string ChangeSortingOrderKey = "change_touch_camera_sorting_order";
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private int _targetSortOrderValue;
        private int _initialSortOrderValue;
        
        private void Awake()
        {
            _initialSortOrderValue = _canvas.sortingOrder;
        }

        public void ChangeSortOrderToDefault()
        {
            if (GameModel.CurrentModel.PreferFullScreenTouchCameraInMouseMode)
            {
                _canvas.sortingOrder = _initialSortOrderValue;
            }
        }

        public void ChangeSortOrder()
        {
            if (GameModel.CurrentModel.PreferFullScreenTouchCameraInMouseMode)
            {
                _canvas.sortingOrder =
                    UWCharacter.Instance.MouseLookEnabled ? _targetSortOrderValue : _initialSortOrderValue;
            }
        }
    }
}