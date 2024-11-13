using UnityEngine;

public class TileMap : Loader
{
	public struct Overlay
	{
		public int header;

		public int link;

		public int duration;

		public int tileX;

		public int tileY;
	}

	public const short TILE_SOLID = 0;

	public const short TILE_OPEN = 1;

	public const short TILE_DIAG_SE = 2;

	public const short TILE_DIAG_SW = 3;

	public const short TILE_DIAG_NE = 4;

	public const short TILE_DIAG_NW = 5;

	public const short TILE_SLOPE_N = 6;

	public const short TILE_SLOPE_S = 7;

	public const short TILE_SLOPE_E = 8;

	public const short TILE_SLOPE_W = 9;

	public const short TILE_VALLEY_NW = 10;

	public const short TILE_VALLEY_NE = 11;

	public const short TILE_VALLEY_SE = 12;

	public const short TILE_VALLEY_SW = 13;

	public const short TILE_RIDGE_SE = 14;

	public const short TILE_RIDGE_SW = 15;

	public const short TILE_RIDGE_NW = 16;

	public const short TILE_RIDGE_NE = 17;

	public const short TileMapSizeX = 63;

	public const short TileMapSizeY = 63;

	public const short ObjectStorageTile = 99;

	public const short SURFACE_FLOOR = 1;

	public const short SURFACE_CEIL = 2;

	public const short SURFACE_WALL = 3;

	public const short SURFACE_SLOPE = 4;

	public const short SLOPE_BOTH_PARALLEL = 0;

	public const short SLOPE_BOTH_OPPOSITE = 1;

	public const short SLOPE_FLOOR_ONLY = 2;

	public const short SLOPE_CEILING_ONLY = 3;

	public const short vTOP = 0;

	public const short vEAST = 1;

	public const short vBOTTOM = 2;

	public const short vWEST = 3;

	public const short vNORTH = 4;

	public const short vSOUTH = 5;

	private const short fSELF = 128;

	private const short fCEIL = 64;

	private const short fNORTH = 32;

	private const short fSOUTH = 16;

	private const short fEAST = 8;

	private const short fWEST = 4;

	private const short fTOP = 2;

	private const short fBOTTOM = 1;

	public const int UW1_TEXTUREMAPSIZE = 64;

	public const int UW2_TEXTUREMAPSIZE = 70;

	public const int UWDEMO_TEXTUREMAPSIZE = 63;

	public const int UW1_NO_OF_LEVELS = 9;

	public const int UW2_NO_OF_LEVELS = 80;

	public Overlay[] Overlays = new Overlay[64];

	public short thisLevelNo;

	public short UW_CEILING_HEIGHT;

	public short CEILING_HEIGHT;

	public short SHOCK_CEILING_HEIGHT;

	public short[] texture_map = new short[272];

	public TileInfo[,] Tiles = new TileInfo[64, 64];

	public static short visitTileX;

	public static short visitTileY;

	public static short visitedTileX;

	public static short visitedTileY;

	public TileMap(short NewLevelNo)
	{
		thisLevelNo = NewLevelNo;
	}

	public bool ValidTile(Vector3 location)
	{
		int num = (int)(location.x / 1.2f);
		int num2 = (int)(location.y / 1.2f);
		if (num > 63 || num < 0 || num2 > 63 || num2 < 0)
		{
			return false;
		}
		int tileType = GetTileType(num, num2);
		bool tileRender = GetTileRender(num, num2);
		return tileType != 0 && tileRender;
	}

	public static bool ValidTile(int tileX, int tileY)
	{
		return tileX >= 0 && tileX <= 63 && tileY >= 0 && tileY <= 63;
	}

	public static bool isTileOpen(int TileType)
	{
		switch (TileType)
		{
		case 1:
		case 6:
		case 7:
		case 8:
		case 9:
			return true;
		default:
			return false;
		}
	}

	public int GetFloorHeight(int tileX, int tileY)
	{
		if (ValidTile(tileX, tileY))
		{
			return Tiles[tileX, tileY].floorHeight;
		}
		return 0;
	}

	public int GetCeilingHeight(int tileX, int tileY)
	{
		return Tiles[tileX, tileY].ceilingHeight;
	}

	public void SetFloorHeight(int tileX, int tileY, short newHeight)
	{
		Tiles[tileX, tileY].floorHeight = newHeight;
	}

	public void SetCeilingHeight(int tileX, int tileY, short newHeight)
	{
		Tiles[tileX, tileY].ceilingHeight = newHeight;
	}

	public int GetTileType(int tileX, int tileY)
	{
		if (ValidTile(tileX, tileY))
		{
			return 0;
		}
		return Tiles[tileX, tileY].tileType;
	}

	public int GetRoom(int tileX, int tileY)
	{
		if (ValidTile(tileX, tileY))
		{
			return Tiles[tileX, tileY].roomRegion;
		}
		return 0;
	}

	private bool GetTileRender(int tileX, int tileY)
	{
		return Tiles[tileX, tileY].Render;
	}

	public Vector3 getTileVector(int tileX, int tileY)
	{
		return new Vector3((float)tileX * 1.2f + 0.6f, (float)GetFloorHeight(tileX, tileY) * 0.15f, (float)tileY * 1.2f + 0.6f);
	}

	public Vector3 getTileVector(int tileX, int tileY, float zpos)
	{
		return new Vector3((float)tileX * 1.2f + 0.6f, zpos, (float)tileY * 1.2f + 0.6f);
	}

