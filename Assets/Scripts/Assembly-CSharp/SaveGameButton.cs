using UnityEngine;
using UnityEngine.UI;

public class SaveGameButton : GuiBase
{
	public MainMenuHud SubmitTarget;

	public int slotNo;

	public static Color32 On = new Color32(byte.MaxValue, 213, 64, byte.MaxValue);

	public static Color32 Off = new Color32(187, 123, 1, byte.MaxValue);

	public Text label;

	public override void Start()
	{
		base.Start();
		label.color = Off;
	}

	public void OnHoverEnter()
	{
		label.color = On;
	}

	public void OnHoverExit()
	{
		label.color = Off;
	}

	public virtual void OnClick()
	{
		SubmitTarget.LoadSave(slotNo);
	}
}
