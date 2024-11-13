using UnityEngine;

public class SpellProp_Acid : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		ProjectileItemId = 22;
		Force = 200f;
		BaseDamage = 3;
		impactFrameStart = 46;
		impactFrameEnd = 50;
		spread = 0f;
		noOfCasts = 1;
		silent = true;
		damagetype = DamageTypes.acid;
	}
}
