public class ClearRunes : GuiBase
{
	public void OnClick()
	{
		if (UWCharacter.Instance != null)
		{
			UWCharacter.Instance.PlayerMagic.ActiveRunes[0] = -1;
			UWCharacter.Instance.PlayerMagic.ActiveRunes[1] = -1;
			UWCharacter.Instance.PlayerMagic.ActiveRunes[2] = -1;
			ActiveRuneSlot.UpdateRuneSlots();
		}
	}
}
