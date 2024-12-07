using System;
using UnityEngine;

namespace UnderworldExporter.Game
{
    public abstract class View<TPresenter> : MonoBehaviour, IView where TPresenter : IPresenter
    {
        protected Navigator Navigator { get; private set; }
        protected TPresenter Presenter { get; private set; }

        public void Initialize(IPresenter presenter, Navigator navigator)
        {
            Presenter = (TPresenter) presenter;
            this.Navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            OnViewInitialized();
        }

        public void DestroyView()
        {
            OnViewDestroyed();
            Destroy(gameObject);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            OnViewEnabled();
        }

        private void OnDisable()
        {
            OnViewDisabled();
        }

        private void Update()
        {
            if (InputManager.EscapeKeyPressed() && this.transform.IsLastSibling())
            {
                OnBackButtonPressed();
            }
        }

        protected virtual void OnBackButtonPressed()
        {
            Navigator.PopView(this);
        }
        
        protected virtual void OnViewInitialized()
        {
        }


        protected virtual void OnViewDestroyed()
        {
        }

        protected virtual void OnViewEnabled()
        {
        }

        protected virtual void OnViewDisabled()
        {
        }
    }
}