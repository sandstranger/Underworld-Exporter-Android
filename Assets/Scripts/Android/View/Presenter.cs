using UnityEngine.Scripting;

namespace UnderworldExporter.Game
{
    [Preserve]
    public abstract class Presenter<TView> : IPresenter where TView : IView
    {
        protected readonly TView View;

        protected Presenter(IView view)
        {
            if (view is TView tView)
            {
                View = tView;
            }
        }
    }
}