using System;
using UnityEngine;
using UnityEngine.UI;

public class Compass : GuiBase_Draggable
{
	private int PreviousHeading = -1;

	public const int NORTH = 0;

	public const int NORTHNORTHEAST = 1;

	public const int NORTHEAST = 2;

	public const int EASTNORTHEAST = 3;

	public const int EAST = 4;

	public const int EASTSOUTHEAST = 5;

	public const int SOUTHEAST = 6;

	public const int SOUTHSOUTHEAST = 7;

	public const int SOUTH = 8;

	public const int SOUTHSOUTHWEST = 9;

	public const int SOUTHWEST = 10;

	public const int WESTSOUTHWEST = 11;

	public const int WEST = 12;

	public const int WESTNORTHWEST = 13;

	public const int NORTHWEST = 14;

	public const int NORTHNORTHWEST = 15;

	private RawImage comp;

	public RawImage[] NorthIndicators = new RawImage[16];

	private Texture2D[] CompassPoles = new Texture2D[4];

	public override void Start()
	{
		base.Start();
		comp = GetComponent<RawImage>();
		for (int i = 0; i < 4; i++)
		{
			CompassPoles[i] = GameWorldController.instance.grCompass.LoadImageAt(i);
		}
		if (UWEBase._RES == "UW2")
		{
			rectT = new RectTransform[2];
			rectT[0] = GetComponent<RectTransform>();
			rectT[1] = UWHUD.instance.powergem.transform.parent.GetComponent<RectTransform>();
		}
	}

	public static int getCompassHeadingOffset(GameObject src, GameObject dst)
	{
		int result = 0;
		Vector3 vector = new Vector3(dst.transform.position.x, 0f, dst.transform.position.z);
		Vector3 vector2 = new Vector3(src.transform.position.x, 0f, src.transform.position.z);
		float num = Mathf.Atan2(vector.z - vector2.z, vector.x - vector2.x) * 180f / (float)Math.PI;
		if ((double)num > -22.5 && (double)num <= 22.5)
		{
			result = 2;
		}
		else if ((double)num > 22.5 && (double)num <= 67.5)
		{
			result = 1;
		}
		else if ((double)num > 67.5 && (double)num <= 112.5)
		{
			result = 0;
		}
		else if ((double)num > 112.5 && (double)num <= 157.5)
		{
			result = 7;
		}
		else if ((double)num > 157.5 && (double)num <= 180.0)
		{
			result = 6;
		}
		else if ((double)num > -180.0 && (double)num <= -157.5)
		{
			result = 5;
		}
		else if ((double)num > -157.5 && (double)num <= -112.5)
		{
			result = 5;
		}
		else if ((double)num > -112.5 && (double)num <= -67.5)
		{
			result = 4;
		}
		else if ((double)num > -67.5 && (double)num <= -22.5)
		{
			result = 3;
		}
		return result;
	}

	public static string getCompassHeading(GameObject src, GameObject dst)
	{
		int compassHeadingOffset = getCompassHeadingOffset(src, dst);
		return StringController.instance.GetString(1, StringController.str_to_the_north + compassHeadingOffset);
	}

	public override void Update()
	{
		base.Update();
		if (PreviousHeading != UWCharacter.Instance.currentHeading)
		{
			UpdateNorthIndicator();
			PreviousHeading = UWCharacter.Instance.currentHeading;
			switch (UWCharacter.Instance.currentHeading)
			{
			case 0:
			case 4:
			case 8:
			case 12:
				comp.texture = CompassPoles[0];
				break;
			case 2:
			case 6:
			case 10:
			case 14:
				comp.texture = CompassPoles[2];
				break;
			case 3:
			case 7:
			case 11:
			case 15:
				comp.texture = CompassPoles[1];
				break;
			default:
				comp.texture = CompassPoles[3];
				break;
			}
		}
	}

	private void UpdateNorthIndicator()
	{
		for (int i = 0; i < 16; i++)
		{
			NorthIndicators[i].enabled = i == UWCharacter.Instance.currentHeading;
		}
	}

	public void OnClick()
	{
		if (!Dragging && !WindowDetect.WaitingForInput && !ConversationVM.InConversation)
		{
			UWHUD.instance.MessageScroll.Clear();
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				StatusStringForUW2();
			}
			else
			{
				StatusStringForUW1();
			}
		}
	}

	private static void StatusStringForUW1()
	{
		GetFedAndFatiqueStatus();
		GetAbyssLevel();
		GetDurationOfQuest();
		GuessTimeOfDay();
	}

	private static void StatusStringForUW2()
	{
		GetFedAndFatiqueStatus();
		GetLabyrinthOfWorldsLevel();
		GuessTimeOfDay();
	}

	private static void GetAbyssLevel()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_are_on_the_) + StringController.instance.GetString(1, 411 + GameWorldController.instance.LevelNo) + StringController.instance.GetString(1, StringController.str__level_of_the_abyss_));
	}

	private static void GetLabyrinthOfWorldsLevel()
	{
		switch (GameWorldController.GetWorld(GameWorldController.instance.LevelNo))
		{
		case GameWorldController.Worlds.Britannia:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 73) + StringController.instance.GetString(1, 75));
			break;
		case GameWorldController.Worlds.PrisonTower:
			GetWorldKnowledgeString(0, 76);
			break;
		case GameWorldController.Worlds.Killorn:
			GetWorldKnowledgeString(1, 77);
			break;
		case GameWorldController.Worlds.Ice:
			GetWorldKnowledgeString(2, 78);
			break;
		case GameWorldController.Worlds.Talorus:
			GetWorldKnowledgeString(3, 79);
			break;
		case GameWorldController.Worlds.Academy:
			GetWorldKnowledgeString(4, 80);
			break;
		case GameWorldController.Worlds.Pits:
			GetWorldKnowledgeString(5, 82);
			break;
		case GameWorldController.Worlds.Tomb:
			GetWorldKnowledgeString(6, 81);
			break;
		case GameWorldController.Worlds.Ethereal:
			GetWorldKnowledgeString(7, 83);
			break;
		}
	}

	public static void GetWorldKnowledgeString(int index, int stringNo)
	{
		int num = Quest.instance.QuestVariables[131];
		num = (num >> index) & 1;
		if (num == 1)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 73) + StringController.instance.GetString(1, stringNo));
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 73) + StringController.instance.GetString(1, 74));
		}
	}

	private static void GetFedAndFatiqueStatus()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_are_currently_) + UWCharacter.Instance.GetFedStatus() + " and " + UWCharacter.Instance.GetFatiqueStatus());
	}

	private static void GetDurationOfQuest()
	{
		if (GameClock.day() < 10)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_it_is_the_) + StringController.instance.GetString(1, 411 + GameClock.day()) + StringController.instance.GetString(1, StringController.str__day_of_your_imprisonment_));
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_it_has_been_an_uncountable_number_of_days_since_you_entered_the_abyss_));
		}
	}

	private static void GuessTimeOfDay()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_guess_that_it_is_currently_) + StringController.instance.GetString(1, StringController.str_night_1 + GameClock.hour() / 2));
	}
}
