public class event_set_race_attitude : event_base
{
	public override void ExecuteEvent()
	{
		base.ExecuteEvent();
		int race = RawData[4];
		int num = RawData[5];
		NPC[] array = findRace(race);
		if (array != null)
		{
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				array[i].npc_attitude = (short)num;
			}
		}
	}

	public override string EventName()
	{
		return "set_race_attitude";
	}

	public override string summary()
	{
		return base.summary() + "\n\t\tRace=" + (int)RawData[4] + ",Attitude=" + (int)RawData[5];
	}
}
