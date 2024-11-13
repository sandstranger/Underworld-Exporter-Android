using UnityEngine;

public class a_change_terrain_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		int num = (base.quality >> 1) & 0xF;
		for (short num2 = 0; num2 <= base.xpos; num2++)
		{
			for (short num3 = 0; num3 <= base.ypos; num3++)
			{
				short num4 = (short)(num2 + triggerX);
				short num5 = (short)(num3 + triggerY);
				GameObject gameObject = GameWorldController.FindTile(num4, num5, 1);
				if (gameObject != null)
				{
					TileInfo tileInfo = UWEBase.CurrentTileMap().Tiles[num4, num5];
					if (num < 10)
					{
						tileInfo.floorTexture = (short)num;
					}
					tileInfo.Render = true;
					for (int i = 0; i < 6; i++)
					{
						tileInfo.VisibleFaces[i] = true;
					}
					short newTileType = (short)(base.quality & 1);
					short newFloorHeight = ((base.zpos != 120) ? ((short)(base.zpos >> 2)) : ((short)(src.zpos >> 2)));
					short wallTexture = tileInfo.wallTexture;
					if (UWEBase._RES == "UW2" && base.owner < 63)
					{
						wallTexture = base.owner;
					}
					tileInfo.DimX = 1;
					tileInfo.DimY = 1;
					tileInfo.TileNeedsUpdate();
					TileMapRenderer.UpdateTile(num4, num5, newTileType, newFloorHeight, tileInfo.floorTexture, wallTexture, false);
					Object.Destroy(gameObject);
					if (tileInfo.isDoor)
					{
						GameObject gameObject2 = DoorControl.findDoor(tileInfo.tileX, tileInfo.tileY);
						if (gameObject2 != null)
						{
							string text = ObjectLoader.UniqueObjectName(gameObject2.GetComponent<ObjectInteraction>().objectloaderinfo);
							DestroyDoorPortion("front_leftside_" + text);
							DestroyDoorPortion("front_over_" + text);
							DestroyDoorPortion("front_rightside_" + text);
							DestroyDoorPortion("side1_filler_" + text);
							DestroyDoorPortion("over_filler_" + text);
							DestroyDoorPortion("side2_filler_" + text);
							DestroyDoorPortion("front_filler_" + text);
							DestroyDoorPortion("rear_leftside_" + text);
							DestroyDoorPortion("rear_over_" + text);
							DestroyDoorPortion("rear_rightside_" + text);
							DestroyDoorPortion(text);
							TileMapRenderer.RenderDoor(GameWorldController.instance.SceneryModel.gameObject, UWEBase.CurrentTileMap(), UWEBase.CurrentObjectList(), gameObject2.GetComponent<ObjectInteraction>().objectloaderinfo.index);
							Vector3 position = gameObject2.transform.position;
							ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), gameObject2.GetComponent<ObjectInteraction>().objectloaderinfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.LevelModel, position);
						}
					}
				}
				else
				{
					Debug.Log(base.name + " Unable to find tile for change terrain trap " + num4 + " " + num5);
				}
			}
		}
		GameWorldController.WorldReRenderPending = true;
		for (int j = -1; j <= base.xpos + 1; j++)
		{
			for (int k = -1; k <= base.ypos + 1; k++)
			{
				int num6 = j + triggerX;
				int num7 = k + triggerY;
				GameObject gameObject3 = GameWorldController.FindTile(num6, num7, 1);
				if (gameObject3 != null)
				{
					Object.Destroy(gameObject3);
				}
				if (num6 >= 0 && num6 <= 63 && num7 >= 0 && num7 <= 63)
				{
					TileInfo tileInfo2 = UWEBase.CurrentTileMap().Tiles[num6, num7];
					tileInfo2.Render = true;
					for (int l = 0; l < 6; l++)
					{
						tileInfo2.VisibleFaces[l] = true;
					}
					tileInfo2.TileNeedsUpdate();
				}
			}
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}

	public void DestroyDoorPortion(string portionName)
	{
		GameObject gameObject = GameObject.Find(portionName);
		if (gameObject != null)
		{
			Object.Destroy(gameObject);
		}
	}
}
