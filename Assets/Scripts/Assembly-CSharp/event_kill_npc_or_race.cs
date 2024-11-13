public class event_kill_npc_or_race : event_base
{
	public override void ExecuteEvent()
	{
		base.ExecuteEvent();
		NPC[] array = null;
		if (RawData[5] == '\u0001')
		{
			int whoAmI = RawData[4];
			array = findNPC(whoAmI);
		}
		else
		{
			int race = RawData[4];
			array = findRace(race);
		}
		if (array != null)
		{
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				array[i].npc_hp = -1;
			}
		}
	}

	public override string EventName()
	{
		return "Kill_NPC_Or_Race";
	}

	public override string summary()
	{
		if (RawData[5] == '\u0001')
		{
			return base.summary() + "\n\t\tIsNPC=" + (int)RawData[5] + ",WhoAmI=" + (int)RawData[4];
		}
		return base.summary() + "\n\t\tIsNPC=" + (int)RawData[5] + ",Race=" + (int)RawData[4];
	}
}
