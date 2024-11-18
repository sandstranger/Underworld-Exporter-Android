﻿using UnityEngine;
using System.Collections;

public class SpellProp_MagicArrow : SpellProp {
	//Properties for the Ort Jux spell.

	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init (effectId,SpellCaster);
		//ProjectileSprite = UWEBase._RES +"/Sprites/Objects/Objects_023";
		ProjectileItemId=23;
		Force=200.0f;
		BaseDamage=3;
		impactFrameStart=46;
		impactFrameEnd=50;
		spread=0;
		noOfCasts=1;
	}
}
