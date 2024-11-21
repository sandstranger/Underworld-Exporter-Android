﻿using UnityEngine;
using System.Collections;

public class Ring : Equipment {

	public SpellEffect SpellEffectApplied;


	
	public override int GetActualSpellIndex ()
	{
		return link-512;
	}

	public override bool EquipEvent (short slotNo)
	{
		if ((slotNo ==9) || (slotNo ==10))
		{
			if (objInt().isEnchanted==true)
			{
				int EffectId=GetActualSpellIndex ();
				switch (EffectId)
				{
				case SpellEffect.UW1_Spell_Effect_MinorProtection:
				case SpellEffect.UW1_Spell_Effect_Protection:
				case SpellEffect.UW1_Spell_Effect_AdditionalProtection:
				case SpellEffect.UW1_Spell_Effect_MajorProtection:
				case SpellEffect.UW1_Spell_Effect_GreatProtection:
				case SpellEffect.UW1_Spell_Effect_VeryGreatProtection:
				case SpellEffect.UW1_Spell_Effect_TremendousProtection:
				case SpellEffect.UW1_Spell_Effect_UnsurpassedProtection:
						//ProtectionBonus=(short)(EffectId-463);
						break;
				case SpellEffect.UW1_Spell_Effect_MinorToughness:
				case SpellEffect.UW1_Spell_Effect_Toughness:
				case SpellEffect.UW1_Spell_Effect_AdditionalToughness:
				case SpellEffect.UW1_Spell_Effect_MajorToughness:
				case SpellEffect.UW1_Spell_Effect_GreatToughness:
				case SpellEffect.UW1_Spell_Effect_VeryGreatToughness:
				case SpellEffect.UW1_Spell_Effect_TremendousToughness:
				case SpellEffect.UW1_Spell_Effect_UnsurpassedToughness:
						//ToughnessBonus=(short)(EffectId-471);
						break;

				default:
					{
						//cast enchantment.
						SpellEffectApplied = UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject,null,GetActualSpellIndex(),Magic.SpellRule_TargetSelf,Magic.SpellRule_Equipable);
						if (SpellEffectApplied!=null)
						{
								SpellEffectApplied.SetPermanent(true);
						}
						break;
					}
				}


			}
		}
		return true;
	}

	public override bool UnEquipEvent (short slotNo)
	{
		if (((slotNo ==9) || (slotNo ==10)) && (item_id!=54))//Not the ring of humility
		{
		if (SpellEffectApplied!=null)
			{
				SpellEffectApplied.CancelEffect();
				return true;
			}
		}
		return false;
	}

	public override bool use ()
	{//Has no use event
		return false;
	}

	public override short Protection ()
	{
		return ProtectionBonus();
	}

	public override short getDurability ()
	{
		return DurabilityBonus();
	}

		public override bool LookAt ()
		{
				if ( (_RES==GAME_UW1) && (item_id==Quest.TalismanRing))
				{
						heading=7;
						switch(objInt().identity())
						{
						case ObjectInteraction.IdentificationFlags.Identified:
								UWHUD.instance.MessageScroll.Add (StringController.instance.GetString(1,StringController.str_you_see_) +  StringController.instance.GetString(1,269));
								break;
						default:
								UWHUD.instance.MessageScroll.Add (StringController.instance.GetFormattedObjectNameUW(objInt(),GetEquipmentConditionString()) + OwnershipString());		
								break;
						}
						return true;
				}
				else
				{
						return base.LookAt ();				
				}
		}


}
