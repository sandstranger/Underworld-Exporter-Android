public class event_always : event_base
{
	public override void Process()
	{
		if (CheckCondition())
		{
			ExecuteEvent();
		}
	}

	public override string EventName()
	{
		return "event_always";
	}
}
