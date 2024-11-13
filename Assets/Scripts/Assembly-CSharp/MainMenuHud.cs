using UnityEngine;
using UnityEngine.UI;

public class MainMenuHud : GuiBase
{
	private string[] saveNames = new string[4] { "", "", "", "" };

	public Texture2D CursorIcon;

	public Rect CursorPosition;

	public GameObject CharGen;

	public GameObject OpScr;

	public GameObject IntroductionButton;

	public GameObject CreateCharacterButton;

	public GameObject CreditsButton;

	public GameObject JourneyOnButton;

	public GameObject[] SaveGameButtons;

	public Text CharName;

	public Text CharGender;

	public Text CharClass;

	public Text CharStr;

	public Text CharDex;

	public Text CharInt;

	public Text CharVit;

	public Text[] CharSkillName;

	public Text[] CharSkillVal;

	public InputField EnterCharName;

	public int MenuMode = 0;

	public int chargenStage = 0;

	public Text CharGenQuestion;

	private int CharClassAns;

	private int SkillSeed;

	public RawImage CharGenBody;

	protected int cursorSizeX = 64;

	protected int cursorSizeY = 64;

	private GRLoader chrBtns;

	public static MainMenuHud instance;

	public void InitChargenScreen()
	{
		CharName.text = "";
		CharGender.text = "";
		CharStr.text = "";
		CharDex.text = "";
		CharInt.text = "";
		CharVit.text = "";
		CharClass.text = "";
		for (int i = 0; i < 5; i++)
		{
			CharSkillName[i].text = "";
			CharSkillVal[i].text = "";
			if (UWEBase._RES == "UW2")
			{
				CharSkillVal[i].color = Color.white;
				CharSkillName[i].color = Color.white;
			}
		}
		CharGenBody.texture = Resources.Load<Texture2D>(UWEBase._RES + "/Sprites/texture_blank");
	}

