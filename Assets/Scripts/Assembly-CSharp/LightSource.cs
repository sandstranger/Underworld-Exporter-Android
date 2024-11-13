using UnityEngine;

public class LightSource : object_base
{
	public static float BaseBrightness = 16f;

	public static float MagicBrightness = 0f;

	public float LightTimerMax = 30f;

	public float LightTimer;

	public bool IsOn()
	{
		switch (base.item_id)
		{
		case 144:
		case 145:
		case 146:
		case 147:
			return false;
		case 148:
		case 149:
		case 150:
		case 151:
			return true;
		default:
			return false;
		}
	}

	public override void Update()
	{
		base.Update();
		if (!IsOn() || !objInt().PickedUp || Duration() == 0)
		{
			return;
		}
		LightTimer -= Time.deltaTime;
		if (LightTimer <= 0f)
		{
			base.quality--;
			LightTimer = LightTimerMax;
			if (base.quality <= 0)
			{
				base.quality = 0;
				SetOff();
			}
		}
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (!objInt().PickedUp)
			{
				if (IsOn())
				{
					SetOff();
				}
				return true;
			}
			if (IsOn())
			{
				SetOff();
			}
			else
			{
				SetOn();
			}
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed != null)
		{
			int itemType = ObjectUsed.GetItemType();
			if (itemType == 89 && base.item_id == 149)
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_think_it_is_a_bad_idea_to_add_oil_to_the_lit_torch_));
				return true;
			}
		}
		return base.ActivateByObject(ObjectUsed);
	}

	public void SetOn()
	{
		if (base.quality <= 0)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_that_light_is_already_used_up_));
			return;
		}
		LightTimer = LightTimerMax;
		PlayerInventory playerInventory = UWCharacter.Instance.playerInventory;
		InventorySlot invSlot = null;
		GetInventorySlotForLightSource(playerInventory, ref invSlot);
		if (invSlot != null)
		{
			if (!objInt().isQuant || (objInt().isQuant && base.link <= 1) || objInt().isEnchanted)
			{
				PutLightSourceInSlot(playerInventory, invSlot);
			}
			else if (objInt().GetItemType() != 88)
			{
				SplitLightSourceIntoSlot();
			}
			else
			{
				PutLightSourceInSlot(playerInventory, invSlot);
			}
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_there_is_no_place_to_put_that_));
		}
		objInt().RefreshAnim();
		UWCharacter.Instance.playerInventory.UpdateLightSources();
	}

	private void SplitLightSourceIntoSlot()
	{
		GameObject gameObject = Object.Instantiate(base.gameObject);
		gameObject.name = ObjectInteraction.UniqueObjectName(gameObject.GetComponent<ObjectInteraction>());
		gameObject.GetComponent<ObjectInteraction>().link = 1;
		base.link--;
		if (base.link < 0)
		{
			base.link = 0;
		}
		gameObject.transform.parent = base.transform.parent;
		gameObject.GetComponent<ObjectInteraction>().Use();
		gameObject.GetComponent<ObjectInteraction>().isquant = 1;
	}

	private void PutLightSourceInSlot(PlayerInventory pInv, InventorySlot invSlot)
	{
		pInv.RemoveItem(objInt());
		pInv.SetObjectAtSlot(invSlot.slotIndex, objInt());
		objInt().inventorySlot = invSlot.slotIndex;
		base.item_id += 4;
		objInt().InvDisplayIndex = base.item_id;
	}

	private void GetInventorySlotForLightSource(PlayerInventory pInv, ref InventorySlot invSlot)
	{
		if (pInv.sRightShoulder == objInt())
		{
			invSlot = UWHUD.instance.RightShoulder_Slot.gameObject.GetComponent<InventorySlot>();
		}
		else if (pInv.sLeftShoulder == objInt())
		{
			invSlot = UWHUD.instance.LeftShoulder_Slot.gameObject.GetComponent<InventorySlot>();
		}
		else if (pInv.sRightHand == objInt())
		{
			invSlot = UWHUD.instance.RightHand_Slot.gameObject.GetComponent<InventorySlot>();
		}
		else if (pInv.sLeftHand == objInt())
		{
			invSlot = UWHUD.instance.LeftHand_Slot.gameObject.GetComponent<InventorySlot>();
		}
	}

	public void SetOff()
	{
		base.item_id -= 4;
		objInt().InvDisplayIndex = base.item_id;
		objInt().WorldDisplayIndex = base.item_id;
		base.isquant = 1;
		objInt().RefreshAnim();
		UWCharacter.Instance.playerInventory.UpdateLightSources();
	}

	public override bool LookAt()
	{
		if (UWEBase._RES == "UW1" && (base.item_id == 147 || base.item_id == 151))
		{
			base.heading = 7;
			ObjectInteraction.IdentificationFlags identificationFlags = objInt().identity();
			if (identificationFlags != ObjectInteraction.IdentificationFlags.Identified)
			{
			}
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_see_) + StringController.instance.GetString(1, 262));
			return true;
		}
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), lightStatusText()) + OwnershipString());
		return true;
	}

	private string lightStatusText()
	{
		if (base.quality == 0)
		{
			return StringController.instance.GetString(5, 60);
		}
		if (base.quality >= 1 && base.quality < 15)
		{
			return StringController.instance.GetString(5, 61);
		}
		if (base.quality >= 15 && base.quality < 32)
		{
			return StringController.instance.GetString(5, 62);
		}
		if (base.quality >= 32 && base.quality < 49)
		{
			return StringController.instance.GetString(5, 63);
		}
		if (base.quality >= 50 && base.quality < 64)
		{
			return StringController.instance.GetString(5, 64);
		}
		return StringController.instance.GetString(5, 64);
	}

	public override bool PutItemAwayEvent(short slotNo)
	{
		if (IsOn())
		{
			SetOff();
		}
		return true;
	}

	public override string UseVerb()
	{
		if (IsOn())
		{
			return "douse";
		}
		return "ignite";
	}

	public float Brightness()
	{
		return (float)GameWorldController.instance.objDat.lightSourceStats[base.item_id - 144].brightness * 1.5f;
	}

	public int Duration()
	{
		return GameWorldController.instance.objDat.lightSourceStats[base.item_id - 144].duration;
	}

	public override bool DropEvent()
	{
		if (IsOn())
		{
			SetOff();
			return true;
		}
		return false;
	}
}
