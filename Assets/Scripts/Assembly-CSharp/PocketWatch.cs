public class PocketWatch : object_base
{
	public override bool use()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 39) + GameClock.hour() + ":" + GameClock.minute().ToString("d2"));
		return true;
	}
}
