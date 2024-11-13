using UnityEngine;

public class SpellProp_SheetLightning : SpellProp_Fireball
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.electric;
		ProjectileItemId = 21;
		Force = 500f;
		BaseDamage = 8;
		splashDamage = 3;
		splashDistance = 1f;
		impactFrameStart = 50;
		impactFrameEnd = 53;
		spread = 4f;
		noOfCasts = Random.Range(1, 3);
		SecondaryStartFrame = 50;
		SecondaryEndFrame = 53;
	}
}
