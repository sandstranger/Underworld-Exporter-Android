using UnityEngine;

public class a_set_variable_trap : a_variable_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (UWEBase._RES == "UW2")
		{
			switch (base.xpos)
			{
			case 1:
				Set_Variables(Quest.instance.BitVariables, base.zpos, base.heading, this, "bitvars");
				break;
			case 0:
				Set_Variables(Quest.instance.variables, base.zpos, base.heading, this, "gamevars");
				break;
			case 2:
				Set_Variables(Quest.instance.QuestVariables, base.zpos, base.heading, this, "questvars");
				break;
			case 3:
				if (base.zpos - 16 >= 0)
				{
					Set_Variables(Quest.instance.x_clocks, base.zpos - 16, base.heading, this, "xclocks");
					break;
				}
				Debug.Log("Ignored Xclock:" + base.zpos + " at " + objInt().objectloaderinfo.index);
				break;
			default:
				Debug.Log("unknown usage of set trap " + base.xpos + " " + base.name);
				break;
			}
		}
		else
		{
			Set_Variables(Quest.instance.variables, base.zpos, base.heading, this, "gamevars");
		}
	}

	private static void Set_Variables(int[] vars, int index, int operation, a_set_variable_trap trap, string debugname)
	{
		string text = "";
		int num = 0;
		if (index != 0)
		{
			num = vars[index];
			switch (operation)
			{
			case 0:
				vars[index] += trap.VariableValue();
				text = "add";
				break;
			case 1:
				vars[index] -= trap.VariableValue();
				text = "Sub";
				break;
			case 2:
				vars[index] = trap.VariableValue();
				text = "Set";
				break;
			case 3:
				vars[index] &= trap.VariableValue();
				text = "And";
				break;
			case 4:
				vars[index] |= trap.VariableValue();
				text = "or";
				break;
			case 5:
				vars[index] ^= trap.VariableValue();
				text = "xor";
				break;
			case 6:
				vars[index] = (vars[index] * (2 * trap.VariableValue())) & 0x3F;
				text = "shl";
				break;
			}
			Debug.Log(debugname + ": Operation + " + text + " Variable " + index + " was " + num + " now =" + vars[index] + " using varvalue" + trap.VariableValue() + " trap " + trap.objInt().objectloaderinfo.index);
		}
		else
		{
			Debug.Log("Bitwise set variable. Not implemented yet");
			switch (operation)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			}
		}
	}

	public override int VariableValue()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			return base.owner;
		}
		return ((base.quality & 0x3F) << 8) | (((base.owner & 0x1F) << 3) | (base.ypos & 7));
	}
}
