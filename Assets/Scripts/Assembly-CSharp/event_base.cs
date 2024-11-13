using UnityEngine;

public class event_base : UWClass
{
	public const int RowTypeSetNPCGoal = 1;

	public const int RowTypeMoveNPC = 2;

	public const int RowTypeKillNPC = 3;

	public const int RowTypeFireTriggers = 5;

	public const int RowTypeCondition = 10;

	public const int RowTypeRemoveNPC = 245;

	public const int RowTypeRaceAttidude = 248;

	public const int RowTypeSetProps = 249;

	public const int RowTypeScheduled = 251;

	public const int RowTypeKillNPCorRace = 253;

	public const int RowTypePlaceNPC = 254;

	public const int RowTypeSetNPCGOAL_Alt = 255;

	public char[] RawData = new char[16];

	public int BlockNo;

	public int type;

	public int LevelNo = 0;

	public int x_clock = 0;

	public bool clear = false;

	public static bool Executing = false;

	public virtual void Process()
	{
		if (Executing && CheckCondition())
		{
			ExecuteEvent();
			PostEvent();
		}
	}

	public virtual void PostEvent()
	{
		if (LevelNo <= 80)
		{
			clear = true;
		}
	}

	public virtual string ReportEventDetails()
	{
		return "\tEvent Type = " + type + " " + EventName() + summary() + "\n\t\t" + GetRawData();
	}

	public virtual string EventName()
	{
		return "Base";
	}

	protected string GetRawData()
	{
		string text = "";
		for (int i = 0; i <= RawData.GetUpperBound(0); i++)
		{
			text = text + (int)RawData[i] + ",";
		}
		return text;
	}

	public virtual string summary()
	{
		return "\n\t\tLevelNo=" + LevelNo + "\n\t\tXclock=" + x_clock;
	}

	public void InitRawData(int blockNo, int add_ptr, char[] fileData)
	{
		BlockNo = blockNo;
		for (int i = 0; i <= RawData.GetUpperBound(0); i++)
		{
			RawData[i] = fileData[add_ptr + i];
		}
		LevelNo = RawData[0] - 1;
		type = RawData[2];
		x_clock = RawData[14];
	}

	public virtual bool CheckCondition()
	{
		return LevelTest() && xclocktest();
	}

	public virtual void ExecuteEvent()
	{
		Debug.Log("Event type :" + type);
	}

	public bool IsLevel(int levelNoToTest)
	{
		if (levelNoToTest == LevelNo)
		{
			return true;
		}
		switch (LevelNo)
		{
		case 246:
			return levelNoToTest >= 8 && levelNoToTest <= 15;
		case 240:
			return levelNoToTest >= 32 && levelNoToTest <= 33;
		case 254:
			return levelNoToTest >= 56 && levelNoToTest <= 58;
		default:
			return false;
		}
	}

	public bool xclocktest()
	{
		if (x_clock == 0)
		{
			return true;
		}
		return Quest.instance.x_clocks[BlockNo] == x_clock;
	}

	public bool LevelTest()
	{
		return IsLevel(GameWorldController.instance.LevelNo);
	}

	public NPC[] findNPC(int WhoAmI)
	{
		int num = 0;
		ObjectLoaderInfo[] objInfo = UWClass.CurrentObjectList().objInfo;
		for (int i = 0; i <= 256; i++)
		{
			if (objInfo[i].instance != null && (bool)objInfo[i].instance.GetComponent<NPC>() && objInfo[i].instance.GetComponent<NPC>().npc_whoami == WhoAmI)
			{
				objInfo[i].instance.GetComponent<NPC>().objInt().UpdatePosition();
				if (objInfo[i].instance.ObjectTileX != 99)
				{
					num++;
				}
			}
		}
		if (num != 0)
		{
			NPC[] array = new NPC[num];
			int num2 = 0;
			for (int j = 0; j <= 256; j++)
			{
				if (objInfo[j].instance != null && (bool)objInfo[j].instance.GetComponent<NPC>() && objInfo[j].instance.GetComponent<NPC>().npc_whoami == WhoAmI && objInfo[j].instance.ObjectTileX != 99)
				{
					array[num2++] = objInfo[j].instance.GetComponent<NPC>();
				}
			}
			return array;
		}
		return null;
	}

	public NPC[] findRace(int Race)
	{
		int num = 0;
		ObjectLoaderInfo[] objInfo = UWClass.CurrentObjectList().objInfo;
		for (int i = 0; i <= 256; i++)
		{
			if (objInfo[i].instance != null && (bool)objInfo[i].instance.GetComponent<NPC>() && objInfo[i].instance.GetComponent<NPC>().GetRace() == Race)
			{
				objInfo[i].instance.GetComponent<NPC>().objInt().UpdatePosition();
				if (objInfo[i].instance.ObjectTileX != 99)
				{
					num++;
				}
			}
		}
		if (num != 0)
		{
			NPC[] array = new NPC[num];
			int num2 = 0;
			for (int j = 0; j <= 256; j++)
			{
				if (objInfo[j].instance != null && (bool)objInfo[j].instance.GetComponent<NPC>() && objInfo[j].instance.GetComponent<NPC>().GetRace() == Race && objInfo[j].instance.ObjectTileX != 99)
				{
					array[num2++] = objInfo[j].instance.GetComponent<NPC>();
				}
			}
			return array;
		}
		return null;
	}
}
