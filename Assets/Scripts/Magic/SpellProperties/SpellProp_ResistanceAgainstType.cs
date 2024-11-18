﻿using UnityEngine;
using System.Collections;

public class SpellProp_ResistanceAgainstType : SpellProp {

	public override void init (int effectId, GameObject SpellCaster)
	{
		base.init (effectId,SpellCaster);
				damagetype = DamageTypes.protection;
				switch (effectId)
				{
				case SpellEffect.UW1_Spell_Effect_MissileProtection:
				case SpellEffect.UW1_Spell_Effect_MissileProtection_alt01:
				case SpellEffect.UW1_Spell_Effect_MissileProtection_alt02:
				//case SpellEffect.UW2_Spell_Effect_MissileProtection:
				case SpellEffect.UW2_Spell_Effect_MissileProtection_alt01:						
				//case SpellEffect.UW2_Spell_Effect_MissileProtection_alt02:

				case SpellEffect.UW1_Spell_Effect_Flameproof:
				case SpellEffect.UW1_Spell_Effect_Flameproof_alt01:
				case SpellEffect.UW1_Spell_Effect_Flameproof_alt02:
				
				//case SpellEffect.UW2_Spell_Effect_Flameproof:							
				case SpellEffect.UW2_Spell_Effect_Flameproof_alt01:						
				case SpellEffect.UW2_Spell_Effect_Flameproof_alt02:
						

				case SpellEffect.UW1_Spell_Effect_PoisonResistance:
						//case SpellEffect.UW2_Spell_Effect_PoisonResistance:

						counter=5;
						break;

				default:
						Debug.Log("Default values used in resistance spell");
						counter=1;break;


				}
	}
}
