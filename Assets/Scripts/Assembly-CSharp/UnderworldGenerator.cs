using UnityEngine;
using UnityEngine.UI;

public class UnderworldGenerator : UWEBase
{
	public static int RegionIndex = 0;

	public GeneratorMap[,] mappings;

	public int startX = 0;

	public int startY = 0;

	public Text output;

	public int Seed;

	public static UnderworldGenerator instance;

	public int NoOfRooms = 4;

	public int sizeForCaveX = 64;

	public int sizeForCaveY = 64;

	public int IterationsForGenerator;

	private void Start()
	{
		instance = this;
	}

	public void GenerateLevel(int levelseed)
	{
		Seed = levelseed;
		mappings = new GeneratorMap[64, 64];
		Random.InitState(levelseed);
		Region region = new Region(RegionIndex++, 0, 0, 0, 64, 64, Random.Range(3, 26), null);
		mappings = region.GetEntireMap();
	}

	public TileMap CreateTileMap(short levelNo)
	{
		TileMap tileMap = new TileMap(levelNo);
		tileMap.texture_map = new short[64];
		for (short num = 0; num <= tileMap.texture_map.GetUpperBound(0); num++)
		{
			if (num <= 57)
			{
				tileMap.texture_map[num] = num;
			}
			else
			{
				tileMap.texture_map[num] = (short)(num - 57);
			}
		}
		tileMap.Tiles = new TileInfo[64, 64];
		tileMap.CEILING_HEIGHT = 32;
		RoomsToTileMap(tileMap, tileMap.Tiles);
		return tileMap;
	}

	public void RoomsToTileMap(TileMap tm, TileInfo[,] Tiles)
	{
		for (short num = 0; num <= 63; num++)
		{
			for (short num2 = 0; num2 <= 63; num2++)
			{
				short newtileType = (short)mappings[num, num2].TileLayoutMap;
				short newfloorHeight = 30;
				short newfloorTexture = 1;
				short newwallTexture = 1;
				short newceilingHeight = 0;
				short newFlags = 0;
				short newnoMagic = 0;
				short newdoorBit = 0;
				int newindexObjectList = 0;
				if (mappings[num, num2].TileLayoutMap != 0)
				{
					newfloorHeight = (short)mappings[num, num2].FloorHeight;
					newfloorTexture = (short)Mathf.Min(Mathf.Abs(mappings[num, num2].RoomMap), 10);
				}
				Tiles[num, num2] = new TileInfo(tm, num, num2, newtileType, newfloorHeight, newceilingHeight, newfloorTexture, newwallTexture, 0, newFlags, newnoMagic, newdoorBit, newindexObjectList);
			}
		}
		tm.SetTileMapWallFacesUW();
	}

	private void StyleJunctions()
	{
	}

	private void PlaceDiagonal(int x, int y)
	{
		if (mappings[x, y].RoomMap > 0)
		{
			return;
		}
		for (int i = 2; i <= 5; i++)
		{
			switch (i)
			{
			case 4:
				if (mappings[x, y - 1].TileLayoutMap == 0)
				{
					TurnTileDiag(x, y - 1, i, -1, -1, mappings[x, y].FloorHeight);
				}
				if (mappings[x - 1, y].TileLayoutMap == 0)
				{
					TurnTileDiag(x - 1, y, i, -1, -1, mappings[x, y].FloorHeight);
				}
				break;
			case 5:
				if (mappings[x, y - 1].TileLayoutMap == 0)
				{
					TurnTileDiag(x, y - 1, i, 1, -1, mappings[x, y].FloorHeight);
				}
				if (mappings[x + 1, y].TileLayoutMap == 0)
				{
					TurnTileDiag(x + 1, y, i, 1, -1, mappings[x, y].FloorHeight);
				}
				break;
			case 2:
				if (mappings[x, y + 1].TileLayoutMap == 0)
				{
					TurnTileDiag(x, y + 1, i, -1, 1, mappings[x, y].FloorHeight);
				}
				if (mappings[x - 1, y].TileLayoutMap == 0)
				{
					TurnTileDiag(x - 1, y, i, -1, 1, mappings[x, y].FloorHeight);
				}
				break;
			case 3:
				if (mappings[x, y + 1].TileLayoutMap == 0)
				{
					TurnTileDiag(x, y + 1, i, 1, 1, mappings[x, y].FloorHeight);
				}
				if (mappings[x + 1, y].TileLayoutMap == 0)
				{
					TurnTileDiag(x + 1, y, i, 1, 1, mappings[x, y].FloorHeight);
				}
				break;
			}
		}
	}

