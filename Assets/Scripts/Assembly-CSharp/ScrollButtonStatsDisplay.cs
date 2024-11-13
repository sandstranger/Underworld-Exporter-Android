public class ScrollButtonStatsDisplay : Scrollbutton
{
	private int PreviousValue = -1;

	public int stepSize;

	public static int ScrollValue = 0;

	public int MaxScrollValue;

	public int MinScrollValue;

	public void OnClick()
	{
		ScrollValue += stepSize;
		if (ScrollValue > MaxScrollValue)
		{
			ScrollValue = MaxScrollValue;
		}
		if (ScrollValue < MinScrollValue)
		{
			ScrollValue = MinScrollValue;
		}
	}

	public override void Update()
	{
		base.Update();
		if (PreviousValue != ScrollValue)
		{
			PreviousValue = ScrollValue;
			StatsDisplay.Offset = ScrollValue;
			StatsDisplay.UpdateNow = true;
		}
	}
}
