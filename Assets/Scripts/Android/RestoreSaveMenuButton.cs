namespace UnderworldExporter.Game
{
    public sealed class RestoreSaveMenuButton : MenuButton
    {
        public override void OnSaveButtonClicked()
        {
            _optionsMenuControl.RestoreFromSlot(_saveSlot - 1);
        }
    }
}