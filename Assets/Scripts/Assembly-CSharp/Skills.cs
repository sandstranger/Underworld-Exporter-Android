using UnityEngine;

public class Skills : MonoBehaviour
{
	public const int SkillAttack = 1;

	public const int SkillDefense = 2;

	public const int SkillUnarmed = 3;

	public const int SkillSword = 4;

	public const int SkillAxe = 5;

	public const int SkillMace = 6;

	public const int SkillMissile = 7;

	public const int SkillMana = 8;

	public const int SkillLore = 9;

	public const int SkillCasting = 10;

	public const int SkillTraps = 11;

	public const int SkillSearch = 12;

	public const int SkillTrack = 13;

	public const int SkillSneak = 14;

	public const int SkillRepair = 15;

	public const int SkillCharm = 16;

	public const int SkillPicklock = 17;

	public const int SkillAcrobat = 18;

	public const int SkillAppraise = 19;

	public const int SkillSwimming = 20;

	public int STR;

	public int DEX;

	public int INT;

	public int Attack;

	public int Defense;

	public int Unarmed;

	public int Sword;

	public int Axe;

	public int Mace;

	public int Missile;

	public int ManaSkill;

	public int Lore;

	public int Casting;

	public int Traps;

	public int Search;

	public int Track;

	public int Sneak;

	public int Repair;

	public int Charm;

	public int PickLock;

	public int Acrobat;

	public int Appraise;

	public int Swimming;

	private string[] Skillnames = new string[21]
	{
		"", "ATTACK", "DEFENSE", "UNARMED", "SWORD", "AXE", "MACE", "MISSILE", "MANA", "LORE",
		"CASTING", "TRAPS", "SEARCH", "TRACK", "SNEAK", "REPAIR", "CHARM", "PICKLOCK", "ACROBAT", "APPRAISE",
		"SWIMMING"
	};

	public bool TrySkill(int SkillToUse, int CheckValue)
	{
		return CheckValue < GetSkill(SkillToUse);
	}

	public int GetSkill(int SkillNo)
	{
		switch (SkillNo)
		{
		case 1:
			return Attack;
		case 2:
			return Defense;
		case 3:
			return Unarmed;
		case 4:
			return Sword;
		case 5:
			return Axe;
		case 6:
			return Mace;
		case 7:
			return Missile;
		case 8:
			return ManaSkill;
		case 9:
			return Lore;
		case 10:
			return Casting;
		case 11:
			return Traps;
		case 12:
			return Search;
		case 13:
			return Track;
		case 14:
			return Sneak;
		case 15:
			return Repair;
		case 16:
			return Charm;
		case 17:
			return PickLock;
		case 18:
			return Acrobat;
		case 19:
			return Appraise;
		case 20:
			return Swimming;
		default:
			return -1;
		}
	}

	public void InitSkills()
	{
		STR = 0;
		INT = 0;
		DEX = 0;
		Attack = 0;
		Defense = 0;
		Unarmed = 0;
		Sword = 0;
		Axe = 0;
		Mace = 0;
		Missile = 0;
		ManaSkill = 0;
		Lore = 0;
		Casting = 0;
		Traps = 0;
		Search = 0;
		Track = 0;
		Sneak = 0;
		Repair = 0;
		Charm = 0;
		PickLock = 0;
		Acrobat = 0;
		Appraise = 0;
		Swimming = 0;
	}

