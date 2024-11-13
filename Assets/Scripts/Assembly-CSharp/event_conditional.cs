public class event_conditional : event_base
{
	public override void Process()
	{
		event_base.Executing = CheckCondition();
	}

	public override bool CheckCondition()
	{
		bool flag = RawData[4] == '\u0001';
		bool flag2 = false;
		int num = RawData[3];
		int num2 = RawData[8];
		flag2 = ((!flag) ? (Quest.instance.variables[num] == num2) : (Quest.instance.QuestVariables[num] == num2));
		return flag2 && xclocktest() && LevelTest();
	}

	public override void PostEvent()
	{
	}

	public override string EventName()
	{
		return "Conditional";
	}

	public override string summary()
	{
		int num = RawData[4];
		int num2 = RawData[3];
		int num3 = RawData[8];
		return base.summary() + "\n\t\tIsQuest=" + num + ",Variable=" + num2 + ",TargetValue=" + num3;
	}
}
