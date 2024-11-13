public class event_set_goal : event_always
{
	public override void ExecuteEvent()
	{
		base.ExecuteEvent();
		bool flag = RawData[3] == '\0';
		int num = RawData[4];
		int num2 = RawData[5];
		int num3 = RawData[6];
		NPC[] array = null;
		array = ((!flag) ? findRace(num) : findNPC(num));
		if (array != null)
		{
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				array[i].npc_goal = (short)num2;
				array[i].npc_gtarg = (short)num3;
			}
		}
	}

	public override string EventName()
	{
		return "Set_Goal";
	}

	public override string summary()
	{
		if (RawData[3] == '\0')
		{
			return base.summary() + "\n\t\tWhoAmI=" + (int)RawData[4] + ",Goal=" + (int)RawData[5] + ",GTarg=" + (int)RawData[6];
		}
		return base.summary() + "\n\t\tRace=" + (int)RawData[4] + ",Goal=" + (int)RawData[5] + ",GTarg=" + (int)RawData[6];
	}
}
