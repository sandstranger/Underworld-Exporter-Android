using UnityEngine;

public class event_set_npc_props : event_base
{
	public override void ExecuteEvent()
	{
		base.ExecuteEvent();
		int num = RawData[5];
		int num2 = RawData[3];
		int num3 = 0;
		int num4 = 0;
		NPC[] array;
		if (num == 0)
		{
			num = RawData[7];
			array = findNPC(num);
			num3 = RawData[8];
			num4 = RawData[9];
		}
		else
		{
			array = findNPC(num);
			num3 = RawData[6];
			num4 = RawData[7];
		}
		if (array != null)
		{
			Vector3 tileVector = UWClass.CurrentTileMap().getTileVector(num3, num4);
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				array[i].transform.position = tileVector;
				array[i].npc_xhome = (short)num3;
				array[i].npc_yhome = (short)num4;
				array[i].npc_goal = (short)num2;
			}
		}
	}

	public override string EventName()
	{
		return "set_npc_props";
	}

	public override string summary()
	{
		if (RawData[5] == '\0')
		{
			return base.summary() + "\n\t\tWhoAmI=" + (int)RawData[7] + ",Goal=" + (int)RawData[3] + ",HomeX=" + (int)RawData[8] + ",HomeY=" + (int)RawData[9];
		}
		return base.summary() + "\n\t\tWhoAmI=" + (int)RawData[5] + ",Goal=" + (int)RawData[3] + ",HomeX=" + (int)RawData[6] + ",HomeY=" + (int)RawData[7];
	}
}
