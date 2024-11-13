using UnityEngine;

public class SpellProp_FlameWind : SpellProp_Fireball
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		ProjectileItemId = 20;
		Force = 500f;
		BaseDamage = 16;
		splashDamage = 8;
		splashDistance = 1f;
		impactFrameStart = 21;
		impactFrameEnd = 25;
		spread = 5f;
		noOfCasts = Random.Range(2, 5);
		SecondaryStartFrame = 31;
		SecondaryEndFrame = 35;
		damagetype = DamageTypes.fire;
	}
}
