using UnityEngine;

public class a_check_variable_trap : a_variable_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		bool flag = false;
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (base.xpos)
			{
			case 3:
				if (base.zpos - 16 >= 0)
				{
					flag = Check_Variables(Quest.instance.x_clocks, base.zpos - 16, base.heading, this, "xclocks");
					break;
				}
				Debug.Log("Ignored Xclock:" + base.zpos + " at " + objInt().objectloaderinfo.index);
				break;
			case 4:
				flag = Check_Variables(Quest.instance.variables, base.zpos, base.heading, this, "gamevars");
				break;
			case 5:
				flag = Check_Variables(Quest.instance.BitVariables, base.zpos, base.heading, this, "bitvars");
				break;
			case 6:
				flag = Check_Variables(Quest.instance.QuestVariables, base.zpos, base.heading, this, "questvars");
				break;
			default:
				Debug.Log("unknown usage of check trap " + base.xpos + " " + base.name);
				break;
			}
		}
		else
		{
			flag = Check_Variables(Quest.instance.variables, base.zpos, base.heading, this, "gamevars");
		}
		if (flag)
		{
			TriggerNext(triggerX, triggerY, State);
			PostActivate(src);
		}
		else
		{
			if (!(UWEBase._RES == "UW2"))
			{
				return;
			}
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
			if (objectIntAt != null && objectIntAt.GetItemType() == 109)
			{
				ObjectInteraction objectIntAt2 = ObjectLoader.getObjectIntAt(objectIntAt.next);
				if (objectIntAt2 != null && objectIntAt2.GetComponent<trap_base>() != null)
				{
					objectIntAt2.GetComponent<trap_base>().Activate(this, triggerX, triggerY, State);
					PostActivate(src);
				}
			}
		}
	}

	private int ComparisonValue()
	{
		int num = 0;
		for (int i = base.zpos; i <= base.zpos + base.heading; i++)
		{
			if (base.xpos != 0)
			{
				num += Quest.instance.variables[i];
				continue;
			}
			num <<= 3;
			num |= Quest.instance.variables[i] & 7;
		}
		return num;
	}

	public override bool Activate(object_base src, int triggerX, int triggerY, int State)
	{
		ExecuteTrap(this, triggerX, triggerY, State);
		return true;
	}

	private static bool Check_Variables(int[] vars, int index, int operation, a_check_variable_trap trap, string debugname)
	{
		if (operation != 0)
		{
			int num = trap.ComparisonValue();
			if (num == trap.VariableValue())
			{
				Debug.Log(debugname + " cmp = " + num + " value=" + trap.VariableValue());
			}
			return num == trap.VariableValue();
		}
		Debug.Log(debugname + ": Comparing " + trap.VariableValue() + " to variable " + index + " which is currently =" + vars[index]);
		return trap.VariableValue() == vars[index];
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
