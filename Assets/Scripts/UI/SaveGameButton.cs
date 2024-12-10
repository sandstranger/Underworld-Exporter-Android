public class SaveGameButton : GuiBase {

		public MainMenuHud SubmitTarget;
		public int slotNo;

		public void OnClick()
		{
			SubmitTarget.LoadSave(slotNo);
		}

}
