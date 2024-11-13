public class event_set_goal_alt : event_set_goal
{
	public override void Process()
	{
		if (event_base.Executing)
		{
			base.Process();
		}
	}

	public override string EventName()
	{
		return "set_goal_alt";
	}
}
