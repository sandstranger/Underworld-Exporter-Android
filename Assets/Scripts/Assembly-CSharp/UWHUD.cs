using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UWHUD : HUD
{
	public const int HUD_MODE_INVENTORY = 0;

	public const int HUD_MODE_STATS = 1;

	public const int HUD_MODE_RUNES = 2;

	public const int HUD_MODE_CONV = 3;

	public const int HUD_MODE_MAP = 4;

	public const int HUD_MODE_CUTS_SMALL = 5;

	public const int HUD_MODE_CUTS_FULL = 6;

	public int CURRENT_HUD_MODE = 0;

	public static UWHUD instance;

	[Header("Panel States")]
	public bool InventoryEnabled = true;

	public bool RuneBagEnabled = false;

	public bool StatsEnabled = false;

	public bool ConversationEnabled = false;

	public bool CutSceneSmallEnabled = true;

	public bool CutSceneFullEnabled = true;

	public bool MapEnabled = false;

	public bool isRotating;

	[Header("Anim Controls")]
	public WeaponAnimationPlayer wpa;

	public CutsceneAnimationFullscreen CutScenesFull;

	public HudAnimation CutScenesSmall;

	[Header("Input Output")]
	public ScrollController MessageScroll;

	public InputField InputControl;

	public GameObject MessageLogScrollEdgeLeft;

	public GameObject MessageLogScrollEdgeRight;

	public GameObject MessageScrollBackground;

	public GameObject MessageScrollDrag;

	public Text ErrorText;

	[Header("Main Windows")]
	public GameObject main_windowUW1;

	public GameObject main_windowUW2;

	public WindowDetectUW window;

	public RawImage mapBackground;

	public RawImage mainwindow_art;

	public GameObject SpeedStartButton;

	public GameObject EditorButton;

	[Header("TopLevelUIs")]
	public GameObject gameUi;

	public GameObject gameSelectUi;

	public MainMenuHud mainmenu;

	[Header("Conversation")]
	public ScrollController Conversation_tl;

	public TradeSlot[] playerTrade;

	public TradeSlot[] npcTrade;

	public RawImage[] ConversationPortraits;

	public Text PCName;

	public Text NPCName;

	public RawImage npcTradeArea;

	public RawImage pcTradeArea;

	public RawImage ConvNPCPortraitBG;

	public RawImage ConvPCPortraitBG;

	public RawImage ConvPCTitleBG;

	public RawImage ConvNPCTitleBG;

	public RawImage ConvTextScrollTop;

	public RawImage ConvTextScrollBottom;

	public RawImage UW2ConversationBG;

	public GameObject UW1ScrollTop;

	public GameObject UW1ScrollBottom;

	public GameObject UW1ScrlEdgeLeft1;

	public GameObject UW1ScrlEdgeLeft2;

	public GameObject UW1ScrlEdgeLeft3;

	public GameObject UW1ScrlEdgeRight1;

	public GameObject UW1ScrlEdgeRight2;

	public GameObject UW1ScrlEdgeRight3;

	public GameObject UW1PortraitFramePC;

	public GameObject UW1PortraitFrameNPC;

	public GameObject UW1PCNameFrame;

	public GameObject UW1NPCNameFrame;

	public GameObject UW1PCTradeArea;

	public GameObject UW1NPCTradeArea;

	public GameObject UW1ConversationPaperBackground;

	public ConversationButton[] ConversationOptions = new ConversationButton[5];

	[Header("Inventory")]
	public RawImage playerBody;

	public Text Encumberance;

	public GameObject ContainerOpened;

	public RawImage Helm_f_Slot;

	public RawImage Chest_f_Slot;

	public RawImage Legs_f_Slot;

	public RawImage Boots_f_Slot;

	public RawImage Gloves_f_Slot;

	public RawImage Helm_m_Slot;

	public RawImage Chest_m_Slot;

	public RawImage Legs_m_Slot;

	public RawImage Boots_m_Slot;

	public RawImage Gloves_m_Slot;

	public RawImage LeftHand_Slot;

	public RawImage RightHand_Slot;

	public RawImage LeftRing_Slot;

	public RawImage RightRing_Slot;

	public RawImage LeftShoulder_Slot;

	public RawImage RightShoulder_Slot;

	public RawImage[] BackPack_Slot = new RawImage[8];

	public Text LeftHand_Qty;

	public Text RightHand_Qty;

	public Text LeftShoulder_Qty;

	public Text RightShoulder_Qty;

	public Text[] Backpack_Slot_Qty = new Text[8];

	public ScrollButtonInventory InvUp;

	public ScrollButtonInventory InvDown;

	[Header("Buttons and Labels")]
	public chains ChainsControl;

	public Text ContextMenu;

	[Header("Map")]
	public Text LevelNoDisplay;

	public RawImage MapDisplay;

	public Texture2D MapQuill;

	public Texture2D MapQuillWriting;

	public Texture2D MapEraser;

	public RectTransform MapUp;

	public RectTransform MapDown;

	public RectTransform MapEraserButton;

	public RectTransform MapClose;

	public RectTransform WorldSelect;

	public MapWorldSelect[] InWorldGemSelect = new MapWorldSelect[9];

	[Header("Magic")]
	public ActiveRuneSlot[] activeRunes;

	public RuneSlot[] runes;

	public SpellEffectsDisplay[] spelleffectdisplay;

	[Header("Panels")]
	public GameObject RuneBagPanel;

	public GameObject StatsDisplayPanel;

	public GameObject MapPanel;

	public GameObject InventoryPanel;

	public GameObject PaperDollFemalePanel;

	public GameObject PaperDollMalePanel;

	public GameObject ConversationPanel;

	public GameObject DragonLeftPanel;

	public GameObject DragonRightPanel;

	public GameObject CutsceneSmallPanel;

	public GameObject CutsceneFullPanel;

	public GameObject currentPanel;

	public GameObject editorPanel;

	[Header("Indicators")]
	public HealthFlask FlaskHealth;

	public HealthFlask FlaskMana;

	public Eyes MonsterEyes;

	public Compass HudCompass;

	public Power powergem;

	[Header("Ingame Editor")]
	public Text editorButtonLabel;

	public IngameEditor editor;

	[Header("Interaction Control")]
	public InteractionModeControl InteractionControlUW1;

	public InteractionModeControl InteractionControlUW2;

	public RawImage InteractionControlUW2BG;

	private void Awake()
	{
		instance = this;
		EnableDisableControl(SpeedStartButton, Application.isEditor);
		EnableDisableControl(EditorButton, Application.isEditor);
	}

	public void Begin()
	{
		gameUi.SetActive(true);
		gameSelectUi.SetActive(false);
		mapBackground.texture = GameWorldController.instance.bytloader.LoadImageAt(0);
		base.CursorIcon = GameWorldController.instance.grCursors.LoadImageAt(0);
		CursorIconDefault = GameWorldController.instance.grCursors.LoadImageAt(0);
		CursorIconTarget = GameWorldController.instance.grCursors.LoadImageAt(9);
		MapQuill = GameWorldController.instance.grCursors.LoadImageAt(14);
		MapQuillWriting = GameWorldController.instance.grCursors.LoadImageAt(12);
		MapEraser = GameWorldController.instance.grCursors.LoadImageAt(13);
		GRLoader gRLoader = new GRLoader(24);
		InventoryPanel.GetComponent<RawImage>().texture = gRLoader.LoadImageAt(0);
		RuneBagPanel.GetComponent<RawImage>().texture = gRLoader.LoadImageAt(1);
		StatsDisplayPanel.GetComponent<RawImage>().texture = gRLoader.LoadImageAt(2);
		instance.EnableDisableControl(instance.main_windowUW1, UWEBase._RES != "UW2");
		instance.EnableDisableControl(instance.main_windowUW2, UWEBase._RES == "UW2");
		instance.EnableDisableControl(instance.InteractionControlUW1.gameObject, UWEBase._RES != "UW2");
		instance.EnableDisableControl(instance.InteractionControlUW2.gameObject, UWEBase._RES == "UW2");
		MapPanel.transform.SetAsLastSibling();
		ConversationPanel.transform.SetAsLastSibling();
		GRLoader gRLoader2 = new GRLoader(5);
		if (gRLoader2 != null)
		{
			InvUp.GetComponent<RawImage>().texture = gRLoader2.LoadImageAt(27);
			InvDown.GetComponent<RawImage>().texture = gRLoader2.LoadImageAt(28);
		}
		if (UWEBase._RES == "UW2")
		{
			MessageScroll.LineWidth = 54;
			Conversation_tl.LineWidth = 48;
		}
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			SetUIElementPosition(window, 128f, 210f, new Vector2(-40.8f, 20f));
			SetUIElementPosition(HudCompass, 16f, 52f, new Vector2(-40f, -56f));
			SetUIElementPosition(HudCompass.NorthIndicators[0], 4f, 10f, new Vector2(0.020000458f, 5.9799995f));
			SetUIElementPosition(HudCompass.NorthIndicators[1], 4f, 10f, new Vector2(7.0099983f, 5.99f));
			SetUIElementPosition(HudCompass.NorthIndicators[2], 5f, 5f, new Vector2(14.459999f, 4.51f));
			SetUIElementPosition(HudCompass.NorthIndicators[3], 4f, 10f, new Vector2(18f, 4.01f));
			SetUIElementPosition(HudCompass.NorthIndicators[4], 5f, 7f, new Vector2(22.52f, 0.5f));
			SetUIElementPosition(HudCompass.NorthIndicators[5], 6f, 9f, new Vector2(20.509998f, -0.98f));
			SetUIElementPosition(HudCompass.NorthIndicators[6], 5f, 8f, new Vector2(16f, -3.5f));
			SetUIElementPosition(HudCompass.NorthIndicators[7], 5f, 11f, new Vector2(7.4900017f, -4.5f));
			SetUIElementPosition(HudCompass.NorthIndicators[8], 5f, 12f, new Vector2(0f, -5.51f));
			SetUIElementPosition(HudCompass.NorthIndicators[9], 5f, 11f, new Vector2(-7.5f, -4.51f));
			SetUIElementPosition(HudCompass.NorthIndicators[10], 5f, 8f, new Vector2(-16.01f, -3.5100002f));
			SetUIElementPosition(HudCompass.NorthIndicators[11], 6f, 10f, new Vector2(-20.96f, -0.9899998f));
			SetUIElementPosition(HudCompass.NorthIndicators[12], 5f, 7f, new Vector2(-22.51f, 0.48999977f));
			SetUIElementPosition(HudCompass.NorthIndicators[13], 5f, 8f, new Vector2(-20.02f, 2.5100002f));
			SetUIElementPosition(HudCompass.NorthIndicators[14], 6f, 8f, new Vector2(-14.01f, 3.9700003f));
			SetUIElementPosition(HudCompass.NorthIndicators[15], 5f, 10f, new Vector2(-6.9699993f, 5.5f));
			SetUIElementPosition(powergem, 5f, 14f, new Vector2(-40f, -53.79f));
			powergem.transform.parent.GetComponent<RectTransform>().SetSiblingIndex(HudCompass.GetComponent<RectTransform>().GetSiblingIndex() + 1);
			SetUIElementPosition(ChainsControl, 32f, 15f, new Vector2(119.7f, -35f));
			FlaskMana.GetComponent<RectTransform>().anchoredPosition = new Vector2(139.5f, -48.4f);
			FlaskHealth.GetComponent<RectTransform>().anchoredPosition = new Vector2(100.14f, -48.5f);
			activeRunes[0].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(1.9f, -11.6f);
			spelleffectdisplay[0].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-35f, -11.6f);
			SetUIElementPosition(InventoryPanel, 112f, 79f, new Vector2(114.6f, 36.7f));
			SetUIElementPosition(RuneBagPanel, 112f, 79f, new Vector2(114.6f, 36.7f));
			SetUIElementPosition(StatsDisplayPanel, 112f, 79f, new Vector2(114.6f, 36.7f));
			SetUIElementPosition(playerBody, 69f, 36f, new Vector2(1.78f, 20.51f));
			SetUIElementPosition(RightShoulder_Slot, 16f, 16f, new Vector2(-22.2f, 43.100002f));
			SetUIElementPosition(LeftShoulder_Slot, 16f, 16f, new Vector2(26.300003f, 43.100002f));
			SetUIElementPosition(RightHand_Slot, 16f, 16f, new Vector2(-26.2f, 20.7f));
			SetUIElementPosition(LeftHand_Slot, 16f, 16f, new Vector2(29.300003f, 20.600002f));
			SetUIElementPosition(BackPack_Slot[0], 16f, 16f, new Vector2(-27.1f, -25.900002f));
			SetUIElementPosition(BackPack_Slot[1], 16f, 16f, new Vector2(-8.1f, -25.900002f));
			SetUIElementPosition(BackPack_Slot[2], 16f, 16f, new Vector2(10.900002f, -25.900002f));
			SetUIElementPosition(BackPack_Slot[3], 16f, 16f, new Vector2(29.900002f, -25.900002f));
			SetUIElementPosition(BackPack_Slot[4], 16f, 16f, new Vector2(-27.1f, -43.9f));
			SetUIElementPosition(BackPack_Slot[5], 16f, 16f, new Vector2(-8.1f, -43.9f));
			SetUIElementPosition(BackPack_Slot[6], 16f, 16f, new Vector2(10.900002f, -43.9f));
			SetUIElementPosition(BackPack_Slot[7], 16f, 16f, new Vector2(29.900002f, -43.9f));
			SetUIElementPosition(ContainerOpened, 16f, 16f, new Vector2(-26f, -4.8f));
			SetUIElementPosition(RightRing_Slot, 8f, 8f, new Vector2(-13.71f, 5.01f));
			SetUIElementPosition(LeftRing_Slot, 8f, 8f, new Vector2(17.69f, 5.01f));
			SetUIElementPosition(MonsterEyes, 3f, 20f, new Vector2(-40.06f, 94.48f));
			SetUIElementPosition(Legs_f_Slot, 51f, 19f, new Vector2(118.999985f, 52.000008f));
			SetUIElementPosition(Chest_f_Slot, 44f, 33f, new Vector2(119.999985f, 57.000008f));
			SetUIElementPosition(Helm_f_Slot, 20f, 20f, new Vector2(118.55f, 82.03001f));
			SetUIElementPosition(Gloves_f_Slot, 15f, 33f, new Vector2(119.06999f, 50.4f));
			SetUIElementPosition(Boots_f_Slot, 14f, 21f, new Vector2(117.999985f, 28.300003f));
			SetUIElementPosition(Legs_m_Slot, 51f, 19f, new Vector2(118.999985f, 52.000008f));
			SetUIElementPosition(Chest_m_Slot, 44f, 33f, new Vector2(119.999985f, 57.000008f));
			SetUIElementPosition(Helm_m_Slot, 20f, 20f, new Vector2(118.55f, 82.03001f));
			SetUIElementPosition(Gloves_m_Slot, 15f, 33f, new Vector2(119.06999f, 50.4f));
			SetUIElementPosition(Boots_m_Slot, 14f, 21f, new Vector2(117.999985f, 28.300003f));
			SetUIElementPosition(ContainerOpened.GetComponent<ContainerOpened>().BackpackBg, 0.9f, -34.4f, new Vector2(40.9f, 21.7f));
			MessageLogScrollEdgeRight.GetComponent<RectTransform>().anchoredPosition = new Vector2(65.87f, -64.4f);
			MessageScrollBackground.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 208.68f);
			MessageScrollBackground.GetComponent<RectTransform>().anchoredPosition = new Vector2(-40.8f, -64.8f);
			MessageScrollDrag.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 216f);
			MessageScrollDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(-41.2f, -64.5f);
			SetUIElementPosition(MessageScroll, 33f, 174f, new Vector2(-40.93f, -62.4f));
			SetUIElementPosition(Conversation_tl, 80f, 198f, new Vector2(-38.54f, -19.9f));
			SetUIElementPosition(ConversationPortraits[0], 70f, 64f, new Vector2(39.9f, 62f));
			SetUIElementPosition(ConversationPortraits[1], 70f, 64f, new Vector2(-125.9f, 62f));
			PCName.GetComponent<RectTransform>().anchoredPosition = new Vector2(-37.6f, 97.871f);
			PCName.alignment = TextAnchor.MiddleRight;
			NPCName.GetComponent<RectTransform>().anchoredPosition = new Vector2(-46.7f, 29.9f);
			NPCName.alignment = TextAnchor.MiddleLeft;
			SetUIElementPosition(playerTrade[0], 16f, 16f, new Vector2(-27.90001f, 80.11f));
			SetUIElementPosition(playerTrade[1], 16f, 16f, new Vector2(-7.400009f, 80.11f));
			SetUIElementPosition(playerTrade[2], 16f, 16f, new Vector2(-27.90001f, 62.509995f));
			SetUIElementPosition(playerTrade[3], 16f, 16f, new Vector2(-7.400009f, 62.509995f));
			SetUIElementPosition(playerTrade[4], 16f, 16f, new Vector2(-27.90001f, 44.929993f));
			SetUIElementPosition(playerTrade[5], 16f, 16f, new Vector2(-7.400009f, 44.910004f));
			SetUIElementPosition(npcTrade[0], 16f, 16f, new Vector2(-77.100006f, 80.11f));
			SetUIElementPosition(npcTrade[1], 16f, 16f, new Vector2(-56.600006f, 80.11f));
			SetUIElementPosition(npcTrade[2], 16f, 16f, new Vector2(-77.100006f, 62.509995f));
			SetUIElementPosition(npcTrade[3], 16f, 16f, new Vector2(-56.600006f, 62.509995f));
			SetUIElementPosition(npcTrade[4], 16f, 16f, new Vector2(-77.100006f, 44.910004f));
			SetUIElementPosition(npcTrade[5], 16f, 16f, new Vector2(-56.40001f, 44.910004f));
			SetUIElementPosition(MapDown, 30f, 30f, new Vector2(145f, 85f));
			SetUIElementPosition(MapUp, 30f, 30f, new Vector2(145f, -85f));
			SetUIElementPosition(MapEraserButton, 30f, 30f, new Vector2(120f, -25f));
			SetUIElementPosition(MapClose, 30f, 60f, new Vector2(120f, -55f));
			MapInteraction.InitMapButtons(InWorldGemSelect);
		}
	}

	private void Update()
	{
		if (!UWCharacter.Instance.MouseLookEnabled || GameWorldController.instance.AtMainMenu)
		{
			FreeLookCursor.enabled = true;
			FreeLookCursor.transform.position = Input.mousePosition;
		}
		else
		{
			FreeLookCursor.enabled = false;
		}
		MouseLookCursor.enabled = UWCharacter.Instance.MouseLookEnabled;
	}

	private static void SetUIElementPosition(RectTransform rectT, float height, float width, Vector2 anchorPos)
	{
		rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		rectT.anchoredPosition = anchorPos;
	}

	private static void SetUIElementPosition(GameObject obj, float height, float width, Vector2 anchorPos)
	{
		if (obj.GetComponent<RectTransform>() != null)
		{
			SetUIElementPosition(obj.GetComponent<RectTransform>(), height, width, anchorPos);
		}
	}

	private static void SetUIElementPosition(RawImage obj, float height, float width, Vector2 anchorPos)
	{
		if (obj.GetComponent<RectTransform>() != null)
		{
			SetUIElementPosition(obj.GetComponent<RectTransform>(), height, width, anchorPos);
		}
	}

	private static void SetUIElementPosition(GuiBase obj, float height, float width, Vector2 anchorPos)
	{
		if (obj.GetComponent<RectTransform>() != null)
		{
			SetUIElementPosition(obj.GetComponent<RectTransform>(), height, width, anchorPos);
		}
	}

	public void RefreshPanels(int ActivePanelMode)
	{
		CURRENT_HUD_MODE = ActivePanelMode;
		switch (ActivePanelMode)
		{
		case -1:
			UpdatePanelStates();
			break;
		case 0:
			InventoryEnabled = true;
			RuneBagEnabled = false;
			StatsEnabled = false;
			ConversationEnabled = false;
			CutSceneSmallEnabled = true;
			CutSceneFullEnabled = false;
			MapEnabled = false;
			if (chains.ActiveControl > 2)
			{
				chains.ActiveControl = ActivePanelMode;
				UpdatePanelStates();
			}
			else
			{
				StartCoroutine(RotatePanels(currentPanel, InventoryPanel));
				chains.ActiveControl = ActivePanelMode;
			}
			break;
		case 1:
			StatsDisplay.UpdateNow = true;
			InventoryEnabled = false;
			RuneBagEnabled = false;
			StatsEnabled = true;
			ConversationEnabled = false;
			CutSceneSmallEnabled = true;
			CutSceneFullEnabled = false;
			MapEnabled = false;
			if (chains.ActiveControl > 2)
			{
				chains.ActiveControl = ActivePanelMode;
				UpdatePanelStates();
			}
			else
			{
				StartCoroutine(RotatePanels(currentPanel, StatsDisplayPanel));
				chains.ActiveControl = ActivePanelMode;
			}
			break;
		case 2:
			InventoryEnabled = false;
			RuneBagEnabled = true;
			StatsEnabled = false;
			ConversationEnabled = false;
			CutSceneSmallEnabled = true;
			CutSceneFullEnabled = false;
			MapEnabled = false;
			if (chains.ActiveControl > 2)
			{
				chains.ActiveControl = ActivePanelMode;
				UpdatePanelStates();
			}
			else
			{
				StartCoroutine(RotatePanels(currentPanel, RuneBagPanel));
				chains.ActiveControl = ActivePanelMode;
			}
			break;
		case 3:
			InventoryPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
			PaperDollFemalePanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
			PaperDollMalePanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
			StatsDisplayPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, -1f, 0f, 0f);
			RuneBagPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, -1f, 0f, 0f);
			InventoryEnabled = true;
			RuneBagEnabled = false;
			StatsEnabled = false;
			ConversationEnabled = true;
			CutSceneSmallEnabled = false;
			CutSceneFullEnabled = false;
			MapEnabled = false;
			if (UWEBase._RES != "UW2")
			{
				GRLoader gRLoader = new GRLoader(10);
				npcTradeArea.texture = gRLoader.LoadImageAt(1);
				pcTradeArea.texture = gRLoader.LoadImageAt(1);
				ConvNPCPortraitBG.texture = gRLoader.LoadImageAt(2);
				ConvPCPortraitBG.texture = gRLoader.LoadImageAt(2);
				ConvTextScrollTop.texture = gRLoader.LoadImageAt(3);
				ConvTextScrollBottom.texture = gRLoader.LoadImageAt(4);
				ConvPCTitleBG.texture = gRLoader.LoadImageAt(0);
				ConvNPCTitleBG.texture = gRLoader.LoadImageAt(0);
			}
			UpdatePanelStates();
			break;
		case 4:
			InventoryPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
			PaperDollFemalePanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
			PaperDollMalePanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
			StatsDisplayPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, -1f, 0f, 0f);
			RuneBagPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, -1f, 0f, 0f);
			InventoryEnabled = false;
			RuneBagEnabled = false;
			StatsEnabled = false;
			ConversationEnabled = false;
			CutSceneSmallEnabled = false;
			CutSceneFullEnabled = false;
			MapEnabled = true;
			UpdatePanelStates();
			break;
		}
		if (ActivePanelMode != -1)
		{
			chains.ActiveControl = ActivePanelMode;
		}
	}

	private IEnumerator RotatePanels(GameObject fromPanel, GameObject toPanel)
	{
		bool refreshed2 = false;
		if (fromPanel == toPanel)
		{
			UpdatePanelStates();
			yield break;
		}
		Quaternion fromQ = fromPanel.GetComponent<RectTransform>().rotation;
		Quaternion toQ = toPanel.GetComponent<RectTransform>().rotation;
		fromPanel.GetComponent<RectTransform>().SetSiblingIndex(5);
		toPanel.GetComponent<RectTransform>().SetSiblingIndex(6);
		float rate = 2f;
		float index = 0f;
		isRotating = true;
		while (index < 1f)
		{
			fromPanel.GetComponent<RectTransform>().rotation = Quaternion.Lerp(fromQ, toQ, index);
			toPanel.GetComponent<RectTransform>().rotation = Quaternion.Lerp(toQ, fromQ, index);
			if (index >= 0.5f)
			{
				if (!refreshed2)
				{
					UpdatePanelStates();
					refreshed2 = true;
				}
				EnableDisableControl(toPanel, true);
				EnableDisableControl(fromPanel, false);
			}
			else
			{
				EnableDisableControl(toPanel, false);
				EnableDisableControl(fromPanel, true);
			}
			index += rate * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		fromPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, -1f, 0f, 0f);
		toPanel.GetComponent<RectTransform>().rotation = new Quaternion(0f, 0f, 0f, 0f);
		if (!refreshed2)
		{
			UpdatePanelStates();
			refreshed2 = true;
		}
		isRotating = false;
		currentPanel = toPanel;
	}

	private void UpdatePanelStates()
	{
		EnableDisableControl(RuneBagPanel, RuneBagEnabled);
		if (RuneBagEnabled)
		{
			RuneSlot.UpdateRuneDisplay();
		}
		EnableDisableControl(ContextMenu.gameObject, WindowDetect.ContextUIEnabled);
		EnableDisableControl(StatsDisplayPanel, StatsEnabled);
		EnableDisableControl(InventoryPanel, InventoryEnabled);
		EnableDisableControl(PaperDollFemalePanel, InventoryEnabled && UWCharacter.Instance.isFemale);
		EnableDisableControl(PaperDollMalePanel, InventoryEnabled && !UWCharacter.Instance.isFemale);
		EnableDisableControl(ConversationPanel, ConversationEnabled);
		EnableDisableControl(ChainsControl.gameObject, (RuneBagEnabled || StatsEnabled || InventoryEnabled) && !ConversationEnabled);
		EnableDisableControl(UW2ConversationBG.gameObject, ConversationEnabled && UWEBase._RES == "UW2");
		EnableDisableControl(UW1ScrollTop, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ScrollBottom, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ScrlEdgeLeft1, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ScrlEdgeLeft2, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ScrlEdgeLeft3, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ScrlEdgeRight1, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ScrlEdgeRight2, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ScrlEdgeRight3, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1PortraitFramePC, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1PortraitFrameNPC, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1PCNameFrame, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1NPCNameFrame, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1PCTradeArea, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1NPCTradeArea, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(UW1ConversationPaperBackground, ConversationEnabled && UWEBase._RES != "UW2");
		EnableDisableControl(MapPanel, MapEnabled);
		EnableDisableControl(DragonLeftPanel, UWEBase._RES != "UW2" && (InventoryEnabled || StatsEnabled || RuneBagEnabled || ConversationEnabled) && !instance.window.FullScreen);
		EnableDisableControl(DragonRightPanel, UWEBase._RES != "UW2" && (InventoryEnabled || StatsEnabled || RuneBagEnabled || ConversationEnabled) && !instance.window.FullScreen);
		EnableDisableControl(CutsceneSmallPanel, CutSceneSmallEnabled);
		EnableDisableControl(CutsceneFullPanel, CutSceneFullEnabled);
		EnableDisableControl(MonsterEyes.gameObject, (InventoryEnabled || StatsEnabled || RuneBagEnabled) && !ConversationEnabled && !instance.window.FullScreen);
		EnableDisableControl(HudCompass.gameObject, !ConversationEnabled);
		EnableDisableControl(powergem.gameObject, !ConversationEnabled);
		EnableDisableControl(InteractionControlUW1.gameObject, !ConversationEnabled && (RuneBagEnabled || StatsEnabled || InventoryEnabled) && UWEBase._RES != "UW2");
		EnableDisableControl(InteractionControlUW2.gameObject, !ConversationEnabled && (RuneBagEnabled || StatsEnabled || InventoryEnabled) && UWEBase._RES == "UW2");
		EnableDisableControl(ContextMenu.gameObject, !ConversationEnabled);
		if (UWEBase._RES == "UW2" && ConversationEnabled)
		{
			ConversationPanel.transform.SetSiblingIndex(5);
			MessageScrollBackground.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300f);
			MessageScrollBackground.GetComponent<RectTransform>().anchoredPosition = new Vector2(-40.8f, -64.8f);
			SetUIElementPosition(MessageScroll, 33f, 250f, new Vector2(4f, -62.4f));
			EnableDisableControl(MessageLogScrollEdgeLeft, false);
			EnableDisableControl(MessageLogScrollEdgeRight, false);
			EnableDisableControl(MessageScrollBackground, false);
			EnableDisableControl(activeRunes[0].gameObject, false);
			EnableDisableControl(activeRunes[1].gameObject, false);
			EnableDisableControl(activeRunes[2].gameObject, false);
		}
		else
		{
			EnableDisableControl(MessageLogScrollEdgeLeft, true);
			EnableDisableControl(MessageLogScrollEdgeRight, true);
			EnableDisableControl(MessageScrollBackground, true);
			EnableDisableControl(activeRunes[0].gameObject, true);
			EnableDisableControl(activeRunes[1].gameObject, true);
			EnableDisableControl(activeRunes[2].gameObject, true);
			if (UWEBase._RES == "UW2")
			{
				MessageScrollBackground.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 208.68f);
				MessageScrollBackground.GetComponent<RectTransform>().anchoredPosition = new Vector2(-40.8f, -64.8f);
				SetUIElementPosition(MessageScroll, 33f, 174f, new Vector2(-40.93f, -62.4f));
			}
		}
		EnableDisableControl(playerTrade[4], ConversationEnabled && UWEBase._RES == "UW2");
		EnableDisableControl(playerTrade[5], ConversationEnabled && UWEBase._RES == "UW2");
		EnableDisableControl(npcTrade[4], ConversationEnabled && UWEBase._RES == "UW2");
		EnableDisableControl(npcTrade[5], ConversationEnabled && UWEBase._RES == "UW2");
		EnableDisableControl(editorPanel, UWEBase.EditorMode);
	}

	public void EnableDisableControl(GameObject control, bool targetState)
	{
		if (control != null)
		{
			control.SetActive(targetState);
		}
	}

	public void EnableDisableControl(GuiBase control, bool targetState)
	{
		if (control != null)
		{
			EnableDisableControl(control.gameObject, targetState);
		}
	}
}
