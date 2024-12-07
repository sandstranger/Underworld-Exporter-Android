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
        private readonly ViewsList _views = new();
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

            if (_views.Count > 0)
            {
                _views[^1].Disable();
            }
            
            _views.Add(view);
        }
        
        public void PopView<TView>() where TView : IView
        {
            var type = typeof(TView);

            if (_views.TryGetView(type, out var view, out var index))
            {
                view.DestroyView();

                if (index > 0)
                {
                    _views[index - 1].Enable();
                }
                _views.Remove(view);
            }
            
            OnPopView?.Invoke(view);
        }
        
        public void PopView(IView view)
        {
            view.DestroyView();

            var viewIndex = _views.IndexOf(view);

            if (viewIndex > 0)
            {
                _views[viewIndex - 1].Enable();
            }

            _views.Remove(view);
            
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
        
     
        private sealed class ViewsList : List<IView>
        {
            public bool TryGetView(Type type, out IView view , out int index)
            {
                view = this.FirstOrDefault(item => item.GetType() == type);

                if (view != null)
                {
                    index = this.IndexOf(view);
                    return true;
                }

                index = -1;
                return false;
            }
        }
    }
}