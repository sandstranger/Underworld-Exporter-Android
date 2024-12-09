using System;
using UnityEngine;

namespace UnderworldExporter.Game
{
    sealed class NavigatorHolder : MonoBehaviour
    {
        public static event Action OnRootViewPushed;
        public static event Action OnRootViewClosed ;

        public static bool HasAnyView => _instance._navigator.HasAnyView;
        
        private static NavigatorHolder _instance;

        [SerializeField] private Transform _viewToPush;
        [SerializeField] private Transform _viewsHolder;
        [SerializeField] private bool _pushViewOnStart;

        private Type _viewToPushType;
        private Navigator _navigator;

        private void Awake()
        {
            _instance = this;
            _viewToPushType = _viewToPush.GetComponent<IView>().GetType();
            
            _navigator = new Navigator(_viewsHolder);

            _navigator.OnPopView += view =>
            {
                if (view.GetType() == _viewToPushType)
                {
                    OnRootViewClosed?.Invoke();
                }
            };

            if (_pushViewOnStart)
            {
                PushView();
            }
        }

        private void OnDestroy()
        {
            _instance = null;
            OnRootViewPushed = null;
            OnRootViewClosed = null;
        }

        public void PushView()
        {
            _navigator.PushView(_viewToPushType);
            OnRootViewPushed?.Invoke();
        }
    }
}