	public override void Start()
	{
		instance = this;
		if (GameWorldController.instance.AtMainMenu)
		{
			WindowDetectUW.SwitchFromMouseLook();
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				OpScr.GetComponent<RawImage>().texture = GameWorldController.instance.bytloader.LoadImageAt(5);
				CharGenQuestion.color = Color.white;
				CharName.color = Color.white;
				CharGender.color = Color.white;
				CharClass.color = Color.white;
				CharStr.color = Color.white;
				CharDex.color = Color.white;
				CharInt.color = Color.white;
				CharVit.color = Color.white;
			}
			else
			{
				OpScr.GetComponent<RawImage>().texture = GameWorldController.instance.bytloader.LoadImageAt(4);
			}
			CharGen.GetComponent<RawImage>().texture = GameWorldController.instance.bytloader.LoadImageAt(1);
			CursorIcon = GameWorldController.instance.grCursors.LoadImageAt(0);
			CursorPosition = new Rect(0f, 0f, cursorSizeX, cursorSizeY);
			CharGen.SetActive(false);
			UWHUD.instance.CutScenesFull.SetAnimationFile = "FadeToBlackSleep";
			UWHUD.instance.CutScenesFull.End();
			Cutscene_Splash cs = UWHUD.instance.gameObject.AddComponent<Cutscene_Splash>();
			UWHUD.instance.CutScenesFull.cs = cs;
			UWHUD.instance.CutScenesFull.Begin();
		}
	}

	private void OnGUI()
	{
		CursorPosition.center = Event.current.mousePosition;
		if (CursorIcon != null)
		{
			GUI.DrawTexture(CursorPosition, CursorIcon);
		}
		if ((MenuMode == 1 || MenuMode == 2) && Input.GetKey(KeyCode.Escape))
		{
			MenuMode = 0;
			chargenStage = 0;
			OpScr.SetActive(true);
			CharGen.SetActive(false);
			ButtonClickMainMenu(4);
		}
	}

	public void ButtonClickMainMenu(int option)
	{
		if (MenuMode == 0)
		{
			switch (option)
			{
			case 0:
			{
				Cutscene_Intro cs = UWHUD.instance.gameObject.AddComponent<Cutscene_Intro>();
				UWHUD.instance.CutScenesFull.cs = cs;
				UWHUD.instance.CutScenesFull.Begin();
				break;
			}
			case 1:
				MenuMode = 1;
				CharGen.SetActive(true);
				OpScr.SetActive(false);
				CharGenQuestion.text = getQuestion(0);
				InitChargenScreen();
				chrBtns = new GRLoader(8);
				chrBtns.PaletteNo = 9;
				PlaceButtons(Chargen.GetChoices(0, -1), false);
				break;
			case 2:
			{
				Cutscene_Credits cs2 = UWHUD.instance.gameObject.AddComponent<Cutscene_Credits>();
				UWHUD.instance.CutScenesFull.cs = cs2;
				UWHUD.instance.CutScenesFull.Begin();
				break;
			}
			case 3:
				MenuMode = 2;
				DisplaySaveGames();
				break;
			case 4:
			{
				IntroductionButton.SetActive(true);
				CreateCharacterButton.SetActive(true);
				CreditsButton.SetActive(true);
				JourneyOnButton.SetActive(true);
				for (int i = 0; i <= SaveGameButtons.GetUpperBound(0); i++)
				{
					SaveGameButtons[i].SetActive(false);
				}
				OpScr.SetActive(true);
				CharGen.SetActive(false);
				break;
			}
			}
		}
		else
		{
			ChargenClick(option);
		}
	}

	private void DisplaySaveGames()
	{
		IntroductionButton.SetActive(false);
		CreateCharacterButton.SetActive(false);
		CreditsButton.SetActive(false);
		JourneyOnButton.SetActive(false);
		UWHUD.instance.MessageScroll.Clear();
		for (int i = 1; i <= 4; i++)
		{
			char[] buffer;
			if (DataLoader.ReadStreamFile(Loader.BasePath + "SAVE" + i + UWEBase.sep + "DESC", out buffer))
			{
				saveNames[i - 1] = new string(buffer);
			}
			else
			{
				saveNames[i - 1] = "";
			}
		}
		for (int j = 0; j <= saveNames.GetUpperBound(0); j++)
		{
			if (saveNames[j] != "")
			{
				SaveGameButtons[j].SetActive(true);
				SaveGameButtons[j].GetComponent<Text>().text = saveNames[j];
			}
			else
			{
				SaveGameButtons[j].SetActive(false);
			}
		}
	}

	public void LoadSave(int SlotNo)
	{
		switch (SlotNo)
		{
		case -2:
			if (UWEBase.EditorMode)
			{
				UWHUD.instance.editorButtonLabel.text = "Enable Editor";
				UWEBase.EditorMode = false;
			}
			else
			{
				UWHUD.instance.editorButtonLabel.text = "Editor Enabled";
				UWEBase.EditorMode = true;
			}
			return;
		case -1:
			GameWorldController.instance.Lev_Ark_File_Selected = "DATA" + UWEBase.sep + "LEV.ARK";
			GameWorldController.instance.SCD_Ark_File_Selected = "DATA" + UWEBase.sep + "SCD.ARK";
			GameWorldController.instance.InitBGlobals(0);
			GameClock.instance._day = 0;
			GameClock.instance._minute = 51;
			GameClock.instance._second = 15;
			UWCharacter.Instance.CurVIT = 255;
			UWCharacter.Instance.MaxVIT = 255;
			JourneyOnwards();
			return;
		}
		GameWorldController.instance.Lev_Ark_File_Selected = "SAVE" + (SlotNo + 1) + UWEBase.sep + "LEV.ARK";
		GameWorldController.instance.SCD_Ark_File_Selected = "SAVE" + (SlotNo + 1) + UWEBase.sep + "SCD.ARK";
		if (UWEBase._RES != "UW2")
		{
			SaveGame.LoadPlayerDatUW1(SlotNo + 1);
		}
		else
		{
			SaveGame.LoadPlayerDatUW2(SlotNo + 1);
		}
		GameWorldController.instance.InitBGlobals(SlotNo + 1);
		JourneyOnwards();
		UWCharacter.Instance.playerInventory.Refresh();
		UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, StringController.str_restore_game_complete_));
	}

	public void ChargenClick(int option)
	{
		switch (chargenStage)
		{
		case 0:
			UWCharacter.Instance.PlayerSkills.InitSkills();
			if (option == 0)
			{
				UWCharacter.Instance.isFemale = false;
			}
			else
			{
				UWCharacter.Instance.isFemale = true;
			}
			CharGender.text = StringController.instance.GetString(2, Chargen.GetChoices(chargenStage, -1)[option]);
			chargenStage++;
			PlaceButtons(Chargen.GetChoices(chargenStage, -1), false);
			break;
		case 1:
			if (option == 0)
			{
				UWCharacter.Instance.isLefty = true;
			}
			else
			{
				UWCharacter.Instance.isLefty = false;
			}
			chargenStage++;
			PlaceButtons(Chargen.GetChoices(chargenStage, -1), false);
			break;
		case 2:
			UWCharacter.Instance.CharClass = option;
			CharClassAns = option;
			SkillSeed = Chargen.getSeed(option);
			UWCharacter.Instance.PlayerSkills.STR = Mathf.Min(Mathf.Max(Chargen.getBaseSTR(option) + Random.Range(1, SkillSeed), 12), 30);
			UWCharacter.Instance.PlayerSkills.INT = Mathf.Min(Mathf.Max(Chargen.getBaseINT(option) + Random.Range(1, SkillSeed), 12), 30);
			UWCharacter.Instance.PlayerSkills.DEX = Mathf.Min(Mathf.Max(Chargen.getBaseDEX(option) + Random.Range(1, SkillSeed), 12), 30);
			CharStr.text = "Str:   " + UWCharacter.Instance.PlayerSkills.STR;
			CharInt.text = "Int:   " + UWCharacter.Instance.PlayerSkills.INT;
			CharDex.text = "Dex:   " + UWCharacter.Instance.PlayerSkills.DEX;
			CharClass.text = getClass(UWCharacter.Instance.CharClass);
			UWCharacter.Instance.MaxVIT = UWCharacter.Instance.PlayerSkills.STR * 2;
			UWCharacter.Instance.CurVIT = UWCharacter.Instance.PlayerSkills.STR * 2;
			CharVit.text = "Vit:   " + UWCharacter.Instance.MaxVIT;
			chargenStage++;
			if (Chargen.GetChoices(chargenStage, CharClassAns).GetUpperBound(0) == 0)
			{
				CharGenQuestion.text = getQuestion(chargenStage);
				ChargenClick(0);
				return;
			}
			PlaceButtons(Chargen.GetChoices(chargenStage, CharClassAns), false);
			break;
		case 3:
		case 4:
		case 5:
		case 6:
			AdvanceSkill(option, chargenStage);
			chargenStage++;
			if (Chargen.GetChoices(chargenStage, CharClassAns).GetUpperBound(0) == 0)
			{
				CharGenQuestion.text = getQuestion(chargenStage);
				ChargenClick(0);
				return;
			}
			PlaceButtons(Chargen.GetChoices(chargenStage, CharClassAns), false);
			break;
		case 7:
			AdvanceSkill(option, chargenStage);
			chargenStage++;
			PlaceButtons(Chargen.GetChoices(chargenStage, CharClassAns), true);
			break;
		case 8:
		{
			chargenStage++;
			PlaceButtons(Chargen.GetChoices(chargenStage, -1), false);
			UWCharacter.Instance.Body = option;
			GRLoader gRLoader = new GRLoader(4);
			if (UWCharacter.Instance.isFemale)
			{
				CharGenBody.texture = chrBtns.LoadImageAt(22 + option);
				UWHUD.instance.playerBody.texture = gRLoader.LoadImageAt(5 + option);
			}
			else
			{
				CharGenBody.texture = chrBtns.LoadImageAt(17 + option);
				UWHUD.instance.playerBody.texture = gRLoader.LoadImageAt(option);
			}
			break;
		}
		case 9:
			chargenStage++;
			GameWorldController.instance.difficulty = option;
			RemoveButtons();
			EnterCharName.gameObject.SetActive(true);
			EnterCharName.GetComponent<RawImage>().texture = chrBtns.LoadImageAt(2);
			EnterCharName.Select();
			break;
		case 10:
			chargenStage++;
			EnterCharName.gameObject.SetActive(false);
			PlaceButtons(Chargen.GetChoices(chargenStage, CharClassAns), false);
			break;
		case 11:
			if (option == 0)
			{
				UWCharacter.Instance.EXP = 50;
				UWCharacter.Instance.TrainingPoints = 1;
				UWCharacter.Instance.PlayerMagic.MaxMana = UWCharacter.Instance.PlayerSkills.ManaSkill * 3;
				UWCharacter.Instance.PlayerMagic.CurMana = UWCharacter.Instance.PlayerMagic.MaxMana;
				UWCharacter.Instance.PlayerMagic.TrueMaxMana = UWCharacter.Instance.PlayerMagic.MaxMana;
				GameWorldController.instance.InitBGlobals(0);
				for (int i = 0; i <= Quest.instance.QuestVariables.GetUpperBound(0); i++)
				{
					Quest.instance.QuestVariables[i] = 0;
				}
				for (int j = 0; j <= UWCharacter.Instance.PlayerMagic.PlayerRunes.GetUpperBound(0); j++)
				{
					UWCharacter.Instance.PlayerMagic.PlayerRunes[j] = false;
				}
				switch (UWEBase._RES)
				{
				case "UW1":
				case "UW0":
					Quest.instance.TalismansRemaining = 8;
					Quest.instance.DayGaramonDream = 0;
					Quest.instance.GaramonDream = 0;
					Quest.instance.IncenseDream = 0;
					Quest.instance.isGaramonBuried = false;
					Quest.instance.isOrbDestroyed = false;
					Quest.instance.isCupFound = false;
					break;
				case "UW2":
					Quest.instance.variables[101] = 255;
					Quest.instance.variables[102] = 255;
					Quest.instance.variables[103] = 255;
					Quest.instance.variables[104] = 255;
					Quest.instance.variables[105] = 255;
					Quest.instance.variables[106] = 255;
					break;
				}
				GameClock.instance._day = 0;
				GameClock.instance._minute = 51;
				GameClock.instance._second = 15;
				UWCharacter.Instance.Fatigue = 20;
				UWCharacter.Instance.FoodLevel = 192;
				JourneyOnwards();
			}
			else
			{
				chargenStage = 0;
				InitChargenScreen();
				PlaceButtons(Chargen.GetChoices(chargenStage, -1), false);
			}
			break;
		}
		CharGenQuestion.text = getQuestion(chargenStage);
	}

	public void EnterCharNameEvent()
	{
		if (EnterCharName.text.TrimEnd() == "")
		{
			EnterCharName.text = "Avatar";
		}
		CharName.text = EnterCharName.text;
		UWCharacter.Instance.CharName = EnterCharName.text;
		EnterCharName.gameObject.SetActive(false);
		ChargenClick(0);
	}

	public void AdvanceSkill(int option, int Stage)
	{
		int skillNo = Chargen.GetChoices(Stage, CharClassAns)[option] - 30;
		int skillPoints = Mathf.Min(Random.Range(1, SkillSeed) + Skills.getSkillAttributeBonus(skillNo), SkillSeed);
		UWCharacter.Instance.PlayerSkills.AdvanceSkill(skillNo, skillPoints);
		string @string = StringController.instance.GetString(2, Chargen.GetChoices(Stage, CharClassAns)[option]);
		for (int i = 0; i < 5; i++)
		{
			if (CharSkillName[i].text == "")
			{
				CharSkillName[i].text = @string;
				CharSkillVal[i].text = UWCharacter.Instance.PlayerSkills.GetSkill(skillNo).ToString();
				break;
			}
			if (CharSkillName[i].text == @string)
			{
				CharSkillVal[i].text = UWCharacter.Instance.PlayerSkills.GetSkill(skillNo).ToString();
				break;
			}
		}
	}

	public string getClass(int option)
	{
		return StringController.instance.GetString(2, 23 + option);
	}

	public string getQuestion(int option)
	{
		switch (option)
		{
		case 0:
		case 1:
		case 2:
			return StringController.instance.GetString(2, 1 + option);
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
			return StringController.instance.GetString(2, 4);
		case 8:
			return "";
		case 9:
		case 10:
		case 11:
			return StringController.instance.GetString(2, option - 3);
		default:
			return "UNKNOWN Option!";
		}
	}

	public void RemoveButtons()
	{
		foreach (Transform item in CharGen.transform)
		{
			if (item.name.Substring(0, 5) == "_Char")
			{
				Object.Destroy(item.transform.gameObject);
			}
		}
	}

	public void PlaceButtons(int[] buttons, bool isImageButton)
	{
		RemoveButtons();
		if (isImageButton)
		{
			int num = 7;
			if (UWCharacter.Instance.isFemale)
			{
				num = 12;
			}
			for (int i = 0; i <= buttons.GetUpperBound(0); i++)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Prefabs/_CharGenImageButton"));
				gameObject.transform.SetParent(CharGen.transform);
				gameObject.GetComponent<ChargenButton>().ButtonBG.texture = chrBtns.LoadImageAt(4);
				gameObject.GetComponent<ChargenButton>().ButtonOff = chrBtns.LoadImageAt(4);
				gameObject.GetComponent<ChargenButton>().ButtonOn = chrBtns.LoadImageAt(5);
				gameObject.GetComponent<ChargenButton>().SubmitTarget = this;
				gameObject.GetComponent<ChargenButton>().Value = i;
				gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(70f, 70f - (float)i * 35f);
				gameObject.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
				gameObject.GetComponent<ChargenButton>().ButtonImage.texture = chrBtns.LoadImageAt(num + i);
			}
			return;
		}
		if (buttons.GetUpperBound(0) <= 8)
		{
			for (int j = 0; j <= buttons.GetUpperBound(0); j++)
			{
				GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Prefabs/_CharGenTextButton"));
				gameObject2.GetComponent<ChargenButton>().ButtonBG.texture = chrBtns.LoadImageAt(2);
				gameObject2.GetComponent<ChargenButton>().ButtonOff = chrBtns.LoadImageAt(2);
				gameObject2.GetComponent<ChargenButton>().ButtonOn = chrBtns.LoadImageAt(6);
				gameObject2.transform.SetParent(CharGen.transform);
				gameObject2.GetComponent<ChargenButton>().SubmitTarget = this;
				gameObject2.GetComponent<ChargenButton>().Value = j;
				gameObject2.GetComponent<RectTransform>().anchoredPosition = new Vector3(70f, 60f - (float)j * 20f);
				gameObject2.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
				gameObject2.GetComponent<ChargenButton>().ButtonText.text = StringController.instance.GetString(2, buttons[j]);
			}
			return;
		}
		for (int k = 0; k <= buttons.GetUpperBound(0); k++)
		{
			GameObject gameObject3 = (GameObject)Object.Instantiate(Resources.Load("Prefabs/_CharGenTextButton"));
			gameObject3.GetComponent<ChargenButton>().ButtonBG.texture = chrBtns.LoadImageAt(2);
			gameObject3.GetComponent<ChargenButton>().ButtonOff = chrBtns.LoadImageAt(2);
			gameObject3.GetComponent<ChargenButton>().ButtonOn = chrBtns.LoadImageAt(6);
			gameObject3.transform.SetParent(CharGen.transform);
			gameObject3.GetComponent<ChargenButton>().SubmitTarget = this;
			gameObject3.GetComponent<ChargenButton>().Value = k;
			if (k < 5)
			{
				gameObject3.GetComponent<RectTransform>().anchoredPosition = new Vector3(40f, 60f - (float)k * 20f);
			}
			else
			{
				gameObject3.GetComponent<RectTransform>().anchoredPosition = new Vector3(110f, 60f - (float)(k - 5) * 20f);
			}
			gameObject3.GetComponent<ChargenButton>().ButtonText.text = StringController.instance.GetString(2, buttons[k]);
			gameObject3.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
		}
	}

	public void JourneyOnwards()
	{
		GameWorldController.LoadingGame = true;
		GameWorldController.instance.SwitchLevel(GameWorldController.instance.startLevel);
		UWCharacter.Instance.transform.position = GameWorldController.instance.StartPos;
		UWHUD.instance.gameObject.SetActive(true);
		UWCharacter.Instance.playerController.enabled = true;
		UWCharacter.Instance.Death = false;
		UWCharacter.Instance.playerMotor.enabled = true;
		GameWorldController.instance.AtMainMenu = false;
		UWCharacter.Instance.playerInventory.Refresh();
		UWCharacter.Instance.playerInventory.UpdateLightSources();
		UWHUD.instance.RefreshPanels(0);
		instance.gameObject.SetActive(false);
		if (UWEBase.EditorMode)
		{
			UWHUD.instance.editor.SelectCurrentTile();
		}
		GameWorldController.LoadingGame = false;
	}
}
