public class Ring : Equipment
{
	public SpellEffect SpellEffectApplied;

	public override int GetActualSpellIndex()
	{
		return base.link - 512;
	}

	public override bool EquipEvent(short slotNo)
	{
		if ((slotNo == 9 || slotNo == 10) && objInt().isEnchanted)
		{
			switch (GetActualSpellIndex())
			{
			default:
				SpellEffectApplied = UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, GetActualSpellIndex(), 1, 1);
				if (SpellEffectApplied != null)
				{
					SpellEffectApplied.SetPermanent(true);
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
		return true;
	}

	public override bool UnEquipEvent(short slotNo)
	{
		if ((slotNo == 9 || slotNo == 10) && base.item_id != 54 && SpellEffectApplied != null)
		{
			SpellEffectApplied.CancelEffect();
			return true;
		}
		return false;
	}

	public override bool use()
	{
		return false;
	}

	public override short getDefence()
	{
		return ProtectionBonus();
	}

	public override short getDurability()
	{
		return DurabilityBonus();
	}

	public override bool LookAt()
	{
		if (UWEBase._RES == "UW1" && base.item_id == 54)
		{
			base.heading = 7;
			ObjectInteraction.IdentificationFlags identificationFlags = objInt().identity();
			if (identificationFlags == ObjectInteraction.IdentificationFlags.Identified)
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_see_) + StringController.instance.GetString(1, 269));
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
