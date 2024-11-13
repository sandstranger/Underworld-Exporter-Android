using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shrine : Model3D
{
	public const string Mantra_FAL = "FAL";

	public const string Mantra_HUNN = "HUNN";

	public const string Mantra_RA = "RA";

	public const string Mantra_SUMM_RA = "SUMM RA";

	public const string Mantra_GAR = "GAR";

	public const string Mantra_SOL = "SOL";

	public const string Mantra_UN = "UN";

	public const string Mantra_ANRA = "ANRA";

	public const string Mantra_LAHN = "LAHN";

	public const string Mantra_KOH = "KOH";

	public const string Mantra_IMU = "IMU";

	public const string Mantra_MU_AHM = "MU AHM";

	public const string Mantra_OM_CAH = "OM CAH";

	public const string Mantra_AAM = "AAM";

	public const string Mantra_FAHM = "FAHM";

	public const string Mantra_LON = "LON";

	public const string Mantra_LU = "LU";

	public const string Mantra_MUL = "MUL";

	public const string Mantra_ONO = "ONO";

	public const string Mantra_AMO = "AMO";

	public const string Mantra_SAHF = "SAHF";

	public const string Mantra_ROMM = "ROMM";

	public const string Mantra_ORA = "ORA";

	public const string Mantra_INSAHN = "INSAHN";

	public const string Mantra_FANLO = "FANLO";

	public static bool HasGivenKey;

	private int[] AttackSkills = new int[7] { 1, 2, 4, 6, 5, 3, 7 };

	private int[] MagicSkills = new int[3] { 8, 10, 9 };

	private int[] OtherSkills = new int[5] { 15, 19, 13, 18, 17 };

	private bool WaitingForInput = false;

	private InputField inputctrl;

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (!WaitingForInput)
			{
				WaitingForInput = true;
				if (inputctrl == null)
				{
					inputctrl = UWHUD.instance.InputControl;
				}
				UWHUD.instance.MessageScroll.Set("Chant the mantra");
				inputctrl.gameObject.SetActive(true);
				inputctrl.gameObject.GetComponent<InputHandler>().target = base.gameObject;
				inputctrl.gameObject.GetComponent<InputHandler>().currentInputMode = 3;
				inputctrl.contentType = InputField.ContentType.Standard;
				inputctrl.Select();
				Time.timeScale = 0f;
				WindowDetect.WaitingForInput = true;
			}
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public void OnSubmitPickup(string Mantra)
	{
		SubmitMantra(Mantra);
		WaitingForInput = false;
		Time.timeScale = 1f;
		inputctrl.text = "";
		WindowDetect.WaitingForInput = false;
		inputctrl.gameObject.SetActive(false);
	}

	private void SubmitMantra(string Mantra)
	{
		if (Mantra.ToUpper() == "FANLO")
		{
			GiveKeyOfTruth();
			return;
		}
		if (Mantra.ToUpper() == "INSAHN")
		{
			TrackCupOfWonder();
			return;
		}
		int num = Random.Range(1, 4);
		Skills playerSkills = UWCharacter.Instance.PlayerSkills;
		if (UWCharacter.Instance.TrainingPoints == 0)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_are_not_ready_to_advance_));
			return;
		}
		string text = "";
		switch (Mantra.ToUpper())
		{
		case "FAL":
			playerSkills.AdvanceSkill(18, num + Skills.getSkillAttributeBonus(18));
			text = "Acrobat";
			break;
		case "HUNN":
			playerSkills.AdvanceSkill(19, num + Skills.getSkillAttributeBonus(19));
			text = "Appraise";
			break;
		case "RA":
			playerSkills.AdvanceSkill(1, num + Skills.getSkillAttributeBonus(1));
			text = "Attack";
			break;
		case "SUMM RA":
		{
			for (int k = 0; k < num; k++)
			{
				int skillNo3 = AttackSkills[Random.Range(0, AttackSkills.GetUpperBound(0) + 1)];
				playerSkills.AdvanceSkill(skillNo3, 1 + Skills.getSkillAttributeBonus(skillNo3));
				if (text.Length > 0)
				{
					text += " and ";
				}
				text += playerSkills.GetSkillName(skillNo3);
			}
			break;
		}
		case "GAR":
			playerSkills.AdvanceSkill(5, num + Skills.getSkillAttributeBonus(5));
			text = "Axe";
			break;
		case "SOL":
			playerSkills.AdvanceSkill(10, num + Skills.getSkillAttributeBonus(10));
			text = "Casting";
			break;
		case "UN":
			playerSkills.AdvanceSkill(16, num + Skills.getSkillAttributeBonus(16));
			text = "Charm";
			break;
		case "ANRA":
			playerSkills.AdvanceSkill(2, num + Skills.getSkillAttributeBonus(2));
			text = "Defense";
			break;
		case "LAHN":
			playerSkills.AdvanceSkill(9, num + Skills.getSkillAttributeBonus(9));
			text = "Lore";
			break;
		case "KOH":
			playerSkills.AdvanceSkill(6, num + Skills.getSkillAttributeBonus(6));
			text = "Mace";
			break;
		case "IMU":
			playerSkills.AdvanceSkill(8, num + Skills.getSkillAttributeBonus(8));
			text = "Mana";
			break;
		case "MU AHM":
		{
			for (int j = 0; j < num; j++)
			{
				int skillNo2 = MagicSkills[Random.Range(0, MagicSkills.GetUpperBound(0) + 1)];
				playerSkills.AdvanceSkill(skillNo2, 1 + Skills.getSkillAttributeBonus(skillNo2));
				if (text.Length > 0)
				{
					text += " and ";
				}
				text += playerSkills.GetSkillName(skillNo2);
			}
			break;
		}
		case "OM CAH":
		{
			for (int i = 0; i < num; i++)
			{
				int skillNo = OtherSkills[Random.Range(0, OtherSkills.GetUpperBound(0) + 1)];
				playerSkills.AdvanceSkill(skillNo, 1 + Skills.getSkillAttributeBonus(skillNo));
				if (text.Length > 0)
				{
					text += " and ";
				}
				text += playerSkills.GetSkillName(skillNo);
			}
			break;
		}
		case "AAM":
			playerSkills.AdvanceSkill(17, num + Skills.getSkillAttributeBonus(17));
			text = "Picklock";
			break;
		case "FAHM":
			playerSkills.AdvanceSkill(7, num + Skills.getSkillAttributeBonus(7));
			text = "Missile";
			break;
		case "LON":
			playerSkills.AdvanceSkill(15, num + Skills.getSkillAttributeBonus(15));
			text = "Repair";
			break;
		case "LU":
			playerSkills.AdvanceSkill(12, num + Skills.getSkillAttributeBonus(12));
			text = "Search";
			break;
		case "MUL":
			playerSkills.AdvanceSkill(14, num + Skills.getSkillAttributeBonus(14));
			text = "Sneak";
			break;
		case "ONO":
			playerSkills.AdvanceSkill(20, num + Skills.getSkillAttributeBonus(20));
			text = "Swimming";
			break;
		case "AMO":
			playerSkills.AdvanceSkill(4, num + Skills.getSkillAttributeBonus(4));
			text = "Sword";
			break;
		case "SAHF":
			playerSkills.AdvanceSkill(13, num + Skills.getSkillAttributeBonus(13));
			text = "Track";
			break;
		case "ROMM":
			playerSkills.AdvanceSkill(11, num + Skills.getSkillAttributeBonus(11));
			text = "Traps";
			break;
		case "ORA":
			playerSkills.AdvanceSkill(18, num + Skills.getSkillAttributeBonus(3));
			text = "Unarmed";
			break;
		case "INSAHN":
			TrackCupOfWonder();
			return;
		case "FANLO":
			GiveKeyOfTruth();
			return;
		}
		if (text != "")
		{
			UWHUD.instance.MessageScroll.Add("You have advanced in " + text);
			UWCharacter.Instance.TrainingPoints--;
		}
		else
		{
			UWHUD.instance.MessageScroll.Add("That is not a mantra");
		}
	}

	public void GiveKeyOfTruth()
	{
		if (!HasGivenKey)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_none_of_your_skills_improved_));
			HasGivenKey = true;
			ObjectLoaderInfo currObj = ObjectLoader.newObject(225, 0, 0, 0, 256);
			ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.InventoryMarker.gameObject, GameWorldController.instance.InventoryMarker.transform.position);
			GameWorldController.MoveToInventory(objectInteraction);
			UWEBase.CurrentObjectInHand = objectInteraction;
			Character.InteractionMode = 2;
			InteractionModeControl.UpdateNow = true;
		}
	}

	public void TrackCupOfWonder()
	{
		string @string = StringController.instance.GetString(1, 35);
		string text = "";
		string text2 = "";
		int visitTileX = TileMap.visitTileX;
		int visitTileY = TileMap.visitTileY;
		float num = Mathf.Atan2(visitTileY - 43, visitTileX - 26);
		num = 57.29578f * num;
		switch (facing(num))
		{
		case 0:
			text2 = StringController.instance.GetString(1, 42);
			break;
		case 1:
			text2 = StringController.instance.GetString(1, 41);
			break;
		case 2:
			text2 = StringController.instance.GetString(1, 40);
			break;
		case 3:
			text2 = StringController.instance.GetString(1, 39);
			break;
		case 4:
			text2 = StringController.instance.GetString(1, 38);
			break;
		case 5:
			text2 = StringController.instance.GetString(1, 37);
			break;
		case 6:
			text2 = StringController.instance.GetString(1, 36);
			break;
		default:
			text2 = StringController.instance.GetString(1, 43);
			break;
		}
		switch (GameWorldController.instance.LevelNo)
		{
		case 0:
			text = "and " + StringController.instance.GetString(1, 47);
			break;
		case 1:
			text = "and " + StringController.instance.GetString(1, 48);
			break;
		case 2:
			text = "";
			break;
		case 3:
			text = "and " + StringController.instance.GetString(1, 52);
			break;
		default:
			text = "and " + StringController.instance.GetString(1, 55);
			break;
		}
		UWHUD.instance.MessageScroll.Add(@string + text2 + text);
	}

	public override bool TalkTo()
	{
		return use();
	}

	public override string UseVerb()
	{
		return "meditate";
	}

	private int facing(float angle)
	{
		if ((double)angle >= -22.5 && (double)angle <= 22.5)
		{
			return 0;
		}
		if ((double)angle > 22.5 && (double)angle <= 67.5)
		{
			return 1;
		}
		if ((double)angle > 67.5 && (double)angle <= 112.5)
		{
			return 2;
		}
		if ((double)angle > 112.5 && (double)angle <= 157.5)
		{
			return 3;
		}
		if (((double)angle > 157.5 && (double)angle <= 180.0) || (angle >= -180f && (double)angle <= -157.5))
		{
			return 4;
		}
		if ((double)angle >= -157.5 && (double)angle < -112.5)
		{
			return 5;
		}
		if ((double)angle > -112.5 && (double)angle < -67.5)
		{
			return 6;
		}
		if ((double)angle > -67.5 && (double)angle < -22.5)
		{
			return 7;
		}
		return 0;
	}

	public override Vector3[] ModelVertices()
	{
		return new Vector3[80]
		{
			new Vector3(-0.09765625f, 0.125f, -5f / 128f),
			new Vector3(-0.1289063f, 3f / 32f, 9f / 128f),
			new Vector3(-0.1601563f, 0f, 13f / 128f),
			new Vector3(-0.09765625f, 0.125f, 5f / 128f),
			new Vector3(-0.1992188f, 63f / 128f, -5f / 128f),
			new Vector3(-0.1992188f, 63f / 128f, 5f / 128f),
			new Vector3(-0.1992188f, 41f / 64f, 5f / 128f),
			new Vector3(-0.1992188f, 41f / 64f, -5f / 128f),
			new Vector3(-0.0625f, 0.625f, 5f / 128f),
			new Vector3(-0.0625f, 0.625f, -5f / 128f),
			new Vector3(0.0625f, 0.625f, -5f / 128f),
			new Vector3(0.0625f, 0.625f, 5f / 128f),
			new Vector3(0.1992188f, 41f / 64f, 5f / 128f),
			new Vector3(0.1992188f, 41f / 64f, -5f / 128f),
			new Vector3(0.1992188f, 63f / 128f, 5f / 128f),
			new Vector3(0.1992188f, 63f / 128f, -5f / 128f),
			new Vector3(0.09765625f, 0.125f, -5f / 128f),
			new Vector3(0.04296875f, 0.5273438f, -5f / 128f),
			new Vector3(0.04296875f, 0.5273438f, 5f / 128f),
			new Vector3(0.09765625f, 0.125f, 5f / 128f),
			new Vector3(3f / 32f, 91f / 128f, -5f / 128f),
			new Vector3(3f / 32f, 91f / 128f, 5f / 128f),
			new Vector3(0.09765625f, 0.7460938f, -5f / 128f),
			new Vector3(0.09765625f, 0.7460938f, 5f / 128f),
			new Vector3(0.09765625f, 25f / 32f, -5f / 128f),
			new Vector3(0.09765625f, 25f / 32f, 5f / 128f),
			new Vector3(5f / 64f, 0.8242188f, -5f / 128f),
			new Vector3(5f / 64f, 0.8242188f, 5f / 128f),
			new Vector3(0.04296875f, 0.8476563f, 5f / 128f),
			new Vector3(0.04296875f, 0.8476563f, -5f / 128f),
			new Vector3(0f, 0.8632813f, -5f / 128f),
			new Vector3(0f, 0.8632813f, 5f / 128f),
			new Vector3(0f, 0.6679688f, -5f / 128f),
			new Vector3(0f, 0.6679688f, 5f / 128f),
			new Vector3(3f / 128f, 45f / 64f, 5f / 128f),
			new Vector3(3f / 128f, 45f / 64f, -5f / 128f),
			new Vector3(0.03515625f, 47f / 64f, 5f / 128f),
			new Vector3(0.03515625f, 47f / 64f, -5f / 128f),
			new Vector3(0.03515625f, 0.7617188f, 5f / 128f),
			new Vector3(0.03515625f, 0.7617188f, -5f / 128f),
			new Vector3(0.02734375f, 0.7773438f, 5f / 128f),
			new Vector3(0.02734375f, 0.7773438f, -5f / 128f),
			new Vector3(1f / 64f, 0.7851563f, 5f / 128f),
			new Vector3(1f / 64f, 0.7851563f, -5f / 128f),
			new Vector3(0f, 101f / 128f, 5f / 128f),
			new Vector3(0f, 101f / 128f, -5f / 128f),
			new Vector3(-1f / 32f, 0.7773438f, -5f / 128f),
			new Vector3(-1f / 32f, 0.7773438f, 5f / 128f),
			new Vector3(-1f / 64f, 0.7851563f, 5f / 128f),
			new Vector3(-1f / 64f, 0.7851563f, -5f / 128f),
			new Vector3(-0.03515625f, 0.7617188f, -5f / 128f),
			new Vector3(-0.03515625f, 0.7617188f, 5f / 128f),
			new Vector3(-0.03515625f, 47f / 64f, -5f / 128f),
			new Vector3(-0.03515625f, 47f / 64f, 5f / 128f),
			new Vector3(-3f / 128f, 45f / 64f, -5f / 128f),
			new Vector3(-3f / 128f, 45f / 64f, 5f / 128f),
			new Vector3(-0.08203125f, 0.8242188f, -5f / 128f),
			new Vector3(-0.08203125f, 0.8242188f, 5f / 128f),
			new Vector3(-0.04296875f, 0.8476563f, 5f / 128f),
			new Vector3(-0.04296875f, 0.8476563f, -5f / 128f),
			new Vector3(-0.09765625f, 25f / 32f, -5f / 128f),
			new Vector3(-0.09765625f, 25f / 32f, 5f / 128f),
			new Vector3(-0.09765625f, 0.7460938f, -5f / 128f),
			new Vector3(-0.09765625f, 0.7460938f, 5f / 128f),
			new Vector3(-3f / 32f, 91f / 128f, -5f / 128f),
			new Vector3(-3f / 32f, 91f / 128f, 5f / 128f),
			new Vector3(0.1289063f, 3f / 32f, 9f / 128f),
			new Vector3(-0.1289063f, 3f / 32f, -9f / 128f),
			new Vector3(0.1289063f, 3f / 32f, -9f / 128f),
			new Vector3(0.1601563f, 0f, 13f / 128f),
			new Vector3(-0.1601563f, 0f, -13f / 128f),
			new Vector3(0.1601563f, 0f, -13f / 128f),
			new Vector3(-0.04296875f, 0.5273438f, 5f / 128f),
			new Vector3(-0.04296875f, 0.5273438f, -5f / 128f),
			new Vector3(0.1992188f, 19f / 32f, -5f / 128f),
			new Vector3(0.1992188f, 19f / 32f, 5f / 128f),
			new Vector3(-11f / 128f, 13f / 64f, -5f / 128f),
			new Vector3(11f / 128f, 13f / 64f, -5f / 128f),
			new Vector3(-13f / 128f, 3f / 32f, -9f / 128f),
			new Vector3(13f / 128f, 3f / 32f, -9f / 128f)
		};
	}

	public override int[] ModelTriangles(int meshNo)
	{
		return new int[450]
		{
			44, 31, 28, 44, 58, 31, 42, 28, 27, 40,
			27, 25, 38, 25, 23, 36, 23, 21, 34, 21,
			11, 48, 57, 58, 47, 61, 57, 51, 63, 61,
			53, 65, 63, 55, 8, 65, 8, 55, 33, 8,
			33, 11, 11, 33, 34, 65, 53, 55, 53, 63,
			51, 51, 51, 51, 51, 61, 47, 47, 57, 48,
			48, 58, 44, 44, 28, 42, 42, 27, 40, 40,
			25, 38, 38, 23, 36, 36, 21, 34, 8, 5,
			6, 8, 72, 5, 11, 12, 14, 11, 14, 18,
			72, 8, 18, 18, 8, 11, 3, 72, 18, 19,
			3, 18, 1, 3, 19, 66, 1, 19, 69, 2,
			1, 69, 1, 66, 45, 30, 59, 49, 59, 56,
			46, 56, 60, 50, 60, 62, 52, 62, 64, 54,
			64, 9, 45, 29, 30, 43, 24, 26, 39, 22,
			24, 37, 20, 22, 35, 10, 20, 9, 10, 32,
			9, 7, 4, 9, 4, 73, 10, 15, 13, 10,
			17, 15, 9, 73, 10, 10, 73, 17, 17, 73,
			76, 17, 76, 77, 76, 0, 77, 77, 0, 16,
			0, 67, 16, 16, 67, 68, 67, 70, 68, 68,
			70, 71, 32, 10, 35, 35, 20, 37, 37, 22,
			39, 39, 24, 43, 43, 29, 45, 45, 59, 49,
			46, 49, 56, 50, 46, 60, 52, 50, 62, 54,
			52, 64, 32, 54, 9, 2, 70, 1, 70, 67,
			1, 67, 0, 1, 1, 0, 3, 3, 0, 73,
			3, 73, 72, 72, 73, 4, 72, 4, 5, 5,
			4, 6, 6, 4, 7, 8, 6, 7, 8, 7,
			9, 65, 8, 9, 65, 9, 64, 63, 65, 64,
			63, 64, 62, 61, 63, 62, 61, 62, 60, 57,
			61, 60, 57, 60, 56, 58, 57, 56, 58, 56,
			59, 31, 58, 59, 31, 59, 30, 28, 31, 30,
			28, 30, 29, 27, 28, 29, 27, 29, 26, 25,
			27, 26, 25, 26, 24, 23, 25, 24, 23, 24,
			22, 21, 23, 22, 21, 22, 20, 11, 21, 20,
			11, 20, 10, 12, 11, 10, 12, 10, 13, 14,
			12, 13, 14, 13, 15, 18, 14, 15, 18, 15,
			17, 19, 18, 17, 19, 17, 16, 66, 19, 16,
			66, 16, 68, 69, 66, 68, 69, 68, 71, 2,
			69, 71, 2, 71, 70, 45, 44, 42, 43, 42,
			40, 41, 40, 38, 39, 38, 36, 37, 36, 34,
			35, 34, 33, 32, 33, 55, 54, 55, 53, 52,
			53, 51, 50, 51, 47, 46, 47, 48, 49, 48,
			44, 42, 43, 45, 40, 41, 43, 38, 39, 41,
			36, 37, 39, 34, 35, 37, 33, 32, 35, 55,
			54, 32, 53, 52, 54, 51, 50, 52, 47, 46,
			50, 48, 49, 46, 44, 45, 49, 26, 29, 43
		}.Reverse().ToArray();
	}

	public override Color ModelColour(int meshNo)
	{
		return Color.yellow;
	}
}
