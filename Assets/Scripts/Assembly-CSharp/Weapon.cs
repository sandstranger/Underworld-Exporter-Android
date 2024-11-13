public class Weapon : Equipment
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if ((objInt().inventorySlot == 7 && !UWCharacter.Instance.isLefty) || (objInt().inventorySlot == 8 && UWCharacter.Instance.isLefty))
			{
				if (Character.InteractionMode == 4)
				{
					Character.InteractionMode = 5;
				}
				else
				{
					Character.InteractionMode = 4;
				}
			}
			InteractionModeControl.UpdateNow = true;
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool EquipEvent(short slotNo)
	{
		if ((slotNo == 7 && !UWCharacter.Instance.isLefty) || (slotNo == 8 && UWCharacter.Instance.isLefty))
		{
			if (GetComponent<WeaponRanged>() != null)
			{
				UWCharacter.Instance.PlayerCombat.currWeaponRanged = (WeaponRanged)this;
			}
			if (GetComponent<WeaponMelee>() != null)
			{
				UWCharacter.Instance.PlayerCombat.currWeapon = (WeaponMelee)this;
			}
		}
		return true;
	}

	public override bool UnEquipEvent(short slotNo)
	{
		if ((slotNo == 7 && !UWCharacter.Instance.isLefty) || (slotNo == 8 && UWCharacter.Instance.isLefty))
		{
			UWCharacter.Instance.PlayerCombat.currWeapon = null;
			UWCharacter.Instance.PlayerCombat.currWeaponRanged = null;
		}
		return false;
	}
}
