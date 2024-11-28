using System;
using UnityEngine;
using System.Collections;
using UnderworldExporter.Game;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowDetectUW : WindowDetect
{
    private static WindowDetectUW _instance;
    
    /// <summary>
    /// Is the game using experimental room manager code.
    /// </summary>
    public static bool UsingRoomManager = false;
    Vector3 windowedPosition;
    Vector3 windowedSize;
    [SerializeField] 
    private RawImage _rayCastImage;
    private static bool _hideScreenControls = false;

    private void Awake()
    {
        _instance = this;
    }

    public override void Start()
    {
        base.Start();
        _hideScreenControls = ScreenControlsManager.HideScreenControls;
        JustClicked = false;
        WindowWaitCount = 0;
        switch (_RES)
        {
            case GAME_UW2:
                UWCharacter.Instance.playerCam.rect = new Rect(0.05f, 0.28f, 0.655f, 0.64f);
                break;
            default:
                UWCharacter.Instance.playerCam.rect = new Rect(0.163f, 0.335f, 0.54f, 0.572f);
                break;
        }

        if (!HudAspectRatioPreserver.PreserveHudAspectRatio)
        {
            SetFullScreen();
        }
    }

    /// <summary>
    /// Cancel all click input for a few seconds.
    /// </summary>
    /// <param name="waitTime">Wait time.</param>
    public void UWWindowWait(float waitTime)
    {
        JustClicked = true;//Prevent catching something I have just thrown.
        WindowWaitCount = waitTime;
    }

    public void OnPointerDown()
    {
        if (UWCharacter.Instance.IsSpellReady)
        {
            return;
        }
        
        WindowDetect.CursorInMainWindow = true;
        MouseHeldDown = true;
        OnPress(true, -1, forcedPress:true);
        OnClick( -1, true);
    }
    
    public void OnPointerUp()
    {
        if (UWCharacter.Instance.IsSpellReady)
        {
            return;
        }
        
        OnPress(false, -1, forcedPress: true);
        MouseHeldDown = false;
    }

    /// <summary>
    /// General Combat UI interface. Controls attack charging
    /// </summary>
    public override void Update()
    {
        InteractionMode();

        //Controls switching between Mouselook and interaction and sets the cursor icon
        if (!GameWorldController.instance.AtMainMenu && ConversationVM.InConversation == true)
        {
            ConversationMouseMode();
        }

        if (!GameWorldController.instance.AtMainMenu && ConversationVM.InConversation == false)
        {
            if (WindowDetect.InMap == false)
            {
                //if ((Event.current.Equals(Event.KeyboardEvent("t"))) && (WaitingForInput==false))
                if (InputManager.OnKeyDown(KeyBindings.instance.TrackSkill) && (WaitingForInput == false))
                    //if ( ( Input.GetKey(KeyCode.T) ) && (WaitingForInput==false))
                {
                    //Tracking skill
                    TryTracking();
                }


                //if ((Event.current.Equals(Event.KeyboardEvent("e"))) && (WaitingForInput==false))
                if (InputManager.OnKeyDown(KeyBindings.instance.ToggleMouseLook) && (WaitingForInput == false))
                {
                    SwitchMouseLook();
                }
            }


            if (UWCharacter.Instance.MouseLookEnabled == false)
            {
                //DrawCursor();
                // UWHUD.instance.MessageScroll.Add(Time.time.ToString());
            }
            else
            {
                //if (UWHUD.instance.MouseLookCursor.texture.name != UWHUD.instance.CursorIcon.name)	
                //{
                // X UWHUD.instance.MouseLookCursor.texture = UWHUD.instance.CursorIcon;
                //}

                //Added due to unity bug where mouse is offscreen!!!!
                //UGH!!!
                //WHen not in combat or with a readied spell.
                if ((UWCharacter.InteractionMode != UWCharacter.InteractionModeAttack) &&
                    (UWCharacter.Instance.PlayerMagic.ReadiedSpell == ""))
                {
                    if (JustClicked == false)
                    {
                        bool mouseLookEnabled = UWCharacter.Instance.MouseLookEnabled;

                        if (mouseLookEnabled && !_hideScreenControls)
                        {
                            return;
                        }

                        if (InputManager.OnKeyDown(KeyCode.Mouse0))
                        {
                            CursorInMainWindow = true;
                            OnPress(true, InputManager.LeftMouseButtonId);
                        }

                        if (InputManager.OnKeyDown(KeyCode.Mouse1))
                        {
                            CursorInMainWindow = true;
                            OnPress(true, InputManager.RightMouseButtonId);
                        }

                        if (InputManager.OnKeyUp(KeyCode.Mouse0))
                        {
                            CursorInMainWindow = true;
                            OnPress(false, InputManager.LeftMouseButtonId);
                            UWWindowWait(1.0f);
                        }

                        if (InputManager.OnKeyUp(KeyCode.Mouse1))
                        {
                            CursorInMainWindow = true;
                            OnPress(false, InputManager.RightMouseButtonId);
                            UWWindowWait(1.0f);
                        }

                        if (InputManager.IsPressed(KeyCode.Mouse0))
                        {
                            CursorInMainWindow = true;
                            OnClick(InputManager.LeftMouseButtonId);
                            UWWindowWait(1.0f);
                        }

                        if (InputManager.IsPressed(KeyCode.Mouse1))
                        {
                            CursorInMainWindow = true;
                            OnClick(InputManager.RightMouseButtonId);
                            UWWindowWait(1.0f);
                        }
                    }
                }
                else
                {
                    //Combat mouse clicks
                    bool mouseLookEnabled = UWCharacter.Instance.MouseLookEnabled;

                    if (mouseLookEnabled && !_hideScreenControls)
                    {
                        return;
                    }

                    CursorInMainWindow = true;
                    if (InputManager.OnKeyDown(KeyCode.Mouse0))
                    {
                        CursorInMainWindow = true;
                        //OnClick(-1);
                        OnPress(true, InputManager.LeftMouseButtonId);
                    }

                    if (InputManager.OnKeyDown(KeyCode.Mouse1))
                    {
                        CursorInMainWindow = true;
                        OnPress(true, InputManager.RightMouseButtonId);
                    }

                    if (InputManager.OnKeyUp(KeyCode.Mouse0))
                    {
                        CursorInMainWindow = true;
                        OnPress(false, InputManager.LeftMouseButtonId);
                    }

                    if (InputManager.OnKeyUp(KeyCode.Mouse1))
                    {
                        CursorInMainWindow = true;
                        OnPress(false, InputManager.RightMouseButtonId);
                    }
                }
            }
        }
    }

    private void InteractionMode()
    {
        if ((UWCharacter.Instance.isRoaming == true) || (UWCharacter.Instance.CurVIT < 0))
        {
            //No inventory use while using wizard eye spell or dead.
            return;
        }
        if (JustClicked == true)
        {//Wait until the timer has gone down before allowing further clicks
            WindowWaitCount = WindowWaitCount - Time.deltaTime;
            if (WindowWaitCount <= 0)
            {
                JustClicked = false;
            }
            return;
        }

        //Choose what actions to take.
        switch (UWCharacter.InteractionMode)
        {
            case UWCharacter.InteractionModeAttack:
            {
                if (UWCharacter.Instance.PlayerMagic.ReadiedSpell != "")
                {
                    //Player has spell to fire off first
                    return;
                }
                if (UWCharacter.Instance.PlayerCombat.AttackExecuting == true)
                {
                    //No attacks can be started while executing the last one.
                    return;
                }
                if ((WindowDetectUW.CursorInMainWindow == false))
                {
                    MouseHeldDown = false;
                    UWCharacter.Instance.PlayerCombat.AttackCharging = false;
                }
                if ((MouseHeldDown == true))
                {
                    if (UWCharacter.Instance.PlayerCombat.AttackCharging == false)
                    {//Begin the attack
                        UWCharacter.Instance.PlayerCombat.CombatBegin();
                    }
                    if ((UWCharacter.Instance.PlayerCombat.AttackCharging == true) && (UWCharacter.Instance.PlayerCombat.Charge < 100))
                    {//While still charging increase the charge by the charge rate.
                        UWCharacter.Instance.PlayerCombat.CombatCharging();
                    }
                    return;
                }
                else if (UWCharacter.Instance.PlayerCombat.AttackCharging == true)
                {
                    //Player has been building an attack up and has released it.
                    UWCharacter.Instance.PlayerCombat.ReleaseAttack();
 
                    if (!_hideScreenControls)
                    {
                        CursorInMainWindow = false;
                    }
                }
                break;
            }
            default:
            {
                break;
            }
        }


        if ((ContextUIEnabled) && (InventorySlot.Hovering == false))
        {
            //if (CursorInMainWindow)
            //{
            ContextUIMode();
            //}					
        }
    }


    void ContextUIMode()
    {
        Ray ray;
        if (UWCharacter.Instance.MouseLookEnabled == true)
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(InputManager.MousePosition);
        }
        UWHUD.instance.ContextMenu.text = "";
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, UWCharacter.Instance.GetUseRange()))
        {
            if (hit.transform.gameObject.GetComponent<ObjectInteraction>() != null)
            {
                UWHUD.instance.ContextMenu.text = hit.transform.gameObject.GetComponent<ObjectInteraction>().LookDescriptionContext();
            }
        }
    }

    /// <summary>
    /// Detects if the mouse if over the window
    /// </summary>
    public void OnMouseEnter()
    {
        CursorInMainWindow = true;
    }

    /// <summary>
    /// Detects if the mouse has left the window.
    /// </summary>
    public void OnMouseExit()
    {
        CursorInMainWindow = false;
    }


    /// <summary>
    /// Raises the mouse down event.
    /// </summary>
    /// <param name="evnt">Evnt.</param>
    public void OnMouseDown(BaseEventData evnt)
    {
        PointerEventData pntr = (PointerEventData)evnt;
        OnPress(true, _hideScreenControls ? pntr.GetPointerId() : InputManager.FakeMouseButtonId);
    }

    /// <summary>
    /// Releases the mouse up event.
    /// </summary>
    /// <param name="evnt">Evnt.</param>
    public void OnMouseUp(BaseEventData evnt)
    {
        PointerEventData pntr = (PointerEventData)evnt;
        OnPress(false, _hideScreenControls ? pntr.GetPointerId() : InputManager.FakeMouseButtonId);
    }
    
    protected override void OnPress(bool isPressed, int PtrID, bool forcedPress = false)
    {
        if (UWCharacter.Instance.isRoaming == true || ( !_hideScreenControls && UWCharacter.Instance.MouseLookEnabled && !forcedPress))
        {//No inventory use while using wizard eye.
            return;
        }
        base.OnPress(isPressed, PtrID, forcedPress);
        if (CursorInMainWindow == false)
        {
            return;
        }
        if (JustClicked == true)
        {
            return;
        }
        switch (UWCharacter.InteractionMode)
        {
            case UWCharacter.InteractionModePickup:
                ClickEvent(PtrID);
                break;
            default:
                break;
        }
    }

    public void OnClick(BaseEventData evnt)
    {
        PointerEventData pntr = (PointerEventData)evnt;
        OnClick(pntr.GetPointerId());
    }


    public void OnClick(int ptrID, bool isForcedClick = false)
    {
        if (UWCharacter.Instance.isRoaming == true || (!_hideScreenControls && UWCharacter.Instance.MouseLookEnabled && !isForcedClick))
        {//No inventory use while using wizard eye.
            return;
        }
        
        if (JustClicked == true)
        {
            return;
        }
        //Cancel out of a small cutscene

        if (UWHUD.instance.CutScenesSmall.anim.SetAnimation.ToUpper() != "ANIM_BASE")
        {
            if ((UWCharacter.Instance.CurVIT > 0))
            {
                UWHUD.instance.CutScenesSmall.anim.SetAnimation = "Anim_Base";
                return;
            }
        }
        switch (UWCharacter.InteractionMode)
        {
            case UWCharacter.InteractionModePickup:
                break;
            default:
                ClickEvent(ptrID);
                break;
        }
    }

    /// <summary>
    /// Handles click events on the main window
    /// </summary>
    /// <param name="ptrID">Ptr I.</param>
    void ClickEvent(int ptrID)
    {
        if ((UWCharacter.Instance.PlayerMagic.ReadiedSpell != "") || (JustClicked == true))
        {
            //Debug.Log("player has a spell to cast");
            return;
        }

        if ((ContextUIEnabled) && (ContextUIUse) && (CurrentObjectInHand == null))
        {//If context sensitive UI is enabled and it is one of the use modes override the interaction mode.
            if ((object_base.UseAvail) && (ptrID == InputManager.LeftMouseButtonId))//Use on left click
            {
                UWCharacter.InteractionMode = UWCharacter.InteractionModeUse;
            }
            if ((object_base.PickAvail) && (ptrID == InputManager.RightMouseButtonId))//Pickup on right click
            {
                UWCharacter.InteractionMode = UWCharacter.InteractionModePickup;
            }
            if ((object_base.TalkAvail) && (ptrID == InputManager.LeftMouseButtonId))//Talk on left click
            {
                UWCharacter.InteractionMode = UWCharacter.InteractionModeTalk;
            }
        }
        
        InteractionModeControl.UpdateNow = true;
        switch (UWCharacter.InteractionMode)
        {
            case UWCharacter.InteractionModeOptions://Options mode
                return;//do nothing
            case UWCharacter.InteractionModeTalk://Talk
                UWCharacter.Instance.TalkMode();
                break;
            case UWCharacter.InteractionModePickup://Pickup
                if (CurrentObjectInHand != null)
                {
                    UWWindowWait(1.0f);
                    ThrowObjectInHand();
                }
                else
                {
                    UWCharacter.Instance.PickupMode(ptrID);
                }
                break;
            case UWCharacter.InteractionModeLook://look
                UWCharacter.Instance.LookMode();//do nothing
                break;
            case UWCharacter.InteractionModeAttack: //attack
                break;
            case UWCharacter.InteractionModeUse://Use
                if (CurrentObjectInHand != null)
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

    /// <summary>
    /// Throws the object in hand along a vector in the 3d view.
    /// </summary>
    protected override void ThrowObjectInHand()
    {
        base.ThrowObjectInHand();
        if (CurrentObjectInHand != null)
        {//The player is holding something
            if (UWCharacter.Instance.playerInventory.JustPickedup == false)//To prevent the click event dropping an object immediately after pickup
            {
                //Determine what is directly in front of the player via a raycast
                //If something is in the way then cancel the drop
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Ray ray;
                Vector2 mousePos = Vector2.zero;
                if (UWCharacter.Instance.MouseLookEnabled == true)
                {
                    ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                }
                else
                {
                    mousePos = InputManager.MousePosition;
                    ray = Camera.main.ScreenPointToRay(mousePos);
                }
                
                RaycastHit hit = new RaycastHit();
                float dropRange = 0.5f;
                if (!Physics.Raycast(ray, out hit, dropRange))
                {//No object interferes with the drop
                 //Calculate the force based on how high the mouse is
                    float force = mousePos.y / Camera.main.pixelHeight * 200;
                    //float force = Camera.main.ViewportToWorldPoint(Input.mousePosition).y/Camera.main.pixelHeight *200;


                    //Get the object being dropped and moved towards the end of the ray

                    GameObject droppedItem = CurrentObjectInHand.gameObject; //GameObject.Find(CurrentObjectInHand);


                    //FIELD PICKUP droppedItem.GetComponent<ObjectInteraction>().PickedUp = false; //Back in the real world
                    droppedItem.GetComponent<ObjectInteraction>().Drop();
                    droppedItem.GetComponent<ObjectInteraction>().UpdateAnimation();
                    GameWorldController.MoveToWorld(droppedItem);
                    droppedItem.transform.parent = GameWorldController.instance.DynamicObjectMarker();

                    if (droppedItem.GetComponent<Container>() != null)
                    {//Set the picked up flag recursively for container items.
                        Container.SetPickedUpFlag(droppedItem.GetComponent<Container>(), false);
                        Container.SetItemsParent(droppedItem.GetComponent<Container>(), GameWorldController.instance.DynamicObjectMarker());
                        Container.SetItemsPosition(droppedItem.GetComponent<Container>(), UWCharacter.Instance.playerInventory.InventoryMarker.transform.position);
                    }
                    droppedItem.transform.position = ray.GetPoint(dropRange - 0.1f);//UWCharacter.Instance.transform.position;

                    UnFreezeMovement(droppedItem);
                    if (Camera.main.ScreenToViewportPoint(InputManager.MousePosition).y > 0.4f)
                    {//throw if above a certain point in the view port.
                        Vector3 ThrowDir = ray.GetPoint(dropRange) - ray.origin;
                        //Apply the force along the direction.
                        if (droppedItem.GetComponent<Rigidbody>() != null)
                        {
                            droppedItem.GetComponent<Rigidbody>().AddForce(ThrowDir * force);
                        }
                    }

                    //Clear the object and reset the cursor
                    //UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
                    //UWCharacter.Instance.playerInventory.SetObjectInHand("");
                    CurrentObjectInHand = null;
                }

            }
            else
            {
                UWCharacter.Instance.playerInventory.JustPickedup = false;//The next click event will allow dropping.
            }
        }
    }


    /// <summary>
    /// Sets the full screen mode.
    /// </summary>
    public void SetFullScreen()
    {
        FullScreen = true;
        setPositions();

        UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW1, false);
        UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW2, false);
        RectTransform pos = this.GetComponent<RectTransform>();
        windowedPosition = pos.localPosition;
        windowedSize = pos.sizeDelta;
        pos.localPosition = new Vector3(0f, 0f, 0f);
        pos.sizeDelta = new Vector3(0f, 0f);
        UWCharacter.Instance.playerCam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        UWHUD.instance.RefreshPanels(-1);//refresh controls
    }

    /// <summary>
    /// Unsets full screen mode.
    /// </summary>
    public void UnSetFullScreen()
    {
        FullScreen = false;
        setPositions();
        switch (_RES)
        {
            case GAME_UW2:
                UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW1, false);
                UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW2, true);
                break;
            default:
                UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW1, true);
                UWHUD.instance.EnableDisableControl(UWHUD.instance.main_windowUW2, false);
                break;
        }

        RectTransform pos = this.GetComponent<RectTransform>();
        //pos.localPosition = new Vector3(-22f,25f,0f);
        pos.localPosition = windowedPosition;
        //pos.sizeDelta = new Vector3(-148f, -88f);
        pos.sizeDelta = windowedSize;
        switch (_RES)
        {
            case GAME_UW2:
                UWCharacter.Instance.playerCam.rect = new Rect(0.05f, 0.28f, 0.655f, 0.64f);
                break;
            default:
                UWCharacter.Instance.playerCam.rect = new Rect(0.163f, 0.335f, 0.54f, 0.572f);
                break;
        }

        UWHUD.instance.RefreshPanels(-1);//refresh controls
    }


    public void SwitchMouseLook()
    {
        if ( GameWorldController.instance.AtMainMenu || ConversationVM.InConversation == true || WindowDetect.InMap == true)
        {
            return;
        }
        
        if (UWCharacter.InteractionMode != UWCharacter.InteractionModeOptions && WaitingForInput == false)
        {
            if (UWCharacter.Instance.MouseLookEnabled == false)
            {//Switch to mouse look.
                SwitchToMouseLook();
            }
            else
            {
                SwitchFromMouseLook();
            }
        }
    }
    
    public static void SwitchToMouseLook()
    {
        UWCharacter.Instance.YAxis.enabled = true;
        UWCharacter.Instance.XAxis.enabled = true;
        UWCharacter.Instance.MouseLookEnabled = true;
        
        if (_hideScreenControls)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            if (!CanvasSortOrderChanger.ChangeSortingOrder)
            {
                _instance._rayCastImage.raycastTarget = false;
            }
            
            UWCharacter.Instance.XAxis.UseTouchCamera = true;
            UWCharacter.Instance.YAxis.UseTouchCamera = true;
        }
        Cursor.visible = false;        
        UWHUD.instance.MouseLookCursor.texture = UWHUD.instance.CursorIcon;
    }

    public static void SwitchFromMouseLook()
    {
        UWCharacter.Instance.XAxis.enabled = false;
        UWCharacter.Instance.YAxis.enabled = false;
        UWCharacter.Instance.MouseLookEnabled = false;
        Cursor.lockState = CursorLockMode.None;

        if (!_hideScreenControls && !CanvasSortOrderChanger.ChangeSortingOrder)
        {
            _instance._rayCastImage.raycastTarget = true;
        }
        
        if (!GameWorldController.instance.AtMainMenu)
        {
            //Cursor.visible = true;            
        }
        UWHUD.instance.MouseLookCursor.texture = UWHUD.instance.CursorIconBlank;
        //UWHUD.instance.MouseLookCursor.texture=UWHUD.instance.CursorIcon;
    }

    void ConversationMouseMode()
    {
        UWCharacter.Instance.XAxis.enabled = false;
        UWCharacter.Instance.YAxis.enabled = false;
        UWCharacter.Instance.MouseLookEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        UWHUD.instance.MouseLookCursor.texture = UWHUD.instance.CursorIconBlank;
    }

    /// <summary>
    /// Tries the tracking skill to detect nearby monsters
    /// </summary>
    public void TryTracking()
    {
        if ( GameWorldController.instance.AtMainMenu || ConversationVM.InConversation == true || WindowDetect.InMap == true)
        {
            return;
        }

        if (WaitingForInput == false)
        {
            bool SkillSucess = UWCharacter.Instance.PlayerSkills.TrySkill(Skills.SkillTrack, Skills.DiceRoll(0, 30));
            int skillLevel = UWCharacter.Instance.PlayerSkills.GetSkill(Skills.SkillTrack);
            Debug.Log("Track test = " + SkillSucess);
            Skills.TrackMonsters(this.gameObject, (float)skillLevel / 3, SkillSucess);
        }
    }
}