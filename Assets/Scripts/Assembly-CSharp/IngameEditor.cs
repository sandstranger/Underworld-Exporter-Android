using UnityEngine;
using UnityEngine.UI;

public class IngameEditor : GuiBase_Draggable
{
	public Camera OverheadCam;

	public static int TileX = 0;

	public static int TileY = 0;

	public ObjectLoaderInfo currObj;

	private bool EditorHidden = false;

	public Material UI_UNLIT;

	public RawImage TileMapView;

	public GameObject EditorBackground;

	public InputField LevelNoToLoad;

	public Dropdown TileTypeSelect;

	public Dropdown FloorTextureSelect;

	public Dropdown WallTextureSelect;

	public Dropdown FloorTextureMapSelect;

	public Dropdown WallTextureMapSelect;

	public Dropdown DoorTextureMapSelect;

	public Dropdown ObjectSelect;

	public Dropdown ObjectItemIds;

	public Toggle ObjectFlagisQuant;

	public Toggle ObjectFlaginVis;

	public Toggle ObjectFlagDoorDir;

	public Toggle ObjectFlagEnchant;

	public InputField ObjectFlagValue;

	public InputField ObjectLink;

	public InputField ObjectOwner;

	public InputField ObjectQuality;

	public InputField ObjectNext;

	public InputField ObjectTileX;

	public InputField ObjectTileY;

	public InputField ObjectXPos;

	public InputField ObjectYPos;

	public InputField ObjectZPos;

	public InputField TileRangeX;

	public InputField TileRangeY;

	public Text LevelDetails;

	public Text TileDetails;

	public InputField TileHeightDetails;

	public RectTransform TileMapDetailsPanel;

	public RectTransform ObjectDetailsPanel;

	public RectTransform TextureMapDetailsPanel;

	public RectTransform MobileObjectDetailsPanel;

	public static IngameEditor instance;

	public GridLayoutGroup FloorTextureMapDisplay;

	public GridLayoutGroup WallTextureMapDisplay;

	public GridLayoutGroup DoorTextureMapDisplay;

	public RawImage SelectedTextureMap;

	public Toggle LockTileType;

	public Toggle LockTileHeight;

	public Toggle LockFloorTextures;

	public Toggle LockWallTextures;

	public InputField npc_whoami;

	public InputField npc_xhome;

	public InputField npc_yhome;

	public InputField npc_hp;

	public Dropdown npc_goal;

	public InputField npc_goaltarget;

	public InputField npc_attitude;

	public InputField npc_talkedto;

	public InputField seed;

	public static bool FollowMeMode = false;

	private void Awake()
	{
		instance = this;
	}

	public override void Start()
	{
		base.Start();
		seed.text = UnderworldGenerator.instance.Seed.ToString();
		if (GameWorldController.instance.LevelNo != -1)
		{
			SwitchPanel(0);
			UpdateFloorTexturesDropDown();
			UpdateWallTexturesDropDown();
			UpdateDoorTexturesGrid();
			RefreshTileMap();
			RefreshTileInfo();
			UpdateNPCGoals();
		}
		for (int i = 0; i <= GameWorldController.instance.objectMaster.objProp.GetUpperBound(0); i++)
		{
			ObjectItemIds.options.Add(new Dropdown.OptionData(GameWorldController.instance.objectMaster.objProp[i].desc));
		}
	}

	private void UpdateObjectsDropDown()
	{
		ObjectSelect.ClearOptions();
		ObjectLoader objectLoader = UWEBase.CurrentObjectList();
		for (int i = 0; i <= objectLoader.objInfo.GetUpperBound(0); i++)
		{
			if (objectLoader.objInfo[i] != null)
			{
				string text = ObjectLoader.UniqueObjectNameEditor(UWEBase.CurrentObjectList().objInfo[i]);
				ObjectSelect.options.Add(new Dropdown.OptionData(text));
			}
		}
		ObjectSelect.RefreshShownValue();
	}

	private void UpdateNPCGoals()
	{
		npc_goal.ClearOptions();
		npc_goal.options.Add(new Dropdown.OptionData("0-Stand Still"));
		npc_goal.options.Add(new Dropdown.OptionData("1-Random Movement"));
		npc_goal.options.Add(new Dropdown.OptionData("2-Random Movement"));
		npc_goal.options.Add(new Dropdown.OptionData("3-Follow Target"));
		npc_goal.options.Add(new Dropdown.OptionData("4-Random Movement"));
		npc_goal.options.Add(new Dropdown.OptionData("5-Attack Target"));
		npc_goal.options.Add(new Dropdown.OptionData("6-Flee Target"));
		npc_goal.options.Add(new Dropdown.OptionData("7-Stand Still"));
		npc_goal.options.Add(new Dropdown.OptionData("8-Random Movement"));
		npc_goal.options.Add(new Dropdown.OptionData("9-Attack Target"));
		npc_goal.options.Add(new Dropdown.OptionData("10-Begin Conversation"));
		npc_goal.options.Add(new Dropdown.OptionData("11-Stand Still"));
		npc_goal.options.Add(new Dropdown.OptionData("12-Stand Still"));
		npc_goal.RefreshShownValue();
	}

