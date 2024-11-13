using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AutoMap : Loader
{
	public struct AutoMapTile
	{
		public short DisplayType;

		public short tileType;
	}

	public const int DisplayTypeClear = 0;

	public const int DisplayTypeWaterUW1 = 1;

	public const int DisplayTypeWaterUW2 = 4;

	public const int DisplayTypeLava = 2;

	public const int DisplayTypeDoorUW1 = 4;

	public const int DisplayTypeDoorUW2 = 1;

	public const int DisplayTypeBridge1UW1 = 9;

	public const int DisplayTypeBridge2UW1 = 10;

	public const int DisplayTypeBridgeUW2 = 6;

	public const int DisplayTypeStairUW1 = 12;

	public const int DisplayTypeStairUW2 = 3;

	public const int DisplayTypeIce = 12;

	public const int TILE_SOLID = 0;

	public const int TILE_OPEN = 1;

	public const int TILE_DIAG_SE = 2;

	public const int TILE_DIAG_SW = 3;

	public const int TILE_DIAG_NE = 4;

	public const int TILE_DIAG_NW = 5;

	public const int TILE_SLOPE_N = 6;

	public const int TILE_SLOPE_S = 7;

	public const int TILE_SLOPE_E = 8;

	public const int TILE_SLOPE_W = 9;

	public const int TILE_VALLEY_NW = 10;

	public const int TILE_VALLEY_NE = 11;

	public const int TILE_VALLEY_SE = 12;

	public const int TILE_VALLEY_SW = 13;

	public const int TILE_RIDGE_SE = 14;

	public const int TILE_RIDGE_SW = 15;

	public const int TILE_RIDGE_NW = 16;

	public const int TILE_RIDGE_NE = 17;

	private const int NORTH = 0;

	private const int SOUTH = 1;

	private const int EAST = 2;

	private const int WEST = 3;

	private const int NORTHWEST = 4;

	private const int NORTHEAST = 5;

	private const int SOUTHWEST = 6;

	private const int SOUTHEAST = 7;

	public int thisLevelNo;

	public AutoMapTile[,] Tiles = new AutoMapTile[64, 64];

	public List<MapNote> MapNotes;

	public const int TileSize = 4;

	public static Color[] OpenTileColour = new Color[3];

	public static Color[] WaterTileColour = new Color[2];

	public static Color[] LavaTileColour = new Color[2];

	public static Color[] BridgeTileColour = new Color[3];

	public static Color[] StairsTileColour = new Color[2];

	public static Color[] BorderColour = new Color[4];

	public static Color[] Background = new Color[1];

	public static Color[] IceTileColour = new Color[3];

	public static long[] AutomapNoteAddresses = new long[9];

	private int TileMapSizeX
	{
		get
		{
			return 63;
		}
	}

	private int TileMapSizeY
	{
		get
		{
			return 63;
		}
	}

	private void ProcessAutomap(char[] lev_ark, long automapAddress)
	{
		int num = 0;
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				short num2 = (short)DataLoader.getValAtAddress(lev_ark, automapAddress + num, 8);
				Tiles[j, i].tileType = (short)(num2 & 0xF);
				Tiles[j, i].DisplayType = (short)((num2 >> 4) & 0xF);
				num++;
			}
		}
	}

	private static void ProcessAutoMapNotes(int LevelNo, char[] lev_ark, long automapNotesAddress, long AUTOMAP_EOF_ADDRESS)
	{
		while (automapNotesAddress < AUTOMAP_EOF_ADDRESS)
		{
			string text = "";
			bool flag = false;
			int num = 0;
			int num2 = 0;
			num = (int)DataLoader.getValAtAddress(lev_ark, automapNotesAddress + 50, 16);
			num2 = (int)DataLoader.getValAtAddress(lev_ark, automapNotesAddress + 52, 16);
			for (int i = 0; i <= 49; i++)
			{
				if (lev_ark[automapNotesAddress + i].ToString() != "\0" && !flag)
				{
					text += lev_ark[automapNotesAddress + i];
				}
				else
				{
					flag = true;
				}
			}
			if (!(text == "") && num2 <= 200 && num <= 320)
			{
				GameWorldController.instance.AutoMaps[LevelNo].MapNotes.Add(new MapNote(num, num2, text));
				automapNotesAddress += 54;
				continue;
			}
			break;
		}
	}

	public void InitAutoMapDemo()
	{
		MapNotes = new List<MapNote>();
		thisLevelNo = 0;
	}

	public void InitAutoMapUW2(int LevelNo, char[] lev_ark)
	{
		MapNotes = new List<MapNote>();
		thisLevelNo = LevelNo;
		long datalen = 0L;
		long num = 0L;
		long num2 = 0L;
		int num3 = (int)DataLoader.getValAtAddress(lev_ark, 0L, 32);
		num = DataLoader.getValAtAddress(lev_ark, LevelNo * 4 + 6 + 640, 32);
		if (num != 0)
		{
			int num4 = (int)DataLoader.getValAtAddress(lev_ark, LevelNo * 4 + 6 + 640 + num3 * 4, 32);
			if (((num4 >> 1) & 1) == 1)
			{
				char[] lev_ark2 = DataLoader.unpackUW2(lev_ark, num, ref datalen);
				ProcessAutomap(lev_ark2, 0L);
			}
			else
			{
				ProcessAutomap(lev_ark, num);
			}
		}
		num2 = DataLoader.getValAtAddress(lev_ark, LevelNo * 4 + 6 + 960, 32);
		if (num2 != 0)
		{
			DataLoader.UWBlock uwb;
			DataLoader.LoadUWBlock(lev_ark, LevelNo + 240, 0L, out uwb);
			if (uwb.Data != null)
			{
				ProcessAutoMapNotes(LevelNo, uwb.Data, 0L, datalen);
			}
		}
	}

	public void InitAutoMapUW1(int LevelNo, char[] lev_ark)
	{
		MapNotes = new List<MapNote>();
		thisLevelNo = LevelNo;
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		bool flag = true;
		for (int i = 0; i <= AutomapNoteAddresses.GetUpperBound(0); i++)
		{
			AutomapNoteAddresses[i] = DataLoader.getValAtAddress(lev_ark, (i + 36) * 4 + 2, 32);
			if (AutomapNoteAddresses[i] != 0)
			{
				flag = false;
			}
		}
		num = DataLoader.getValAtAddress(lev_ark, (LevelNo + 27) * 4 + 2, 32);
		num2 = DataLoader.getValAtAddress(lev_ark, (LevelNo + 36) * 4 + 2, 32);
		num3 = getNextAutomapBlock(LevelNo, lev_ark);
		if (flag)
		{
			MapNotes.Add(new MapNote(0, 0, LevelNo.ToString()));
		}
		if (num != 0)
		{
			ProcessAutomap(lev_ark, num);
		}
		if (num2 != 0 && num3 <= lev_ark.GetUpperBound(0))
		{
			ProcessAutoMapNotes(LevelNo, lev_ark, num2, num3);
		}
	}

	private void WriteDebugMap()
	{
		StreamWriter streamWriter = new StreamWriter(Application.dataPath + "//..//_automap_" + thisLevelNo + ".txt", false);
		string text = "";
		for (int num = 63; num >= 0; num--)
		{
			for (int i = 0; i < 63; i++)
			{
				text = text + Tiles[i, num].DisplayType + ",";
			}
			text += "\n";
		}
		streamWriter.Write(text);
		streamWriter.Close();
	}

	public Texture2D TileMapImage()
	{
		InitColours();
		if (GameWorldController.instance.CreateReports)
		{
			WriteDebugMap();
		}
		Texture2D texture2D;
		switch (UWClass._RES)
		{
		case "UW0":
			texture2D = GameWorldController.instance.grCursors.LoadImageAt(10);
			break;
		default:
			texture2D = GameWorldController.instance.grCursors.LoadImageAt(18);
			break;
		}
		Texture2D texture2D2 = new Texture2D(256, 256, TextureFormat.ARGB32, false);
		texture2D2.filterMode = FilterMode.Point;
		texture2D2.wrapMode = TextureWrapMode.Clamp;
		for (int i = 0; i < 63; i++)
		{
			for (int num = 63; num > 0; num--)
			{
				DrawSolidTile(texture2D2, i, num, 4, 4, Background);
			}
		}
		for (int j = 0; j < 63; j++)
		{
			for (int num2 = 63; num2 > 0; num2--)
			{
				if (GetTileRender(j, num2) == 1 && GetTileVisited(j, num2))
				{
					fillTile(texture2D2, j, num2, 4, 4, OpenTileColour, WaterTileColour, LavaTileColour, BridgeTileColour, IceTileColour);
				}
			}
		}
		for (int k = 0; k < 63; k++)
		{
			for (int num3 = 63; num3 > 0; num3--)
			{
				if (GetTileVisited(k, num3))
				{
					switch (GetTileType(k, num3))
					{
					case 1:
					case 6:
					case 7:
					case 8:
					case 9:
						DrawOpenTile(texture2D2, k, num3, 4, 4, BorderColour);
						break;
					case 4:
						DrawDiagNE(texture2D2, k, num3, 4, 4, BorderColour);
						break;
					case 2:
						DrawDiagSE(texture2D2, k, num3, 4, 4, BorderColour);
						break;
					case 5:
						DrawDiagNW(texture2D2, k, num3, 4, 4, BorderColour);
						break;
					case 3:
						DrawDiagSW(texture2D2, k, num3, 4, 4, BorderColour);
						break;
					}
				}
				else
				{
					DrawSolidTile(texture2D2, k, num3, 4, 4, Background);
				}
			}
		}
		for (int l = 0; l < 63; l++)
		{
			for (int num4 = 63; num4 > 0; num4--)
			{
				if (GetIsDoor(l, num4) && GetTileVisited(l, num4))
				{
					DrawDoor(texture2D2, l, num4, 4, 4, BorderColour);
				}
			}
		}
		if (thisLevelNo == GameWorldController.instance.LevelNo)
		{
			Color[] pixels = texture2D.GetPixels();
			if (UWEBase.EditorMode)
			{
				float num5 = (float)IngameEditor.TileX * 1.2f / 76.8f;
				float num6 = (float)IngameEditor.TileY * 1.2f / 76.8f;
				texture2D2.SetPixels((int)((float)texture2D2.width * num5), (int)((float)texture2D2.width * num6), texture2D.width, texture2D.height, pixels);
			}
			else
			{
				float num7 = UWCharacter.Instance.transform.position.x / 76.8f;
				float num8 = UWCharacter.Instance.transform.position.z / 76.8f;
				texture2D2.SetPixels((int)((float)texture2D2.width * num7), (int)((float)texture2D2.width * num8), texture2D.width, texture2D.height, pixels);
			}
		}
		texture2D2.Apply();
		return texture2D2;
	}

	private void fillTile(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] GroundColour, Color[] WaterColour, Color[] LavaColour, Color[] BridgeColour, Color[] IceColour)
	{
		Color[] array;
		Color clear;
		if (GetIsBridge(TileX, TileY))
		{
			array = BridgeColour;
			clear = Color.clear;
		}
		else if (GetIsWater(TileX, TileY))
		{
			array = WaterColour;
			clear = Color.clear;
		}
		else if (GetIsLava(TileX, TileY))
		{
			array = LavaColour;
			clear = Color.clear;
		}
		else if (GetIsStair(TileX, TileY))
		{
			array = StairsTileColour;
			clear = Color.clear;
		}
		else if (GetIsIce(TileX, TileY))
		{
			array = IceColour;
			clear = Color.clear;
		}
		else
		{
			array = GroundColour;
			clear = Color.clear;
		}
		switch (GetTileType(TileX, TileY))
		{
		case 0:
			break;
		case 4:
		{
			for (int num = 0; num <= TileWidth; num++)
			{
				for (int num2 = 0; num2 <= TileHeight; num2++)
				{
					if (num >= TileHeight - num2)
					{
						OutputTile.SetPixel(num + TileX * TileWidth, num2 + TileY * TileHeight, PickColour(array));
					}
					else
					{
						OutputTile.SetPixel(num + TileX * TileWidth, num2 + TileY * TileHeight, clear);
					}
				}
			}
			break;
		}
		case 5:
		{
			for (int k = 0; k <= TileWidth; k++)
			{
				for (int l = 0; l <= TileHeight; l++)
				{
					if (k <= l)
					{
						OutputTile.SetPixel(k + TileX * TileWidth, l + TileY * TileHeight, PickColour(array));
					}
					else
					{
						OutputTile.SetPixel(k + TileX * TileWidth, l + TileY * TileHeight, clear);
					}
				}
			}
			break;
		}
		case 2:
		{
			for (int m = 0; m <= TileWidth; m++)
			{
				for (int n = 0; n <= TileHeight; n++)
				{
					if (m >= n)
					{
						OutputTile.SetPixel(m + TileX * TileWidth, n + TileY * TileHeight, PickColour(array));
					}
					else
					{
						OutputTile.SetPixel(m + TileX * TileWidth, n + TileY * TileHeight, clear);
					}
				}
			}
			break;
		}
		case 3:
		{
			for (int i = 0; i <= TileWidth; i++)
			{
				for (int j = 0; j <= TileHeight; j++)
				{
					if (TileWidth - i >= j)
					{
						OutputTile.SetPixel(i + TileX * TileWidth, j + TileY * TileHeight, PickColour(array));
					}
					else
					{
						OutputTile.SetPixel(i + TileX * TileWidth, j + TileY * TileHeight, clear);
					}
				}
			}
			break;
		}
		case 1:
		case 6:
		case 7:
		case 8:
		case 9:
			DrawSolidTile(OutputTile, TileX, TileY, TileWidth, TileHeight, array);
			break;
		default:
			DrawSolidTile(OutputTile, TileX, TileY, TileWidth, TileHeight, Background);
			break;
		}
	}

	private void DrawSolidTile(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour)
	{
		for (int i = 0; i < TileWidth; i++)
		{
			for (int j = 0; j < TileHeight; j++)
			{
				OutputTile.SetPixel(i + TileX * TileWidth, j + TileY * TileHeight, PickColour(InputColour));
			}
		}
	}

	private void DrawOpenTile(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour)
	{
		if (TileY < 63 && GetTileType(TileX, TileY + 1) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 0);
		}
		if (TileY > 0 && GetTileType(TileX, TileY - 1) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 1);
		}
		if (TileX < 63 && GetTileType(TileX + 1, TileY) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 2);
		}
		if (TileX > 0 && GetTileType(TileX - 1, TileY) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 3);
		}
	}

	private void DrawDoor(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour)
	{
		bool flag = isTileOpen(GetTileType(TileX, TileY + 1));
		bool flag2 = isTileOpen(GetTileType(TileX, TileY - 1));
		bool flag3 = isTileOpen(GetTileType(TileX + 1, TileY));
		bool flag4 = isTileOpen(GetTileType(TileX - 1, TileY));
		if (!isTileOpen(GetTileType(TileX, TileY)))
		{
			return;
		}
		if (flag3 || flag4)
		{
			for (int i = 0; i < TileHeight; i++)
			{
				OutputTile.SetPixel(TileWidth / 2 + TileX * TileWidth, i + TileY * TileHeight, PickColour(InputColour));
			}
		}
		else if (flag || flag2)
		{
			for (int j = 0; j < TileWidth; j++)
			{
				OutputTile.SetPixel(j + TileX * TileWidth, TileHeight / 2 + TileY * TileHeight, PickColour(InputColour));
			}
		}
	}

	private void DrawLine(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour, int Direction)
	{
		switch (Direction)
		{
		case 0:
		{
			for (int num2 = 0; num2 < TileWidth; num2++)
			{
				OutputTile.SetPixel(num2 + TileX * TileWidth, TileHeight + TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		case 1:
		{
			for (int l = 0; l < TileWidth; l++)
			{
				OutputTile.SetPixel(l + TileX * TileWidth, TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		case 2:
		{
			for (int n = 0; n < TileHeight; n++)
			{
				OutputTile.SetPixel(TileWidth + TileX * TileWidth, n + TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		case 3:
		{
			for (int j = 0; j < TileHeight; j++)
			{
				OutputTile.SetPixel(TileX * TileWidth, j + TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		case 5:
		{
			for (int num = 0; num <= TileHeight; num++)
			{
				OutputTile.SetPixel(TileWidth - num + TileX * TileWidth, num + TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		case 6:
		{
			for (int m = 0; m <= TileWidth; m++)
			{
				OutputTile.SetPixel(m + TileX * TileWidth, TileHeight - m + TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		case 4:
		{
			for (int k = 0; k <= TileWidth; k++)
			{
				OutputTile.SetPixel(k + TileX * TileWidth, k + TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		case 7:
		{
			for (int i = 0; i <= TileWidth; i++)
			{
				OutputTile.SetPixel(i + TileX * TileWidth, i + TileY * TileHeight, PickColour(InputColour));
			}
			break;
		}
		}
	}

	private void DrawDiagSW(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour)
	{
		DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 6);
		if (TileY < 63)
		{
			int tileType = GetTileType(TileX, TileY + 1);
			if (isTileOpen(tileType) || tileType == 3)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 0);
			}
		}
		if (TileX < 63)
		{
			int tileType2 = GetTileType(TileX + 1, TileY);
			if (isTileOpen(tileType2) || tileType2 == 3)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 2);
			}
		}
		if (TileY > 0 && GetTileType(TileX, TileY - 1) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 1);
		}
		if (TileX > 0 && GetTileType(TileX - 1, TileY) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 3);
		}
	}

	private void DrawDiagNE(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour)
	{
		DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 5);
		if (TileY > 0)
		{
			int tileType = GetTileType(TileX, TileY - 1);
			if (isTileOpen(tileType) || tileType == 4)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 1);
			}
		}
		if (TileX > 0)
		{
			int tileType2 = GetTileType(TileX - 1, TileY);
			if (isTileOpen(tileType2) || tileType2 == 4)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 3);
			}
		}
		if (TileY < 63 && GetTileType(TileX, TileY + 1) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 0);
		}
		if (TileX < 63 && GetTileType(TileX + 1, TileY) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 2);
		}
	}

	private void DrawDiagNW(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour)
	{
		DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 4);
		if (TileY > 0)
		{
			int tileType = GetTileType(TileX, TileY - 1);
			if (isTileOpen(tileType) || tileType == 5)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 1);
			}
		}
		if (TileX < 63)
		{
			int tileType2 = GetTileType(TileX + 1, TileY);
			if (isTileOpen(tileType2) || tileType2 == 5)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 2);
			}
		}
		if (TileY < 63 && GetTileType(TileX, TileY + 1) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 0);
		}
		if (TileX > 0 && GetTileType(TileX - 1, TileY) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 3);
		}
	}

	private void DrawDiagSE(Texture2D OutputTile, int TileX, int TileY, int TileWidth, int TileHeight, Color[] InputColour)
	{
		DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 7);
		if (TileY < 63)
		{
			int tileType = GetTileType(TileX, TileY + 1);
			if (isTileOpen(tileType) || tileType == 2)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 0);
			}
		}
		if (TileX > 0)
		{
			int tileType2 = GetTileType(TileX - 1, TileY);
			if (isTileOpen(tileType2) || tileType2 == 2)
			{
				DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 3);
			}
		}
		if (TileY > 0 && GetTileType(TileX, TileY - 1) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 1);
		}
		if (TileX < 63 && GetTileType(TileX + 1, TileY) == 0)
		{
			DrawLine(OutputTile, TileX, TileY, TileWidth, TileHeight, InputColour, 2);
		}
	}

	public Color PickColour(Color[] Selection)
	{
		return Selection[Random.Range(0, Selection.GetUpperBound(0) + 1)];
	}

	private void InitColours()
	{
		OpenTileColour[0].r = 0.45490196f;
		OpenTileColour[0].g = 27f / 85f;
		OpenTileColour[0].b = 0.21960784f;
		OpenTileColour[0].a = 1f;
		OpenTileColour[1].r = 0.4f;
		OpenTileColour[1].g = 14f / 51f;
		OpenTileColour[1].b = 0.18431373f;
		OpenTileColour[1].a = 1f;
		OpenTileColour[2].r = 0.41960785f;
		OpenTileColour[2].g = 0.29411766f;
		OpenTileColour[2].b = 0.18431373f;
		OpenTileColour[2].a = 1f;
		WaterTileColour[0].r = 0.24313726f;
		WaterTileColour[0].g = 0.23921569f;
		WaterTileColour[0].b = 0.5254902f;
		WaterTileColour[0].a = 1f;
		WaterTileColour[1].r = 10f / 51f;
		WaterTileColour[1].g = 0.2f;
		WaterTileColour[1].b = 23f / 51f;
		WaterTileColour[1].a = 1f;
		LavaTileColour[0].r = 23f / 51f;
		LavaTileColour[0].g = 0.09019608f;
		LavaTileColour[0].b = 9f / 85f;
		LavaTileColour[0].a = 1f;
		LavaTileColour[1].r = 26f / 85f;
		LavaTileColour[1].g = 1f / 17f;
		LavaTileColour[1].b = 0.05490196f;
		LavaTileColour[1].a = 1f;
		IceTileColour[0].r = 16f / 85f;
		IceTileColour[0].g = 48f / 85f;
		IceTileColour[0].b = 0.7137255f;
		IceTileColour[0].a = 1f;
		IceTileColour[1].r = 5f / 51f;
		IceTileColour[1].g = 0.7137255f;
		IceTileColour[1].b = 84f / 85f;
		IceTileColour[1].a = 1f;
		IceTileColour[2].r = 8f / 85f;
		IceTileColour[2].g = 0.45490196f;
		IceTileColour[2].b = 0.654902f;
		IceTileColour[2].a = 1f;
		BridgeTileColour[0].r = 0.2509804f;
		BridgeTileColour[0].g = 0.10980392f;
		BridgeTileColour[0].b = 0f;
		BridgeTileColour[0].a = 1f;
		BridgeTileColour[1].r = 0.23137255f;
		BridgeTileColour[1].g = 0.09019608f;
		BridgeTileColour[1].b = 0f;
		BridgeTileColour[1].a = 1f;
		BridgeTileColour[2].r = 0.2901961f;
		BridgeTileColour[2].g = 0.10980392f;
		BridgeTileColour[2].b = 0f;
		BridgeTileColour[2].a = 1f;
		StairsTileColour[0].r = 0.30980393f;
		StairsTileColour[0].g = 0.20392157f;
		StairsTileColour[0].b = 9f / 85f;
		StairsTileColour[0].a = 1f;
		StairsTileColour[1].r = 14f / 51f;
		StairsTileColour[1].g = 0.16078432f;
		StairsTileColour[1].b = 8f / 85f;
		StairsTileColour[1].a = 1f;
		BorderColour[0].r = 22f / 85f;
		BorderColour[0].g = 0.16078432f;
		BorderColour[0].b = 0.08627451f;
		BorderColour[0].a = 1f;
		BorderColour[1].r = 31f / 85f;
		BorderColour[1].g = 0.23529412f;
		BorderColour[1].b = 0.14509805f;
		BorderColour[1].a = 1f;
		BorderColour[2].r = 0.38431373f;
		BorderColour[2].g = 13f / 51f;
		BorderColour[2].b = 14f / 85f;
		BorderColour[2].a = 1f;
		BorderColour[3].r = 0.34509805f;
		BorderColour[3].g = 0.21960784f;
		BorderColour[3].b = 11f / 85f;
		BorderColour[3].a = 1f;
	}

	private int GetTileRender(int tileX, int tileY)
	{
		return 1;
	}

	private bool GetTileVisited(int tileX, int tileY)
	{
		if (UWEBase.EditorMode)
		{
			return true;
		}
		if (Tiles[tileX, tileY].tileType == 0)
		{
			return false;
		}
		return true;
	}

	public int GetTileType(int tileX, int tileY)
	{
		if (tileX > 63 || tileY > 63 || tileX < 0 || tileY < 0)
		{
			return 0;
		}
		if (Tiles[tileX, tileY].tileType == 10 && !UWEBase.EditorMode)
		{
			return 0;
		}
		return Tiles[tileX, tileY].tileType;
	}

	private bool GetIsDoor(int tileX, int tileY)
	{
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			return Tiles[tileX, tileY].DisplayType == 1;
		}
		return Tiles[tileX, tileY].DisplayType == 4;
	}

	private bool GetIsWater(int tileX, int tileY)
	{
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			return Tiles[tileX, tileY].DisplayType == 4;
		}
		return Tiles[tileX, tileY].DisplayType == 1;
	}

	private bool GetIsLava(int tileX, int tileY)
	{
		return Tiles[tileX, tileY].DisplayType == 2;
	}

	private bool GetIsIce(int tileX, int tileY)
	{
		return Tiles[tileX, tileY].DisplayType == 12;
	}

	private bool GetIsBridge(int tileX, int tileY)
	{
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			return Tiles[tileX, tileY].DisplayType == 6;
		}
		return Tiles[tileX, tileY].DisplayType == 9 || Tiles[tileX, tileY].DisplayType == 10;
	}

	private bool GetIsStair(int tileX, int tileY)
	{
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			return Tiles[tileX, tileY].DisplayType == 3;
		}
		return Tiles[tileX, tileY].DisplayType == 12;
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

	public void MarkTile(int tileX, int tileY, int Mark, int DisplayType)
	{
		if (tileX < 0 || tileX > 63 || tileY < 0 || tileY > 63)
		{
			return;
		}
		Tiles[tileX, tileY].tileType = (short)Mark;
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			if (Tiles[tileX, tileY].DisplayType != 3)
			{
				Tiles[tileX, tileY].DisplayType = (short)DisplayType;
			}
		}
		else if (Tiles[tileX, tileY].DisplayType != 12)
		{
			Tiles[tileX, tileY].DisplayType = (short)DisplayType;
		}
	}

	public void MarkTileDisplayType(int tileX, int tileY, int DisplayType)
	{
		if (tileX >= 0 && tileX <= 63 && tileY >= 0 && tileY <= 63)
		{
			Tiles[tileX, tileY].DisplayType = (short)DisplayType;
		}
	}

	public static int GetDisplayType(TileInfo t)
	{
		if (t.hasBridge)
		{
			string rES = UWClass._RES;
			if (rES != null && rES == "UW2")
			{
				return 6;
			}
			return 9;
		}
		if (t.isWater)
		{
			string rES2 = UWClass._RES;
			if (rES2 != null && rES2 == "UW2")
			{
				return 4;
			}
			return 1;
		}
		if (t.isLava)
		{
			return 2;
		}
		if (t.isIce)
		{
			return 12;
		}
		if (t.isDoor)
		{
			string rES3 = UWClass._RES;
			if (rES3 != null && rES3 == "UW2")
			{
				return 1;
			}
			return 4;
		}
		return 0;
	}

	public static long getNextAutomapBlock(int thisLevelNo, char[] lev_ark)
	{
		long num = AutomapNoteAddresses[thisLevelNo];
		long num2 = lev_ark.GetUpperBound(0);
		for (int i = 0; i <= AutomapNoteAddresses.GetUpperBound(0); i++)
		{
			if (AutomapNoteAddresses[i] > num && num2 > AutomapNoteAddresses[i])
			{
				num2 = AutomapNoteAddresses[i];
			}
		}
		return num2;
	}

	public char[] AutoMapVisitedToBytes()
	{
		char[] array = new char[(TileMapSizeX + 1) * (TileMapSizeY + 1)];
		int num = 0;
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				int num2 = (Tiles[j, i].DisplayType << 4) | Tiles[j, i].tileType;
				array[num] = (char)num2;
				num++;
			}
		}
		return array;
	}

	public char[] AutoMapNotesToBytes()
	{
		if (MapNotes.Count > 0)
		{
			int num = 0;
			char[] array = new char[MapNotes.Count * 54];
			foreach (MapNote mapNote in MapNotes)
			{
				bool flag = false;
				for (int i = 0; i < 54; i++)
				{
					if (i <= 49)
					{
						if (i < mapNote.NoteText.Length)
						{
							char c = mapNote.NoteText.ToUpper().ToCharArray()[i];
							array[num + i] = c;
						}
						else if (flag)
						{
							array[num + i] = 'o';
						}
						else
						{
							array[num + i] = '\0';
							flag = true;
						}
						continue;
					}
					switch (i)
					{
					case 50:
					{
						int posX = mapNote.PosX;
						array[num + i] = (char)((uint)posX & 0xFFu);
						array[num + i + 1] = (char)((uint)(posX >> 8) & 0xFFu);
						break;
					}
					case 52:
					{
						int posY = mapNote.PosY;
						array[num + i] = (char)((uint)posY & 0xFFu);
						array[num + i + 1] = (char)((uint)(posY >> 8) & 0xFFu);
						break;
					}
					}
				}
				num += 54;
			}
			return array;
		}
		return null;
	}
}
