﻿using UnityEngine;
using System.Collections;
/// <summary>
/// Pauses animations and movement for the npc
/// Effect applies to player and npc.
/// Player effect is used for controlling the stop time state.
/// </summary>
public class SpellEffectFreezeTime : SpellEffect {
	///To pause the npc animations.
	public Animator anim;
	///The state the npc was in before the spell was cast at them
	//public int state;
	///To associated the spell effect on the NPC with the spell effect on the player
	public long Key;
	public bool isNPC;
	public override void ApplyEffect ()
	{
			/*if (isNPC)
			{
				this.GetComponent<NPC>().Frozen=true;
			}
			else
				{//Player applies the effect to other npcs?
						
				}*/
				UWCharacter.Instance.isTimeFrozen=true;

		base.ApplyEffect ();
	}

	void Update()
	{
			//	if (isNPC==true)
			//	{
			//			this.GetComponent<NPC>().Frozen=true;				
			//	}
				UWCharacter.Instance.isTimeFrozen=true;
		
	}


	public override void CancelEffect ()
	{
			/*	if (isNPC==true)
				{
						this.GetComponent<NPC>().Frozen=false;
						//this.GetComponent<NPC>().CurrentAnim="";
						//this.GetComponent<NPC>().currentState=-1;
						//this.GetComponent<NPC>().state=state;
						if (anim!=null)
						{
								anim.enabled=true;
						}
				}
				else
				{
						//Find npcs with this effect and key and cancel them.
						GameObject[] npcs= GameObject.FindGameObjectsWithTag("NPCs");
						for (int i = 0; i<=npcs.GetUpperBound(0); i++)
						{
								if( npcs[i].gameObject.GetComponent<SpellEffectFreezeTime>()!=null)
								{
										if  (npcs[i].gameObject.GetComponent<SpellEffectFreezeTime>().Key==Key)
										{
												npcs[i].gameObject.GetComponent<SpellEffectFreezeTime>().CancelEffect();	
										}
								}
						}
				}*/
				UWCharacter.Instance.isTimeFrozen=false;

		base.CancelEffect ();
	}
}