	private void UpdateFloorTexturesDropDown()
	{
		FloorTextureSelect.ClearOptions();
		FloorTextureMapSelect.ClearOptions();
		foreach (Transform item in FloorTextureMapDisplay.transform)
		{
			Object.Destroy(item.gameObject);
		}
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			for (int i = 0; i < 64; i++)
			{
				int num = UWEBase.CurrentTileMap().texture_map[i];
				string text = num + " " + StringController.instance.GetTextureName(num);
				Texture2D texture2D = GameWorldController.instance.texLoader.LoadImageAt(num);
				Sprite image = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
				FloorTextureSelect.options.Add(new Dropdown.OptionData(text, image));
				CreateTextureMapButton(GameWorldController.instance.texLoader.LoadImageAt(num), i, num, FloorTextureMapDisplay.transform, 0);
			}
			for (int j = 0; j < 256; j++)
			{
				string text2 = j + " " + StringController.instance.GetTextureName(j);
				Texture2D texture2D2 = GameWorldController.instance.texLoader.LoadImageAt(j);
				Sprite image2 = Sprite.Create(texture2D2, new Rect(0f, 0f, texture2D2.width, texture2D2.height), new Vector2(0.5f, 0.5f));
				FloorTextureMapSelect.options.Add(new Dropdown.OptionData(text2, image2));
			}
			FloorTextureMapDisplay.constraintCount = 20;
			FloorTextureMapDisplay.spacing = new Vector2(-18f, -20f);
		}
		else
		{
			for (int k = 48; k <= 57; k++)
			{
				int num2 = UWEBase.CurrentTileMap().texture_map[k];
				string text3 = num2 + " " + StringController.instance.GetTextureName(num2);
				Texture2D texture2D3 = GameWorldController.instance.texLoader.LoadImageAt(num2);
				Sprite image3 = Sprite.Create(texture2D3, new Rect(0f, 0f, texture2D3.width, texture2D3.height), new Vector2(0.5f, 0.5f));
				FloorTextureSelect.options.Add(new Dropdown.OptionData(text3, image3));
				CreateTextureMapButton(GameWorldController.instance.texLoader.LoadImageAt(num2), k, num2, FloorTextureMapDisplay.transform, 0);
			}
			for (int l = 210; l <= 261; l++)
			{
				string text4 = l + " " + StringController.instance.GetTextureName(l);
				Texture2D texture2D4 = GameWorldController.instance.texLoader.LoadImageAt(l);
				Sprite image4 = Sprite.Create(texture2D4, new Rect(0f, 0f, texture2D4.width, texture2D4.height), new Vector2(0.5f, 0.5f));
				FloorTextureMapSelect.options.Add(new Dropdown.OptionData(text4, image4));
			}
		}
		FloorTextureSelect.RefreshShownValue();
		FloorTextureMapSelect.RefreshShownValue();
	}

	private void UpdateWallTexturesDropDown()
	{
		WallTextureSelect.ClearOptions();
		WallTextureMapSelect.ClearOptions();
		foreach (Transform item in WallTextureMapDisplay.transform)
		{
			Object.Destroy(item.gameObject);
		}
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			for (int i = 0; i < 64; i++)
			{
				int num = UWEBase.CurrentTileMap().texture_map[i];
				string text = num + " " + StringController.instance.GetTextureName(num);
				Texture2D texture2D = GameWorldController.instance.texLoader.LoadImageAt(num);
				Sprite image = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
				WallTextureSelect.options.Add(new Dropdown.OptionData(text, image));
				CreateTextureMapButton(GameWorldController.instance.texLoader.LoadImageAt(num), i, num, WallTextureMapDisplay.transform, 1);
			}
			for (int j = 0; j < 256; j++)
			{
				string text2 = j + " " + StringController.instance.GetTextureName(j);
				Texture2D texture2D2 = GameWorldController.instance.texLoader.LoadImageAt(j);
				Sprite image2 = Sprite.Create(texture2D2, new Rect(0f, 0f, texture2D2.width, texture2D2.height), new Vector2(0.5f, 0.5f));
				WallTextureMapSelect.options.Add(new Dropdown.OptionData(text2, image2));
			}
			WallTextureMapDisplay.constraintCount = 20;
			WallTextureMapDisplay.spacing = new Vector2(-18f, -20f);
		}
		else
		{
			for (int k = 0; k <= 47; k++)
			{
				int num2 = UWEBase.CurrentTileMap().texture_map[k];
				string text3 = num2 + " " + StringController.instance.GetTextureName(num2);
				Texture2D texture2D3 = GameWorldController.instance.texLoader.LoadImageAt(num2);
				Sprite image3 = Sprite.Create(texture2D3, new Rect(0f, 0f, texture2D3.width, texture2D3.height), new Vector2(0.5f, 0.5f));
				WallTextureSelect.options.Add(new Dropdown.OptionData(text3, image3));
				CreateTextureMapButton(GameWorldController.instance.texLoader.LoadImageAt(num2), k, num2, WallTextureMapDisplay.transform, 1);
			}
			for (int l = 0; l < 210; l++)
			{
				string text4 = l + " " + StringController.instance.GetTextureName(l);
				Texture2D texture2D4 = GameWorldController.instance.texLoader.LoadImageAt(l);
				Sprite image4 = Sprite.Create(texture2D4, new Rect(0f, 0f, texture2D4.width, texture2D4.height), new Vector2(0.5f, 0.5f));
				WallTextureMapSelect.options.Add(new Dropdown.OptionData(text4, image4));
			}
		}
		WallTextureSelect.RefreshShownValue();
		WallTextureMapSelect.RefreshShownValue();
	}

	private void UpdateDoorTexturesGrid()
	{
		DoorTextureMapSelect.ClearOptions();
		foreach (Transform item in DoorTextureMapDisplay.transform)
		{
			Object.Destroy(item.gameObject);
		}
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			for (int i = 64; i < 70; i++)
			{
				int num = UWEBase.CurrentTileMap().texture_map[i];
				CreateTextureMapButton(GameWorldController.instance.MaterialDoors[num].mainTexture, i, num, DoorTextureMapDisplay.transform, 2);
			}
			for (int j = 0; j <= GameWorldController.instance.MaterialDoors.GetUpperBound(0); j++)
			{
				DoorTextureMapSelect.options.Add(new Dropdown.OptionData("Door_" + j.ToString("D2")));
			}
		}
		else
		{
			for (int k = 58; k < 64; k++)
			{
				int num2 = UWEBase.CurrentTileMap().texture_map[k];
				CreateTextureMapButton(GameWorldController.instance.MaterialDoors[num2].mainTexture, k, num2, DoorTextureMapDisplay.transform, 2);
			}
			for (int l = 0; l <= GameWorldController.instance.MaterialDoors.GetUpperBound(0); l++)
			{
				DoorTextureMapSelect.options.Add(new Dropdown.OptionData("Door_" + l.ToString("D2")));
			}
		}
		DoorTextureMapSelect.RefreshShownValue();
	}

	private static void CreateTextureMapButton(Texture tex, int index, int textureIndex, Transform parent, short textureType)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Prefabs/_TextureMapButton"));
		gameObject.transform.parent = parent;
		gameObject.GetComponent<TextureMapButton>().MapIndex = index;
		gameObject.GetComponent<RawImage>().texture = tex;
		gameObject.GetComponent<TextureMapButton>().img = gameObject.GetComponent<RawImage>();
		gameObject.GetComponent<TextureMapButton>().textureType = textureType;
		gameObject.GetComponent<TextureMapButton>().TextureIndex = textureIndex;
	}

	public void ChangeLevel()
	{
		int result = 0;
		if (int.TryParse(LevelNoToLoad.text, out result))
		{
			if (result <= GameWorldController.instance.Tilemaps.GetUpperBound(0))
			{
				GameWorldController.instance.SwitchLevel((short)result);
				RefreshTileMap();
				RefreshTileInfo();
				UpdateFloorTexturesDropDown();
				UpdateWallTexturesDropDown();
				UpdateObjectsDropDown();
			}
			else
			{
				UWHUD.instance.MessageScroll.Add("Invalid Level No");
			}
		}
	}

	public void RefreshTileMap()
	{
		AutoMap autoMap = UWEBase.CurrentAutoMap();
		TileMap tileMap = UWEBase.CurrentTileMap();
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				autoMap.MarkTile(i, j, tileMap.Tiles[i, j].tileType, AutoMap.GetDisplayType(tileMap.Tiles[i, j]));
			}
		}
		TileMapView.texture = UWEBase.CurrentAutoMap().TileMapImage();
		LevelDetails.text = "Level + " + GameWorldController.instance.LevelNo;
	}

	public void ChangeTileX(int offset)
	{
		TileX += offset;
		if (TileX > 63)
		{
			TileX = 0;
		}
		if (TileX < 0)
		{
			TileX = 63;
		}
		RefreshTileInfo();
	}

	public void ChangeTileY(int offset)
	{
		TileY += offset;
		if (TileY > 63)
		{
			TileY = 0;
		}
		if (TileY < 0)
		{
			TileY = 63;
		}
		RefreshTileInfo();
	}

	public void RefreshTileInfo()
	{
		TileDetails.text = "X=" + TileX + " Y=" + TileY;
		TileTypeSelect.value = UWEBase.CurrentTileMap().Tiles[TileX, TileY].tileType;
		TileHeightDetails.text = ((float)UWEBase.CurrentTileMap().Tiles[TileX, TileY].floorHeight / 2f).ToString();
		FloorTextureSelect.value = UWEBase.CurrentTileMap().Tiles[TileX, TileY].floorTexture;
		WallTextureSelect.value = UWEBase.CurrentTileMap().Tiles[TileX, TileY].wallTexture;
		RefreshTileMap();
	}

	public void UpdateTile()
	{
		int result = 0;
		int result2 = 0;
		int result3 = 0;
		int wallTextureSelected = WallTextureSelect.value;
		int floorTextureSelected = FloorTextureSelect.value;
		int num = TileTypeSelect.value;
		int.TryParse(TileHeightDetails.text, out result3);
		if (LockTileHeight.isOn)
		{
			result3 = -1;
		}
		if (LockTileType.isOn)
		{
			num = -1;
		}
		if (LockFloorTextures.isOn)
		{
			floorTextureSelected = -1;
		}
		if (LockWallTextures.isOn)
		{
			wallTextureSelected = -1;
		}
		if (!int.TryParse(TileRangeX.text, out result))
		{
			result = 0;
		}
		if (!int.TryParse(TileRangeY.text, out result2))
		{
			result2 = 0;
		}
		if (result == 0 && result2 == 0)
		{
			UpdateTile(TileX, TileY, num, floorTextureSelected, wallTextureSelected, result3);
			return;
		}
		int num2 = Mathf.Min(TileX, TileX + result);
		int num3 = Mathf.Max(TileX, TileX + result);
		int num4 = Mathf.Min(TileY, TileY + result2);
		int num5 = Mathf.Max(TileY, TileY + result2);
		switch (num)
		{
		case -1:
		case 0:
		case 1:
		{
			for (int j = num2; j <= num3; j++)
			{
				for (int k = num4; k <= num5; k++)
				{
					if (TileMap.ValidTile(j, k))
					{
						if (result3 != -1)
						{
							UWEBase.CurrentTileMap().Tiles[j, k].floorHeight = (short)result3;
						}
						if (num == 1 || num == -1)
						{
							UWEBase.CurrentTileMap().Tiles[j, k].VisibleFaces[0] = true;
						}
						else
						{
							UWEBase.CurrentTileMap().Tiles[j, k].VisibleFaces[0] = false;
						}
						UpdateTile(j, k, num, floorTextureSelected, wallTextureSelected, result3);
					}
				}
			}
			break;
		}
		case 8:
		{
			int num15 = result3;
			if (num2 < TileX)
			{
				num15 += Mathf.Abs(result) * -1;
			}
			for (int num16 = num2; num16 <= num3; num16++)
			{
				for (int num17 = num4; num17 <= num5; num17++)
				{
					if (TileMap.ValidTile(num16, num17) && num15 >= 0 && num15 <= 15)
					{
						UWEBase.CurrentTileMap().Tiles[num16, num17].VisibleFaces[0] = true;
						if (result3 != -1)
						{
							UWEBase.CurrentTileMap().Tiles[num16, num17].floorHeight = (short)num15;
						}
						UWEBase.CurrentTileMap().Tiles[num16, num17].shockSteep = 2;
						UpdateTile(num16, num17, num, floorTextureSelected, wallTextureSelected, num15);
					}
				}
				num15++;
			}
			break;
		}
		case 9:
		{
			int num12 = result3;
			if (num2 < TileX)
			{
				num12 += Mathf.Abs(result);
			}
			for (int num13 = num2; num13 <= num3; num13++)
			{
				for (int num14 = num4; num14 <= num5; num14++)
				{
					if (TileMap.ValidTile(num13, num14) && num12 >= 0 && num12 <= 15)
					{
						UWEBase.CurrentTileMap().Tiles[num13, num14].VisibleFaces[0] = true;
						if (result3 != -1)
						{
							UWEBase.CurrentTileMap().Tiles[num13, num14].floorHeight = (short)num12;
						}
						UWEBase.CurrentTileMap().Tiles[num13, num14].shockSteep = 2;
						UpdateTile(num13, num14, num, floorTextureSelected, wallTextureSelected, num12);
					}
				}
				num12--;
			}
			break;
		}
		case 6:
		{
			int num8 = result3;
			if (num4 < TileY)
			{
				num8 += Mathf.Abs(result2) * -1;
			}
			for (int m = num4; m <= num5; m++)
			{
				for (int n = num2; n <= num3; n++)
				{
					if (TileMap.ValidTile(n, m) && num8 >= 0 && num8 <= 15)
					{
						UWEBase.CurrentTileMap().Tiles[n, m].VisibleFaces[0] = true;
						if (result3 != -1)
						{
							UWEBase.CurrentTileMap().Tiles[n, m].floorHeight = (short)num8;
						}
						UWEBase.CurrentTileMap().Tiles[n, m].shockSteep = 2;
						UpdateTile(n, m, num, floorTextureSelected, wallTextureSelected, num8);
					}
				}
				num8++;
			}
			break;
		}
		case 7:
		{
			int num9 = result3;
			if (num2 < TileX)
			{
				num9 += Mathf.Abs(result2);
			}
			for (int num10 = num4; num10 <= num5; num10++)
			{
				for (int num11 = num2; num11 <= num3; num11++)
				{
					if (TileMap.ValidTile(num11, num10) && num9 >= 0 && num9 <= 15)
					{
						UWEBase.CurrentTileMap().Tiles[num11, num10].VisibleFaces[0] = true;
						if (result3 != -1)
						{
							UWEBase.CurrentTileMap().Tiles[num11, num10].floorHeight = (short)num9;
						}
						UWEBase.CurrentTileMap().Tiles[num11, num10].shockSteep = 2;
						UpdateTile(num11, num10, num, floorTextureSelected, wallTextureSelected, num9);
					}
				}
				num9--;
			}
			break;
		}
		case 2:
		case 5:
		{
			int num7 = Mathf.Abs(Mathf.Min(result, result2));
			for (int l = 0; l <= num7; l++)
			{
				if (TileMap.ValidTile(num2 + l, num4 + l))
				{
					UWEBase.CurrentTileMap().Tiles[num2 + l, num4 + l].VisibleFaces[0] = true;
					UpdateTile(num2 + l, num4 + l, num, floorTextureSelected, wallTextureSelected, result3);
				}
			}
			break;
		}
		case 3:
		case 4:
		{
			int num6 = Mathf.Abs(Mathf.Min(result, result2));
			for (int i = 0; i <= num6; i++)
			{
				if (TileMap.ValidTile(num3 - i, num4 + i))
				{
					UWEBase.CurrentTileMap().Tiles[num3 - i, num4 + i].VisibleFaces[0] = true;
					UpdateTile(num3 - i, num4 + i, num, floorTextureSelected, wallTextureSelected, result3);
				}
			}
			break;
		}
		}
	}

	public void UpdateTile(int tileXtoUpdate, int tileYtoUpdate, int TileTypeSelected, int FloorTextureSelected, int WallTextureSelected, int FloorHeight)
	{
		bool flag = false;
		if (!UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].NeedsReRender)
		{
			TileMapRenderer.DestroyTile(tileXtoUpdate, tileYtoUpdate);
		}
		UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].TileNeedsUpdate();
		switch (TileTypeSelected)
		{
		case 6:
		case 7:
		case 8:
		case 9:
			UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].shockSteep = 2;
			goto default;
		default:
			UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].tileType = (short)TileTypeSelected;
			break;
		case -1:
			break;
		}
		if (FloorHeight != -1)
		{
			UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].floorHeight = (short)(FloorHeight * 2);
		}
		if (FloorTextureSelected != -1)
		{
			UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].floorTexture = (short)FloorTextureSelected;
		}
		if (UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].wallTexture != WallTextureSelected)
		{
			if (UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].tileType == 0 && WallTextureSelected != -1)
			{
				UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].North = (short)WallTextureSelected;
				UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].South = (short)WallTextureSelected;
				UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].East = (short)WallTextureSelected;
				UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].West = (short)WallTextureSelected;
			}
			if (WallTextureSelected != -1)
			{
				UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate].wallTexture = (short)WallTextureSelected;
				if (tileYtoUpdate > 0)
				{
					if (UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate - 1].tileType > 0)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate - 1].North = (short)WallTextureSelected;
						flag = true;
					}
					else if (FollowMeMode && WallTextureSelected != -1)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].wallTexture = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].North = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].South = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].East = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].West = (short)WallTextureSelected;
						flag = true;
					}
				}
				if (tileYtoUpdate < 63)
				{
					if (UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate + 1].tileType > 0)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate + 1].South = (short)WallTextureSelected;
						flag = true;
					}
					else if (FollowMeMode && WallTextureSelected != -1)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate + 1].wallTexture = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate + 1].North = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate + 1].South = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate + 1].East = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate, tileYtoUpdate + 1].West = (short)WallTextureSelected;
						flag = true;
					}
				}
				if (tileXtoUpdate > 0)
				{
					if (UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].tileType > 0)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].East = (short)WallTextureSelected;
						flag = true;
					}
					else if (FollowMeMode && WallTextureSelected != -1)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].wallTexture = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].North = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].South = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].East = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate - 1, tileYtoUpdate].West = (short)WallTextureSelected;
						flag = true;
					}
				}
				if (tileXtoUpdate < 63)
				{
					if (UWEBase.CurrentTileMap().Tiles[tileXtoUpdate + 1, tileYtoUpdate].tileType > 0)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate + 1, tileYtoUpdate].West = (short)WallTextureSelected;
						flag = true;
					}
					else if (FollowMeMode && WallTextureSelected != -1)
					{
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate + 1, tileYtoUpdate].wallTexture = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate + 1, tileYtoUpdate].North = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate + 1, tileYtoUpdate].South = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate + 1, tileYtoUpdate].East = (short)WallTextureSelected;
						UWEBase.CurrentTileMap().Tiles[tileXtoUpdate + 1, tileYtoUpdate].West = (short)WallTextureSelected;
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			return;
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if ((i != 0 || j != 0) && i + tileXtoUpdate <= 63 && i + tileXtoUpdate >= 0 && j + tileYtoUpdate <= 63 && j + tileYtoUpdate >= 0)
				{
					if (!UWEBase.CurrentTileMap().Tiles[i + tileXtoUpdate, j + tileYtoUpdate].NeedsReRender)
					{
						TileMapRenderer.DestroyTile(i + tileXtoUpdate, j + tileYtoUpdate);
					}
					UWEBase.CurrentTileMap().Tiles[i + tileXtoUpdate, j + tileYtoUpdate].TileNeedsUpdate();
				}
			}
		}
	}

	public void Teleport()
	{
		float x = (float)TileX * 1.2f + 0.6f;
		float z = (float)TileY * 1.2f + 0.6f;
		if (ObjectDetailsPanel.gameObject.activeInHierarchy)
		{
			if (currObj.ObjectTileX != 99)
			{
				x = (float)currObj.ObjectTileX * 1.2f + 0.6f;
			}
			if (currObj.ObjectTileY != 99)
			{
				z = (float)currObj.ObjectTileY * 1.2f + 0.6f;
			}
		}
		float num = (float)UWEBase.CurrentTileMap().GetFloorHeight(TileX, TileY) * 0.15f;
		UWCharacter.Instance.gameObject.transform.position = new Vector3(x, num + 0.3f, z);
	}

	public void SelectCurrentTile()
	{
		TileX = TileMap.visitTileX;
		TileY = TileMap.visitTileY;
		RefreshTileInfo();
	}

	public void SelectTile(int tileX, int tileY)
	{
		TileX = tileX;
		TileY = tileY;
		RefreshTileInfo();
	}

	public void SwitchPanel(int Panel)
	{
		TileMapDetailsPanel.gameObject.SetActive(Panel == 0);
		ObjectDetailsPanel.gameObject.SetActive(Panel == 1);
		TextureMapDetailsPanel.gameObject.SetActive(Panel == 2);
		if (Panel == 1)
		{
			UpdateObjectsDropDown();
		}
		if (Panel != 1)
		{
			MobileObjectDetailsPanel.gameObject.SetActive(false);
		}
	}

	public void TogglePanels()
	{
		if (!EditorHidden)
		{
			SwitchPanel(-1);
			EditorHidden = true;
		}
		else
		{
			SwitchPanel(0);
			EditorHidden = false;
		}
		EditorBackground.SetActive(!EditorHidden);
	}

	public void RefreshObjectInfo()
	{
		currObj = UWEBase.CurrentObjectList().objInfo[ObjectSelect.value];
		ObjectItemIds.value = currObj.item_id;
		ObjectFlagDoorDir.isOn = currObj.doordir == 1;
		ObjectFlagEnchant.isOn = currObj.enchantment == 1;
		ObjectFlaginVis.isOn = currObj.invis == 1;
		ObjectFlagisQuant.isOn = currObj.is_quant == 1;
		ObjectFlagValue.text = currObj.flags.ToString();
		ObjectOwner.text = currObj.owner.ToString();
		ObjectLink.text = currObj.link.ToString();
		ObjectNext.text = currObj.next.ToString();
		ObjectQuality.text = currObj.quality.ToString();
		if (currObj.instance != null)
		{
			currObj.instance.UpdatePosition();
		}
		ObjectTileX.text = currObj.ObjectTileX.ToString();
		ObjectTileY.text = currObj.ObjectTileY.ToString();
		ObjectXPos.text = currObj.xpos.ToString();
		ObjectYPos.text = currObj.ypos.ToString();
		ObjectZPos.text = currObj.zpos.ToString();
		MobileObjectDetailsPanel.gameObject.SetActive(currObj.index <= 255);
		if (currObj.index <= 255)
		{
			npc_whoami.text = currObj.npc_whoami.ToString();
			npc_xhome.text = currObj.npc_xhome.ToString();
			npc_yhome.text = currObj.npc_yhome.ToString();
			npc_hp.text = currObj.npc_hp.ToString();
			npc_goal.value = currObj.npc_goal;
			npc_goaltarget.text = currObj.npc_gtarg.ToString();
			npc_attitude.text = currObj.npc_attitude.ToString();
			npc_talkedto.text = currObj.npc_talkedto.ToString();
		}
	}

	public void ToggleFollowMeMode()
	{
		FollowMeMode = !FollowMeMode;
		UWHUD.instance.MessageScroll.Add("Follow me mode = " + FollowMeMode);
	}

	public static void UpdateFollowMeMode(int tileX, int tileY)
	{
		int result = 0;
		int wallTextureSelected = instance.WallTextureSelect.value;
		int floorTextureSelected = instance.FloorTextureSelect.value;
		int num = instance.TileTypeSelect.value;
		int.TryParse(instance.TileHeightDetails.text, out result);
		if (instance.LockTileHeight.isOn)
		{
			result = -1;
		}
		if (instance.LockTileType.isOn || num == 0)
		{
			num = -1;
		}
		if (instance.LockFloorTextures.isOn)
		{
			floorTextureSelected = -1;
		}
		if (instance.LockWallTextures.isOn)
		{
			wallTextureSelected = -1;
		}
		UWEBase.CurrentTileMap().Tiles[tileX, tileY].VisibleFaces[0] = true;
		instance.UpdateTile(tileX, tileY, num, floorTextureSelected, wallTextureSelected, result);
	}

	public void ObjectEditorApplyChanges()
	{
		currObj.item_id = ObjectItemIds.value;
		if (ObjectFlagisQuant.isOn)
		{
			currObj.is_quant = 1;
		}
		else
		{
			currObj.is_quant = 0;
		}
		if (ObjectFlaginVis.isOn)
		{
			currObj.invis = 1;
		}
		else
		{
			currObj.invis = 0;
		}
		if (ObjectFlagDoorDir.isOn)
		{
			currObj.doordir = 1;
		}
		else
		{
			currObj.doordir = 0;
		}
		if (ObjectFlagEnchant.isOn)
		{
			currObj.enchantment = 1;
		}
		else
		{
			currObj.enchantment = 0;
		}
		int result = 0;
		if (int.TryParse(ObjectFlagValue.text, out result))
		{
			currObj.flags = (short)(result & 7);
			ObjectFlagValue.text = currObj.flags.ToString();
		}
		if (int.TryParse(ObjectOwner.text, out result))
		{
			currObj.owner = (short)(result & 0x3F);
			ObjectOwner.text = currObj.owner.ToString();
		}
		if (int.TryParse(ObjectLink.text, out result))
		{
			currObj.link = (short)(result & 0x3FF);
			ObjectLink.text = currObj.link.ToString();
		}
		if (int.TryParse(ObjectQuality.text, out result))
		{
			currObj.quality = (short)(result & 0x3F);
			ObjectQuality.text = currObj.quality.ToString();
		}
		if (currObj.index <= 255)
		{
			if (int.TryParse(npc_whoami.text, out result))
			{
				currObj.npc_whoami = (short)result;
				npc_whoami.text = currObj.npc_whoami.ToString();
			}
			if (int.TryParse(npc_xhome.text, out result))
			{
				currObj.npc_xhome = (short)result;
				npc_xhome.text = currObj.npc_xhome.ToString();
			}
			if (int.TryParse(npc_yhome.text, out result))
			{
				currObj.npc_yhome = (short)result;
				npc_yhome.text = currObj.npc_yhome.ToString();
			}
			if (int.TryParse(npc_hp.text, out result))
			{
				currObj.npc_hp = (short)result;
				npc_hp.text = currObj.npc_hp.ToString();
			}
			currObj.npc_goal = (short)npc_goal.value;
			if (int.TryParse(npc_goaltarget.text, out result))
			{
				currObj.npc_gtarg = (short)result;
				npc_goaltarget.text = currObj.npc_gtarg.ToString();
			}
			if (int.TryParse(npc_attitude.text, out result))
			{
				currObj.npc_attitude = (short)result;
				npc_attitude.text = currObj.npc_attitude.ToString();
			}
			if (int.TryParse(npc_talkedto.text, out result))
			{
				currObj.npc_talkedto = (short)(result & 1);
				npc_talkedto.text = currObj.npc_talkedto.ToString();
			}
		}
		int itemType = currObj.GetItemType();
		if ((itemType == 21 || itemType == 56) && int.TryParse(ObjectNext.text, out result))
		{
			currObj.next = (short)(result & 0x3FF);
			ObjectNext.text = currObj.next.ToString();
		}
		if (int.TryParse(ObjectTileX.text, out result))
		{
			if (result < 0 || result > 63)
			{
				result = 99;
			}
			currObj.ObjectTileX = (short)result;
			ObjectTileX.text = currObj.ObjectTileX.ToString();
		}
		if (int.TryParse(ObjectTileY.text, out result))
		{
			if (result < 0 || result > 63)
			{
				result = 99;
			}
			currObj.ObjectTileY = (short)result;
			ObjectTileY.text = currObj.ObjectTileY.ToString();
		}
		if (int.TryParse(ObjectXPos.text, out result))
		{
			currObj.xpos = (short)(result & 7);
			ObjectXPos.text = currObj.xpos.ToString();
		}
		if (int.TryParse(ObjectYPos.text, out result))
		{
			currObj.ypos = (short)(result & 7);
			ObjectYPos.text = currObj.ypos.ToString();
		}
		if (int.TryParse(ObjectZPos.text, out result))
		{
			currObj.zpos = (short)(result & 0x7F);
			ObjectZPos.text = currObj.zpos.ToString();
		}
		if (currObj.instance != null)
		{
			Object.Destroy(currObj.instance.gameObject);
			Vector3 position = ObjectLoader.CalcObjectXYZ(currObj.index, 0);
			ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, position);
		}
		else if (currObj.ObjectTileX <= 63)
		{
			currObj.InUseFlag = 1;
			Vector3 position2 = ObjectLoader.CalcObjectXYZ(currObj.index, 0);
			ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, position2);
		}
	}

	public void ToggleCamera()
	{
		if (!OverheadCam.gameObject.activeInHierarchy)
		{
			if (GameWorldController.instance.ceiling != null)
			{
				GameWorldController.instance.ceiling.SetActive(false);
			}
			OverheadCam.gameObject.SetActive(true);
			UWCharacter.Instance.playerCam.tag = "Untagged";
			UWCharacter.Instance.playerCam.enabled = false;
			OverheadCam.tag = "MainCamera";
		}
		else
		{
			if (GameWorldController.instance.ceiling != null)
			{
				GameWorldController.instance.ceiling.SetActive(true);
			}
			OverheadCam.gameObject.SetActive(false);
			UWCharacter.Instance.playerCam.tag = "MainCamera";
			UWCharacter.Instance.playerCam.enabled = true;
			OverheadCam.tag = "Untagged";
		}
	}

	private void OnGUI()
	{
		if (!OverheadCam.gameObject.activeInHierarchy)
		{
			return;
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			if (Input.GetAxis("Mouse ScrollWheel") <= 0f)
			{
				OverheadCam.orthographicSize += 1f;
			}
			else
			{
				OverheadCam.orthographicSize -= 1f;
			}
		}
		if (OverheadCam.orthographicSize <= 0f)
		{
			OverheadCam.orthographicSize = 1f;
		}
	}

	public override void Update()
	{
		base.Update();
		if (OverheadCam.gameObject.activeInHierarchy)
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				OverheadCam.gameObject.transform.Translate(new Vector3(10f * Time.deltaTime, 0f, 0f));
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				OverheadCam.gameObject.transform.Translate(new Vector3(-10f * Time.deltaTime, 0f, 0f));
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				OverheadCam.gameObject.transform.Translate(new Vector3(0f, -10f * Time.deltaTime, 0f));
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				OverheadCam.gameObject.transform.Translate(new Vector3(0f, 10f * Time.deltaTime, 0f));
			}
		}
	}

	public void GenerateRandomLevel()
	{
		int result = 0;
		if (int.TryParse(seed.text, out result))
		{
			UnderworldGenerator.instance.GenerateLevel(result);
			UnderworldGenerator.instance.RoomsToTileMap(UWEBase.CurrentTileMap(), UWEBase.CurrentTileMap().Tiles);
			GameWorldController.WorldReRenderPending = true;
			GameWorldController.FullReRender = true;
			float x = (float)UnderworldGenerator.instance.startX * 1.2f + 0.6f;
			float z = (float)UnderworldGenerator.instance.startY * 1.2f + 0.6f;
			float num = (float)UWEBase.CurrentTileMap().GetFloorHeight(UnderworldGenerator.instance.startX, UnderworldGenerator.instance.startY) * 0.15f;
			UWCharacter.Instance.transform.position = new Vector3(x, num + 0.1f, z);
		}
	}
}
