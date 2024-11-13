using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuControl : GuiBase_Draggable
{
	private string[] saveNames = new string[4] { "", "", "", "" };

	public GameObject test;

	private const int SAVE = 0;

	private const int SAVE_SLOT_0 = 1;

	private const int SAVE_SLOT_1 = 2;

	private const int SAVE_SLOT_2 = 3;

	private const int SAVE_SLOT_3 = 4;

	private const int SAVE_SLOT_CANCEL = 5;

	private const int RESTORE = 6;

	private const int RESTORE_SLOT_0 = 7;

	private const int RESTORE_SLOT_1 = 8;

	private const int RESTORE_SLOT_2 = 9;

	private const int RESTORE_SLOT_3 = 10;

	private const int RESTORE_SLOT_CANCEL = 11;

	private const int MUSIC = 12;

	private const int MUSIC_ON = 13;

	private const int MUSIC_OFF = 14;

	private const int MUSIC_CANCEL = 15;

	private const int SOUND = 16;

	private const int SOUND_ON = 17;

	private const int SOUND_OFF = 18;

	private const int SOUND_CANCEL = 19;

	private const int DETAIL = 20;

	private const int DETAIL_LOW = 21;

	private const int DETAIL_MED = 22;

	private const int DETAIL_HI = 23;

	private const int DETAIL_BEST = 24;

	private const int DETAIL_DONE = 25;

	private const int RETURN = 26;

	private const int QUIT = 27;

	private const int QUIT_YES = 28;

	private const int QUIT_NO = 29;

	public InteractionModeControl InteractionMenu;

	public RawImage DisplayBG;

	public Texture2D MainBG;

	public Texture2D SaveBG;

	public Texture2D RestoreBG;

	public Texture2D MusicBG;

	public Texture2D SoundBG;

	public Texture2D DetailBG;

	public Texture2D QuitBG;

	public Texture2D MusicStateOn;

	public Texture2D MusicStateOff;

	public RawImage MusicState;

	public Texture2D SoundStateOn;

	public Texture2D SoundStateOff;

	public RawImage SoundState;

	public Texture2D DetailStateLow;

	public Texture2D DetailStateMed;

	public Texture2D DetailStateHi;

	public Texture2D DetailStateBest;

	public RawImage DetailState;

	public GameObject SaveMenu;

	public GameObject SaveSlot_0;

	public GameObject SaveSlot_1;

	public GameObject SaveSlot_2;

	public GameObject SaveSlot_3;

	public GameObject Save_Cancel;

	public GameObject RestoreMenu;

	public GameObject RestoreSlot_0;

	public GameObject RestoreSlot_1;

	public GameObject RestoreSlot_2;

	public GameObject RestoreSlot_3;

	public GameObject Restore_Cancel;

	public GameObject Restore_State;

	public GameObject MusicMenu;

	public GameObject MusicOn;

	public GameObject MusicOff;

	public GameObject Music_Cancel;

	public GameObject SoundMenu;

	public GameObject SoundOn;

	public GameObject SoundOff;

	public GameObject Sound_Cancel;

	public GameObject Sound_Label;

	public GameObject DetailMenu;

	public GameObject DetailLow;

	public GameObject DetailMed;

	public GameObject DetailHi;

	public GameObject DetailBest;

	public GameObject Detail_Cancel;

	public GameObject ReturnMenu;

	public GameObject QuitMenu;

	public GameObject QuitYes;

	public GameObject QuitNo;

	public Texture2D[] UW2Imgs;

	public override void Start()
	{
		base.Start();
		InitOptionButtonsArt();
	}

	private void InitOptionButtonsArt()
	{
		if (UWEBase._RES == "UW2")
		{
			Texture2D dstImg = ArtLoader.CreateBlankImage(80, 16);
			UW2Imgs = new Texture2D[63];
			GetComponent<RawImage>().texture = GameWorldController.instance.grOptbtns.LoadImageAt(3);
			UW2Imgs[1] = GameWorldController.instance.grOptbtns.LoadImageAt(3);
			UW2Imgs[2] = GameWorldController.instance.grOptbtns.LoadImageAt(6);
			UW2Imgs[3] = GameWorldController.instance.grOptbtns.LoadImageAt(5);
			UW2Imgs[4] = GameWorldController.instance.grOptbtns.LoadImageAt(7);
			UW2Imgs[5] = GameWorldController.instance.grOptbtns.LoadImageAt(4);
			UW2Imgs[6] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(3), dstImg, 0, -98);
			UW2Imgs[7] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(8), dstImg, 0, -98);
			UW2Imgs[8] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(3), dstImg, 0, -82);
			UW2Imgs[9] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(8), dstImg, 0, -82);
			UW2Imgs[10] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(3), dstImg, 0, -66);
			UW2Imgs[11] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(8), dstImg, 0, -66);
			UW2Imgs[12] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(3), dstImg, 0, -50);
			UW2Imgs[13] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(8), dstImg, 0, -50);
			UW2Imgs[14] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(3), dstImg, 0, -34);
			UW2Imgs[15] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(8), dstImg, 0, -34);
			UW2Imgs[16] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(3), dstImg, 0, -3);
			UW2Imgs[17] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(8), dstImg, 0, -3);
			UW2Imgs[18] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(3), dstImg, 0, -18);
			UW2Imgs[19] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(8), dstImg, 0, -18);
			UW2Imgs[20] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(7), dstImg, 0, -66);
			UW2Imgs[21] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(12), dstImg, 0, -66);
			UW2Imgs[22] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(7), dstImg, 0, -50);
			UW2Imgs[23] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(12), dstImg, 0, -50);
			UW2Imgs[24] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(6), dstImg, 0, -18);
			UW2Imgs[25] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(11), dstImg, 0, -18);
			UW2Imgs[26] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(4), dstImg, 0, -3);
			UW2Imgs[27] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(9), dstImg, 0, -3);
			UW2Imgs[28] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(7), dstImg, 0, -34);
			UW2Imgs[29] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(12), dstImg, 0, -34);
			UW2Imgs[30] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(6), dstImg, 0, -82);
			UW2Imgs[31] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(11), dstImg, 0, -82);
			UW2Imgs[32] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(6), dstImg, 0, -66);
			UW2Imgs[33] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(11), dstImg, 0, -66);
			UW2Imgs[34] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(6), dstImg, 0, -50);
			UW2Imgs[35] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(11), dstImg, 0, -50);
			UW2Imgs[36] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(6), dstImg, 0, -34);
			UW2Imgs[37] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(11), dstImg, 0, -34);
			UW2Imgs[38] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(4), dstImg, 0, -66);
			UW2Imgs[39] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(9), dstImg, 0, -66);
			UW2Imgs[40] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(4), dstImg, 0, -50);
			UW2Imgs[41] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(9), dstImg, 0, -50);
			UW2Imgs[42] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(4), dstImg, 0, -34);
			UW2Imgs[43] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(9), dstImg, 0, -34);
			UW2Imgs[44] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(4), dstImg, 0, -18);
			UW2Imgs[45] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(9), dstImg, 0, -18);
			UW2Imgs[46] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(14), dstImg, 0, -33);
			UW2Imgs[47] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(15), dstImg, 0, -33);
			UW2Imgs[48] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(15), dstImg, 0, -49);
			UW2Imgs[49] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(15), dstImg, 0, -17);
			UW2Imgs[50] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(15), dstImg, 0, -1);
			UW2Imgs[52] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(14), dstImg, 0, -17);
			UW2Imgs[53] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(13), dstImg, 0, -49);
			UW2Imgs[54] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(13), dstImg, 0, -33);
			UW2Imgs[55] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(13), dstImg, 0, -17);
			UW2Imgs[56] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(13), dstImg, 0, -1);
			UW2Imgs[57] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(5), dstImg, 0, -66);
			UW2Imgs[58] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(10), dstImg, 0, -66);
			UW2Imgs[59] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(5), dstImg, 0, -50);
			UW2Imgs[60] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(10), dstImg, 0, -50);
			UW2Imgs[61] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(4), dstImg, 0, -2);
			UW2Imgs[62] = ArtLoader.InsertImage(GameWorldController.instance.grOptbtns.LoadImageAt(9), dstImg, 0, -2);
		}
		else
		{
			GetComponent<RawImage>().texture = GameWorldController.instance.grOptbtns.LoadImageAt(1);
		}
		SetArt(ref MainBG, 1);
		SetArt(ref SaveBG, 2);
		SetArt(ref RestoreBG, 2);
		SetArt(ref MusicBG, 4);
		SetArt(ref SoundBG, 4);
		SetArt(ref DetailBG, 5);
		SetArt(ref QuitBG, 3);
		SetArt(ref MusicStateOn, 47);
		SetArt(ref MusicStateOff, 48);
		SetArt(ref SoundStateOff, 49);
		SetArt(ref SoundStateOn, 50);
		SetArt(ref DetailStateLow, 53);
		SetArt(ref DetailStateMed, 54);
		SetArt(ref DetailStateHi, 55);
		SetArt(ref DetailStateBest, 56);
		SetArt(Restore_State, 46);
		SetArt(MusicState, 47);
		SetArt(SoundState, 49);
		SetArt(Sound_Label, 52);
		SetArt(DetailState, 56);
	}

	private void SetArt(ref Texture2D tex, int artIndex)
	{
		if (UWEBase._RES != "UW2")
		{
			tex = GameWorldController.instance.grOptbtns.LoadImageAt(artIndex);
		}
		else
		{
			tex = UW2Imgs[artIndex];
		}
	}

	private void SetArt(RawImage image, int artIndex)
	{
		if (UWEBase._RES != "UW2")
		{
			image.texture = GameWorldController.instance.grOptbtns.LoadImageAt(artIndex);
		}
		else
		{
			image.texture = UW2Imgs[artIndex];
		}
	}

	private void SetArt(GameObject obj, int artIndex)
	{
		if (UWEBase._RES != "UW2")
		{
			if (obj.GetComponent<RawImage>() != null)
			{
				obj.GetComponent<RawImage>().texture = GameWorldController.instance.grOptbtns.LoadImageAt(artIndex);
			}
		}
		else if (obj.GetComponent<RawImage>() != null)
		{
			obj.GetComponent<RawImage>().texture = UW2Imgs[artIndex];
		}
	}

	private void ClearHighlights()
	{
		foreach (Transform item in base.gameObject.transform)
		{
			if (item.GetComponent<OptionsMenuButton>() != null)
			{
				item.GetComponent<OptionsMenuButton>().OnHoverExit();
			}
		}
	}

	public void ButtonClickOptionsMenu(int index)
	{
		ClearHighlights();
		switch (index)
		{
		case 0:
			OptionSave();
			break;
		case 1:
		case 2:
		case 3:
		case 4:
			SaveToSlot(index - 1);
			break;
		case 5:
			initMenu();
			break;
		case 6:
			OptionRestore();
			break;
		case 7:
		case 8:
		case 9:
		case 10:
			RestoreFromSlot(index - 7);
			break;
		case 11:
			initMenu();
			break;
		case 12:
			OptionMusic();
			break;
		case 13:
			ToggleMusic(true);
			break;
		case 14:
			ToggleMusic(false);
			break;
		case 15:
			initMenu();
			break;
		case 16:
			OptionSound();
			break;
		case 17:
			ToggleSound(true);
			break;
		case 18:
			ToggleSound(false);
			break;
		case 19:
			initMenu();
			break;
		case 20:
			OptionDetail();
			break;
		case 21:
		case 22:
		case 23:
		case 24:
			SetDetail(index);
			break;
		case 25:
			initMenu();
			break;
		case 26:
			ReturnToGame();
			break;
		case 27:
			OptionQuit();
			break;
		case 28:
			OptionQuitYes();
			break;
		case 29:
			OptionQuitNo();
			break;
		}
	}

	public void initMenu()
	{
		Time.timeScale = 0f;
		DisplayBG.texture = MainBG;
		SaveMenu.SetActive(true);
		RestoreMenu.SetActive(true);
		DetailMenu.SetActive(true);
		SoundMenu.SetActive(true);
		MusicMenu.SetActive(true);
		ReturnMenu.SetActive(true);
		QuitMenu.SetActive(true);
		SaveSlot_0.SetActive(false);
		SaveSlot_1.SetActive(false);
		SaveSlot_2.SetActive(false);
		SaveSlot_3.SetActive(false);
		Save_Cancel.SetActive(false);
		RestoreSlot_0.SetActive(false);
		RestoreSlot_1.SetActive(false);
		RestoreSlot_2.SetActive(false);
		RestoreSlot_3.SetActive(false);
		Restore_Cancel.SetActive(false);
		Restore_State.SetActive(false);
		MusicState.gameObject.SetActive(false);
		MusicOff.SetActive(false);
		MusicOn.SetActive(false);
		Music_Cancel.SetActive(false);
		SoundState.gameObject.SetActive(false);
		SoundOff.SetActive(false);
		SoundOn.SetActive(false);
		Sound_Cancel.SetActive(false);
		Sound_Label.SetActive(false);
		DetailState.gameObject.SetActive(false);
		DetailLow.SetActive(false);
		DetailMed.SetActive(false);
		DetailHi.SetActive(false);
		DetailBest.SetActive(false);
		Detail_Cancel.SetActive(false);
		QuitYes.SetActive(false);
		QuitNo.SetActive(false);
	}

	private void ReturnToGame()
	{
		InteractionMenu.gameObject.SetActive(true);
		Character.InteractionMode = 5;
		InteractionModeControl.UpdateNow = true;
		UWHUD.instance.EnableDisableControl(UWHUD.instance.InteractionControlUW2BG.gameObject, false);
		base.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

	private void OptionSave()
	{
		DisplayBG.texture = SaveBG;
		SaveMenu.SetActive(false);
		RestoreMenu.SetActive(false);
		DetailMenu.SetActive(false);
		SoundMenu.SetActive(false);
		MusicMenu.SetActive(false);
		ReturnMenu.SetActive(false);
		QuitMenu.SetActive(false);
		SaveSlot_0.SetActive(true);
		SaveSlot_1.SetActive(true);
		SaveSlot_2.SetActive(true);
		SaveSlot_3.SetActive(true);
		Save_Cancel.SetActive(true);
		DisplaySaves();
	}

	private void OptionRestore()
	{
		DisplayBG.texture = RestoreBG;
		SaveMenu.SetActive(false);
		RestoreMenu.SetActive(false);
		DetailMenu.SetActive(false);
		SoundMenu.SetActive(false);
		MusicMenu.SetActive(false);
		ReturnMenu.SetActive(false);
		QuitMenu.SetActive(false);
		RestoreSlot_0.SetActive(true);
		RestoreSlot_1.SetActive(true);
		RestoreSlot_2.SetActive(true);
		RestoreSlot_3.SetActive(true);
		Restore_Cancel.SetActive(true);
		Restore_State.SetActive(true);
		DisplaySaves();
	}

	private void DisplaySaves()
	{
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
				UWHUD.instance.MessageScroll.Add(j + 1 + " " + saveNames[j]);
			}
			else
			{
				UWHUD.instance.MessageScroll.Add(j + 1 + " No Save Data");
			}
		}
	}

	private void OptionMusic()
	{
		DisplayBG.texture = MusicBG;
		SaveMenu.SetActive(false);
		RestoreMenu.SetActive(false);
		DetailMenu.SetActive(false);
		SoundMenu.SetActive(false);
		MusicMenu.SetActive(false);
		ReturnMenu.SetActive(false);
		QuitMenu.SetActive(false);
		MusicState.gameObject.SetActive(true);
		MusicOff.SetActive(true);
		MusicOn.SetActive(true);
		Music_Cancel.SetActive(true);
	}

	private void OptionSound()
	{
		DisplayBG.texture = SoundBG;
		SaveMenu.SetActive(false);
		RestoreMenu.SetActive(false);
		DetailMenu.SetActive(false);
		SoundMenu.SetActive(false);
		MusicMenu.SetActive(false);
		ReturnMenu.SetActive(false);
		QuitMenu.SetActive(false);
		SoundState.gameObject.SetActive(true);
		SoundOff.SetActive(true);
		SoundOn.SetActive(true);
		Sound_Cancel.SetActive(true);
		Sound_Label.SetActive(true);
	}

	private void OptionDetail()
	{
		DisplayBG.texture = DetailBG;
		SaveMenu.SetActive(false);
		RestoreMenu.SetActive(false);
		DetailMenu.SetActive(false);
		SoundMenu.SetActive(false);
		MusicMenu.SetActive(false);
		ReturnMenu.SetActive(false);
		QuitMenu.SetActive(false);
		DetailState.gameObject.SetActive(true);
		DetailLow.SetActive(true);
		DetailMed.SetActive(true);
		DetailHi.SetActive(true);
		DetailBest.SetActive(true);
		Detail_Cancel.SetActive(true);
	}

	private void OptionQuit()
	{
		DisplayBG.texture = QuitBG;
		SaveMenu.SetActive(false);
		RestoreMenu.SetActive(false);
		DetailMenu.SetActive(false);
		SoundMenu.SetActive(false);
		MusicMenu.SetActive(false);
		ReturnMenu.SetActive(false);
		QuitMenu.SetActive(false);
		QuitYes.SetActive(true);
		QuitNo.SetActive(true);
	}

	private void OptionQuitYes()
	{
		Application.Quit();
	}

	private void OptionQuitNo()
	{
		ReturnToGame();
	}

	private void SaveToSlot(int SlotNo)
	{
		if (UWEBase._RES == "UW1" && GameWorldController.instance.LevelNo == 8)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_impossible_you_are_between_worlds_));
			return;
		}
		if (!Directory.Exists(Loader.BasePath + "SAVE" + (SlotNo + 1)))
		{
			Directory.CreateDirectory(Loader.BasePath + "SAVE" + (SlotNo + 1));
		}
		if (UWEBase._RES == "UW2")
		{
			LevArk.WriteBackLevArkUW2(SlotNo + 1);
			GameWorldController.instance.WriteBGlobals(SlotNo + 1);
			File.WriteAllText(Loader.BasePath + "SAVE" + (SlotNo + 1) + UWEBase.sep + "DESC", SaveGame.SaveGameName(SlotNo + 1));
			SaveGame.WritePlayerDatUW2(SlotNo + 1);
		}
		else
		{
			LevArk.WriteBackLevArkUW1(SlotNo + 1);
			GameWorldController.instance.WriteBGlobals(SlotNo + 1);
			File.WriteAllText(Loader.BasePath + "SAVE" + (SlotNo + 1) + UWEBase.sep + "DESC", SaveGame.SaveGameName(SlotNo + 1));
			SaveGame.WritePlayerDatUW1(SlotNo + 1);
		}
		UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, StringController.str_save_game_succeeded_));
		UWHUD.instance.RefreshPanels(0);
		ReturnToGame();
	}

	private void RestoreFromSlot(int SlotNo)
	{
		if (saveNames[SlotNo] != "")
		{
			GameWorldController.LoadingGame = true;
			GameWorldController.instance.LevelNo = -1;
			GameWorldController.instance.AtMainMenu = true;
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
			GameWorldController.instance.SwitchLevel(GameWorldController.instance.startLevel);
			GameWorldController.instance.InitBGlobals(SlotNo + 1);
			UWCharacter.Instance.transform.position = GameWorldController.instance.StartPos;
			UWHUD.instance.gameObject.SetActive(true);
			UWCharacter.Instance.playerController.enabled = true;
			UWCharacter.Instance.playerMotor.enabled = true;
			UWCharacter.Instance.playerMotor.movement.velocity = Vector3.zero;
			GameWorldController.instance.AtMainMenu = false;
			UWCharacter.Instance.playerInventory.Refresh();
			UWCharacter.Instance.playerInventory.UpdateLightSources();
			UWHUD.instance.RefreshPanels(0);
			UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, StringController.str_restore_game_complete_));
			GameWorldController.LoadingGame = false;
			ReturnToGame();
		}
	}

	private void ToggleMusic(bool state)
	{
		if (state)
		{
			MusicState.texture = MusicStateOn;
			MusicController.instance.ResumeAll();
		}
		else
		{
			MusicState.texture = MusicStateOff;
			MusicController.instance.StopAll();
		}
		MusicController.PlayMusic = state;
	}

	private void ToggleSound(bool state)
	{
		if (state)
		{
			SoundState.texture = SoundStateOn;
		}
		else
		{
			SoundState.texture = SoundStateOff;
		}
		ObjectInteraction.PlaySoundEffects = state;
	}

	public void SetDetail(int DetailLevel)
	{
		switch (DetailLevel)
		{
		case 21:
			DetailState.texture = DetailStateLow;
			QualitySettings.SetQualityLevel(0, true);
			break;
		case 22:
			DetailState.texture = DetailStateMed;
			QualitySettings.SetQualityLevel(1, true);
			break;
		case 23:
			DetailState.texture = DetailStateHi;
			QualitySettings.SetQualityLevel(2, true);
			break;
		case 24:
			DetailState.texture = DetailStateBest;
			QualitySettings.SetQualityLevel(3, true);
			break;
		}
	}
}
