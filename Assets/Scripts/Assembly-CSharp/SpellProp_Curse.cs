using UnityEngine;

public class SpellProp_Curse : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		counter = 10;
		impactFrameStart = 40;
		impactFrameEnd = 44;
		damagetype = DamageTypes.psychic;
	}
}
