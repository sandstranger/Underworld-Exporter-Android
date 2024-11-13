using UnityEngine;

public class InteractionModeControlItem : GuiBase
{
	public int InteractionMode;

	public bool isOn;

	public InteractionModeControl imc;

	public KeyCode ShortCutKey;

	public void OnClick()
	{
		if (!(UWEBase.CurrentObjectInHand != null) && !ConversationVM.InConversation && !WindowDetect.WaitingForInput)
		{
			if (isOn && InteractionMode != 0)
			{
				isOn = false;
				InteractionModeControl.UpdateNow = true;
				Character.InteractionMode = Character.DefaultInteractionMode;
			}
			else
			{
				isOn = true;
				imc.TurnOffOthers(InteractionMode);
				InteractionModeControl.UpdateNow = true;
				Character.InteractionMode = InteractionMode;
			}
			UWHUD.instance.EnableDisableControl(UWHUD.instance.InteractionControlUW2BG.gameObject, UWEBase._RES == "UW2" && Character.InteractionMode == 0);
		}
	}

	public override void Update()
	{
		base.Update();
		if (Input.GetKeyUp(ShortCutKey))
		{
			OnClick();
		}
	}
}
