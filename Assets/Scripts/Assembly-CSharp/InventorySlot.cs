using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : GuiBase
{
	public short slotIndex;

	public short SlotCategory;

	public const int GeneralItems = -1;

	public const int ARMOUR = 2;

	public const int HELM = 73;

	public const int RING = 74;

	public const int BOOT = 75;

	public const int GLOVES = 76;

	public const int LEGGINGS = 77;

	public static bool LookingAt;

	public static string TempLookAt;

	private ObjectInteraction QuantityObj = null;

	public static bool Hovering;

	public void BeginDrag()
	{
		if (!UWCharacter.Instance.isRoaming && !Quest.instance.InDreamWorld && Character.InteractionMode != 0 && UWEBase.CurrentObjectInHand == null)
		{
			Character.InteractionMode = 2;
			InteractionModeControl.UpdateNow = true;
			ClickEvent(-2);
		}
	}

	private void UseFromSlot()
	{
		ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
		if (objectIntAtSlot != null)
		{
			objectIntAtSlot.Use();
		}
		else if (UWEBase.CurrentObjectInHand != null)
		{
			UWEBase.CurrentObjectInHand = null;
		}
	}

	private void LookFromSlot()
	{
		ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
		if (objectIntAtSlot != null)
		{
			if (objectIntAtSlot.GetComponent<Readable>() != null)
			{
				objectIntAtSlot.GetComponent<Readable>().Read();
			}
			else
			{
				objectIntAtSlot.LookDescription();
			}
		}
	}

	public void OnClick(BaseEventData evnt)
	{
		PointerEventData pointerEventData = (PointerEventData)evnt;
		ClickEvent(pointerEventData.pointerId);
	}

	private void ClickEvent(int pointerID)
	{
		if (UWCharacter.Instance.isRoaming || Quest.instance.InDreamWorld || Character.InteractionMode == 0)
		{
			return;
		}
		bool flag = true;
		if (pointerID == -2)
		{
			flag = false;
		}
		if (UWCharacter.Instance.PlayerMagic.ReadiedSpell == "")
		{
			switch (Character.InteractionMode)
			{
			case 1:
				if (flag)
				{
					UseFromSlot();
				}
				else
				{
					LookFromSlot();
				}
				break;
			case 2:
				if (flag)
				{
					LeftClickPickup();
				}
				else
				{
					RightClickPickup();
				}
				break;
			case 3:
				if (flag)
				{
					UseFromSlot();
				}
				else
				{
					LookFromSlot();
				}
				break;
			case 4:
				if (flag)
				{
					UseFromSlot();
				}
				else
				{
					LookFromSlot();
				}
				break;
			case 5:
				if (WindowDetect.ContextUIEnabled && WindowDetect.ContextUIUse)
				{
					if (flag || UWEBase.CurrentObjectInHand != null)
					{
						UseFromSlot();
						break;
					}
					RightClickPickup();
					Character.InteractionMode = 2;
					InteractionModeControl.UpdateNow = true;
				}
				else
				{
					UseFromSlot();
				}
				break;
			case 7:
				ConversationClick(flag);
				break;
			case 6:
				break;
			}
		}
		else
		{
			UWCharacter.Instance.PlayerMagic.ObjectInSlot = null;
			if (UWCharacter.Instance.PlayerMagic.InventorySpell)
			{
				UWCharacter.Instance.PlayerMagic.ObjectInSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
				UWCharacter.Instance.PlayerMagic.castSpell(base.gameObject, UWCharacter.Instance.PlayerMagic.ReadiedSpell, false);
				UWCharacter.Instance.PlayerMagic.SpellCost = 0;
				UWHUD.instance.window.UWWindowWait(1f);
			}
		}
	}

	private void ConversationClick(bool isLeftClick)
	{
		if (!isLeftClick)
		{
			ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
			if (objectIntAtSlot != null && objectIntAtSlot.GetComponent<Container>() != null)
			{
				objectIntAtSlot.GetComponent<Container>().OpenContainer();
			}
		}
		else
		{
			RightClickPickup();
		}
	}

	private void LeftClickPickup()
	{
		ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
		bool flag = false;
		if (UWEBase.CurrentObjectInHand != null)
		{
			if (SlotCategory == 73 && UWEBase.CurrentObjectInHand.GetItemType() == 24)
			{
				UWEBase.CurrentObjectInHand.Use();
				flag = true;
				return;
			}
			if (SlotCategory != UWEBase.CurrentObjectInHand.GetItemType() && SlotCategory != -1)
			{
				flag = true;
			}
			if (UWEBase.CurrentObjectInHand.IsStackable() && objectIntAtSlot != null && ObjectInteraction.CanMerge(objectIntAtSlot, UWEBase.CurrentObjectInHand))
			{
				ObjectInteraction.Merge(objectIntAtSlot, UWEBase.CurrentObjectInHand);
				UWEBase.CurrentObjectInHand = null;
				UWCharacter.Instance.playerInventory.Refresh();
				return;
			}
		}
		if (objectIntAtSlot == null)
		{
			if (!flag && Container.TestContainerRules(UWCharacter.Instance.playerInventory.GetCurrentContainer(), slotIndex, false))
			{
				UWCharacter.Instance.playerInventory.SetObjectAtSlot(slotIndex, UWEBase.CurrentObjectInHand);
				UWEBase.CurrentObjectInHand = null;
			}
		}
		else
		{
			if (objectIntAtSlot.Use())
			{
				return;
			}
			if (UWEBase.CurrentObjectInHand != null)
			{
				if (!flag && Container.TestContainerRules(UWCharacter.Instance.playerInventory.GetCurrentContainer(), slotIndex, true))
				{
					UWCharacter.Instance.playerInventory.SwapObjects(objectIntAtSlot, slotIndex, UWEBase.CurrentObjectInHand);
				}
				return;
			}
			UWEBase.CurrentObjectInHand = objectIntAtSlot;
			if (slotIndex >= 11)
			{
				Container component = GameObject.Find(UWCharacter.Instance.playerInventory.currentContainer).GetComponent<Container>();
				component.RemoveItemFromContainer(UWCharacter.Instance.playerInventory.ContainerOffset + slotIndex - 11);
			}
			UWCharacter.Instance.playerInventory.ClearSlot(slotIndex);
		}
	}

	private void RightClickPickup()
	{
		ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
		bool flag = false;
		if (UWEBase.CurrentObjectInHand != null)
		{
			if (SlotCategory != UWEBase.CurrentObjectInHand.GetItemType() && SlotCategory != -1)
			{
				flag = true;
			}
			if (SlotCategory == 73 && UWEBase.CurrentObjectInHand.GetItemType() == 24)
			{
				UWEBase.CurrentObjectInHand.Use();
				flag = true;
				return;
			}
			if (UWEBase.CurrentObjectInHand.IsStackable() && objectIntAtSlot != null && ObjectInteraction.CanMerge(objectIntAtSlot, UWEBase.CurrentObjectInHand))
			{
				ObjectInteraction.Merge(objectIntAtSlot, UWEBase.CurrentObjectInHand);
				UWEBase.CurrentObjectInHand = null;
				UWCharacter.Instance.playerInventory.Refresh();
				return;
			}
		}
		if (UWCharacter.Instance.playerInventory.GetObjectAtSlot(slotIndex) != null && UWEBase.CurrentObjectInHand == null && objectIntAtSlot.GetComponent<Container>() != null)
		{
			if (!objectIntAtSlot.GetComponent<Container>().isOpenOnPanel)
			{
				UWEBase.CurrentObjectInHand = objectIntAtSlot.GetComponent<ObjectInteraction>();
				if (slotIndex >= 11)
				{
					UWCharacter.Instance.playerInventory.GetCurrentContainer().RemoveItemFromContainer(UWCharacter.Instance.playerInventory.ContainerOffset + slotIndex - 11);
				}
				UWCharacter.Instance.playerInventory.ClearSlot(slotIndex);
			}
			return;
		}
		if (UWCharacter.Instance.playerInventory.GetObjectAtSlot(slotIndex) == null)
		{
			if (!flag && Container.TestContainerRules(UWCharacter.Instance.playerInventory.GetCurrentContainer(), slotIndex, false))
			{
				UWCharacter.Instance.playerInventory.SetObjectAtSlot(slotIndex, UWEBase.CurrentObjectInHand);
				UWEBase.CurrentObjectInHand = null;
			}
			return;
		}
		bool flag2 = false;
		if (UWEBase.CurrentObjectInHand != null)
		{
			flag2 = objectIntAtSlot.GetComponent<ObjectInteraction>().Use();
		}
		if (flag2)
		{
			return;
		}
		if (UWEBase.CurrentObjectInHand != null)
		{
			if (!flag && Container.TestContainerRules(UWCharacter.Instance.playerInventory.GetCurrentContainer(), slotIndex, true))
			{
				UWCharacter.Instance.playerInventory.SwapObjects(objectIntAtSlot, slotIndex, UWEBase.CurrentObjectInHand);
				UWCharacter.Instance.playerInventory.Refresh();
			}
		}
		else
		{
			if (flag)
			{
				return;
			}
			ObjectInteraction component = objectIntAtSlot.GetComponent<ObjectInteraction>();
			if (!component.IsStackable() || (component.IsStackable() && component.GetQty() <= 1))
			{
				UWEBase.CurrentObjectInHand = objectIntAtSlot;
				if (slotIndex >= 11)
				{
					UWCharacter.Instance.playerInventory.GetCurrentContainer().RemoveItemFromContainer(UWCharacter.Instance.playerInventory.ContainerOffset + slotIndex - 11);
				}
				UWCharacter.Instance.playerInventory.ClearSlot(slotIndex);
				return;
			}
			if (ConversationVM.InConversation)
			{
				TempLookAt = UWHUD.instance.MessageScroll.NewUIOUt.text;
				UWHUD.instance.MessageScroll.Set("Move how many?");
				ConversationVM.EnteringQty = true;
			}
			else
			{
				UWHUD.instance.MessageScroll.Set("Move how many?");
			}
			InputField inputControl = UWHUD.instance.InputControl;
			inputControl.gameObject.SetActive(true);
			inputControl.text = component.GetQty().ToString();
			inputControl.gameObject.GetComponent<InputHandler>().target = base.gameObject;
			inputControl.gameObject.GetComponent<InputHandler>().currentInputMode = 1;
			inputControl.contentType = InputField.ContentType.IntegerNumber;
			inputControl.Select();
			WindowDetect.WaitingForInput = true;
			Time.timeScale = 0f;
			QuantityObj = objectIntAtSlot;
		}
	}

	public void OnSubmitPickup(int quant)
	{
		InputField inputControl = UWHUD.instance.InputControl;
		inputControl.text = "";
		inputControl.gameObject.SetActive(false);
		WindowDetect.WaitingForInput = false;
		ConversationVM.EnteringQty = false;
		if (!ConversationVM.InConversation)
		{
			UWHUD.instance.MessageScroll.Clear();
			Time.timeScale = 1f;
		}
		else
		{
			UWHUD.instance.MessageScroll.NewUIOUt.text = TempLookAt;
		}
		if (quant == 0)
		{
			QuantityObj = null;
		}
		if (!(QuantityObj != null))
		{
			return;
		}
		if (quant >= QuantityObj.GetComponent<ObjectInteraction>().link)
		{
			UWEBase.CurrentObjectInHand = QuantityObj;
			if (slotIndex >= 11)
			{
				UWCharacter.Instance.playerInventory.GetCurrentContainer().RemoveItemFromContainer(UWCharacter.Instance.playerInventory.ContainerOffset + slotIndex - 11);
			}
			UWCharacter.Instance.playerInventory.ClearSlot(slotIndex);
			return;
		}
		ObjectInteraction component = QuantityObj.GetComponent<ObjectInteraction>();
		component.link -= quant;
		ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(component.item_id, component.quality, component.owner, quant, -1);
		objectLoaderInfo.is_quant = 1;
		ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.InventoryMarker, GameWorldController.instance.InventoryMarker.transform.position);
		GameWorldController.MoveToInventory(objectInteraction);
		UWEBase.CurrentObjectInHand = objectInteraction;
		ObjectInteraction.Split(component, objectInteraction);
		UWCharacter.Instance.playerInventory.Refresh();
		QuantityObj = null;
	}

	public ObjectInteraction GetGameObjectInteration()
	{
		return UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
	}

	private void TemporaryLookAt()
	{
		if (!LookingAt)
		{
			ObjectInteraction gameObjectInteration = GetGameObjectInteration();
			if (gameObjectInteration != null)
			{
				LookingAt = true;
				TempLookAt = UWHUD.instance.MessageScroll.NewUIOUt.text;
				StartCoroutine(ClearTempLookAt());
				UWHUD.instance.MessageScroll.DirectSet(StringController.instance.GetFormattedObjectNameUW(gameObjectInteration));
			}
		}
	}

	private IEnumerator ClearTempLookAt()
	{
		Time.timeScale = 0.1f;
		yield return new WaitForSeconds(0.1f);
		LookingAt = false;
		if (ConversationVM.InConversation)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
		UWHUD.instance.MessageScroll.DirectSet(TempLookAt);
	}

	public void OnHoverEnter()
	{
		Hovering = true;
		UWHUD.instance.ContextMenu.text = "";
		ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex);
		if (!(objectIntAtSlot != null))
		{
			return;
		}
		string text = "";
		string text2 = "";
		text = objectIntAtSlot.GetComponent<object_base>().ContextMenuDesc(objectIntAtSlot.item_id);
		if (UWEBase.CurrentObjectInHand == null)
		{
			switch (Character.InteractionMode)
			{
			case 5:
				text2 = "L-Click to " + objectIntAtSlot.UseVerb() + " R-Click to " + objectIntAtSlot.PickupVerb();
				break;
			case 3:
				text2 = "L-Click to " + objectIntAtSlot.UseVerb() + " R-Click to " + objectIntAtSlot.ExamineVerb();
				break;
			case 2:
				text2 = "L-Click to " + objectIntAtSlot.UseVerb() + " R-Click to " + objectIntAtSlot.PickupVerb();
				break;
			}
		}
		else
		{
			text2 = "L-Click to " + objectIntAtSlot.UseObjectOnVerb_Inv();
		}
		UWHUD.instance.ContextMenu.text = text + "\n" + text2;
	}

	public void OnHoverExit()
	{
		UWHUD.instance.ContextMenu.text = "";
		Hovering = false;
	}
}
