using System;
using System.IO;
using UnityEngine;

public class ObjectLoader : DataLoader
{
	private struct xrefTable
	{
		public short tileX;

		public short tileY;

		public int next;

		public int MstIndex;

		public int nextTile;

		public short duplicate;

		public short duplicateAssigned;

		public short duplicateNextAssigned;
	}

	private const int UWDEMO = 0;

	private const int UW1 = 1;

	private const int UW2 = 2;

	private const int SHOCK = 3;

	private const int GUNS_WEAPONS = 0;

	private const int AMMUNITION = 1;

	private const int PROJECTILES = 2;

	private const int GRENADE_EXPLOSIONS = 3;

	private const int PATCHES = 4;

	private const int HARDWARE = 5;

	private const int SOFTWARE_LOGS = 6;

	private const int FIXTURES = 7;

	private const int GETTABLES_OTHER = 8;

	private const int SWITCHES_PANELS = 9;

	private const int DOORS_GRATINGS = 10;

	private const int ANIMATED = 11;

	private const int TRAPS_MARKERS = 12;

	private const int CONTAINERS_CORPSES = 13;

	private const int CRITTERS = 14;

	private const int GUNS_WEAPONS_OFFSET = 4010;

	private const int AMMUNITION_OFFSET = 4011;

	private const int PROJECTILES_OFFSET = 4012;

	private const int GRENADE_EXPLOSIONS_OFFSET = 4013;

	private const int PATCHES_OFFSET = 4014;

	private const int HARDWARE_OFFSET = 4015;

	private const int SOFTWARE_LOGS_OFFSET = 4016;

	private const int FIXTURES_OFFSET = 4017;

	private const int GETTABLES_OTHER_OFFSET = 4018;

	private const int SWITCHES_PANELS_OFFSET = 4019;

	private const int DOORS_GRATINGS_OFFSET = 4020;

	private const int ANIMATED_OFFSET = 4021;

	private const int TRAPS_MARKERS_OFFSET = 4022;

	private const int CONTAINERS_CORPSES_OFFSET = 4023;

	private const int CRITTERS_OFFSET = 4024;

	private const int SURVELLANCE_OFFSET = 4043;

	private const int SOFT_PROPERTY_VERSION = 0;

	private const int SOFT_PROPERTY_LOG = 9;

	private const int SOFT_PROPERTY_LEVEL = 2;

	private const int BUTTON_PROPERTY_TRIGGER = 0;

	private const int BUTTON_PROPERTY_PUZZLE = 1;

	private const int BUTTON_PROPERTY_COMBO = 2;

	private const int BUTTON_PROPERTY_TRIGGER_2 = 3;

	private const int TRIG_PROPERTY_OBJECT = 0;

	private const int TRIG_PROPERTY_TARGET_X = 1;

	private const int TRIG_PROPERTY_TARGET_Y = 2;

	private const int TRIG_PROPERTY_TARGET_Z = 3;

	private const int TRIG_PROPERTY_FLAG = 4;

	private const int TRIG_PROPERTY_VARIABLE = 5;

	private const int TRIG_PROPERTY_VALUE = 6;

	private const int TRIG_PROPERTY_OPERATION = 7;

	private const int TRIG_PROPERTY_MESSAGE1 = 8;

	private const int TRIG_PROPERTY_MESSAGE2 = 9;

	private const int TRIG_PROPERTY_LIGHT_OP = 3;

	private const int TRIG_PROPERTY_CONTROL_1 = 4;

	private const int TRIG_PROPERTY_CONTROL_2 = 5;

	private const int TRIG_PROPERTY_UPPERSHADE_1 = 6;

	private const int TRIG_PROPERTY_LOWERSHADE_1 = 7;

	private const int TRIG_PROPERTY_UPPERSHADE_2 = 8;

	private const int TRIG_PROPERTY_LOWERSHADE_2 = 9;

	private const int TRIG_PROPERTY_FLOOR = 5;

	private const int TRIG_PROPERTY_CEILING = 6;

	private const int TRIG_PROPERTY_SPEED = 7;

	private const int TRIG_PROPERTY_TRIG_1 = 5;

	private const int TRIG_PROPERTY_TRIG_2 = 6;

	private const int TRIG_PROPERTY_EMAIL = 9;

	private const int TRIG_PROPERTY_TYPE = 8;

	private const int CONTAINER_CONTENTS_1 = 0;

	private const int CONTAINER_CONTENTS_2 = 1;

	private const int CONTAINER_CONTENTS_3 = 2;

	private const int CONTAINER_CONTENTS_4 = 3;

	private const int CONTAINER_WIDTH = 5;

	private const int CONTAINER_HEIGHT = 6;

	private const int CONTAINER_DEPTH = 7;

	private const int CONTAINER_TOP = 8;

	private const int CONTAINER_SIDE = 9;

	private const int SCREEN_NO_OF_FRAMES = 0;

	private const int SCREEN_LOOP_FLAG = 1;

	private const int SCREEN_START = 2;

	private const int SCREEN_SURVEILLANCE_TARGET = 3;

	private const int WORDS_STRING_NO = 0;

	private const int WORDS_FONT = 1;

	private const int WORDS_SIZE = 2;

	private const int WORDS_COLOUR = 3;

	private const int BRIDGE_X_SIZE = 0;

	private const int BRIDGE_Y_SIZE = 1;

	private const int BRIDGE_HEIGHT = 2;

	private const int BRIDGE_TOP_BOTTOM_TEXTURE = 3;

	private const int BRIDGE_TOP_BOTTOM_TEXTURE_SOURCE = 4;

	private const int BRIDGE_SIDE_TEXTURE = 5;

	private const int BRIDGE_SIDE_TEXTURE_SOURCE = 6;

	public ObjectLoaderInfo[] objInfo;

	public int NoOfFreeMobile;

	public int NoOfFreeStatic;

	public int[] FreeMobileList = new int[254];

	public int[] FreeStaticList = new int[768];

