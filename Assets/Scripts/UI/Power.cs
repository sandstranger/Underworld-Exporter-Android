﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Power Gem UI Element
/// 
/// Updates the glowing power gem in the UW HUD to reflect the charge level of the current attack.
/// </summary>
public class Power : GuiBase {

	public float PreviousCharge=-1.0f;	/// The player Combat charge level recorded in the previous frame.
	public int RepeatCounter=0; 	/// Controls the repetition of the max charge.
	public int PreviousIndex=-1;	/// Tracks the previous level of the charge.
	public RawImage uiPowerGem;		/// The UI texture to display
	private Texture2D[] PowerGemArt=new Texture2D[14];

	public override void Start()
	{			
		base.Start();
		GRLoader powerArt = new GRLoader(GRLoader.POWER_GR);
		if (uiPowerGem==null)
		{
			uiPowerGem= this.gameObject.GetComponent<RawImage>();
		}
		PowerGemArt = new Texture2D[powerArt.NoOfFileImages()];
		for (int i=0;i<=PowerGemArt.GetUpperBound(0);i++)
		{
			PowerGemArt[i]=powerArt.LoadImageAt(i);	
		}
		if (_RES==GAME_UW2)
		{
			uiPowerGem.GetComponent<GuiBase_Draggable>().rectT = new RectTransform[2];
			uiPowerGem.GetComponent<GuiBase_Draggable>().rectT[0] = this.GetComponent<RectTransform>();
			uiPowerGem.GetComponent<GuiBase_Draggable>().rectT[1] = UWHUD.instance.HudCompass.GetComponent<RectTransform>();
		}
	}

	/// <summary>
	/// Compares the previous charge with the current attack charge. If the charge level has changed it changes the power gem image.
	/// Once it reaches full charge it will loop the max charge animation.
	/// </summary>
	public override void Update ()
	{
		base.Update();
		if ((PreviousCharge!=UWCharacter.Instance.PlayerCombat.Charge)||(UWCharacter.Instance.PlayerCombat.AttackCharging==true))
		{
			PreviousCharge=UWCharacter.Instance.PlayerCombat.Charge;
			 
			int index= (int)UWCharacter.Instance.PlayerCombat.Charge/10;
	
			if (index==10)
			{
				if (!IsInvoking("UpdateMaxCharge"))
				{
					InvokeRepeating("UpdateMaxCharge",0.0f,0.1f);
				}
			}
			else
			{
				if (index!=PreviousIndex)
				{
					RepeatCounter=0;
					uiPowerGem.texture=PowerGemArt[index];
				}
			}
		}
	else
		{
			if (IsInvoking("UpdateMaxCharge"))
			{
				CancelInvoke("UpdateMaxCharge");
				//uiPowerGem.texture=Resources.Load <Texture2D> (_RES +"/HUD/Power/Power_"+ 0.ToString("0000"));
				uiPowerGem.texture=PowerGemArt[0];
			}
			RepeatCounter=0;
		}
	}

	/// <summary>
	/// Loops the max charge image.
	/// </summary>
	public void UpdateMaxCharge()
	{
		if (_RES!=GAME_UW2)
		{
				uiPowerGem.texture=PowerGemArt[10+RepeatCounter];		
		}
		else
		{
				uiPowerGem.texture=PowerGemArt[10];
		}

		RepeatCounter++;
		if (RepeatCounter>3)
		{
			RepeatCounter=0;
		}
	}
}