	public void AdvanceSkill(int SkillNo, int SkillPoints, int SkillPointCost)
	{
		if (UWCharacter.Instance.TrainingPoints >= SkillPointCost)
		{
			AdvanceSkill(SkillNo, SkillPoints);
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_are_not_ready_to_advance_));
		}
	}

	public void AdvanceSkill(int SkillNo, int SkillPoints)
	{
		switch (SkillNo)
		{
		case 1:
			Attack += SkillPoints;
			Attack = Mathf.Min(30, Attack);
			break;
		case 2:
			Defense += SkillPoints;
			Defense = Mathf.Min(30, Defense);
			break;
		case 3:
			Unarmed += SkillPoints;
			Unarmed = Mathf.Min(30, Unarmed);
			break;
		case 4:
			Sword += SkillPoints;
			Sword = Mathf.Min(30, Sword);
			break;
		case 5:
			Axe += SkillPoints;
			Axe = Mathf.Min(30, Axe);
			break;
		case 6:
			Mace += SkillPoints;
			Mace = Mathf.Min(30, Mace);
			break;
		case 7:
			Missile += SkillPoints;
			Missile = Mathf.Min(30, Missile);
			break;
		case 8:
			ManaSkill += SkillPoints;
			ManaSkill = Mathf.Min(30, ManaSkill);
			UWCharacter.Instance.PlayerMagic.MaxMana = ManaSkill * 3;
			break;
		case 9:
			Lore += SkillPoints;
			Lore = Mathf.Min(30, Lore);
			break;
		case 10:
			Casting += SkillPoints;
			Casting = Mathf.Min(30, Casting);
			break;
		case 11:
			Traps += SkillPoints;
			Traps = Mathf.Min(30, Traps);
			break;
		case 12:
			Search += SkillPoints;
			Search = Mathf.Min(30, Search);
			break;
		case 13:
			Track += SkillPoints;
			Track = Mathf.Min(30, Track);
			break;
		case 14:
			Sneak += SkillPoints;
			Sneak = Mathf.Min(30, Sneak);
			break;
		case 15:
			Repair += SkillPoints;
			Repair = Mathf.Min(30, Repair);
			break;
		case 16:
			Charm += SkillPoints;
			Charm = Mathf.Min(30, Charm);
			break;
		case 17:
			PickLock += SkillPoints;
			PickLock = Mathf.Min(30, PickLock);
			break;
		case 18:
			Acrobat += SkillPoints;
			Acrobat = Mathf.Min(30, Acrobat);
			break;
		case 19:
			Appraise += SkillPoints;
			Appraise = Mathf.Min(30, Appraise);
			break;
		case 20:
			Swimming += SkillPoints;
			Swimming = Mathf.Min(30, Swimming);
			break;
		}
		StatsDisplay.UpdateNow = true;
	}

	public void SetSkill(int SkillNo, int SkillPoints)
	{
		switch (SkillNo)
		{
		case 1:
			Attack = SkillPoints;
			break;
		case 2:
			Defense = SkillPoints;
			break;
		case 3:
			Unarmed = SkillPoints;
			break;
		case 4:
			Sword = SkillPoints;
			break;
		case 5:
			Axe = SkillPoints;
			break;
		case 6:
			Mace = SkillPoints;
			break;
		case 7:
			Missile = SkillPoints;
			break;
		case 8:
			ManaSkill = SkillPoints;
			break;
		case 9:
			Lore = SkillPoints;
			break;
		case 10:
			Casting = SkillPoints;
			break;
		case 11:
			Traps = SkillPoints;
			break;
		case 12:
			Search = SkillPoints;
			break;
		case 13:
			Track = SkillPoints;
			break;
		case 14:
			Sneak = SkillPoints;
			break;
		case 15:
			Repair = SkillPoints;
			break;
		case 16:
			Charm = SkillPoints;
			break;
		case 17:
			PickLock = SkillPoints;
			break;
		case 18:
			Acrobat = SkillPoints;
			break;
		case 19:
			Appraise = SkillPoints;
			break;
		case 20:
			Swimming = SkillPoints;
			break;
		}
		StatsDisplay.UpdateNow = true;
	}

	public string GetSkillName(int skillNo)
	{
		return Skillnames[skillNo];
	}

	public static void TrackMonsters(GameObject origin, float Range, bool SkillCheckPassed)
	{
		if (SkillCheckPassed)
		{
			int[] array = new int[8];
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				array[i] = 0;
			}
			bool flag = false;
			Collider[] array2 = Physics.OverlapSphere(origin.transform.position, Range);
			foreach (Collider collider in array2)
			{
				if (collider.gameObject.GetComponent<NPC>() != null)
				{
					array[Compass.getCompassHeadingOffset(origin, collider.gameObject)]++;
					flag = true;
				}
			}
			if (flag)
			{
				int num = 0;
				int num2 = 0;
				for (int k = 0; k <= array.GetUpperBound(0); k++)
				{
					if (array[k] > num)
					{
						num = array[k];
						num2 = k;
					}
				}
				string @string = StringController.instance.GetString(1, StringController.str_to_the_north + num2);
				switch (num)
				{
				case 0:
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_no_monster_activity_) + @string);
					break;
				case 1:
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_a_creature_) + @string);
					break;
				case 2:
				case 3:
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_a_few_creatures_) + @string);
					break;
				default:
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_the_activity_of_many_creatures_) + @string);
					break;
				}
			}
			else
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_no_monster_activity_));
			}
		}
		else
		{
			string string2 = StringController.instance.GetString(1, Random.Range(StringController.str_to_the_north, StringController.str_to_the_northwest + 1));
			switch (Random.Range(0, 5))
			{
			case 0:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_no_monster_activity_) + string2);
				break;
			case 1:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_a_creature_) + string2);
				break;
			case 2:
			case 3:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_a_few_creatures_) + string2);
				break;
			default:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_detect_the_activity_of_many_creatures_) + string2);
				break;
			}
		}
	}

	public static int getSkillAttributeBonus(int skillNo)
	{
		int num = 0;
		int skill = UWCharacter.Instance.PlayerSkills.GetSkill(skillNo);
		switch (skillNo)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 20:
			num = UWCharacter.Instance.PlayerSkills.STR;
			break;
		case 7:
		case 8:
		case 9:
		case 10:
		case 13:
		case 16:
			num = UWCharacter.Instance.PlayerSkills.INT;
			break;
		case 11:
		case 12:
		case 14:
		case 15:
		case 17:
		case 18:
		case 19:
			num = UWCharacter.Instance.PlayerSkills.DEX;
			break;
		}
		int num2 = Mathf.Max(0, num - skill);
		return num2 / 5;
	}

	public static int DiceRoll(int minRoll, int MaxRoll)
	{
		if (UWCharacter.Instance.isLucky)
		{
			int num = Random.Range(minRoll, MaxRoll);
			return Mathf.Min(num + 5, MaxRoll);
		}
		return Random.Range(minRoll, MaxRoll);
	}
}
