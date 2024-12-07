using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnderworldExporter.Game
{
    public sealed class Navigator
    {
        private readonly Transform _viewsParent;
        private readonly Dictionary<Type, IView> _views = new();
        private readonly Type[] _presentersTypes;

        public event Action<IView> OnPopView;

        public bool HasAnyView => _views.Count > 0;
        
        public Navigator(Transform viewsParent)
        {
            if (viewsParent == null)
            {
                throw new ArgumentNullException(nameof(viewsParent));
            }

            _viewsParent = viewsParent;

            Type presenterType = typeof(IPresenter);

            _presentersTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => presenterType.IsAssignableFrom(type)).ToArray();
        }

        public void PushView<TView>() where TView : IView
        {
            PushView(typeof(TView));
        }
        
        public void PushView(Type viewType)
        {
            var go = Object.Instantiate(Resources.Load<GameObject>($"Views/{viewType.Name}"), _viewsParent, false);
            go.transform.SetAsLastSibling();
            var view = go.GetComponent<IView>();
            view.Initialize( (IPresenter) Activator.CreateInstance(GetPresenterType(viewType), view), this);
            _views[viewType] = view;
        }
        
        public void PopView<TView>() where TView : IView
        {
            var type = typeof(TView);
            var view = _views[type];
            view.DestroyView();
            _views.Remove(type);
            OnPopView?.Invoke(view);
        }
        
        public void PopView(IView view)
        {
            view.DestroyView();
            _views.Remove(view.GetType());
            OnPopView?.Invoke(view);
        }

        private Type GetPresenterType(Type type)
        {
            foreach (var presenterType in _presentersTypes)
            {
                var baseType = presenterType.BaseType;

                if (baseType is { IsGenericType: true } && baseType.GenericTypeArguments[0] == (type))
                {
                    return presenterType;
                }
            }

            return default;
        }
    }
}