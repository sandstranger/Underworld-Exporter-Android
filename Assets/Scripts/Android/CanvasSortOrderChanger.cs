using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class CanvasSortOrderChanger : MonoBehaviour
    {
        private const string ChangeSortingOrderKey = "change_touch_camera_sorting_order";
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private int _targetSortOrderValue;
        private int _initialSortOrderValue;
        private bool _changeSortingOrder;
        
        public static bool ChangeSortingOrder
        {
            get => PlayerPrefsExtensions.GetBool(ChangeSortingOrderKey, true);
            set => PlayerPrefsExtensions.SetBool(ChangeSortingOrderKey, value);
        }

        private void Awake()
        {
            _changeSortingOrder = ChangeSortingOrder;
            _initialSortOrderValue = _canvas.sortingOrder;
        }

        public void ChangeSortOrderToDefault()
        {
            if (_changeSortingOrder)
            {
                _canvas.sortingOrder = _initialSortOrderValue;
            }
        }

        public void ChangeSortOrder()
        {
            if (_changeSortingOrder)
            {
                _canvas.sortingOrder =
                    UWCharacter.Instance.MouseLookEnabled ? _targetSortOrderValue : _initialSortOrderValue;
            }
        }
    }
}