	public bool BuildTileMapUW(int levelNo, DataLoader.UWBlock lev_ark, DataLoader.UWBlock tex_ark, DataLoader.UWBlock ovl_ark)
	{
		long num = 0L;
		short CeilingTexture = 0;
		UW_CEILING_HEIGHT = 32;
		CEILING_HEIGHT = UW_CEILING_HEIGHT;
		BuildTextureMap(tex_ark, ref CeilingTexture, levelNo);
		for (short num2 = 0; num2 <= 63; num2++)
		{
			for (short num3 = 0; num3 <= 63; num3++)
			{
				int firstTileInt = (int)DataLoader.getValAtAddress(lev_ark, num, 16);
				int secondTileInt = (int)DataLoader.getValAtAddress(lev_ark, num + 2, 16);
				Tiles[num3, num2] = new TileInfo(this, num3, num2, firstTileInt, secondTileInt, CeilingTexture);
				num += 4;
			}
		}
		SetTileMapWallFacesUW();
		switch (UWClass._RES)
		{
		case "UW1":
		{
			if (ovl_ark.DataLen == 0)
			{
				break;
			}
			long num5 = 0L;
			for (int j = 0; j < 64; j++)
			{
				Overlays[j].header = (int)DataLoader.getValAtAddress(ovl_ark, num5, 16);
				Overlays[j].link = (int)(DataLoader.getValAtAddress(ovl_ark, num5, 16) >> 6) & 0x3FF;
				Overlays[j].duration = (int)DataLoader.getValAtAddress(ovl_ark, num5 + 2, 16);
				Overlays[j].tileX = (int)DataLoader.getValAtAddress(ovl_ark, num5 + 4, 8);
				Overlays[j].tileY = (int)DataLoader.getValAtAddress(ovl_ark, num5 + 5, 8);
				if (Overlays[j].link != 0)
				{
				}
				num5 += 6;
			}
			break;
		}
		case "UW2":
		{
			long num4 = 31752L;
			for (int i = 0; i < 64; i++)
			{
				if (num4 + 5 <= lev_ark.Data.GetUpperBound(0))
				{
					Overlays[i].header = (int)DataLoader.getValAtAddress(lev_ark, num4, 16);
					Overlays[i].link = (int)(DataLoader.getValAtAddress(lev_ark, num4, 16) >> 6) & 0x3FF;
					Overlays[i].duration = (int)DataLoader.getValAtAddress(lev_ark, num4 + 2, 16);
					Overlays[i].tileX = (int)DataLoader.getValAtAddress(lev_ark, num4 + 4, 8);
					Overlays[i].tileY = (int)DataLoader.getValAtAddress(lev_ark, num4 + 5, 8);
					if (Overlays[i].link == 0)
					{
					}
				}
				num4 += 6;
			}
			break;
		}
		}
		return true;
	}

	public void SetTileMapWallFacesUW()
	{
		for (short num = 0; num <= 63; num++)
		{
			for (short num2 = 0; num2 <= 63; num2++)
			{
				SetTileWallFacesUW(num2, num);
			}
		}
	}

	public void SetTileWallFacesUW(short x, short y)
	{
		if (Tiles[x, y].tileType >= 0)
		{
			if (y < 63)
			{
				Tiles[x, y].North = Tiles[x, y + 1].wallTexture;
			}
			else
			{
				Tiles[x, y].North = -1;
			}
			if (y > 0)
			{
				Tiles[x, y].South = Tiles[x, y - 1].wallTexture;
			}
			else
			{
				Tiles[x, y].South = -1;
			}
			if (x < 63)
			{
				Tiles[x, y].East = Tiles[x + 1, y].wallTexture;
			}
			else
			{
				Tiles[x, y].East = -1;
			}
			if (x > 0)
			{
				Tiles[x, y].West = Tiles[x - 1, y].wallTexture;
			}
			else
			{
				Tiles[x, y].West = -1;
			}
		}
	}

