using UnityEngine;

public class SpellProp_ElectricBolt : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		ProjectileItemId = 21;
		Force = 200f;
		BaseDamage = 8;
		impactFrameStart = 46;
		impactFrameEnd = 50;
		damagetype = DamageTypes.electric;
	}
}
