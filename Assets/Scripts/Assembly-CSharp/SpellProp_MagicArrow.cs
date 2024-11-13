using UnityEngine;

public class SpellProp_MagicArrow : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		ProjectileItemId = 23;
		Force = 200f;
		BaseDamage = 3;
		impactFrameStart = 46;
		impactFrameEnd = 50;
		spread = 0f;
		noOfCasts = 1;
	}
}
