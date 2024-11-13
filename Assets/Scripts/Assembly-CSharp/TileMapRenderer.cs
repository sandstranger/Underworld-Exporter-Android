using UnityEngine;

public class TileMapRenderer : Loader
{
	private static bool debugtextures = false;

	private const int TILE_SOLID = 0;

	private const int TILE_OPEN = 1;

	private const int TILE_DIAG_SE = 2;

	private const int TILE_DIAG_SW = 3;

	private const int TILE_DIAG_NE = 4;

	private const int TILE_DIAG_NW = 5;

	private const int TILE_SLOPE_N = 6;

	private const int TILE_SLOPE_S = 7;

	private const int TILE_SLOPE_E = 8;

	private const int TILE_SLOPE_W = 9;

	private const int TILE_VALLEY_NW = 10;

	private const int TILE_VALLEY_NE = 11;

	private const int TILE_VALLEY_SE = 12;

	private const int TILE_VALLEY_SW = 13;

	private const int TILE_RIDGE_SE = 14;

	private const int TILE_RIDGE_SW = 15;

	private const int TILE_RIDGE_NW = 16;

	private const int TILE_RIDGE_NE = 17;

	private const int SLOPE_BOTH_PARALLEL = 0;

	private const int SLOPE_BOTH_OPPOSITE = 1;

	private const int SLOPE_FLOOR_ONLY = 2;

	private const int SLOPE_CEILING_ONLY = 3;

	private const int vTOP = 0;

	private const int vEAST = 1;

	private const int vBOTTOM = 2;

	private const int vWEST = 3;

	private const int vNORTH = 4;

	private const int vSOUTH = 5;

	private const int fSELF = 128;

	private const int fCEIL = 64;

	private const int fNORTH = 32;

	private const int fSOUTH = 16;

	private const int fEAST = 8;

	private const int fWEST = 4;

	private const int fTOP = 2;

	private const int fBOTTOM = 1;

	private const int NORTH = 180;

	private const int SOUTH = 0;

	private const int EAST = 270;

	private const int WEST = 90;

	public static bool EnableCollision = true;

	private static int CEILING_HEIGHT;

	private const int CEIL_ADJ = 0;

	private const int FLOOR_ADJ = 0;

	private const float doorwidth = 0.8f;

	private const float doorframewidth = 1.2f;

	private const float doorSideWidth = 0.20000002f;

	private const float doorheight = 1.0500001f;

