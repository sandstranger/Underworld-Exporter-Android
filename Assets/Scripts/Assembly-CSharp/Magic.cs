using System;
using UnityEngine;

public class Magic : UWEBase
{
	public const int SpellRule_Consumable = 2;

	public const int SpellRule_Equipable = 1;

	public const int SpellRule_Immediate = 0;

	public const int SpellRule_TargetOther = 0;

	public const int SpellRule_TargetSelf = 1;

	public const int SpellRule_TargetVector = 2;

	public const int SpellResultNone = 0;

	public const int SpellResultPassive = 1;

	public const int SpellResultActive = 2;

	public string ReadiedSpell;

	public bool[] PlayerRunes = new bool[24];

	public int[] ActiveRunes = new int[3];

	public static bool InfiniteMana;

	[SerializeField]
	private int _MaxMana;

	private int _CurMana;

	public int TrueMaxMana;

	public int SpellCost;

	public ObjectInteraction ObjectInSlot;

	public bool InventorySpell;

	private string[] Runes = new string[24]
	{
		"An", "Bet", "Corp", "Des", "Ex", "Flam", "Grav", "Hur", "In", "Jux",
		"Kal", "Lor", "Mani", "Nox", "Ort", "Por", "Quas", "Rel", "Sanct", "Tym",
		"Uus", "Vas", "Wis", "Ylem"
	};

	public long SummonCount = 0L;

	public int MaxMana
	{
		get
		{
			return _MaxMana;
		}
		set
		{
			_MaxMana = value;
			UWHUD.instance.FlaskMana.UpdateFlaskDisplay();
		}
	}

	public int CurMana
	{
		get
		{
			return _CurMana;
		}
		set
		{
			_CurMana = value;
			UWHUD.instance.FlaskMana.UpdateFlaskDisplay();
		}
	}

	public void Update()
	{
		if (InfiniteMana && CurMana < MaxMana)
		{
			CurMana = MaxMana;
		}
	}

	public void SetSpellCost(int SpellCircle)
	{
		SpellCost = SpellCircle * 3;
	}

	public void ApplySpellCost()
	{
		CurMana -= SpellCost;
		SpellCost = 0;
	}

