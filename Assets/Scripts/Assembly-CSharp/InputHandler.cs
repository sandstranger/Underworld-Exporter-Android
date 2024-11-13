using UnityEngine;
using UnityEngine.UI;

public class InputHandler : GuiBase
{
	public const int InputCharacterQty = 0;

	public const int InputInventoryQty = 1;

	public const int InputConversationWords = 2;

	public const int InputMantraWords = 3;

	public const int InputAnvil = 4;

	public GameObject target;

	public int currentInputMode;

	public void OnSubmit()
	{
		if (!(target == null))
		{
			int quant = 0;
			string text = "";
			switch (currentInputMode)
			{
			case 0:
			case 1:
				quant = ParseInteger();
				break;
			case 2:
			case 3:
			case 4:
				text = ParseString();
				break;
			}
			switch (currentInputMode)
			{
			case 0:
				target.gameObject.GetComponent<UWCharacter>().OnSubmitPickup(quant);
				break;
			case 1:
				target.gameObject.GetComponent<InventorySlot>().OnSubmitPickup(quant);
				break;
			case 2:
				ConversationVM.OnSubmitPickup(text);
				break;
			case 3:
				target.gameObject.GetComponent<Shrine>().OnSubmitPickup(text);
				break;
			case 4:
				target.gameObject.GetComponent<Equipment>().OnSubmitPickup(text);
				break;
			}
		}
	}

	public int ParseInteger()
	{
		InputField inputControl = UWHUD.instance.InputControl;
		int result = 0;
		if (!int.TryParse(inputControl.text, out result))
		{
			return 0;
		}
		return result;
	}

	public string ParseString()
	{
		InputField inputControl = UWHUD.instance.InputControl;
		return inputControl.text;
	}
}
