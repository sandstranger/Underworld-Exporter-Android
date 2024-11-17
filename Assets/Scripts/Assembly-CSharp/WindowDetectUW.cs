using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDetectUW : WindowDetect
{
	public static bool UsingRoomManager = false;

	private Vector3 windowedPosition;

	private Vector3 windowedSize;

	public override void Start()
	{
		base.Start();
		JustClicked = false;
		WindowWaitCount = 0f;
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			UWCharacter.Instance.playerCam.rect = new Rect(0.05f, 0.28f, 0.655f, 0.64f);
		}
		else
		{
			UWCharacter.Instance.playerCam.rect = new Rect(0.163f, 0.335f, 0.54f, 0.572f);
		}
		
		SetFullScreen();
	}

	public void UWWindowWait(float waitTime)
	{
		JustClicked = true;
		WindowWaitCount = waitTime;
	}

	public override void Update()
	{
		if (UWCharacter.Instance.isRoaming || UWCharacter.Instance.CurVIT < 0)
		{
			return;
		}
		if (JustClicked)
		{
			WindowWaitCount -= Time.deltaTime;
			if (WindowWaitCount <= 0f)
			{
				JustClicked = false;
			}
			return;
		}
		int interactionMode = Character.InteractionMode;
		if (interactionMode == 4)
		{
			if (UWCharacter.Instance.PlayerMagic.ReadiedSpell != "" || UWCharacter.Instance.PlayerCombat.AttackExecuting)
			{
				return;
			}
			if (!WindowDetect.CursorInMainWindow)
			{
				MouseHeldDown = false;
				UWCharacter.Instance.PlayerCombat.AttackCharging = false;
			}
			if (MouseHeldDown)
			{
				if (!UWCharacter.Instance.PlayerCombat.AttackCharging)
				{
					UWCharacter.Instance.PlayerCombat.CombatBegin();
				}
				if (UWCharacter.Instance.PlayerCombat.AttackCharging && UWCharacter.Instance.PlayerCombat.Charge < 100f)
				{
					UWCharacter.Instance.PlayerCombat.CombatCharging();
				}
				return;
			}
			if (UWCharacter.Instance.PlayerCombat.AttackCharging)
			{
				UWCharacter.Instance.PlayerCombat.ReleaseAttack();
			}
		}
		if (WindowDetect.ContextUIEnabled && !InventorySlot.Hovering)
		{
			ContextUIMode();
		}
	}

	private void ContextUIMode()
	{
		Ray ray = ((!UWCharacter.Instance.MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
		UWHUD.instance.ContextMenu.text = "";
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, UWCharacter.Instance.GetUseRange()) && hitInfo.transform.gameObject.GetComponent<ObjectInteraction>() != null)
		{
			UWHUD.instance.ContextMenu.text = hitInfo.transform.gameObject.GetComponent<ObjectInteraction>().LookDescriptionContext();
		}
	}

	public void OnMouseEnter()
	{
		WindowDetect.CursorInMainWindow = true;
	}

	public void OnMouseExit()
	{
		WindowDetect.CursorInMainWindow = false;
	}

	public void OnMouseDown(BaseEventData evnt)
	{
		PointerEventData pointerEventData = (PointerEventData)evnt;
		OnPress(true, pointerEventData.pointerId);
	}

	public void OnMouseUp(BaseEventData evnt)
	{
		PointerEventData pointerEventData = (PointerEventData)evnt;
		OnPress(false, pointerEventData.pointerId);
	}

	protected override void OnPress(bool isPressed, int PtrID)
	{
		if (UWCharacter.Instance.isRoaming)
		{
			return;
		}
		base.OnPress(isPressed, PtrID);
		if (WindowDetect.CursorInMainWindow && !JustClicked)
		{
			int interactionMode = Character.InteractionMode;
			if (interactionMode == 2)
			{
				ClickEvent(PtrID);
			}
		}
	}

	public void OnClick(BaseEventData evnt)
	{
		PointerEventData pointerEventData = (PointerEventData)evnt;
		OnClick(pointerEventData.pointerId);
	}

	public void OnClick(int ptrID)
	{
		if (UWCharacter.Instance.isRoaming || JustClicked)
		{
			return;
		}
		if (UWHUD.instance.CutScenesSmall.anim.SetAnimation.ToUpper() != "ANIM_BASE" && UWCharacter.Instance.CurVIT > 0)
		{
			UWHUD.instance.CutScenesSmall.anim.SetAnimation = "Anim_Base";
			return;
		}
		int interactionMode = Character.InteractionMode;
		if (interactionMode != 2)
		{
			ClickEvent(ptrID);
		}
	}

	private void ClickEvent(int ptrID)
	{
		if (UWCharacter.Instance.PlayerMagic.ReadiedSpell != "" || JustClicked)
		{
			return;
		}
		if (WindowDetect.ContextUIEnabled && WindowDetect.ContextUIUse && UWEBase.CurrentObjectInHand == null)
		{
			if (object_base.UseAvail && ptrID == -1)
			{
				Character.InteractionMode = 5;
			}
			if (object_base.PickAvail && ptrID == -2)
			{
				Character.InteractionMode = 2;
			}
			if (object_base.TalkAvail && ptrID == -1)
			{
				Character.InteractionMode = 1;
			}
		}
		InteractionModeControl.UpdateNow = true;
		switch (Character.InteractionMode)
		{
		case 0:
			break;
		case 1:
			UWCharacter.Instance.TalkMode();
			break;
		case 2:
			if (UWEBase.CurrentObjectInHand != null)
			{
				UWWindowWait(1f);
				ThrowObjectInHand();
			}
			else
			{
				UWCharacter.Instance.PickupMode(ptrID);
			}
			break;
		case 3:
			UWCharacter.Instance.LookMode();
			break;
		case 4:
			break;
		case 5:
			if (UWEBase.CurrentObjectInHand != null)
			{
				UWCharacter.Instance.UseMode();
			}
			else
			{
				UWCharacter.Instance.UseMode();
			}
			break;
		}
	}

	protected override void ThrowObjectInHand()
	{
		base.ThrowObjectInHand();
		if (!(UWEBase.CurrentObjectInHand != null))
		{
			return;
		}
		if (!UWCharacter.Instance.playerInventory.JustPickedup)
		{
			Ray ray = ((!UWCharacter.Instance.MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
			RaycastHit hitInfo = default(RaycastHit);
			float num = 0.5f;
			if (Physics.Raycast(ray, out hitInfo, num))
			{
				return;
			}
			float num2 = Input.mousePosition.y / (float)Camera.main.pixelHeight * 200f;
			GameObject gameObject = UWEBase.CurrentObjectInHand.gameObject;
			gameObject.GetComponent<ObjectInteraction>().Drop();
			gameObject.GetComponent<ObjectInteraction>().UpdateAnimation();
			GameWorldController.MoveToWorld(gameObject);
			gameObject.transform.parent = GameWorldController.instance.DynamicObjectMarker();
			if (gameObject.GetComponent<Container>() != null)
			{
				Container.SetPickedUpFlag(gameObject.GetComponent<Container>(), false);
				Container.SetItemsParent(gameObject.GetComponent<Container>(), GameWorldController.instance.DynamicObjectMarker());
				Container.SetItemsPosition(gameObject.GetComponent<Container>(), UWCharacter.Instance.playerInventory.InventoryMarker.transform.position);
			}
			gameObject.transform.position = ray.GetPoint(num - 0.1f);
			UWEBase.UnFreezeMovement(gameObject);
			if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.4f)
			{
				Vector3 vector = ray.GetPoint(num) - ray.origin;
				if (gameObject.GetComponent<Rigidbody>() != null)
				{
					gameObject.GetComponent<Rigidbody>().AddForce(vector * num2);
				}
			}
			UWEBase.CurrentObjectInHand = null;
		}
		else
		{
			UWCharacter.Instance.playerInventory.JustPickedup = false;
		}
	}

	public void SetFullScreen()
	{
		FullScreen = true;
		setPositions();
		UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW1, false);
		UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW2, false);
		RectTransform component = GetComponent<RectTransform>();
		windowedPosition = component.localPosition;
		windowedSize = component.sizeDelta;
		component.localPosition = new Vector3(0f, 0f, 0f);
		component.sizeDelta = new Vector3(0f, 0f);
		UWCharacter.Instance.playerCam.rect = new Rect(0f, 0f, 1f, 1f);
		UWHUD.instance.RefreshPanels(-1);
	}

	public void UnSetFullScreen()
	{
		FullScreen = false;
		setPositions();
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW1, false);
			UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW2, true);
		}
		else
		{
			UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW1, true);
			UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW2, false);
		}
		RectTransform component = GetComponent<RectTransform>();
		component.localPosition = windowedPosition;
		component.sizeDelta = windowedSize;
		string rES2 = UWEBase._RES;
		if (rES2 != null && rES2 == "UW2")
		{
			UWCharacter.Instance.playerCam.rect = new Rect(0.05f, 0.28f, 0.655f, 0.64f);
		}
		else
		{
			UWCharacter.Instance.playerCam.rect = new Rect(0.163f, 0.335f, 0.54f, 0.572f);
		}
		UWHUD.instance.RefreshPanels(-1);
	}

	private void OnGUI()
	{
		if (GameWorldController.instance.AtMainMenu)
		{
			DrawCursor();
			return;
		}
		if (ConversationVM.InConversation)
		{
			ConversationMouseMode();
			return;
		}
		if (!WindowDetect.InMap)
		{
			if (Event.current.keyCode == KeyBindings.instance.ToggleFullScreen && !WindowDetect.WaitingForInput && Event.current.type == EventType.KeyDown)
			{
				if (FullScreen)
				{
					UnSetFullScreen();
				}
				else
				{
					SetFullScreen();
				}
			}
			if (Event.current.keyCode == KeyBindings.instance.TrackSkill && !WindowDetect.WaitingForInput && Event.current.type == EventType.KeyDown)
			{
				TryTracking();
			}
			if (Event.current.keyCode == KeyBindings.instance.ToggleMouseLook && !WindowDetect.WaitingForInput && Event.current.type == EventType.KeyDown && Character.InteractionMode != 0)
			{
				if (!UWCharacter.Instance.MouseLookEnabled)
				{
					SwitchToMouseLook();
				}
				else
				{
					SwitchFromMouseLook();
				}
			}
		}
		if (!UWCharacter.Instance.MouseLookEnabled)
		{
			return;
		}
		if (Character.InteractionMode != 4 && UWCharacter.Instance.PlayerMagic.ReadiedSpell == "")
		{
			if (!JustClicked)
			{
				if (Input.GetMouseButtonDown(0))
				{
					WindowDetect.CursorInMainWindow = true;
					OnPress(true, -1);
				}
				if (Input.GetMouseButtonDown(1))
				{
					WindowDetect.CursorInMainWindow = true;
					OnPress(true, -2);
				}
				if (Input.GetMouseButtonUp(0))
				{
					WindowDetect.CursorInMainWindow = true;
					OnPress(false, -1);
					UWWindowWait(1f);
				}
				if (Input.GetMouseButtonUp(1))
				{
					WindowDetect.CursorInMainWindow = true;
					OnPress(false, -2);
					UWWindowWait(1f);
				}
				if (Input.GetMouseButton(0))
				{
					WindowDetect.CursorInMainWindow = true;
					OnClick(-1);
					UWWindowWait(1f);
				}
				if (Input.GetMouseButton(1))
				{
					WindowDetect.CursorInMainWindow = true;
					OnClick(-2);
					UWWindowWait(1f);
				}
			}
		}
		else
		{
			WindowDetect.CursorInMainWindow = true;
			if (Input.GetMouseButtonDown(0))
			{
				WindowDetect.CursorInMainWindow = true;
				OnPress(true, -1);
			}
			if (Input.GetMouseButtonDown(1))
			{
				WindowDetect.CursorInMainWindow = true;
				OnPress(true, -2);
			}
			if (Input.GetMouseButtonUp(0))
			{
				WindowDetect.CursorInMainWindow = true;
				OnPress(false, -1);
			}
			if (Input.GetMouseButtonUp(1))
			{
				WindowDetect.CursorInMainWindow = true;
				OnPress(false, -2);
			}
		}
	}

	public void DrawCursor()
	{
	}

	public static void SwitchToMouseLook()
	{
		UWCharacter.Instance.YAxis.enabled = true;
		UWCharacter.Instance.XAxis.enabled = true;
		UWCharacter.Instance.MouseLookEnabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		UWHUD.instance.MouseLookCursor.texture = UWHUD.instance.CursorIcon;
	}

	public static void SwitchFromMouseLook()
	{
		UWCharacter.Instance.XAxis.enabled = false;
		UWCharacter.Instance.YAxis.enabled = false;
		UWCharacter.Instance.MouseLookEnabled = false;
		Cursor.lockState = CursorLockMode.None;
		if (!GameWorldController.instance.AtMainMenu)
		{
		}
		UWHUD.instance.MouseLookCursor.texture = UWHUD.instance.CursorIconBlank;
	}

	private void ConversationMouseMode()
	{
		UWCharacter.Instance.XAxis.enabled = false;
		UWCharacter.Instance.YAxis.enabled = false;
		UWCharacter.Instance.MouseLookEnabled = false;
		Cursor.lockState = CursorLockMode.None;
		CursorPosition.center = Event.current.mousePosition;
		GUI.DrawTexture(CursorPosition, UWHUD.instance.CursorIcon);
		UWHUD.instance.MouseLookCursor.texture = UWHUD.instance.CursorIconBlank;
	}

	private void TryTracking()
	{
		bool flag = UWCharacter.Instance.PlayerSkills.TrySkill(13, Skills.DiceRoll(0, 30));
		int skill = UWCharacter.Instance.PlayerSkills.GetSkill(13);
		Debug.Log("Track test = " + flag);
		Skills.TrackMonsters(base.gameObject, (float)skill / 3f, flag);
	}
}
