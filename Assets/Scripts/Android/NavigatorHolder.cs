using System;
using UnityEngine;

namespace UnderworldExporter.Game
{
    sealed class NavigatorHolder : MonoBehaviour
    {
        public static event Action OnRootViewPushed;
        public static event Action OnRootViewClosed ;
        
        [SerializeField] private Transform _viewToPush;
        [SerializeField] private Transform _viewsHolder;
        [SerializeField] private bool _pushViewOnStart;

        private Type _viewToPushType;
        private Navigator _navigator;

        private void Start()
        {
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
            OnRootViewPushed = null;
            OnRootViewClosed = null;
        }

        private void PushView()
        {
            UWCharacter.InteractionMode = UWCharacter.InteractionModeOptions;
            _navigator.PushView(_viewToPush.GetComponent<IView>().GetType());
            OnRootViewPushed?.Invoke();
        }
    }
}