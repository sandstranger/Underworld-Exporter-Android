using UnityEngine;

public class TerrainDatLoader : Loader
{
	public enum TerrainTypes
	{
		Normal = 0,
		Ankh = 2,
		Stairsup = 3,
		Stairsdown = 4,
		Pipe = 5,
		Grating = 6,
		Drain = 7,
		ChainedPrincess = 8,
		Window = 9,
		Tapestry = 10,
		Textured_door = 11,
		Water = 16,
		Lava = 32,
		Waterfall = 64,
		Ice_wall = 192,
		Ice_walls = 193,
		Lavafall = 128,
		IceNonSlip = 248,
		WaterFlowSouth = 72,
		WaterFlowNorth = 80,
		WaterFlowWest = 88,
		WaterFlowEast = 96,
		Unknown = -1
	}

	public const int Normal = 0;

	public const int Ankh = 2;

	public const int Stairsup = 3;

	public const int Stairsdown = 4;

	public const int Pipe = 5;

	public const int Grating = 6;

	public const int Drain = 7;

	public const int ChainedPrincess = 8;

	public const int Window = 9;

	public const int Tapestry = 10;

	public const int Textured_door = 11;

	public const int Water = 16;

	public const int Lava = 32;

	public const int Waterfall = 64;

	public const int Ice_wall = 192;

	public const int Ice_walls = 232;

	public const int Lavafall = 128;

	public const int IceNonSlip = 248;

	public const int WaterFlowSouth = 72;

	public const int WaterFlowNorth = 80;

	public const int WaterFlowWest = 88;

	public const int WaterFlowEast = 96;

	public int[] Terrain;

	public TerrainDatLoader()
	{
		string text = "TERRAIN.DAT";
		if (UWClass._RES == "UW0")
		{
			text = "DTERRAIN.DAT";
		}
		Terrain = new int[512];
		int num = 0;
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + UWClass.sep + "DATA" + UWClass.sep + text, out buffer))
		{
			return;
		}
		switch (UWClass._RES)
		{
		case "UW1":
		case "UW0":
		{
			for (int j = 0; j < 256; j++)
			{
				Terrain[j] = (int)DataLoader.getValAtAddress(buffer, num, 16);
				num += 2;
			}
			num = 512;
			for (int k = 256; k < 512; k++)
			{
				Terrain[k] = (int)DataLoader.getValAtAddress(buffer, num, 16);
				num += 2;
			}
			break;
		}
		case "UW2":
		{
			for (int i = 0; i < 256; i++)
			{
				Terrain[i] = (int)DataLoader.getValAtAddress(buffer, num, 16);
				num += 2;
			}
			break;
		}
		}
	}

	public static TerrainTypes getTerrain(int terrainNo)
	{
		switch (terrainNo)
		{
		case 0:
			return TerrainTypes.Normal;
		case 2:
			return TerrainTypes.Ankh;
		case 3:
			return TerrainTypes.Stairsup;
		case 4:
			return TerrainTypes.Stairsdown;
		case 5:
			return TerrainTypes.Pipe;
		case 6:
			return TerrainTypes.Grating;
		case 7:
			return TerrainTypes.Drain;
		case 8:
			return TerrainTypes.ChainedPrincess;
		case 9:
			return TerrainTypes.Window;
		case 10:
			return TerrainTypes.Tapestry;
		case 11:
			return TerrainTypes.Textured_door;
		case 16:
			return TerrainTypes.Water;
		case 32:
			return TerrainTypes.Lava;
		case 64:
			return TerrainTypes.Waterfall;
		case 192:
			return TerrainTypes.Ice_wall;
		case 232:
			return TerrainTypes.Ice_walls;
		case 128:
			return TerrainTypes.Lavafall;
		case 248:
			return TerrainTypes.IceNonSlip;
		case 72:
			return TerrainTypes.WaterFlowSouth;
		case 88:
			return TerrainTypes.WaterFlowWest;
		case 96:
			return TerrainTypes.WaterFlowEast;
		case 80:
			return TerrainTypes.WaterFlowNorth;
		default:
			Debug.Log("terrain unknown:" + terrainNo);
			return TerrainTypes.Unknown;
		}
	}
}
