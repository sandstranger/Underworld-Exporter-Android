﻿using System;
using UnityEngine;
using System.Collections;
using UnderworldExporter.Game;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RuneSlot : GuiBase,IPointerDownHandler,IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
	private TimeSpan _itemPressedTime;
	private bool _isPointerInRuneSlot;
	//public static UWCharacter playerUW;
	public int SlotNumber;
	private RawImage thisRune;
	public bool isSet;
		/*
	static string[] Runes=new string[]{"An","Bet","Corp","Des",
		"Ex","Flam","Grav","Hur",
		"In","Jux","Kal","Lor",
		"Mani","Nox","Ort","Por",
		"Quas","Rel","Sanct","Tym",
		"Uus","Vas","Wis","Ylem"};*/
	// Use this for initialization
	public override void Start () {
		base.Start();
		thisRune = this.GetComponent<RawImage>();
		//thisRune.texture= Resources.Load <Texture2D> (_RES +"/HUD/Runes/rune_blank");
	}
	
	
	public static void UpdateRuneDisplay () {
		for  (int i=0; i<24; i++)
		{
			if (UWHUD.instance.runes[i].thisRune==null)
			{
					UWHUD.instance.runes[i].thisRune=UWHUD.instance.runes[i].gameObject.GetComponent<RawImage>();
			}
			if ((UWCharacter.Instance.PlayerMagic.PlayerRunes[i] != false))
			{					
				UWHUD.instance.runes[i].thisRune.texture=GameWorldController.instance.ObjectArt.LoadImageAt(232 + i);
				UWHUD.instance.runes[i].isSet=true;
			}
			else
			{
				UWHUD.instance.runes[i].thisRune.texture=Resources.Load <Texture2D> (_RES +"/HUD/Runes/rune_blank");
				UWHUD.instance.runes[i].isSet=false;	
			}
		}

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_itemPressedTime = GameWorldController.Stopwatch.Elapsed;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_isPointerInRuneSlot = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_isPointerInRuneSlot = false;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!_isPointerInRuneSlot)
		{
			return;
		}
        
		if (GameWorldController.Stopwatch.Elapsed - _itemPressedTime > InputManager.TimeForEmulateRightMouseBtnClicks)
		{
			ClickEvent(InputManager.RightMouseButtonId);
		}
		else
		{
			ClickEvent(InputManager.IsTouchActive ? InputManager.LeftMouseButtonId : eventData.GetPointerId());
		}
        
		UWCharacter.Instance.playerInventory.Refresh();
	}
	
	public void ClickEvent(int ptrID)
	{

		if (UWCharacter.Instance.PlayerMagic.PlayerRunes[SlotNumber] == false)
		{
			return;//Slot is unfilled
		}
		else
		{
			if (ptrID==InputManager.LeftMouseButtonId)
			{//left click select the rune.
				//add the rune to the first available active slot.
				//If all the slots are in use then push the stack down.
				if (UWCharacter.Instance.PlayerMagic.ActiveRunes[0]==-1)
				{
					UWCharacter.Instance.PlayerMagic.ActiveRunes[0]=SlotNumber;
				}
				else if(UWCharacter.Instance.PlayerMagic.ActiveRunes[1]==-1)
				{
				UWCharacter.Instance.PlayerMagic.ActiveRunes[1]=SlotNumber;
				}
				else if(UWCharacter.Instance.PlayerMagic.ActiveRunes[2]==-1)
				{
					UWCharacter.Instance.PlayerMagic.ActiveRunes[2]=SlotNumber;
				}
				else
				{//No free slot. Push everything down.
					UWCharacter.Instance.PlayerMagic.ActiveRunes[0]=UWCharacter.Instance.PlayerMagic.ActiveRunes[1];
					UWCharacter.Instance.PlayerMagic.ActiveRunes[1]=UWCharacter.Instance.PlayerMagic.ActiveRunes[2];
					UWCharacter.Instance.PlayerMagic.ActiveRunes[2]=SlotNumber;
				}
				ActiveRuneSlot.UpdateRuneSlots();
			}
		else
			{//right click id the rune.
				UWHUD.instance.MessageScroll.Add ("You see " + StringController.instance.GetSimpleObjectNameUW(232+SlotNumber));
			}
		}
	}
}
