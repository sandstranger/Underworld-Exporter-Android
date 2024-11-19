using UnityEngine;

namespace UnderworldExporter.Game
{
    public sealed class CanvasSortOrderChanger : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private int _targetSortOrderValue;
        private int _initialSortOrderValue;
      
        private void Awake() =>
            _initialSortOrderValue = _canvas.sortingOrder;

        public void ChangeSortOrderToDefault()
        {
            _canvas.sortingOrder = _initialSortOrderValue;
        }
        
        public void ChangeSortOrder() =>
            _canvas.sortingOrder = UWCharacter.Instance.MouseLookEnabled ? _targetSortOrderValue : _initialSortOrderValue;
    }
}