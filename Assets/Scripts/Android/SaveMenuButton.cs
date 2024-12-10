using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class SaveMenuButton : MenuButton
    {
        public override void OnSaveButtonClicked()
        {
            UWHUD.instance.MessageScroll.Set("Set save game name.");
            InputField inputctrl = UWHUD.instance.InputControl;
            inputctrl.text = SaveGame.SaveGameName(_saveSlot);
            inputctrl.gameObject.SetActive(true);
            inputctrl.gameObject.GetComponent<InputHandler>().target = this.gameObject;
            inputctrl.gameObject.GetComponent<InputHandler>().currentInputMode = InputHandler.SaveGame;
            WindowDetect.WaitingForInput = true;
        }

        public void OnTextSumbit(string text)
        {
            WindowDetect.WaitingForInput = false;
            _optionsMenuControl.SaveToSlot(_saveSlot - 1,text);
        }
    }
}