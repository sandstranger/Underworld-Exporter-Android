using UnityEngine;

public class WeaponMelee : Weapon
{
	public virtual void onHit(GameObject target)
	{
		if (!(target == null) && objInt().isEnchanted)
		{
			int actualSpellIndex = GetActualSpellIndex();
			UWCharacter.Instance.PlayerMagic.CastEnchantmentImmediate(UWCharacter.Instance.gameObject, target, actualSpellIndex, 0, 0);
		}
	}

	public short GetSlash()
	{
		return (short)(GameWorldController.instance.objDat.weaponStats[base.item_id].Slash + DamageBonus());
	}

	public short GetBash()
	{
		return (short)(GameWorldController.instance.objDat.weaponStats[base.item_id].Bash + DamageBonus());
	}

	public short GetStab()
	{
		return (short)(GameWorldController.instance.objDat.weaponStats[base.item_id].Stab + DamageBonus());
	}

	public override short getDurability()
	{
		return (short)(GameWorldController.instance.objDat.weaponStats[base.item_id].Durability + DurabilityBonus());
	}

	public static short getMeleeSlash()
	{
		return GameWorldController.instance.objDat.weaponStats[15].Slash;
	}

	public static short getMeleeBash()
	{
		return GameWorldController.instance.objDat.weaponStats[15].Bash;
	}

	public static short getMeleeStab()
	{
		return GameWorldController.instance.objDat.weaponStats[15].Stab;
	}

	public int GetSkill()
	{
		return GameWorldController.instance.objDat.weaponStats[base.item_id].Skill;
	}

	public override void UpdateQuality()
	{
		if (base.quality > 0 && base.quality <= 15)
		{
			EquipIconIndex = 4;
		}
		else if (base.quality > 15 && base.quality <= 30)
		{
			EquipIconIndex = 3;
		}
		else if (base.quality > 30 && base.quality <= 45)
		{
			EquipIconIndex = 2;
		}
		else
		{
			EquipIconIndex = 1;
		}
	}

	public short DamageBonus()
	{
		switch (base.link)
		{
		case 456:
		case 457:
		case 458:
		case 459:
		case 460:
		case 461:
		case 462:
		case 463:
			return (short)(base.link - 453);
		default:
			return 0;
		}
	}

	public short AccuracyBonus()
	{
		switch (base.link)
		{
		case 448:
		case 449:
		case 450:
		case 451:
		case 452:
		case 453:
		case 454:
		case 455:
			return (short)(base.link - 445);
		default:
			return 0;
		}
	}

	public override bool LookAt()
	{
		if (UWEBase._RES == "UW1" && base.item_id == 10)
		{
			base.heading = 7;
			ObjectInteraction.IdentificationFlags identificationFlags = objInt().identity();
			if (identificationFlags == ObjectInteraction.IdentificationFlags.Identified)
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_see_) + StringController.instance.GetString(1, 268));
			}
			else
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), GetEquipmentConditionString()) + OwnershipString());
			}
			return true;
		}
		return base.LookAt();
	}
}