	private void TurnTileDiag(int x, int y, int newTileType, int dirX, int dirY, int Height)
	{
		if (mappings[x, y].TileLayoutMap == 0)
		{
			if (mappings[x, y + dirY].TileLayoutMap == 0 && mappings[x + dirX, y].TileLayoutMap == 0)
			{
				mappings[x, y].TileLayoutMap = newTileType;
				mappings[x, y].FloorHeight = Height;
			}
		}
		else
		{
			mappings[x, y].TileLayoutMap = 1;
		}
	}

	private void PlaceSlope(int x, int y)
	{
		bool[] array = new bool[4]
		{
			isTileWideOpen(x + 1, y),
			isTileWideOpen(x - 1, y),
			isTileWideOpen(x, y + 1),
			isTileWideOpen(x, y - 1)
		};
		int[] array2 = new int[4];
		int height = getHeight(x, y);
		array2[0] = height - getHeight(x + 1, y);
		array2[1] = height - getHeight(x - 1, y);
		array2[2] = height - getHeight(x, y + 1);
		array2[3] = height - getHeight(x, y - 1);
		if (x == 27 && y == 61)
		{
			Debug.Log("Here");
		}
		if (array[0] && array[1])
		{
			if (array2[0] == -2 && array2[1] == 2)
			{
				mappings[x, y].TileLayoutMap = 8;
				return;
			}
			if (array2[0] == 2 && array2[1] == -2)
			{
				mappings[x, y].TileLayoutMap = 9;
				return;
			}
		}
		if (array[2] && array[3])
		{
			if (array2[2] == -2 && array2[3] == 2)
			{
				mappings[x, y].TileLayoutMap = 6;
			}
			else if (array2[2] == 2 && array2[3] == -2)
			{
				mappings[x, y].TileLayoutMap = 7;
			}
		}
	}

	private bool isTileWideOpen(int x, int y)
	{
		return mappings[x, y].TileLayoutMap == 1 || (mappings[x, y].TileLayoutMap >= 6 && mappings[x, y].TileLayoutMap <= 9);
	}

	private int getHeight(int x, int y)
	{
		return mappings[x, y].FloorHeight;
	}

	private int getTileType(int x, int y)
	{
		return mappings[x, y].TileLayoutMap;
	}

	private void PlaceCorners()
	{
		for (int i = 1; i <= 62; i++)
		{
			for (int j = 1; j <= 62; j++)
			{
				if (getTileType(i, j) == 1)
				{
					if (getTileType(i + 1, j) == 0 && getTileType(i, j + 1) == 0)
					{
						mappings[i, j].TileLayoutMap = 3;
					}
					if (getTileType(i + 1, j) == 0 && getTileType(i, j - 1) == 0)
					{
						mappings[i, j].TileLayoutMap = 5;
					}
					if (getTileType(i - 1, j) == 0 && getTileType(i, j + 1) == 0)
					{
						mappings[i, j].TileLayoutMap = 2;
					}
					if (getTileType(i - 1, j) == 0 && getTileType(i, j - 1) == 0)
					{
						mappings[i, j].TileLayoutMap = 4;
					}
				}
			}
		}
	}
}
