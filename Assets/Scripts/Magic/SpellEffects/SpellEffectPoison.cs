﻿using UnityEngine;
using System.Collections;
/// <summary>
/// Poison effect
/// </summary>
public class SpellEffectPoison : SpellEffect {

	///The amount of damage per counter tick.
	public short DOT; 

	///The effect is applied to an npc
	public bool isNPC;
	
	///The NPC the spell is applied to.
	private NPC npc;	

	public override void ApplyEffect ()
	{
		if (isNPC==false)
		{//Apply to player
           // UWCharacter.Instance.play_poison += Value;
            //if (UWCharacter.Instance==null)
            //{
            //	UWCharacter.Instance= this.GetComponent<UWCharacter>();
            //}
            //UWCharacter.Instance.Poisoned=true;
            //if (Value!=0)
            //{
            //	UWCharacter.Instance.play_poison += Value;		
            //	UWCharacter.Instance.play_poison=(short)(Mathf.Min(15, UWCharacter.Instance.play_poison));//Is this the value the poison should apply?
            //}


        }
		else
		{
			if (npc==null)
			{
				npc=this.GetComponent<NPC>();
			}
			npc.Poisoned=true;
			DOT=(short)(Value/counter);//How many hp per tick gets knocked off the players/npcs health.
			DOT= Value;
		}
		base.ApplyEffect();
	}

	public override void CancelEffect ()
	{
		if (isNPC==false)
		{
			//UWCharacter.Instance.Poisoned=false;
			//UWCharacter.Instance.play_poison=0;
		}
		else
		{
			npc.Poisoned=false;
		}
		base.CancelEffect();
	}

	public override void EffectOverTime ()
	{
		if (isNPC==false)
		{
			//if (UWCharacter.Instance.play_poison!=0)
			//{
			//	UWCharacter.Instance.CurVIT=UWCharacter.Instance.CurVIT-DOT;
			//}
		}
		else
		{
			if (npc.Poisoned==true)
			{
				npc.npc_hp=(short)(npc.npc_hp-DOT);
			}
		}

		base.EffectOverTime ();
	}

	void Update()
	{
		if (isNPC==true)
		{
			if ((npc.Poisoned==false) && (Active==true))
			{
				CancelEffect();
			}
		}
		//else
		//{
			//DOT = (short)(UWCharacter.Instance.play_poison * 5);
			//if ((UWCharacter.Instance.play_poison==0) && (Active==true))
			//{
			//	CancelEffect();
			//}
		//}
	}
}
