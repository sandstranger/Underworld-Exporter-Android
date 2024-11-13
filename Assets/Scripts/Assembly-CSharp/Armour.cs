using UnityEngine;

public class Armour : Equipment
{
	public SpellEffect SpellEffectApplied;

	public override int GetActualSpellIndex()
	{
		int num = base.link - 256 + 16;
		if (UWEBase._RES == "UW2" && num >= 325 && num <= 326)
		{
			return num + 69;
		}
		return num;
	}

	public override void UpdateQuality()
	{
		int num = base.item_id - 32;
		if (num < 15)
		{
			if (base.quality > 0 && base.quality <= 15)
			{
				EquipIconIndex = num;
			}
			else if (base.quality > 15 && base.quality <= 30)
			{
				EquipIconIndex = num + 15;
			}
			else if (base.quality > 30 && base.quality <= 45)
			{
				EquipIconIndex = num + 30;
			}
			else
			{
				EquipIconIndex = num + 45;
			}
		}
		else
		{
			EquipIconIndex = 60 + (base.item_id - 47);
		}
	}

	public override Sprite GetEquipDisplay()
	{
		if (UWCharacter.Instance.isFemale)
		{
			return GameWorldController.instance.armor_f.RequestSprite(EquipIconIndex);
		}
		return GameWorldController.instance.armor_m.RequestSprite(EquipIconIndex);
	}

	public override bool EquipEvent(short slotNo)
	{
		if (slotNo >= 0 && slotNo <= 4)
		{
			UpdateQuality();
			if (objInt().isEnchanted || (UWEBase._RES == "UW1" && base.item_id == 47))
			{
				switch (GetActualSpellIndex())
				{
				default:
					SpellEffectApplied = UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, GetActualSpellIndex(), 1, 1);
					if (SpellEffectApplied != null)
					{
						SpellEffectApplied.SetPermanent(true);
					}
					else
					{
						Debug.Log(base.name + " was unable to apply effect. " + GetActualSpellIndex());
					}
					break;
				case 464:
				case 465:
				case 466:
				case 467:
				case 468:
				case 469:
				case 470:
				case 471:
				case 472:
				case 473:
				case 474:
				case 475:
				case 476:
				case 477:
				case 478:
				case 479:
					break;
				}
			}
		}
		return true;
	}

	public override bool UnEquipEvent(short slotNo)
	{
		if (slotNo >= 0 && slotNo <= 4 && SpellEffectApplied != null)
		{
			SpellEffectApplied.CancelEffect();
			return true;
		}
		return false;
	}

	public override short getDurability()
	{
		return (short)(GameWorldController.instance.objDat.armourStats[base.item_id - 32].durability + DurabilityBonus());
	}

	public override short getDefence()
	{
		return (short)(GameWorldController.instance.objDat.armourStats[base.item_id - 32].protection + ProtectionBonus());
	}
}
