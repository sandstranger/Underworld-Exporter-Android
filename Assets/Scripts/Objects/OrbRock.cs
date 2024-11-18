﻿using UnityEngine;
using System.Collections;

public class OrbRock : object_base {

	public static void DestroyOrb(ObjectInteraction orbToDestroy)
	{
		//Spawn an impact
		Impact.SpawnHitImpact(Impact.ImpactDamage(), orbToDestroy.GetImpactPoint(),46,50);
		Quest.instance.isOrbDestroyed=true;
		UWCharacter.Instance.PlayerMagic.MaxMana=UWCharacter.Instance.PlayerMagic.TrueMaxMana;
		UWCharacter.Instance.PlayerMagic.CurMana=UWCharacter.Instance.PlayerMagic.MaxMana;
		//000-001-133 The orb is destroyed
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString (1,133));

		orbToDestroy.consumeObject();						
	}

	void OnCollisionEnter(Collision collision)
	{//	
		if (objInt().PickedUp==false)
		{
			if ((GameWorldController.instance.LevelNo==6) && (_RES==GAME_UW1))
			{
				if(collision.gameObject.name.Contains("orb"))
				{
					ObjectInteraction HitobjInt = collision.gameObject.GetComponent<ObjectInteraction>();
					if (HitobjInt.GetItemType()==ObjectInteraction.ORB)
					{
						DestroyOrb(HitobjInt);
					}
				}
			}	
		}

	}


}
