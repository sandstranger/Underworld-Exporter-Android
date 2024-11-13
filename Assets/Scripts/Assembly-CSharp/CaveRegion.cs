using System.Collections.Generic;
using UnityEngine;

public class CaveRegion : Region
{
	private int NoOfIterations = 2;

	private int[,] Flood;

	private int[,] BorderFlood;

	private int floodIndex = 1;

	private int borderfloodIndex = 1;

	private List<int> floodSizes = new List<int>();

	public int PercentAreWalls { get; set; }

	public CaveRegion(int index, int RegionLayer, int x, int y, int width, int height, int NoOfSubRegions, Region Parent)
	{
		InitRegion(index, RegionLayer, x, y, width, height, Parent);
		Generate(NoOfSubRegions);
		BuildSubRegions(NoOfSubRegions);
	}

	protected override void Generate(int NoOfSubRegions)
	{
		PercentAreWalls = 40;
		RandomFillMap();
		for (int i = 0; i < NoOfIterations; i++)
		{
			MakeCaverns();
		}
		Flood = new int[base.MapWidth + 1, base.MapHeight + 1];
		FillCaves();
		if (layer > 1 && (ParentRegion == null || !(ParentRegion.RegionType() == "Base")))
		{
			CleanUpBorder();
		}
	}

	public void MakeCaverns()
	{
		int num = 0;
		for (int i = 0; i <= base.MapHeight; i++)
		{
			for (num = 0; num <= base.MapWidth; num++)
			{
				Map[num, i].TileLayoutMap = PlaceWallLogic(num, i);
			}
		}
	}

	public int PlaceWallLogic(int x, int y)
	{
		int adjacentWalls = GetAdjacentWalls(x, y, 1, 1);
		if (Map[x, y].TileLayoutMap == 0)
		{
			if (adjacentWalls >= 4)
			{
				return 0;
			}
			if (adjacentWalls < 2)
			{
				return 1;
			}
		}
		else if (adjacentWalls >= 5)
		{
			return 0;
		}
		return 1;
	}

	public void RandomFillMap()
	{
		BlankMap();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i <= base.MapHeight; i++)
		{
			for (num2 = 0; num2 <= base.MapWidth; num2++)
			{
				Map[num2, i] = new GeneratorMap();
				Map[num2, i].TileLayoutMap = 1;
				if (num2 == 0)
				{
					Map[num2, i].TileLayoutMap = 0;
					continue;
				}
				if (i == 0)
				{
					Map[num2, i].TileLayoutMap = 0;
					continue;
				}
				if (num2 == base.MapWidth - 1)
				{
					Map[num2, i].TileLayoutMap = 0;
					continue;
				}
				if (i == base.MapHeight - 1)
				{
					Map[num2, i].TileLayoutMap = 0;
					continue;
				}
				num = base.MapHeight / 2;
				if (i == num)
				{
					Map[num2, i].TileLayoutMap = 0;
				}
				else
				{
					Map[num2, i].TileLayoutMap = RandomPercentWall(PercentAreWalls);
				}
			}
		}
	}

	private int RandomPercentWall(int percent)
	{
		if (percent >= Random.Range(1, 101))
		{
			return 0;
		}
		return 1;
	}

	public override string RegionType()
	{
		return "Cave";
	}

	private void FillCaves()
	{
		floodSizes.Add(0);
		for (int i = 0; i <= Map.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= Map.GetUpperBound(1); j++)
			{
				if (Flood[i, j] == 0 && Map[i, j].TileLayoutMap != 0)
				{
					floodSizes.Add(0);
					FloodFill(i, j, floodIndex++);
				}
			}
		}
		int num = 0;
		for (int k = 1; k < floodSizes.Count; k++)
		{
			if (floodSizes[k] > floodSizes[num])
			{
				num = k;
			}
		}
		for (int l = 0; l <= Map.GetUpperBound(0); l++)
		{
			for (int m = 0; m <= Map.GetUpperBound(1); m++)
			{
				if (Flood[l, m] != num)
				{
					Map[l, m].TileLayoutMap = 0;
					Flood[l, m] = 0;
				}
			}
		}
	}

	private void FloodFill(int x, int y, int indexToFill)
	{
		Flood[x, y] = indexToFill;
		floodSizes[indexToFill] += 1;
		if (x + 1 <= Map.GetUpperBound(0) && Map[x + 1, y].TileLayoutMap != 0 && Flood[x + 1, y] == 0)
		{
			FloodFill(x + 1, y, indexToFill);
		}
		if (x - 1 >= 0 && Map[x - 1, y].TileLayoutMap != 0 && Flood[x - 1, y] == 0)
		{
			FloodFill(x - 1, y, indexToFill);
		}
		if (y + 1 <= Map.GetUpperBound(0) && Map[x, y + 1].TileLayoutMap != 0 && Flood[x, y + 1] == 0)
		{
			FloodFill(x, y + 1, indexToFill);
		}
		if (y - 1 >= 0 && Map[x, y - 1].TileLayoutMap != 0 && Flood[x, y - 1] == 0)
		{
			FloodFill(x, y - 1, indexToFill);
		}
	}

	private void CleanUpBorderX()
	{
		for (int i = 0; i <= Map.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= Map.GetUpperBound(1); j++)
			{
				bool flag = false;
				for (int k = -1; k <= 1; k++)
				{
					for (int l = -1; l <= 1; l++)
					{
						if (k != 0 && l != 0 && i + k >= 0 && i + k <= Map.GetUpperBound(0) && j + l >= 0 && j + l <= Map.GetUpperBound(1) && Flood[i + k, j + l] != 0)
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					Map[i, j].TileLayoutMap = 1;
				}
			}
		}
	}

	private void CleanUpBorder()
	{
		BorderFlood = new int[base.MapWidth + 1, base.MapHeight + 1];
		for (int i = 0; i <= Map.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= Map.GetUpperBound(1); j++)
			{
				if ((i == 0 || j == 0 || i == Map.GetUpperBound(0) || j == Map.GetUpperBound(0)) && !isTouchingOpenArea(i, j))
				{
					floodFillBorder(i, j, borderfloodIndex++);
				}
			}
		}
		for (int k = 0; k <= Map.GetUpperBound(0); k++)
		{
			for (int l = 0; l <= Map.GetUpperBound(1); l++)
			{
				if (BorderFlood[k, l] != 0)
				{
					Map[k, l].TileLayoutMap = 1;
				}
			}
		}
	}

	private void floodFillBorder(int x, int y, int floodIndex)
	{
		if (BorderFlood[x, y] != 0)
		{
			return;
		}
		BorderFlood[x, y] = floodIndex;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i != 0 && j != 0 && x + i >= 0 && x + i <= Map.GetUpperBound(0) && y + j >= 0 && y + j <= Map.GetUpperBound(1) && !isTouchingOpenArea(x + i, y + j))
				{
					floodFillBorder(x + i, y + j, floodIndex);
				}
			}
		}
	}

	private bool isTouchingOpenArea(int x, int y)
	{
		bool result = false;
		if (Map[x, y].TileLayoutMap != 0)
		{
			return true;
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i != 0 && j != 0 && x + i >= 0 && y + j >= 0 && x + i <= Map.GetUpperBound(0) && y + j <= Map.GetUpperBound(1) && Flood[x + i, y + j] != 0)
				{
					return true;
				}
			}
		}
		return result;
	}
}
