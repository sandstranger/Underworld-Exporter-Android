using UnityEngine;

public class SpellProp_Homing : SpellProp
{
	public MagicProjectile projectileToTrack;

	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		ProjectileItemId = 27;
		Force = 200f;
		BaseDamage = 12;
		impactFrameStart = 43;
		impactFrameEnd = 47;
		spread = 0f;
		noOfCasts = 1;
		homing = true;
		hasTrail = true;
	}
}
