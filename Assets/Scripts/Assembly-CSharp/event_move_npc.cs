using UnityEngine;

public class event_move_npc : event_base
{
	public override void ExecuteEvent()
	{
		base.ExecuteEvent();
		int num = RawData[3];
		int num2 = RawData[4];
		int whoAmI = RawData[6];
		Vector3 tileVector = UWClass.CurrentTileMap().getTileVector(num, num2);
		NPC[] array = findNPC(whoAmI);
		if (array != null)
		{
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				array[i].transform.position = tileVector;
				array[i].npc_xhome = (short)num;
				array[i].npc_yhome = (short)num2;
			}
		}
	}

	public override string EventName()
	{
		return "Move_NPC";
	}

	public override string summary()
	{
		return base.summary() + "\n\t\tTileX=" + (int)RawData[3] + ",TileY=" + (int)RawData[4] + "WhoAmI=" + (int)RawData[6];
	}
}
