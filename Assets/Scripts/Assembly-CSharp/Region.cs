using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Region : GeneratorClasses
{
	protected struct RoomCandidate
	{
		public int index;

		public int x;

		public int y;

		public int dimX;

		public int dimY;
	}

	public int RegionIndex;

	public int layer;

	protected const int SOLID = 0;

	protected const int OPEN = 1;

	public GeneratorMap[,] Map;

	public int BaseHeight;

	public Region ParentRegion;

	private List<Region> SubRegions = new List<Region>();

	public int MapWidth { get; set; }

	public int MapHeight { get; set; }

	public int originX { get; set; }

	public int originY { get; set; }

	public Region()
	{
	}

	public Region(int index, int RegionLayer, int x, int y, int width, int height, int NoOfSubRegions, Region Parent)
	{
		InitRegion(index, RegionLayer, x, y, width, height, Parent);
		Generate(NoOfSubRegions);
		BuildSubRegions(NoOfSubRegions);
	}

	protected void InitRegion(int index, int RegionLayer, int x, int y, int width, int height, Region Parent)
	{
		Debug.Log("Region " + RegionType() + " " + index + " at " + x + "," + y + " " + width + "x" + height);
		ParentRegion = Parent;
		RegionIndex = index;
		layer = RegionLayer;
		originX = x;
		originY = y;
		MapHeight = height;
		MapWidth = width;
		SetBaseHeight();
		Map = new GeneratorMap[MapWidth + 1, MapHeight + 1];
		for (int i = 0; i <= Map.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= Map.GetUpperBound(1); j++)
			{
				Map[i, j] = new GeneratorMap();
				Map[i, j].FloorTexture = index;
				Map[i, j].FloorHeight = BaseHeight;
				Map[i, j].TileLayoutMap = 1;
			}
		}
	}

	protected virtual void Generate(int NoOfSubRegions)
	{
		for (int i = 0; i < MapWidth; i++)
		{
			for (int j = 0; j < MapHeight; j++)
			{
				Map[i, j].TileLayoutMap = 0;
			}
		}
	}

	protected void BuildSubRegions(int NoOfSubRegions)
	{
		if (NoOfSubRegions > 0)
		{
			switch (layer)
			{
			case 0:
				FillSubRegionLarge(NoOfSubRegions);
				break;
			case 1:
				FillSubRegionMedium(NoOfSubRegions);
				break;
			case 2:
				FillSubRegionSmall(NoOfSubRegions);
				break;
			}
		}
	}

	public virtual void FillSubRegionLarge(int NoOfSubRegions)
	{
		int noOfSubRegions = Random.Range(1, 26);
		switch (Random.Range(0, 4))
		{
		case 0:
			SubRegions.Add(new Region(UnderworldGenerator.RegionIndex++, layer + 1, 1, 1, 62, 62, noOfSubRegions, this));
			break;
		case 1:
			SubRegions.Add(new RoomRegion(UnderworldGenerator.RegionIndex++, layer + 1, 1, 1, 62, 62, noOfSubRegions, this));
			break;
		default:
			SubRegions.Add(new CaveRegion(UnderworldGenerator.RegionIndex++, layer + 1, 1, 1, 62, 62, noOfSubRegions, this));
			break;
		}
	}

	public virtual void FillSubRegionMedium(int NoOfSubRegions)
	{
		int num = NoOfSubRegions * 8;
		int minDimX = 10;
		int minDimY = 10;
		while (NoOfSubRegions > 0 && num > 0)
		{
			num--;
			RoomCandidate candidate = NewRoom(MapWidth - 1, MapHeight - 1, 30, 30, minDimX, minDimY);
			if (!DoesRoomCollide(candidate))
			{
				PlaceRoom(candidate, UnderworldGenerator.RegionIndex + 1);
				switch (Random.Range(0, 2))
				{
				case 0:
					SubRegions.Add(new RoomRegion(UnderworldGenerator.RegionIndex++, layer + 1, candidate.x, candidate.y, candidate.dimX, candidate.dimY, Random.Range(0, 10), this));
					break;
				default:
					SubRegions.Add(new CaveRegion(UnderworldGenerator.RegionIndex++, layer + 1, candidate.x, candidate.y, candidate.dimX, candidate.dimY, Random.Range(0, 10), this));
					break;
				}
				NoOfSubRegions--;
			}
		}
	}

	public virtual void FillSubRegionSmall(int NoOfSubRegions)
	{
		int num = NoOfSubRegions * 8;
		int minDimX = 3;
		int minDimY = 3;
		while (NoOfSubRegions > 0 && num > 0)
		{
			num--;
			RoomCandidate candidate = NewRoom(MapWidth - 1, MapHeight - 1, 15, 15, minDimX, minDimY);
			if (!DoesRoomCollide(candidate))
			{
				PlaceRoom(candidate, UnderworldGenerator.RegionIndex + 1);
				switch (Random.Range(0, 2))
				{
				case 0:
					SubRegions.Add(new RoomRegion(UnderworldGenerator.RegionIndex++, layer + 1, candidate.x, candidate.y, candidate.dimX, candidate.dimY, 0, this));
					break;
				default:
					SubRegions.Add(new CaveRegion(UnderworldGenerator.RegionIndex++, layer + 1, candidate.x, candidate.y, candidate.dimX, candidate.dimY, 0, this));
					break;
				}
				NoOfSubRegions--;
			}
		}
	}

	protected RoomCandidate NewRoom(int MaxX, int MaxY, int MaxDimX, int MaxDimY, int MinDimX, int MinDimY)
	{
		RoomCandidate result = default(RoomCandidate);
		if (MinDimX >= MaxDimX)
		{
			MinDimX = MaxDimX - 1;
		}
		if (MinDimY >= MaxDimY)
		{
			MinDimY = MaxDimY - 1;
		}
		result.x = Random.Range(1, MaxX);
		result.y = Random.Range(1, MaxY);
		result.dimX = Random.Range(MinDimX, MaxDimX);
		result.dimY = Random.Range(MinDimY, MaxDimY);
		if (result.x + result.dimX >= MaxX)
		{
			result.dimX = MaxX - result.x;
		}
		if (result.y + result.dimY >= MaxY)
		{
			result.dimY = MaxY - result.y;
		}
		return result;
	}

	protected bool DoesRoomCollide(RoomCandidate candidate)
	{
		for (int i = candidate.x; i <= candidate.x + candidate.dimX && i <= Map.GetUpperBound(0); i++)
		{
			for (int j = candidate.y; j <= candidate.y + candidate.dimY && j <= Map.GetUpperBound(1); j++)
			{
				if (Map[i, j].RoomMap != 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected void PlaceRoom(RoomCandidate candidate, int roomIndex)
	{
		for (int i = candidate.x; i <= candidate.x + candidate.dimX && i <= Map.GetUpperBound(0); i++)
		{
			for (int j = candidate.y; j <= candidate.y + candidate.dimY && j <= Map.GetUpperBound(1); j++)
			{
				Map[i, j].RoomMap = roomIndex;
			}
		}
	}

	public virtual void StyleArea()
	{
	}

	protected virtual void SetBaseHeight()
	{
		BaseHeight = Random.Range(0, 13) * 2;
	}

	public GeneratorMap[,] GetEntireMap()
	{
		GeneratorMap[,] array = new GeneratorMap[MapWidth + 1, MapHeight + 1];
		for (int i = 0; i <= Map.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= Map.GetUpperBound(1); j++)
			{
				array[i, j] = Map[i, j];
			}
		}
		for (int k = 0; k < SubRegions.Count; k++)
		{
			GeneratorMap[,] entireMap = SubRegions[k].GetEntireMap();
			int num = 0;
			for (int l = SubRegions[k].originX; l <= SubRegions[k].originX + SubRegions[k].MapWidth; l++)
			{
				int num2 = 0;
				for (int m = SubRegions[k].originY; m <= SubRegions[k].originY + SubRegions[k].MapHeight; m++)
				{
					array[l, m] = entireMap[num, num2];
					num2++;
				}
				num++;
			}
		}
		return array;
	}

	private string MapToString()
	{
		string text = "";
		int num = 0;
		for (int i = 0; i < MapHeight; i++)
		{
			for (num = 0; num < MapWidth; num++)
			{
				text = text + Map[num, i].TileLayoutMap + ",";
			}
			text += "\n";
		}
		return text;
	}

	public void PrintMap()
	{
		StreamWriter streamWriter = new StreamWriter(Application.dataPath + "//..//" + index + "_rnd.txt");
		streamWriter.Write(MapToString());
		streamWriter.Close();
	}

	public virtual string RegionType()
	{
		return "Base";
	}

	public int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
	{
		int num = x - scopeX;
		int num2 = y - scopeY;
		int num3 = x + scopeX;
		int num4 = y + scopeY;
		int num5 = num;
		int num6 = num2;
		int num7 = 0;
		for (num6 = num2; num6 <= num4; num6++)
		{
			for (num5 = num; num5 <= num3; num5++)
			{
				if ((num5 != x || num6 != y) && IsWall(num5, num6))
				{
					num7++;
				}
			}
		}
		return num7;
	}

	private bool IsWall(int x, int y)
	{
		if (IsOutOfBounds(x, y))
		{
			return true;
		}
		if (Map[x, y].TileLayoutMap == 0)
		{
			return true;
		}
		if (Map[x, y].TileLayoutMap == 1)
		{
			return false;
		}
		return false;
	}

	private bool IsOutOfBounds(int x, int y)
	{
		if (x < 0 || y < 0)
		{
			return true;
		}
		if (x > MapWidth - 1 || y > MapHeight - 1)
		{
			return true;
		}
		return false;
	}

	public void BlankMap()
	{
		int num = 0;
		for (int i = 0; i <= MapHeight; i++)
		{
			for (num = 0; num <= MapWidth; num++)
			{
				Map[num, i].TileLayoutMap = 1;
			}
		}
	}
}
