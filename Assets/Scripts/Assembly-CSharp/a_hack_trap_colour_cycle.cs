using UnityEngine;

public class a_hack_trap_colour_cycle : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		TileMap tileMap = UWEBase.CurrentTileMap();
		short num = base.quality;
		short num2 = base.owner;
		for (int i = triggerX; i <= triggerX + 4; i++)
		{
			for (int j = triggerY; j <= triggerY + 4; j++)
			{
				if (TileMap.ValidTile(i, j))
				{
					tileMap.Tiles[i, j].floorTexture++;
					if (tileMap.Tiles[i, j].floorTexture > num)
					{
						tileMap.Tiles[i, j].floorTexture = num2;
					}
					tileMap.Tiles[i, j].wallTexture++;
					if (tileMap.Tiles[i, j].wallTexture > num)
					{
						tileMap.Tiles[i, j].wallTexture = num2;
					}
				}
			}
		}
		UWEBase.CurrentTileMap().SetTileMapWallFacesUW();
		for (int k = triggerX - 1; k <= triggerX + 5; k++)
		{
			for (int l = triggerY - 1; l <= triggerY + 5; l++)
			{
				if (TileMap.ValidTile(k, l))
				{
					tileMap.Tiles[k, l].TileNeedsUpdate();
					GameObject gameObject = null;
					GameObject gameObject2 = null;
					switch (tileMap.Tiles[k, l].tileType)
					{
					case 2:
					case 3:
					case 4:
					case 5:
						gameObject = GameWorldController.FindTile(k, l, 1);
						gameObject2 = GameWorldController.FindTile(k, l, 3);
						break;
					default:
						gameObject = GameWorldController.FindTile(k, l, 1);
						break;
					}
					if (gameObject != null)
					{
						Object.Destroy(gameObject);
					}
					if (gameObject2 != null)
					{
						Object.Destroy(gameObject2);
					}
				}
			}
		}
	}

	public override void PostActivate(object_base src)
	{
	}
}
