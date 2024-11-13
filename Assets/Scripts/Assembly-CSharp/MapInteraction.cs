using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapInteraction : GuiBase
{
	public MapWorldSelect[] MapSelectButtons = new MapWorldSelect[9];

	public const int MapInteractionNormal = 0;

	public const int MapInteractionDelete = 1;

	public const int MapInteractionWriting = 2;

	public static int MapNo;

	public static Vector2 caretAdjustment = Vector2.zero;

	private Text mapNoteCurrent;

	public InputField MapNoteInput;

	private Vector2 pos;

	public static Vector2 CursorPos;

	public static int InteractionMode;

	public GameWorldController.Worlds CurrentWorld = GameWorldController.Worlds.Britannia;

	public static MapInteraction instance;

	private void Awake()
	{
		instance = this;
		if (UWEBase._RES != "UW2")
		{
			for (int i = 0; i <= MapSelectButtons.GetUpperBound(0); i++)
			{
				if (MapSelectButtons[i] != null)
				{
					UWHUD.instance.EnableDisableControl(MapSelectButtons[i].gameObject, false);
				}
			}
		}
		else
		{
			InitMapButtons(MapSelectButtons);
		}
	}

	public static void InitMapButtons(MapWorldSelect[] Buttons)
	{
		GRLoader gRLoader = new GRLoader(32, 3);
		for (int i = 0; i <= Buttons.GetUpperBound(0); i++)
		{
			if (Buttons[i] != null)
			{
				switch (Buttons[i].world)
				{
				case GameWorldController.Worlds.Britannia:
					Buttons[i].ButtonOff = ArtLoader.CreateBlankImage(29, 29);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(16);
					break;
				case GameWorldController.Worlds.PrisonTower:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(0);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(8);
					break;
				case GameWorldController.Worlds.Killorn:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(1);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(9);
					break;
				case GameWorldController.Worlds.Ice:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(2);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(10);
					break;
				case GameWorldController.Worlds.Talorus:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(3);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(11);
					break;
				case GameWorldController.Worlds.Academy:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(4);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(12);
					break;
				case GameWorldController.Worlds.Pits:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(6);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(14);
					break;
				case GameWorldController.Worlds.Tomb:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(5);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(13);
					break;
				case GameWorldController.Worlds.Ethereal:
					Buttons[i].ButtonOff = gRLoader.LoadImageAt(7);
					Buttons[i].ButtonOn = gRLoader.LoadImageAt(15);
					break;
				}
			}
		}
	}

	public void MapClose()
	{
		Time.timeScale = 1f;
		WindowDetect.InMap = false;
		UWCharacter.Instance.playerMotor.jumping.enabled = true;
		InventorySlot.Hovering = false;
		if (MusicController.instance != null)
		{
			MusicController.instance.InMap = false;
		}
		UWHUD.instance.RefreshPanels(0);
		UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
	}

	public void MapUp()
	{
		if (MapNo > 0 && MapNo > (int)CurrentWorld)
		{
			MapNo--;
			UpdateMap(MapNo);
		}
	}

	public void MapDown()
	{
		if (MapNo < GameWorldController.instance.AutoMaps.GetUpperBound(0) && MapNo < MaxWorld(MapNo))
		{
			MapNo++;
			UpdateMap(MapNo);
		}
	}

	public static void UpdateMap(int LevelNo)
	{
		WindowDetect.InMap = true;
		instance.CurrentWorld = GameWorldController.GetWorld(LevelNo);
		UWHUD.instance.LevelNoDisplay.text = string.Concat(((int)(1 + (LevelNo - instance.CurrentWorld))).ToString(), " ", instance.CurrentWorld, " (", LevelNo, ")");
		if (UWEBase._RES == "UW2")
		{
			for (int i = 0; i <= instance.MapSelectButtons.GetUpperBound(0); i++)
			{
				if (instance.MapSelectButtons[i] != null)
				{
					if (instance.MapSelectButtons[i].world == instance.CurrentWorld)
					{
						instance.MapSelectButtons[i].SetOn();
					}
					else
					{
						instance.MapSelectButtons[i].SetOff();
					}
				}
			}
		}
		MapNo = LevelNo;
		UWHUD.instance.CursorIcon = UWHUD.instance.MapQuill;
		UWHUD.instance.MapDisplay.texture = GameWorldController.instance.AutoMaps[MapNo].TileMapImage();
		foreach (Transform item in UWHUD.instance.MapPanel.transform)
		{
			if (item.name.Substring(0, 4) == "_Map")
			{
				Object.Destroy(item.transform.gameObject);
			}
		}
		if (GameWorldController.instance.AutoMaps[MapNo] != null && GameWorldController.instance.AutoMaps[MapNo].MapNotes != null)
		{
			for (int j = 0; j < GameWorldController.instance.AutoMaps[MapNo].MapNotes.Count; j++)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Prefabs/_MapNoteTemplate"));
				gameObject.name = "_Map-Note Number " + j;
				gameObject.transform.parent = UWHUD.instance.MapPanel.transform;
				gameObject.GetComponent<Text>().text = GameWorldController.instance.AutoMaps[MapNo].MapNotes[j].NoteText;
				gameObject.GetComponent<RectTransform>().anchoredPosition = GameWorldController.instance.AutoMaps[MapNo].MapNotes[j].NotePosition();
				gameObject.GetComponent<MapNoteId>().guid = GameWorldController.instance.AutoMaps[MapNo].MapNotes[j].guid;
				gameObject.GetComponent<RectTransform>().SetSiblingIndex(4);
			}
		}
	}

	public void ClickEraser()
	{
		if (InteractionMode == 0)
		{
			InteractionMode = 1;
			UWHUD.instance.CursorIcon = UWHUD.instance.MapEraser;
		}
		else
		{
			InteractionMode = 0;
			UWHUD.instance.CursorIcon = UWHUD.instance.MapQuill;
		}
	}

	public void OnClick(BaseEventData evnt)
	{
		switch (InteractionMode)
		{
		case 0:
		{
			InteractionMode = 2;
			UWHUD.instance.CursorIcon = UWHUD.instance.MapQuillWriting;
			pos = Vector2.zero;
			RectTransform component = GetComponent<RectTransform>();
			PointerEventData pointerEventData = (PointerEventData)evnt;
			CursorPos = UWHUD.instance.window.CursorPosition.center;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(component, pointerEventData.pressPosition, pointerEventData.pressEventCamera, out pos);
			pos += new Vector2(150f, -4f);
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Prefabs/_MapNoteTemplate"));
			mapNoteCurrent = gameObject.GetComponent<Text>();
			mapNoteCurrent.transform.parent = base.transform;
			mapNoteCurrent.GetComponent<RectTransform>().anchoredPosition = pos;
			gameObject.GetComponent<RectTransform>().SetSiblingIndex(4);
			MapNoteInput.textComponent = mapNoteCurrent;
			MapNoteInput.text = "";
			MapNoteInput.Select();
			break;
		}
		case 1:
			InteractionMode = 0;
			UWHUD.instance.CursorIcon = UWHUD.instance.MapQuill;
			break;
		case 2:
			OnNoteComplete();
			break;
		}
	}

	public void OnNoteComplete()
	{
		InteractionMode = 0;
		if (MapNoteInput.text.TrimEnd() == "")
		{
			Object.Destroy(mapNoteCurrent.gameObject);
		}
		else
		{
			MapNote mapNote = new MapNote((int)pos.x, (int)(pos.y + 100f), MapNoteInput.text);
			mapNoteCurrent.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			GameWorldController.instance.AutoMaps[MapNo].MapNotes.Add(mapNote);
			mapNoteCurrent.GetComponent<MapNoteId>().guid = mapNote.guid;
		}
		InteractionMode = 0;
		caretAdjustment = Vector2.zero;
		UWHUD.instance.CursorIcon = UWHUD.instance.MapQuill;
	}

	public void OnLetterType()
	{
		caretAdjustment = new Vector2((float)MapNoteInput.text.Length * 9f, 0f);
	}

	public override void Update()
	{
		base.Update();
		if (InteractionMode == 2)
		{
			MapNoteInput.Select();
		}
	}

	private int MaxWorld(int levelNo)
	{
		switch (GameWorldController.GetWorld(levelNo))
		{
		case GameWorldController.Worlds.Britannia:
			if (UWEBase._RES == "UW2")
			{
				return 4;
			}
			return GameWorldController.instance.AutoMaps.GetUpperBound(0);
		case GameWorldController.Worlds.PrisonTower:
			return 15;
		case GameWorldController.Worlds.Killorn:
			return 17;
		case GameWorldController.Worlds.Ice:
			return 25;
		case GameWorldController.Worlds.Talorus:
			return 33;
		case GameWorldController.Worlds.Academy:
			return 47;
		case GameWorldController.Worlds.Tomb:
			return 51;
		case GameWorldController.Worlds.Pits:
			return 58;
		default:
			return GameWorldController.instance.AutoMaps.GetUpperBound(0);
		}
	}
}
