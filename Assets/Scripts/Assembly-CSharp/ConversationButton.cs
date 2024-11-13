using TMPro;

public class ConversationButton : GuiBase
{
	public int option;

	public void OnClick()
	{
		if (ConversationVM.WaitingForInput)
		{
			ConversationVM.instance.CheckAnswer(option);
		}
	}

	public void SetText(string text)
	{
		GetComponent<TextMeshProUGUI>().text = text;
	}
}
