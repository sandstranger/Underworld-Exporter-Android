public class TradeIndicator : GuiBase
{
	public TradeSlot ts;

	public void OnClick()
	{
		ts.Selected = !ts.Selected;
	}
}
