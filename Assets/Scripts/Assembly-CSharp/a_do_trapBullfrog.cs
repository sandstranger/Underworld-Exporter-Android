using UnityEngine;

public class a_do_trapBullfrog : a_hack_trap
{
	public static int targetX;

	public static int targetY;

	public static int BaseX = 48;

	public static int BaseY = 48;

	public TileMap tm;

	protected override void Start()
	{
		tm = UWEBase.CurrentTileMap();
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		switch (base.owner)
		{
		case 0:
			RaiseLowerBullfrog(1);
			break;
		case 1:
			RaiseLowerBullfrog(-1);
			break;
		case 2:
			targetX++;
			if (targetX >= 8)
			{
				targetX = 0;
			}
			break;
		case 3:
			targetY++;
			if (targetY >= 8)
			{
				targetY = 0;
			}
			break;
		case 4:
			ResetBullFrog();
			break;
		}
	}

	public void ResetBullFrog()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 193));
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				GameObject obj = GameWorldController.FindTile(BaseX + i, BaseY + j, 1);
				UWEBase.CurrentTileMap().Tiles[BaseX + i, BaseY + j].floorHeight = 8;
				UWEBase.CurrentTileMap().Tiles[BaseX + targetX + i, BaseY + targetY + j].TileNeedsUpdate();
				Object.Destroy(obj);
			}
		}
	}

	public void RaiseLowerBullfrog(int dir)
	{
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0 && j == 0)
				{
					if ((tm.Tiles[BaseX + targetX + i, BaseY + targetY + j].floorHeight < 20 && dir == 1) || (tm.Tiles[BaseX + targetX + i, BaseY + targetY + j].floorHeight > 2 && dir == -1))
					{
						GameObject obj = GameWorldController.FindTile(BaseX + targetX + i, BaseY + targetY + j, 1);
						UWEBase.CurrentTileMap().Tiles[BaseX + targetX + i, BaseY + targetY + j].floorHeight += (short)(dir * 2);
						UWEBase.CurrentTileMap().Tiles[BaseX + targetX + i, BaseY + targetY + j].TileNeedsUpdate();
						Object.Destroy(obj);
					}
				}
				else if (targetX + i >= 0 && targetX + i < 8 && targetY + j >= 0 && targetY + j < 8 && ((tm.Tiles[BaseX + targetX + i, BaseY + targetY + j].floorHeight < 20 && dir == 1) || (tm.Tiles[BaseX + targetX + i, BaseY + targetY + j].floorHeight > 1 && dir == -1)))
				{
					GameObject obj2 = GameWorldController.FindTile(BaseX + targetX + i, BaseY + targetY + j, 1);
					UWEBase.CurrentTileMap().Tiles[BaseX + targetX + i, BaseY + targetY + j].floorHeight += (short)dir;
					UWEBase.CurrentTileMap().Tiles[BaseX + targetX + i, BaseY + targetY + j].TileNeedsUpdate();
					Object.Destroy(obj2);
				}
			}
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