	public static void GenerateLevelFromTileMap(GameObject parent, GameObject sceneryParent, string game, TileMap Level, ObjectLoader objList, bool UpdateOnly)
	{
		bool skipCeil = true;
		CEILING_HEIGHT = Level.CEILING_HEIGHT;
		if (game == "SHOCK")
		{
			skipCeil = false;
		}
		if (!UpdateOnly)
		{
			foreach (Transform item in parent.transform)
			{
				Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in sceneryParent.transform)
			{
				Object.Destroy(item2.gameObject);
			}
		}
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				if ((UpdateOnly && Level.Tiles[j, i].NeedsReRender) || !UpdateOnly)
				{
					RenderTile(parent, j, i, Level.Tiles[j, i], false, false, false, skipCeil);
					if (game != "SHOCK")
					{
						RenderTile(parent, j, i, Level.Tiles[j, i], true, false, false, skipCeil);
						Level.Tiles[j, i].NeedsReRender = false;
					}
				}
				Level.Tiles[j, i].NeedsReRender = false;
			}
		}
		if (game != "SHOCK" && !UpdateOnly)
		{
			TileInfo tileInfo = new TileInfo(Level, 0, 0, 1, 0, 0, 0, 0, Level.Tiles[0, 0].shockCeilingTexture, 0, 0, 0, 0);
			tileInfo.DimX = 64;
			tileInfo.DimY = 64;
			tileInfo.ceilingHeight = 0;
			tileInfo.floorTexture = Level.Tiles[0, 0].shockCeilingTexture;
			tileInfo.shockCeilingTexture = Level.Tiles[0, 0].shockCeilingTexture;
			tileInfo.VisibleFaces[0] = false;
			tileInfo.VisibleFaces[1] = false;
			tileInfo.VisibleFaces[2] = true;
			tileInfo.VisibleFaces[3] = false;
			tileInfo.VisibleFaces[4] = false;
			tileInfo.VisibleFaces[5] = false;
			GameWorldController.instance.ceiling = RenderTile(sceneryParent, tileInfo.tileX, tileInfo.tileX, tileInfo, false, false, true, false);
			for (short num = 98; num <= 100; num++)
			{
				for (short num2 = 98; num2 <= 100; num2++)
				{
					tileInfo.tileX = num;
					tileInfo.tileY = num2;
					if (num != 99 || num2 != 99)
					{
						tileInfo.tileType = 0;
					}
					else
					{
						tileInfo.tileType = 1;
					}
					RenderTile(sceneryParent, num, num2, tileInfo, false, false, false, false);
				}
			}
		}
		if (!UpdateOnly)
		{
			string rES = UWClass._RES;
			if (rES == null || !(rES == "SHOCK"))
			{
				RenderPillars(sceneryParent, Level, objList);
				RenderDoorways(sceneryParent, Level, objList);
			}
		}
		if (UWEBase.EditorMode && UpdateOnly)
		{
			UWHUD.instance.editor.RefreshTileMap();
		}
	}

	public static void RenderDoorways(GameObject Parent, TileMap level, ObjectLoader objList)
	{
		if (objList == null)
		{
			return;
		}
		for (int i = 0; i <= objList.objInfo.GetUpperBound(0); i++)
		{
			if (objList.objInfo[i] != null && objList.objInfo[i].item_id >= 320 && objList.objInfo[i].item_id <= 335 && objList.objInfo[i].InUseFlag == 1)
			{
				RenderDoor(Parent, level, objList, i);
			}
		}
	}

	public static void RenderDoor(GameObject Parent, TileMap level, ObjectLoader objList, int i)
	{
		if (level.Tiles[objList.objInfo[i].ObjectTileX, objList.objInfo[i].ObjectTileY].tileType != 0)
		{
			float floorHeight = (float)level.Tiles[objList.objInfo[i].ObjectTileX, objList.objInfo[i].ObjectTileY].floorHeight * 0.15f;
			int num = ObjectLoader.findObjectByTypeInTile(objList.objInfo, objList.objInfo[i].ObjectTileX, objList.objInfo[i].ObjectTileY, 7);
			if (num != -1)
			{
				floorHeight = ObjectLoader.CalcObjectXYZ(num, 0).y;
			}
			RenderDoorwayFront(Parent, level, objList, objList.objInfo[i], floorHeight);
			RenderDoorwayRear(Parent, level, objList, objList.objInfo[i], floorHeight);
		}
	}

	public static void RenderDoorwayRear(GameObject Parent, TileMap level, ObjectLoader objList, ObjectLoaderInfo currDoor, float floorHeight)
	{
		Material[] array = new Material[1];
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			array[i] = GameWorldController.instance.MaterialMasterList[UWClass.CurrentTileMap().texture_map[UWClass.CurrentTileMap().Tiles[currDoor.ObjectTileX, currDoor.ObjectTileY].wallTexture]];
		}
		float num = 0f;
		float num2 = num + 1f / 6f;
		float x = num2 + 0.6666666f;
		float x2 = 1f;
		Vector3 position = ObjectLoader.CalcObjectXYZ(currDoor.index, 0);
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			position = new Vector3(position.x - 0.02f, 0f, (float)currDoor.ObjectTileY * 1.2f + 0.6f);
			break;
		case 0:
		case 180:
			position = new Vector3((float)currDoor.ObjectTileX * 1.2f + 0.6f, 0f, position.z - 0.02f);
			break;
		}
		float num3 = 0f;
		float num4 = -0.6f;
		float num5 = 0.6f;
		float z = 0f;
		float z2 = (float)CEILING_HEIGHT * 0.15f;
		Vector3[] array2 = new Vector3[4];
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			array2[0] = new Vector3(num3, num4, z);
			array2[1] = new Vector3(num3, num4, z2);
			array2[2] = new Vector3(num3, num4 + 0.20000002f, z2);
			array2[3] = new Vector3(num3, num4 + 0.20000002f, z);
			break;
		default:
			array2[0] = new Vector3(num4, num3, z);
			array2[1] = new Vector3(num4, num3, z2);
			array2[2] = new Vector3(num4 + 0.20000002f, num3, z2);
			array2[3] = new Vector3(num4 + 0.20000002f, num3, z);
			break;
		}
		Vector2[] array3 = new Vector2[4]
		{
			new Vector2(num, 0f),
			new Vector2(num, 4f),
			new Vector2(num2, 4f),
			new Vector2(num2, 0f)
		};
		GameObject gameObject = RenderCuboid(Parent, array2, array3, position, array, 1, "rear_leftside_" + ObjectLoader.UniqueObjectName(currDoor));
		gameObject.transform.Rotate(new Vector3(0f, 0f, -180f));
		num4 = -0.4f;
		num5 = 0.4f;
		z = floorHeight + 1.0500001f;
		z2 = (float)CEILING_HEIGHT * 0.15f;
		Vector3[] array4 = new Vector3[4];
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			array4[0] = new Vector3(num3, num4, z);
			array4[1] = new Vector3(num3, num4, z2);
			array4[2] = new Vector3(num3, num5, z2);
			array4[3] = new Vector3(num3, num5, z);
			break;
		default:
			array4[0] = new Vector3(num4, num3, z);
			array4[1] = new Vector3(num4, num3, z2);
			array4[2] = new Vector3(num5, num3, z2);
			array4[3] = new Vector3(num5, num3, z);
			break;
		}
		float num6 = z / 0.15f;
		num6 /= 8f;
		array3[0] = new Vector2(num2, num6);
		array3[1] = new Vector2(num2, (float)CEILING_HEIGHT / 8f);
		array3[2] = new Vector2(x, (float)CEILING_HEIGHT / 8f);
		array3[3] = new Vector2(x, num6);
		gameObject = RenderCuboid(Parent, array4, array3, position, array, 1, "rear_over_" + ObjectLoader.UniqueObjectName(currDoor));
		gameObject.transform.Rotate(new Vector3(0f, 0f, -180f));
		if (UWClass._RES == "UW1" && currDoor.ObjectTileX == 12 && currDoor.ObjectTileY == 51 && level.thisLevelNo == 3)
		{
			gameObject.layer = LayerMask.NameToLayer("UWObjects");
		}
		num4 = -0.6f;
		num5 = 0.6f;
		z = 0f;
		z2 = (float)CEILING_HEIGHT * 0.15f;
		Vector3[] array5 = new Vector3[4];
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			array5[0] = new Vector3(num3, num4 + 0.20000002f + 0.8f, z);
			array5[1] = new Vector3(num3, num4 + 0.20000002f + 0.8f, z2);
			array5[2] = new Vector3(num3, num4 + 0.20000002f + 0.8f + 0.20000002f, z2);
			array5[3] = new Vector3(num3, num4 + 0.20000002f + 0.8f + 0.20000002f, z);
			break;
		default:
			array5[0] = new Vector3(num4 + 0.20000002f + 0.8f, num3, z);
			array5[1] = new Vector3(num4 + 0.20000002f + 0.8f, num3, z2);
			array5[2] = new Vector3(num4 + 0.20000002f + 0.8f + 0.20000002f, num3, z2);
			array5[3] = new Vector3(num4 + 0.20000002f + 0.8f + 0.20000002f, num3, z);
			break;
		}
		gameObject = RenderCuboid(Parent, array5, new Vector2[4]
		{
			new Vector2(x, 0f),
			new Vector2(x, 4f),
			new Vector2(x2, 4f),
			new Vector2(x2, 0f)
		}, position, array, 1, "rear_rightside_" + ObjectLoader.UniqueObjectName(currDoor));
		gameObject.transform.Rotate(new Vector3(0f, 0f, -180f));
	}

	public static void RenderDoorwayFront(GameObject Parent, TileMap level, ObjectLoader objList, ObjectLoaderInfo currDoor, float floorHeight)
	{
		Material[] array = new Material[1];
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			array[i] = GameWorldController.instance.MaterialMasterList[UWClass.CurrentTileMap().texture_map[UWClass.CurrentTileMap().Tiles[currDoor.ObjectTileX, currDoor.ObjectTileY].wallTexture]];
		}
		float num = 0f;
		float num2 = num + 1f / 6f;
		float x = num2 + 0.6666666f;
		float x2 = 1f;
		Vector3 position = ObjectLoader.CalcObjectXYZ(currDoor.index, 0);
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			position = new Vector3(position.x + 0.02f, 0f, (float)currDoor.ObjectTileY * 1.2f + 0.6f);
			break;
		case 0:
		case 180:
			position = new Vector3((float)currDoor.ObjectTileX * 1.2f + 0.6f, 0f, position.z + 0.02f);
			break;
		}
		float num3 = 0f;
		float num4 = -0.6f;
		float num5 = 0.6f;
		float z = 0f;
		float z2 = (float)CEILING_HEIGHT * 0.15f;
		Vector3[] array2 = new Vector3[4];
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			array2[0] = new Vector3(num3, num4, z);
			array2[1] = new Vector3(num3, num4, z2);
			array2[2] = new Vector3(num3, num4 + 0.20000002f, z2);
			array2[3] = new Vector3(num3, num4 + 0.20000002f, z);
			break;
		default:
			array2[0] = new Vector3(num4, num3, z);
			array2[1] = new Vector3(num4, num3, z2);
			array2[2] = new Vector3(num4 + 0.20000002f, num3, z2);
			array2[3] = new Vector3(num4 + 0.20000002f, num3, z);
			break;
		}
		Vector2[] array3 = new Vector2[4]
		{
			new Vector2(num, 0f),
			new Vector2(num, 4f),
			new Vector2(num2, 4f),
			new Vector2(num2, 0f)
		};
		RenderCuboid(Parent, array2, array3, position, array, 1, "front_leftside_" + ObjectLoader.UniqueObjectName(currDoor));
		num4 = -0.4f;
		num5 = 0.4f;
		z = floorHeight + 1.0500001f;
		z2 = (float)CEILING_HEIGHT * 0.15f;
		Vector3[] array4 = new Vector3[4];
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			array4[0] = new Vector3(num3, num4, z);
			array4[1] = new Vector3(num3, num4, z2);
			array4[2] = new Vector3(num3, num5, z2);
			array4[3] = new Vector3(num3, num5, z);
			break;
		default:
			array4[0] = new Vector3(num4, num3, z);
			array4[1] = new Vector3(num4, num3, z2);
			array4[2] = new Vector3(num5, num3, z2);
			array4[3] = new Vector3(num5, num3, z);
			break;
		}
		float num6 = z / 0.15f;
		num6 /= 8f;
		array3[0] = new Vector2(num2, num6);
		array3[1] = new Vector2(num2, (float)CEILING_HEIGHT / 8f);
		array3[2] = new Vector2(x, (float)CEILING_HEIGHT / 8f);
		array3[3] = new Vector2(x, num6);
		GameObject gameObject = RenderCuboid(Parent, array4, array3, position, array, 1, "front_over_" + ObjectLoader.UniqueObjectName(currDoor));
		if (UWClass._RES == "UW1" && currDoor.ObjectTileX == 12 && currDoor.ObjectTileY == 51 && level.thisLevelNo == 3)
		{
			gameObject.layer = LayerMask.NameToLayer("UWObjects");
		}
		num4 = -0.6f;
		num5 = 0.6f;
		z = 0f;
		z2 = (float)CEILING_HEIGHT * 0.15f;
		Vector3[] array5 = new Vector3[4];
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
			array5[0] = new Vector3(num3, num4 + 0.20000002f + 0.8f, z);
			array5[1] = new Vector3(num3, num4 + 0.20000002f + 0.8f, z2);
			array5[2] = new Vector3(num3, num4 + 0.20000002f + 0.8f + 0.20000002f, z2);
			array5[3] = new Vector3(num3, num4 + 0.20000002f + 0.8f + 0.20000002f, z);
			break;
		default:
			array5[0] = new Vector3(num4 + 0.20000002f + 0.8f, num3, z);
			array5[1] = new Vector3(num4 + 0.20000002f + 0.8f, num3, z2);
			array5[2] = new Vector3(num4 + 0.20000002f + 0.8f + 0.20000002f, num3, z2);
			array5[3] = new Vector3(num4 + 0.20000002f + 0.8f + 0.20000002f, num3, z);
			break;
		}
		RenderCuboid(Parent, array5, new Vector2[4]
		{
			new Vector2(x, 0f),
			new Vector2(x, 4f),
			new Vector2(x2, 4f),
			new Vector2(x2, 0f)
		}, position, array, 1, "front_rightside_" + ObjectLoader.UniqueObjectName(currDoor));
		Vector3[] array6 = new Vector3[12];
		int num7 = 0;
		position = new Vector3(position.x, floorHeight, position.z);
		array3 = new Vector2[12]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		switch (currDoor.heading * 45)
		{
		case 90:
		case 270:
		{
			num7 = 0;
			array6[num7++] = new Vector3(0.04f, -0.4f, 1.0500001f);
			array6[num7++] = new Vector3(0.04f, -0.4f, 0f);
			array6[num7++] = new Vector3(-0f, -0.4f, 0f);
			array6[num7++] = new Vector3(-0f, -0.4f, 1.0500001f);
			RenderCuboid(Parent, array6, array3, position, array, 1, "side1_filler_" + ObjectLoader.UniqueObjectName(currDoor));
			num7 = 0;
			array6[num7++] = new Vector3(0f, 0.4f, 1.0500001f);
			array6[num7++] = new Vector3(0.04f, 0.4f, 1.0500001f);
			array6[num7++] = new Vector3(0.04f, -0.4f, 1.0500001f);
			array6[num7++] = new Vector3(0f, -0.4f, 1.0500001f);
			GameObject gameObject3 = RenderCuboid(Parent, array6, array3, position, array, 1, "over_filler_" + ObjectLoader.UniqueObjectName(currDoor));
			if (UWClass._RES == "UW1" && currDoor.ObjectTileX == 12 && currDoor.ObjectTileY == 51 && level.thisLevelNo == 3)
			{
				gameObject3.layer = LayerMask.NameToLayer("UWObjects");
			}
			num7 = 0;
			array6[num7++] = new Vector3(0.04f, 0.4f, 0f);
			array6[num7++] = new Vector3(0.04f, 0.4f, 1.0500001f);
			array6[num7++] = new Vector3(0f, 0.4f, 1.0500001f);
			array6[num7++] = new Vector3(0f, 0.4f, 0f);
			RenderCuboid(Parent, array6, array3, position, array, 1, "side2_filler_" + ObjectLoader.UniqueObjectName(currDoor));
			break;
		}
		default:
		{
			num7 = 0;
			array6[num7++] = new Vector3(-0.4f, 0f, 0f);
			array6[num7++] = new Vector3(-0.4f, 0f, 1.0500001f);
			array6[num7++] = new Vector3(-0.4f, -0.04f, 1.0500001f);
			array6[num7++] = new Vector3(-0.4f, -0.04f, 0f);
			RenderCuboid(Parent, array6, array3, position, array, 1, "side1_filler_" + ObjectLoader.UniqueObjectName(currDoor));
			num7 = 0;
			array6[num7++] = new Vector3(0.4f, -0f, 1.0500001f);
			array6[num7++] = new Vector3(0.4f, -0.04f, 1.0500001f);
			array6[num7++] = new Vector3(-0.4f, -0.04f, 1.0500001f);
			array6[num7++] = new Vector3(-0.4f, 0f, 1.0500001f);
			GameObject gameObject2 = RenderCuboid(Parent, array6, array3, position, array, 1, "over_filler_" + ObjectLoader.UniqueObjectName(currDoor));
			if (UWClass._RES == "UW1" && currDoor.ObjectTileX == 12 && currDoor.ObjectTileY == 51 && level.thisLevelNo == 3)
			{
				gameObject2.layer = LayerMask.NameToLayer("UWObjects");
			}
			num7 = 0;
			array6[num7++] = new Vector3(0.4f, 0f, 1.0500001f);
			array6[num7++] = new Vector3(0.4f, 0f, 0f);
			array6[num7++] = new Vector3(0.4f, -0.04f, 0f);
			array6[num7++] = new Vector3(0.4f, -0.04f, 1.0500001f);
			RenderCuboid(Parent, array6, array3, position, array, 1, "side2_filler_" + ObjectLoader.UniqueObjectName(currDoor));
			break;
		}
		}
		RenderCuboid(Parent, array6, array3, position, array, 3, "front_filler_" + ObjectLoader.UniqueObjectName(currDoor));
	}

	public static void RenderPillars(GameObject Parent, TileMap level, ObjectLoader objList)
	{
		if (objList == null)
		{
			return;
		}
		for (int i = 0; i <= objList.objInfo.GetUpperBound(0); i++)
		{
			if (objList.objInfo[i] != null && objList.objInfo[i].GetItemType() == 31 && objList.objInfo[i].InUseFlag == 1)
			{
				Vector3 position = ObjectLoader.CalcObjectXYZ(i, 0);
				Vector3[] array = new Vector3[24];
				Vector2[] array2 = new Vector2[24];
				int num = 0;
				float x = 0.03f;
				float x2 = -0.03f;
				float y = -0.03f;
				float y2 = 0.03f;
				float z = (float)CEILING_HEIGHT * 0.15f - position.y;
				float z2 = 0f - position.y;
				array[num++] = new Vector3(x2, y2, z2);
				array[num++] = new Vector3(x2, y2, z);
				array[num++] = new Vector3(x, y2, z);
				array[num++] = new Vector3(x, y2, z2);
				array[num++] = new Vector3(x, y, z2);
				array[num++] = new Vector3(x, y, z);
				array[num++] = new Vector3(x2, y, z);
				array[num++] = new Vector3(x2, y, z2);
				array[num++] = new Vector3(x, y2, z2);
				array[num++] = new Vector3(x, y2, z);
				array[num++] = new Vector3(x, y, z);
				array[num++] = new Vector3(x, y, z2);
				array[num++] = new Vector3(x2, y, z2);
				array[num++] = new Vector3(x2, y2, z2);
				array[num++] = new Vector3(x, y2, z2);
				array[num++] = new Vector3(x, y, z2);
				array[num++] = new Vector3(x2, y, z2);
				array[num++] = new Vector3(x2, y, z);
				array[num++] = new Vector3(x2, y2, z);
				array[num++] = new Vector3(x2, y2, z2);
				array[num++] = new Vector3(x, y, z);
				array[num++] = new Vector3(x, y2, z);
				array[num++] = new Vector3(x2, y2, z);
				array[num++] = new Vector3(x2, y, z);
				for (int j = 0; j < 6; j++)
				{
					array2[j * 4] = new Vector2(0f, 0f);
					array2[j * 4 + 1] = new Vector2(0f, CEILING_HEIGHT);
					array2[j * 4 + 2] = new Vector2(1f, CEILING_HEIGHT);
					array2[j * 4 + 3] = new Vector2(1f, 0f);
				}
				int num2 = objList.objInfo[i].flags & 3;
				Material material = (Material)Resources.Load(UWClass._RES + "/Materials/tmobj/tmobj_" + num2.ToString("d2"));
				if (!(material != null))
				{
					Debug.Log("RenderPillar: Missing material resource for tmobj/" + num2);
					break;
				}
				if (material.mainTexture == null)
				{
					material.mainTexture = GameWorldController.instance.TmObjArt.LoadImageAt(num2);
				}
				Material[] array3 = new Material[6];
				for (int k = 0; k <= array3.GetUpperBound(0); k++)
				{
					array3[k] = material;
				}
				RenderCuboid(Parent, array, array2, position, array3, 6, ObjectLoader.UniqueObjectName(objList.objInfo[i]));
			}
		}
	}

	public static void RenderBridges(GameObject Parent, TileMap level, ObjectLoader objList)
	{
		if (objList == null)
		{
			return;
		}
		for (int i = 0; i <= objList.objInfo.GetUpperBound(0); i++)
		{
			if (objList.objInfo[i] != null && objList.objInfo[i].GetItemType() == 7 && objList.objInfo[i].InUseFlag == 1 && objList.objInfo[i].invis == 0)
			{
				RenderBridge(Parent, level, objList, i);
			}
		}
	}

	public static void RenderBridge(GameObject Parent, TileMap level, ObjectLoader objList, int i)
	{
		Vector3 vector = ObjectLoader.CalcObjectXYZ(i, 0);
		vector = new Vector3((float)objList.objInfo[i].ObjectTileX * 1.2f + 0.6f, vector.y, (float)objList.objInfo[i].ObjectTileY * 1.2f + 0.6f);
		Vector3[] array = new Vector3[24];
		Vector2[] array2 = new Vector2[24];
		int num = 0;
		float x = 0.6f;
		float x2 = -0.6f;
		float y = -0.6f;
		float y2 = 0.6f;
		float z = 0.075f;
		float z2 = -0.075f;
		array[num++] = new Vector3(x2, y2, z2);
		array[num++] = new Vector3(x2, y2, z);
		array[num++] = new Vector3(x, y2, z);
		array[num++] = new Vector3(x, y2, z2);
		array[num++] = new Vector3(x, y, z2);
		array[num++] = new Vector3(x, y, z);
		array[num++] = new Vector3(x2, y, z);
		array[num++] = new Vector3(x2, y, z2);
		array[num++] = new Vector3(x, y2, z2);
		array[num++] = new Vector3(x, y2, z);
		array[num++] = new Vector3(x, y, z);
		array[num++] = new Vector3(x, y, z2);
		array[num++] = new Vector3(x2, y, z2);
		array[num++] = new Vector3(x2, y2, z2);
		array[num++] = new Vector3(x, y2, z2);
		array[num++] = new Vector3(x, y, z2);
		array[num++] = new Vector3(x2, y, z2);
		array[num++] = new Vector3(x2, y, z);
		array[num++] = new Vector3(x2, y2, z);
		array[num++] = new Vector3(x2, y2, z2);
		array[num++] = new Vector3(x, y, z);
		array[num++] = new Vector3(x, y2, z);
		array[num++] = new Vector3(x2, y2, z);
		array[num++] = new Vector3(x2, y, z);
		for (int j = 0; j < 6; j++)
		{
			array2[j * 4] = new Vector2(0f, 0f);
			array2[j * 4 + 1] = new Vector2(0f, 1f);
			array2[j * 4 + 2] = new Vector2(1f, 1f);
			array2[j * 4 + 3] = new Vector2(1f, 0f);
		}
		int num2 = (objList.objInfo[i].enchantment << 3) | (objList.objInfo[i].flags & 0x3F);
		Material material;
		if (num2 >= 2)
		{
			num2 = ((!(UWClass._RES == "UW2")) ? UWClass.CurrentTileMap().texture_map[num2 - 2 + 48] : UWClass.CurrentTileMap().texture_map[num2 - 2]);
			material = GameWorldController.instance.MaterialMasterList[num2];
		}
		else
		{
			material = (Material)Resources.Load(UWClass._RES + "/Materials/tmobj/tmobj_" + (30 + num2));
			if (material.mainTexture == null)
			{
				material.mainTexture = GameWorldController.instance.TmObjArt.LoadImageAt(30 + num2);
			}
		}
		Material[] array3 = new Material[6];
		for (int k = 0; k <= array3.GetUpperBound(0); k++)
		{
			array3[k] = material;
		}
		RenderCuboid(Parent, array, array2, vector, array3, 6, ObjectLoader.UniqueObjectName(objList.objInfo[i]));
	}

	public static GameObject RenderTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert, bool skipFloor, bool skipCeil)
	{
		switch (t.tileType)
		{
		case 0:
			return RenderSolidTile(parent, x, y, t, Water);
		case 1:
			if (!skipFloor)
			{
				return RenderOpenTile(parent, x, y, t, Water, false);
			}
			if (!skipCeil)
			{
				return RenderOpenTile(parent, x, y, t, Water, true);
			}
			break;
		case 2:
			if (!skipFloor)
			{
				RenderDiagSETile(parent, x, y, t, Water, false);
			}
			if (!skipCeil)
			{
				RenderDiagSETile(parent, x, y, t, Water, true);
			}
			return null;
		case 3:
			if (!skipFloor)
			{
				RenderDiagSWTile(parent, x, y, t, Water, false);
			}
			if (!skipCeil)
			{
				RenderDiagSWTile(parent, x, y, t, Water, true);
			}
			return null;
		case 4:
			if (!skipFloor)
			{
				RenderDiagNETile(parent, x, y, t, Water, invert);
			}
			if (!skipCeil)
			{
				RenderDiagNETile(parent, x, y, t, Water, true);
			}
			return null;
		case 5:
			if (!skipFloor)
			{
				RenderDiagNWTile(parent, x, y, t, Water, invert);
			}
			if (!skipCeil)
			{
				RenderDiagNWTile(parent, x, y, t, Water, true);
			}
			return null;
		case 6:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderSlopeNTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderSlopeNTile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderSlopeNTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderSlopeSTile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderSlopeNTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				RenderSlopeNTile(parent, x, y, t, Water, true);
				break;
			}
			return null;
		case 7:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderSlopeSTile(parent, x, y, t, Water, false);
				}
				RenderSlopeSTile(parent, x, y, t, Water, true);
				break;
			case 1:
				if (!skipFloor)
				{
					RenderSlopeSTile(parent, x, y, t, Water, false);
				}
				RenderSlopeNTile(parent, x, y, t, Water, true);
				break;
			case 2:
				if (!skipFloor)
				{
					RenderSlopeSTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderSlopeSTile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 8:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderSlopeETile(parent, x, y, t, Water, false);
				}
				RenderSlopeETile(parent, x, y, t, Water, true);
				break;
			case 1:
				if (!skipFloor)
				{
					RenderSlopeETile(parent, x, y, t, Water, false);
				}
				RenderSlopeWTile(parent, x, y, t, Water, true);
				break;
			case 2:
				if (!skipFloor)
				{
					RenderSlopeETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderSlopeETile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 9:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderSlopeWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderSlopeWTile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderSlopeWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderSlopeETile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderSlopeWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderSlopeWTile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 10:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderValleyNWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeNWTile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderValleyNWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleySETile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderValleyNWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeNWTile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 11:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderValleyNETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeNETile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderValleyNETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleySWTile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderValleyNETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeNETile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 12:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderValleySETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeSETile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderValleySETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleyNWTile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderValleySETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeSETile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 13:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderValleySWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeSWTile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderValleySWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleyNETile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderValleySWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeSWTile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 14:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderRidgeSETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleySETile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderRidgeSETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeNWTile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderRidgeSETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleySETile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 15:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderRidgeSWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleySWTile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderRidgeSWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeNETile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderRidgeSWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleySWTile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 16:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderRidgeNWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleyNWTile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderRidgeNWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeSETile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderRidgeNWTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleyNWTile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		case 17:
			switch (t.shockSlopeFlag)
			{
			case 0:
				if (!skipFloor)
				{
					RenderRidgeNETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleyNETile(parent, x, y, t, Water, true);
				}
				break;
			case 1:
				if (!skipFloor)
				{
					RenderRidgeNETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderRidgeSWTile(parent, x, y, t, Water, true);
				}
				break;
			case 2:
				if (!skipFloor)
				{
					RenderRidgeNETile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderOpenTile(parent, x, y, t, Water, true);
				}
				break;
			case 3:
				if (!skipFloor)
				{
					RenderOpenTile(parent, x, y, t, Water, false);
				}
				if (!skipCeil)
				{
					RenderValleyNETile(parent, x, y, t, Water, true);
				}
				break;
			}
			return null;
		}
		return null;
	}

	private static GameObject RenderCuboid(GameObject parent, int x, int y, TileInfo t, bool Water, int Bottom, int Top, string TileName)
	{
		if (UWEBase.EditorMode && t.tileType == 0)
		{
			t.VisibleFaces[0] = true;
		}
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			if (t.VisibleFaces[i])
			{
				num++;
			}
		}
		Vector3[] array = new Vector3[num * 4];
		Vector2[] array2 = new Vector2[num * 4];
		float z = (float)Top * 0.15f;
		float z2 = (float)Bottom * 0.15f;
		float num2 = t.DimX;
		float num3 = t.DimY;
		GameObject gameObject = new GameObject(TileName);
		SetTileLayer(t, gameObject);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = new Vector3((float)x * 1.2f, 0f, (float)y * 1.2f);
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num;
		Material[] array3 = new Material[num];
		int num4 = 0;
		float num5 = Top - Bottom;
		float num6 = (float)Bottom * 0.125f;
		float num7 = num5 / 8f + num6;
		float num8 = 0f;
		for (int j = 0; j < 6; j++)
		{
			if (!t.VisibleFaces[j])
			{
				continue;
			}
			switch (j)
			{
			case 0:
				array3[num4] = GameWorldController.instance.MaterialMasterList[FloorTexture(128, t)];
				if (UWClass._RES == "UW1" && GameWorldController.instance.LevelNo == 6 && t.floorTexture == 4)
				{
					array3[num4] = GameWorldController.instance.SpecialMaterials[0];
				}
				if (t.tileType == 0 && UWEBase.EditorMode)
				{
					array3[num4] = GameWorldController.instance.Jorge;
				}
				array[4 * num4] = new Vector3(0f, 0f, z);
				array[1 + 4 * num4] = new Vector3(0f, 1.2f * num3, z);
				array[2 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z);
				array[3 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z);
				array2[4 * num4] = new Vector2(0f, 1f * num3);
				array2[1 + 4 * num4] = new Vector2(0f, 0f);
				array2[2 + 4 * num4] = new Vector2(1f * num2, 0f);
				array2[3 + 4 * num4] = new Vector2(1f * num2, 1f * num3);
				break;
			case 4:
				num8 = CalcCeilOffset(32, t);
				array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(32, t)];
				array[4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
				array[1 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z);
				array[2 + 4 * num4] = new Vector3(0f, 1.2f * num3, z);
				array[3 + 4 * num4] = new Vector3(0f, 1.2f * num3, z2);
				array2[4 * num4] = new Vector2(0f, num6 - num8);
				array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
				array2[2 + 4 * num4] = new Vector2(num2, num7 - num8);
				array2[3 + 4 * num4] = new Vector2(num2, num6 - num8);
				break;
			case 3:
				num8 = CalcCeilOffset(4, t);
				array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(4, t)];
				array[4 * num4] = new Vector3(0f, 1.2f * num3, z2);
				array[1 + 4 * num4] = new Vector3(0f, 1.2f * num3, z);
				array[2 + 4 * num4] = new Vector3(0f, 0f, z);
				array[3 + 4 * num4] = new Vector3(0f, 0f, z2);
				array2[4 * num4] = new Vector2(0f, num6 - num8);
				array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
				array2[2 + 4 * num4] = new Vector2(num3, num7 - num8);
				array2[3 + 4 * num4] = new Vector2(num3, num6 - num8);
				break;
			case 1:
				num8 = CalcCeilOffset(8, t);
				array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(8, t)];
				array[4 * num4] = new Vector3(-1.2f * num2, 0f, z2);
				array[1 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z);
				array[2 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z);
				array[3 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
				array2[4 * num4] = new Vector2(0f, num6 - num8);
				array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
				array2[2 + 4 * num4] = new Vector2(num3, num7 - num8);
				array2[3 + 4 * num4] = new Vector2(num3, num6 - num8);
				break;
			case 5:
				num8 = CalcCeilOffset(16, t);
				array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(16, t)];
				array[4 * num4] = new Vector3(0f, 0f, z2);
				array[1 + 4 * num4] = new Vector3(0f, 0f, z);
				array[2 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z);
				array[3 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z2);
				array2[4 * num4] = new Vector2(0f, num6 - num8);
				array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
				array2[2 + 4 * num4] = new Vector2(num2, num7 - num8);
				array2[3 + 4 * num4] = new Vector2(num2, num6 - num8);
				break;
			case 2:
				array3[num4] = GameWorldController.instance.MaterialMasterList[FloorTexture(64, t)];
				array[4 * num4] = new Vector3(0f, 1.2f * num3, z2);
				array[1 + 4 * num4] = new Vector3(0f, 0f, z2);
				array[2 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z2);
				array[3 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
				array2[4 * num4] = new Vector2(0f, 0f);
				array2[1 + 4 * num4] = new Vector2(0f, 1f * num3);
				array2[2 + 4 * num4] = new Vector2(num2, 1f * num3);
				array2[3 + 4 * num4] = new Vector2(num2, 0f);
				break;
			}
			num4++;
		}
		mesh.vertices = array;
		mesh.uv = array2;
		num4 = 0;
		int[] array4 = new int[6];
		for (int k = 0; k < 6; k++)
		{
			if (t.VisibleFaces[k])
			{
				array4[0] = 4 * num4;
				array4[1] = 1 + 4 * num4;
				array4[2] = 2 + 4 * num4;
				array4[3] = 4 * num4;
				array4[4] = 2 + 4 * num4;
				array4[5] = 3 + 4 * num4;
				mesh.SetTriangles(array4, num4);
				num4++;
			}
		}
		meshRenderer.materials = array3;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
		return gameObject;
	}

	private static GameObject RenderCuboid(GameObject parent, Vector3[] verts, Vector2[] uvs, Vector3 position, Material[] MatsToUse, int NoOfFaces, string name)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = position;
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		gameObject.layer = LayerMask.NameToLayer("MapMesh");
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.subMeshCount = NoOfFaces;
		int num = 0;
		int[] array = new int[6];
		for (int i = 0; i < NoOfFaces; i++)
		{
			array[0] = 4 * num;
			array[1] = 1 + 4 * num;
			array[2] = 2 + 4 * num;
			array[3] = 4 * num;
			array[4] = 2 + 4 * num;
			array[5] = 3 + 4 * num;
			mesh.SetTriangles(array, num);
			num++;
		}
		meshRenderer.materials = MatsToUse;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
		return gameObject;
	}

	private static GameObject RenderPrism(GameObject parent, int x, int y, TileInfo t, bool Water, int Bottom, int Top, string TileName)
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			if (t.VisibleFaces[i])
			{
				num++;
			}
		}
		Vector3[] array = new Vector3[num * 4];
		Vector2[] array2 = new Vector2[num * 4];
		float z = (float)Top * 0.15f;
		float z2 = (float)Bottom * 0.15f;
		float num2 = t.DimX;
		float num3 = t.DimY;
		GameObject gameObject = new GameObject(TileName);
		SetTileLayer(t, gameObject);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = new Vector3((float)x * 1.2f, 0f, (float)y * 1.2f);
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num;
		Material[] array3 = new Material[num];
		int num4 = 0;
		float num5 = Top - Bottom;
		float num6 = (float)Bottom * 0.125f;
		float num7 = num5 / 8f + num6;
		float num8 = 0f;
		for (int j = 0; j < 6; j++)
		{
			if (t.VisibleFaces[j])
			{
				switch (j)
				{
				case 0:
					array3[num4] = GameWorldController.instance.MaterialMasterList[FloorTexture(128, t)];
					array[4 * num4] = new Vector3(0f, 0f, z);
					array[1 + 4 * num4] = new Vector3(0f, 1.2f * num3, z);
					array[2 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z);
					array[3 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z);
					array2[4 * num4] = new Vector2(0f, 1f * num3);
					array2[1 + 4 * num4] = new Vector2(0f, 0f);
					array2[2 + 4 * num4] = new Vector2(1f * num2, 0f);
					array2[3 + 4 * num4] = new Vector2(1f * num2, 1f * num3);
					break;
				case 4:
					num8 = CalcCeilOffset(32, t);
					array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(32, t)];
					array[4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
					array[1 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z);
					array[2 + 4 * num4] = new Vector3(0f, 1.2f * num3, z);
					array[3 + 4 * num4] = new Vector3(0f, 1.2f * num3, z2);
					array2[4 * num4] = new Vector2(0f, num6 - num8);
					array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
					array2[2 + 4 * num4] = new Vector2(num2, num7 - num8);
					array2[3 + 4 * num4] = new Vector2(num2, num6 - num8);
					break;
				case 3:
					num8 = CalcCeilOffset(4, t);
					array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(4, t)];
					array[4 * num4] = new Vector3(0f, 1.2f * num3, z2);
					array[1 + 4 * num4] = new Vector3(0f, 1.2f * num3, z);
					array[2 + 4 * num4] = new Vector3(0f, 0f, z);
					array[3 + 4 * num4] = new Vector3(0f, 0f, z2);
					array2[4 * num4] = new Vector2(0f, num6 - num8);
					array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
					array2[2 + 4 * num4] = new Vector2(num3, num7 - num8);
					array2[3 + 4 * num4] = new Vector2(num3, num6 - num8);
					break;
				case 1:
					num8 = CalcCeilOffset(8, t);
					array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(8, t)];
					array[4 * num4] = new Vector3(-1.2f * num2, 0f, z2);
					array[1 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z);
					array[2 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z);
					array[3 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
					array2[4 * num4] = new Vector2(0f, num6 - num8);
					array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
					array2[2 + 4 * num4] = new Vector2(num3, num7 - num8);
					array2[3 + 4 * num4] = new Vector2(num3, num6 - num8);
					break;
				case 5:
					num8 = CalcCeilOffset(16, t);
					array3[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(16, t)];
					array[4 * num4] = new Vector3(0f, 0f, z2);
					array[1 + 4 * num4] = new Vector3(0f, 0f, z);
					array[2 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z);
					array[3 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z2);
					array2[4 * num4] = new Vector2(0f, num6 - num8);
					array2[1 + 4 * num4] = new Vector2(0f, num7 - num8);
					array2[2 + 4 * num4] = new Vector2(num2, num7 - num8);
					array2[3 + 4 * num4] = new Vector2(num2, num6 - num8);
					break;
				case 2:
					array3[num4] = GameWorldController.instance.MaterialMasterList[FloorTexture(64, t)];
					array[4 * num4] = new Vector3(0f, 1.2f * num3, z2);
					array[1 + 4 * num4] = new Vector3(0f, 0f, z2);
					array[2 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z2);
					array[3 + 4 * num4] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
					array2[4 * num4] = new Vector2(0f, 0f);
					array2[1 + 4 * num4] = new Vector2(0f, 1f * num3);
					array2[2 + 4 * num4] = new Vector2(num2, 1f * num3);
					array2[3 + 4 * num4] = new Vector2(num2, 0f);
					break;
				}
				num4++;
			}
		}
		mesh.vertices = array;
		mesh.uv = array2;
		num4 = 0;
		int num9 = 0;
		for (int k = 0; k < 6; k++)
		{
			int[] array4 = ((num9 != 0) ? new int[6] : new int[3]);
			if (!t.VisibleFaces[k])
			{
				continue;
			}
			if (k == 0)
			{
				switch (t.tileType)
				{
				case 4:
					array4[0] = 1 + 4 * num4;
					array4[1] = 2 + 4 * num4;
					array4[2] = 3 + 4 * num4;
					break;
				case 2:
					array4[0] = 4 * num4;
					array4[1] = 2 + 4 * num4;
					array4[2] = 3 + 4 * num4;
					break;
				case 3:
					array4[0] = 4 * num4;
					array4[1] = 1 + 4 * num4;
					array4[2] = 3 + 4 * num4;
					break;
				default:
					array4[0] = 4 * num4;
					array4[1] = 1 + 4 * num4;
					array4[2] = 2 + 4 * num4;
					break;
				}
				mesh.SetTriangles(array4, num4);
			}
			else
			{
				array4[0] = 4 * num4;
				array4[1] = 1 + 4 * num4;
				array4[2] = 2 + 4 * num4;
				array4[3] = 4 * num4;
				array4[4] = 2 + 4 * num4;
				array4[5] = 3 + 4 * num4;
				mesh.SetTriangles(array4, num4);
			}
			num4++;
			num9++;
		}
		meshRenderer.materials = array3;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
		return gameObject;
	}

	private static GameObject RenderSolidTile(GameObject parent, int x, int y, TileInfo t, bool Water)
	{
		if (t.Render && t.isWater == Water)
		{
			string tileName = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[0] = false;
			t.VisibleFaces[2] = false;
			return RenderCuboid(parent, x, y, t, Water, 0, CEILING_HEIGHT, tileName);
		}
		return null;
	}

	private static GameObject RenderOpenTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (t.Render)
		{
			string text = "";
			if (t.isWater == Water)
			{
				if (!invert)
				{
					if (t.TerrainChange)
					{
						text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
						return RenderCuboid(parent, x, y, t, Water, -16, t.floorHeight, text);
					}
					text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
					return RenderCuboid(parent, x, y, t, Water, -CEILING_HEIGHT, t.floorHeight, text);
				}
				bool flag = t.VisibleFaces[2];
				bool flag2 = t.VisibleFaces[0];
				t.VisibleFaces[2] = true;
				t.VisibleFaces[0] = false;
				text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
				GameObject result = RenderCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, text);
				t.VisibleFaces[2] = flag;
				t.VisibleFaces[0] = flag2;
				return result;
			}
		}
		return null;
	}

	private static GameObject RenderDiagOpenTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (t.Render)
		{
			string text = "";
			if (t.isWater == Water)
			{
				if (!invert)
				{
					if (t.TerrainChange)
					{
						text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
						return RenderPrism(parent, x, y, t, Water, -16, t.floorHeight, text);
					}
					text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
					return RenderPrism(parent, x, y, t, Water, -CEILING_HEIGHT, t.floorHeight, text);
				}
				bool flag = t.VisibleFaces[2];
				bool flag2 = t.VisibleFaces[0];
				t.VisibleFaces[2] = true;
				t.VisibleFaces[0] = false;
				text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
				GameObject result = RenderCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, text);
				t.VisibleFaces[2] = flag;
				t.VisibleFaces[0] = flag2;
				return result;
			}
		}
		return null;
	}

	private static void RenderDiagSETile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (!Water)
			{
				text = "Wall_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderDiagSEPortion(parent, 0, CEILING_HEIGHT, t, text);
			}
			if (t.isWater == Water)
			{
				bool flag = t.VisibleFaces[4];
				bool flag2 = t.VisibleFaces[3];
				t.VisibleFaces[4] = false;
				t.VisibleFaces[3] = false;
				RenderDiagOpenTile(parent, x, y, t, Water, false);
				t.VisibleFaces[4] = flag;
				t.VisibleFaces[3] = flag2;
			}
		}
		else
		{
			bool flag3 = t.VisibleFaces[2];
			t.VisibleFaces[2] = true;
			RenderOpenTile(parent, x, y, t, Water, true);
			t.VisibleFaces[2] = flag3;
		}
	}

	private static void RenderDiagSWTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (!Water)
			{
				text = "Wall_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderDiagSWPortion(parent, 0, CEILING_HEIGHT, t, text);
			}
			if (t.isWater == Water)
			{
				bool flag = t.VisibleFaces[4];
				bool flag2 = t.VisibleFaces[1];
				t.VisibleFaces[4] = false;
				t.VisibleFaces[1] = false;
				RenderDiagOpenTile(parent, x, y, t, Water, false);
				t.VisibleFaces[4] = flag;
				t.VisibleFaces[1] = flag2;
			}
		}
		else
		{
			bool flag3 = t.VisibleFaces[2];
			t.VisibleFaces[2] = true;
			RenderOpenTile(parent, x, y, t, Water, true);
			t.VisibleFaces[2] = flag3;
		}
	}

	private static void RenderDiagNETile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (!Water)
			{
				text = "Wall_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderDiagNEPortion(parent, 0, CEILING_HEIGHT, t, text);
			}
			if (t.isWater == Water)
			{
				bool flag = t.VisibleFaces[5];
				bool flag2 = t.VisibleFaces[3];
				t.VisibleFaces[5] = false;
				t.VisibleFaces[3] = false;
				RenderDiagOpenTile(parent, x, y, t, Water, false);
				t.VisibleFaces[5] = flag;
				t.VisibleFaces[3] = flag2;
			}
		}
		else
		{
			bool flag3 = t.VisibleFaces[2];
			t.VisibleFaces[2] = true;
			RenderOpenTile(parent, x, y, t, Water, true);
			t.VisibleFaces[2] = flag3;
		}
	}

	private static void RenderDiagNWTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (!Water)
			{
				text = "Wall_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderDiagNWPortion(parent, 0, CEILING_HEIGHT, t, text);
			}
			if (t.isWater == Water)
			{
				bool flag = t.VisibleFaces[5];
				bool flag2 = t.VisibleFaces[1];
				t.VisibleFaces[5] = false;
				t.VisibleFaces[1] = false;
				RenderDiagOpenTile(parent, x, y, t, Water, false);
				t.VisibleFaces[5] = flag;
				t.VisibleFaces[1] = flag2;
			}
		}
		else
		{
			bool flag3 = t.VisibleFaces[2];
			t.VisibleFaces[2] = true;
			RenderOpenTile(parent, x, y, t, Water, true);
			t.VisibleFaces[2] = flag3;
		}
	}

	private static void RenderSlopeNTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 6, t.shockSteep, 1, text);
			}
			return;
		}
		text = "N_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
		bool flag = t.VisibleFaces[2];
		bool flag2 = t.VisibleFaces[0];
		t.VisibleFaces[2] = true;
		t.VisibleFaces[0] = false;
		RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 6, t.shockSteep, 0, text);
		t.VisibleFaces[2] = flag;
		t.VisibleFaces[0] = flag2;
	}

	private static void RenderSlopeSTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 7, t.shockSteep, 1, text);
			}
			return;
		}
		text = "S_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
		bool flag = t.VisibleFaces[2];
		bool flag2 = t.VisibleFaces[0];
		t.VisibleFaces[2] = true;
		t.VisibleFaces[0] = false;
		RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 7, t.shockSteep, 0, text);
		t.VisibleFaces[2] = flag;
		t.VisibleFaces[0] = flag2;
	}

	private static void RenderSlopeWTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 9, t.shockSteep, 1, text);
			}
			return;
		}
		text = "W_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
		bool flag = t.VisibleFaces[2];
		bool flag2 = t.VisibleFaces[0];
		t.VisibleFaces[2] = true;
		t.VisibleFaces[0] = false;
		RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 9, t.shockSteep, 0, text);
		t.VisibleFaces[2] = flag;
		t.VisibleFaces[0] = flag2;
	}

	private static void RenderSlopeETile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		string text = "";
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				text = "Tile_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 8, t.shockSteep, 1, text);
			}
			return;
		}
		text = "E_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
		bool flag = t.VisibleFaces[2];
		bool flag2 = t.VisibleFaces[0];
		t.VisibleFaces[2] = true;
		t.VisibleFaces[0] = false;
		RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 8, t.shockSteep, 0, text);
		t.VisibleFaces[2] = flag;
		t.VisibleFaces[0] = flag2;
	}

	private static void RenderValleyNWTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!invert)
		{
			string tileName = "VNW_" + x.ToString("D2") + "_" + y.ToString("D2");
			RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 10, t.shockSteep, 1, tileName);
		}
		else
		{
			string tileName2 = "VNW_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 14, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderValleyNETile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!invert)
		{
			string tileName = "VNE_" + x.ToString("D2") + "_" + y.ToString("D2");
			RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 11, t.shockSteep, 1, tileName);
		}
		else
		{
			string tileName2 = "VNE_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 15, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderValleySWTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!invert)
		{
			string tileName = "VSW_" + x.ToString("D2") + "_" + y.ToString("D2");
			RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 13, t.shockSteep, 1, tileName);
		}
		else
		{
			string tileName2 = "VSW_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 17, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderValleySETile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!invert)
		{
			string tileName = "VSE_" + x.ToString("D2") + "_" + y.ToString("D2");
			RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 12, t.shockSteep, 1, tileName);
		}
		else
		{
			string tileName2 = "VSE_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 16, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderRidgeNWTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				string tileName = "TileRNW_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 16, t.shockSteep, 1, tileName);
			}
		}
		else
		{
			string tileName2 = "RNW_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 12, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderRidgeNETile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				string tileName = "TileRNE_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 17, t.shockSteep, 1, tileName);
			}
		}
		else
		{
			string tileName2 = "RNE_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 13, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderRidgeSWTile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				string tileName = "TileRSW_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 15, t.shockSteep, 1, tileName);
			}
		}
		else
		{
			string tileName2 = "RSW_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 11, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderRidgeSETile(GameObject parent, int x, int y, TileInfo t, bool Water, bool invert)
	{
		if (!t.Render)
		{
			return;
		}
		if (!invert)
		{
			if (t.isWater == Water)
			{
				string tileName = "TileRSE_" + x.ToString("D2") + "_" + y.ToString("D2");
				RenderSlopedCuboid(parent, x, y, t, Water, 0, t.floorHeight, 14, t.shockSteep, 1, tileName);
			}
		}
		else
		{
			string tileName2 = "RNW_Ceiling_" + x.ToString("D2") + "_" + y.ToString("D2");
			t.VisibleFaces[2] = true;
			RenderSlopedCuboid(parent, x, y, t, Water, CEILING_HEIGHT - t.ceilingHeight, CEILING_HEIGHT, 10, t.shockSteep, 0, tileName2);
		}
	}

	private static void RenderDiagSEPortion(GameObject parent, int Bottom, int Top, TileInfo t, string TileName)
	{
		int num = 1;
		for (int i = 0; i < 6; i++)
		{
			if ((i == 4 || i == 3) && t.VisibleFaces[i])
			{
				num++;
			}
		}
		Material[] array = new Material[num];
		Vector3[] array2 = new Vector3[num * 4];
		Vector2[] array3 = new Vector2[num * 4];
		float num2 = 0f;
		float z = (float)Top * 0.15f;
		float z2 = (float)Bottom * 0.15f;
		GameObject gameObject = new GameObject(TileName);
		gameObject.layer = LayerMask.NameToLayer("MapMesh");
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = new Vector3((float)t.tileX * 1.2f, 0f, (float)t.tileY * 1.2f);
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num;
		int num3 = 0;
		array[num3] = GameWorldController.instance.MaterialMasterList[WallTexture(128, t)];
		float num4 = Top - Bottom;
		float num5 = (float)Bottom * 0.125f;
		float num6 = num4 / 8f + num5;
		array2[0] = new Vector3(0f, 0f, z2);
		array2[1] = new Vector3(0f, 0f, z);
		array2[2] = new Vector3(-1.2f, 1.2f, z);
		array2[3] = new Vector3(-1.2f, 1.2f, z2);
		array3[0] = new Vector2(0f, num5);
		array3[1] = new Vector2(0f, num6);
		array3[2] = new Vector2(1f, num6);
		array3[3] = new Vector2(1f, num5);
		num3++;
		for (int j = 0; j < 6; j++)
		{
			if (t.VisibleFaces[j] && (j == 4 || j == 3))
			{
				switch (j)
				{
				case 4:
					num2 = CalcCeilOffset(32, t);
					array[num3] = GameWorldController.instance.MaterialMasterList[WallTexture(32, t)];
					array2[4 * num3] = new Vector3(-1.2f, 1.2f, z2);
					array2[1 + 4 * num3] = new Vector3(-1.2f, 1.2f, z);
					array2[2 + 4 * num3] = new Vector3(0f, 1.2f, z);
					array2[3 + 4 * num3] = new Vector3(0f, 1.2f, z2);
					array3[4 * num3] = new Vector2(0f, num5 - num2);
					array3[1 + 4 * num3] = new Vector2(0f, num6 - num2);
					array3[2 + 4 * num3] = new Vector2(1f, num6 - num2);
					array3[3 + 4 * num3] = new Vector2(1f, num5 - num2);
					break;
				case 3:
					num2 = CalcCeilOffset(4, t);
					array[num3] = GameWorldController.instance.MaterialMasterList[WallTexture(4, t)];
					array2[4 * num3] = new Vector3(0f, 1.2f, z2);
					array2[1 + 4 * num3] = new Vector3(0f, 1.2f, z);
					array2[2 + 4 * num3] = new Vector3(0f, 0f, z);
					array2[3 + 4 * num3] = new Vector3(0f, 0f, z2);
					array3[4 * num3] = new Vector2(0f, num5 - num2);
					array3[1 + 4 * num3] = new Vector2(0f, num6 - num2);
					array3[2 + 4 * num3] = new Vector2(1f, num6 - num2);
					array3[3 + 4 * num3] = new Vector2(1f, num5 - num2);
					break;
				}
				num3++;
			}
		}
		mesh.vertices = array2;
		mesh.uv = array3;
		int[] array4 = new int[6] { 0, 1, 2, 0, 2, 3 };
		mesh.SetTriangles(array4, 0);
		num3 = 1;
		for (int k = 0; k < 6; k++)
		{
			if ((k == 4 || k == 3) && t.VisibleFaces[k])
			{
				array4[0] = 4 * num3;
				array4[1] = 1 + 4 * num3;
				array4[2] = 2 + 4 * num3;
				array4[3] = 4 * num3;
				array4[4] = 2 + 4 * num3;
				array4[5] = 3 + 4 * num3;
				mesh.SetTriangles(array4, num3);
				num3++;
			}
		}
		meshRenderer.materials = array;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
	}

	private static void RenderDiagSWPortion(GameObject parent, int Bottom, int Top, TileInfo t, string TileName)
	{
		int num = 1;
		for (int i = 0; i < 6; i++)
		{
			if ((i == 4 || i == 1) && t.VisibleFaces[i])
			{
				num++;
			}
		}
		Material[] array = new Material[num];
		Vector3[] array2 = new Vector3[num * 4];
		Vector2[] array3 = new Vector2[num * 4];
		float z = (float)Top * 0.15f;
		float z2 = (float)Bottom * 0.15f;
		float num2 = t.DimX;
		float num3 = t.DimY;
		float num4 = 0f;
		GameObject gameObject = new GameObject(TileName);
		gameObject.layer = LayerMask.NameToLayer("MapMesh");
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = new Vector3((float)t.tileX * 1.2f, 0f, (float)t.tileY * 1.2f);
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num;
		int num5 = 0;
		array[num5] = GameWorldController.instance.MaterialMasterList[WallTexture(128, t)];
		float num6 = Top - Bottom;
		float num7 = (float)Bottom * 0.125f;
		float num8 = num6 / 8f + num7;
		array2[0] = new Vector3(0f, 1.2f, z2);
		array2[1] = new Vector3(0f, 1.2f, z);
		array2[2] = new Vector3(-1.2f, 0f, z);
		array2[3] = new Vector3(-1.2f, 0f, z2);
		array3[0] = new Vector2(0f, num7);
		array3[1] = new Vector2(0f, num8);
		array3[2] = new Vector2(1f, num8);
		array3[3] = new Vector2(1f, num7);
		num5++;
		for (int j = 0; j < 6; j++)
		{
			if (t.VisibleFaces[j] && (j == 4 || j == 1))
			{
				switch (j)
				{
				case 4:
					num4 = CalcCeilOffset(32, t);
					array[num5] = GameWorldController.instance.MaterialMasterList[WallTexture(32, t)];
					array2[4 * num5] = new Vector3(-1.2f, 1.2f, z2);
					array2[1 + 4 * num5] = new Vector3(-1.2f, 1.2f, z);
					array2[2 + 4 * num5] = new Vector3(0f, 1.2f, z);
					array2[3 + 4 * num5] = new Vector3(0f, 1.2f, z2);
					array3[4 * num5] = new Vector2(0f, num7 - num4);
					array3[1 + 4 * num5] = new Vector2(0f, num8 - num4);
					array3[2 + 4 * num5] = new Vector2(1f, num8 - num4);
					array3[3 + 4 * num5] = new Vector2(1f, num7 - num4);
					break;
				case 1:
					num4 = CalcCeilOffset(8, t);
					array[num5] = GameWorldController.instance.MaterialMasterList[WallTexture(8, t)];
					array2[4 * num5] = new Vector3(-1.2f * num2, 0f, z2);
					array2[1 + 4 * num5] = new Vector3(-1.2f * num2, 0f, z);
					array2[2 + 4 * num5] = new Vector3(-1.2f * num2, 1.2f * num3, z);
					array2[3 + 4 * num5] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
					array3[4 * num5] = new Vector2(0f, num7 - num4);
					array3[1 + 4 * num5] = new Vector2(0f, num8 - num4);
					array3[2 + 4 * num5] = new Vector2(num3, num8 - num4);
					array3[3 + 4 * num5] = new Vector2(num3, num7 - num4);
					break;
				}
				num5++;
			}
		}
		mesh.vertices = array2;
		mesh.uv = array3;
		int[] array4 = new int[6] { 0, 1, 2, 0, 2, 3 };
		mesh.SetTriangles(array4, 0);
		num5 = 1;
		for (int k = 0; k < 6; k++)
		{
			if ((k == 4 || k == 1) && t.VisibleFaces[k])
			{
				array4[0] = 4 * num5;
				array4[1] = 1 + 4 * num5;
				array4[2] = 2 + 4 * num5;
				array4[3] = 4 * num5;
				array4[4] = 2 + 4 * num5;
				array4[5] = 3 + 4 * num5;
				mesh.SetTriangles(array4, num5);
				num5++;
			}
		}
		meshRenderer.materials = array;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
	}

	private static void RenderDiagNWPortion(GameObject parent, int Bottom, int Top, TileInfo t, string TileName)
	{
		int num = 1;
		for (int i = 0; i < 6; i++)
		{
			if ((i == 5 || i == 1) && t.VisibleFaces[i])
			{
				num++;
			}
		}
		Material[] array = new Material[num];
		Vector3[] array2 = new Vector3[num * 4];
		Vector2[] array3 = new Vector2[num * 4];
		float z = (float)Top * 0.15f;
		float z2 = (float)Bottom * 0.15f;
		float num2 = t.DimX;
		float num3 = t.DimY;
		float num4 = 0f;
		GameObject gameObject = new GameObject(TileName);
		gameObject.layer = LayerMask.NameToLayer("MapMesh");
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = new Vector3((float)t.tileX * 1.2f, 0f, (float)t.tileY * 1.2f);
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num;
		int num5 = 0;
		float num6 = Top - Bottom;
		float num7 = (float)Bottom * 0.125f;
		float num8 = num6 / 8f + num7;
		array[num5] = GameWorldController.instance.MaterialMasterList[WallTexture(128, t)];
		array2[0] = new Vector3(-1.2f, 1.2f, z2);
		array2[1] = new Vector3(-1.2f, 1.2f, z);
		array2[2] = new Vector3(0f, 0f, z);
		array2[3] = new Vector3(0f, 0f, z2);
		array3[0] = new Vector2(0f, num7);
		array3[1] = new Vector2(0f, num8);
		array3[2] = new Vector2(1f, num8);
		array3[3] = new Vector2(1f, num7);
		num5++;
		for (int j = 0; j < 6; j++)
		{
			if (t.VisibleFaces[j] && (j == 5 || j == 1))
			{
				switch (j)
				{
				case 1:
					num4 = CalcCeilOffset(8, t);
					array[num5] = GameWorldController.instance.MaterialMasterList[WallTexture(8, t)];
					array2[4 * num5] = new Vector3(-1.2f * num2, 0f, z2);
					array2[1 + 4 * num5] = new Vector3(-1.2f * num2, 0f, z);
					array2[2 + 4 * num5] = new Vector3(-1.2f * num2, 1.2f * num3, z);
					array2[3 + 4 * num5] = new Vector3(-1.2f * num2, 1.2f * num3, z2);
					array3[4 * num5] = new Vector2(0f, num7 - num4);
					array3[1 + 4 * num5] = new Vector2(0f, num8 - num4);
					array3[2 + 4 * num5] = new Vector2(num3, num8 - num4);
					array3[3 + 4 * num5] = new Vector2(num3, num7 - num4);
					break;
				case 5:
					num4 = CalcCeilOffset(16, t);
					array[num5] = GameWorldController.instance.MaterialMasterList[WallTexture(16, t)];
					array2[4 * num5] = new Vector3(0f, 0f, z2);
					array2[1 + 4 * num5] = new Vector3(0f, 0f, z);
					array2[2 + 4 * num5] = new Vector3(-1.2f * num2, 0f, z);
					array2[3 + 4 * num5] = new Vector3(-1.2f * num2, 0f, z2);
					array3[4 * num5] = new Vector2(0f, num7 - num4);
					array3[1 + 4 * num5] = new Vector2(0f, num8 - num4);
					array3[2 + 4 * num5] = new Vector2(num2, num8 - num4);
					array3[3 + 4 * num5] = new Vector2(num2, num7 - num4);
					break;
				}
				num5++;
			}
		}
		mesh.vertices = array2;
		mesh.uv = array3;
		int[] array4 = new int[6] { 0, 1, 2, 0, 2, 3 };
		mesh.SetTriangles(array4, 0);
		num5 = 1;
		for (int k = 0; k < 6; k++)
		{
			if ((k == 5 || k == 1) && t.VisibleFaces[k])
			{
				array4[0] = 4 * num5;
				array4[1] = 1 + 4 * num5;
				array4[2] = 2 + 4 * num5;
				array4[3] = 4 * num5;
				array4[4] = 2 + 4 * num5;
				array4[5] = 3 + 4 * num5;
				mesh.SetTriangles(array4, num5);
				num5++;
			}
		}
		meshRenderer.materials = array;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
	}

	private static void RenderDiagNEPortion(GameObject parent, int Bottom, int Top, TileInfo t, string TileName)
	{
		int num = 1;
		for (int i = 0; i < 6; i++)
		{
			if ((i == 5 || i == 3) && t.VisibleFaces[i])
			{
				num++;
			}
		}
		Material[] array = new Material[num];
		Vector3[] array2 = new Vector3[num * 4];
		Vector2[] array3 = new Vector2[num * 4];
		float z = (float)Top * 0.15f;
		float z2 = (float)Bottom * 0.15f;
		float num2 = t.DimX;
		float num3 = 0f;
		GameObject gameObject = new GameObject(TileName);
		gameObject.layer = LayerMask.NameToLayer("MapMesh");
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = new Vector3((float)t.tileX * 1.2f, 0f, (float)t.tileY * 1.2f);
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num;
		int num4 = 0;
		float num5 = Top - Bottom;
		float num6 = (float)Bottom * 0.125f;
		float num7 = num5 / 8f + num6;
		array[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(128, t)];
		array2[0] = new Vector3(-1.2f, 0f, z2);
		array2[1] = new Vector3(-1.2f, 0f, z);
		array2[2] = new Vector3(0f, 1.2f, z);
		array2[3] = new Vector3(0f, 1.2f, z2);
		array3[0] = new Vector2(0f, num6);
		array3[1] = new Vector2(0f, num7);
		array3[2] = new Vector2(1f, num7);
		array3[3] = new Vector2(1f, num6);
		num4++;
		for (int j = 0; j < 6; j++)
		{
			if (t.VisibleFaces[j] && (j == 5 || j == 3))
			{
				switch (j)
				{
				case 5:
					num3 = CalcCeilOffset(16, t);
					array[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(16, t)];
					array2[4 * num4] = new Vector3(0f, 0f, z2);
					array2[1 + 4 * num4] = new Vector3(0f, 0f, z);
					array2[2 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z);
					array2[3 + 4 * num4] = new Vector3(-1.2f * num2, 0f, z2);
					array3[4 * num4] = new Vector2(0f, num6 - num3);
					array3[1 + 4 * num4] = new Vector2(0f, num7 - num3);
					array3[2 + 4 * num4] = new Vector2(num2, num7 - num3);
					array3[3 + 4 * num4] = new Vector2(num2, num6 - num3);
					break;
				case 3:
					num3 = CalcCeilOffset(4, t);
					array[num4] = GameWorldController.instance.MaterialMasterList[WallTexture(4, t)];
					array2[4 * num4] = new Vector3(0f, 1.2f, z2);
					array2[1 + 4 * num4] = new Vector3(0f, 1.2f, z);
					array2[2 + 4 * num4] = new Vector3(0f, 0f, z);
					array2[3 + 4 * num4] = new Vector3(0f, 0f, z2);
					array3[4 * num4] = new Vector2(0f, num6 - num3);
					array3[1 + 4 * num4] = new Vector2(0f, num7 - num3);
					array3[2 + 4 * num4] = new Vector2(1f, num7 - num3);
					array3[3 + 4 * num4] = new Vector2(1f, num6 - num3);
					break;
				}
				num4++;
			}
		}
		mesh.vertices = array2;
		mesh.uv = array3;
		int[] array4 = new int[6] { 0, 1, 2, 0, 2, 3 };
		mesh.SetTriangles(array4, 0);
		num4 = 1;
		for (int k = 0; k < 6; k++)
		{
			if ((k == 5 || k == 3) && t.VisibleFaces[k])
			{
				array4[0] = 4 * num4;
				array4[1] = 1 + 4 * num4;
				array4[2] = 2 + 4 * num4;
				array4[3] = 4 * num4;
				array4[4] = 2 + 4 * num4;
				array4[5] = 3 + 4 * num4;
				mesh.SetTriangles(array4, num4);
				num4++;
			}
		}
		meshRenderer.materials = array;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
	}

	public static int WallTexture(int face, TileInfo t)
	{
		int num = t.wallTexture;
		switch (face)
		{
		case 16:
			num = t.South;
			break;
		case 32:
			num = t.North;
			break;
		case 8:
			num = t.East;
			break;
		case 4:
			num = t.West;
			break;
		}
		if (num < 0 || num > 512)
		{
			num = 0;
		}
		if (debugtextures)
		{
			return num;
		}
		return UWClass.CurrentTileMap().texture_map[num];
	}

	public static int FloorTexture(int face, TileInfo t)
	{
		if (debugtextures)
		{
			return t.floorTexture;
		}
		int num;
		if (face == 64)
		{
			num = UWClass.CurrentTileMap().texture_map[t.shockCeilingTexture];
		}
		else
		{
			switch (UWClass._RES)
			{
			case "SHOCK":
			case "UW2":
				num = UWClass.CurrentTileMap().texture_map[t.floorTexture];
				break;
			default:
				num = UWClass.CurrentTileMap().texture_map[t.floorTexture + 48];
				break;
			}
		}
		if (num < 0 || num > 512)
		{
			num = 0;
		}
		return num;
	}

	private static float CalcCeilOffset(int face, TileInfo t)
	{
		int num = t.ceilingHeight;
		if (UWClass._RES != "SHOCK")
		{
			return 0f;
		}
		switch (face)
		{
		case 8:
			num = t.shockEastCeilHeight;
			break;
		case 4:
			num = t.shockWestCeilHeight;
			break;
		case 16:
			num = t.shockSouthCeilHeight;
			break;
		case 32:
			num = t.shockNorthCeilHeight;
			break;
		}
		float num2 = UWClass.CurrentTileMap().SHOCK_CEILING_HEIGHT;
		float num3;
		for (num3 = num2 - (float)num - 8f; num3 >= 8f; num3 -= 8f)
		{
		}
		return num3 * 0.125f;
	}

	private static void CalcUV(int Top, int Bottom, out float uv0, out float uv1)
	{
		float num = Top - Bottom;
		uv0 = (float)Bottom * 0.125f;
		uv1 = num / 8f + uv0;
	}

	private static void RenderSlopedCuboid(GameObject parent, int x, int y, TileInfo t, bool Water, int Bottom, int Top, int SlopeDir, int Steepness, int Floor, string TileName)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		float num13 = 0f;
		float num14 = 0f;
		float num15 = 0f;
		float num16 = 0f;
		if (Floor == 1)
		{
			switch (SlopeDir)
			{
			case 6:
				num = (float)Steepness * 0.15f;
				break;
			case 7:
				num2 = (float)Steepness * 0.15f;
				break;
			case 8:
				num3 = (float)Steepness * 0.15f;
				break;
			case 9:
				num4 = (float)Steepness * 0.15f;
				break;
			case 11:
				num9 = (float)Steepness * 0.15f;
				num10 = (float)Steepness * 0.15f;
				num11 = (float)Steepness * 0.15f;
				break;
			case 12:
				num11 = (float)Steepness * 0.15f;
				num12 = (float)Steepness * 0.15f;
				num9 = (float)Steepness * 0.15f;
				break;
			case 10:
				num10 = (float)Steepness * 0.15f;
				num9 = (float)Steepness * 0.15f;
				num12 = (float)Steepness * 0.15f;
				break;
			case 13:
				num12 = (float)Steepness * 0.15f;
				num11 = (float)Steepness * 0.15f;
				num10 = (float)Steepness * 0.15f;
				break;
			case 17:
				num9 = (float)Steepness * 0.15f;
				break;
			case 14:
				num11 = (float)Steepness * 0.15f;
				break;
			case 16:
				num10 = (float)Steepness * 0.15f;
				break;
			case 15:
				num12 = (float)Steepness * 0.15f;
				break;
			}
		}
		if (Floor == 0)
		{
			switch (SlopeDir)
			{
			case 6:
				num5 = (0f - (float)Steepness) * 0.15f;
				break;
			case 7:
				num6 = (0f - (float)Steepness) * 0.15f;
				break;
			case 8:
				num7 = (0f - (float)Steepness) * 0.15f;
				break;
			case 9:
				num8 = (0f - (float)Steepness) * 0.15f;
				break;
			case 11:
				num16 = (0f - (float)Steepness) * 0.15f;
				break;
			case 12:
				num14 = (0f - (float)Steepness) * 0.15f;
				break;
			case 10:
				num15 = (0f - (float)Steepness) * 0.15f;
				break;
			case 13:
				num13 = (0f - (float)Steepness) * 0.15f;
				break;
			case 17:
				num14 = (0f - (float)Steepness) * 0.15f;
				num15 = (0f - (float)Steepness) * 0.15f;
				num16 = (0f - (float)Steepness) * 0.15f;
				break;
			case 14:
				num14 = (0f - (float)Steepness) * 0.15f;
				num16 = (0f - (float)Steepness) * 0.15f;
				num13 = (0f - (float)Steepness) * 0.15f;
				break;
			case 16:
				num16 = (0f - (float)Steepness) * 0.15f;
				num15 = (0f - (float)Steepness) * 0.15f;
				num13 = (0f - (float)Steepness) * 0.15f;
				break;
			case 15:
				num14 = (0f - (float)Steepness) * 0.15f;
				num15 = (0f - (float)Steepness) * 0.15f;
				num13 = (0f - (float)Steepness) * 0.15f;
				break;
			}
		}
		int num17 = 0;
		int num18 = 0;
		int num19 = 0;
		for (int i = 0; i < 6; i++)
		{
			if (t.VisibleFaces[i])
			{
				num17++;
				if (((SlopeDir == 6 || SlopeDir == 7) && (i == 3 || i == 1)) || ((SlopeDir == 8 || SlopeDir == 9) && (i == 4 || i == 5)))
				{
					num18++;
				}
			}
		}
		Material[] array = new Material[num17 + num18];
		Vector3[] array2 = new Vector3[num17 * 4 + num18 * 3];
		Vector2[] array3 = new Vector2[num17 * 4 + num18 * 3];
		float num20 = 0f;
		float num21 = (float)Top * 0.15f;
		float num22 = (float)Bottom * 0.15f;
		float num23 = 0f;
		float num24 = t.DimX;
		float num25 = t.DimY;
		GameObject gameObject = new GameObject(TileName);
		SetTileLayer(t, gameObject);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = new Vector3((float)x * 1.2f, 0f, (float)y * 1.2f);
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num17 + num18;
		int num26 = 0;
		float uv = 0f;
		float uv2 = 0f;
		CalcUV(Top, Bottom, out uv, out uv2);
		float uv3 = 0f;
		float uv4 = 1f;
		if (Floor == 1)
		{
			CalcUV(Top + Steepness, Bottom, out uv3, out uv4);
			num23 = num21;
		}
		else
		{
			CalcUV(Top, Bottom - Steepness, out uv3, out uv4);
			num23 = num22;
		}
		for (int j = 0; j < 6; j++)
		{
			if (!t.VisibleFaces[j])
			{
				continue;
			}
			switch (j)
			{
			case 0:
				array[num26] = GameWorldController.instance.MaterialMasterList[FloorTexture(128, t)];
				switch (SlopeDir)
				{
				case 10:
				case 12:
				case 14:
				case 16:
					array2[3 + 4 * num26] = new Vector3(0f, 0f, num21 + num4 + num2 + num12);
					array2[4 * num26] = new Vector3(0f, 1.2f * num25, num21 + num4 + num + num10);
					array2[1 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num21 + num + num3 + num9);
					array2[2 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num21 + num2 + num3 + num11);
					break;
				default:
					array2[4 * num26] = new Vector3(0f, 0f, num21 + num4 + num2 + num12);
					array2[1 + 4 * num26] = new Vector3(0f, 1.2f * num25, num21 + num4 + num + num10);
					array2[2 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num21 + num + num3 + num9);
					array2[3 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num21 + num2 + num3 + num11);
					break;
				}
				array3[4 * num26] = new Vector2(0f, 1f * num25);
				array3[1 + 4 * num26] = new Vector2(0f, 0f);
				array3[2 + 4 * num26] = new Vector2(1f * num24, 0f);
				array3[3 + 4 * num26] = new Vector2(1f * num24, 1f * num25);
				break;
			case 4:
			{
				num20 = CalcCeilOffset(32, t);
				array[num26] = GameWorldController.instance.MaterialMasterList[WallTexture(32, t)];
				int num37 = SlopeDir;
				if (Floor == 0 && SlopeDir == 7)
				{
					SlopeDir = 6;
				}
				if (SlopeDir == 6)
				{
					array2[4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num22 + num6 + num8);
					array2[1 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num21 + num + num3);
					array2[2 + 4 * num26] = new Vector3(0f, 1.2f * num25, num21 + num + num4);
					array2[3 + 4 * num26] = new Vector3(0f, 1.2f * num25, num22 + num6 + num7);
					array3[4 * num26] = new Vector2(0f, uv3 - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv4 - num20);
					array3[2 + 4 * num26] = new Vector2(num24, uv4 - num20);
					array3[3 + 4 * num26] = new Vector2(num24, uv3 - num20);
				}
				else
				{
					array2[4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num22);
					array2[1 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num21 + num9);
					array2[2 + 4 * num26] = new Vector3(0f, 1.2f * num25, num21 + num10);
					array2[3 + 4 * num26] = new Vector3(0f, 1.2f * num25, num22);
					array3[4 * num26] = new Vector2(0f, uv - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv2 - num20);
					array3[2 + 4 * num26] = new Vector2(num24, uv2 - num20);
					array3[3 + 4 * num26] = new Vector2(num24, uv - num20);
				}
				if (SlopeDir == 8 || SlopeDir == 9)
				{
					int num38 = array3.GetUpperBound(0) - (num18 - num19) * 3 + 1;
					array[array.GetUpperBound(0) - num18 + num19 + 1] = array[num26];
					int num39 = SlopeDir;
					if (Floor == 0)
					{
						SlopeDir = ((SlopeDir != 8) ? 8 : 9);
					}
					switch (SlopeDir)
					{
					case 8:
					{
						array2[num38] = new Vector3(-1.2f * num24, 1.2f * num25, num23 + num6 + num8);
						array2[num38 + 1] = new Vector3(-1.2f * num24, 1.2f * num25, num23 + num + num3);
						array2[num38 + 2] = new Vector3(0f, 1.2f * num25, num23 + num + num4);
						float uv15 = 0f;
						float uv16 = 0f;
						float num41 = 0f;
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv15, out uv16);
							num41 = ((num20 != 0f) ? (uv15 - num20) : uv15);
							array3[num38] = new Vector2(0f, num41);
							array3[num38 + 1] = new Vector2(0f, num41 + (float)Steepness * 0.125f);
							array3[num38 + 2] = new Vector2(1f, num41);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv15, out uv16);
							num41 = ((num20 != 0f) ? (uv15 - num20) : uv15);
							array3[num38] = new Vector2(0f, num41);
							array3[num38 + 1] = new Vector2(0f, num41 + (float)Steepness * 0.125f);
							array3[num38 + 2] = new Vector2(1f, num41 + (float)Steepness * 0.125f);
						}
						break;
					}
					case 9:
					{
						array2[num38] = new Vector3(-1.2f * num24, 1.2f * num25, num23 + num + num3);
						array2[num38 + 1] = new Vector3(0f, 1.2f * num25, num23 + num + num4);
						array2[num38 + 2] = new Vector3(0f, 1.2f * num25, num23 + num6 + num7);
						float uv13 = 0f;
						float uv14 = 0f;
						float num40 = 0f;
						if (num20 == 0f)
						{
							num40 = uv13;
						}
						else
						{
							num40 = uv13 - num20;
						}
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv13, out uv14);
							num40 = ((num20 != 0f) ? (uv13 - num20) : uv13);
							array3[num38] = new Vector2(0f, num40);
							array3[num38 + 1] = new Vector2(1f, num40 + (float)Steepness * 0.125f);
							array3[num38 + 2] = new Vector2(1f, num40);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv13, out uv14);
							num40 = ((num20 != 0f) ? (uv13 - num20) : uv13);
							array3[num38] = new Vector2(0f, num40 + (float)Steepness * 0.125f);
							array3[num38 + 1] = new Vector2(1f, num40 + (float)Steepness * 0.125f);
							array3[num38 + 2] = new Vector2(1f, num40);
						}
						break;
					}
					}
					SlopeDir = num39;
					num19++;
				}
				SlopeDir = num37;
				break;
			}
			case 5:
			{
				num20 = CalcCeilOffset(16, t);
				array[num26] = GameWorldController.instance.MaterialMasterList[WallTexture(16, t)];
				int num42 = SlopeDir;
				if (Floor == 0 && SlopeDir == 6)
				{
					SlopeDir = 7;
				}
				if (SlopeDir == 7)
				{
					array2[4 * num26] = new Vector3(0f, 0f, num22 + num5 + num7);
					array2[1 + 4 * num26] = new Vector3(0f, 0f, num21 + num2 + num4);
					array2[2 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num21 + num2 + num3);
					array2[3 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num22 + num5 + num8);
					array3[4 * num26] = new Vector2(0f, uv3 - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv4 - num20);
					array3[2 + 4 * num26] = new Vector2(num24, uv4 - num20);
					array3[3 + 4 * num26] = new Vector2(num24, uv3 - num20);
				}
				else
				{
					array2[4 * num26] = new Vector3(0f, 0f, num22);
					array2[1 + 4 * num26] = new Vector3(0f, 0f, num21 + num12);
					array2[2 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num21 + num11);
					array2[3 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num22);
					array3[4 * num26] = new Vector2(0f, uv - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv2 - num20);
					array3[2 + 4 * num26] = new Vector2(num24, uv2 - num20);
					array3[3 + 4 * num26] = new Vector2(num24, uv - num20);
				}
				if (SlopeDir == 8 || SlopeDir == 9)
				{
					int num43 = SlopeDir;
					if (Floor == 0)
					{
						SlopeDir = ((SlopeDir != 8) ? 8 : 9);
					}
					int num44 = array3.GetUpperBound(0) - (num18 - num19) * 3 + 1;
					array[array.GetUpperBound(0) - num18 + num19 + 1] = array[num26];
					switch (SlopeDir)
					{
					case 9:
					{
						array2[num44] = new Vector3(0f, 0f, num23 + num5 + num7);
						array2[num44 + 1] = new Vector3(0f, 0f, num23 + num2 + num4);
						array2[num44 + 2] = new Vector3(-1.2f * num24, 0f, num23 + num2 + num3);
						float uv19 = 0f;
						float uv20 = 0f;
						float num46 = 0f;
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv19, out uv20);
							num46 = ((num20 != 0f) ? (uv19 - num20) : uv19);
							array3[num44] = new Vector2(0f, num46);
							array3[num44 + 1] = new Vector2(0f, num46 + (float)Steepness * 0.125f);
							array3[num44 + 2] = new Vector2(1f, num46);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv19, out uv20);
							num46 = ((num20 != 0f) ? (uv19 - num20) : uv19);
							array3[num44] = new Vector2(0f, num46);
							array3[num44 + 1] = new Vector2(0f, num46 + (float)Steepness * 0.125f);
							array3[num44 + 2] = new Vector2(1f, num46 + (float)Steepness * 0.125f);
						}
						break;
					}
					case 8:
					{
						array2[num44] = new Vector3(0f, 0f, num23 + num5 + num7);
						array2[num44 + 1] = new Vector3(-1.2f * num24, 0f, num23 + num2 + num3);
						array2[num44 + 2] = new Vector3(-1.2f * num24, 0f, num23 + num5 + num8);
						float uv17 = 0f;
						float uv18 = 0f;
						float num45 = 0f;
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv17, out uv18);
							num45 = ((num20 != 0f) ? (uv17 - num20) : uv17);
							array3[num44] = new Vector2(0f, num45);
							array3[num44 + 1] = new Vector2(1f, num45 + (float)Steepness * 0.125f);
							array3[num44 + 2] = new Vector2(1f, num45);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv17, out uv18);
							num45 = ((num20 != 0f) ? (uv17 - num20) : uv17);
							array3[num44] = new Vector2(0f, num45 + (float)Steepness * 0.125f);
							array3[num44 + 1] = new Vector2(1f, num45 + (float)Steepness * 0.125f);
							array3[num44 + 2] = new Vector2(1f, num45);
						}
						break;
					}
					}
					SlopeDir = num43;
					num19++;
				}
				SlopeDir = num42;
				break;
			}
			case 3:
			{
				num20 = CalcCeilOffset(4, t);
				array[num26] = GameWorldController.instance.MaterialMasterList[WallTexture(4, t)];
				int num27 = SlopeDir;
				if (Floor == 0 && SlopeDir == 8)
				{
					SlopeDir = 9;
				}
				if (SlopeDir == 9)
				{
					array2[4 * num26] = new Vector3(0f, 1.2f * num25, num22 + num7 + num6);
					array2[1 + 4 * num26] = new Vector3(0f, 1.2f * num25, num21 + num4 + num);
					array2[2 + 4 * num26] = new Vector3(0f, 0f, num21 + num4 + num2);
					array2[3 + 4 * num26] = new Vector3(0f, 0f, num22 + num7 + num5);
					array3[4 * num26] = new Vector2(0f, uv3 - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv4 - num20);
					array3[2 + 4 * num26] = new Vector2(num24, uv4 - num20);
					array3[3 + 4 * num26] = new Vector2(num24, uv3 - num20);
				}
				else
				{
					array2[4 * num26] = new Vector3(0f, 1.2f * num25, num22);
					array2[1 + 4 * num26] = new Vector3(0f, 1.2f * num25, num21 + num10);
					array2[2 + 4 * num26] = new Vector3(0f, 0f, num21 + num12);
					array2[3 + 4 * num26] = new Vector3(0f, 0f, num22);
					array3[4 * num26] = new Vector2(0f, uv - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv2 - num20);
					array3[2 + 4 * num26] = new Vector2(num25, uv2 - num20);
					array3[3 + 4 * num26] = new Vector2(num25, uv - num20);
				}
				if (SlopeDir == 6 || SlopeDir == 7)
				{
					array[array.GetUpperBound(0) - num18 + num19 + 1] = array[num26];
					int num28 = array3.GetUpperBound(0) - (num18 - num19) * 3 + 1;
					int num29 = SlopeDir;
					if (Floor == 0)
					{
						SlopeDir = ((SlopeDir != 6) ? 6 : 7);
					}
					switch (SlopeDir)
					{
					case 6:
					{
						array2[num28] = new Vector3(0f, 1.2f * num25, num23 + num7 + num6);
						array2[num28 + 1] = new Vector3(0f, 1.2f * num25, num23 + num4 + num);
						array2[num28 + 2] = new Vector3(0f, 0f, num23 + num4 + num2);
						float uv7 = 0f;
						float uv8 = 0f;
						float num31 = 0f;
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv7, out uv8);
							num31 = ((num20 != 0f) ? (uv7 - num20) : uv7);
							array3[num28] = new Vector2(0f, num31);
							array3[num28 + 1] = new Vector2(0f, num31 + (float)Steepness * 0.125f);
							array3[num28 + 2] = new Vector2(1f, num31);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv7, out uv8);
							num31 = ((num20 != 0f) ? (uv7 - num20) : uv7);
							array3[num28] = new Vector2(0f, num31);
							array3[num28 + 1] = new Vector2(0f, num31 + (float)Steepness * 0.125f);
							array3[num28 + 2] = new Vector2(1f, num31 + (float)Steepness * 0.125f);
						}
						break;
					}
					case 7:
					{
						array2[num28] = new Vector3(0f, 1.2f * num25, num23 + num4 + num);
						array2[num28 + 1] = new Vector3(0f, 0f, num23 + num4 + num2);
						array2[num28 + 2] = new Vector3(0f, 0f, num23 + num7 + num5);
						float uv5 = 0f;
						float uv6 = 0f;
						float num30 = 0f;
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv5, out uv6);
							num30 = ((num20 != 0f) ? (uv5 - num20) : uv5);
							array3[num28] = new Vector2(0f, num30);
							array3[num28 + 1] = new Vector2(1f, num30 + (float)Steepness * 0.125f);
							array3[num28 + 2] = new Vector2(1f, num30);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv5, out uv6);
							num30 = ((num20 != 0f) ? (uv5 - num20) : uv5);
							array3[num28] = new Vector2(0f, num30 + (float)Steepness * 0.125f);
							array3[num28 + 1] = new Vector2(1f, num30 + (float)Steepness * 0.125f);
							array3[num28 + 2] = new Vector2(1f, num30);
						}
						break;
					}
					}
					SlopeDir = num29;
					num19++;
				}
				SlopeDir = num27;
				break;
			}
			case 1:
			{
				num20 = CalcCeilOffset(8, t);
				array[num26] = GameWorldController.instance.MaterialMasterList[WallTexture(8, t)];
				int num32 = SlopeDir;
				if (Floor == 0 && SlopeDir == 9)
				{
					SlopeDir = 8;
				}
				if (SlopeDir == 8)
				{
					array2[4 * num26] = new Vector3(-1.2f * num24, 0f, num22 + num8 + num5);
					array2[1 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num21 + num3 + num2);
					array2[2 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num21 + num3 + num);
					array2[3 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num22 + num8 + num6);
					array3[4 * num26] = new Vector2(0f, uv3 - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv4 - num20);
					array3[2 + 4 * num26] = new Vector2(num24, uv4 - num20);
					array3[3 + 4 * num26] = new Vector2(num24, uv3 - num20);
				}
				else
				{
					array2[4 * num26] = new Vector3(-1.2f * num24, 0f, num22);
					array2[1 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num21 + num11);
					array2[2 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num21 + num9);
					array2[3 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num22);
					array3[4 * num26] = new Vector2(0f, uv - num20);
					array3[1 + 4 * num26] = new Vector2(0f, uv2 - num20);
					array3[2 + 4 * num26] = new Vector2(num25, uv2 - num20);
					array3[3 + 4 * num26] = new Vector2(num25, uv - num20);
				}
				if (SlopeDir == 6 || SlopeDir == 7)
				{
					array[array.GetUpperBound(0) - num18 + num19 + 1] = array[num26];
					int num33 = array3.GetUpperBound(0) - (num18 - num19) * 3 + 1;
					int num34 = SlopeDir;
					if (Floor == 0)
					{
						SlopeDir = ((SlopeDir != 6) ? 6 : 7);
					}
					switch (SlopeDir)
					{
					case 7:
					{
						array2[num33] = new Vector3(-1.2f * num24, 0f, num23 + num8 + num5);
						array2[num33 + 1] = new Vector3(-1.2f * num24, 0f, num23 + num3 + num2);
						array2[num33 + 2] = new Vector3(-1.2f * num24, 1.2f * num25, num23 + num3 + num);
						float uv11 = 0f;
						float uv12 = 0f;
						float num36 = 0f;
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv11, out uv12);
							num36 = ((num20 != 0f) ? (uv11 - num20) : uv11);
							array3[num33] = new Vector2(0f, num36);
							array3[num33 + 1] = new Vector2(0f, num36 + (float)Steepness * 0.125f);
							array3[num33 + 2] = new Vector2(1f, num36);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv11, out uv12);
							num36 = ((num20 != 0f) ? (uv11 - num20) : uv11);
							array3[num33] = new Vector2(0f, num36);
							array3[num33 + 1] = new Vector2(0f, num36 + (float)Steepness * 0.125f);
							array3[num33 + 2] = new Vector2(1f, num36 + (float)Steepness * 0.125f);
						}
						break;
					}
					case 6:
					{
						array2[num33] = new Vector3(-1.2f * num24, 0f, num23 + num3 + num2);
						array2[num33 + 1] = new Vector3(-1.2f * num24, 1.2f * num25, num23 + num3 + num);
						array2[num33 + 2] = new Vector3(-1.2f * num24, 1.2f * num25, num23 + num8 + num6);
						float uv9 = 0f;
						float uv10 = 0f;
						float num35 = 0f;
						if (Floor == 1)
						{
							CalcUV(Top + Steepness, Top, out uv9, out uv10);
							num35 = ((num20 != 0f) ? (uv9 - num20) : uv9);
							array3[num33] = new Vector2(0f, num35);
							array3[num33 + 1] = new Vector2(1f, num35 + (float)Steepness * 0.125f);
							array3[num33 + 2] = new Vector2(1f, num35);
						}
						else
						{
							CalcUV(Bottom, Bottom - Steepness, out uv9, out uv10);
							num35 = ((num20 != 0f) ? (uv9 - num20) : uv9);
							array3[num33] = new Vector2(0f, num35 + (float)Steepness * 0.125f);
							array3[num33 + 1] = new Vector2(1f, num35 + (float)Steepness * 0.125f);
							array3[num33 + 2] = new Vector2(1f, num35);
						}
						break;
					}
					}
					SlopeDir = num34;
					num19++;
				}
				SlopeDir = num32;
				break;
			}
			case 2:
				array[num26] = GameWorldController.instance.MaterialMasterList[FloorTexture(64, t)];
				switch (SlopeDir)
				{
				case 11:
				case 13:
				case 15:
				case 17:
					array2[1 + 4 * num26] = new Vector3(0f, 1.2f * num25, num22 + num6 + num7 + num15);
					array2[2 + 4 * num26] = new Vector3(0f, 0f, num22 + num7 + num5 + num13);
					array2[3 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num22 + num5 + num8 + num14);
					array2[4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num22 + num6 + num8 + num16);
					break;
				default:
					array2[4 * num26] = new Vector3(0f, 1.2f * num25, num22 + num6 + num7 + num15);
					array2[1 + 4 * num26] = new Vector3(0f, 0f, num22 + num7 + num5 + num13);
					array2[2 + 4 * num26] = new Vector3(-1.2f * num24, 0f, num22 + num5 + num8 + num14);
					array2[3 + 4 * num26] = new Vector3(-1.2f * num24, 1.2f * num25, num22 + num6 + num8 + num16);
					break;
				}
				array3[4 * num26] = new Vector2(0f, 0f);
				array3[1 + 4 * num26] = new Vector2(0f, 1f * num25);
				array3[2 + 4 * num26] = new Vector2(num24, 1f * num25);
				array3[3 + 4 * num26] = new Vector2(num24, 0f);
				break;
			}
			num26++;
		}
		mesh.vertices = array2;
		mesh.uv = array3;
		num26 = 0;
		int[] array4 = new int[6];
		int num47 = 0;
		for (int k = 0; k < 6; k++)
		{
			if (t.VisibleFaces[k])
			{
				array4[0] = 4 * num26;
				array4[1] = 1 + 4 * num26;
				array4[2] = 2 + 4 * num26;
				array4[3] = 4 * num26;
				array4[4] = 2 + 4 * num26;
				array4[5] = 3 + 4 * num26;
				num47 = 3 + 4 * num26;
				mesh.SetTriangles(array4, num26);
				num26++;
			}
		}
		array4 = new int[3];
		num19 = 0;
		num47++;
		for (int l = 0; l < 6; l++)
		{
			if (t.VisibleFaces[l] && (((SlopeDir == 6 || SlopeDir == 7) && (l == 3 || l == 1)) || ((SlopeDir == 8 || SlopeDir == 9) && (l == 4 || l == 5))))
			{
				array4[0] = num47 + 3 * num19;
				array4[1] = 1 + num47 + 3 * num19;
				array4[2] = 2 + num47 + 3 * num19;
				mesh.SetTriangles(array4, num26 + num19);
				num19++;
			}
		}
		meshRenderer.materials = array;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		if (EnableCollision)
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
	}

	public static void UpdateTile(int TileX, int TileY, short NewTileType, short NewFloorHeight, short NewFloorTexture, short NewWallTexture, bool RenderImmediate)
	{
		bool flag = false;
		if (RenderImmediate)
		{
			DestroyTile(TileX, TileY);
		}
		switch (NewTileType)
		{
		case 6:
		case 7:
		case 8:
		case 9:
			UWClass.CurrentTileMap().Tiles[TileX, TileY].shockSteep = 1;
			break;
		}
		UWClass.CurrentTileMap().Tiles[TileX, TileY].tileType = NewTileType;
		UWClass.CurrentTileMap().Tiles[TileX, TileY].floorHeight = NewFloorHeight;
		UWClass.CurrentTileMap().Tiles[TileX, TileY].floorTexture = NewFloorTexture;
		if (UWClass.CurrentTileMap().Tiles[TileX, TileY].wallTexture != NewWallTexture)
		{
			if (UWClass.CurrentTileMap().Tiles[TileX, TileY].tileType == 0)
			{
				UWClass.CurrentTileMap().Tiles[TileX, TileY].North = NewWallTexture;
				UWClass.CurrentTileMap().Tiles[TileX, TileY].South = NewWallTexture;
				UWClass.CurrentTileMap().Tiles[TileX, TileY].East = NewWallTexture;
				UWClass.CurrentTileMap().Tiles[TileX, TileY].West = NewWallTexture;
			}
			UWClass.CurrentTileMap().Tiles[TileX, TileY].wallTexture = NewWallTexture;
			if (TileY > 0)
			{
				UWClass.CurrentTileMap().Tiles[TileX, TileY - 1].North = NewWallTexture;
				flag = true;
			}
			if (TileY < 63)
			{
				UWClass.CurrentTileMap().Tiles[TileX, TileY + 1].South = NewWallTexture;
				flag = true;
			}
			if (TileX > 0)
			{
				UWClass.CurrentTileMap().Tiles[TileX - 1, TileY].East = NewWallTexture;
				flag = true;
			}
			if (TileY < 63)
			{
				UWClass.CurrentTileMap().Tiles[TileX + 1, TileY].West = NewWallTexture;
				flag = true;
			}
		}
		if (RenderImmediate)
		{
			RenderTile(GameWorldController.instance.LevelModel, TileX, TileY, UWClass.CurrentTileMap().Tiles[TileX, TileY], false, false, false, true);
		}
		if (!flag)
		{
			return;
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if ((i != 0 || j != 0) && i + TileX <= 63 && i + TileX >= 0 && j + TileY <= 63 && j + TileY >= 0 && RenderImmediate)
				{
					DestroyTile(i + TileX, j + TileY);
					RenderTile(GameWorldController.instance.LevelModel, TileX + i, TileY + j, UWClass.CurrentTileMap().Tiles[TileX + i, TileY + j], false, false, false, true);
				}
			}
		}
	}

	public static void DestroyTile(int x, int y)
	{
		switch (UWClass.CurrentTileMap().Tiles[x, y].tileType)
		{
		case 0:
		{
			GameObject gameObject = GameWorldController.FindTile(x, y, 1);
			if (gameObject != null)
			{
				gameObject.gameObject.transform.position = GameWorldController.instance.InventoryMarker.transform.position;
				gameObject.name += "_destroyed";
				Object.DestroyImmediate(gameObject);
			}
			break;
		}
		case 2:
		case 3:
		case 4:
		case 5:
		{
			GameObject gameObject = GameWorldController.FindTile(x, y, 1);
			if (gameObject != null)
			{
				gameObject.gameObject.transform.position = GameWorldController.instance.InventoryMarker.transform.position;
				gameObject.name += "_destroyed";
				Object.DestroyImmediate(gameObject);
			}
			gameObject = GameWorldController.FindTile(x, y, 3);
			if (gameObject != null)
			{
				gameObject.gameObject.transform.position = GameWorldController.instance.InventoryMarker.transform.position;
				gameObject.name += "_destroyed";
				Object.DestroyImmediate(gameObject);
			}
			break;
		}
		default:
		{
			GameObject gameObject = GameWorldController.FindTile(x, y, 1);
			if (gameObject != null)
			{
				gameObject.gameObject.transform.position = GameWorldController.instance.InventoryMarker.transform.position;
				gameObject.name += "_destroyed";
				Object.DestroyImmediate(gameObject);
			}
			break;
		}
		}
	}

	private static void SetTileLayer(TileInfo t, GameObject Tile)
	{
		if (!t.isWater)
		{
			if (t.isLava)
			{
				Tile.layer = LayerMask.NameToLayer("Lava");
				AddLavaContact(Tile);
			}
			else if (t.isNothing)
			{
				Tile.layer = LayerMask.NameToLayer("Nothing");
			}
			else if (t.isIce)
			{
				Tile.layer = LayerMask.NameToLayer("Ice");
			}
			else
			{
				Tile.layer = LayerMask.NameToLayer("MapMesh");
			}
		}
		else
		{
			Tile.layer = LayerMask.NameToLayer("Water");
			if (t.tileType != 0)
			{
				AddWaterContact(Tile, t);
			}
		}
	}

	public static bool RenderTNovaMap(Transform parent, char[] data)
	{
		short[,] array = new short[513, 513];
		short[,] array2 = new short[513, 513];
		short[,] array3 = new short[513, 513];
		short[] array4 = new short[64];
		short[] array5 = new short[64];
		float num = 12f;
		long num2 = 0L;
		num2 = 0L;
		int num3 = 1;
		short num4 = 0;
		short num5 = 0;
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= array.GetUpperBound(1); j++)
			{
				num3++;
				int num6 = (int)DataLoader.getValAtAddress(data, num2++, 8);
				int num7 = (int)DataLoader.getValAtAddress(data, num2++, 8);
				int num8 = (int)DataLoader.getValAtAddress(data, num2++, 8);
				if (num6 > 191)
				{
					num6 -= 64;
				}
				if (num6 > 127)
				{
					num6 -= 128;
				}
				if (num6 > 63)
				{
					num6 -= 64;
				}
				array2[i, j] = (short)num6;
				array3[i, j] = (short)((num7 & 0xF) >> 2);
				array[i, j] = (short)((num8 << 4) | ((num7 & 0xF0) >> 4));
				if (num8 > 127)
				{
					array[i, j] -= 4096;
				}
				if (i == 0 && j == 0)
				{
					num4 = array[i, j];
					num5 = array[i, j];
				}
				if (array[i, j] > num4)
				{
					num4 = array[i, j];
				}
				if (array[i, j] < num5)
				{
					num5 = array[i, j];
				}
			}
		}
		for (int k = 0; k < 8; k++)
		{
			for (int l = 0; l < 8; l++)
			{
				for (int m = k * 64; m < (k + 1) * 64; m++)
				{
					for (int n = l * 64; n < (l + 1) * 64; n++)
					{
						array4[array2[m, n]]++;
					}
				}
				short num9 = 0;
				for (short num10 = 0; num10 <= array5.GetUpperBound(0); num10++)
				{
					if (array4[num10] != 0)
					{
						array5[num10] = num9++;
					}
				}
				Material[] array6 = new Material[num9];
				int num11 = 0;
				for (short num12 = 0; num12 <= array5.GetUpperBound(0); num12++)
				{
					if (array4[num12] != 0)
					{
						array6[num11++] = (Material)Resources.Load("Nova/Materials/nova" + num12);
					}
				}
				GameObject gameObject = new GameObject("TNOVAMAP_" + k + "_" + l);
				gameObject.transform.parent = parent;
				gameObject.transform.position = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				Mesh mesh = new Mesh();
				num3 = 4096;
				mesh.subMeshCount = num9;
				Vector3[] array7 = new Vector3[num3 * 4];
				Vector2[] array8 = new Vector2[num3 * 4];
				int num13 = 0;
				for (int num14 = k * 64; num14 < (k + 1) * 64; num14++)
				{
					for (int num15 = l * 64; num15 < (l + 1) * 64; num15++)
					{
						float[] array9 = new float[4]
						{
							-array[num14, num15],
							-array[num14, num15 + 1],
							-array[num14 + 1, num15 + 1],
							-array[num14 + 1, num15]
						};
						float num16 = (float)num14 * num;
						float num17 = (float)num15 * num;
						array7[4 * num13] = new Vector3(num16, num17, array9[0]);
						array7[1 + 4 * num13] = new Vector3(num16, num17 + num, array9[1]);
						array7[2 + 4 * num13] = new Vector3(num16 + num, num17 + num, array9[2]);
						array7[3 + 4 * num13] = new Vector3(num16 + num, num17, array9[3]);
						switch (array3[num14, num15])
						{
						case 1:
							array8[1 + 4 * num13] = new Vector2(0f, 1f);
							array8[2 + 4 * num13] = new Vector2(1f, 1f);
							array8[3 + 4 * num13] = new Vector2(1f, 0f);
							array8[4 * num13] = new Vector2(0f, 0f);
							break;
						case 2:
							array8[4 * num13] = new Vector2(1f, 0f);
							array8[1 + 4 * num13] = new Vector2(0f, 0f);
							array8[2 + 4 * num13] = new Vector2(0f, 1f);
							array8[3 + 4 * num13] = new Vector2(1f, 1f);
							break;
						case 3:
							array8[3 + 4 * num13] = new Vector2(0f, 1f);
							array8[4 * num13] = new Vector2(1f, 1f);
							array8[1 + 4 * num13] = new Vector2(1f, 0f);
							array8[2 + 4 * num13] = new Vector2(0f, 0f);
							break;
						default:
							array8[4 * num13] = new Vector2(0f, 1f);
							array8[1 + 4 * num13] = new Vector2(1f, 1f);
							array8[2 + 4 * num13] = new Vector2(1f, 0f);
							array8[3 + 4 * num13] = new Vector2(0f, 0f);
							break;
						}
						num13++;
					}
				}
				mesh.vertices = array7;
				mesh.uv = array8;
				for (int num18 = 0; num18 <= array4.GetUpperBound(0); num18++)
				{
					if (array4[num18] <= 0)
					{
						continue;
					}
					int[] array10 = new int[array4[num18] * 6];
					num13 = 0;
					int num19 = 0;
					for (int num20 = k * 64; num20 < (k + 1) * 64; num20++)
					{
						for (int num21 = l * 64; num21 < (l + 1) * 64; num21++)
						{
							if (array2[num20, num21] == num18)
							{
								array10[6 * num19] = 4 * num13;
								array10[1 + 6 * num19] = 1 + 4 * num13;
								array10[2 + 6 * num19] = 2 + 4 * num13;
								array10[3 + 6 * num19] = 4 * num13;
								array10[4 + 6 * num19] = 2 + 4 * num13;
								array10[5 + 6 * num19] = 3 + 4 * num13;
								num19++;
							}
							num13++;
						}
					}
					mesh.SetTriangles(array10, array5[num18]);
				}
				meshRenderer.materials = array6;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				meshFilter.mesh = mesh;
			}
		}
		return true;
	}

	private static void AddWaterContact(GameObject tile, TileInfo t)
	{
		if (UWClass._RES == "UW2" && FloorTexture(2, t) == 193)
		{
			tile.AddComponent<TileContactMud>();
		}
		else
		{
			tile.AddComponent<TileContactWater>();
		}
	}

	private static void AddLavaContact(GameObject tile)
	{
		tile.AddComponent<TileContactLava>();
	}
}
