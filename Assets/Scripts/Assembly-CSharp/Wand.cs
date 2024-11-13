using UnityEngine;

public class Wand : enchantment_base
{
	public a_spell linkedspell;

	protected override int GetActualSpellIndex()
	{
		if (linkedspell != null)
		{
			return linkedspell.link - 256;
		}
		if (UWEBase._RES != "UW2")
		{
			switch (base.link)
			{
			case 579:
			case 580:
			case 581:
				return base.link - 368;
			default:
				return base.link - 256;
			}
		}
		int num = base.link;
		if (num == 576)
		{
			return base.link - 368;
		}
		return base.link - 256;
	}

	public override bool use()
	{
		if (ConversationVM.InConversation)
		{
			return false;
		}
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (!objInt().PickedUp)
			{
				return true;
			}
			if (base.item_id >= 156 && base.item_id <= 159)
			{
				return true;
			}
			if (GetActualSpellIndex() < 0)
			{
				if (base.item_id >= 152 && base.item_id <= 155)
				{
					BreakWand();
				}
				return true;
			}
			if (base.quality > 0)
			{
				UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, GetActualSpellIndex(), 1, 0);
				if (!objInt().isEnchanted)
				{
					base.quality--;
					if (base.quality == 0 && base.item_id >= 152 && base.item_id <= 155)
					{
						BreakWand();
					}
				}
			}
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	private void BreakWand()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_with_a_loud_snap_the_wand_cracks_));
		base.item_id += 4;
		objInt().InvDisplayIndex = objInt().InvDisplayIndex + 4;
		objInt().WorldDisplayIndex = objInt().WorldDisplayIndex + 4;
		objInt().RefreshAnim();
	}

	public override bool LookAt()
	{
		string text = "";
		bool flag = true;
		switch (objInt().identity())
		{
		case ObjectInteraction.IdentificationFlags.Identified:
			text = StringController.instance.GetFormattedObjectNameUW(objInt()) + " of " + StringController.instance.GetString(6, GetActualSpellIndex());
			break;
		case ObjectInteraction.IdentificationFlags.Unidentified:
		case ObjectInteraction.IdentificationFlags.PartiallyIdentified:
			if (UWCharacter.Instance.PlayerSkills.TrySkill(9, getIdentificationLevels(GetActualSpellIndex())))
			{
				base.heading = 7;
				text = StringController.instance.GetFormattedObjectNameUW(objInt()) + " of " + StringController.instance.GetString(6, GetActualSpellIndex());
			}
			else
			{
				flag = false;
				text = StringController.instance.GetFormattedObjectNameUW(objInt());
			}
			break;
		}
		if (base.quality > 0 && !objInt().isEnchanted && flag)
		{
			UWHUD.instance.MessageScroll.Add(text + " with " + base.quality + " charges remaining.");
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(text);
		}
		return true;
	}

	public override void InventoryEventOnLevelExit()
	{
		base.InventoryEventOnLevelExit();
		if (UWEBase._RES == "UW2" && GameWorldController.instance.LevelNo == 42 && SpellIndex == 295 && linkedspell == null && objInt() != UWEBase.CurrentObjectInHand)
		{
			UWCharacter.Instance.playerInventory.RemoveItem(objInt());
			GameWorldController.MoveToWorld(objInt());
			base.transform.position = UWEBase.CurrentTileMap().getTileVector(29, 29);
		}
	}

	public override void MoveToWorldEvent()
	{
		if (base.enchantment != 0 || !(linkedspell != null))
		{
			return;
		}
		bool flag = false;
		ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
		for (int i = 0; i <= objInfo.GetUpperBound(0); i++)
		{
			if (objInfo[i].GetItemType() == 99 && objInfo[i].instance != null && objInfo[i].link == linkedspell.link)
			{
				Object.Destroy(linkedspell.gameObject);
				linkedspell = objInfo[i].instance.GetComponent<a_spell>();
				base.link = i;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			GameWorldController.MoveToWorld(linkedspell.gameObject);
		}
	}

	public override void MoveToInventoryEvent()
	{
		if (base.enchantment == 0 && linkedspell != null)
		{
			GameObject gameObject = Object.Instantiate(linkedspell.gameObject);
			gameObject.name = ObjectInteraction.UniqueObjectName(gameObject.GetComponent<ObjectInteraction>());
			gameObject.gameObject.transform.parent = GameWorldController.instance.InventoryMarker.transform;
			linkedspell = gameObject.GetComponent<a_spell>();
		}
	}

	public override string UseVerb()
	{
		return "cast";
	}
}
