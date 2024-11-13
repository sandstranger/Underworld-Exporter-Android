using UnityEngine;

public class enchantment_base : object_base
{
	[Header("Enchantment Properties")]
	public string DisplayEnchantment;

	public int SpellIndex;

	protected override void Start()
	{
		base.Start();
		SetDisplayEnchantment();
		SpellIndex = GetActualSpellIndex();
	}

	public void SetDisplayEnchantment()
	{
		DisplayEnchantment = StringController.instance.GetString(6, GetActualSpellIndex());
		if (DisplayEnchantment == "")
		{
			DisplayEnchantment = "NONE";
		}
	}

	public override bool use()
	{
		if (ConversationVM.InConversation)
		{
			return false;
		}
		if (UWEBase.CurrentObjectInHand == null)
		{
			UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, GetActualSpellIndex(), 1, 1);
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	protected virtual int GetActualSpellIndex()
	{
		int num = base.link - 512;
		if (objInt().GetItemType() != 74)
		{
			if (num < 63)
			{
				return num + 256;
			}
			return num + 144;
		}
		return num;
	}

	public override bool LookAt()
	{
		if (!objInt().PickedUp)
		{
			return base.LookAt();
		}
		switch (base.item_id)
		{
		case 54:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
			break;
		case 184:
		case 185:
		case 186:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
			break;
		default:
		{
			string text = StringController.instance.GetString(6, GetActualSpellIndex());
			switch (objInt().identity())
			{
			case ObjectInteraction.IdentificationFlags.Identified:
				if (text == "")
				{
					text = "UNNAMED";
				}
				DisplayEnchantment = text;
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()) + " of " + text);
				break;
			case ObjectInteraction.IdentificationFlags.Unidentified:
			case ObjectInteraction.IdentificationFlags.PartiallyIdentified:
				if (UWCharacter.Instance.PlayerSkills.TrySkill(9, getIdentificationLevels(GetActualSpellIndex())))
				{
					base.heading = 7;
					if (text != "")
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()) + " of " + text);
					}
					else
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
					}
				}
				else
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
				}
				break;
			}
			break;
		}
		}
		return true;
	}

	public override string ContextMenuDesc(int item_id)
	{
		ObjectInteraction.IdentificationFlags identificationFlags = objInt().identity();
		if (identificationFlags == ObjectInteraction.IdentificationFlags.Identified)
		{
			return StringController.instance.GetFormattedObjectNameUW(objInt()) + " of " + DisplayEnchantment;
		}
		return base.ContextMenuDesc(item_id);
	}

	public override float GetWeight()
	{
		return (float)GameWorldController.instance.commonObject.properties[base.item_id].mass * 0.1f;
	}
}
