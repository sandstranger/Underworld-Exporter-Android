﻿using UnityEngine;
using System.Collections;

public class a_damage_trap : trap_base {
	/*
Per uw-formats.txt
	0180  a_damage trap
	player vitality is decreased; number of hit points are in "quality"
	field; if the "owner" field is != 0, the hit points are added
	instead. the trap is only set of when a random value [0..10] is >= 7.

Examples of usage
Ironwits maze on Level2 (rotworm area)

//UPDATE
It appears the above may be wrong?
owner = 0 damage trap
owner != 0 poison trap
*/

	//Question to answer. What impact does the flags value have?

	public override void ExecuteTrap (object_base src, int triggerX, int triggerY, int State)
	{
		//Debug.Log (this.name);
		if (owner ==0)
		{
			if (Random.Range(0,11) >= 7)
			{
				UWCharacter.Instance.CurVIT= UWCharacter.Instance.CurVIT- quality;
			}
		}
		else//poison version
		{
			if (!UWCharacter.Instance.isPoisonResistant())
			{
				if (UWCharacter.Instance.play_poison==0)
				{
					//UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject,null,SpellEffect.UW1_Spell_Effect_Poison,Magic.SpellRule_TargetSelf, Magic.SpellRule_Consumable);
					UWCharacter.Instance.play_poison =(short) Random.Range(1,6);
					if (UWCharacter.Instance.poison_timer==0)
					{
						UWCharacter.Instance.poison_timer=30f;	
					}
				}	
			}	
		}
	}
}
