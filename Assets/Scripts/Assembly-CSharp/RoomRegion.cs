public class RoomRegion : Region
{
	public RoomRegion(int index, int RegionLayer, int x, int y, int width, int height, int NoOfSubRegions, Region Parent)
	{
		InitRegion(index, RegionLayer, x, y, width, height, Parent);
		Generate(NoOfSubRegions);
		BuildSubRegions(NoOfSubRegions);
	}

	protected override void Generate(int NoOfSubRegions)
	{
		for (int i = 0; i <= Map.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= Map.GetUpperBound(1); j++)
			{
				if (i == 0 || j == 0 || i == Map.GetUpperBound(0) || j == Map.GetUpperBound(1))
				{
					Map[i, j].TileLayoutMap = 0;
				}
				else
				{
					Map[i, j].TileLayoutMap = 1;
				}
			}
		}
		BuildSubRegions(NoOfSubRegions);
	}

	public override string RegionType()
	{
		return "Room";
	}
}
