using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TradeSlot : GuiBase
{
	public static int TradeSlotUBound = 3;

	public bool PlayerSlot = false;

	public int SlotNo;

	public bool pressedDown = false;

	public ObjectInteraction objectInSlot;

	public bool Hovering = false;

	public RawImage SlotImage;

	public bool Selected = false;

	private Texture2D Blank;

	private Texture2D IndicatorSelected;

	public RawImage Indicator;

	public Text Quantity;

	public static bool LookingAt;

	public static string TempLookAt;

	public static bool Locked;

	public override void Start()
	{
		base.Start();
		if (UWEBase._RES == "UW2")
		{
			TradeSlotUBound = 5;
		}
		SlotImage = GetComponent<RawImage>();
		Blank = Resources.Load<Texture2D>(UWEBase._RES + "/Sprites/Texture_Blank");
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW1")
		{
			IndicatorSelected = GameWorldController.instance.grCursors.LoadImageAt(18);
		}
		else
		{
			IndicatorSelected = GameWorldController.instance.grCursors.LoadImageAt(0);
		}
		SlotImage.texture = Blank;
		Quantity.text = "";
		Indicator.texture = Blank;
	}

	public override void Update()
	{
		base.Update();
		if (isSelected())
		{
			Indicator.texture = IndicatorSelected;
		}
		else
		{
			Indicator.texture = Blank;
		}
		if (objectInSlot == null)
		{
			Quantity.text = "";
		}
	}

	public void PlayerSlotRightClick()
	{
		if (!LookingAt && !Locked)
		{
			ObjectInteraction gameObjectInteraction = GetGameObjectInteraction();
			if (gameObjectInteraction != null)
			{
				LookingAt = true;
				TempLookAt = UWHUD.instance.MessageScroll.NewUIOUt.text;
				StartCoroutine(ClearTempLookAt());
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
		UWHUD.instance.MessageScroll.NewUIOUt.text = TempLookAt;
	}

	public void PlayerSlotLeftClick()
	{
		if (Locked)
		{
			return;
		}
		ObjectInteraction gameObjectInteraction = GetGameObjectInteraction();
		if (UWEBase.CurrentObjectInHand != null)
		{
			if (objectInSlot == null)
			{
				if (UWEBase.CurrentObjectInHand != null)
				{
					if (UWEBase.CurrentObjectInHand.transform.parent != GameWorldController.instance.DynamicObjectMarker())
					{
						GameWorldController.MoveToWorld(UWEBase.CurrentObjectInHand);
						ConversationVM.BuildObjectList();
					}
					objectInSlot = UWEBase.CurrentObjectInHand;
					UWEBase.CurrentObjectInHand = null;
					SlotImage.texture = UWHUD.instance.CursorIcon;
					Selected = true;
				}
			}
			else
			{
				if (gameObjectInteraction != null)
				{
					gameObjectInteraction.transform.parent = GameWorldController.instance.InventoryMarker.transform;
					GameWorldController.MoveToInventory(gameObjectInteraction);
				}
				if (UWEBase.CurrentObjectInHand != null && UWEBase.CurrentObjectInHand.transform.parent != GameWorldController.instance.DynamicObjectMarker())
				{
					GameWorldController.MoveToWorld(UWEBase.CurrentObjectInHand);
				}
				objectInSlot = UWEBase.CurrentObjectInHand;
				SlotImage.texture = UWEBase.CurrentObjectInHand.GetInventoryDisplay().texture;
				UWEBase.CurrentObjectInHand = gameObjectInteraction;
				ConversationVM.BuildObjectList();
				Selected = true;
			}
		}
		else if (objectInSlot != null)
		{
			UWEBase.CurrentObjectInHand = gameObjectInteraction;
			objectInSlot = null;
			SlotImage.texture = Blank;
			Selected = false;
			if (UWEBase.CurrentObjectInHand != null)
			{
				UWEBase.CurrentObjectInHand.transform.parent = GameWorldController.instance.InventoryMarker.transform;
				GameWorldController.MoveToInventory(UWEBase.CurrentObjectInHand);
				UWEBase.CurrentObjectInHand.GetComponent<object_base>().PickupEvent();
			}
		}
		if (objectInSlot == null)
		{
			Quantity.text = "";
			return;
		}
		int qty = GetGameObjectInteraction().GetQty();
		if (qty <= 1)
		{
			Quantity.text = "";
		}
		else
		{
			Quantity.text = qty.ToString();
		}
	}

	public void NPCSlotClick()
	{
		Selected = !Selected;
	}

	public void OnClick(BaseEventData evnt)
	{
		PointerEventData pointerEventData = (PointerEventData)evnt;
		ClickEvent(pointerEventData.pointerId);
	}

	public void ClickEvent(int ptrID)
	{
		if (Locked)
		{
			return;
		}
		if (PlayerSlot)
		{
			if (ptrID == -2)
			{
				PlayerSlotRightClick();
			}
			else
			{
				PlayerSlotLeftClick();
			}
		}
		else if (ptrID == -2)
		{
			PlayerSlotRightClick();
		}
		else
		{
			NPCSlotClick();
		}
		UWCharacter.Instance.playerInventory.Refresh();
	}

	public bool isSelected()
	{
		return Selected && objectInSlot != null;
	}

	public void clear()
	{
		objectInSlot = null;
		SlotImage.texture = Blank;
	}

	public int GetObjectID()
	{
		if (isSelected() && objectInSlot != null)
		{
			return objectInSlot.item_id;
		}
		return 0;
	}

	public ObjectInteraction GetGameObjectInteraction()
	{
		Debug.Log("REMOVE3");
		return objectInSlot;
	}
}