	public bool TestSpellCast(UWCharacter casterUW, int Rune1, int Rune2, int Rune3)
	{
		int num = 0;
		string text = TranslateSpellRune(Rune1, Rune2, Rune3);
		switch (text)
		{
		case "An An An":
			num = 1;
			break;
		case "In Mani Ylem":
		case "In Lor":
		case "Ort Jux":
		case "Bet In Sanct":
		case "Sanct Hur":
			num = 1;
			break;
		case "Quas Corp":
		case "Wis Mani":
		case "Uus Por":
		case "In Bet Mani":
		case "Rel Des Por":
		case "In Sanct":
			num = 2;
			break;
		case "In Jux":
			if (UWEBase._RES != "UW2")
			{
				num = 2;
			}
			break;
		case "Quas Mani Ylem":
			if (UWEBase._RES == "UW2")
			{
				num = 2;
			}
			break;
		case "Bet Sanct Lor":
		case "Ort Grav":
		case "Quas Lor":
		case "Rel Tym Por":
		case "Ylem Por":
			num = 3;
			break;
		case "Sanct Jux":
			num = 3;
			break;
		case "An Sanct":
		case "Sanct Flam":
		case "In Mani":
		case "Hur Por":
		case "Nox Ylem":
		case "An Jux":
			num = 4;
			break;
		case "Por Flam":
		case "Grav Sanct Por":
		case "Ort Wis Ylem":
		case "Ex Ylem":
		case "An Nox":
		case "An Corp Mani":
			num = 5;
			break;
		case "Vas In Lor":
		case "Vas Rel Por":
		case "Vas In Mani":
		case "An Ex Por":
		case "Vas Ort Grav":
		case "Ort Por Ylem":
			num = 6;
			break;
		case "Wis Ex":
			if (UWEBase._RES == "UW2")
			{
				num = 6;
			}
			break;
		case "In Mani Rel":
		case "Vas An Wis":
		case "Vas Sanct Lor":
		case "Vas Hur Por":
		case "Kal Mani":
		case "Ort An Quas":
			num = 7;
			break;
		case "Vas Kal Corp":
		case "Flam Hur":
		case "An Tym":
		case "In Vas Sanct":
		case "Ort Por Wis":
		case "Vas Por Ylem":
			num = 8;
			break;
		default:
			UWHUD.instance.MessageScroll.Add("Not a spell.");
			return false;
		}
		if (UWEBase._RES == "UW2")
		{
			num--;
		}
		if (Mathf.Max(Mathf.Round(casterUW.CharLevel / 2), 1f) < (float)num)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_are_not_experienced_enough_to_cast_spells_of_that_circle_));
			return false;
		}
		if (CurMana < num * 3)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_do_not_have_enough_mana_to_cast_the_spell_));
			return false;
		}
		int num2 = Mathf.Max(num * 3 - casterUW.PlayerSkills.GetSkill(10), 1);
		int num3 = UnityEngine.Random.Range(0, 31);
		if (num3 < num2)
		{
			switch (num3)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_spell_backfires_));
				casterUW.CurVIT -= 3;
				break;
			default:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_casting_was_not_successful_));
				break;
			}
			return false;
		}
		UWHUD.instance.MessageScroll.Add("Casting " + text);
		return true;
	}

	public void TestSpell(GameObject caster)
	{
	}

	private Vector3 GetBestSpellVector(GameObject casterToEval)
	{
		GameObject gameObject = ((!casterToEval.name.Contains("NPC_Launcher")) ? casterToEval : casterToEval.transform.parent.gameObject);
		if (gameObject.GetComponent<NPC>() != null)
		{
			if (gameObject.GetComponent<NPC>().gtargName == "_Gronk")
			{
				return (UWCharacter.Instance.GetImpactPoint() - gameObject.GetComponent<UWEBase>().GetImpactPoint()).normalized;
			}
			return (gameObject.GetComponent<NPC>().getGtarg().transform.position - gameObject.GetComponent<UWEBase>().GetImpactPoint()).normalized;
		}
		return gameObject.transform.forward;
	}

	public void castSpell(GameObject caster, string MagicWords, bool ready)
	{
		switch (MagicWords)
		{
		case "An An An":
			TestSpell(caster);
			break;
		case "In Mani Ylem":
			SetSpellCost(1);
			if (UWEBase._RES == "UW2")
			{
				Cast_InManiYlem(caster, 129);
			}
			else
			{
				Cast_InManiYlem(caster, 129);
			}
			break;
		case "In Lor":
			SetSpellCost(1);
			if (UWEBase._RES == "UW2")
			{
				Cast_LightSpells(caster, 3);
			}
			else
			{
				Cast_LightSpells(caster, 3);
			}
			break;
		case "Bet Wis Ex":
			SetSpellCost(1);
			Debug.Log(MagicWords + " Locate Cast");
			break;
		case "Ort Jux":
			SetSpellCost(1);
			if (UWEBase._RES == "UW2")
			{
				Cast_OrtJux(caster, ready, 81);
			}
			else
			{
				Cast_OrtJux(caster, ready, 81);
			}
			break;
		case "Bet In Sanct":
			SetSpellCost(1);
			if (UWEBase._RES == "UW2")
			{
				Cast_ResistanceSpells(caster, 34);
			}
			else
			{
				Cast_ResistanceSpells(caster, 34);
			}
			break;
		case "Sanct Hur":
			SetSpellCost(1);
			if (UWEBase._RES == "UW2")
			{
				Cast_StealthSpells(caster, 50);
			}
			else
			{
				Cast_StealthSpells(caster, 50);
			}
			break;
		case "Quas Corp":
			SetSpellCost(2);
			if (UWEBase._RES == "UW2")
			{
				Cast_QuasCorp(caster, 113);
			}
			else
			{
				Cast_QuasCorp(caster, 113);
			}
			break;
		case "Wis Mani":
			SetSpellCost(2);
			if (UWEBase._RES == "UW2")
			{
				Cast_DetectMonster(caster, 119);
			}
			else
			{
				Cast_DetectMonster(caster, 177);
			}
			break;
		case "Uus Por":
			SetSpellCost(2);
			if (UWEBase._RES == "UW2")
			{
				Cast_UusPor(caster, 17);
			}
			else
			{
				Cast_UusPor(caster, 17);
			}
			break;
		case "In Bet Mani":
			SetSpellCost(2);
			if (UWEBase._RES == "UW2")
			{
				Cast_Heal(caster, 64);
			}
			else
			{
				Cast_Heal(caster, 64);
			}
			break;
		case "Rel Des Por":
			SetSpellCost(2);
			if (UWEBase._RES == "UW2")
			{
				Cast_RelDesPor(caster, 18);
			}
			else
			{
				Cast_RelDesPor(caster, 18);
			}
			break;
		case "In Sanct":
			SetSpellCost(2);
			if (UWEBase._RES == "UW2")
			{
				Cast_ResistanceSpells(caster, 35);
			}
			else
			{
				Cast_ResistanceSpells(caster, 35);
			}
			break;
		case "In Jux":
			SetSpellCost(2);
			Cast_InJux(caster, 131);
			break;
		case "Quas Mani Ylem":
			SetSpellCost(2);
			Cast_DispelHunger(caster, 189);
			break;
		case "Bet Sanct Lor":
			SetSpellCost(3);
			if (UWEBase._RES == "UW2")
			{
				Cast_StealthSpells(caster, 51);
			}
			else
			{
				Cast_StealthSpells(caster, 51);
			}
			break;
		case "Ort Grav":
			SetSpellCost(3);
			if (UWEBase._RES == "UW2")
			{
				Cast_OrtGrav(caster, ready, 82);
			}
			else
			{
				Cast_OrtGrav(caster, ready, 82);
			}
			break;
		case "Quas Lor":
			SetSpellCost(3);
			if (UWEBase._RES == "UW2")
			{
				Cast_LightSpells(caster, 5);
			}
			else
			{
				Cast_LightSpells(caster, 5);
			}
			break;
		case "An Kal Corp":
			SetSpellCost(3);
			if (UWEBase._RES == "UW2")
			{
				Debug.Log(MagicWords + " Repel Undead Cast uw2 version");
			}
			else
			{
				Debug.Log(MagicWords + " Repel Undead Cast");
			}
			break;
		case "Rel Tym Por":
			SetSpellCost(3);
			if (UWEBase._RES == "UW2")
			{
				Cast_RelTymPor(caster, 176);
			}
			else
			{
				Cast_RelTymPor(caster, 176);
			}
			break;
		case "Ylem Por":
			SetSpellCost(3);
			if (UWEBase._RES == "UW2")
			{
				Cast_YlemPor(caster, 20);
			}
			else
			{
				Cast_YlemPor(caster, 20);
			}
			break;
		case "Sanct Jux":
			SetSpellCost(3);
			Cast_SanctJux(caster, ready, 178);
			break;
		case "An Sanct":
			SetSpellCost(4);
			if (UWEBase._RES == "UW2")
			{
				Cast_Curse(caster, 49);
			}
			else
			{
				Cast_Curse(caster, 49);
			}
			break;
		case "Sanct Flam":
			SetSpellCost(4);
			if (UWEBase._RES == "UW2")
			{
				Cast_SanctFlam(caster, 54);
			}
			else
			{
				Cast_SanctFlam(caster, 54);
			}
			break;
		case "In Mani":
			SetSpellCost(4);
			if (UWEBase._RES == "UW2")
			{
				Cast_Heal(caster, 68);
			}
			else
			{
				Cast_Heal(caster, 68);
			}
			break;
		case "Hur Por":
			SetSpellCost(4);
			if (UWEBase._RES == "UW2")
			{
				Cast_LevitateSpells(caster, 19);
			}
			else
			{
				Cast_LevitateSpells(caster, 19);
			}
			break;
		case "Nox Ylem":
			SetSpellCost(4);
			if (UWEBase._RES == "UW2")
			{
				Cast_NoxYlem(caster, 116);
			}
			else
			{
				Cast_NoxYlem(caster, 116);
			}
			break;
		case "An Jux":
			SetSpellCost(4);
			Debug.Log(MagicWords + " Remove Trap Cast");
			break;
		case "Por Flam":
			SetSpellCost(5);
			if (UWEBase._RES == "UW2")
			{
				Cast_PorFlam(caster, ready, 83);
			}
			else
			{
				Cast_PorFlam(caster, ready, 83);
			}
			break;
		case "Grav Sanct Por":
			SetSpellCost(5);
			if (UWEBase._RES == "UW2")
			{
				Cast_GravSanctPor(caster, 394);
			}
			else
			{
				Cast_GravSanctPor(caster, 393);
			}
			break;
		case "Ort Wis Ylem":
			SetSpellCost(5);
			if (UWEBase._RES == "UW2")
			{
				Cast_NameEnchantment(caster, ready, 123);
			}
			else
			{
				Cast_NameEnchantment(caster, ready, 180);
			}
			break;
		case "Ex Ylem":
			SetSpellCost(5);
			if (UWEBase._RES == "UW2")
			{
				Cast_ExYlem(caster, ready, 124);
			}
			else
			{
				Cast_ExYlem(caster, ready, 181);
			}
			break;
		case "An Nox":
			SetSpellCost(5);
			if (UWEBase._RES == "UW2")
			{
				Cast_AnNox(caster, 182);
			}
			else
			{
				Cast_AnNox(caster, 182);
			}
			break;
		case "An Corp Mani":
			SetSpellCost(5);
			if (UWEBase._RES == "UW2")
			{
				Cast_AnCorpMani(caster, 114);
			}
			else
			{
				Cast_AnCorpMani(caster, 114);
			}
			break;
		case "Vas In Lor":
			SetSpellCost(6);
			if (UWEBase._RES == "UW2")
			{
				Cast_LightSpells(caster, 6);
			}
			else
			{
				Cast_LightSpells(caster, 6);
			}
			break;
		case "Vas Rel Por":
			SetSpellCost(6);
			if (UWEBase._RES == "UW2")
			{
				Cast_VasRelPor(caster, 127);
			}
			else
			{
				Cast_VasRelPor(caster, 186);
			}
			break;
		case "Vas In Mani":
			SetSpellCost(6);
			if (UWEBase._RES == "UW2")
			{
				Cast_Heal(caster, 76);
			}
			else
			{
				Cast_Heal(caster, 76);
			}
			break;
		case "An Ex Por":
			SetSpellCost(6);
			if (UWEBase._RES == "UW2")
			{
				Cast_AnExPor(caster, 117);
			}
			else
			{
				Cast_AnExPor(caster, 117);
			}
			break;
		case "Vas Ort Grav":
			SetSpellCost(6);
			if (UWEBase._RES == "UW2")
			{
				Cast_VasOrtGrav(caster, 98, false);
			}
			else
			{
				Cast_VasOrtGrav(caster, 98, false);
			}
			break;
		case "Ort Por Ylem":
			SetSpellCost(6);
			if (UWEBase._RES == "UW2")
			{
				Cast_OrtPorYlem(caster, 184);
			}
			else
			{
				Cast_OrtPorYlem(caster, 184);
			}
			break;
		case "Wis Ex":
			SetSpellCost(6);
			if (UWEBase._RES == "UW2")
			{
				Cast_MapArea(caster, 215);
			}
			break;
		case "In Mani Rel":
			SetSpellCost(7);
			if (UWEBase._RES == "UW2")
			{
				Cast_InManiRel(caster, 115);
			}
			else
			{
				Cast_InManiRel(caster, 115);
			}
			break;
		case "Vas An Wis":
			SetSpellCost(7);
			if (UWEBase._RES == "UW2")
			{
				Cast_VasAnWis(caster, 99);
			}
			else
			{
				Cast_VasAnWis(caster, 99);
			}
			break;
		case "Vas Sanct Lor":
			SetSpellCost(7);
			if (UWEBase._RES == "UW2")
			{
				Cast_StealthSpells(caster, 52);
			}
			else
			{
				Cast_StealthSpells(caster, 52);
			}
			break;
		case "Vas Hur Por":
			SetSpellCost(7);
			if (UWEBase._RES == "UW2")
			{
				Cast_LevitateSpells(caster, 21);
			}
			else
			{
				Cast_LevitateSpells(caster, 21);
			}
			break;
		case "Kal Mani":
			SetSpellCost(7);
			if (UWEBase._RES == "UW2")
			{
				Cast_KalMani(caster, 132);
			}
			else
			{
				Cast_KalMani(caster, 132);
			}
			break;
		case "Ort An Quas":
			SetSpellCost(7);
			Debug.Log(MagicWords + " Reveal Cast");
			break;
		case "Vas Kal Corp":
			SetSpellCost(8);
			if (UWEBase._RES == "UW2")
			{
				Cast_VasKalCorp(caster, 188);
			}
			else
			{
				Cast_VasKalCorp(caster, 188);
			}
			break;
		case "Flam Hur":
			SetSpellCost(8);
			if (UWEBase._RES == "UW2")
			{
				Cast_FlamHur(caster, 100);
			}
			else
			{
				Cast_FlamHur(caster, 100);
			}
			break;
		case "An Tym":
			SetSpellCost(8);
			if (UWEBase._RES == "UW2")
			{
				Cast_AnTym(caster, 187);
			}
			else
			{
				Cast_AnTym(caster, 187);
			}
			break;
		case "In Vas Sanct":
			SetSpellCost(8);
			if (UWEBase._RES == "UW2")
			{
				Cast_ResistanceSpells(caster, 37);
				IronFleshXClock();
			}
			else
			{
				Cast_ResistanceSpells(caster, 37);
			}
			break;
		case "Ort Por Wis":
			SetSpellCost(8);
			if (UWEBase._RES == "UW2")
			{
				Cast_OrtPorWis(caster, 183);
			}
			else
			{
				Cast_OrtPorWis(caster, 183);
			}
			break;
		case "Vas Por Ylem":
			SetSpellCost(8);
			if (UWEBase._RES == "UW2")
			{
				Cast_VasPorYlem(caster, 185);
			}
			else
			{
				Cast_VasPorYlem(caster, 185);
			}
			break;
		case "Deadly Seeker":
			Cast_DeadlySeeker(caster, false, 85);
			break;
		default:
			Debug.Log("Unknown spell cast:" + MagicWords);
			break;
		}
	}

	public string TranslateSpellRune(int Rune1, int Rune2, int Rune3)
	{
		string text = "";
		if (Rune1 >= 0 && Rune1 <= 23)
		{
			text = Runes[Rune1];
		}
		if (Rune2 >= 0 && Rune2 <= 23)
		{
			text = text + " " + Runes[Rune2];
		}
		if (Rune3 >= 0 && Rune3 <= 23)
		{
			text = text + " " + Runes[Rune3];
		}
		return text;
	}

	public void castSpell(GameObject caster, int Rune1, int Rune2, int Rune3, bool ready)
	{
		string text = "";
		text = TranslateSpellRune(Rune1, Rune2, Rune3);
		castSpell(caster, text, ready);
	}

	private void Cast_OrtJux(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			ReadiedSpell = "Ort Jux";
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconTarget;
		}
		else
		{
			SpellProp_MagicArrow spellProp_MagicArrow = new SpellProp_MagicArrow();
			spellProp_MagicArrow.init(EffectID, caster);
			CastProjectile(caster, spellProp_MagicArrow);
		}
	}

	private void Cast_OrtGrav(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			ReadiedSpell = "Ort Grav";
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconTarget;
		}
		else
		{
			SpellProp_ElectricBolt spellProp_ElectricBolt = new SpellProp_ElectricBolt();
			spellProp_ElectricBolt.init(EffectID, caster);
			CastProjectile(caster, spellProp_ElectricBolt);
		}
	}

	private void Cast_Acid(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconTarget;
			return;
		}
		SpellProp_Acid spellProp_Acid = new SpellProp_Acid();
		spellProp_Acid.init(EffectID, caster);
		CastProjectile(caster, spellProp_Acid);
	}

	private void Cast_PorFlam(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			ReadiedSpell = "Por Flam";
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconTarget;
			return;
		}
		SpellProp_Fireball spellProp_Fireball = new SpellProp_Fireball();
		spellProp_Fireball.init(EffectID, caster);
		spellProp_Fireball.caster = caster;
		CastProjectile(caster, spellProp_Fireball);
	}

	private void Cast_FlamHur(GameObject caster, int EffectID)
	{
		SpellProp_FlameWind spellProp_FlameWind = new SpellProp_FlameWind();
		spellProp_FlameWind.init(EffectID, caster);
		spellProp_FlameWind.caster = caster;
		CastProjectile(caster, spellProp_FlameWind);
	}

	private void Cast_VasOrtGrav(GameObject caster, int EffectID, bool CastFromWindow)
	{
		SpellProp_SheetLightning spellProp_SheetLightning = new SpellProp_SheetLightning();
		spellProp_SheetLightning.init(EffectID, caster);
		spellProp_SheetLightning.CastRaySource = CastFromWindow;
		CastProjectile(caster, spellProp_SheetLightning);
	}

	private void Cast_DeadlySeeker(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			ReadiedSpell = "Deadly Seeker";
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconTarget;
			return;
		}
		SpellProp_Homing spellProp_Homing = new SpellProp_Homing();
		spellProp_Homing.init(EffectID, caster);
		spellProp_Homing.caster = caster;
		CastProjectile(caster, spellProp_Homing);
	}

	private void Cast_ExYlem(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			UWCharacter.Instance.PlayerMagic.ReadiedSpell = "Ex Ylem";
			UWHUD.instance.CursorIcon = GameWorldController.instance.grCursors.LoadImageAt(10);
			return;
		}
		UWCharacter.Instance.PlayerMagic.ReadiedSpell = "";
		UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
		Ray ray = getRay(caster);
		RaycastHit hitInfo = default(RaycastHit);
		float useRange = UWCharacter.Instance.GetUseRange();
		if (Physics.Raycast(ray, out hitInfo, useRange))
		{
			DoorControl component = hitInfo.transform.gameObject.GetComponent<DoorControl>();
			if (component != null)
			{
				component.UnlockDoor(true);
			}
			else if (hitInfo.transform.GetComponent<PortcullisInteraction>() != null)
			{
				hitInfo.transform.GetComponent<PortcullisInteraction>().getParentObjectInteraction().gameObject.GetComponent<DoorControl>().UnlockDoor(true);
			}
		}
	}

	private void Cast_SanctJux(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			UWCharacter.Instance.PlayerMagic.ReadiedSpell = "Sanct Jux";
			UWHUD.instance.CursorIcon = GameWorldController.instance.grCursors.LoadImageAt(10);
			return;
		}
		UWCharacter.Instance.PlayerMagic.ReadiedSpell = "";
		UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
		Ray ray = getRay(caster);
		RaycastHit hitInfo = default(RaycastHit);
		float useRange = UWCharacter.Instance.GetUseRange();
		if (Physics.Raycast(ray, out hitInfo, useRange))
		{
			DoorControl component = hitInfo.transform.gameObject.GetComponent<DoorControl>();
			if (component != null)
			{
				component.Spike();
			}
		}
	}

	private void Cast_AnNox(GameObject caster, int EffectID)
	{
		UWCharacter.Instance.play_poison = 0;
	}

	private void Cast_GravSanctPor(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_ResistanceAgainstType(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_AnTym(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_FreezeTime(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_FreezeTime(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		SpellEffectFreezeTime spellEffectFreezeTime = (SpellEffectFreezeTime)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectFreezeTime.counter = spellProp_Mind.counter;
		long key = UnityEngine.Random.Range(1, 200000);
		spellEffectFreezeTime.Key = key;
		spellEffectFreezeTime.Go();
	}

	private void Cast_LightSpells(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Light(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_InManiYlem(GameObject caster, int EffectID)
	{
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hitInfo = default(RaycastHit);
		float num = 1.2f;
		if (!Physics.Raycast(ray, out hitInfo, num))
		{
			ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(176 + UnityEngine.Random.Range(0, 7), 40, 0, 1, 256);
			objectLoaderInfo.is_quant = 1;
			objectLoaderInfo.InUseFlag = 1;
			UWEBase.UnFreezeMovement(GameWorldController.MoveToWorld(ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, ray.GetPoint(num))).gameObject);
		}
	}

	private void Cast_KalMani(GameObject caster, int EffectID)
	{
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hitInfo = default(RaycastHit);
		float num = 1.2f;
		if (!Physics.Raycast(ray, out hitInfo, num))
		{
			int num2 = 1;
			NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hitInfo);
			if (nPCTargetRandom != null && nPCTargetRandom.npc_attitude == 0)
			{
				num2 = nPCTargetRandom.GetComponent<ObjectInteraction>().objectloaderinfo.index;
			}
			SpellProp_SummonMonster spellProp_SummonMonster = new SpellProp_SummonMonster();
			spellProp_SummonMonster.init(132, caster);
			ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(spellProp_SummonMonster.RndNPC, 0, 0, 0, 2);
			GameObject gameObject = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, ray.GetPoint(num)).gameObject;
			gameObject.GetComponent<NPC>().npc_gtarg = (short)num2;
			gameObject.GetComponent<NPC>().npc_goal = 3;
			gameObject.GetComponent<NPC>().npc_hp = GameWorldController.instance.objDat.critterStats[spellProp_SummonMonster.RndNPC - 64].AvgHit;
			objectLoaderInfo.InUseFlag = 1;
			UWEBase.UnFreezeMovement(gameObject);
		}
	}

	private void Cast_VasKalCorp(GameObject caster, int EffectID)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if ((array[i].layer == LayerMask.NameToLayer("UWObjects") || array[i].layer == LayerMask.NameToLayer("NPCs") || array[i].layer == LayerMask.NameToLayer("Doors")) && array[i].transform.parent == null)
			{
				array[i].SetActive(false);
			}
		}
	}

	private void Cast_UusPor(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Leap(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_RelDesPor(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_SlowFall(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_NoxYlem(GameObject caster, int EffectID)
	{
		RaycastHit hit = default(RaycastHit);
		NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hit);
		if (nPCTargetRandom != null)
		{
			SpellProp_Poison spellProp_Poison = new SpellProp_Poison();
			spellProp_Poison.init(EffectID, caster);
			Impact.SpawnHitImpact(Impact.ImpactMagic(), nPCTargetRandom.GetImpactPoint(), spellProp_Poison.impactFrameStart, spellProp_Poison.impactFrameEnd);
			int num = CheckPassiveSpellEffectNPC(nPCTargetRandom.gameObject);
			if (num != -1)
			{
				SpellEffectPoison spellEffectPoison = (SpellEffectPoison)SetSpellEffect(nPCTargetRandom.gameObject, nPCTargetRandom.NPCStatusEffects, num, EffectID);
				spellEffectPoison.Value = spellProp_Poison.BaseDamage;
				spellEffectPoison.counter = spellProp_Poison.counter;
				spellEffectPoison.isNPC = true;
				spellEffectPoison.Go();
			}
		}
	}

	private void Cast_Curse(GameObject caster, int EffectID)
	{
		RaycastHit hit = default(RaycastHit);
		NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hit);
		if (nPCTargetRandom != null)
		{
			SpellProp_Curse spellProp_Curse = new SpellProp_Curse();
			spellProp_Curse.init(EffectID, caster);
			Impact.SpawnHitImpact(Impact.ImpactMagic(), nPCTargetRandom.GetImpactPoint(), spellProp_Curse.impactFrameStart, spellProp_Curse.impactFrameEnd);
			int num = CheckPassiveSpellEffectNPC(nPCTargetRandom.gameObject);
			if (num != -1)
			{
				SpellEffectCurse spellEffectCurse = (SpellEffectCurse)SetSpellEffect(nPCTargetRandom.gameObject, nPCTargetRandom.NPCStatusEffects, num, EffectID);
				spellEffectCurse.isNPC = true;
				spellEffectCurse.Go();
			}
		}
	}

	private void Cast_InManiRel(GameObject caster, int EffectID)
	{
		RaycastHit hit = default(RaycastHit);
		NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hit);
		if (!(nPCTargetRandom != null))
		{
			return;
		}
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		Impact.SpawnHitImpact(Impact.ImpactMagic(), nPCTargetRandom.GetImpactPoint(), spellProp_Mind.impactFrameStart, spellProp_Mind.impactFrameEnd);
		if (nPCTargetRandom.gameObject.GetComponent<SpellEffectAlly>() != null)
		{
			nPCTargetRandom.gameObject.GetComponent<SpellEffectAlly>().counter = spellProp_Mind.counter;
			return;
		}
		int num = CheckPassiveSpellEffectNPC(nPCTargetRandom.gameObject);
		if (num != -1)
		{
			SpellEffectAlly spellEffectAlly = (SpellEffectAlly)SetSpellEffect(nPCTargetRandom.gameObject, nPCTargetRandom.NPCStatusEffects, num, EffectID);
			spellEffectAlly.counter = spellProp_Mind.counter;
			spellEffectAlly.Go();
		}
	}

	private void Cast_VasAnWis(GameObject caster, int EffectID)
	{
		RaycastHit hit = default(RaycastHit);
		NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hit);
		if (!(nPCTargetRandom != null))
		{
			return;
		}
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		Impact.SpawnHitImpact(Impact.ImpactMagic(), nPCTargetRandom.GetImpactPoint(), spellProp_Mind.impactFrameStart, spellProp_Mind.impactFrameEnd);
		if (nPCTargetRandom.gameObject.GetComponent<SpellEffectConfusion>() != null)
		{
			nPCTargetRandom.gameObject.GetComponent<SpellEffectConfusion>().counter = 5;
			return;
		}
		int num = CheckPassiveSpellEffectNPC(nPCTargetRandom.gameObject);
		if (num != -1)
		{
			SpellEffectConfusion spellEffectConfusion = (SpellEffectConfusion)SetSpellEffect(nPCTargetRandom.gameObject, nPCTargetRandom.NPCStatusEffects, num, EffectID);
			spellEffectConfusion.counter = spellProp_Mind.counter;
			spellEffectConfusion.Go();
		}
	}

	private void Cast_QuasCorp(GameObject caster, int EffectID)
	{
		RaycastHit hit = default(RaycastHit);
		NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hit);
		if (!(nPCTargetRandom != null))
		{
			return;
		}
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		Impact.SpawnHitImpact(Impact.ImpactMagic(), nPCTargetRandom.GetImpactPoint(), spellProp_Mind.impactFrameStart, spellProp_Mind.impactFrameEnd);
		if (nPCTargetRandom.gameObject.GetComponent<SpellEffectFear>() != null)
		{
			nPCTargetRandom.gameObject.GetComponent<SpellEffectFear>().counter = 5;
			return;
		}
		int num = CheckPassiveSpellEffectNPC(nPCTargetRandom.gameObject);
		if (num != -1)
		{
			SpellEffectFear spellEffectFear = (SpellEffectFear)SetSpellEffect(nPCTargetRandom.gameObject, nPCTargetRandom.NPCStatusEffects, num, 113);
			spellEffectFear.counter = spellProp_Mind.counter;
			spellEffectFear.Go();
		}
	}

	private void Cast_AnExPor(GameObject caster, int EffectID)
	{
		RaycastHit hit = default(RaycastHit);
		NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hit);
		if (nPCTargetRandom != null)
		{
			SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
			spellProp_Mind.init(EffectID, caster);
			Impact.SpawnHitImpact(Impact.ImpactMagic(), nPCTargetRandom.GetImpactPoint(), spellProp_Mind.impactFrameStart, spellProp_Mind.impactFrameEnd);
			int num = CheckPassiveSpellEffectNPC(nPCTargetRandom.gameObject);
			if (num != -1)
			{
				SpellEffectParalyze spellEffectParalyze = (SpellEffectParalyze)SetSpellEffect(nPCTargetRandom.gameObject, nPCTargetRandom.NPCStatusEffects, num, EffectID);
				spellEffectParalyze.isNPC = true;
				spellEffectParalyze.counter = spellProp_Mind.counter;
				spellEffectParalyze.Go();
			}
		}
	}

	private void Cast_AnCorpMani(GameObject caster, int EffectID)
	{
		RaycastHit hit = default(RaycastHit);
		NPC nPCTargetRandom = GetNPCTargetRandom(caster, ref hit, 1);
		if (nPCTargetRandom != null)
		{
			SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
			spellProp_Mind.init(EffectID, caster);
			Impact.SpawnHitImpact(Impact.ImpactMagic(), nPCTargetRandom.GetImpactPoint(), spellProp_Mind.impactFrameStart, spellProp_Mind.impactFrameEnd);
			SpellProp_DirectDamage spellProp_DirectDamage = new SpellProp_DirectDamage();
			spellProp_DirectDamage.init(EffectID, caster);
			nPCTargetRandom.ApplyAttack(spellProp_DirectDamage.BaseDamage, caster);
		}
	}

	private void Cast_OrtPorYlem(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Telekinesis(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_VasRelPor(GameObject caster, int EffectID)
	{
		if (UWCharacter.Instance.MoonGateLevel != 0)
		{
			if (UWCharacter.Instance.MoonGateLevel != GameWorldController.instance.LevelNo + 1)
			{
				if (UWEBase._RES == "UW1")
				{
					UWCharacter.ResetTrueMana();
				}
				GameWorldController.instance.SwitchLevel((short)(UWCharacter.Instance.MoonGateLevel - 1));
			}
			caster.transform.position = UWCharacter.Instance.MoonGatePosition;
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_moonstone_is_not_available_));
		}
	}

	private void Cast_LevitateSpells(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Levitate(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_RelTymPor(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Speed(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_YlemPor(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_WaterWalk(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_ResistanceSpells(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Resistance(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_SanctFlam(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Flameproof(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_OrtPorWis(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_RoamingSight(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_StealthSpells(GameObject caster, int EffectID)
	{
		int num = CheckActiveSpellEffect(caster);
		if (num != -1)
		{
			Cast_Stealth(caster, caster.GetComponent<UWCharacter>().ActiveSpell, EffectID, num);
		}
		else
		{
			SpellIncantationFailed(caster);
		}
	}

	private void Cast_DetectMonster(GameObject caster, int EffectID)
	{
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		Skills.TrackMonsters(caster, spellProp_Mind.BaseDamage, true);
	}

	private void Cast_NameEnchantment(GameObject caster, bool Ready, int EffectID)
	{
		if (Ready)
		{
			InventorySpell = true;
			UWCharacter.Instance.PlayerMagic.ReadiedSpell = "Ort Wis Ylem";
			UWHUD.instance.CursorIcon = GameWorldController.instance.grCursors.LoadImageAt(10);
			return;
		}
		InventorySpell = false;
		if (WindowDetect.CursorInMainWindow)
		{
			UWCharacter.Instance.PlayerMagic.ReadiedSpell = "";
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
			Ray ray = getRay(caster);
			RaycastHit hitInfo = default(RaycastHit);
			float useRange = UWCharacter.Instance.GetUseRange();
			if (Physics.Raycast(ray, out hitInfo, useRange))
			{
				ObjectInteraction component = hitInfo.transform.gameObject.GetComponent<ObjectInteraction>();
				if (component != null)
				{
					component.heading = 7;
					component.LookDescription();
				}
			}
		}
		else if (ObjectInSlot != null)
		{
			ReadiedSpell = "";
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
			if (ObjectInSlot != null)
			{
				ObjectInSlot.heading = 7;
				ObjectInSlot.LookDescription();
			}
		}
	}

	private void Cast_VasPorYlem(GameObject caster, int EffectID)
	{
		TileMap tileMap = UWEBase.CurrentTileMap();
		for (int i = 0; i <= UnityEngine.Random.Range(1, 4); i++)
		{
			Vector3 vector = caster.transform.position + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(1, 3);
			if (tileMap.ValidTile(vector))
			{
				vector.Set(vector.x, 4.5f, vector.z);
				ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(386, 40, 0, 0, 256);
				GameObject gameObject = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, vector).gameObject;
				gameObject.GetComponent<a_arrow_trap>().ExecuteTrap(gameObject.GetComponent<a_arrow_trap>(), 0, 0, 0);
				objectLoaderInfo.InUseFlag = 1;
			}
		}
	}

	private void Cast_InJux(GameObject caster, int EffectID)
	{
		Cast_RuneOfWarding(caster.transform.position + base.transform.forward * 0.3f, EffectID);
	}

	private void CastTheFrog(GameObject caster, int EffectID)
	{
		a_do_trapBullfrog a_do_trapBullfrog2 = (a_do_trapBullfrog)UnityEngine.Object.FindObjectOfType(typeof(a_do_trapBullfrog));
		if (a_do_trapBullfrog2 != null)
		{
			a_do_trapBullfrog2.ResetBullFrog();
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 191));
		}
	}

	private void Cast_Heal(GameObject caster, int EffectID)
	{
		SpellProp_Heal spellProp_Heal = new SpellProp_Heal();
		spellProp_Heal.init(EffectID, caster);
		int baseDamage = spellProp_Heal.BaseDamage;
		UWCharacter.Instance.CurVIT = UWCharacter.Instance.CurVIT + baseDamage;
		if (UWCharacter.Instance.CurVIT > UWCharacter.Instance.MaxVIT)
		{
			UWCharacter.Instance.CurVIT = UWCharacter.Instance.MaxVIT;
		}
	}

	private void Cast_Resistance(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Resistance spellProp_Resistance = new SpellProp_Resistance();
		spellProp_Resistance.init(EffectID, caster);
		SpellEffectResistance spellEffectResistance = (SpellEffectResistance)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectResistance.Value = spellProp_Resistance.BaseDamage;
		spellEffectResistance.counter = spellProp_Resistance.counter;
		spellEffectResistance.Go();
	}

	private void Cast_Flameproof(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_ResistanceAgainstType spellProp_ResistanceAgainstType = new SpellProp_ResistanceAgainstType();
		spellProp_ResistanceAgainstType.init(EffectID, caster);
		SpellEffectFlameproof spellEffectFlameproof = (SpellEffectFlameproof)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectFlameproof.counter = spellProp_ResistanceAgainstType.counter;
		spellEffectFlameproof.Go();
	}

	private void Cast_ResistPoison(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_ResistanceAgainstType spellProp_ResistanceAgainstType = new SpellProp_ResistanceAgainstType();
		spellProp_ResistanceAgainstType.init(EffectID, caster);
		SpellEffectImmunityPoison spellEffectImmunityPoison = (SpellEffectImmunityPoison)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectImmunityPoison.counter = spellProp_ResistanceAgainstType.counter;
		spellEffectImmunityPoison.Go();
	}

	private void Cast_Mana(GameObject caster, int EffectID)
	{
		SpellProp_Mana spellProp_Mana = new SpellProp_Mana();
		spellProp_Mana.init(EffectID, caster);
		int baseDamage = spellProp_Mana.BaseDamage;
		UWCharacter.Instance.PlayerMagic.CurMana = UWCharacter.Instance.PlayerMagic.CurMana + baseDamage;
		if (UWCharacter.Instance.PlayerMagic.CurMana > UWCharacter.Instance.PlayerMagic.MaxMana)
		{
			UWCharacter.Instance.PlayerMagic.CurMana = UWCharacter.Instance.PlayerMagic.MaxMana;
		}
	}

	private void Cast_Light(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Light spellProp_Light = new SpellProp_Light();
		spellProp_Light.init(EffectID, caster);
		LightSource.MagicBrightness = spellProp_Light.BaseDamage;
		SpellEffect spellEffect = (SpellEffectLight)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.Value = spellProp_Light.BaseDamage;
		spellEffect.counter = spellProp_Light.counter;
		spellEffect.Go();
	}

	private void Cast_NightVision(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Light spellProp_Light = new SpellProp_Light();
		spellProp_Light.init(EffectID, caster);
		LightSource.MagicBrightness = spellProp_Light.BaseDamage;
		SpellEffect spellEffect = (SpellEffectNightVision)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.Value = spellProp_Light.BaseDamage;
		spellEffect.counter = spellProp_Light.counter;
		spellEffect.Go();
	}

	private void Cast_Hallucination(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectHallucination)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Mind.counter;
		spellEffect.Go();
	}

	private void Cast_Leap(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Movement spellProp_Movement = new SpellProp_Movement();
		spellProp_Movement.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectLeap)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Movement.counter;
		spellEffect.Go();
	}

	private void Cast_Luck(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Luck spellProp_Luck = new SpellProp_Luck();
		spellProp_Luck.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectLucky)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Luck.counter;
		spellEffect.Go();
	}

	private void Cast_Bounce(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Bounce spellProp_Bounce = new SpellProp_Bounce();
		spellProp_Bounce.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectBounce)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Bounce.counter;
		spellEffect.Go();
	}

	private void Cast_ManaRegen(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Regen spellProp_Regen = new SpellProp_Regen();
		spellProp_Regen.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectRegenerationMana)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Regen.counter;
		spellEffect.Value = spellProp_Regen.BaseDamage;
		spellEffect.Go();
	}

	private void Cast_HealthRegen(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Regen spellProp_Regen = new SpellProp_Regen();
		spellProp_Regen.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectRegenerationHealth)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Regen.counter;
		spellEffect.Value = spellProp_Regen.BaseDamage;
		spellEffect.Go();
	}

	private void Cast_SlowFall(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Movement spellProp_Movement = new SpellProp_Movement();
		spellProp_Movement.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectSlowFall)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Movement.counter;
		spellEffect.Go();
	}

	private void Cast_RoamingSight(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		SpellEffect spellEffect = (SpellEffectRoamingSight)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffect.counter = spellProp_Mind.counter;
		spellEffect.Go();
	}

	private void Cast_Stealth(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Stealth spellProp_Stealth = new SpellProp_Stealth();
		spellProp_Stealth.init(EffectID, caster);
		SpellEffectStealth spellEffectStealth = (SpellEffectStealth)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectStealth.counter = spellProp_Stealth.counter;
		spellEffectStealth.StealthLevel = spellProp_Stealth.StealthLevel;
		spellEffectStealth.Go();
	}

	private void Cast_RuneOfWarding(Vector3 pos, int EffectID)
	{
		ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(393, 40, 0, 0, 256);
		ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, pos);
		objectLoaderInfo.InUseFlag = 1;
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 276));
	}

	public void Cast_Poison(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		if (caster.name != UWCharacter.Instance.name)
		{
			SpellProp_Poison spellProp_Poison = new SpellProp_Poison();
			spellProp_Poison.init(EffectID, caster);
			SpellEffectPoison spellEffectPoison = (SpellEffectPoison)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
			spellEffectPoison.Value = spellProp_Poison.BaseDamage;
			spellEffectPoison.counter = spellProp_Poison.counter;
			spellEffectPoison.isNPC = true;
			spellEffectPoison.Go();
		}
	}

	public void Cast_ResistanceAgainstType(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_ResistanceAgainstType spellProp_ResistanceAgainstType = new SpellProp_ResistanceAgainstType();
		spellProp_ResistanceAgainstType.init(EffectID, caster);
		SpellEffectResistanceAgainstType spellEffectResistanceAgainstType = (SpellEffectResistanceAgainstType)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectResistanceAgainstType.counter = spellProp_ResistanceAgainstType.counter;
		spellEffectResistanceAgainstType.Go();
	}

	public void Cast_Paralyze(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		SpellEffectParalyze spellEffectParalyze = (SpellEffectParalyze)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectParalyze.counter = spellProp_Mind.counter;
		if (caster.name != UWCharacter.Instance.name)
		{
			spellEffectParalyze.isNPC = true;
		}
		spellEffectParalyze.Go();
	}

	public void Cast_Telekinesis(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		SpellEffectTelekinesis spellEffectTelekinesis = (SpellEffectTelekinesis)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectTelekinesis.counter = spellProp_Mind.counter;
		spellEffectTelekinesis.Go();
	}

	public void Cast_Levitate(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Movement spellProp_Movement = new SpellProp_Movement();
		spellProp_Movement.init(EffectID, caster);
		SpellEffectLevitate spellEffectLevitate = (SpellEffectLevitate)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectLevitate.counter = spellProp_Movement.counter;
		spellEffectLevitate.Go();
	}

	public void Cast_Speed(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Movement spellProp_Movement = new SpellProp_Movement();
		spellProp_Movement.init(EffectID, caster);
		SpellEffectSpeed spellEffectSpeed = (SpellEffectSpeed)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectSpeed.counter = spellProp_Movement.counter;
		spellEffectSpeed.speedMultiplier = spellProp_Movement.Speed;
		spellEffectSpeed.Go();
	}

	public void Cast_WaterWalk(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Movement spellProp_Movement = new SpellProp_Movement();
		spellProp_Movement.init(EffectID, caster);
		SpellEffectWaterWalk spellEffectWaterWalk = (SpellEffectWaterWalk)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectWaterWalk.counter = spellProp_Movement.counter;
		spellEffectWaterWalk.Go();
	}

	public void Cast_MazeNavigation(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Mind spellProp_Mind = new SpellProp_Mind();
		spellProp_Mind.init(EffectID, caster);
		SpellEffectMazeNavigation spellEffectMazeNavigation = (SpellEffectMazeNavigation)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectMazeNavigation.counter = spellProp_Mind.counter;
		spellEffectMazeNavigation.Go();
	}

	public void Cast_CursedItem(GameObject caster, SpellEffect[] ActiveSpellArray, int EffectID, int EffectSlot)
	{
		SpellProp_Curse spellProp_Curse = new SpellProp_Curse();
		spellProp_Curse.init(EffectID, caster);
		SpellEffectCurse spellEffectCurse = (SpellEffectCurse)SetSpellEffect(caster, ActiveSpellArray, EffectSlot, EffectID);
		spellEffectCurse.counter = spellProp_Curse.counter;
		if (caster.name != UWCharacter.Instance.name)
		{
			spellEffectCurse.isNPC = true;
		}
		spellEffectCurse.Go();
	}

	public void Cast_StoneStrike(GameObject caster, GameObject target, int effectID)
	{
		if (UnityEngine.Random.Range(1, 6) > 4)
		{
			SpellEffectPetrified spellEffectPetrified = target.AddComponent<SpellEffectPetrified>();
			spellEffectPetrified.counter = (short)UnityEngine.Random.Range(5, 15);
			spellEffectPetrified.Go();
		}
	}

	private void SpellIncantationFailed(GameObject caster)
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_incantation_failed_));
	}

	private NPC GetNPCTargetRandom(GameObject caster, ref RaycastHit hit)
	{
		return GetNPCTargetRandom(caster, ref hit, 0);
	}

	public static NPC GetNPCTargetRandom(GameObject caster, ref RaycastHit hit, int isUndead)
	{
		Camera componentInChildren = caster.GetComponentInChildren<Camera>();
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(componentInChildren);
		Collider[] array = Physics.OverlapSphere(caster.transform.position, 5f);
		foreach (Collider collider in array)
		{
			bool flag = false;
			if (!(collider.gameObject.GetComponent<NPC>() != null))
			{
				continue;
			}
			switch (isUndead)
			{
			case 0:
				flag = true;
				break;
			case 1:
				if (collider.gameObject.GetComponent<NPC>().isUndead)
				{
					flag = true;
				}
				break;
			case 2:
				if (!collider.gameObject.GetComponent<NPC>().isUndead)
				{
					flag = true;
				}
				break;
			}
			if (flag && GeometryUtility.TestPlanesAABB(planes, collider.bounds))
			{
				Vector3 start = Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
				if (!Physics.Linecast(start, collider.gameObject.transform.position, out hit))
				{
					return collider.gameObject.GetComponent<NPC>();
				}
				if (hit.collider.gameObject.name == collider.gameObject.name)
				{
					return collider.gameObject.GetComponent<NPC>();
				}
			}
		}
		return null;
	}

	public static NPC GetNPCTargetRandom(GameObject caster, float range)
	{
		Collider[] array = Physics.OverlapSphere(caster.transform.position, range);
		foreach (Collider collider in array)
		{
			if (collider.gameObject.GetComponent<NPC>() != null)
			{
				return collider.gameObject.GetComponent<NPC>();
			}
		}
		return null;
	}

	private void Cast_Altara(GameObject caster, int EffectID)
	{
		Debug.Log("Perform the wand of altara spell");
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = -1;
		switch (GameWorldController.instance.LevelNo)
		{
		case 15:
			num = 27;
			num3 = 31;
			num2 = 29;
			num4 = 35;
			num5 = 0;
			break;
		case 17:
			num = 28;
			num3 = 26;
			num2 = 31;
			num4 = 30;
			num5 = 1;
			break;
		case 25:
			num = 18;
			num3 = 48;
			num2 = 21;
			num4 = 51;
			num5 = 2;
			break;
		case 32:
			num = 56;
			num3 = 2;
			num2 = 60;
			num4 = 6;
			num5 = 3;
			break;
		case 47:
			num = 35;
			num3 = 29;
			num2 = 37;
			num4 = 32;
			num5 = 4;
			break;
		case 48:
			num = 23;
			num3 = 39;
			num2 = 25;
			num4 = 41;
			num5 = 5;
			break;
		case 56:
			num = 28;
			num3 = 59;
			num2 = 34;
			num4 = 61;
			num5 = 6;
			break;
		case 68:
			num = 29;
			num3 = 25;
			num2 = 35;
			num4 = 31;
			num5 = 7;
			break;
		}
		int num6 = 1;
		if (num5 != -1)
		{
			num6 = (Quest.instance.QuestVariables[128] >> num5) & 1;
		}
		if (num6 == 0)
		{
			int visitTileX = TileMap.visitTileX;
			int visitTileY = TileMap.visitTileY;
			if (visitTileX >= num && visitTileY >= num3 && visitTileX <= num2 && visitTileY <= num4)
			{
				UWHUD.instance.MessageScroll.Add("Imagine the screen is shaking now");
				num6 = 1 << num5;
				Quest.instance.QuestVariables[128] |= num6;
				return;
			}
		}
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 322));
	}

	private void Cast_MapArea(GameObject caster, int EffectID)
	{
		for (int i = -5; i <= 5; i++)
		{
			for (int j = -5; j <= 5; j++)
			{
				if (TileMap.ValidTile(TileMap.visitTileX + i, TileMap.visitTileY + j))
				{
					UWEBase.CurrentAutoMap().MarkTile(TileMap.visitTileX + i, TileMap.visitTileY + j, UWEBase.CurrentTileMap().Tiles[TileMap.visitTileX + i, TileMap.visitTileY + j].tileType, AutoMap.GetDisplayType(UWEBase.CurrentTileMap().Tiles[TileMap.visitTileX + i, TileMap.visitTileY + j]));
				}
			}
		}
	}

	private void Cast_DispelHunger(GameObject caster, int EffectID)
	{
		UWCharacter.Instance.FoodLevel = 255;
	}

	private SpellEffect SetSpellEffect(GameObject caster, SpellEffect[] ActiveSpellArray, int index, int EffectID)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (EffectID)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
			case 7:
			case 261:
			case 298:
			case 407:
			case 410:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectLight>();
				break;
			case 17:
			case 270:
			case 384:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectLeap>();
				break;
			case 18:
			case 269:
			case 385:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectSlowFall>();
				break;
			case 19:
			case 21:
			case 288:
			case 313:
			case 386:
			case 388:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectLevitate>();
				break;
			case 20:
			case 279:
			case 387:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectWaterWalk>();
				break;
			case 34:
			case 35:
			case 37:
			case 259:
			case 285:
			case 315:
			case 405:
			case 406:
			case 408:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectResistance>();
				break;
			case 50:
			case 51:
			case 52:
			case 301:
			case 391:
			case 392:
			case 393:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectStealth>();
				break;
			case 53:
			case 56:
			case 57:
			case 287:
			case 394:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectResistanceAgainstType>();
				break;
			case 54:
			case 284:
			case 395:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectFlameproof>();
				break;
			case 55:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectImmunityPoison>();
				break;
			case 176:
			case 278:
			case 403:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectSpeed>();
				break;
			case 184:
			case 295:
			case 404:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectTelekinesis>();
				break;
			case 187:
			case 314:
			case 401:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectFreezeTime>();
				break;
			case 190:
			case 214:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectRegenerationHealth>();
				break;
			case 191:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectRegenerationMana>();
				break;
			case 212:
			case 213:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectHallucination>();
				break;
			case 5:
			case 276:
			case 409:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectNightVision>();
				break;
			case 116:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectPoison>();
				break;
			case 101:
			case 117:
			case 303:
			case 320:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectParalyze>();
				break;
			case 115:
			case 296:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectAlly>();
				break;
			case 99:
			case 304:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectConfusion>();
				break;
			case 113:
			case 264:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectFear>();
				break;
			case 448:
			case 449:
			case 450:
			case 451:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectAccuracy>();
				break;
			case 452:
			case 453:
			case 454:
			case 455:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectDamage>();
				break;
			case 464:
			case 465:
			case 466:
			case 467:
			case 468:
			case 469:
			case 470:
			case 471:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectProtection>();
				break;
			case 472:
			case 473:
			case 474:
			case 475:
			case 476:
			case 477:
			case 478:
			case 479:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectToughness>();
				break;
			case 183:
			case 317:
			case 402:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectRoamingSight>();
				break;
			case 49:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectCurse>();
				break;
			case 257:
			case 390:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectLucky>();
				break;
			case 22:
			case 262:
			case 389:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectBounce>();
				break;
			default:
				Debug.Log("effect Id is " + EffectID);
				ActiveSpellArray[index] = caster.AddComponent<SpellEffect>();
				break;
			}
		}
		else
		{
			switch (EffectID)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
			case 7:
			case 256:
			case 290:
			case 401:
			case 404:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectLight>();
				break;
			case 17:
			case 261:
			case 384:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectLeap>();
				break;
			case 18:
			case 263:
			case 385:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectSlowFall>();
				break;
			case 19:
			case 21:
			case 276:
			case 292:
			case 386:
			case 388:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectLevitate>();
				break;
			case 20:
			case 274:
			case 387:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectWaterWalk>();
				break;
			case 34:
			case 35:
			case 37:
			case 257:
			case 273:
			case 298:
			case 399:
			case 400:
			case 402:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectResistance>();
				break;
			case 50:
			case 51:
			case 52:
			case 260:
			case 269:
			case 295:
			case 390:
			case 391:
			case 392:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectStealth>();
				break;
			case 53:
			case 56:
			case 57:
			case 283:
			case 393:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectResistanceAgainstType>();
				break;
			case 54:
			case 278:
			case 394:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectFlameproof>();
				break;
			case 55:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectImmunityPoison>();
				break;
			case 176:
			case 397:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectSpeed>();
				break;
			case 184:
			case 291:
			case 398:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectTelekinesis>();
				break;
			case 187:
			case 302:
			case 395:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectFreezeTime>();
				break;
			case 190:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectRegenerationHealth>();
				break;
			case 191:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectRegenerationMana>();
				break;
			case 212:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectMazeNavigation>();
				break;
			case 213:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectHallucination>();
				break;
			case 5:
			case 270:
			case 403:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectNightVision>();
				break;
			case 116:
			case 277:
			case 491:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectPoison>();
				break;
			case 117:
			case 289:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectParalyze>();
				break;
			case 115:
			case 293:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectAlly>();
				break;
			case 99:
			case 296:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectConfusion>();
				break;
			case 113:
			case 266:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectFear>();
				break;
			case 448:
			case 449:
			case 450:
			case 451:
			case 452:
			case 453:
			case 454:
			case 455:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectAccuracy>();
				break;
			case 456:
			case 457:
			case 458:
			case 459:
			case 460:
			case 461:
			case 462:
			case 463:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectDamage>();
				break;
			case 464:
			case 465:
			case 466:
			case 467:
			case 468:
			case 469:
			case 470:
			case 471:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectProtection>();
				break;
			case 472:
			case 473:
			case 474:
			case 475:
			case 476:
			case 477:
			case 478:
			case 479:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectToughness>();
				break;
			case 183:
			case 300:
			case 396:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectRoamingSight>();
				break;
			case 49:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 262:
			case 389:
				ActiveSpellArray[index] = caster.AddComponent<SpellEffectCurse>();
				break;
			default:
				Debug.Log("effect Id is " + EffectID);
				ActiveSpellArray[index] = caster.AddComponent<SpellEffect>();
				break;
			}
		}
		ActiveSpellArray[index].EffectID = EffectID;
		return ActiveSpellArray[index];
	}

	private int CheckActiveSpellEffect(GameObject caster)
	{
		if (UWCharacter.Instance != null)
		{
			for (int i = 0; i < 3; i++)
			{
				if (UWCharacter.Instance.ActiveSpell[i] == null)
				{
					return i;
				}
			}
			return -1;
		}
		return -1;
	}

	private int CheckPassiveSpellEffectPC(GameObject caster)
	{
		if (UWCharacter.Instance != null)
		{
			for (int i = 0; i < 10; i++)
			{
				if (UWCharacter.Instance.PassiveSpell[i] == null)
				{
					return i;
				}
			}
			return -1;
		}
		return -1;
	}

	private int CheckPassiveSpellEffectNPC(GameObject caster)
	{
		if (caster == null)
		{
			return -1;
		}
		NPC component = caster.GetComponent<NPC>();
		if (component != null)
		{
			for (int i = 0; i < 3; i++)
			{
				if (component.NPCStatusEffects[i] == null)
				{
					return i;
				}
			}
			return -1;
		}
		return -1;
	}

	private bool CastProjectile(GameObject caster, SpellProp spellprop)
	{
		UWCharacter component = caster.GetComponent<UWCharacter>();
		if (component != null)
		{
			Ray ray = getRay(caster, spellprop.CastRaySource);
			RaycastHit hitInfo = default(RaycastHit);
			float num = 0.5f;
			if (!Physics.Raycast(ray, out hitInfo, num))
			{
				ReadiedSpell = "";
				if (spellprop.noOfCasts >= 1 && ObjectInteraction.PlaySoundEffects)
				{
					if (caster == UWCharacter.Instance.gameObject)
					{
						UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[36];
						UWCharacter.Instance.aud.Play();
					}
					else if (!spellprop.silent)
					{
						caster.GetComponent<AudioSource>().clip = MusicController.instance.SoundEffects[36];
						caster.GetComponent<AudioSource>().Play();
					}
				}
				for (int i = 0; i < spellprop.noOfCasts; i++)
				{
					GameObject gameObject = CreateMagicProjectile(ray.GetPoint(num / 2f), caster, spellprop);
					gameObject.transform.rotation = Quaternion.LookRotation(ray.direction.normalized);
					LaunchMagicProjectile(gameObject, spellprop.spread);
				}
				UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
				return true;
			}
			return false;
		}
		for (int j = 0; j < spellprop.noOfCasts; j++)
		{
			GameObject projectile = CreateMagicProjectile(caster.GetComponent<ObjectInteraction>().GetImpactPoint(), caster.GetComponent<ObjectInteraction>().GetImpactGameObject(), spellprop);
			LaunchMagicProjectile(projectile, spellprop.spread);
		}
		return true;
	}

	private bool CastProjectile(GameObject caster, GameObject target, SpellProp spellprop)
	{
		GameObject gameObject = CreateMagicProjectile(caster.transform.position, caster, spellprop);
		Vector3 forward;
		if (spellprop.spread == 0f)
		{
			forward = target.transform.position - caster.transform.position;
			forward.Normalize();
		}
		else
		{
			float num = UnityEngine.Random.Range(0f, spellprop.spread);
			float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
			forward = new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f), 10f);
			forward = gameObject.transform.TransformDirection(forward.normalized);
		}
		gameObject.transform.rotation = Quaternion.LookRotation(forward);
		LaunchMagicProjectile(gameObject, spellprop.spread);
		return true;
	}

	private bool CastProjectile(GameObject caster, Vector3 targetV, SpellProp spellprop)
	{
		GameObject gameObject = CreateMagicProjectile(caster.transform.position, caster, spellprop);
		if (ObjectInteraction.PlaySoundEffects)
		{
			if (caster == UWCharacter.Instance.gameObject)
			{
				if (!spellprop.silent)
				{
					UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[36];
					UWCharacter.Instance.aud.Play();
				}
			}
			else if (caster.name.Contains("_NPC_Launcher"))
			{
				if (!spellprop.silent && caster.GetComponent<AudioSource>() != null)
				{
					caster.transform.parent.GetComponent<AudioSource>().clip = MusicController.instance.SoundEffects[36];
					caster.transform.parent.GetComponent<AudioSource>().Play();
				}
			}
			else if (!spellprop.silent && caster.GetComponent<AudioSource>() != null)
			{
				caster.GetComponent<AudioSource>().clip = MusicController.instance.SoundEffects[36];
				caster.GetComponent<AudioSource>().Play();
			}
		}
		if (gameObject != null)
		{
			gameObject.transform.rotation = Quaternion.LookRotation(targetV);
			LaunchMagicProjectile(gameObject, spellprop.spread);
		}
		return true;
	}

	private GameObject CreateMagicProjectile(Vector3 Location, GameObject Caster, SpellProp spellprop)
	{
		int index;
		UWEBase.CurrentObjectList().getFreeSlot(100, out index);
		if (index != -1)
		{
			ObjectLoaderInfo objectLoaderInfo = UWEBase.CurrentObjectList().objInfo[index];
			objectLoaderInfo.guid = Guid.NewGuid();
			objectLoaderInfo.item_id = spellprop.ProjectileItemId;
			objectLoaderInfo.invis = 0;
			objectLoaderInfo.enchantment = 0;
			objectLoaderInfo.doordir = 0;
			objectLoaderInfo.is_quant = 1;
			objectLoaderInfo.flags = 3;
			objectLoaderInfo.quality = 60;
			objectLoaderInfo.InUseFlag = 1;
			GameObject gameObject = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, Location).gameObject;
			gameObject.layer = LayerMask.NameToLayer("MagicProjectile");
			gameObject.transform.parent = GameWorldController.instance.DynamicObjectMarker();
			MagicProjectile magicProjectile = gameObject.GetComponent<MagicProjectile>();
			if (magicProjectile == null)
			{
				magicProjectile = gameObject.AddComponent<MagicProjectile>();
			}
			magicProjectile.spellprop = spellprop;
			if (Caster.name.Contains("NPC_Launcher"))
			{
				magicProjectile.caster = Caster.transform.parent.gameObject;
			}
			else
			{
				magicProjectile.caster = Caster;
			}
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			component.size = new Vector3(0.1f, 0.1f, 0.1f);
			component.center = new Vector3(0f, 0.1f, 0f);
			Rigidbody component2 = gameObject.GetComponent<Rigidbody>();
			component2.freezeRotation = true;
			magicProjectile.rgd = component2;
			UWEBase.UnFreezeMovement(gameObject);
			component2.useGravity = false;
			component2.collisionDetectionMode = CollisionDetectionMode.Continuous;
			if (Caster.name != UWCharacter.Instance.name)
			{
				gameObject.transform.position = Caster.transform.position;
			}
			else
			{
				gameObject.transform.position = Location;
			}
			return gameObject;
		}
		return null;
	}

	private void LaunchMagicProjectile(GameObject projectile, float spread)
	{
		float num = UnityEngine.Random.Range(0f, spread);
		float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
		Vector3 vector = new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f), 10f);
		vector = projectile.transform.TransformDirection(vector.normalized);
		projectile.transform.rotation = Quaternion.identity;
		float num2 = Vector3.SignedAngle(Vector3.forward, new Vector3(vector.x, 0f, vector.z), Vector3.up);
		MagicProjectile component = projectile.GetComponent<MagicProjectile>();
		if (component != null)
		{
			component.Projectile_Pitch = 0;
			if (vector.y < 0f)
			{
				component.Projectile_Sign = 0;
			}
			else
			{
				component.Projectile_Sign = 1;
			}
			component.Projectile_Pitch = (short)Mathf.Abs(vector.y * 7f);
			if (num2 >= 0f && num2 < 45f)
			{
				component.ProjectileHeadingMajor = 0;
				component.ProjectileHeadingMinor = (short)(num2 / 45f * 32f);
			}
			else if (num2 >= 45f && num2 < 90f)
			{
				component.ProjectileHeadingMajor = 1;
				component.ProjectileHeadingMinor = (short)((num2 - 45f) / 45f * 32f);
			}
			else if (num2 >= 90f && num2 < 135f)
			{
				component.ProjectileHeadingMajor = 2;
				component.ProjectileHeadingMinor = (short)((num2 - 90f) / 45f * 32f);
			}
			else if (num2 >= 135f && num2 <= 180f)
			{
				component.ProjectileHeadingMajor = 3;
				component.ProjectileHeadingMinor = (short)((num2 - 135f) / 45f * 32f);
			}
			else if (num2 < 0f && num2 >= -45f)
			{
				component.ProjectileHeadingMajor = 7;
				component.ProjectileHeadingMinor = (short)(32f - Mathf.Abs((0f - num2) / 45f * 32f));
			}
			else if (num2 < -45f && num2 >= -90f)
			{
				component.ProjectileHeadingMajor = 6;
				component.ProjectileHeadingMinor = (short)(32f - Mathf.Abs((0f - num2 - 45f) / 45f * 32f));
			}
			else if (num2 < -90f && num2 >= -135f)
			{
				component.ProjectileHeadingMajor = 5;
				component.ProjectileHeadingMinor = (short)(32f - Mathf.Abs((0f - num2 - 90f) / 45f * 32f));
			}
			else if (num2 < -135f && num2 >= -180f)
			{
				component.ProjectileHeadingMajor = 4;
				component.ProjectileHeadingMinor = (short)(32f - Mathf.Abs((0f - num2 - 135f) / 45f * 32f));
			}
		}
		if (component.spellprop.homing)
		{
			component.BeginHoming();
		}
		if (component.spellprop.hasTrail)
		{
			component.BeginVapourTrail();
		}
	}

	private void OnGUI()
	{
		if (!WindowDetect.InMap && !WindowDetect.WaitingForInput && !ConversationVM.InConversation && Event.current.keyCode == KeyBindings.instance.CastSpell && Event.current.type == EventType.KeyDown && !UWHUD.instance.window.JustClicked && !UWCharacter.Instance.PlayerCombat.AttackCharging && !UWCharacter.Instance.PlayerCombat.AttackExecuting && ReadiedSpell == "" && TestSpellCast(base.gameObject.GetComponent<UWCharacter>(), ActiveRunes[0], ActiveRunes[1], ActiveRunes[2]))
		{
			castSpell(base.gameObject, ActiveRunes[0], ActiveRunes[1], ActiveRunes[2], true);
			ApplySpellCost();
		}
	}

	private Ray getRay(GameObject caster)
	{
		if (caster.GetComponent<UWCharacter>().MouseLookEnabled)
		{
			return Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		}
		return Camera.main.ScreenPointToRay(Input.mousePosition);
	}

	private Ray getRay(GameObject caster, bool castFromWindow)
	{
		if (!castFromWindow)
		{
			return getRay(caster);
		}
		return Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
	}

	public SpellEffect CastEnchantmentImmediate(GameObject caster, GameObject target, int EffectID, int SpellRule, int CastType)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			return CastEnchantmentUW2(caster, target, EffectID, false, SpellRule, CastType);
		}
		return CastEnchantmentUW1(caster, target, EffectID, false, SpellRule, CastType);
	}

	public SpellEffect CastEnchantment(GameObject caster, GameObject target, int EffectID, int SpellRule, int CastType)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			return CastEnchantmentUW2(caster, target, EffectID, true, SpellRule, CastType);
		}
		return CastEnchantmentUW1(caster, target, EffectID, true, SpellRule, CastType);
	}

	public SpellEffect CastEnchantmentUW1(GameObject caster, GameObject target, int EffectID, bool ready, int SpellRule, int CastType)
	{
		int num = -1;
		int num2 = -1;
		int num3 = 0;
		SpellEffect[] activeSpellArray = null;
		if (SpellRule != 2)
		{
			num = UWCharacter.Instance.PlayerMagic.CheckActiveSpellEffect(caster);
			num2 = UWCharacter.Instance.PlayerMagic.CheckPassiveSpellEffectPC(caster);
			if (target != null)
			{
				num2 = CheckPassiveSpellEffectNPC(target);
				if (target.GetComponent<NPC>() != null)
				{
					activeSpellArray = target.GetComponent<NPC>().NPCStatusEffects;
				}
			}
		}
		switch (EffectID)
		{
		case 64:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
		case 78:
		case 79:
		case 264:
		case 275:
		case 286:
			Cast_Heal(caster, EffectID);
			num3 = 0;
			break;
		case 160:
		case 161:
		case 162:
		case 163:
		case 164:
		case 165:
		case 166:
		case 167:
		case 168:
		case 169:
		case 170:
		case 171:
		case 172:
		case 173:
		case 174:
		case 175:
		case 307:
		case 308:
			Cast_Mana(caster, EffectID);
			num3 = 0;
			break;
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 6:
		case 7:
		case 256:
		case 290:
		case 401:
		case 404:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Light(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Light(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 17:
		case 261:
		case 384:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Leap(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				}
				num3 = 1;
				break;
			default:
				if (num != -1)
				{
					Cast_Leap(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 18:
		case 263:
		case 385:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_SlowFall(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_SlowFall(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 19:
		case 21:
		case 276:
		case 292:
		case 386:
		case 388:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Levitate(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Levitate(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 20:
		case 274:
		case 387:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_WaterWalk(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_WaterWalk(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 34:
		case 35:
		case 37:
		case 257:
		case 273:
		case 298:
		case 399:
		case 400:
		case 402:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Resistance(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Resistance(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 50:
		case 51:
		case 52:
		case 260:
		case 269:
		case 295:
		case 390:
		case 391:
		case 392:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Stealth(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Stealth(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 53:
		case 283:
		case 393:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 54:
		case 278:
		case 394:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Flameproof(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Flameproof(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 56:
		case 57:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 55:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ResistPoison(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ResistPoison(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 176:
		case 397:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Speed(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Speed(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 184:
		case 291:
		case 398:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Telekinesis(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Telekinesis(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 187:
		case 302:
		case 395:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_FreezeTime(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_FreezeTime(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 190:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_HealthRegen(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_HealthRegen(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 191:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ManaRegen(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ManaRegen(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 212:
			if (num2 != -1)
			{
				Cast_MazeNavigation(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				num3 = 1;
			}
			break;
		case 213:
			Cast_Hallucination(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
			num3 = 1;
			break;
		case 5:
		case 270:
		case 403:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_NightVision(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_NightVision(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 116:
		case 277:
		case 491:
			switch (SpellRule)
			{
			case 0:
				if (target != null && num2 != -1)
				{
					Cast_Poison(target, activeSpellArray, EffectID, num2);
					num3 = 0;
				}
				break;
			case 1:
				if (num2 != -1)
				{
					Cast_Poison(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			}
			break;
		case 117:
		case 289:
			switch (SpellRule)
			{
			case 0:
				if (target != null && num2 != -1)
				{
					Cast_Paralyze(target, activeSpellArray, EffectID, num2);
					num3 = 0;
				}
				break;
			case 1:
				if (num2 != -1)
				{
					Cast_Paralyze(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			}
			break;
		case 115:
		case 293:
			Cast_InManiRel(caster, EffectID);
			num3 = 0;
			break;
		case 99:
		case 296:
			Cast_VasAnWis(caster, EffectID);
			num3 = 0;
			break;
		case 448:
		case 449:
		case 450:
		case 451:
		case 452:
		case 453:
		case 454:
		case 455:
			Debug.Log("accuracy enchantment");
			num3 = 0;
			break;
		case 456:
		case 457:
		case 458:
		case 459:
		case 460:
		case 461:
		case 462:
		case 463:
			num3 = 0;
			Debug.Log("damage enchantment");
			break;
		case 464:
		case 465:
		case 466:
		case 467:
		case 468:
		case 469:
		case 470:
		case 471:
			num3 = 0;
			break;
		case 472:
		case 473:
		case 474:
		case 475:
		case 476:
		case 477:
		case 478:
		case 479:
			num3 = 0;
			break;
		case 181:
		case 284:
			Cast_ExYlem(caster, ready, EffectID);
			num3 = 0;
			break;
		case 129:
		case 259:
			Cast_InManiYlem(caster, EffectID);
			num3 = 0;
			break;
		case 182:
		case 285:
			Cast_AnNox(caster, EffectID);
			num3 = 0;
			break;
		case 98:
		case 287:
			if (SpellRule != 2)
			{
				Cast_VasOrtGrav(caster, EffectID, true);
			}
			else
			{
				SpellProp_SheetLightning spellProp_SheetLightning = new SpellProp_SheetLightning();
				spellProp_SheetLightning.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_SheetLightning);
			}
			num3 = 0;
			break;
		case 188:
		case 303:
			Cast_VasKalCorp(caster, EffectID);
			num3 = 0;
			break;
		case 186:
		case 288:
			Cast_VasRelPor(caster, EffectID);
			num3 = 0;
			break;
		case 81:
		case 258:
			if (SpellRule != 2)
			{
				Cast_OrtJux(caster, ready, EffectID);
			}
			else
			{
				SpellProp_MagicArrow spellProp_MagicArrow = new SpellProp_MagicArrow();
				spellProp_MagicArrow.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_MagicArrow);
			}
			num3 = 0;
			break;
		case 305:
			if (SpellRule != 2)
			{
				Cast_Acid(caster, ready, EffectID);
			}
			else
			{
				SpellProp_Acid spellProp_Acid = new SpellProp_Acid();
				spellProp_Acid.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_Acid);
			}
			num3 = 0;
			break;
		case 82:
		case 271:
			if (SpellRule != 2)
			{
				Cast_OrtGrav(caster, ready, EffectID);
			}
			else
			{
				SpellProp_ElectricBolt spellProp_ElectricBolt = new SpellProp_ElectricBolt();
				spellProp_ElectricBolt.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_ElectricBolt);
			}
			num3 = 0;
			break;
		case 83:
		case 280:
			if (SpellRule != 2)
			{
				Cast_PorFlam(caster, ready, EffectID);
			}
			else
			{
				SpellProp_Fireball spellProp_Fireball = new SpellProp_Fireball();
				spellProp_Fireball.init(EffectID, caster);
				spellProp_Fireball.caster = caster;
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_Fireball);
			}
			num3 = 0;
			break;
		case 100:
		case 301:
			if (SpellRule != 2)
			{
				Cast_FlamHur(caster, EffectID);
			}
			else
			{
				SpellProp_FlameWind spellProp_FlameWind = new SpellProp_FlameWind();
				spellProp_FlameWind.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_FlameWind);
			}
			num3 = 0;
			break;
		case 183:
		case 300:
		case 396:
			Cast_RoamingSight(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
			num3 = 0;
			break;
		case 131:
		case 267:
			Cast_RuneOfWarding(caster.transform.position + base.transform.forward * 0.3f, EffectID);
			num3 = 0;
			break;
		case 178:
		case 272:
			Cast_SanctJux(caster, ready, EffectID);
			num3 = 0;
			break;
		case 185:
		case 299:
			Cast_VasPorYlem(caster, EffectID);
			num3 = 0;
			break;
		case 132:
		case 294:
			Cast_KalMani(caster, EffectID);
			num3 = 0;
			break;
		case 113:
		case 266:
			Cast_QuasCorp(caster, EffectID);
			num3 = 0;
			break;
		case 114:
		case 281:
			Cast_AnCorpMani(caster, EffectID);
			num3 = 0;
			break;
		case 177:
		case 265:
			Cast_DetectMonster(caster, EffectID);
			num3 = 0;
			break;
		case 180:
		case 282:
			Cast_NameEnchantment(caster, ready, EffectID);
			num3 = 0;
			break;
		case 49:
		case 262:
		case 389:
			Cast_Curse(caster, EffectID);
			num3 = 0;
			break;
		case 144:
		case 145:
		case 146:
		case 147:
		case 148:
		case 149:
		case 150:
		case 151:
		case 152:
		case 153:
		case 154:
		case 155:
		case 156:
		case 157:
		case 158:
		case 159:
			if (num2 != -1)
			{
				Cast_CursedItem(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				num3 = 1;
			}
			break;
		case 97:
		case 130:
		case 179:
		case 279:
		case 297:
		case 304:
		case 306:
			num3 = 0;
			break;
		case 211:
			CastTheFrog(caster, EffectID);
			num3 = 0;
			break;
		case 224:
		{
			Cutscene_Intro cs6 = UWHUD.instance.gameObject.AddComponent<Cutscene_Intro>();
			UWHUD.instance.CutScenesFull.cs = cs6;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 225:
		{
			Cutscene_EndGame cs5 = UWHUD.instance.gameObject.AddComponent<Cutscene_EndGame>();
			UWHUD.instance.CutScenesFull.cs = cs5;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 226:
		{
			Cutscene_Tybal cs4 = UWHUD.instance.gameObject.AddComponent<Cutscene_Tybal>();
			UWHUD.instance.CutScenesFull.cs = cs4;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 227:
		{
			Cutscene_Arial cs3 = UWHUD.instance.gameObject.AddComponent<Cutscene_Arial>();
			UWHUD.instance.CutScenesFull.cs = cs3;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 233:
		{
			Cutscene_Splash cs2 = UWHUD.instance.gameObject.AddComponent<Cutscene_Splash>();
			UWHUD.instance.CutScenesFull.cs = cs2;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 234:
		{
			Cutscene_Credits cs = UWHUD.instance.gameObject.AddComponent<Cutscene_Credits>();
			UWHUD.instance.CutScenesFull.cs = cs;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 235:
			Debug.Log("Vision - IN");
			break;
		case 236:
			Debug.Log("Vision - SA");
			break;
		case 237:
			Debug.Log("Vision - HN");
			break;
		default:
			if (target != null)
			{
				Debug.Log("Unimplemented effect Id is " + EffectID + " target is " + target.name);
			}
			else
			{
				Debug.Log("Unimplemented effect Id is " + EffectID);
			}
			num3 = 0;
			break;
		}
		switch (num3)
		{
		case 0:
			return null;
		case 1:
			return UWCharacter.Instance.PassiveSpell[num2];
		case 2:
			return UWCharacter.Instance.ActiveSpell[num];
		default:
			return null;
		}
	}

	public SpellEffect CastEnchantmentUW2(GameObject caster, GameObject target, int EffectID, bool ready, int SpellRule, int CastType)
	{
		int num = -1;
		int num2 = -1;
		int num3 = 0;
		SpellEffect[] activeSpellArray = null;
		if (SpellRule != 2)
		{
			num = UWCharacter.Instance.PlayerMagic.CheckActiveSpellEffect(caster);
			num2 = UWCharacter.Instance.PlayerMagic.CheckPassiveSpellEffectPC(caster);
			if (target != null)
			{
				num2 = CheckPassiveSpellEffectNPC(target);
				if (target.GetComponent<NPC>() != null)
				{
					activeSpellArray = target.GetComponent<NPC>().NPCStatusEffects;
				}
			}
		}
		switch (EffectID)
		{
		case 208:
			Cast_Altara(caster, EffectID);
			num3 = 0;
			break;
		case 188:
		case 319:
			Cast_VasKalCorp(caster, EffectID);
			num3 = 0;
			break;
		case 22:
		case 262:
		case 389:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Bounce(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				}
				num3 = 1;
				break;
			default:
				if (num != -1)
				{
					Cast_Bounce(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 182:
		case 273:
			Cast_AnNox(caster, EffectID);
			num3 = 0;
			break;
		case 49:
			Cast_Curse(caster, EffectID);
			num3 = 0;
			break;
		case 144:
		case 145:
		case 146:
		case 147:
		case 148:
		case 149:
		case 150:
		case 151:
		case 152:
		case 153:
		case 154:
		case 155:
		case 156:
		case 157:
		case 158:
		case 159:
			if (num2 != -1)
			{
				Cast_CursedItem(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				num3 = 1;
			}
			break;
		case 85:
		case 266:
			if (SpellRule != 2)
			{
				Cast_DeadlySeeker(caster, ready, EffectID);
			}
			else
			{
				SpellProp_Homing spellProp_Homing = new SpellProp_Homing();
				spellProp_Homing.init(EffectID, caster);
				spellProp_Homing.caster = caster;
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_Homing);
			}
			num3 = 0;
			break;
		case 115:
		case 296:
			Debug.Log("charm");
			num3 = 0;
			break;
		case 99:
		case 304:
			Cast_VasAnWis(caster, EffectID);
			num3 = 0;
			break;
		case 113:
		case 264:
			Cast_QuasCorp(caster, EffectID);
			num3 = 0;
			break;
		case 458:
			Debug.Log("Firedoom");
			num3 = 0;
			break;
		case 54:
		case 284:
		case 395:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Flameproof(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Flameproof(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 19:
		case 21:
		case 288:
		case 313:
		case 386:
		case 388:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Levitate(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Levitate(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 18:
		case 269:
		case 385:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_SlowFall(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_SlowFall(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 129:
		case 256:
			Cast_InManiYlem(caster, EffectID);
			num3 = 0;
			break;
		case 189:
		case 271:
			Cast_DispelHunger(caster, EffectID);
			num3 = 0;
			break;
		case 187:
		case 314:
		case 401:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_FreezeTime(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_FreezeTime(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 103:
		case 280:
			Debug.Log("stay frosty");
			num3 = 0;
			break;
		case 212:
		case 213:
			Cast_Hallucination(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
			num3 = 1;
			break;
		case 64:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
		case 78:
		case 79:
		case 178:
		case 267:
		case 281:
		case 300:
		case 316:
			Cast_Heal(caster, EffectID);
			num3 = 0;
			break;
		case 190:
		case 214:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_HealthRegen(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_HealthRegen(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 50:
		case 51:
		case 52:
		case 301:
		case 391:
		case 392:
		case 393:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Stealth(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Stealth(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 118:
		case 318:
			Debug.Log("smite foe");
			num3 = 0;
			break;
		case 17:
		case 270:
		case 384:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Leap(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				}
				num3 = 1;
				break;
			default:
				if (num != -1)
				{
					Cast_Leap(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 456:
			Debug.Log("life stealer");
			num3 = 0;
			break;
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 261:
		case 276:
		case 298:
		case 407:
		case 409:
		case 410:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Light(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Light(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 179:
		case 263:
			Debug.Log("locate");
			num3 = 0;
			break;
		case 123:
		case 290:
		case 305:
			Cast_NameEnchantment(caster, ready, EffectID);
			num3 = 0;
			break;
		case 119:
		case 286:
			Debug.Log("study monster");
			num3 = 0;
			break;
		case 257:
		case 390:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Luck(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				}
				num3 = 1;
				break;
			default:
				if (num != -1)
				{
					Cast_Luck(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 56:
		case 57:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 311:
			Debug.Log("magic satellite");
			num3 = 0;
			break;
		case 160:
		case 161:
		case 162:
		case 163:
		case 164:
		case 165:
		case 166:
		case 167:
		case 172:
		case 173:
		case 174:
		case 175:
		case 220:
		case 221:
		case 222:
		case 223:
		case 323:
		case 324:
			Cast_Mana(caster, EffectID);
			num3 = 0;
			break;
		case 168:
		case 169:
		case 170:
		case 171:
		case 191:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ManaRegen(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ManaRegen(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 215:
		case 302:
			Cast_MapArea(caster, EffectID);
			num3 = 0;
			break;
		case 210:
			Debug.Log("mind blast");
			num3 = 0;
			break;
		case 53:
		case 287:
		case 394:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ResistanceAgainstType(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 211:
			Debug.Log("basilisk oil");
			num3 = 0;
			break;
		case 124:
		case 291:
			Cast_ExYlem(caster, ready, EffectID);
			num3 = 0;
			break;
		case 126:
			Debug.Log("enchantment");
			num3 = 0;
			break;
		case 101:
		case 117:
		case 303:
		case 320:
			switch (SpellRule)
			{
			case 0:
				if (target != null && num2 != -1)
				{
					Cast_Paralyze(target, activeSpellArray, EffectID, num2);
					num3 = 0;
				}
				break;
			case 1:
				if (num2 != -1)
				{
					Cast_Paralyze(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			}
			break;
		case 116:
			switch (SpellRule)
			{
			case 0:
				if (target != null && num2 != -1)
				{
					Cast_Poison(target, activeSpellArray, EffectID, num2);
					num3 = 0;
				}
				break;
			case 1:
				if (num2 != -1)
				{
					Cast_Poison(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			}
			break;
		case 55:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_ResistPoison(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_ResistPoison(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 59:
		case 282:
		case 400:
			Debug.Log("Poison Weapon");
			num3 = 0;
			break;
		case 127:
		case 186:
		case 299:
			Cast_VasRelPor(caster, EffectID);
			num3 = 0;
			break;
		case 177:
		case 310:
			Debug.Log("Portal");
			num3 = 0;
			break;
		case 81:
		case 258:
			if (SpellRule != 2)
			{
				Cast_OrtJux(caster, ready, EffectID);
			}
			else
			{
				SpellProp_MagicArrow spellProp_MagicArrow = new SpellProp_MagicArrow();
				spellProp_MagicArrow.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_MagicArrow);
			}
			num3 = 0;
			break;
		case 83:
		case 289:
			if (SpellRule != 2)
			{
				Cast_PorFlam(caster, ready, EffectID);
			}
			else
			{
				SpellProp_Fireball spellProp_Fireball = new SpellProp_Fireball();
				spellProp_Fireball.init(EffectID, caster);
				spellProp_Fireball.caster = caster;
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_Fireball);
			}
			num3 = 0;
			break;
		case 84:
		case 216:
		case 321:
			Debug.Log("acid");
			num3 = 0;
			break;
		case 86:
		case 328:
			Debug.Log("snowballs");
			num3 = 0;
			break;
		case 82:
		case 275:
			if (SpellRule != 2)
			{
				Cast_OrtGrav(caster, ready, EffectID);
			}
			else
			{
				SpellProp_ElectricBolt spellProp_ElectricBolt = new SpellProp_ElectricBolt();
				spellProp_ElectricBolt.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_ElectricBolt);
			}
			num3 = 0;
			break;
		case 98:
		case 297:
			if (SpellRule != 2)
			{
				Cast_VasOrtGrav(caster, EffectID, true);
			}
			else
			{
				SpellProp_SheetLightning spellProp_SheetLightning = new SpellProp_SheetLightning();
				spellProp_SheetLightning.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_SheetLightning);
			}
			num3 = 0;
			break;
		case 100:
		case 312:
			if (SpellRule != 2)
			{
				Cast_FlamHur(caster, EffectID);
			}
			else
			{
				SpellProp_FlameWind spellProp_FlameWind = new SpellProp_FlameWind();
				spellProp_FlameWind.init(EffectID, caster);
				CastProjectile(caster, GetBestSpellVector(caster), spellProp_FlameWind);
			}
			num3 = 0;
			break;
		case 102:
		case 307:
			Debug.Log("SHockwave");
			num3 = 0;
			break;
		case 121:
		case 294:
			Debug.Log("mending (check uw1 version!)");
			num3 = 0;
			break;
		case 34:
		case 35:
		case 37:
			if (num2 != -1)
			{
				Cast_Resistance(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
				if (EffectID == 37)
				{
					IronFleshXClock();
				}
				num3 = 1;
			}
			break;
		case 259:
		case 285:
		case 315:
		case 405:
		case 406:
		case 408:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Resistance(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Resistance(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 97:
		case 306:
			Debug.Log("reveal");
			num3 = 0;
			break;
		case 183:
		case 317:
		case 402:
			Cast_RoamingSight(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
			num3 = 0;
			break;
		case 120:
		case 130:
		case 131:
		case 268:
		case 274:
		case 293:
			Debug.Log("uw2 rune traps");
			num3 = 0;
			break;
		case 176:
		case 278:
		case 403:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Speed(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Speed(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 459:
			Cast_StoneStrike(caster, target, EffectID);
			num3 = 0;
			break;
		case 132:
		case 133:
		case 308:
			Cast_KalMani(caster, EffectID);
			num3 = 0;
			break;
		case 184:
		case 295:
		case 404:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_Telekinesis(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_Telekinesis(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 322:
			Debug.Log("local teleport");
			num3 = 0;
			break;
		case 122:
		case 125:
		case 260:
		case 283:
			Debug.Log("trap spells");
			num3 = 0;
			break;
		case 185:
		case 309:
			Cast_VasPorYlem(caster, EffectID);
			num3 = 0;
			break;
		case 114:
		case 292:
			Cast_AnCorpMani(caster, EffectID);
			num3 = 0;
			break;
		case 277:
			Debug.Log("repel undead");
			num3 = 0;
			break;
		case 457:
			Debug.Log("undeath bane");
			num3 = 0;
			break;
		case 58:
		case 265:
		case 399:
			Debug.Log("valor");
			num3 = 0;
			break;
		case 20:
		case 279:
		case 387:
			switch (CastType)
			{
			case 1:
				if (num2 != -1)
				{
					Cast_WaterWalk(caster, UWCharacter.Instance.PassiveSpell, EffectID, num2);
					num3 = 1;
				}
				break;
			default:
				if (num != -1)
				{
					Cast_WaterWalk(caster, UWCharacter.Instance.ActiveSpell, EffectID, num);
					num3 = 2;
				}
				break;
			}
			break;
		case 224:
			Debug.Log("cutscene" + EffectID);
			break;
		case 225:
			Debug.Log("cutscene" + EffectID);
			break;
		case 226:
			Debug.Log("cutscene" + EffectID);
			break;
		case 227:
			Debug.Log("cutscene" + EffectID);
			break;
		case 228:
		{
			Cutscene_SeeThyFuture cs4 = UWHUD.instance.gameObject.AddComponent<Cutscene_SeeThyFuture>();
			UWHUD.instance.CutScenesFull.cs = cs4;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 229:
		{
			Cutscene_PeaceSuchAsThis cs3 = UWHUD.instance.gameObject.AddComponent<Cutscene_PeaceSuchAsThis>();
			UWHUD.instance.CutScenesFull.cs = cs3;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 230:
		{
			Cutscene_AllWhoOppose cs2 = UWHUD.instance.gameObject.AddComponent<Cutscene_AllWhoOppose>();
			UWHUD.instance.CutScenesFull.cs = cs2;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 231:
		{
			Cutscene_ThisWorldIsMine cs = UWHUD.instance.gameObject.AddComponent<Cutscene_ThisWorldIsMine>();
			UWHUD.instance.CutScenesFull.cs = cs;
			UWHUD.instance.CutScenesFull.Begin();
			break;
		}
		case 232:
			Debug.Log("cutscene" + EffectID);
			break;
		case 233:
			Debug.Log("cutscene" + EffectID);
			break;
		case 234:
			Debug.Log("cutscene" + EffectID);
			break;
		default:
			Debug.Log(string.Concat("Unimplemented effect Id is ", EffectID, " caster =", caster, " target =", target));
			num3 = 0;
			break;
		}
		switch (num3)
		{
		case 0:
			return null;
		case 1:
			return UWCharacter.Instance.PassiveSpell[num2];
		case 2:
			return UWCharacter.Instance.ActiveSpell[num];
		default:
			return null;
		}
	}

	private void IronFleshXClock()
	{
		if (Quest.instance.x_clocks[3] == 4)
		{
			Quest.instance.x_clocks[3] = 5;
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 335));
		}
	}
}
