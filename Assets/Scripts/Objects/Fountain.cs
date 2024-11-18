﻿using UnityEngine;
using System.Collections;

public class Fountain : object_base {
//Code for handing fountain behaviour.

	public override bool use ()
	{
		if (CurrentObjectInHand==null)
		{
			if ((objInt().isEnchanted) &&(link>=512))
			{
				UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject,null,link-512,Magic.SpellRule_TargetSelf,Magic.SpellRule_Consumable);
			}
			UWHUD.instance.MessageScroll.Add (StringController.instance.GetString (1,StringController.str_the_water_refreshes_you_));
			return true;
		}
		else
		{
			return ActivateByObject(CurrentObjectInHand);		
		}
	}


	public override string UseVerb ()
	{
		return "drink";
	}
}
