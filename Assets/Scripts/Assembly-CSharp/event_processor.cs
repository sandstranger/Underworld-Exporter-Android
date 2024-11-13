using System.IO;
using UnityEngine;

public class event_processor : UWClass
{
	public struct event_block
	{
		public int[] eventheader;

		public event_base[] events;
	}

	public event_block[] events_blocks;

	public event_processor()
	{
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + GameWorldController.instance.SCD_Ark_File_Selected, out buffer))
		{
			Debug.Log(Loader.BasePath + GameWorldController.instance.SCD_Ark_File_Selected + " File not loaded");
			return;
		}
		int num = (int)DataLoader.getValAtAddress(buffer, 0L, 32);
		events_blocks = new event_block[num];
		for (int i = 0; i <= events_blocks.GetUpperBound(0); i++)
		{
			long num2 = 6L;
			int num3 = (int)DataLoader.getValAtAddress(buffer, num2 + num * 4 * 2 + i * 4, 32);
			num2 = i * 4 + 6;
			if ((int)DataLoader.getValAtAddress(buffer, num2, 32) == 0)
			{
				Debug.Log("No Scd.ark data for this level");
			}
			long valAtAddress = DataLoader.getValAtAddress(buffer, num2, 32);
			int num4 = 0;
			num2 = 0L;
			char[] array = new char[num3];
			for (long num5 = valAtAddress; num5 < valAtAddress + num3; num5++)
			{
				array[num4] = buffer[num5];
				num4++;
			}
			int num6 = 0;
			int num7 = (int)DataLoader.getValAtAddress(array, 0L, 8);
			if (num7 == 0)
			{
				continue;
			}
			events_blocks[i] = default(event_block);
			events_blocks[i].eventheader = new int[325];
			for (int j = 1; j < 324; j++)
			{
				events_blocks[i].eventheader[j - 1] = (int)DataLoader.getValAtAddress(array, num6++, 8);
			}
			num6 = 326;
			events_blocks[i].events = new event_base[num7];
			int num8 = 0;
			for (int k = 0; k < events_blocks[i].events.GetUpperBound(0); k++)
			{
				switch ((int)DataLoader.getValAtAddress(array, num6 + 2, 8))
				{
				case 10:
					events_blocks[i].events[num8] = new event_conditional();
					break;
				case 1:
					events_blocks[i].events[num8] = new event_set_goal();
					break;
				case 255:
					events_blocks[i].events[num8] = new event_set_goal_alt();
					break;
				case 2:
					events_blocks[i].events[num8] = new event_move_npc();
					break;
				case 3:
					events_blocks[i].events[num8] = new event_kill_npc();
					break;
				case 5:
					events_blocks[i].events[num8] = new event_fire_triggers();
					break;
				case 251:
					events_blocks[i].events[num8] = new event_scheduled();
					break;
				case 245:
					events_blocks[i].events[num8] = new event_remove_npc();
					break;
				case 248:
					events_blocks[i].events[num8] = new event_set_race_attitude();
					break;
				case 249:
					events_blocks[i].events[num8] = new event_set_npc_props();
					break;
				case 253:
					events_blocks[i].events[num8] = new event_kill_npc_or_race();
					break;
				case 254:
					events_blocks[i].events[num8] = new event_place_npc();
					break;
				default:
					events_blocks[i].events[num8] = new event_base();
					break;
				}
				events_blocks[i].events[num8].InitRawData(i, num6, array);
				num8++;
				num6 += 16;
			}
		}
		if (GameWorldController.instance.CreateReports)
		{
			WriteEventReport();
		}
	}

	private void WriteEventReport()
	{
		StreamWriter streamWriter = new StreamWriter(Application.dataPath + "//..//_scd_report.txt", false);
		string text = "";
		for (int i = 0; i <= events_blocks.GetUpperBound(0); i++)
		{
			if (events_blocks[i].events == null)
			{
				continue;
			}
			for (int j = 0; j <= events_blocks[i].events.GetUpperBound(0); j++)
			{
				if (events_blocks[i].events[j] != null)
				{
					string text2 = text;
					text = text2 + "\nBlock " + i + " Row " + j + "\n";
					text += events_blocks[i].events[j].ReportEventDetails();
				}
			}
		}
		streamWriter.Write(text);
		streamWriter.Close();
	}

	public void ProcessEvents()
	{
		for (int i = 0; i <= events_blocks.GetUpperBound(0); i++)
		{
			event_base.Executing = true;
			if (events_blocks[i].events == null)
			{
				continue;
			}
			for (int j = 0; j <= events_blocks[i].events.GetUpperBound(0); j++)
			{
				if (events_blocks[i].events[j] != null)
				{
					events_blocks[i].events[j].Process();
					if (events_blocks[i].events[j].clear)
					{
						events_blocks[i].events[j] = null;
					}
				}
			}
		}
	}
}
