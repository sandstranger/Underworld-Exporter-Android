namespace UnderworldExporter.Game
{
    public interface IView
    {
        void Initialize(IPresenter presenter, Navigator navigator);

        void DestroyView();
    }
}