	public void LoadObjectList(TileMap tileMap, UWBlock lev_ark)
	{
		objInfo = new ObjectLoaderInfo[1024];
		BuildObjectListUW(tileMap.Tiles, objInfo, tileMap.texture_map, lev_ark, tileMap.thisLevelNo);
		setObjectTileXY(1, tileMap.Tiles, objInfo);
		setDoorBits(tileMap.Tiles, objInfo);
		setElevatorBits(tileMap.Tiles, objInfo);
		setTerrainChangeBits(tileMap.Tiles, objInfo);
		SetBullFrog(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
		if (UWClass._RES == "UW2")
		{
			setQbert(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
			FindOffMapOscillatorTiles(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
			SetColourCyclingTiles(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
			SetFloorCollapseTiles(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
		}
	}

	public void LoadObjectList_OLD(TileMap tileMap, char[] lev_ark)
	{
		Debug.Log("OLD VERSION OF LOADOBJECT LIST");
		objInfo = new ObjectLoaderInfo[1024];
		BuildObjectListUW__OLD(tileMap.Tiles, objInfo, tileMap.texture_map, lev_ark, tileMap.thisLevelNo);
		setObjectTileXY(1, tileMap.Tiles, objInfo);
		setDoorBits(tileMap.Tiles, objInfo);
		setElevatorBits(tileMap.Tiles, objInfo);
		setTerrainChangeBits(tileMap.Tiles, objInfo);
		SetBullFrog(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
		if (UWClass._RES == "UW2")
		{
			setQbert(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
			FindOffMapOscillatorTiles(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
			SetColourCyclingTiles(tileMap.Tiles, objInfo, tileMap.thisLevelNo);
		}
	}

	public void LoadObjectListShock(TileMap tileMap, char[] lev_ark)
	{
		objInfo = new ObjectLoaderInfo[1600];
		BuildObjectListShock(tileMap.Tiles, objInfo, tileMap.texture_map, lev_ark, tileMap.thisLevelNo);
		setObjectTileXY(1, tileMap.Tiles, objInfo);
	}

	private bool BuildObjectListShock(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, short[] texture_map, char[] archive_ark, short LevelNo)
	{
		Chunk data_ark;
		if (!DataLoader.LoadChunk(archive_ark, LevelNo * 100 + 4009, out data_ark))
		{
			return false;
		}
		xrefTable[] xref = new xrefTable[data_ark.chunkUnpackedLength / 10];
		int num = 0;
		int num2 = 0;
		for (num = 0; num < data_ark.chunkUnpackedLength / 10; num++)
		{
			xref[num].tileX = (short)DataLoader.getValAtAddress(data_ark.data, num2, 16);
			xref[num].tileY = (short)DataLoader.getValAtAddress(data_ark.data, num2 + 2, 16);
			xref[num].MstIndex = (int)DataLoader.getValAtAddress(data_ark.data, num2 + 4, 16);
			xref[num].next = (int)DataLoader.getValAtAddress(data_ark.data, num2 + 6, 16);
			xref[num].nextTile = (int)DataLoader.getValAtAddress(data_ark.data, num2 + 8, 16);
			if (xref[num].nextTile != num)
			{
				xref[num].duplicate = 1;
				xref[num].duplicateAssigned = 0;
			}
			num2 += 10;
		}
		Chunk data_ark2;
		if (!DataLoader.LoadChunk(archive_ark, LevelNo * 100 + 4008, out data_ark2))
		{
			return false;
		}
		long num3 = 0L;
		for (num = 0; num < data_ark2.chunkUnpackedLength / 27; num++)
		{
			num2 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 5, 16);
			if (xref[num2].duplicate == 1)
			{
				xref[num2].duplicateAssigned = 1;
			}
			num3 += 27;
		}
		for (num = 0; num < data_ark.chunkUnpackedLength / 10; num++)
		{
			if (xref[num].duplicate == 1 && xref[num].duplicateAssigned != 1)
			{
				replaceLink(ref xref, (int)data_ark.chunkUnpackedLength / 10, num, xref[num].next);
				replaceMapLink(ref LevelInfo, num, xref[num].next);
			}
		}
		for (num = 0; num <= objList.GetUpperBound(0); num++)
		{
			objList[num] = new ObjectLoaderInfo();
			objList[num].index = num;
			objList[num].next = 0;
			objList[num].item_id = 0;
			objList[num].link = 0;
			objList[num].owner = 0;
		}
		num3 = 0L;
		for (num = 0; num < data_ark2.chunkUnpackedLength / 27; num++)
		{
			num2 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 5, 16);
			int mstIndex = xref[num2].MstIndex;
			objList[mstIndex].index = mstIndex;
			objList[mstIndex].link = 0;
			objList[mstIndex].heading = 0;
			objList[mstIndex].invis = 0;
			objList[mstIndex].xpos = 0;
			objList[mstIndex].ypos = 0;
			objList[mstIndex].zpos = 0;
			objList[mstIndex].address = num3;
			short inUseFlag = (short)DataLoader.getValAtAddress(data_ark2.data, num3, 8);
			objList[mstIndex].InUseFlag = inUseFlag;
			objList[mstIndex].levelno = LevelNo;
			objList[mstIndex].ObjectTileX = xref[num2].tileX;
			objList[mstIndex].ObjectTileY = xref[num2].tileY;
			objList[mstIndex].next = xref[xref[num2].next].MstIndex;
			objList[mstIndex].parentList = this;
			int num4 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 1, 8);
			objList[mstIndex].ObjectClass = num4;
			int num5 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 2, 8);
			objList[mstIndex].ObjectSubClass = num5;
			int index = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 3, 16);
			int num6 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 20, 8);
			objList[mstIndex].ObjectSubClassIndex = num6;
			int shockObjectIndex = getShockObjectIndex(num4, num5, num6);
			if (shockObjectIndex != -1)
			{
				objList[mstIndex].item_id = shockObjectIndex;
				objList[mstIndex].xpos = (short)DataLoader.getValAtAddress(data_ark2.data, num3 + 11, 8);
				objList[mstIndex].ypos = (short)DataLoader.getValAtAddress(data_ark2.data, num3 + 13, 8);
				objList[mstIndex].zpos = (short)DataLoader.getValAtAddress(data_ark2.data, num3 + 15, 8);
				objList[mstIndex].Angle1 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 16, 8);
				objList[mstIndex].Angle2 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 17, 8);
				objList[mstIndex].Angle3 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 18, 8);
				objList[mstIndex].sprite = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 23, 8);
				objList[mstIndex].State = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 23, 8);
				objList[mstIndex].unk1 = (int)DataLoader.getValAtAddress(data_ark2.data, num3 + 24, 8);
				switch (num4)
				{
				case 6:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4016, 6, index, 9, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				case 7:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4017, 7, index, 16, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				case 8:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4018, 8, index, 16, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				case 9:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4019, 9, index, 30, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				case 10:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4020, 10, index, 14, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				case 12:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4022, 12, index, 28, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				case 13:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4023, 13, index, 21, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				case 14:
					lookUpSubClass(archive_ark, LevelInfo, LevelNo * 100 + 4024, 14, index, 46, xref, objList, texture_map, mstIndex, LevelNo);
					break;
				}
				UniqueObjectName(objList[mstIndex]);
			}
			else
			{
				objList[mstIndex].InUseFlag = 0;
			}
			num3 += 27;
		}
		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				if (LevelInfo[i, j].indexObjectList != 0)
				{
					LevelInfo[i, j].indexObjectList = xref[LevelInfo[i, j].indexObjectList].MstIndex;
				}
			}
		}
		return true;
	}

	private void BuildObjectListUW(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, short[] texture_map, UWBlock lev_ark, int LevelNo)
	{
		long address_pointer = 0L;
		long objectsAddress = 16384L;
		for (int i = 0; i < 1024; i++)
		{
			int[] array = new int[4];
			for (int j = 0; j <= array.GetUpperBound(0); j++)
			{
				array[j] = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + j * 2, 16);
			}
			objList[i] = new ObjectLoaderInfo();
			objList[i].parentList = this;
			objList[i].guid = Guid.NewGuid();
			objList[i].index = i;
			objList[i].ObjectTileX = 99;
			objList[i].ObjectTileY = 99;
			objList[i].levelno = (short)LevelNo;
			objList[i].next = 0;
			objList[i].address = lev_ark.Address + objectsAddress + address_pointer;
			objList[i].invis = 0;
			objList[i].item_id = DataLoader.ExtractBits(array[0], 0, 9);
			if (objList[i].item_id >= 464 && (UWClass._RES == "UW1" || UWClass._RES == "UW0"))
			{
				objList[i].item_id = 0;
			}
			objList[i].flags = (short)DataLoader.ExtractBits(array[0], 9, 3);
			objList[i].enchantment = (short)DataLoader.ExtractBits(array[0], 12, 1);
			objList[i].doordir = (short)DataLoader.ExtractBits(array[0], 13, 1);
			objList[i].invis = (short)DataLoader.ExtractBits(array[0], 14, 1);
			objList[i].is_quant = (short)DataLoader.ExtractBits(array[0], 15, 1);
			objList[i].zpos = (short)DataLoader.ExtractBits(array[1], 0, 7);
			objList[i].heading = (short)DataLoader.ExtractBits(array[1], 7, 3);
			objList[i].ypos = (short)DataLoader.ExtractBits(array[1], 10, 3);
			objList[i].xpos = (short)DataLoader.ExtractBits(array[1], 13, 3);
			objList[i].quality = (short)DataLoader.ExtractBits(array[2], 0, 6);
			objList[i].next = (short)DataLoader.ExtractBits(array[2], 6, 10);
			objList[i].owner = (short)DataLoader.ExtractBits(array[3], 0, 6);
			objList[i].link = (short)DataLoader.ExtractBits(array[3], 6, 10);
			HandleMovingDoors(objList, i);
			SetObjectTextureValue(objList, texture_map, i);
			if (i < 256)
			{
				objList[i].npc_hp = (short)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 8, 8);
				int value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 9, 8);
				objList[i].ProjectileHeadingMinor = (short)DataLoader.ExtractBits(value, 0, 5);
				objList[i].ProjectileHeadingMajor = (short)DataLoader.ExtractBits(value, 5, 3);
				objList[i].MobileUnk01 = (short)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 10, 8);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 11, 16);
				objList[i].npc_goal = (short)DataLoader.ExtractBits(value, 0, 4);
				objList[i].npc_gtarg = (short)DataLoader.ExtractBits(value, 4, 8);
				objList[i].MobileUnk02 = (short)DataLoader.ExtractBits(value, 12, 4);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 13, 16);
				objList[i].npc_level = (short)DataLoader.ExtractBits(value, 0, 4);
				objList[i].MobileUnk03 = (short)DataLoader.ExtractBits(value, 4, 8);
				objList[i].MobileUnk04 = (short)DataLoader.ExtractBits(value, 12, 1);
				objList[i].npc_talkedto = (short)DataLoader.ExtractBits(value, 13, 1);
				objList[i].npc_attitude = (short)DataLoader.ExtractBits(value, 14, 2);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 15, 16);
				objList[i].MobileUnk05 = (short)DataLoader.ExtractBits(value, 0, 6);
				objList[i].npc_height = (short)DataLoader.ExtractBits(value, 6, 7);
				objList[i].MobileUnk06 = (short)DataLoader.ExtractBits(value, 13, 3);
				objList[i].MobileUnk07 = (short)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 17, 8);
				objList[i].MobileUnk08 = (short)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 18, 8);
				objList[i].MobileUnk09 = (short)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 19, 8);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 20, 8);
				objList[i].Projectile_Speed = (short)DataLoader.ExtractBits(value, 0, 4);
				objList[i].Projectile_Pitch = (short)DataLoader.ExtractBits(value, 4, 3);
				objList[i].Projectile_Sign = (short)DataLoader.ExtractBits(value, 7, 1);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 21, 8);
				objList[i].npc_voidanim = (short)DataLoader.ExtractBits(value, 0, 4);
				objList[i].MobileUnk11 = (short)DataLoader.ExtractBits(value, 4, 4);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 22, 16);
				objList[i].MobileUnk12 = (short)DataLoader.ExtractBits(value, 0, 4);
				objList[i].npc_yhome = (short)DataLoader.ExtractBits(value, 4, 6);
				objList[i].npc_xhome = (short)DataLoader.ExtractBits(value, 10, 6);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 24, 8);
				objList[i].npc_heading = (short)DataLoader.ExtractBits(value, 0, 5);
				objList[i].MobileUnk13 = (short)DataLoader.ExtractBits(value, 5, 3);
				value = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 25, 8);
				objList[i].npc_hunger = (short)DataLoader.ExtractBits(value, 0, 6);
				objList[i].MobileUnk14 = (short)DataLoader.ExtractBits(value, 6, 2);
				objList[i].npc_whoami = (short)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer + 26, 8);
				address_pointer = address_pointer + 8 + 19;
			}
			else
			{
				address_pointer += 8;
			}
		}
		BuildFreeObjectLists(objList, lev_ark, ref address_pointer, ref objectsAddress);
	}

	private void BuildFreeObjectLists(ObjectLoaderInfo[] objList, UWBlock lev_ark, ref long address_pointer, ref long objectsAddress)
	{
		NoOfFreeMobile = (int)DataLoader.getValAtAddress(lev_ark, 31746L, 16);
		NoOfFreeStatic = (int)DataLoader.getValAtAddress(lev_ark, 31748L, 16);
		for (int i = 0; i <= objList.GetUpperBound(0); i++)
		{
			if (i > 2)
			{
				objList[i].InUseFlag = 1;
			}
		}
		objectsAddress = 29440L;
		address_pointer = 0L;
		StreamWriter streamWriter = new StreamWriter(Application.dataPath + "//..//_objInUse_At_Load_ark.txt", false);
		string text = "Mobile List\n";
		for (int j = 0; j <= NoOfFreeMobile; j++)
		{
			int num = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer, 16);
			objList[num].InUseFlag = 0;
			text = text + "Mobile Free:" + j + " = " + num + "\n";
			address_pointer += 2L;
		}
		text = text + "Count:" + NoOfFreeMobile + "\n";
		for (int k = NoOfFreeMobile + 1; k < 254; k++)
		{
			int num2 = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer, 16);
			text = text + "Mobile Junk:" + k + " = " + num2 + "\n";
			address_pointer += 2L;
		}
		text += "Static List\n";
		objectsAddress = 29948L;
		address_pointer = 0L;
		for (int l = 0; l <= NoOfFreeStatic; l++)
		{
			int num3 = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer, 16);
			objList[num3].InUseFlag = 0;
			text = text + "Static Free:" + l + " = " + num3 + "\n";
			address_pointer += 2L;
		}
		text = text + "Count (static):" + NoOfFreeStatic + "\n";
		for (int m = NoOfFreeStatic + 1; m < 768; m++)
		{
			int num4 = (int)DataLoader.getValAtAddress(lev_ark, objectsAddress + address_pointer, 16);
			text = text + "Static Junk:" + m + " = " + num4 + "\n";
			address_pointer += 2L;
		}
		streamWriter.Write(text);
		streamWriter.Close();
	}

	private void BuildObjectListUW__OLD(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, short[] texture_map, char[] lev_ark, int LevelNo)
	{
		Debug.Log("OLD VERSION OF BuildObjectListUW");
		long num8;
		long num4;
		switch (UWClass._RES)
		{
		default:
			return;
		case "UW0":
		{
			long num7 = 0L;
			num8 = num7 + 16384;
			num4 = 0L;
			break;
		}
		case "UW1":
		{
			int num = (int)DataLoader.getValAtAddress(lev_ark, 0L, 32);
			long num7 = DataLoader.getValAtAddress(lev_ark, LevelNo * 4 + 2, 32);
			num8 = num7 + 16384;
			num4 = 0L;
			break;
		}
		case "UW2":
		{
			char[] array = new char[lev_ark.GetUpperBound(0) + 1];
			for (int i = 0; i <= lev_ark.GetUpperBound(0); i++)
			{
				array[i] = lev_ark[i];
			}
			int num = (int)DataLoader.getValAtAddress(array, 0L, 32);
			int num2 = (int)DataLoader.getValAtAddress(array, 6 + LevelNo * 4 + num * 4, 32);
			int num3 = (num2 >> 1) & 1;
			num4 = LevelNo * 4 + 6;
			if (DataLoader.getValAtAddress(array, num4, 32) == 0)
			{
				return;
			}
			long num7;
			if (num3 == 1)
			{
				long datalen = 0L;
				lev_ark = DataLoader.unpackUW2(array, DataLoader.getValAtAddress(array, num4, 32), ref datalen);
			}
			else
			{
				int num5 = (int)DataLoader.getValAtAddress(array, num4, 32);
				int num6 = 0;
				num7 = 0L;
				lev_ark = new char[31752];
				for (int j = num5; j < num5 + 31752; j++)
				{
					lev_ark[num6] = array[j];
					num6++;
				}
			}
			num4 += 4;
			num7 = 0L;
			num8 = 16384L;
			num4 = 0L;
			break;
		}
		}
		for (int k = 0; k < 1024; k++)
		{
			objList[k] = new ObjectLoaderInfo();
			objList[k].parentList = this;
			objList[k].guid = Guid.NewGuid();
			objList[k].index = k;
			objList[k].ObjectTileX = 99;
			objList[k].ObjectTileY = 99;
			objList[k].levelno = (short)LevelNo;
			objList[k].next = 0;
			objList[k].address = num8 + num4;
			objList[k].invis = 0;
			objList[k].item_id = (int)DataLoader.getValAtAddress(lev_ark, num8 + num4, 16) & 0x1FF;
			if (objList[k].item_id >= 464 && (UWClass._RES == "UW1" || UWClass._RES == "UW0"))
			{
				objList[k].item_id = 0;
			}
			objList[k].flags = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4, 16) >> 9) & 7);
			objList[k].enchantment = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4, 16) >> 12) & 1);
			objList[k].doordir = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4, 16) >> 13) & 1);
			objList[k].invis = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4, 16) >> 14) & 1);
			objList[k].is_quant = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4, 16) >> 15) & 1);
			objList[k].zpos = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 2, 16) & 0x7F);
			objList[k].heading = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 2, 16) >> 7) & 7);
			objList[k].ypos = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 2, 16) >> 10) & 7);
			objList[k].xpos = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 2, 16) >> 13) & 7);
			objList[k].quality = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 4, 16) & 0x3F);
			objList[k].next = (int)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 4, 16) >> 6) & 0x3FF);
			objList[k].owner = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 6, 16) & 0x3F);
			objList[k].link = (int)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 6, 16) >> 6) & 0x3FF);
			if (objList[k].GetItemType() == 34 || objList[k].GetItemType() == 35)
			{
				objList[k].texture = texture_map[objList[k].owner];
			}
			if (objList[k].GetItemType() == 113)
			{
				objList[k].item_id = 320 + objList[k].owner;
				switch (objList[k].item_id)
				{
				case 320:
				case 321:
				case 322:
				case 323:
				case 324:
				case 325:
				case 327:
					objList[k].item_id += 8;
					objList[k].flags = 5;
					objList[k].enchantment = 1;
					objList[k].owner = 0;
					break;
				case 326:
					objList[k].item_id += 8;
					objList[k].flags = 4;
					objList[k].enchantment = 1;
					objList[k].owner = 0;
					break;
				case 328:
				case 329:
				case 330:
				case 331:
				case 332:
				case 333:
				case 334:
				case 335:
					objList[k].item_id -= 8;
					objList[k].flags = 0;
					objList[k].enchantment = 0;
					objList[k].owner = 0;
					break;
				}
			}
			if (objList[k].GetItemType() == 7)
			{
				if (objList[k].flags >= 2)
				{
					if (UWClass._RES == "UW2")
					{
						objList[k].texture = texture_map[objList[k].flags - 2];
					}
					else
					{
						objList[k].texture = texture_map[objList[k].flags - 2 + 48];
					}
				}
				else
				{
					objList[k].texture = 267 + (objList[k].flags & 0x3F);
				}
			}
			if (objList[k].GetItemType() == 8)
			{
				objList[k].texture = objList[k].flags;
			}
			if (objList[k].GetItemType() == 84)
			{
				objList[k].texture = objList[k].flags + 28;
			}
			if (objList[k].GetItemType() == 44)
			{
				objList[k].xpos = 4;
				objList[k].ypos = 4;
			}
			if (objList[k].GetItemType() == 42 && UWClass._RES == "UW1")
			{
				int num9 = (objList[k].quality >> 1) & 0xF;
				if (num9 == 10)
				{
					objList[k].texture = -1;
				}
				else if (num9 > 10)
				{
					objList[k].texture = -1;
				}
				else
				{
					objList[k].texture = texture_map[num9 + 48];
				}
				if (objList[k].zpos > 96)
				{
					objList[k].zpos = 96;
				}
			}
			if (k < 256)
			{
				objList[k].npc_hp = (short)DataLoader.getValAtAddress(lev_ark, num8 + num4 + 8, 8);
				objList[k].npc_goal = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 11, 16) & 0xF);
				objList[k].npc_gtarg = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 11, 16) >> 4) & 0xFF);
				objList[k].npc_level = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 13, 16) & 0xF);
				objList[k].npc_talkedto = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 13, 16) >> 13) & 1);
				objList[k].npc_attitude = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 13, 16) >> 14) & 3);
				objList[k].npc_voidanim = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 21, 8) & 7);
				objList[k].npc_yhome = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 22, 16) >> 4) & 0x3F);
				objList[k].npc_xhome = (short)((DataLoader.getValAtAddress(lev_ark, num8 + num4 + 22, 16) >> 10) & 0x3F);
				objList[k].npc_heading = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 24, 8) & 0x1F);
				objList[k].npc_hunger = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 25, 8) & 0x3F);
				objList[k].npc_whoami = (short)DataLoader.getValAtAddress(lev_ark, num8 + num4 + 26, 8);
				objList[k].ProjectileHeadingMinor = (short)(DataLoader.getValAtAddress(lev_ark, num8 + num4 + 9, 8) & 0x1F);
				num4 = num4 + 8 + 19;
			}
			else
			{
				num4 += 8;
			}
		}
	}

	private static void HandleMovingDoors(ObjectLoaderInfo[] objList, int x)
	{
		if (objList[x].GetItemType() == 113)
		{
			objList[x].item_id = 320 + objList[x].owner;
			switch (objList[x].item_id)
			{
			case 320:
			case 321:
			case 322:
			case 323:
			case 324:
			case 325:
			case 327:
				objList[x].item_id += 8;
				objList[x].flags = 5;
				objList[x].enchantment = 1;
				objList[x].owner = 0;
				break;
			case 326:
				objList[x].item_id += 8;
				objList[x].flags = 4;
				objList[x].enchantment = 1;
				objList[x].owner = 0;
				break;
			case 328:
			case 329:
			case 330:
			case 331:
			case 332:
			case 333:
			case 334:
			case 335:
				objList[x].item_id -= 8;
				objList[x].flags = 0;
				objList[x].enchantment = 0;
				objList[x].owner = 0;
				break;
			}
		}
	}

	private static void SetObjectTextureValue(ObjectLoaderInfo[] objList, short[] texture_map, int x)
	{
		if (objList[x].GetItemType() == 34 || objList[x].GetItemType() == 35)
		{
			objList[x].texture = texture_map[objList[x].owner];
		}
		if (objList[x].GetItemType() == 7)
		{
			if (objList[x].flags >= 2)
			{
				if (UWClass._RES == "UW2")
				{
					objList[x].texture = texture_map[objList[x].flags - 2];
				}
				else
				{
					objList[x].texture = texture_map[objList[x].flags - 2 + 48];
				}
			}
			else
			{
				objList[x].texture = 267 + (objList[x].flags & 0x3F);
			}
		}
		if (objList[x].GetItemType() == 8)
		{
			objList[x].texture = objList[x].flags;
		}
		if (objList[x].GetItemType() == 84)
		{
			objList[x].texture = objList[x].flags + 28;
		}
		if (objList[x].GetItemType() == 44)
		{
		}
		if (objList[x].GetItemType() == 42 && UWClass._RES == "UW1")
		{
			int num = (objList[x].quality >> 1) & 0xF;
			if (num == 10)
			{
				objList[x].texture = -1;
			}
			else if (num > 10)
			{
				objList[x].texture = -1;
			}
			else
			{
				objList[x].texture = texture_map[num + 48];
			}
			if (objList[x].zpos > 96)
			{
				objList[x].zpos = 96;
			}
		}
	}

	public static string UniqueObjectName(ObjectLoaderInfo currObj)
	{
		switch (currObj.GetItemType())
		{
		case 4:
		case 29:
		case 30:
			return "door_" + currObj.ObjectTileX.ToString("d3") + "_" + currObj.ObjectTileY.ToString("d3");
		case 0:
		{
			string @string = StringController.instance.GetString(7, currObj.npc_whoami + 16);
			if (currObj.npc_whoami != 0 && @string != "")
			{
				return @string + "_" + currObj.ObjectTileX.ToString("d2") + "_" + currObj.ObjectTileY.ToString("d2") + "_" + currObj.levelno.ToString("d2") + "_" + currObj.index.ToString("d4") + "_" + currObj.guid.ToString();
			}
			return currObj.getDesc() + "_" + currObj.ObjectTileX.ToString("d2") + "_" + currObj.ObjectTileY.ToString("d2") + "_" + currObj.levelno.ToString("d2") + "_" + currObj.index.ToString("d4") + "_" + currObj.guid.ToString();
		}
		default:
			return currObj.getDesc() + "_" + currObj.ObjectTileX.ToString("d2") + "_" + currObj.ObjectTileY.ToString("d2") + "_" + currObj.levelno.ToString("d2") + "_" + currObj.index.ToString("d4") + "_" + currObj.guid.ToString();
		}
	}

	public static string UniqueObjectNameEditor(ObjectLoaderInfo currObj)
	{
		return StringController.instance.GetSimpleObjectNameUW(currObj.item_id) + "_" + currObj.index.ToString("d4");
	}

	private void setObjectTileXY(int game, TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList)
	{
		for (short num = 0; num < 64; num++)
		{
			for (short num2 = 0; num2 < 64; num2++)
			{
				if (LevelInfo[num, num2].indexObjectList != 0)
				{
					for (long num3 = LevelInfo[num, num2].indexObjectList; num3 != 0; num3 = objList[num3].next)
					{
						objList[num3].ObjectTileX = num;
						objList[num3].ObjectTileY = num2;
					}
				}
			}
		}
	}

	public static bool isContainer(ObjectLoaderInfo currobj)
	{
		return currobj.GetItemType() == 19 || currobj.GetItemType() == 33;
	}

	public static bool isStatic(ObjectLoaderInfo currobj)
	{
		if (isTrap(currobj) || isTrigger(currobj))
		{
			return true;
		}
		switch (currobj.GetItemType())
		{
		case 4:
		case 7:
		case 8:
		case 10:
		case 21:
		case 29:
		case 30:
		case 31:
		case 34:
		case 35:
		case 82:
		case 84:
			return true;
		default:
			return false;
		}
	}

	public static bool isAlwaysInUse(ObjectLoaderInfo currobj)
	{
		int itemType = currobj.GetItemType();
		if (itemType == 99 || itemType == 21)
		{
			return true;
		}
		return false;
	}

	public static bool isTrigger(ObjectLoaderInfo currobj)
	{
		switch (currobj.GetItemType())
		{
		case 54:
		case 55:
		case 56:
		case 57:
		case 58:
		case 59:
		case 60:
		case 101:
		case 102:
		case 114:
		case 115:
		case 117:
		case 128:
			return true;
		default:
			return false;
		}
	}

	public static bool isTrap(ObjectLoaderInfo currobj)
	{
		switch (currobj.GetItemType())
		{
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 100:
		case 103:
		case 104:
		case 105:
		case 109:
		case 118:
		case 119:
		case 121:
		case 128:
		case 133:
			return true;
		default:
			return false;
		}
	}

	public static bool isMobile(ObjectLoaderInfo currobj)
	{
		return currobj.index < 256;
	}

	private void setElevatorBits(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList)
	{
		for (short num = 0; num < 64; num++)
		{
			for (short num2 = 0; num2 < 64; num2++)
			{
				if (LevelInfo[num, num2].indexObjectList != 0)
				{
					ObjectLoaderInfo objectLoaderInfo = objList[LevelInfo[num, num2].indexObjectList];
					do
					{
						if ((objectLoaderInfo.GetItemType() == 40 && objectLoaderInfo.quality == 3) || objectLoaderInfo.GetItemType() == 100 || objectLoaderInfo.GetItemType() == 41)
						{
							LevelInfo[num, num2].TerrainChange = true;
							objectLoaderInfo.ObjectTileX = num;
							objectLoaderInfo.ObjectTileY = num2;
							break;
						}
						objectLoaderInfo = objList[objectLoaderInfo.next];
					}
					while (objectLoaderInfo.index != 0);
				}
			}
		}
	}

	private void setTerrainChangeBits(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList)
	{
		for (short num = 0; num < 64; num++)
		{
			for (short num2 = 0; num2 < 64; num2++)
			{
				if (LevelInfo[num, num2].indexObjectList != 0)
				{
					ObjectLoaderInfo objectLoaderInfo = objList[LevelInfo[num, num2].indexObjectList];
					do
					{
						if (objectLoaderInfo.GetItemType() == 42)
						{
							LevelInfo[num, num2].TerrainChange = true;
							for (int i = num; i <= num + objectLoaderInfo.xpos; i++)
							{
								for (int j = num2; j <= num2 + objectLoaderInfo.ypos; j++)
								{
									LevelInfo[i, j].TerrainChange = true;
									for (int k = -1; k <= 1; k++)
									{
										for (int l = -1; l <= 1; l++)
										{
											if (i + l >= 0 && i + l < 63 && j + k >= 0 && j + k < 63)
											{
												LevelInfo[i + l, j + k].TerrainChange = true;
												LevelInfo[i + l, j + k].Render = true;
											}
										}
									}
								}
							}
							objectLoaderInfo.ObjectTileX = num;
							objectLoaderInfo.ObjectTileY = num2;
						}
						objectLoaderInfo = objList[objectLoaderInfo.next];
					}
					while (objectLoaderInfo.index != 0);
				}
			}
		}
	}

	private void SetBullFrog(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, int LevelNo)
	{
		if (LevelNo == 3 && UWClass._RES == "UW1")
		{
			for (int i = 48; i < 56; i++)
			{
				for (int j = 48; j < 56; j++)
				{
					LevelInfo[i, j].TerrainChange = true;
				}
			}
		}
		else if (UWClass._RES == "UW2" && LevelNo == 64)
		{
			LevelInfo[10, 49].TerrainChange = true;
			LevelInfo[10, 50].TerrainChange = true;
			LevelInfo[10, 51].TerrainChange = true;
			LevelInfo[10, 45].TerrainChange = true;
			LevelInfo[10, 46].TerrainChange = true;
			LevelInfo[10, 47].TerrainChange = true;
		}
	}

	private void SetFloorCollapseTiles(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, int LevelNo)
	{
		for (int i = 256; i <= objList.GetUpperBound(0); i++)
		{
			ObjectLoaderInfo objectLoaderInfo = objList[i];
			if (objectLoaderInfo.link <= 0 || objectLoaderInfo.link > objList.GetUpperBound(0))
			{
				continue;
			}
			ObjectLoaderInfo objectLoaderInfo2 = objList[objectLoaderInfo.link];
			if (!isTrigger(objectLoaderInfo) || objectLoaderInfo2.item_id != 387 || objectLoaderInfo2.quality != 17)
			{
				continue;
			}
			int quality = objectLoaderInfo.quality;
			int owner = objectLoaderInfo.owner;
			for (int j = -10; j <= 10; j++)
			{
				for (int k = -10; k <= 10; k++)
				{
					if (TileMap.ValidTile(quality + j, owner + k) && LevelInfo[quality + j, owner + k].tileType == 1)
					{
						LevelInfo[quality + j, owner + k].TerrainChange = true;
					}
				}
			}
		}
	}

	private void setQbert(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, int LevelNo)
	{
		if (LevelNo == 68 && UWClass._RES == "UW2")
		{
			LevelInfo[45, 50].TerrainChange = true;
			LevelInfo[46, 50].TerrainChange = true;
			LevelInfo[47, 50].TerrainChange = true;
			LevelInfo[48, 50].TerrainChange = true;
			LevelInfo[49, 50].TerrainChange = true;
			LevelInfo[44, 51].TerrainChange = true;
			LevelInfo[45, 52].TerrainChange = true;
			LevelInfo[46, 53].TerrainChange = true;
			LevelInfo[47, 54].TerrainChange = true;
			LevelInfo[48, 55].TerrainChange = true;
			LevelInfo[49, 56].TerrainChange = true;
			LevelInfo[50, 51].TerrainChange = true;
			LevelInfo[50, 52].TerrainChange = true;
			LevelInfo[50, 53].TerrainChange = true;
			LevelInfo[50, 54].TerrainChange = true;
			LevelInfo[50, 55].TerrainChange = true;
		}
	}

	private void FindOffMapOscillatorTiles(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, int LevelNo)
	{
		if (LevelNo == 71)
		{
			LevelInfo[34, 44].TerrainChange = true;
		}
	}

	private void SetColourCyclingTiles(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList, int LevelNo)
	{
		for (int i = 0; i <= objList.GetUpperBound(0); i++)
		{
			if (objList[i] == null || objList[i].item_id != 387 || objList[i].quality != 14)
			{
				continue;
			}
			for (int j = objList[i].ObjectTileX - 1; j <= objList[i].ObjectTileX + 5; j++)
			{
				for (int k = objList[i].ObjectTileY - 1; k <= objList[i].ObjectTileY + 5; k++)
				{
					if (TileMap.ValidTile(j, k))
					{
						LevelInfo[j, k].TerrainChange = true;
					}
				}
			}
		}
	}

	public static Vector3 CalcObjectXYZ(int ObjectIndex, short WallAdjust)
	{
		ObjectLoaderInfo[] array = UWClass.CurrentObjectList().objInfo;
		TileMap tileMap = UWClass.CurrentTileMap();
		int objectTileX = array[ObjectIndex].ObjectTileX;
		int objectTileY = array[ObjectIndex].ObjectTileY;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 7f;
		float num5 = 128f;
		if (UWClass._RES == "SHOCK")
		{
			num4 = 256f;
			num5 = 256f;
		}
		float num6 = 120f;
		float num7 = 120f;
		float num8 = 15f;
		float num9 = array[ObjectIndex].xpos;
		float num10 = array[ObjectIndex].ypos;
		num = (float)objectTileX * num6 + (float)array[ObjectIndex].xpos * (num6 / num4);
		num2 = (float)objectTileY * num7 + (float)array[ObjectIndex].ypos * (num7 / num4);
		float num11 = array[ObjectIndex].zpos;
		float num12 = tileMap.CEILING_HEIGHT;
		num3 = num11 / num5 * num12 * num8;
		if (UWClass._RES != "SHOCK" && objectTileX < 64 && objectTileY < 64)
		{
			switch (tileMap.Tiles[objectTileX, objectTileY].tileType)
			{
			case 6:
				num3 += num10 * (48f / num8);
				break;
			case 8:
				num += num9 * (48f / num8);
				break;
			case 7:
				num3 += (8f - num10) * (48f / num8);
				break;
			case 9:
				num3 += (8f - num9) * (48f / num8);
				break;
			}
		}
		if (objectTileX < 64 && objectTileY < 64)
		{
			switch (array[ObjectIndex].GetItemType())
			{
			case 34:
			case 35:
				switch (array[ObjectIndex].heading * 45)
				{
				case 90:
				case 270:
					num2 = (float)objectTileY * num7 + 60f;
					if (array[ObjectIndex].xpos == 0)
					{
						num += 1f;
					}
					if (array[ObjectIndex].xpos == 7)
					{
						num -= 1f;
					}
					break;
				case 0:
				case 180:
					num = (float)objectTileX * num6 + 60f;
					if (array[ObjectIndex].ypos == 0)
					{
						num2 += 1f;
					}
					if (array[ObjectIndex].ypos == 7)
					{
						num2 -= 1f;
					}
					break;
				}
				break;
			case 4:
			case 29:
			case 30:
			{
				float num14 = 80f;
				int num15 = tileMap.Tiles[objectTileX, objectTileY].floorHeight * 4;
				num3 = (float)num15 / num5 * num12 * num8;
				int num16 = findObjectByTypeInTile(array, array[ObjectIndex].ObjectTileX, array[ObjectIndex].ObjectTileY, 7);
				if (num16 != -1)
				{
					num3 = CalcObjectXYZ(num16, 0).y * 100f;
				}
				switch (array[ObjectIndex].heading * 45)
				{
				case 90:
					num2 = (float)array[ObjectIndex].ObjectTileY * num7 + num14 + (num7 - num14) / 2f;
					break;
				case 270:
					num2 = (float)array[ObjectIndex].ObjectTileY * num7 + (num7 - num14) / 2f;
					break;
				case 180:
					num = (float)array[ObjectIndex].ObjectTileX * num6 + num14 + (num6 - num14) / 2f;
					break;
				case 0:
					num = (float)array[ObjectIndex].ObjectTileX * num6 + (num6 - num14) / 2f;
					break;
				}
				if (array[ObjectIndex].xpos == 0)
				{
					num += 2f;
				}
				if (array[ObjectIndex].xpos == 7)
				{
					num -= 2f;
				}
				if (array[ObjectIndex].ypos == 0)
				{
					num2 += 2f;
				}
				if (array[ObjectIndex].ypos == 7)
				{
					num2 -= 2f;
				}
				break;
			}
			case 54:
				if (tileMap.Tiles[objectTileX, objectTileY].tileType != 0 && !tileMap.Tiles[objectTileX, objectTileY].TerrainChange && array[ObjectIndex].zpos < tileMap.Tiles[objectTileX, objectTileY].floorHeight * 4)
				{
					int num13 = tileMap.Tiles[objectTileX, objectTileY].floorHeight * 4;
					num3 = (float)num13 / num5 * num12 * num8;
				}
				break;
			case 8:
			case 10:
				if (array[ObjectIndex].xpos == 0)
				{
					num += 1.5f;
				}
				if (array[ObjectIndex].xpos == 7)
				{
					num -= 1.5f;
				}
				if (array[ObjectIndex].ypos == 0)
				{
					num2 += 1.5f;
				}
				if (array[ObjectIndex].ypos == 7)
				{
					num2 -= 1.5f;
				}
				if (num11 == 127f)
				{
					num3 -= 25f;
				}
				break;
			default:
			{
				if (WallAdjust != 1)
				{
					break;
				}
				string rES = UWClass._RES;
				if (rES != null && rES == "SHOCK")
				{
					if (array[ObjectIndex].xpos == 0)
					{
						num += 4f;
					}
					if (array[ObjectIndex].xpos == 128)
					{
						num -= 4f;
					}
					if (array[ObjectIndex].ypos == 0)
					{
						num2 += 4f;
					}
					if (array[ObjectIndex].ypos == 128)
					{
						num2 -= 3f;
					}
				}
				else
				{
					if (array[ObjectIndex].xpos == 0)
					{
						num += 4f;
					}
					if (array[ObjectIndex].xpos == 7)
					{
						num -= 4f;
					}
					if (array[ObjectIndex].ypos == 0)
					{
						num2 += 4f;
					}
					if (array[ObjectIndex].ypos == 7)
					{
						num2 -= 4f;
					}
				}
				break;
			}
			}
		}
		return new Vector3(num / 100f, num3 / 100f, num2 / 100f);
	}

	public static void RenderObjectList(ObjectLoader instance, TileMap tilemap, GameObject parent)
	{
		GameWorldController.LoadingObjects = true;
		foreach (Transform item in parent.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		for (int i = 0; i <= instance.objInfo.GetUpperBound(0); i++)
		{
			if (instance.objInfo[i] != null && instance.objInfo[i].InUseFlag == 1)
			{
				Vector3 position = ((tilemap != null) ? CalcObjectXYZ(i, 1) : new Vector3(118.8f, 5f, 118.8f));
				instance.objInfo[i].instance = ObjectInteraction.CreateNewObject(tilemap, instance.objInfo[i], instance.objInfo, parent, position);
			}
		}
		LinkObjectListWands(instance);
		LinkObjectListPotions(instance);
		GameWorldController.LoadingObjects = false;
	}

	public static ObjectLoaderInfo getObjectInfoAt(int index)
	{
		return UWClass.CurrentObjectList().objInfo[index];
	}

	public static ObjectLoaderInfo getObjectInfoAt(int index, ObjectLoader objList)
	{
		return objList.objInfo[index];
	}

	public static ObjectInteraction getObjectIntAt(int index)
	{
		return UWClass.CurrentObjectList().objInfo[index].instance;
	}

	public static int GetItemTypeAt(int index)
	{
		return getObjectInfoAt(index).GetItemType();
	}

	public static int GetItemTypeAt(int index, ObjectLoader objList)
	{
		return getObjectInfoAt(index, objList).GetItemType();
	}

	public static GameObject getGameObjectAt(int index)
	{
		if (UWClass.CurrentObjectList().objInfo[index].instance != null)
		{
			return UWClass.CurrentObjectList().objInfo[index].instance.gameObject;
		}
		return null;
	}

	public static void UpdateObjectList(TileMap currTileMap, ObjectLoader currObjList)
	{
		if (currObjList == null)
		{
			return;
		}
		int[,] array = new int[64, 64];
		for (int i = 0; i <= currObjList.objInfo.GetUpperBound(0); i++)
		{
			currObjList.objInfo[i].index = i;
			if (UWClass._RES == "UW2" && currObjList.objInfo[i].InUseFlag == 0 && i > 2)
			{
				ObjectLoaderInfo.CleanUp(currObjList.objInfo[i]);
			}
			if (currObjList.objInfo[i].ObjectTileX != 99)
			{
				currObjList.objInfo[i].next = 0;
				if (currObjList.objInfo[i].instance != null)
				{
					currObjList.objInfo[i].instance.next = 0;
				}
			}
		}
		if (currTileMap != null)
		{
			for (int j = 0; j <= 63; j++)
			{
				for (int k = 0; k <= 63; k++)
				{
					currTileMap.Tiles[j, k].indexObjectList = 0;
				}
			}
		}
		foreach (Transform item in GameWorldController.instance.DynamicObjectMarker())
		{
			if (item.gameObject.GetComponent<ObjectInteraction>() != null)
			{
				item.gameObject.GetComponent<ObjectInteraction>().OnSaveObjectEvent();
			}
		}
		foreach (Transform item2 in GameWorldController.instance.DynamicObjectMarker())
		{
			if (!(item2.gameObject.GetComponent<ObjectInteraction>() != null))
			{
				continue;
			}
			ObjectInteraction component = item2.gameObject.GetComponent<ObjectInteraction>();
			if (component.objectloaderinfo == null)
			{
				component.objectloaderinfo = new ObjectLoaderInfo();
				component.objectloaderinfo.InUseFlag = 0;
				component.objectloaderinfo.ObjectTileX = 99;
				component.objectloaderinfo.ObjectTileY = 99;
			}
			component.UpdatePosition();
			if (component.objectloaderinfo.InUseFlag == 1)
			{
				if (item2.gameObject.GetComponent<Container>() != null)
				{
					item2.gameObject.GetComponent<ObjectInteraction>().link = 0;
					linkContainerContents(item2.gameObject.GetComponent<Container>());
				}
				currObjList.CopyDataToList(component, ref component.objectloaderinfo);
			}
			else
			{
				Debug.Log(component.name + " exists but is flagged as not in use");
			}
		}
		for (int l = 0; l <= currObjList.objInfo.GetUpperBound(0); l++)
		{
			int objectTileX = currObjList.objInfo[l].ObjectTileX;
			int objectTileY = currObjList.objInfo[l].ObjectTileY;
			if (currObjList.objInfo[l].InUseFlag == 1 && objectTileX != 99 && objectTileY != 99)
			{
				if (array[objectTileX, objectTileY] == 0)
				{
					currTileMap.Tiles[objectTileX, objectTileY].indexObjectList = l;
					array[objectTileX, objectTileY] = l;
				}
				else
				{
					currObjList.objInfo[array[objectTileX, objectTileY]].next = l;
					currObjList.objInfo[array[objectTileX, objectTileY]].instance.next = l;
					array[objectTileX, objectTileY] = l;
				}
			}
		}
		currObjList.FreeMobileList = new int[254];
		currObjList.FreeStaticList = new int[768];
		int num = 0;
		int num2 = 0;
		for (int m = 2; m < 256; m++)
		{
			if (currObjList.objInfo[m].InUseFlag == 0)
			{
				currObjList.objInfo[m].instance = null;
				currObjList.FreeMobileList[num++] = m;
			}
		}
		for (int n = 256; n <= currObjList.objInfo.GetUpperBound(0); n++)
		{
			if (currObjList.objInfo[n].InUseFlag == 0)
			{
				currObjList.objInfo[n].instance = null;
				currObjList.FreeStaticList[num2++] = n;
			}
		}
		for (int num3 = 2; num3 < currObjList.objInfo.GetUpperBound(0); num3++)
		{
			if (currObjList.objInfo[num3].instance != null)
			{
				currObjList.CopyDataToList(currObjList.objInfo[num3].instance, ref currObjList.objInfo[num3]);
			}
		}
		if (num > 0)
		{
			num--;
		}
		if (num2 > 0)
		{
			num2--;
		}
		currObjList.NoOfFreeMobile = num;
		currObjList.NoOfFreeStatic = num2;
	}

	public static string[] UpdateInventoryObjectList(out int NoOfInventoryItems)
	{
		PlayerInventory playerInventory = UWCharacter.Instance.playerInventory;
		NoOfInventoryItems = 0;
		ObjectInteraction objectInteraction = null;
		foreach (Transform item in GameWorldController.instance.InventoryMarker.transform)
		{
			if (item != null)
			{
				NoOfInventoryItems++;
				item.gameObject.GetComponent<ObjectInteraction>().next = 0;
			}
		}
		string[] InventoryObjects = new string[NoOfInventoryItems];
		int num = 0;
		for (short num2 = 0; num2 <= 18; num2++)
		{
			ObjectInteraction objectInteraction2 = null;
			objectInteraction2 = ((num2 > 10) ? playerInventory.playerContainer.GetItemAt((short)(num2 - 11)) : playerInventory.GetObjectIntAtSlot(num2));
			if (objectInteraction2 != null)
			{
				InventoryObjects[num++] = objectInteraction2.name;
			}
		}
		foreach (Transform item2 in GameWorldController.instance.InventoryMarker.transform)
		{
			if (item2 != null && Array.IndexOf(InventoryObjects, item2.name) < 0)
			{
				InventoryObjects[num++] = item2.name;
			}
		}
		for (int i = 0; i <= 18; i++)
		{
			ObjectInteraction objectInteraction3 = ((i > 10) ? playerInventory.playerContainer.GetItemAt((short)(i - 11)) : playerInventory.GetObjectIntAtSlot(i));
			if (objectInteraction3 != null)
			{
				if (objectInteraction3.GetComponent<Container>() != null)
				{
					linkInventoryContainers(objectInteraction3.GetComponent<Container>(), ref InventoryObjects);
				}
				if (objectInteraction != null)
				{
					objectInteraction.GetComponent<ObjectInteraction>().next = Array.IndexOf(InventoryObjects, objectInteraction3.name) + 1;
				}
				objectInteraction = objectInteraction3;
			}
		}
		return InventoryObjects;
	}

	private static void linkInventoryContainers(Container cn, ref string[] InventoryObjects)
	{
		bool flag = false;
		cn.gameObject.GetComponent<ObjectInteraction>().link = 0;
		int num = Array.IndexOf(InventoryObjects, cn.name) + 1;
		ObjectInteraction objectInteraction = null;
		for (short num2 = 0; num2 <= cn.MaxCapacity(); num2++)
		{
			ObjectInteraction itemAt = cn.GetItemAt(num2);
			if (itemAt != null)
			{
				num = Array.IndexOf(InventoryObjects, itemAt.name) + 1;
				if (!flag)
				{
					cn.gameObject.GetComponent<ObjectInteraction>().link = num;
					flag = true;
					objectInteraction = itemAt;
				}
				else
				{
					objectInteraction.GetComponent<ObjectInteraction>().next = num;
					objectInteraction = itemAt;
				}
				if (itemAt.GetComponent<Container>() != null)
				{
					linkInventoryContainers(itemAt.GetComponent<Container>(), ref InventoryObjects);
				}
			}
		}
	}

	private static void linkContainerContents(Container cn)
	{
		if (cn == null)
		{
			Debug.Log("Null container in LinkContainerContents");
			return;
		}
		int num = 0;
		ObjectInteraction component = cn.gameObject.GetComponent<ObjectInteraction>();
		int index = component.objectloaderinfo.index;
		if (cn.LockObject != 0)
		{
			ObjectInteraction objectIntAt = getObjectIntAt(cn.LockObject);
			if (objectIntAt != null && objectIntAt.GetItemType() == 21)
			{
				num++;
				component.link = objectIntAt.objectloaderinfo.index;
				component.objectloaderinfo.link = objectIntAt.objectloaderinfo.index;
				index = objectIntAt.objectloaderinfo.index;
			}
		}
		short num2 = 0;
		while ((float)num2 < cn.GetCapacity())
		{
			ObjectInteraction itemAt = cn.GetItemAt(num2);
			if (itemAt != null)
			{
				if (num == 0)
				{
					component.link = itemAt.objectloaderinfo.index;
					component.objectloaderinfo.link = itemAt.objectloaderinfo.index;
					index = itemAt.objectloaderinfo.index;
				}
				else
				{
					if (itemAt == null)
					{
						Debug.Log("null object on " + num2 + " for container " + cn.name);
					}
					getObjectIntAt(index).next = itemAt.objectloaderinfo.index;
					getObjectIntAt(index).objectloaderinfo.next = itemAt.objectloaderinfo.index;
					itemAt.next = 0;
					itemAt.objectloaderinfo.next = 0;
					index = itemAt.objectloaderinfo.index;
				}
				num++;
			}
			num2++;
		}
	}

	public static int GetTileIndexNext(int tileX, int tileY)
	{
		return UWClass.CurrentTileMap().Tiles[tileX, tileY].indexObjectList;
	}

	public static int AssignObjectToList(ref ObjectInteraction objInt)
	{
		int startIndex = 1;
		if (objInt.GetComponent<NPC>() == null)
		{
			startIndex = 256;
		}
		int index;
		if (UWClass.CurrentObjectList().getFreeSlot(startIndex, out index))
		{
			objInt.objectloaderinfo = UWClass.CurrentObjectList().objInfo[index];
			objInt.objectloaderinfo.InUseFlag = 1;
			objInt.objectloaderinfo.index = index;
			if ((bool)objInt.GetComponent<Container>() || (bool)objInt.GetComponent<NPC>())
			{
				Container component = objInt.GetComponent<Container>();
				int num = 0;
				int num2 = index;
				short num3 = 0;
				while ((float)num3 <= component.GetCapacity())
				{
					ObjectInteraction objInt2 = component.GetItemAt(num3);
					if (objInt2 != null)
					{
						int num4 = AssignObjectToList(ref objInt2);
						if (num == 0)
						{
							objInt.link = num4;
							objInt.objectloaderinfo.link = num4;
							num2 = num4;
						}
						else
						{
							UWClass.CurrentObjectList().objInfo[num2].next = num4;
							UWClass.CurrentObjectList().objInfo[num2].instance.next = num4;
							num2 = num4;
						}
						objInt2.objectloaderinfo.next = 0;
						objInt2.next = 0;
						num++;
					}
					num3++;
				}
				if (num == 0)
				{
					objInt.link = 0;
				}
			}
			UWClass.CurrentObjectList().CopyDataToList(objInt, ref objInt.objectloaderinfo);
		}
		else
		{
			Debug.Log("Unable to assign object to list " + objInt.name);
		}
		return index;
	}

	public bool getFreeSlot(int startIndex, out int index)
	{
		if (startIndex < 2)
		{
			startIndex = 2;
		}
		for (int i = startIndex; i <= objInfo.GetUpperBound(0); i++)
		{
			if (objInfo[i].InUseFlag == 0)
			{
				index = i;
				return true;
			}
		}
		index = -1;
		return false;
	}

	private void CopyDataToList(ObjectInteraction objInt, ref ObjectLoaderInfo info)
	{
		info.item_id = objInt.item_id;
		info.flags = objInt.flags;
		info.enchantment = objInt.enchantment;
		info.doordir = objInt.doordir;
		info.invis = objInt.invis;
		info.is_quant = objInt.isquant;
		info.zpos = objInt.zpos;
		info.heading = objInt.heading;
		info.xpos = objInt.xpos;
		info.ypos = objInt.ypos;
		info.quality = objInt.quality;
		info.next = objInt.next;
		info.owner = objInt.owner;
		info.link = objInt.link;
		info.ObjectTileX = objInt.ObjectTileX;
		info.ObjectTileY = objInt.ObjectTileY;
		if (info.index < 256)
		{
			info.npc_hp = objInt.npc_hp;
			info.npc_goal = objInt.npc_goal;
			info.npc_gtarg = objInt.npc_gtarg;
			info.npc_level = objInt.npc_level;
			info.npc_talkedto = objInt.npc_talkedto;
			info.npc_attitude = objInt.npc_attitude;
			info.npc_voidanim = objInt.npc_voidanim;
			info.npc_yhome = objInt.npc_yhome;
			info.npc_xhome = objInt.npc_xhome;
			info.npc_heading = objInt.npc_heading;
			info.npc_hunger = objInt.npc_hunger;
			info.npc_whoami = objInt.npc_whoami;
			info.npc_health = objInt.npc_health;
			info.npc_arms = objInt.npc_arms;
			info.npc_power = objInt.npc_power;
			info.npc_name = objInt.npc_name;
			info.npc_health = objInt.npc_height;
			info.Projectile_Pitch = objInt.Projectile_Pitch;
			info.Projectile_Speed = objInt.Projectile_Speed;
			info.ProjectileHeadingMinor = objInt.ProjectileHeadingMinor;
			info.ProjectileHeadingMajor = objInt.ProjectileHeadingMajor;
			info.MobileUnk01 = objInt.MobileUnk01;
			info.MobileUnk02 = objInt.MobileUnk02;
			info.MobileUnk03 = objInt.MobileUnk03;
			info.MobileUnk04 = objInt.MobileUnk04;
			info.MobileUnk05 = objInt.MobileUnk05;
			info.MobileUnk06 = objInt.MobileUnk06;
			info.MobileUnk07 = objInt.MobileUnk07;
			info.MobileUnk08 = objInt.MobileUnk08;
			info.MobileUnk09 = objInt.MobileUnk09;
			info.Projectile_Sign = objInt.Projectile_Sign;
			info.MobileUnk11 = objInt.MobileUnk11;
			info.MobileUnk12 = objInt.MobileUnk12;
			info.MobileUnk13 = objInt.MobileUnk13;
			info.MobileUnk14 = objInt.MobileUnk14;
		}
		info.instance = objInt;
		objInt.objectloaderinfo = info;
	}

	public static ObjectLoaderInfo newObject(int item_id, int quality, int owner, int link, int startIndex)
	{
		int index = 0;
		if (startIndex >= 0)
		{
			if (UWClass.CurrentObjectList().getFreeSlot(startIndex, out index))
			{
				UWClass.CurrentObjectList().objInfo[index].guid = Guid.NewGuid();
				UWClass.CurrentObjectList().objInfo[index].quality = (short)quality;
				UWClass.CurrentObjectList().objInfo[index].flags = 0;
				UWClass.CurrentObjectList().objInfo[index].owner = (short)owner;
				UWClass.CurrentObjectList().objInfo[index].item_id = item_id;
				UWClass.CurrentObjectList().objInfo[index].next = 0;
				UWClass.CurrentObjectList().objInfo[index].link = link;
				UWClass.CurrentObjectList().objInfo[index].zpos = 0;
				UWClass.CurrentObjectList().objInfo[index].xpos = 0;
				UWClass.CurrentObjectList().objInfo[index].ypos = 0;
				UWClass.CurrentObjectList().objInfo[index].invis = 0;
				UWClass.CurrentObjectList().objInfo[index].doordir = 0;
				UWClass.CurrentObjectList().objInfo[index].is_quant = 0;
				UWClass.CurrentObjectList().objInfo[index].enchantment = 0;
				UWClass.CurrentObjectList().objInfo[index].ObjectTileX = 99;
				UWClass.CurrentObjectList().objInfo[index].ObjectTileY = 99;
				UWClass.CurrentObjectList().objInfo[index].InUseFlag = 1;
				UWClass.CurrentObjectList().objInfo[index].index = index;
				return UWClass.CurrentObjectList().objInfo[index];
			}
			return null;
		}
		ObjectLoaderInfo objectLoaderInfo = new ObjectLoaderInfo();
		objectLoaderInfo.guid = Guid.NewGuid();
		objectLoaderInfo.quality = (short)quality;
		objectLoaderInfo.flags = 0;
		objectLoaderInfo.owner = (short)owner;
		objectLoaderInfo.item_id = item_id;
		objectLoaderInfo.next = 0;
		objectLoaderInfo.link = link;
		objectLoaderInfo.zpos = 0;
		objectLoaderInfo.xpos = 0;
		objectLoaderInfo.ypos = 0;
		objectLoaderInfo.invis = 0;
		objectLoaderInfo.doordir = 0;
		objectLoaderInfo.is_quant = 0;
		objectLoaderInfo.enchantment = 0;
		objectLoaderInfo.ObjectTileX = 99;
		objectLoaderInfo.ObjectTileY = 99;
		objectLoaderInfo.InUseFlag = 1;
		objectLoaderInfo.index = index;
		return objectLoaderInfo;
	}

	private void replaceLink(ref xrefTable[] xref, int tableSize, int indexToFind, int linkToReplace)
	{
		if (indexToFind == 0 && linkToReplace == 0)
		{
			return;
		}
		for (int i = 0; i < tableSize; i++)
		{
			if (xref[i].next == indexToFind)
			{
				xref[i].next = linkToReplace;
			}
		}
	}

	private void replaceMapLink(ref TileInfo[,] levelInfo, int indexToFind, int linkToReplace)
	{
		if (indexToFind == 0 && linkToReplace == 0)
		{
			return;
		}
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				if (levelInfo[i, j].indexObjectList == indexToFind)
				{
					levelInfo[i, j].indexObjectList = linkToReplace;
				}
			}
		}
	}

	private int getShockObjectIndex(int objClass, int objSubClass, int objSubClassIndex)
	{
		for (int i = 0; i <= GameWorldController.instance.objectMaster.objProp.GetUpperBound(0); i++)
		{
			if (GameWorldController.instance.objectMaster.objProp[i].objClass == objClass && GameWorldController.instance.objectMaster.objProp[i].objSubClass == objSubClass && GameWorldController.instance.objectMaster.objProp[i].objSubClassIndex == objSubClassIndex)
			{
				return i;
			}
		}
		return -1;
	}

	private bool lookUpSubClass(char[] archive_ark, TileInfo[,] LevelInfo, int BlockNo, int ClassType, int index, int RecordSize, xrefTable[] xRef, ObjectLoaderInfo[] objList, short[] texture_map, int objIndex, short levelNo)
	{
		Chunk data_ark;
		if (!DataLoader.LoadChunk(archive_ark, BlockNo, out data_ark))
		{
			return false;
		}
		int i = 0;
		int num = 0;
		for (; i <= data_ark.chunkUnpackedLength; i++)
		{
			if (i == index)
			{
				switch (ClassType)
				{
				case 6:
					objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(data_ark.data, num + 6, 8);
					objList[objIndex].shockProperties[9] = (int)DataLoader.getValAtAddress(data_ark.data, num + 7, 8) + 2488;
					objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 8);
					return true;
				case 7:
					switch (objList[objIndex].ObjectSubClass)
					{
					case 2:
						switch (objList[objIndex].ObjectSubClassIndex)
						{
						case 3:
						{
							int[] array = new int[4] { 4, 7, 0, 10 };
							objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(data_ark.data, num + 6, 16);
							int num3 = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 16);
							objList[objIndex].shockProperties[1] = array[num3 & 3] + 602;
							objList[objIndex].shockProperties[2] = array[(num3 >> 4) & 3];
							objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(data_ark.data, num + 10, 16);
							break;
						}
						case 6:
						case 8:
						case 9:
						{
							objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(data_ark.data, num + 6, 16);
							objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 16);
							objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(data_ark.data, num + 12, 16);
							Chunk data_ark2;
							if (objList[objIndex].shockProperties[2] >= 248 && objList[objIndex].shockProperties[2] <= 255 && DataLoader.LoadChunk(archive_ark, levelNo * 100 + 4043, out data_ark2))
							{
								objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(data_ark2.data, (objList[objIndex].shockProperties[2] - 248) * 2, 16);
							}
							break;
						}
						}
						break;
					case 7:
					{
						int num2 = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 8);
						objList[objIndex].shockProperties[0] = num2 & 0xF;
						objList[objIndex].shockProperties[1] = (num2 >> 4) & 0xF;
						objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(data_ark.data, num + 9, 8);
						num2 = (int)DataLoader.getValAtAddress(data_ark.data, num + 10, 8);
						objList[objIndex].shockProperties[4] = (num2 >> 7) & 1;
						if (objList[objIndex].shockProperties[4] == 1)
						{
							objList[objIndex].shockProperties[3] = texture_map[num2 & 0x7F];
						}
						else
						{
							objList[objIndex].shockProperties[3] = num2 & 0x7F;
						}
						num2 = (int)DataLoader.getValAtAddress(data_ark.data, num + 11, 8);
						objList[objIndex].shockProperties[6] = (num2 >> 7) & 1;
						if (objList[objIndex].shockProperties[6] == 1)
						{
							objList[objIndex].shockProperties[5] = texture_map[num2 & 0x7F];
						}
						else
						{
							objList[objIndex].shockProperties[5] = num2 & 0x7F;
						}
						break;
					}
					}
					return true;
				case 8:
					if (objList[objIndex].item_id == 191)
					{
						objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(data_ark.data, num + 6, 16);
						objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 16);
						objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(data_ark.data, num + 10, 16);
						objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(data_ark.data, num + 12, 16);
					}
					return true;
				case 9:
					objList[objIndex].TriggerAction = (int)DataLoader.getValAtAddress(data_ark.data, num + 6, 16);
					getShockButtons(LevelInfo, data_ark, num, objList, objIndex);
					return true;
				case 10:
					return true;
				case 12:
					objList[objIndex].conditions[0] = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 8);
					objList[objIndex].conditions[1] = (int)DataLoader.getValAtAddress(data_ark.data, num + 9, 8);
					objList[objIndex].conditions[2] = (int)DataLoader.getValAtAddress(data_ark.data, num + 10, 8);
					objList[objIndex].conditions[3] = (int)DataLoader.getValAtAddress(data_ark.data, num + 11, 8);
					objList[objIndex].TriggerOnce = (int)DataLoader.getValAtAddress(data_ark.data, num + 7, 8);
					if (objList[objIndex].GetItemType() == 1010)
					{
						objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(data_ark.data, num + 21, 8);
						objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(data_ark.data, num + 24, 8);
						if (objList[objIndex].shockProperties[4] != 1)
						{
						}
					}
					else
					{
						getShockTriggerAction(LevelInfo, data_ark, num, xRef, objList, objIndex);
					}
					return true;
				case 13:
					objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(data_ark.data, num + 6, 16);
					objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 16);
					objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(data_ark.data, num + 10, 16);
					objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(data_ark.data, num + 12, 16);
					objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(data_ark.data, num + 14, 8);
					objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(data_ark.data, num + 15, 8);
					objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(data_ark.data, num + 16, 8);
					objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(data_ark.data, num + 17, 8);
					objList[objIndex].shockProperties[9] = (int)DataLoader.getValAtAddress(data_ark.data, num + 18, 8);
					return true;
				case 14:
					return true;
				}
			}
			num += RecordSize;
		}
		return false;
	}

	private void getShockTriggerAction(TileInfo[,] LevelInfo, Chunk sub_ark, int add_ptr, xrefTable[] xRef, ObjectLoaderInfo[] objList, int objIndex)
	{
		int num = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 6, 8);
		objList[objIndex].TriggerAction = num;
		switch (num)
		{
		case 0:
			break;
		case 1:
			objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
			break;
		case 2:
			objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			break;
		case 3:
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
			objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
			break;
		case 4:
			objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
			objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
			break;
		case 6:
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
			objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
			objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
			objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 22, 16);
			objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
			objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 26, 16);
			break;
		case 7:
			objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 22, 8);
			objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 23, 8);
			objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 8);
			objList[objIndex].shockProperties[9] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 25, 8);
			break;
		case 8:
			break;
		case 9:
			setElevatorProperties(LevelInfo, sub_ark, add_ptr, objList, objIndex);
			break;
		case 11:
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
			objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
			objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
			objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 22, 16);
			objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
			objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 26, 16);
			break;
		case 12:
			objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			break;
		case 15:
			objList[objIndex].shockProperties[9] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16) + 2441;
			break;
		case 16:
			break;
		case 19:
			break;
		case 21:
			objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
			break;
		case 22:
			objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[9] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			break;
		case 23:
			objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 32);
			objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
			break;
		case 24:
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 8);
			break;
		case 5:
		case 10:
		case 13:
		case 14:
		case 17:
		case 18:
		case 20:
			break;
		}
	}

	private void getShockButtons(TileInfo[,] LevelInfo, Chunk sub_ark, int add_ptr, ObjectLoaderInfo[] objList, int objIndex)
	{
		if (objList[objIndex].ObjectSubClass == 0)
		{
			switch (objList[objIndex].TriggerAction)
			{
			case 4:
				objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
				objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
				objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
				objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
				break;
			case 6:
				objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
				objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
				objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
				objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
				objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
				objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 22, 16);
				objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
				objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 26, 16);
				break;
			case 9:
				setElevatorProperties(LevelInfo, sub_ark, add_ptr, objList, objIndex);
				break;
			case 12:
				objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
				objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
				break;
			case 7:
				objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
				if (objList[objIndex].shockProperties[4] <= 3)
				{
					objList[objIndex].shockProperties[4] = objIndex;
				}
				objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
				objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 22, 8);
				objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 8);
				objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 23, 8);
				objList[objIndex].shockProperties[9] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 25, 8);
				break;
			case 24:
				objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
				objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 8);
				break;
			case 19:
				objList[objIndex].shockProperties[8] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
				objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
				break;
			default:
				objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
				objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
				break;
			}
		}
		else if (objList[objIndex].ObjectSubClass == 2 && objList[objIndex].ObjectSubClassIndex == 0)
		{
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
			objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
		}
		else if (objList[objIndex].ObjectSubClass == 2 && objList[objIndex].ObjectSubClassIndex >= 1)
		{
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
		}
		else if (objList[objIndex].ObjectSubClass == 3 && objList[objIndex].ObjectSubClassIndex <= 3)
		{
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[1] = ((int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 8) >> 28) & 1;
			if (objList[objIndex].shockProperties[1] != 1)
			{
			}
		}
		else if (objList[objIndex].ObjectSubClass == 3 && (objList[objIndex].ObjectSubClassIndex == 4 || objList[objIndex].ObjectSubClassIndex == 5 || objList[objIndex].ObjectSubClassIndex == 6))
		{
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
			objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 18, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
			objList[objIndex].shockProperties[4] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 26, 16);
		}
		else if (objList[objIndex].ObjectSubClass == 3 && (objList[objIndex].ObjectSubClassIndex == 7 || objList[objIndex].ObjectSubClassIndex == 8))
		{
			int num = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
			int num2 = (num & 0xF) + ((num >> 4) & 0xF) * 10 + ((num >> 8) & 0xF) * 100;
			objList[objIndex].shockProperties[2] = num2;
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 14, 16);
			objList[objIndex].shockProperties[3] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
		}
		else
		{
			objList[objIndex].shockProperties[0] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
		}
	}

	private void setElevatorProperties(TileInfo[,] LevelInfo, Chunk sub_ark, int add_ptr, ObjectLoaderInfo[] objList, int objIndex)
	{
		objList[objIndex].shockProperties[1] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 12, 16);
		objList[objIndex].shockProperties[2] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 16, 16);
		objList[objIndex].shockProperties[5] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 20, 16);
		objList[objIndex].shockProperties[6] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 22, 16);
		objList[objIndex].shockProperties[7] = (int)DataLoader.getValAtAddress(sub_ark.data, add_ptr + 24, 16);
	}

	private void setDoorBits(TileInfo[,] LevelInfo, ObjectLoaderInfo[] objList)
	{
		for (short num = 0; num < 64; num++)
		{
			for (int i = 0; i < 64; i++)
			{
				if (LevelInfo[num, i].indexObjectList == 0)
				{
					continue;
				}
				ObjectLoaderInfo objectLoaderInfo = objList[LevelInfo[num, i].indexObjectList];
				do
				{
					if (objList[objectLoaderInfo.index].GetItemType() == 4 || objList[objectLoaderInfo.index].GetItemType() == 29 || objList[objectLoaderInfo.index].GetItemType() == 30)
					{
						LevelInfo[num, i].isDoor = true;
						break;
					}
					objectLoaderInfo = objList[objectLoaderInfo.next];
				}
				while (objectLoaderInfo.index != 0);
			}
		}
	}

	public static void LinkObjectListWands(ObjectLoader objLoader)
	{
		for (int i = 1; i <= objLoader.objInfo.GetUpperBound(0); i++)
		{
			if (objLoader.objInfo[i] != null && objLoader.objInfo[i].instance != null && objLoader.objInfo[i].instance.GetComponent<Wand>() != null && objLoader.objInfo[i].instance.enchantment != 1)
			{
				int link = objLoader.objInfo[i].link;
				if (link != 0 && link <= objLoader.objInfo.GetUpperBound(0) && objLoader.objInfo[link].GetItemType() == 99 && objLoader.objInfo[link].instance != null && objLoader.objInfo[link].instance.GetComponent<a_spell>() != null)
				{
					objLoader.objInfo[i].instance.GetComponent<Wand>().linkedspell = objLoader.objInfo[link].instance.GetComponent<a_spell>();
				}
			}
		}
	}

	public static void LinkObjectListPotions(ObjectLoader objLoader)
	{
		for (int i = 1; i <= objLoader.objInfo.GetUpperBound(0); i++)
		{
			if (objLoader.objInfo[i] != null && objLoader.objInfo[i].instance != null && objLoader.objInfo[i].instance.GetComponent<Potion>() != null && objLoader.objInfo[i].instance.isquant != 1)
			{
				int link = objLoader.objInfo[i].link;
				if (link != 0 && link <= objLoader.objInfo.GetUpperBound(0) && (objLoader.objInfo[link].GetItemType() == 99 || objLoader.objInfo[link].GetItemType() == 37) && objLoader.objInfo[link].instance != null)
				{
					objLoader.objInfo[i].instance.GetComponent<Potion>().linked = objLoader.objInfo[link].instance;
				}
			}
		}
	}

	public static int findObjectByTypeInTile(ObjectLoaderInfo[] objList, short tileX, short tileY, int itemType)
	{
		for (int i = 0; i <= objList.GetUpperBound(0); i++)
		{
			if (objList[i].InUseFlag != 0 && objList[i].ObjectTileX == tileX && objList[i].ObjectTileY == tileY && objList[i].GetItemType() == itemType)
			{
				return i;
			}
		}
		return -1;
	}
}