	public bool BuildTileMapShock(char[] archive_ark, int LevelNo)
	{
		long num = 4L;
		DataLoader.Chunk data_ark;
		if (!DataLoader.LoadChunk(archive_ark, LevelNo * 100 + 4004, out data_ark))
		{
			return false;
		}
		int num2 = (int)DataLoader.getValAtAddress(data_ark.data, 16L, 32);
		if (num2 > 3)
		{
			num2 = 3;
		}
		SHOCK_CEILING_HEIGHT = (short)((256 >> num2) * 8 >> 3);
		CEILING_HEIGHT = SHOCK_CEILING_HEIGHT;
		DataLoader.Chunk data_ark2;
		if (!DataLoader.LoadChunk(archive_ark, LevelNo * 100 + 4005, out data_ark2))
		{
			return false;
		}
		DataLoader.Chunk data_ark3;
		if (!DataLoader.LoadChunk(archive_ark, LevelNo * 100 + 4007, out data_ark3))
		{
			return false;
		}
		num = 0L;
		for (long num3 = 0L; num3 < data_ark3.chunkUnpackedLength / 2; num3++)
		{
			texture_map[num3] = (short)DataLoader.getValAtAddress(data_ark3.data, num, 16);
			num += 2;
		}
		num = 0L;
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				Tiles[j, i] = new TileInfo();
				Tiles[j, i].tileX = (short)j;
				Tiles[j, i].tileY = (short)i;
				Tiles[j, i].tileType = (short)data_ark2.data[num];
				switch (Tiles[j, i].tileType)
				{
				case 4:
					Tiles[j, i].tileType = 5;
					break;
				case 5:
					Tiles[j, i].tileType = 4;
					break;
				case 7:
					Tiles[j, i].tileType = 8;
					break;
				case 8:
					Tiles[j, i].tileType = 7;
					break;
				}
				Tiles[j, i].indexObjectList = 0;
				Tiles[j, i].Render = true;
				Tiles[j, i].DimX = 1;
				Tiles[j, i].DimY = 1;
				Tiles[j, i].Grouped = false;
				for (int k = 0; k < 6; k++)
				{
					Tiles[j, i].VisibleFaces[k] = true;
				}
				Tiles[j, i].wallTexture = (short)(DataLoader.getValAtAddress(data_ark2.data, num + 6, 16) & 0x3F);
				Tiles[j, i].shockCeilingTexture = (short)((DataLoader.getValAtAddress(data_ark2.data, num + 6, 16) >> 6) & 0x1F);
				Tiles[j, i].floorTexture = (short)((DataLoader.getValAtAddress(data_ark2.data, num + 6, 16) >> 11) & 0x1F);
				Tiles[j, i].North = Tiles[j, i].wallTexture;
				Tiles[j, i].South = Tiles[j, i].wallTexture;
				Tiles[j, i].East = Tiles[j, i].wallTexture;
				Tiles[j, i].West = Tiles[j, i].wallTexture;
				Tiles[j, i].floorHeight = (short)(data_ark2.data[num + 1] & 0x1F);
				Tiles[j, i].floorHeight = (short)((Tiles[j, i].floorHeight << 3 >> num2) * 8 >> 3);
				Tiles[j, i].ceilingHeight = (short)(data_ark2.data[num + 2] & 0x1F);
				Tiles[j, i].ceilingHeight = (short)((Tiles[j, i].ceilingHeight << 3 >> num2) * 8 >> 3);
				Tiles[j, i].shockFloorOrientation = (short)(((int)data_ark2.data[num + 1] >> 5) & 3);
				Tiles[j, i].shockCeilOrientation = (short)(((int)data_ark2.data[num + 2] >> 5) & 3);
				Tiles[j, i].shockNorthCeilHeight = Tiles[j, i].ceilingHeight;
				Tiles[j, i].shockSouthCeilHeight = Tiles[j, i].ceilingHeight;
				Tiles[j, i].shockEastCeilHeight = Tiles[j, i].ceilingHeight;
				Tiles[j, i].shockWestCeilHeight = Tiles[j, i].ceilingHeight;
				Tiles[j, i].shockSteep = (short)(data_ark2.data[num + 3] & 0xF);
				Tiles[j, i].shockSteep = (short)((Tiles[j, i].shockSteep << 3 >> num2) * 8 >> 3);
				if (Tiles[j, i].shockSteep == 0 && Tiles[j, i].tileType >= 6)
				{
					Tiles[j, i].tileType = 1;
				}
				if (Tiles[j, i].tileType == 1 && Tiles[j, i].shockSteep > 0)
				{
					Tiles[j, i].shockSteep = 0;
				}
				Tiles[j, i].indexObjectList = (int)DataLoader.getValAtAddress(data_ark2.data, num + 4, 16);
				Tiles[j, i].shockSlopeFlag = (short)((DataLoader.getValAtAddress(data_ark2.data, num + 8, 32) >> 10) & 3);
				Tiles[j, i].UseAdjacentTextures = (short)((DataLoader.getValAtAddress(data_ark2.data, num + 8, 32) >> 8) & 1);
				Tiles[j, i].shockTextureOffset = (short)(DataLoader.getValAtAddress(data_ark2.data, num + 8, 32) & 0xF);
				Tiles[j, i].shockShadeLower = (short)(((int)DataLoader.getValAtAddress(data_ark2.data, num + 8, 32) >> 16) & 0xF);
				Tiles[j, i].shockShadeUpper = (short)(((int)DataLoader.getValAtAddress(data_ark2.data, num + 8, 32) >> 24) & 0xF);
				Tiles[j, i].shockNorthOffset = Tiles[j, i].shockTextureOffset;
				Tiles[j, i].shockSouthOffset = Tiles[j, i].shockTextureOffset;
				Tiles[j, i].shockEastOffset = Tiles[j, i].shockTextureOffset;
				Tiles[j, i].shockWestOffset = Tiles[j, i].shockTextureOffset;
				Tiles[j, i].SHOCKSTATE[0] = (int)DataLoader.getValAtAddress(data_ark2.data, num + 12, 8);
				Tiles[j, i].SHOCKSTATE[1] = (int)DataLoader.getValAtAddress(data_ark2.data, num + 13, 8);
				Tiles[j, i].SHOCKSTATE[2] = (int)DataLoader.getValAtAddress(data_ark2.data, num + 14, 8);
				Tiles[j, i].SHOCKSTATE[3] = (int)DataLoader.getValAtAddress(data_ark2.data, num + 15, 8);
				num += 16;
			}
		}
		for (int l = 1; l < 63; l++)
		{
			for (int m = 1; m < 63; m++)
			{
				if (Tiles[m + 1, l].UseAdjacentTextures != 1)
				{
					Tiles[m, l].East = Tiles[m + 1, l].wallTexture;
					Tiles[m, l].shockEastOffset = Tiles[m + 1, l].shockTextureOffset;
				}
				if (Tiles[m - 1, l].UseAdjacentTextures != 1)
				{
					Tiles[m, l].West = Tiles[m - 1, l].wallTexture;
					Tiles[m, l].shockWestOffset = Tiles[m - 1, l].shockTextureOffset;
				}
				if (Tiles[m, l + 1].UseAdjacentTextures != 1)
				{
					Tiles[m, l].North = Tiles[m, l + 1].wallTexture;
					Tiles[m, l].shockNorthOffset = Tiles[m, l + 1].shockTextureOffset;
				}
				if (Tiles[m, l - 1].UseAdjacentTextures != 1)
				{
					Tiles[m, l].South = Tiles[m, l - 1].wallTexture;
					Tiles[m, l].shockSouthOffset = Tiles[m, l - 1].shockTextureOffset;
				}
				Tiles[m, l].shockEastCeilHeight = (short)CalcNeighbourCeilHeight(Tiles[m, l], Tiles[m + 1, l], 8);
				Tiles[m, l].shockWestCeilHeight = (short)CalcNeighbourCeilHeight(Tiles[m, l], Tiles[m - 1, l], 4);
				Tiles[m, l].shockNorthCeilHeight = (short)CalcNeighbourCeilHeight(Tiles[m, l], Tiles[m, l + 1], 32);
				Tiles[m, l].shockSouthCeilHeight = (short)CalcNeighbourCeilHeight(Tiles[m, l], Tiles[m, l - 1], 16);
			}
		}
		return true;
	}

	public void CleanUp(string game)
	{
		if (!GameWorldController.instance.DoCleanUp)
		{
			return;
		}
		int num;
		for (num = 0; num <= 63; num++)
		{
			for (int i = 0; i <= 63; i++)
			{
				if (Tiles[num, i].tileType == 0)
				{
					Tiles[num, i].VisibleFaces[2] = false;
					Tiles[num, i].VisibleFaces[0] = false;
				}
				else
				{
					Tiles[num, i].VisibleFaces[2] = false;
					Tiles[num, i].VisibleFaces[0] = false;
				}
			}
			for (num = 0; num <= 63; num++)
			{
				for (int i = 0; i <= 63; i++)
				{
					if (Tiles[num, i].tileType != 0 || Tiles[num, i].indexObjectList != 0 || Tiles[num, i].TerrainChange)
					{
						continue;
					}
					switch (i)
					{
					case 0:
						switch (num)
						{
						case 0:
							if (Tiles[num + 1, i].tileType == 0 && Tiles[num, i + 1].tileType == 0 && !Tiles[num + 1, i].TerrainChange && !Tiles[num, i + 1].TerrainChange)
							{
								Tiles[num, i].Render = false;
							}
							else
							{
								Tiles[num, i].Render = true;
							}
							break;
						case 63:
							if (Tiles[num - 1, i].tileType == 0 && Tiles[num, i + 1].tileType == 0 && !Tiles[num - 1, i].TerrainChange && !Tiles[num, i + 1].TerrainChange)
							{
								Tiles[num, i].Render = false;
							}
							else
							{
								Tiles[num, i].Render = true;
							}
							break;
						default:
							if (Tiles[num + 1, i].tileType == 0 && Tiles[num, i + 1].tileType == 0 && Tiles[num + 1, i].tileType == 0 && !Tiles[num + 1, i].TerrainChange && !Tiles[num, i + 1].TerrainChange && !Tiles[num + 1, i].TerrainChange)
							{
								Tiles[num, i].Render = false;
							}
							else
							{
								Tiles[num, i].Render = true;
							}
							break;
						}
						continue;
					case 63:
						switch (num)
						{
						case 0:
							if (Tiles[num + 1, i].tileType == 0 && Tiles[num, i - 1].tileType == 0 && !Tiles[num + 1, i].TerrainChange && !Tiles[num, i - 1].TerrainChange)
							{
								Tiles[num, i].Render = false;
							}
							else
							{
								Tiles[num, i].Render = true;
							}
							break;
						case 63:
							if (Tiles[num - 1, i].tileType == 0 && Tiles[num, i - 1].tileType == 0 && !Tiles[num - 1, i].TerrainChange && !Tiles[num, i - 1].TerrainChange)
							{
								Tiles[num, i].Render = false;
							}
							else
							{
								Tiles[num, i].Render = true;
							}
							break;
						default:
							if (Tiles[num + 1, i].tileType == 0 && Tiles[num, i - 1].tileType == 0 && Tiles[num - 1, i].tileType == 0 && !Tiles[num + 1, i].TerrainChange && !Tiles[num, i - 1].TerrainChange && !Tiles[num - 1, i].TerrainChange)
							{
								Tiles[num, i].Render = false;
							}
							else
							{
								Tiles[num, i].Render = true;
							}
							break;
						}
						continue;
					}
					switch (num)
					{
					case 0:
						if (Tiles[num, i + 1].tileType == 0 && Tiles[num + 1, i].tileType == 0 && Tiles[num, i - 1].tileType == 0 && !Tiles[num, i + 1].TerrainChange && !Tiles[num + 1, i].TerrainChange && !Tiles[num, i - 1].TerrainChange)
						{
							Tiles[num, i].Render = false;
						}
						else
						{
							Tiles[num, i].Render = true;
						}
						break;
					case 63:
						if (Tiles[num, i + 1].tileType == 0 && Tiles[num - 1, i].tileType == 0 && Tiles[num, i - 1].tileType == 0 && !Tiles[num, i + 1].TerrainChange && !Tiles[num - 1, i].TerrainChange && !Tiles[num, i - 1].TerrainChange)
						{
							Tiles[num, i].Render = false;
						}
						else
						{
							Tiles[num, i].Render = true;
						}
						break;
					default:
						if (Tiles[num, i + 1].tileType == 0 && Tiles[num + 1, i].tileType == 0 && Tiles[num, i - 1].tileType == 0 && Tiles[num - 1, i].tileType == 0 && !Tiles[num, i + 1].TerrainChange && !Tiles[num + 1, i].TerrainChange && !Tiles[num, i - 1].TerrainChange && !Tiles[num - 1, i].TerrainChange)
						{
							Tiles[num, i].Render = false;
						}
						else
						{
							Tiles[num, i].Render = true;
						}
						break;
					}
				}
			}
		}
		if (game == "SHOCK")
		{
			return;
		}
		int num2 = 1;
		for (num = 0; num < 63; num++)
		{
			for (int i = 0; i < 63; i++)
			{
				if (!Tiles[num, i].Grouped)
				{
					num2 = 1;
					while (Tiles[num, i].Render && Tiles[num, i + num2].Render && !Tiles[num, i + num2].Grouped && DoTilesMatch(Tiles[num, i], Tiles[num, i + num2]))
					{
						Tiles[num, i + num2].Render = false;
						Tiles[num, i + num2].Grouped = true;
						Tiles[num, i].Grouped = true;
						num2++;
					}
					Tiles[num, i].DimY = (short)(Tiles[num, i].DimY + num2 - 1);
					num2 = 1;
				}
			}
		}
		num2 = 1;
		for (int i = 0; i < 63; i++)
		{
			for (num = 0; num < 63; num++)
			{
				if (!Tiles[num, i].Grouped)
				{
					num2 = 1;
					while (Tiles[num, i].Render && Tiles[num + num2, i].Render && !Tiles[num + num2, i].Grouped && DoTilesMatch(Tiles[num, i], Tiles[num + num2, i]))
					{
						Tiles[num + num2, i].Render = false;
						Tiles[num + num2, i].Grouped = true;
						Tiles[num, i].Grouped = true;
						num2++;
					}
					Tiles[num, i].DimX = (short)(Tiles[num, i].DimX + num2 - 1);
					num2 = 1;
				}
			}
		}
		for (int i = 0; i <= 63; i++)
		{
			for (num = 0; num <= 63; num++)
			{
				if (Tiles[num, i].tileType != 0)
				{
					continue;
				}
				int dimX = Tiles[num, i].DimX;
				int dimY = Tiles[num, i].DimY;
				if (num == 0)
				{
					Tiles[num, i].VisibleFaces[3] = false;
				}
				if (num == 63)
				{
					Tiles[num, i].VisibleFaces[1] = false;
				}
				if (i == 0)
				{
					Tiles[num, i].VisibleFaces[5] = false;
				}
				if (i == 63)
				{
					Tiles[num, i].VisibleFaces[4] = false;
				}
				if (num + dimX <= 63 && i + dimY <= 63)
				{
					if (Tiles[num + dimX, i].tileType == 0 && !Tiles[num + dimX, i].TerrainChange && !Tiles[num, i].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[1] = false;
						Tiles[num + dimX, i].VisibleFaces[3] = false;
					}
					if (Tiles[num, i + dimY].tileType == 0 && !Tiles[num, i].TerrainChange && !Tiles[num, i + dimY].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[4] = false;
						Tiles[num, i + dimY].VisibleFaces[5] = false;
					}
				}
			}
		}
		for (int i = 1; i < 63; i++)
		{
			for (num = 1; num < 63; num++)
			{
				switch (Tiles[num, i].tileType)
				{
				case 5:
					if (Tiles[num, i - 1].tileType == 0 && !Tiles[num, i - 1].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[5] = false;
						Tiles[num, i - 1].VisibleFaces[4] = false;
					}
					if (Tiles[num + 1, i].tileType == 0 && !Tiles[num + 1, i].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[1] = false;
						Tiles[num + 1, i].VisibleFaces[3] = false;
					}
					break;
				case 4:
					if (Tiles[num, i - 1].tileType == 0 && !Tiles[num, i - 1].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[5] = false;
						Tiles[num, i - 1].VisibleFaces[4] = false;
					}
					if (Tiles[num - 1, i].tileType == 0 && !Tiles[num - 1, i].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[3] = false;
						Tiles[num - 1, i].VisibleFaces[1] = false;
					}
					break;
				case 2:
					if (Tiles[num, i + 1].tileType == 0 && !Tiles[num, i + 1].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[4] = false;
						Tiles[num, i + 1].VisibleFaces[5] = false;
					}
					if (Tiles[num - 1, i].tileType == 0 && !Tiles[num - 1, i].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[3] = false;
						Tiles[num - 1, i].VisibleFaces[1] = false;
					}
					break;
				case 3:
					if (Tiles[num, i + 1].tileType == 0 && !Tiles[num, i + 1].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[4] = false;
						Tiles[num, i + 1].VisibleFaces[5] = false;
					}
					if (Tiles[num + 1, i].tileType == 0 && !Tiles[num + 1, i].TerrainChange)
					{
						Tiles[num, i].VisibleFaces[1] = false;
						Tiles[num + 1, i].VisibleFaces[3] = false;
					}
					break;
				}
			}
		}
		for (int i = 1; i < 63; i++)
		{
			for (num = 1; num < 63; num++)
			{
				if (Tiles[num, i].tileType == 1 && !Tiles[num, i].TerrainChange)
				{
					if ((Tiles[num + 1, i].tileType == 1 && !Tiles[num + 1, i].TerrainChange && Tiles[num + 1, i].floorHeight >= Tiles[num, i].floorHeight) || (Tiles[num + 1, i].tileType == 0 && !Tiles[num + 1, i].TerrainChange))
					{
						Tiles[num, i].VisibleFaces[1] = false;
					}
					if ((Tiles[num - 1, i].tileType == 1 && !Tiles[num - 1, i].TerrainChange && Tiles[num - 1, i].floorHeight >= Tiles[num, i].floorHeight) || (Tiles[num - 1, i].tileType == 0 && !Tiles[num - 1, i].TerrainChange))
					{
						Tiles[num, i].VisibleFaces[3] = false;
					}
					if ((Tiles[num, i + 1].tileType == 1 && !Tiles[num, i + 1].TerrainChange && Tiles[num, i + 1].floorHeight >= Tiles[num, i].floorHeight) || (Tiles[num, i + 1].tileType == 0 && !Tiles[num, i + 1].TerrainChange))
					{
						Tiles[num, i].VisibleFaces[4] = false;
					}
					if ((Tiles[num, i - 1].tileType == 1 && !Tiles[num, i - 1].TerrainChange && Tiles[num, i - 1].floorHeight >= Tiles[num, i].floorHeight) || (Tiles[num, i - 1].tileType == 0 && !Tiles[num, i - 1].TerrainChange))
					{
						Tiles[num, i].VisibleFaces[5] = false;
					}
				}
			}
		}
		for (int i = 1; i < 63; i++)
		{
			for (num = 1; num < 63; num++)
			{
				if (Tiles[num, i].tileType != 0 && Tiles[num, i].tileType != 1)
				{
					continue;
				}
				int dimX2 = Tiles[num, i].DimX;
				int dimY2 = Tiles[num, i].DimY;
				if (dimX2 > 1)
				{
					Tiles[num, i].VisibleFaces[1] = Tiles[num + dimX2 - 1, i].VisibleFaces[1];
				}
				if (dimY2 > 1)
				{
					Tiles[num, i].VisibleFaces[4] = Tiles[num, i + dimY2 - 1].VisibleFaces[4];
				}
				for (int j = 0; j < Tiles[num, i].DimX; j++)
				{
					if (Tiles[num + j, i].VisibleFaces[4])
					{
						Tiles[num, i].VisibleFaces[4] = true;
					}
					if (Tiles[num + j, i].VisibleFaces[5])
					{
						Tiles[num, i].VisibleFaces[5] = true;
					}
				}
				for (int k = 0; k < Tiles[num, i].DimY; k++)
				{
					if (Tiles[num, i + k].VisibleFaces[1])
					{
						Tiles[num, i].VisibleFaces[1] = true;
					}
					if (Tiles[num, i + k].VisibleFaces[3])
					{
						Tiles[num, i].VisibleFaces[3] = true;
					}
				}
			}
		}
		for (int i = 0; i <= 63; i++)
		{
			Tiles[0, i].VisibleFaces[1] = true;
			Tiles[63, i].VisibleFaces[3] = true;
		}
		for (num = 0; num <= 63; num++)
		{
			Tiles[num, 0].VisibleFaces[4] = true;
			Tiles[num, 63].VisibleFaces[5] = true;
		}
	}

	private bool DoTilesMatch(TileInfo t1, TileInfo t2)
	{
		if (UWClass._RES == "SHOCK")
		{
			return false;
		}
		if (t1.tileType > 1 || t1.TerrainChange || t2.TerrainChange)
		{
			return false;
		}
		if (t1.tileType == 0 && t2.tileType == 0)
		{
			return t1.wallTexture == t2.wallTexture && t1.West == t2.West && t1.South == t2.South && t1.East == t2.East && t1.North == t2.North && t1.UseAdjacentTextures == t2.UseAdjacentTextures;
		}
		return t1.shockCeilingTexture == t2.shockCeilingTexture && t1.floorTexture == t2.floorTexture && t1.floorHeight == t2.floorHeight && t1.ceilingHeight == t2.ceilingHeight && t1.DimX == t2.DimX && t1.DimY == t2.DimY && t1.wallTexture == t2.wallTexture && t1.tileType == t2.tileType && !t1.isDoor && !t2.isDoor;
	}

	public static bool isTerrainWater(int terraintype)
	{
		if (terraintype == 16 || terraintype == 64 || terraintype == 72 || terraintype == 80 || terraintype == 88 || terraintype == 96)
		{
			return true;
		}
		return false;
	}

	public static bool isTerrainLava(int terraintype)
	{
		if (terraintype == 32 || terraintype == 128)
		{
			return true;
		}
		return false;
	}

	public static bool isTerrainIce(int terraintype)
	{
		if (terraintype == 192 || terraintype == 248 || terraintype == 232)
		{
			return true;
		}
		return false;
	}

	public static bool isTextureNothing(int textureNo)
	{
		if (textureNo == 236)
		{
			return true;
		}
		return false;
	}

	private int CalcNeighbourCeilHeight(TileInfo t1, TileInfo t2, int Direction)
	{
		if (t2.tileType <= 1 || t2.shockSlopeFlag == 2)
		{
			return t2.ceilingHeight;
		}
		switch (Direction)
		{
		case 32:
		{
			short tileType2 = t2.tileType;
			if (tileType2 == 6 || tileType2 == 7)
			{
				if (t2.shockSlopeFlag == 1 || t2.shockSlopeFlag == 3)
				{
					return t2.ceilingHeight + t2.shockSteep;
				}
				return t2.ceilingHeight;
			}
			return t2.ceilingHeight;
		}
		case 16:
		{
			short tileType4 = t2.tileType;
			if (tileType4 == 7 || tileType4 == 6)
			{
				if (t2.shockSlopeFlag == 1 || t2.shockSlopeFlag == 3)
				{
					return t2.ceilingHeight + t2.shockSteep;
				}
				return t2.ceilingHeight;
			}
			return t2.ceilingHeight;
		}
		case 8:
		{
			short tileType3 = t2.tileType;
			if (tileType3 == 8 || tileType3 == 9)
			{
				if (t2.shockSlopeFlag == 1 || t2.shockSlopeFlag == 3)
				{
					return t2.ceilingHeight + t2.shockSteep;
				}
				return t2.ceilingHeight;
			}
			return t2.ceilingHeight;
		}
		case 4:
		{
			short tileType = t2.tileType;
			if (tileType == 9 || tileType == 8)
			{
				if (t2.shockSlopeFlag == 1 || t2.shockSlopeFlag == 3)
				{
					return t2.ceilingHeight + t2.shockSteep;
				}
				return t2.ceilingHeight;
			}
			return t2.ceilingHeight;
		}
		default:
			return t2.ceilingHeight;
		}
	}

	public char[] TileMapToBytes(char[] lev_ark_file_data, out long datalen)
	{
		char[] array = new char[31752];
		DataLoader.UWBlock uwb = default(DataLoader.UWBlock);
		DataLoader.LoadUWBlock(lev_ark_file_data, thisLevelNo, 31752L, out uwb);
		for (int i = 31484; i < 31752; i++)
		{
			array[i] = uwb.Data[i];
		}
		datalen = uwb.DataLen;
		long num = 0L;
		for (int j = 0; j <= 63; j++)
		{
			for (int k = 0; k <= 63; k++)
			{
				TileInfo tileInfo = Tiles[k, j];
				int tileType = tileInfo.tileType;
				int num2 = tileInfo.floorHeight / 2 << 4;
				int num3 = tileType | num2;
				array[num] = (char)num3;
				int num4 = tileInfo.flags & 3;
				int num5 = tileInfo.floorTexture << 2;
				int num6 = tileInfo.noMagic << 6;
				int num7 = tileInfo.doorBit << 7;
				num3 = num5 | num6 | num7 | num4;
				array[num + 1] = (char)num3;
				num3 = ((tileInfo.indexObjectList & 0x3FF) << 6) | (tileInfo.wallTexture & 0x3F);
				array[num + 2] = (char)((uint)num3 & 0xFFu);
				array[num + 3] = (char)((uint)(num3 >> 8) & 0xFFu);
				num += 4;
			}
		}
		for (int l = 0; l <= GameWorldController.instance.objectList[thisLevelNo].objInfo.GetUpperBound(0); l++)
		{
			ObjectLoaderInfo objectLoaderInfo = GameWorldController.instance.objectList[thisLevelNo].objInfo[l];
			if (objectLoaderInfo == null)
			{
				continue;
			}
			if (IsObjectFree(l))
			{
				array[num] = '\0';
				array[num + 1] = '\0';
				array[num + 2] = '\0';
				array[num + 3] = '\0';
				array[num + 4] = '\0';
				array[num + 5] = '\0';
				array[num + 6] = '\0';
				array[num + 7] = '\0';
				if (l < 256)
				{
					array[num + 8] = '\0';
					array[num + 9] = '\0';
					array[num + 10] = '\0';
					array[num + 11] = '\0';
					array[num + 12] = '\0';
					array[num + 13] = '\0';
					array[num + 14] = '\0';
					array[num + 15] = '\0';
					array[num + 16] = '\0';
					array[num + 17] = '\0';
					array[num + 18] = '\0';
					array[num + 19] = '\0';
					array[num + 20] = '\0';
					array[num + 21] = '\0';
					array[num + 22] = '\0';
					array[num + 23] = '\0';
					array[num + 24] = '\0';
					array[num + 25] = '\0';
					array[num + 26] = '\0';
					num = num + 8 + 19;
				}
				else
				{
					num += 8;
				}
				continue;
			}
			int num8 = (objectLoaderInfo.is_quant << 15) | (objectLoaderInfo.invis << 14) | (objectLoaderInfo.doordir << 13) | (objectLoaderInfo.enchantment << 12) | ((objectLoaderInfo.flags & 7) << 9) | (objectLoaderInfo.item_id & 0x1FF);
			array[num] = (char)((uint)num8 & 0xFFu);
			array[num + 1] = (char)((uint)(num8 >> 8) & 0xFFu);
			num8 = ((objectLoaderInfo.xpos & 7) << 13) | ((objectLoaderInfo.ypos & 7) << 10) | ((objectLoaderInfo.heading & 7) << 7) | (objectLoaderInfo.zpos & 0x7F);
			array[num + 2] = (char)((uint)num8 & 0xFFu);
			array[num + 3] = (char)((uint)(num8 >> 8) & 0xFFu);
			num8 = ((objectLoaderInfo.next & 0x3FF) << 6) | (objectLoaderInfo.quality & 0x3F);
			array[num + 4] = (char)((uint)num8 & 0xFFu);
			array[num + 5] = (char)((uint)(num8 >> 8) & 0xFFu);
			num8 = ((objectLoaderInfo.link & 0x3FF) << 6) | (objectLoaderInfo.owner & 0x3F);
			array[num + 6] = (char)((uint)num8 & 0xFFu);
			array[num + 7] = (char)((uint)(num8 >> 8) & 0xFFu);
			if (l < 256)
			{
				array[num + 8] = (char)objectLoaderInfo.npc_hp;
				array[num + 9] = (char)(((uint)objectLoaderInfo.ProjectileHeadingMajor & 0xE0u) | (ushort)((uint)objectLoaderInfo.ProjectileHeadingMinor & 0x1Fu));
				num8 = (objectLoaderInfo.npc_goal & 0xF) | ((objectLoaderInfo.npc_gtarg & 0xFF) << 4) | ((array[num + 11 + 1] & 0xF0) << 8);
				array[num + 11] = (char)((uint)num8 & 0xFFu);
				array[num + 11 + 1] = (char)((uint)(num8 >> 8) & 0xFFu);
				int num9 = (int)DataLoader.getValAtAddress(array, num + 13, 16);
				num9 &= 0x1FF0;
				num8 = ((objectLoaderInfo.npc_attitude & 3) << 14) | ((objectLoaderInfo.npc_talkedto & 1) << 13) | (objectLoaderInfo.npc_level & 0xF) | num9;
				array[num + 13] = (char)((uint)num8 & 0xFFu);
				array[num + 13 + 1] = (char)((uint)(num8 >> 8) & 0xFFu);
				array[num + 20] = (char)(((uint)(objectLoaderInfo.Projectile_Sign << 7) & 1u) | (uint)((objectLoaderInfo.Projectile_Pitch & 3) << 4) | ((uint)objectLoaderInfo.Projectile_Speed & 0xFu));
				num8 = ((objectLoaderInfo.npc_xhome & 0x3F) << 10) | ((objectLoaderInfo.npc_yhome & 0x3F) << 4) | (array[num + 22] & 0xF);
				array[num + 22] = (char)((uint)num8 & 0xFFu);
				array[num + 22 + 1] = (char)((uint)(num8 >> 8) & 0xFFu);
				num8 = (array[num + 24] & 0xE0) | (objectLoaderInfo.npc_heading & 0x1F);
				array[num + 24] = (char)((uint)num8 & 0xFFu);
				array[num + 24 + 1] = (char)((uint)(num8 >> 8) & 0xFFu);
				array[num + 25] = (char)((uint)objectLoaderInfo.npc_hunger & 0x3Fu);
				array[num + 26] = (char)((uint)objectLoaderInfo.npc_whoami & 0xFFu);
				num = num + 8 + 19;
			}
			else
			{
				num += 8;
			}
		}
		num = 29440L;
		int num10 = 0;
		for (int m = 0; m <= GameWorldController.instance.objectList[thisLevelNo].NoOfFreeMobile; m++)
		{
			int num11 = GameWorldController.instance.objectList[thisLevelNo].FreeMobileList[num10];
			array[num] = (char)((uint)num11 & 0xFFu);
			array[num + 1] = (char)((uint)(num11 >> 8) & 0xFFu);
			num10++;
			num += 2;
		}
		num = 29948L;
		num10 = 0;
		for (int n = 0; n <= GameWorldController.instance.objectList[thisLevelNo].NoOfFreeStatic; n++)
		{
			int num12 = GameWorldController.instance.objectList[thisLevelNo].FreeStaticList[num10];
			array[num] = (char)((uint)num12 & 0xFFu);
			array[num + 1] = (char)((uint)(num12 >> 8) & 0xFFu);
			num10++;
			num += 2;
		}
		array[31746] = (char)((uint)GameWorldController.instance.objectList[thisLevelNo].NoOfFreeMobile & 0xFFu);
		array[31747] = (char)((uint)(GameWorldController.instance.objectList[thisLevelNo].NoOfFreeMobile >> 8) & 0xFFu);
		array[31748] = (char)((uint)GameWorldController.instance.objectList[thisLevelNo].NoOfFreeStatic & 0xFFu);
		array[31749] = (char)((uint)(GameWorldController.instance.objectList[thisLevelNo].NoOfFreeStatic >> 8) & 0xFFu);
		return array;
	}

	private bool IsObjectFree(int index)
	{
		if (index < 256)
		{
			for (int i = 2; i <= GameWorldController.instance.objectList[thisLevelNo].NoOfFreeMobile; i++)
			{
				if (index == GameWorldController.instance.objectList[thisLevelNo].FreeMobileList[i])
				{
					return true;
				}
			}
		}
		else
		{
			for (int j = 0; j <= GameWorldController.instance.objectList[thisLevelNo].NoOfFreeStatic; j++)
			{
				if (index == GameWorldController.instance.objectList[thisLevelNo].FreeStaticList[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public char[] OverlayInfoToBytes()
	{
		char[] array = new char[384];
		int num = 0;
		for (int i = 0; i < 64; i++)
		{
			int num2 = Overlays[i].link << 6;
			array[num] = (char)((uint)num2 & 0xFFu);
			array[num + 1] = (char)((uint)(num2 >> 8) & 0xFFu);
			array[num + 2] = (char)((uint)Overlays[i].duration & 0xFFu);
			array[num + 3] = (char)((uint)(Overlays[i].duration >> 8) & 0xFFu);
			array[num + 4] = (char)((uint)Overlays[i].tileX & 0xFFu);
			array[num + 5] = (char)((uint)Overlays[i].tileY & 0xFFu);
			num += 6;
		}
		return array;
	}

	public char[] TextureMapToBytes()
	{
		char[] array = new char[122];
		short num = 64;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (i < 48)
			{
				array[num2] = (char)((uint)texture_map[i] & 0xFFu);
				array[num2 + 1] = (char)((uint)(texture_map[i] >> 8) & 0xFFu);
				num2 += 2;
			}
			else if (i <= 57)
			{
				array[num2] = (char)((uint)(texture_map[i] - 210) & 0xFFu);
				array[num2 + 1] = (char)((uint)(texture_map[i] - 210 >> 8) & 0xFFu);
				num2 += 2;
			}
			else
			{
				array[num2] = (char)texture_map[i];
				num2++;
			}
		}
		return array;
	}

	public void CreateRooms()
	{
		short num = 1;
		for (int num2 = 63; num2 >= 0; num2--)
		{
			for (int i = 0; i <= 63; i++)
			{
				Tiles[i, num2].roomRegion = 0;
			}
		}
		for (int num3 = 63; num3 >= 0; num3--)
		{
			for (int j = 0; j <= 63; j++)
			{
				if (Tiles[j, num3].tileType != 0 && Tiles[j, num3].roomRegion == 0)
				{
					Tiles[j, num3].roomRegion = num;
					fillRoomRegion(j, num3, TileTerrainType(j, num3), num);
					num++;
				}
			}
		}
	}

	private void fillRoomRegion(int startX, int startY, int terrainType, short RegionNo)
	{
		short floorHeight = Tiles[startX, startY].floorHeight;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (((i == -1 && j == 0) || (i == 1 && j == 0) || (i == 0 && j == -1) || (i == 0 && j == 1)) && ValidTile(startX + i, startY + j) && Tiles[startX + i, startY + j].tileType != 0 && Tiles[startX + i, startY + j].roomRegion == 0 && terrainType == TileTerrainType(startX + i, startY + j) && isTileOpenFromDirection(startX + i, startY + j, i, j) && ((floorHeight >= Tiles[startX + i, startY + j].floorHeight - 2 && floorHeight <= Tiles[startX + i, startY + j].floorHeight + 2) || ((Tiles[startX + i, startY + j].hasBridge || Tiles[startX, startY].hasBridge) && terrainType == 0)))
				{
					Tiles[startX + i, startY + j].roomRegion = RegionNo;
					fillRoomRegion(startX + i, startY + j, terrainType, RegionNo);
				}
			}
		}
	}

	private int TileTerrainType(int x, int y)
	{
		if (Tiles[x, y].isLand || Tiles[x, y].hasBridge)
		{
			return 0;
		}
		if (Tiles[x, y].isWater)
		{
			return 1;
		}
		if (Tiles[x, y].isLava)
		{
			return 2;
		}
		if (Tiles[x, y].isNothing)
		{
			return 3;
		}
		return 0;
	}

	private bool isTileOpenFromDirection(int X, int Y, int directionX, int directionY)
	{
		switch (Tiles[X, Y].tileType)
		{
		case 1:
		case 6:
		case 7:
		case 8:
		case 9:
			return true;
		case 4:
			if (directionX == 1 || directionY == -1)
			{
				return true;
			}
			break;
		case 5:
			if (directionX == 1 || directionY == 1)
			{
				return true;
			}
			break;
		case 2:
			if (directionX == -1 || directionY == -1)
			{
				return true;
			}
			break;
		case 3:
			if (directionX == -1 || directionY == 1)
			{
				return true;
			}
			break;
		default:
			return false;
		}
		return false;
	}

	public string getSignature()
	{
		string text = "";
		for (int i = 0; i < 63; i++)
		{
			for (int j = 0; j < 63; j++)
			{
				text = text + Tiles[i, j].tileType + Tiles[i, j].floorHeight;
			}
		}
		return text;
	}

	private void BuildTextureMap(DataLoader.UWBlock tex_ark, ref short CeilingTexture, int LevelNo)
	{
		short num;
		switch (UWClass._RES)
		{
		case "UW2":
			num = 70;
			break;
		case "UW0":
			num = 63;
			break;
		default:
			num = 64;
			break;
		}
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			switch (UWEBase._RES)
			{
			case "UW0":
				if (i < 48)
				{
					texture_map[i] = (short)DataLoader.getValAtAddress(tex_ark, num2, 16);
					num2 += 2;
				}
				else if (i <= 57)
				{
					texture_map[i] = (short)(DataLoader.getValAtAddress(tex_ark, num2, 16) + 48);
					num2 += 2;
					if (i == 57)
					{
						CeilingTexture = (short)i;
					}
				}
				else
				{
					texture_map[i] = (short)DataLoader.getValAtAddress(tex_ark, num2, 8);
					num2++;
				}
				break;
			case "UW1":
				if (i < 48)
				{
					texture_map[i] = (short)DataLoader.getValAtAddress(tex_ark, num2, 16);
					num2 += 2;
				}
				else if (i <= 57)
				{
					texture_map[i] = (short)(DataLoader.getValAtAddress(tex_ark, num2, 16) + 210);
					num2 += 2;
					if (i == 57)
					{
						CeilingTexture = (short)i;
					}
				}
				else
				{
					texture_map[i] = (short)DataLoader.getValAtAddress(tex_ark, num2, 8);
					num2++;
				}
				break;
			case "UW2":
				if (i < 64)
				{
					texture_map[i] = (short)DataLoader.getValAtAddress(tex_ark, num2, 16);
					num2 += 2;
				}
				else
				{
					texture_map[i] = (short)DataLoader.getValAtAddress(tex_ark, num2, 8);
					num2++;
				}
				if (i == 15)
				{
					CeilingTexture = (short)i;
				}
				if (LevelNo == 68 && i == 16)
				{
					CeilingTexture = (short)i;
				}
				break;
			}
		}
	}
}
