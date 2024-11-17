using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class GameWorldController : UWEBase
{
	public enum UW1_LevelNos
	{
		EntranceLevel = 0,
		MountainMen = 1,
		Swamp = 2,
		Knights = 3,
		Catacombs = 4,
		Seers = 5,
		Tybal = 6,
		Volcano = 7,
		Ethereal = 8
	}

	public enum Worlds
	{
		Britannia = 0,
		PrisonTower = 8,
		Killorn = 16,
		Ice = 24,
		Talorus = 32,
		Academy = 40,
		Tomb = 48,
		Pits = 56,
		Ethereal = 64
	}

	public enum UW2_LevelNos
	{
		Britannia0 = 0,
		Britannia1 = 1,
		Britannia2 = 2,
		Britannia3 = 3,
		Britannia4 = 4,
		Prison0 = 8,
		Prison1 = 9,
		Prison2 = 10,
		Prison3 = 11,
		Prison4 = 12,
		Prison5 = 13,
		Prison6 = 14,
		Prison7 = 15,
		Killorn0 = 16,
		Killorn1 = 17,
		Ice0 = 24,
		Ice1 = 25,
		Talorus0 = 32,
		Talorus1 = 33,
		Academy0 = 40,
		Academy1 = 41,
		Academy2 = 42,
		Academy3 = 43,
		Academy4 = 44,
		Academy5 = 45,
		Academy6 = 46,
		Academy7 = 47,
		Tomb0 = 48,
		Tomb1 = 49,
		Tomb2 = 50,
		Tomb3 = 51,
		Pits0 = 56,
		Pits1 = 57,
		Pits2 = 58,
		Ethereal0 = 64,
		Ethereal1 = 65,
		Ethereal2 = 66,
		Ethereal3 = 67,
		Ethereal4 = 68,
		Ethereal5 = 69,
		Ethereal6 = 70,
		Ethereal7 = 71,
		Ethereal8 = 72
	}

	public struct bablGlobal
	{
		public int ConversationNo;

		public int Size;

		public int[] Globals;
	}

	public bool EnableUnderworldGenerator = false;

	public bool DoCleanUp = true;

	public GameObject ceiling;

	public WhatTheHellIsSCD_ARK whatTheHellIsThatFileFor;

	public static string[] UW1_LevelNames = new string[9] { "Outcast", "Dwarf", "Swamp", "Knight", "Tombs", "Seers", "Tybal", "Abyss", "Void" };

	[Header("Controls")]
	public MouseLook MouseX;

	public MouseLook MouseY;

	[Header("World Options")]
	public bool EnableTextureAnimation;

	public Shader greyScale;

	public Shader vortex;

	public bool AtMainMenu;

	public bool EnableTimerTriggers = true;

	public float TimerRate = 1f;

	[Header("Parent Objects")]
	public GameObject LevelModel;

	public GameObject TNovaLevelModel;

	public GameObject SceneryModel;

	public GameObject _ObjectMarker;

	public static GameWorldController instance;

	public GameObject InventoryMarker;

	[Header("Level")]
	public short LevelNo;

	public static bool LoadingGame = false;

	public static bool NavMeshReady = false;

	public bool[] NavMeshesReady = new bool[4];

	private static string LevelSignature;

	public short startLevel = 0;

	public Vector3 StartPos = new Vector3(38f, 4f, 2.7f);

	public bool CreateReports;

	public bool ShowOnlyInUse;

	[Header("Palettes")]
	public Texture2D[] paletteArray = new Texture2D[8];

	public int paletteIndex = 0;

	public int paletteIndexReverse = 0;

	public PaletteLoader palLoader;

	[Header("LevelMaps")]
	public TileMap[] Tilemaps = new TileMap[9];

	public AutoMap[] AutoMaps = new AutoMap[9];

	public ObjectLoader[] objectList = new ObjectLoader[9];

	public ObjectLoader inventoryLoader = new ObjectLoader();

	private MusicController mus;

	[Header("Property Lists")]
	public ObjectMasters objectMaster;

	public Critters critterData;

	public ObjectDatLoader objDat;

	public CommonObjectDatLoader commonObject;

	public ObjectPropLoader ShockObjProp;

	public TerrainDatLoader terrainData;

	[Header("Paths")]
	public string Lev_Ark_File_Selected = "";

	public string SCD_Ark_File_Selected = "";

	public string path_uw0;

	public string path_uw1;

	public string path_uw2;

	public string path_shock;

	public string path_tnova;

	[Header("Material Lists")]
	public Material[] MaterialMasterList = new Material[260];

	public Material[] SpecialMaterials = new Material[1];

	public Material Jorge;

	public Material[] MaterialDoors = new Material[13];

	public Material[] MaterialObj = new Material[54];

	public Material modelMaterial;

	[Header("Nav Meshes")]
	public bool bGenNavMeshes = true;

	public int GenNavMeshNextFrame = -1;

	public NavMeshSurface NavMeshLand;

	public NavMeshSurface NavMeshWater;

	public NavMeshSurface NavMeshAir;

	public NavMeshSurface NavMeshLava;

	public int MapMeshLayerMask = 0;

	public int DoorLayerMask = 0;

	[Header("Art Loaders")]
	public BytLoader bytloader;

	public TextureLoader texLoader;

	public GRLoader SpellIcons;

	public GRLoader ObjectArt;

	public GRLoader DoorArt;

	public GRLoader TmObjArt;

	public GRLoader TmFlatArt;

	public GRLoader TmAnimo;

	public GRLoader armor_f;

	public GRLoader armor_m;

	public GRLoader grCursors;

	public GRLoader grFlasks;

	public GRLoader grOptbtns;

	public GRLoader grCompass;

	public CutsLoader cutsLoader;

	public CritLoader[] critsLoader = new CritLoader[64];

	public WeaponAnimation weaps;

	public WeaponsLoader weapongr;

	public int difficulty = 1;

	public static bool LoadingObjects = false;

	public bablGlobal[] bGlobals;

	public ConversationVM convVM;

	public static bool WorldReRenderPending = false;

	public static bool ObjectReRenderPending = false;

	public static bool FullReRender = false;

	public KeyBindings keybinds;

	public event_processor events;

	private int startX = -1;

	private int startY = -1;

	private int StartHeight = -1;

	private void LoadPath(string _RES)
	{
		string basePath = "";
		switch (_RES)
		{
		case "UW0":
			basePath = instance.path_uw0;
			break;
		case "UW1":
			basePath = instance.path_uw1;
			break;
		case "UW2":
			basePath = instance.path_uw2;
			break;
		case "SHOCK":
			basePath = instance.path_shock;
			break;
		case "TNOVA":
			basePath = instance.path_tnova;
			break;
		}
		Loader.BasePath = basePath;
	}

	private void Awake()
	{
		instance = this;
		UWClass.sep = Path.AltDirectorySeparatorChar;
		Lev_Ark_File_Selected = "DATA" + UWEBase.sep + "LEV.ARK";
		SCD_Ark_File_Selected = "DATA" + UWEBase.sep + "SCD.ARK";
		LoadConfigFile();
	}

	private void Start()
	{
		instance = this;
		AtMainMenu = true;
		Begin("UW1");
	}

	private void Update()
	{
		PositionDetect();
	}

	private IEnumerator UpdateNavMeshes()
	{
		NavMeshReady = false;
		NavMeshesReady[0] = false;
		NavMeshesReady[1] = false;
		NavMeshesReady[2] = false;
		while (LoadingGame)
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(GenerateNavmesh(NavMeshLand, 0));
		StartCoroutine(GenerateNavmesh(NavMeshWater, 1));
		StartCoroutine(GenerateNavmesh(NavMeshLava, 2));
		StartCoroutine(GenerateNavmesh(NavMeshAir, 3));
		while (!NavMeshesReady[0] || !NavMeshesReady[1] || !NavMeshesReady[2] || !NavMeshesReady[3])
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(1.5f);
		NavMeshReady = true;
		yield return 0;
	}

	private IEnumerator GenerateNavmesh(NavMeshSurface navmeshobj, int index)
	{
		if (navmeshobj.navMeshData == null)
		{
			navmeshobj.BuildNavMesh();
		}
		else
		{
			AsyncOperation task = navmeshobj.UpdateNavMesh(navmeshobj.navMeshData);
			while (!task.isDone)
			{
				yield return new WaitForSeconds(0.1f);
			}
		}
		NavMeshesReady[index] = true;
		yield return 0;
	}

	private void LateUpdate()
	{
		if (WorldReRenderPending)
		{
			if (!FullReRender || !UWEBase.EditorMode)
			{
			}
			TileMapRenderer.GenerateLevelFromTileMap(instance.LevelModel, instance.SceneryModel, UWEBase._RES, UWEBase.CurrentTileMap(), UWEBase.CurrentObjectList(), !FullReRender);
			if (ObjectReRenderPending)
			{
				ObjectReRenderPending = false;
				ObjectLoader.RenderObjectList(UWEBase.CurrentObjectList(), UWEBase.CurrentTileMap(), DynamicObjectMarker().gameObject);
			}
			WorldReRenderPending = false;
			FullReRender = false;
			if (!UWEBase.EditorMode)
			{
				NavMeshLand.UpdateNavMesh(NavMeshLand.navMeshData);
				NavMeshWater.UpdateNavMesh(NavMeshWater.navMeshData);
				NavMeshLava.UpdateNavMesh(NavMeshLava.navMeshData);
			}
			else
			{
				IngameEditor.instance.RefreshTileMap();
			}
		}
	}

	public void Begin(string res)
	{
		UWHUD.instance.gameSelectUi.SetActive(false);
		LoadPath(res);
		UWEBase._RES = res;
		UWClass._RES = res;
		keybinds.ApplyBindings();
		MapMeshLayerMask = 1 << LevelModel.layer;
		DoorLayerMask = 1 << LayerMask.NameToLayer("Doors");
		switch (res)
		{
		case "TNOVA":
			UWCharacter.Instance.XAxis.enabled = true;
			UWCharacter.Instance.YAxis.enabled = true;
			UWCharacter.Instance.MouseLookEnabled = true;
			UWCharacter.Instance.speedMultiplier = 20f;
			break;
		case "SHOCK":
			palLoader = new PaletteLoader("res" + UWEBase.sep + "DATA" + UWEBase.sep + "GAMEPAL.RES", 700);
			texLoader = new TextureLoader();
			objectMaster = new ObjectMasters();
			ObjectArt = new GRLoader("res" + UWEBase.sep + "DATA" + UWEBase.sep + "OBJART.RES", 1350);
			ShockObjProp = new ObjectPropLoader();
			UWCharacter.Instance.XAxis.enabled = true;
			UWCharacter.Instance.YAxis.enabled = true;
			UWCharacter.Instance.MouseLookEnabled = true;
			UWCharacter.Instance.speedMultiplier = 20f;
			break;
		default:
		{
			StartCoroutine(MusicController.instance.Begin());
			objectMaster = new ObjectMasters();
			objDat = new ObjectDatLoader();
			commonObject = new CommonObjectDatLoader();
			palLoader = new PaletteLoader("DATA" + UWEBase.sep + "PALS.DAT", -1);
			PaletteLoader paletteLoader = new PaletteLoader("DATA" + UWEBase.sep + "PALS.DAT", -1);
			for (int i = 0; i <= 27; i++)
			{
				string rES = UWEBase._RES;
				if (rES != null && rES == "UW2")
				{
					Palette.cyclePalette(paletteLoader.Palettes[0], 224, 16);
					Palette.cyclePaletteReverse(paletteLoader.Palettes[0], 3, 6);
				}
				else
				{
					Palette.cyclePalette(paletteLoader.Palettes[0], 48, 16);
					Palette.cyclePaletteReverse(paletteLoader.Palettes[0], 16, 7);
				}
				paletteArray[i] = Palette.toImage(paletteLoader.Palettes[0]);
			}
			bytloader = new BytLoader();
			texLoader = new TextureLoader();
			ObjectArt = new GRLoader(20);
			ObjectArt.xfer = true;
			SpellIcons = new GRLoader(28);
			DoorArt = new GRLoader(12);
			TmObjArt = new GRLoader(30);
			TmFlatArt = new GRLoader(29);
			TmAnimo = new GRLoader(1);
			TmAnimo.xfer = true;
			armor_f = new GRLoader(2);
			armor_m = new GRLoader(3);
			grCursors = new GRLoader(11);
			grFlasks = new GRLoader(15);
			grOptbtns = new GRLoader(23);
			grCompass = new GRLoader(9);
			terrainData = new TerrainDatLoader();
			weaps = new WeaponAnimation();
			break;
		}
		}
		switch (UWEBase._RES)
		{
		case "UW2":
			if (instance.startLevel == 0)
			{
				instance.StartPos = new Vector3(23.43f, 3.95f, 58.29f);
			}
			break;
		case "UW0":
			instance.StartPos = new Vector3(39.06f, 3.96f, 3f);
			break;
		default:
			if (instance.startLevel == 0)
			{
				instance.StartPos = new Vector3(39.06f, 3.96f, 3f);
			}
			break;
		case "SHOCK":
		case "TNOVA":
			break;
		}
		switch (res)
		{
		case "TNOVA":
			AtMainMenu = false;
			TileMapRenderer.EnableCollision = false;
			bGenNavMeshes = false;
			UWHUD.instance.gameObject.SetActive(false);
			UWHUD.instance.window.SetFullScreen();
			UWCharacter.Instance.isFlying = true;
			UWCharacter.Instance.playerMotor.enabled = true;
			UWCharacter.Instance.playerCam.backgroundColor = Color.white;
			SwitchTNovaMap("");
			return;
		case "SHOCK":
			TileMapRenderer.EnableCollision = false;
			bGenNavMeshes = false;
			AtMainMenu = false;
			UWCharacter.Instance.isFlying = true;
			UWCharacter.Instance.playerMotor.enabled = true;
			UWHUD.instance.gameObject.SetActive(false);
			UWHUD.instance.window.SetFullScreen();
			SwitchLevel(startLevel);
			return;
		case "UW0":
			AtMainMenu = false;
			UWCharacter.Instance.transform.position = instance.StartPos;
			UWHUD.instance.Begin();
			UWCharacter.Instance.Begin();
			UWCharacter.Instance.playerInventory.Begin();
			StringController.instance.LoadStringsPak(Loader.BasePath + "DATA" + UWEBase.sep + "STRINGS.PAK");
			break;
		case "UW2":
			UWHUD.instance.Begin();
			UWCharacter.Instance.Begin();
			UWCharacter.Instance.playerInventory.Begin();
			Quest.instance.QuestVariables = new int[250];
			StringController.instance.LoadStringsPak(Loader.BasePath + "DATA" + UWEBase.sep + "STRINGS.PAK");
			break;
		default:
			UWHUD.instance.Begin();
			UWCharacter.Instance.Begin();
			UWCharacter.Instance.playerInventory.Begin();
			StringController.instance.LoadStringsPak(Loader.BasePath + "DATA" + UWEBase.sep + "STRINGS.PAK");
			break;
		}
		if (EnableTextureAnimation)
		{
			UWHUD.instance.CutsceneFullPanel.SetActive(false);
			InvokeRepeating("UpdateAnimation", 0.2f, 0.2f);
		}
		if (AtMainMenu)
		{
			SwitchLevel(-1);
			UWHUD.instance.CutsceneFullPanel.SetActive(true);
			UWHUD.instance.mainmenu.gameObject.SetActive(true);
			UWCharacter.Instance.playerController.enabled = false;
			UWCharacter.Instance.playerMotor.enabled = false;
			UWCharacter.Instance.transform.position = Vector3.zero;
			MusicController.instance.InIntro = true;
		}
		else
		{
			UWHUD.instance.CutsceneFullPanel.SetActive(false);
			UWHUD.instance.mainmenu.gameObject.SetActive(false);
			UWHUD.instance.RefreshPanels(0);
			SwitchLevel(startLevel);
		}
	}

	private void UpdateAnimation()
	{
		Shader.SetGlobalTexture("_ColorPaletteIn", paletteArray[paletteIndex]);
		if (paletteIndex < paletteArray.GetUpperBound(0))
		{
			paletteIndex++;
		}
		else
		{
			paletteIndex = 0;
		}
	}

	public static GameObject FindTile(int x, int y, int surface)
	{
		string tileName = GetTileName(x, y, surface);
		Transform transform = instance.LevelModel.transform.Find(tileName);
		if (transform != null)
		{
			return transform.gameObject;
		}
		Debug.Log("Cannot find " + tileName);
		return null;
	}

	public static string GetTileName(int x, int y, int surface)
	{
		string text = x.ToString("D2");
		string text2 = y.ToString("D2");
		switch (surface)
		{
		case 3:
			return "Wall_" + text + "_" + text2;
		case 2:
			return "Ceiling_" + text + "_" + text2;
		default:
			return "Tile_" + text + "_" + text2;
		}
	}

	public static GameObject FindTileByName(string tileName)
	{
		return instance.LevelModel.transform.Find(tileName).gameObject;
	}

	public Transform DynamicObjectMarker()
	{
		return _ObjectMarker.transform;
	}

	public void SwitchLevel(short newLevelNo)
	{
		if (newLevelNo != -1)
		{
			if (LevelNo == -1)
			{
				critsLoader = new CritLoader[64];
				InitLevelData();
			}
			if (UWEBase._RES == "UW2")
			{
				MusicController.instance.ChangeTrackListForUW2(newLevelNo);
			}
			if (Tilemaps[newLevelNo] == null)
			{
				Tilemaps[newLevelNo] = new TileMap(newLevelNo);
				if (UWEBase._RES != "SHOCK")
				{
					DataLoader.UWBlock uWBlock = default(DataLoader.UWBlock);
					DataLoader.UWBlock tex_ark_block = default(DataLoader.UWBlock);
					DataLoader.UWBlock uwb = default(DataLoader.UWBlock);
					uWBlock = LoadLevArkBlock(newLevelNo);
					if (UWEBase._RES == "UW1")
					{
						DataLoader.LoadUWBlock(LevArk.lev_ark_file_data, newLevelNo + 9, 384L, out uwb);
					}
					tex_ark_block = LoadTexArkBlock(newLevelNo, tex_ark_block);
					if (uWBlock.DataLen > 0 && tex_ark_block.DataLen > 0)
					{
						if (EnableUnderworldGenerator)
						{
							UnderworldGenerator.instance.GenerateLevel(UnderworldGenerator.instance.Seed);
							Tilemaps[newLevelNo] = UnderworldGenerator.instance.CreateTileMap(newLevelNo);
							startX = UnderworldGenerator.instance.startX;
							startY = UnderworldGenerator.instance.startY;
						}
						else
						{
							Tilemaps[newLevelNo].BuildTileMapUW(newLevelNo, uWBlock, tex_ark_block, uwb);
						}
						objectList[newLevelNo] = new ObjectLoader();
						objectList[newLevelNo].LoadObjectList(Tilemaps[newLevelNo], uWBlock);
						if (CreateReports)
						{
							CreateObjectReport(objectList[newLevelNo].objInfo, newLevelNo);
						}
						if (EnableUnderworldGenerator)
						{
							for (int i = 0; i <= objectList[newLevelNo].objInfo.GetUpperBound(0); i++)
							{
								objectList[newLevelNo].objInfo[i].InUseFlag = 0;
							}
						}
					}
				}
				else
				{
					Tilemaps[newLevelNo].BuildTileMapShock(LevArk.lev_ark_file_data, newLevelNo);
					objectList[newLevelNo] = new ObjectLoader();
					objectList[newLevelNo].LoadObjectListShock(Tilemaps[newLevelNo], LevArk.lev_ark_file_data);
				}
				if (!UWEBase.EditorMode)
				{
					Tilemaps[newLevelNo].CleanUp(UWEBase._RES);
				}
			}
			if (UWEBase._RES != "SHOCK" && LevelNo != -1)
			{
				foreach (Transform item in instance.InventoryMarker.transform)
				{
					if (item.gameObject.GetComponent<object_base>() != null)
					{
						item.gameObject.GetComponent<object_base>().InventoryEventOnLevelExit();
					}
				}
			}
			if (LevelNo != -1 && !UWEBase.EditorMode)
			{
				ObjectLoader.UpdateObjectList(UWEBase.CurrentTileMap(), UWEBase.CurrentObjectList());
			}
			LevelNo = newLevelNo;
			string rES = UWEBase._RES;
			if ((rES == null || !(rES == "SHOCK")) && !UWEBase.EditorMode && !LoadingGame)
			{
				foreach (Transform item2 in instance.InventoryMarker.transform)
				{
					if (item2.gameObject.GetComponent<object_base>() != null)
					{
						item2.gameObject.GetComponent<object_base>().InventoryEventOnLevelEnter();
					}
				}
			}
			TileMapRenderer.GenerateLevelFromTileMap(LevelModel, SceneryModel, UWEBase._RES, Tilemaps[newLevelNo], objectList[newLevelNo], false);
			PlaceCharacter(newLevelNo);
			string rES2 = UWEBase._RES;
			if (rES2 == null || !(rES2 == "SHOCK"))
			{
			}
			ObjectLoader.RenderObjectList(objectList[newLevelNo], Tilemaps[newLevelNo], DynamicObjectMarker().gameObject);
			if (bGenNavMeshes && !UWEBase.EditorMode)
			{
				string signature = UWEBase.CurrentTileMap().getSignature();
				if (signature != LevelSignature)
				{
					NavMeshReady = false;
					StartCoroutine(UpdateNavMeshes());
				}
				LevelSignature = signature;
			}
			if (LevelNo == 7 && UWEBase._RES == "UW1")
			{
				CreateShrineLava();
			}
		}
		if (UWEBase._RES == "UW2" && !UWEBase.EditorMode && events != null && !LoadingGame)
		{
			events.ProcessEvents();
		}
	}

	private void CreateShrineLava()
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = SceneryModel.transform;
		gameObject.transform.localPosition = new Vector3(-39f, 39.61f, 0.402f);
		gameObject.transform.localScale = new Vector3(6f, 0.2f, 4.8f);
		gameObject.AddComponent<ShrineLava>();
		gameObject.AddComponent<BoxCollider>();
		gameObject.GetComponent<BoxCollider>().isTrigger = true;
	}

	private void PlaceCharacter(short newLevelNo)
	{
		if (startX != -1 && startY != -1)
		{
			float x = (float)startX * 1.2f + 0.6f;
			float z = (float)startY * 1.2f + 0.6f;
			float num = ((StartHeight != -1) ? ((float)StartHeight * 0.15f) : ((float)instance.Tilemaps[newLevelNo].GetFloorHeight(startX, startY) * 0.15f));
			UWCharacter.Instance.transform.position = new Vector3(x, num + 0.5f, z);
			UWCharacter.Instance.TeleportPosition = new Vector3(x, num + 0.1f, z);
			if (EnableUnderworldGenerator)
			{
				instance.StartPos = UWCharacter.Instance.transform.position;
			}
		}
		startX = -1;
		startY = -1;
	}

	private static DataLoader.UWBlock LoadTexArkBlock(short newLevelNo, DataLoader.UWBlock tex_ark_block)
	{
		switch (UWEBase._RES)
		{
		case "UW0":
			DataLoader.ReadStreamFile(Loader.BasePath + "DATA" + UWEBase.sep + "LEVEL13.TXM", out tex_ark_block.Data);
			tex_ark_block.DataLen = tex_ark_block.Data.GetUpperBound(0);
			break;
		case "UW2":
			DataLoader.LoadUWBlock(LevArk.lev_ark_file_data, newLevelNo + 80, -1L, out tex_ark_block);
			break;
		default:
			DataLoader.LoadUWBlock(LevArk.lev_ark_file_data, newLevelNo + 18, 122L, out tex_ark_block);
			break;
		}
		return tex_ark_block;
	}

	private static DataLoader.UWBlock LoadLevArkBlock(short newLevelNo)
	{
		DataLoader.UWBlock uwb;
		if (UWEBase._RES == "UW0")
		{
			uwb = default(DataLoader.UWBlock);
			uwb.DataLen = 31750L;
			uwb.Data = LevArk.lev_ark_file_data;
		}
		else
		{
			DataLoader.LoadUWBlock(LevArk.lev_ark_file_data, newLevelNo, 31750L, out uwb);
		}
		return uwb;
	}

	public void SwitchLevel(short newLevelNo, short newTileX, short newTileY)
	{
		startX = newTileX;
		startY = newTileY;
		StartHeight = -1;
		SwitchLevel(newLevelNo);
	}

	public void SwitchLevel(short newLevelNo, short newTileX, short newTileY, short newStartHeight)
	{
		startX = newTileX;
		startY = newTileY;
		StartHeight = newStartHeight;
		SwitchLevel(newLevelNo);
	}

	public void PositionDetect()
	{
		if (AtMainMenu || WindowDetect.InMap || (UWEBase._RES != "UW1" && UWEBase._RES != "UW0" && UWEBase._RES != "UW2"))
		{
			return;
		}
		TileMap.visitTileX = (short)(UWCharacter.Instance.transform.position.x / 1.2f);
		TileMap.visitTileY = (short)(UWCharacter.Instance.transform.position.z / 1.2f);
		if (UWEBase.EditorMode && (TileMap.visitedTileX != TileMap.visitTileX || TileMap.visitedTileY != TileMap.visitTileY) && IngameEditor.FollowMeMode)
		{
			IngameEditor.UpdateFollowMeMode(TileMap.visitTileX, TileMap.visitTileY);
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (TileMap.visitTileX + i >= 0 && TileMap.visitTileX + i <= 63 && TileMap.visitTileY + j >= 0 && TileMap.visitTileY + j <= 63)
				{
					UWEBase.CurrentAutoMap().MarkTile(TileMap.visitTileX + i, TileMap.visitTileY + j, UWEBase.CurrentTileMap().Tiles[TileMap.visitTileX + i, TileMap.visitTileY + j].tileType, AutoMap.GetDisplayType(UWEBase.CurrentTileMap().Tiles[TileMap.visitTileX + i, TileMap.visitTileY + j]));
				}
			}
		}
		TileMap.visitedTileX = TileMap.visitTileX;
		TileMap.visitedTileY = TileMap.visitTileY;
		UWCharacter.Instance.CurrentTerrain = UWEBase.CurrentTileMap().Tiles[TileMap.visitTileX, TileMap.visitTileY].terrain;
		UWCharacter.Instance.terrainType = TerrainDatLoader.getTerrain(UWCharacter.Instance.CurrentTerrain);
	}

	public static void MoveToWorld(GameObject obj)
	{
		MoveToWorld(obj.GetComponent<ObjectInteraction>());
	}

	public static ObjectInteraction MoveToWorld(ObjectInteraction obj)
	{
		obj.UpdatePosition();
		obj.transform.parent = instance.DynamicObjectMarker();
		ObjectLoader.AssignObjectToList(ref obj);
		obj.GetComponent<object_base>().MoveToWorldEvent();
		if (ConversationVM.InConversation)
		{
			Debug.Log("Use of MoveToWorld in conversation. Review usage to avoid object list corruption! " + obj.name);
			ConversationVM.BuildObjectList();
		}
		return obj;
	}

	public static void MoveToInventory(GameObject obj)
	{
		MoveToInventory(obj.GetComponent<ObjectInteraction>());
	}

	public static void MoveToInventory(ObjectInteraction obj)
	{
		obj.objectloaderinfo.InUseFlag = 0;
		obj.objectloaderinfo.instance = null;
		if (UWEBase._RES == "UW2")
		{
			ObjectLoaderInfo.CleanUp(obj.objectloaderinfo);
		}
		obj.GetComponent<object_base>().MoveToInventoryEvent();
		if (ConversationVM.InConversation)
		{
			ConversationVM.BuildObjectList();
		}
	}

	public void UpdatePositions()
	{
		foreach (Transform item in instance.DynamicObjectMarker())
		{
			if (item.gameObject.GetComponent<ObjectInteraction>() != null)
			{
				item.gameObject.GetComponent<ObjectInteraction>().UpdatePosition();
			}
		}
	}

	private void InitLevelData()
	{
		switch (UWEBase._RES)
		{
		case "SHOCK":
			Tilemaps = new TileMap[15];
			objectList = new ObjectLoader[15];
			break;
		case "UW0":
			Tilemaps = new TileMap[1];
			objectList = new ObjectLoader[1];
			AutoMaps = new AutoMap[1];
			break;
		case "UW2":
			Tilemaps = new TileMap[80];
			objectList = new ObjectLoader[80];
			AutoMaps = new AutoMap[80];
			break;
		default:
			Tilemaps = new TileMap[9];
			objectList = new ObjectLoader[9];
			AutoMaps = new AutoMap[9];
			break;
		}
		switch (UWEBase._RES)
		{
		case "SHOCK":
			MaterialMasterList = new Material[273];
			break;
		case "UW0":
			MaterialMasterList = new Material[58];
			break;
		case "UW2":
			MaterialMasterList = new Material[256];
			break;
		default:
			MaterialMasterList = new Material[260];
			break;
		}
		for (int i = 0; i <= MaterialMasterList.GetUpperBound(0); i++)
		{
			if (File.Exists(texLoader.ModPath(i)))
			{
				MaterialMasterList[i] = (Material)Resources.Load("Materials/ModShaders/" + UWEBase._RES + "_" + i.ToString("d3"));
			}
			else
			{
				MaterialMasterList[i] = (Material)Resources.Load(UWEBase._RES + "/Materials/textures/" + UWEBase._RES + "_" + i.ToString("d3"));
			}
			switch (MaterialMasterList[i].shader.name.ToUpper())
			{
			case "COLOURREPLACEMENT":
			case "COLOURREPLACEMENTREVERSE":
				MaterialMasterList[i].mainTexture = texLoader.LoadImageAt(i, 1);
				break;
			case "BASICUWSHADER":
				MaterialMasterList[i].mainTexture = texLoader.LoadImageAt(i, 0);
				break;
			case "LEGACY SHADERS/BUMPED DIFFUSE":
			{
				Texture2D texture2D = texLoader.LoadImageAt(i, 2);
				MaterialMasterList[i].mainTexture = texLoader.LoadImageAt(i, 0);
				if (texture2D != null)
				{
					MaterialMasterList[i].SetTexture("_BumpMap", TextureLoader.NormalMap(texture2D, 1f));
				}
				break;
			}
			default:
				Debug.Log(i + " is " + MaterialMasterList[i].shader.name);
				MaterialMasterList[i].mainTexture = texLoader.LoadImageAt(i, 0);
				break;
			}
		}
		if (UWEBase._RES == "UW1")
		{
			SpecialMaterials[0] = (Material)Resources.Load(UWEBase._RES + "/Materials/textures/" + UWEBase._RES + "_224_maze");
			SpecialMaterials[0].mainTexture = texLoader.LoadImageAt(224);
		}
		MaterialObj = new Material[TmObjArt.NoOfFileImages()];
		for (int j = 0; j <= MaterialObj.GetUpperBound(0); j++)
		{
			MaterialObj[j] = (Material)Resources.Load(UWEBase._RES + "/Materials/tmobj/tmobj_" + j.ToString("d2"));
			if (MaterialObj[j] != null)
			{
				MaterialObj[j].mainTexture = TmObjArt.LoadImageAt(j);
			}
		}
		string rES = UWEBase._RES;
		if (rES == null || !(rES == "SHOCK"))
		{
			for (int k = 0; k <= MaterialDoors.GetUpperBound(0); k++)
			{
				MaterialDoors[k] = (Material)Resources.Load(UWEBase._RES + "/Materials/doors/doors_" + k.ToString("d2") + "_material");
				MaterialDoors[k].mainTexture = DoorArt.LoadImageAt(k);
			}
		}
		string text;
		switch (UWEBase._RES)
		{
		case "SHOCK":
			text = "RES" + UWEBase.sep + "DATA" + UWEBase.sep + "ARCHIVE.DAT";
			break;
		case "UW0":
			text = "DATA" + UWEBase.sep + "LEVEL13.ST";
			break;
		default:
			text = Lev_Ark_File_Selected;
			break;
		}
		if (!DataLoader.ReadStreamFile(Loader.BasePath + text, out LevArk.lev_ark_file_data))
		{
			Debug.Log(Loader.BasePath + text + "File not loaded");
			Application.Quit();
		}
		switch (UWEBase._RES)
		{
		case "UW0":
			AutoMaps[0] = new AutoMap();
			AutoMaps[0].InitAutoMapDemo();
			break;
		case "UW1":
		{
			for (int m = 0; m <= AutoMaps.GetUpperBound(0); m++)
			{
				AutoMaps[m] = new AutoMap();
				AutoMaps[m].InitAutoMapUW1(m, LevArk.lev_ark_file_data);
			}
			break;
		}
		case "UW2":
		{
			for (int l = 0; l <= AutoMaps.GetUpperBound(0); l++)
			{
				AutoMaps[l] = new AutoMap();
				AutoMaps[l].InitAutoMapUW2(l, LevArk.lev_ark_file_data);
			}
			break;
		}
		}
		string rES2 = UWEBase._RES;
		if (rES2 != null && rES2 == "UW2")
		{
			events = new event_processor();
			if (whatTheHellIsThatFileFor != null)
			{
				whatTheHellIsThatFileFor.DumpScdArkInfo(SCD_Ark_File_Selected);
			}
		}
	}

	public void InitBGlobals(int SlotNo)
	{
		char[] buffer;
		if (SlotNo == 0)
		{
			if (DataLoader.ReadStreamFile(Loader.BasePath + "DATA" + UWEBase.sep + "BABGLOBS.DAT", out buffer))
			{
				int num = buffer.GetUpperBound(0) / 4;
				int num2 = 0;
				bGlobals = new bablGlobal[num + 1];
				for (int i = 0; i <= num; i++)
				{
					bGlobals[i].ConversationNo = (int)DataLoader.getValAtAddress(buffer, num2, 16);
					bGlobals[i].Size = (int)DataLoader.getValAtAddress(buffer, num2 + 2, 16);
					bGlobals[i].Globals = new int[bGlobals[i].Size];
					num2 += 4;
				}
			}
			return;
		}
		int num3 = 0;
		if (DataLoader.ReadStreamFile(Loader.BasePath + "DATA" + UWEBase.sep + "BABGLOBS.DAT", out buffer))
		{
			num3 = buffer.GetUpperBound(0) / 4;
			num3++;
		}
		if (!DataLoader.ReadStreamFile(Loader.BasePath + "SAVE" + SlotNo + UWEBase.sep + "BGLOBALS.DAT", out buffer))
		{
			return;
		}
		int num4 = 0;
		bGlobals = new bablGlobal[num3];
		for (int j = 0; j < num3; j++)
		{
			bGlobals[j].ConversationNo = (int)DataLoader.getValAtAddress(buffer, num4, 16);
			bGlobals[j].Size = (int)DataLoader.getValAtAddress(buffer, num4 + 2, 16);
			bGlobals[j].Globals = new int[bGlobals[j].Size];
			num4 += 4;
			for (int k = 0; k < bGlobals[j].Size; k++)
			{
				bGlobals[j].Globals[k] = (int)DataLoader.getValAtAddress(buffer, num4, 16);
				if (bGlobals[j].Globals[k] == 65535)
				{
					bGlobals[j].Globals[k] = 0;
				}
				num4 += 2;
			}
		}
	}

	public void WriteBGlobals(int SlotNo)
	{
		int num = 0;
		for (int i = 0; i <= bGlobals.GetUpperBound(0); i++)
		{
			num += 4;
			num += bGlobals[i].Size * 2;
		}
		byte[] array = new byte[num];
		int num2 = 0;
		for (int j = 0; j <= bGlobals.GetUpperBound(0); j++)
		{
			array[num2] = (byte)((uint)bGlobals[j].ConversationNo & 0xFFu);
			array[num2 + 1] = (byte)((uint)(bGlobals[j].ConversationNo >> 8) & 0xFFu);
			array[num2 + 2] = (byte)((uint)bGlobals[j].Size & 0xFFu);
			array[num2 + 3] = (byte)((uint)(bGlobals[j].Size >> 8) & 0xFFu);
			num2 += 4;
			for (int k = 0; k <= bGlobals[j].Globals.GetUpperBound(0); k++)
			{
				array[num2] = (byte)((uint)bGlobals[j].Globals[k] & 0xFFu);
				array[num2 + 1] = (byte)((uint)(bGlobals[j].Globals[k] >> 8) & 0xFFu);
				num2 += 2;
			}
		}
		File.WriteAllBytes(Loader.BasePath + "SAVE" + SlotNo + UWEBase.sep + "BGLOBALS.DAT", array);
	}

	public void SwitchTNovaMap(string levelFileName)
	{
		string path = ((!(levelFileName == "")) ? levelFileName : NovaLevelSelect.MapSelected);
		char[] buffer;
		DataLoader.Chunk data_ark;
		if (DataLoader.ReadStreamFile(path, out buffer) && DataLoader.LoadChunk(buffer, 86, out data_ark))
		{
			UWCharacter.Instance.playerCam.GetComponent<Light>().range = 200f;
			UWCharacter.Instance.playerCam.farClipPlane = 3000f;
			UWCharacter.Instance.playerCam.renderingPath = RenderingPath.DeferredShading;
			TileMapRenderer.RenderTNovaMap(TNovaLevelModel.transform, data_ark.data);
		}
	}

	private bool LoadConfigFile()
	{
#if UNITY_EDITOR		
		string path = Application.dataPath + UWEBase.sep + ".." + UWEBase.sep + "config.ini";
#else		
		string path = Path.Combine(Application.persistentDataPath, "config.ini");
#endif
		if (File.Exists(path))
		{
			StreamReader streamReader = new StreamReader(path, Encoding.Default);
			using (streamReader)
			{
				string text;
				do
				{
					text = streamReader.ReadLine();
					if (text == null || text.Length <= 1 || !(text.Substring(1, 1) != ";") || !text.Contains("="))
					{
						continue;
					}
					string[] array = text.Split('=');
					KeyCode value;
					KeyBindings.instance.chartoKeycode.TryGetValue(array[1].ToLower(), out value);
					switch (array[0].ToUpper())
					{
					case "MOUSEX":
					{
						float result4 = 15f;
						if (float.TryParse(array[1], out result4))
						{
							MouseX.sensitivityX = result4;
						}
						break;
					}
					case "MOUSEY":
					{
						float result2 = 15f;
						if (float.TryParse(array[1], out result2))
						{
							MouseY.sensitivityY = result2;
						}
						break;
					}
					case "PATH_UW0":
						path_uw0 = UWClass.CleanPath(array[1]);
						break;
					case "PATH_UW1":
						path_uw1 = UWClass.CleanPath(array[1]);
						break;
					case "PATH_UW2":
						path_uw2 = UWClass.CleanPath(array[1]);
						break;
					case "PATH_SHOCK":
						path_shock = UWClass.CleanPath(array[1]);
						break;
					case "PATH_TNOVA":
						path_tnova = UWClass.CleanPath(array[1]);
						break;
					case "FLYUP":
						KeyBindings.instance.FlyUp = value;
						break;
					case "FLYDOWN":
						KeyBindings.instance.FlyDown = value;
						break;
					case "TOGGLEMOUSELOOK":
						KeyBindings.instance.ToggleMouseLook = value;
						break;
					case "TOGGLEFULLSCREEN":
						KeyBindings.instance.ToggleFullScreen = value;
						break;
					case "INTERACTIONOPTIONS":
						KeyBindings.instance.InteractionOptions = value;
						break;
					case "INTERACTIONTALK":
						KeyBindings.instance.InteractionTalk = value;
						break;
					case "INTERACTIONPICKUP":
						KeyBindings.instance.InteractionPickup = value;
						break;
					case "INTERACTIONLOOK":
						KeyBindings.instance.InteractionLook = value;
						break;
					case "INTERACTIONATTACK":
						KeyBindings.instance.InteractionAttack = value;
						break;
					case "INTERACTIONUSE":
						KeyBindings.instance.InteractionUse = value;
						break;
					case "CASTSPELL":
						KeyBindings.instance.CastSpell = value;
						break;
					case "TRACKSKILL":
						KeyBindings.instance.TrackSkill = value;
						break;
					case "DEFAULTLIGHTLEVEL":
					{
						float result3 = 16f;
						if (float.TryParse(array[1], out result3))
						{
							LightSource.BaseBrightness = result3;
						}
						break;
					}
					case "FOV":
					{
						float result = 75f;
						if (float.TryParse(array[1], out result))
						{
							Camera.main.fieldOfView = result;
						}
						break;
					}
					case "INFINITEMANA":
						Magic.InfiniteMana = array[1] == "1";
						break;
					case "GODMODE":
						Character.Invincible = array[1] == "1";
						break;
					case "CONTEXTUIENABLED":
						WindowDetect.ContextUIEnabled = array[1] == "1";
						break;
					case "UW1_SOUNDBANK":
						MusicController.UW1Path = UWClass.CleanPath(array[1]);
						break;
					case "UW2_SOUNDBANK":
						MusicController.UW2Path = UWClass.CleanPath(array[1]);
						break;
					case "GENREPORT":
						CreateReports = array[1] == "1";
						break;
					case "SHOWINUSE":
						ShowOnlyInUse = array[1] == "1";
						break;
					case "AUTOKEYUSE":
						Character.AutoKeyUse = array[1] == "1";
						break;
					}
				}
				while (text != null);
				streamReader.Close();
				return true;
			}
		}
		return false;
	}

	private void CreateObjectReport(ObjectLoaderInfo[] objList, int ReportLevelNo)
	{
		StreamWriter streamWriter = new StreamWriter(Application.dataPath + "//..//_objectreport.xml");
		streamWriter.WriteLine("<ObjectReport level =" + ReportLevelNo + "> ");
		for (int i = 0; i <= objList.GetUpperBound(0); i++)
		{
			if ((objList[i].InUseFlag == 0 && !ShowOnlyInUse) || objList[i].InUseFlag == 1)
			{
				WriteObjectXML(objList, streamWriter, i);
			}
		}
		streamWriter.WriteLine("</ObjectReport>");
		streamWriter.Close();
	}

	private static void WriteObjectXML(ObjectLoaderInfo[] objList, StreamWriter writer, int o)
	{
		writer.WriteLine("\t<Object>");
		writer.WriteLine("\t\t<ObjectName>" + ObjectLoader.UniqueObjectNameEditor(objList[o]) + "</ObjectName>");
		writer.WriteLine("\t\t<Index>" + o + "</Index>");
		writer.WriteLine("\t\t<Address>" + objList[o].address + "</Address>");
		writer.WriteLine("\t\t<StaticProperties>");
		writer.WriteLine("\t\t\t<ItemID>" + objList[o].item_id + "</ItemID>");
		writer.WriteLine("\t\t\t<InUse>" + objList[o].InUseFlag + "</InUse>");
		writer.WriteLine("\t\t\t<Flags>" + objList[o].flags + "</Flags>");
		writer.WriteLine("\t\t\t<Enchant>" + objList[o].enchantment + "</Enchant>");
		writer.WriteLine("\t\t\t<DoorDir>" + objList[o].doordir + "</DoorDir>");
		writer.WriteLine("\t\t\t<Invis>" + objList[o].invis + "</Invis>");
		writer.WriteLine("\t\t\t<IsQuant>" + objList[o].is_quant + "</IsQuant>");
		writer.WriteLine("\t\t\t<Texture>" + objList[o].texture + "</Texture>");
		writer.WriteLine("\t\t\t<Position>");
		writer.WriteLine("\t\t\t\t<ObjectTileX>" + objList[o].ObjectTileX + "</ObjectTileX>");
		writer.WriteLine("\t\t\t\t<ObjectTileY>" + objList[o].ObjectTileY + "</ObjectTileY>");
		writer.WriteLine("\t\t\t\t<heading>" + objList[o].heading + "</heading>");
		writer.WriteLine("\t\t\t\t<xpos>" + objList[o].xpos + "</xpos>");
		writer.WriteLine("\t\t\t\t<ypos>" + objList[o].ypos + "</ypos>");
		writer.WriteLine("\t\t\t\t<zpos>" + objList[o].zpos + "</zpos>");
		writer.WriteLine("\t\t\t</Position>");
		writer.WriteLine("\t\t\t<Quality>" + objList[o].quality + "</Quality>");
		writer.WriteLine("\t\t\t<Next>" + objList[o].next + "</Next>");
		writer.WriteLine("\t\t\t<Owner>" + objList[o].owner + "</Owner>");
		writer.WriteLine("\t\t\t<Link>" + objList[o].link + "</Link>");
		writer.WriteLine("\t\t</StaticProperties>");
		if (o < 256)
		{
			writer.WriteLine("\t\t<MobileProperties>");
			writer.WriteLine("\t\t\t<npc_hp>" + objList[o].npc_hp + "</npc_hp>");
			writer.WriteLine("\t\t\t<ProjectileHeadingMinor>" + objList[o].ProjectileHeadingMinor + "</ProjectileHeadingMinor>");
			writer.WriteLine("\t\t\t<ProjectileHeadingMajor>" + objList[o].ProjectileHeadingMajor + "</ProjectileHeadingMajor>");
			writer.WriteLine("\t\t\t<MobileUnk01>" + objList[o].MobileUnk01 + "</MobileUnk01>");
			writer.WriteLine("\t\t\t<npc_goal>" + objList[o].npc_goal + "</npc_goal>");
			writer.WriteLine("\t\t\t<npc_gtarg>" + objList[o].npc_gtarg + "</npc_gtarg>");
			writer.WriteLine("\t\t\t<MobileUnk02>" + objList[o].MobileUnk02 + "</MobileUnk02>");
			writer.WriteLine("\t\t\t<npc_level>" + objList[o].npc_level + "</npc_level>");
			writer.WriteLine("\t\t\t<MobileUnk03>" + objList[o].MobileUnk03 + "</MobileUnk03>");
			writer.WriteLine("\t\t\t<MobileUnk04>" + objList[o].MobileUnk04 + "</MobileUnk04>");
			writer.WriteLine("\t\t\t<npc_talkedto>" + objList[o].npc_talkedto + "</npc_talkedto>");
			writer.WriteLine("\t\t\t<npc_attitude>" + objList[o].npc_attitude + "</npc_attitude>");
			writer.WriteLine("\t\t\t<MobileUnk05>" + objList[o].MobileUnk05 + "</MobileUnk05>");
			writer.WriteLine("\t\t\t<npc_height>" + objList[o].npc_height + "</npc_height>");
			writer.WriteLine("\t\t\t<MobileUnk06>" + objList[o].MobileUnk06 + "</MobileUnk06>");
			writer.WriteLine("\t\t\t<MobileUnk07>" + objList[o].MobileUnk07 + "</MobileUnk07>");
			writer.WriteLine("\t\t\t<MobileUnk08>" + objList[o].MobileUnk08 + "</MobileUnk08>");
			writer.WriteLine("\t\t\t<MobileUnk09>" + objList[o].MobileUnk09 + "</MobileUnk09>");
			writer.WriteLine("\t\t\t<Projectile_Speed>" + objList[o].Projectile_Speed + "</Projectile_Speed>");
			writer.WriteLine("\t\t\t<Projectile_Pitch>" + objList[o].Projectile_Pitch + "</Projectile_Pitch>");
			writer.WriteLine("\t\t\t<Projectile_Sign>" + objList[o].Projectile_Sign + "</Projectile_Sign>");
			writer.WriteLine("\t\t\t<npc_voidanim>" + objList[o].npc_voidanim + "</npc_voidanim>");
			writer.WriteLine("\t\t\t<MobileUnk11>" + objList[o].MobileUnk11 + "</MobileUnk11>");
			writer.WriteLine("\t\t\t<MobileUnk12>" + objList[o].MobileUnk12 + "</MobileUnk12>");
			writer.WriteLine("\t\t\t<npc_yhome>" + objList[o].npc_yhome + "</npc_yhome>");
			writer.WriteLine("\t\t\t<npc_xhome>" + objList[o].npc_xhome + "</npc_xhome>");
			writer.WriteLine("\t\t\t<npc_heading>" + objList[o].npc_heading + "</npc_heading>");
			writer.WriteLine("\t\t\t<MobileUnk13>" + objList[o].MobileUnk13 + "</MobileUnk13>");
			writer.WriteLine("\t\t\t<npc_hunger>" + objList[o].npc_hunger + "</npc_hunger>");
			writer.WriteLine("\t\t\t<MobileUnk14>" + objList[o].MobileUnk14 + "</MobileUnk14>");
			writer.WriteLine("\t\t\t<npc_whoami>" + objList[o].npc_whoami + "</npc_whoami>");
			writer.WriteLine("\t\t</MobileProperties>");
		}
		writer.WriteLine("\t</Object>");
	}

	public static Worlds GetWorld(int levelNo)
	{
		if (UWEBase._RES != "UW2")
		{
			return Worlds.Britannia;
		}
		switch ((UW2_LevelNos)levelNo)
		{
		case UW2_LevelNos.Britannia0:
		case UW2_LevelNos.Britannia1:
		case UW2_LevelNos.Britannia2:
		case UW2_LevelNos.Britannia3:
		case UW2_LevelNos.Britannia4:
			return Worlds.Britannia;
		case UW2_LevelNos.Prison0:
		case UW2_LevelNos.Prison1:
		case UW2_LevelNos.Prison2:
		case UW2_LevelNos.Prison3:
		case UW2_LevelNos.Prison4:
		case UW2_LevelNos.Prison5:
		case UW2_LevelNos.Prison6:
		case UW2_LevelNos.Prison7:
			return Worlds.PrisonTower;
		case UW2_LevelNos.Killorn0:
		case UW2_LevelNos.Killorn1:
			return Worlds.Killorn;
		case UW2_LevelNos.Ice0:
		case UW2_LevelNos.Ice1:
			return Worlds.Ice;
		case UW2_LevelNos.Talorus0:
		case UW2_LevelNos.Talorus1:
			return Worlds.Talorus;
		case UW2_LevelNos.Academy0:
		case UW2_LevelNos.Academy1:
		case UW2_LevelNos.Academy2:
		case UW2_LevelNos.Academy3:
		case UW2_LevelNos.Academy4:
		case UW2_LevelNos.Academy5:
		case UW2_LevelNos.Academy6:
		case UW2_LevelNos.Academy7:
			return Worlds.Academy;
		case UW2_LevelNos.Tomb0:
		case UW2_LevelNos.Tomb1:
		case UW2_LevelNos.Tomb2:
		case UW2_LevelNos.Tomb3:
			return Worlds.Tomb;
		case UW2_LevelNos.Pits0:
		case UW2_LevelNos.Pits1:
		case UW2_LevelNos.Pits2:
			return Worlds.Pits;
		case UW2_LevelNos.Ethereal0:
		case UW2_LevelNos.Ethereal1:
		case UW2_LevelNos.Ethereal2:
		case UW2_LevelNos.Ethereal3:
		case UW2_LevelNos.Ethereal4:
		case UW2_LevelNos.Ethereal5:
		case UW2_LevelNos.Ethereal6:
		case UW2_LevelNos.Ethereal7:
		case UW2_LevelNos.Ethereal8:
			return Worlds.Ethereal;
		default:
			Debug.Log("Unknown level/world");
			return Worlds.Ethereal;
		}
	}
}
