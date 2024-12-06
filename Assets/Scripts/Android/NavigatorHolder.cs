using System;
using UnityEngine;

namespace UnderworldExporter.Game
{
    sealed class NavigatorHolder : MonoBehaviour
    {
        [SerializeField] private Transform _viewToPush;
        [SerializeField] private Transform _viewsHolder;
        [SerializeField] private bool _pushViewOnStart;

        private Type _viewToPushType;
        private int _characterInteractionMode;
        private Navigator _navigator;

        private void Start()
        {
            _viewToPushType = _viewToPush.GetComponent<IView>().GetType();
            
            _navigator = new Navigator(_viewsHolder);

            _navigator.OnPopView += view =>
            {
                if (view.GetType() == _viewToPushType)
                {
                    UWCharacter.InteractionMode = _characterInteractionMode;
                }
            };

            if (_pushViewOnStart)
            {
                PushView();
            }
        }

        private void PushView()
        {
            _characterInteractionMode = UWCharacter.InteractionMode;
            UWCharacter.InteractionMode = UWCharacter.InteractionModeOptions;
            _navigator.PushView(_viewToPush.GetComponent<IView>().GetType());
        }
    }
}