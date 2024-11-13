using System.IO;
using UnityEngine;

public class WhatTheHellIsSCD_ARK : UWEBase
{
	public bool TableView = true;

	public int InfoSize = 16;

	public bool DoTheThingThisCodeDoes = false;

	public void DumpScdArkInfo(string SCD_Ark_File_Path)
	{
		if (!DoTheThingThisCodeDoes)
		{
			return;
		}
		int num = 0;
		string text = "";
		StreamWriter streamWriter;
		if (TableView)
		{
			streamWriter = new StreamWriter(Application.dataPath + "//..//_scd_ark.csv", false);
			text += "Address,Block,Level,1,Type,TypeDesc,Variable/TileX,IsQuest/TileY,5,6,7,8,9,10,11,12,13,14,15\n";
		}
		else
		{
			streamWriter = new StreamWriter(Application.dataPath + "//..//_scd_ark.txt", false);
		}
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + SCD_Ark_File_Path, out buffer))
		{
			Debug.Log(Loader.BasePath + SCD_Ark_File_Path + " File not loaded");
			return;
		}
		int num2 = (int)DataLoader.getValAtAddress(buffer, 0L, 32);
		for (num = 0; num < num2; num++)
		{
			long num3 = 6L;
			int num4 = (int)DataLoader.getValAtAddress(buffer, num3 + num2 * 4 + num * 4, 32);
			long datalen = DataLoader.getValAtAddress(buffer, num3 + num2 * 4 * 2 + num * 4, 32);
			int num5 = (num4 >> 1) & 1;
			num3 = num * 4 + 6;
			if ((int)DataLoader.getValAtAddress(buffer, num3, 32) == 0)
			{
				Debug.Log("No Scd.ark data for this level");
			}
			char[] array;
			long num6;
			if (num5 == 1)
			{
				datalen = 0L;
				array = DataLoader.unpackUW2(buffer, DataLoader.getValAtAddress(buffer, num3, 32), ref datalen);
				num3 += 4;
				num6 = 0L;
				num3 = 0L;
			}
			else
			{
				long valAtAddress = DataLoader.getValAtAddress(buffer, num3, 32);
				int num7 = 0;
				num6 = valAtAddress;
				num3 = 0L;
				array = new char[datalen];
				for (long num8 = valAtAddress; num8 < valAtAddress + datalen; num8++)
				{
					array[num7] = buffer[num8];
					num7++;
				}
			}
			int num9 = 0;
			if (TableView)
			{
				if ((int)DataLoader.getValAtAddress(array, 0L, 8) == 0)
				{
					continue;
				}
				num9 = 326;
				int num10 = 0;
				for (int i = 326; i < datalen; i++)
				{
					if (num10 == 0)
					{
						text = text + num6 + num9 + "," + num + ",";
					}
					if (num10 == 2)
					{
						text = text + (int)DataLoader.getValAtAddress(array, num9, 8) + ",";
						switch ((int)DataLoader.getValAtAddress(array, num9, 8))
						{
						case 1:
							text += "SetGoal";
							break;
						case 10:
							text += "Condition";
							break;
						case 3:
							text += "KillNPC";
							break;
						case 5:
							text += "FireTrigger";
							break;
						case 251:
							text += "ScheduledEvent";
							break;
						case 248:
							text += "RaceAttitude";
							break;
						case 249:
							text += "SetProps";
							break;
						case 245:
							text += "RemoveNPC";
							break;
						case 253:
							text += "KillNPCorRace";
							break;
						case 254:
							text += "PlaceNPC";
							break;
						case 255:
							text += "SetGoal Alt";
							break;
						default:
							text += "UNK";
							break;
						}
						num9++;
					}
					else
					{
						text += (int)DataLoader.getValAtAddress(array, num9++, 8);
					}
					num10++;
					if (num10 == 16)
					{
						num10 = 0;
						text += "\n";
					}
					else
					{
						text += ",";
					}
				}
				text += "\n";
				continue;
			}
			text = text + "Block no " + num + " at address " + num6 + "\n";
			text = text + "No of rows " + (int)DataLoader.getValAtAddress(array, num9++, 8) + "\n";
			if ((int)DataLoader.getValAtAddress(array, 0L, 8) == 0)
			{
				continue;
			}
			text += "Unknown info 1-325\n";
			for (int j = 1; j < 324; j++)
			{
				text = text + (int)DataLoader.getValAtAddress(array, num9++, 8) + "\n";
			}
			text += "Row Data\nr\n";
			num9 = 326;
			int num11 = 0;
			for (int k = 326; k < datalen; k++)
			{
				text = text + (int)DataLoader.getValAtAddress(array, num9++, 8) + "\n";
				num11++;
				if (num11 == 16)
				{
					num11 = 0;
					text += "r\n";
				}
			}
		}
		streamWriter.WriteLine(text);
		streamWriter.Close();
	}
}
