using UnityEngine;
using UnityEngine.UI;

public class Equipment : object_base
{
	public int EquipIconIndex;

	public string DisplayEnchantment;

	protected override void Start()
	{
		base.Start();
		UpdateQuality();
		SetDisplayEnchantment();
	}

	public void SetDisplayEnchantment()
	{
		DisplayEnchantment = StringController.instance.GetString(6, GetActualSpellIndex());
		if (DisplayEnchantment == "")
		{
			DisplayEnchantment = "NONE";
		}
	}

	public virtual int GetActualSpellIndex()
	{
		return base.link - 256;
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand != null)
		{
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		return false;
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed != null)
		{
			int itemType = ObjectUsed.GetItemType();
			if (itemType == 85)
			{
				UWHUD.instance.MessageScroll.Set("[placeholder]You think it will be hard/easy to repair this item. Press Y or N followed by enter to proceed");
				InputField inputControl = UWHUD.instance.InputControl;
				inputControl.gameObject.SetActive(true);
				inputControl.gameObject.GetComponent<InputHandler>().target = base.gameObject;
				inputControl.gameObject.GetComponent<InputHandler>().currentInputMode = 4;
				inputControl.contentType = InputField.ContentType.Alphanumeric;
				inputControl.Select();
				WindowDetect.WaitingForInput = true;
				Time.timeScale = 0f;
				return true;
			}
		}
		return false;
	}

	public virtual void UpdateQuality()
	{
	}

	public void OnSubmitPickup(string ans)
	{
		Time.timeScale = 1f;
		UWHUD.instance.InputControl.gameObject.SetActive(false);
		WindowDetect.WaitingForInput = false;
		UWHUD.instance.MessageScroll.Clear();
		if (ans.Substring(0, 1).ToUpper() == "Y")
		{
			UWHUD.instance.CutScenesSmall.anim.SetAnimation = "cs404.n01";
			if (UWCharacter.Instance.PlayerSkills.TrySkill(15, 0))
			{
				base.quality += 5;
				if (base.quality > 63)
				{
					base.quality = 63;
				}
				UWHUD.instance.MessageScroll.Add("You repair the item");
			}
			else
			{
				UWHUD.instance.MessageScroll.Add("You fail to repair the item");
			}
			UpdateQuality();
		}
		UWEBase.CurrentObjectInHand = null;
	}

	public override bool LookAt()
	{
		if (objInt().isEnchanted)
		{
			switch (objInt().identity())
			{
			case ObjectInteraction.IdentificationFlags.Identified:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), GetEquipmentConditionString()) + " of " + StringController.instance.GetString(6, GetActualSpellIndex()) + OwnershipString());
				SetDisplayEnchantment();
				break;
			case ObjectInteraction.IdentificationFlags.Unidentified:
			case ObjectInteraction.IdentificationFlags.PartiallyIdentified:
				if (UWCharacter.Instance.PlayerSkills.TrySkill(9, getIdentificationLevels(GetActualSpellIndex())))
				{
					base.heading = 7;
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), GetEquipmentConditionString()) + " of " + StringController.instance.GetString(6, GetActualSpellIndex()) + OwnershipString());
				}
				else
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), GetEquipmentConditionString()) + OwnershipString());
				}
				break;
			}
			return true;
		}
		if (objInt().PickedUp)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), GetEquipmentConditionString()));
			return true;
		}
		return base.LookAt();
	}

	public virtual string GetEquipmentConditionString()
	{
		if (UWEBase._RES == "UW1" && (base.item_id == 10 || base.item_id == 55 || base.item_id == 47 || base.item_id == 48 || base.item_id == 49 || base.item_id == 50))
		{
			return StringController.instance.GetString(5, 10);
		}
		if (base.quality > 0 && base.quality <= 15)
		{
			return StringController.instance.GetString(5, 7);
		}
		if (base.quality > 15 && base.quality <= 30)
		{
			return StringController.instance.GetString(5, 8);
		}
		if (base.quality > 30 && base.quality <= 45)
		{
			return StringController.instance.GetString(5, 9);
		}
		return StringController.instance.GetString(5, 10);
	}

	public virtual void SelfDamage(short damage)
	{
		if (UWEBase._RES == "UW1" && (base.item_id == 10 || base.item_id == 55 || base.item_id == 47 || base.item_id == 48 || base.item_id == 49 || base.item_id == 50))
		{
			return;
		}
		short num = (short)EquipIconIndex;
		base.quality -= damage;
		UpdateQuality();
		if (base.quality <= 0)
		{
			UWHUD.instance.MessageScroll.Add("Your " + StringController.instance.GetSimpleObjectNameUW(base.item_id) + " was destroyed");
			ChangeType(208);
			base.gameObject.AddComponent<object_base>();
			if ((bool)GetComponent<Weapon>())
			{
				UWCharacter.Instance.PlayerCombat.currWeapon = null;
			}
		}
		else if (num != EquipIconIndex)
		{
			UWHUD.instance.MessageScroll.Add("Your " + StringController.instance.GetSimpleObjectNameUW(base.item_id) + " was damaged");
		}
	}

	public virtual short getDefence()
	{
		return GameWorldController.instance.objDat.armourStats[base.item_id - 32].protection;
	}

	public virtual short getDurability()
	{
		return DurabilityBonus();
	}

	public short DurabilityBonus()
	{
		switch (base.link)
		{
		case 472:
		case 473:
		case 474:
		case 475:
		case 476:
		case 477:
		case 478:
		case 479:
			return (short)(base.link - 472);
		default:
			return 0;
		}
	}

	public short ProtectionBonus()
	{
		switch (base.link)
		{
		case 464:
		case 465:
		case 466:
		case 467:
		case 468:
		case 469:
		case 470:
		case 471:
			return (short)(base.link - 461);
		default:
			return 0;
		}
	}

	public override string ContextMenuDesc(int item_id)
	{
		if (objInt().isEnchanted)
		{
			ObjectInteraction.IdentificationFlags identificationFlags = objInt().identity();
			if (identificationFlags == ObjectInteraction.IdentificationFlags.Identified)
			{
				return StringController.instance.GetFormattedObjectNameUW(objInt()) + " of " + DisplayEnchantment;
			}
			return base.ContextMenuDesc(item_id);
		}
		return base.ContextMenuDesc(item_id);
	}
}
