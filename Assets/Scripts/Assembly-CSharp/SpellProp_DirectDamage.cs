using UnityEngine;

public class SpellProp_DirectDamage : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		if (effectId == 114 || effectId == 281)
		{
			BaseDamage = 100;
		}
	}
}
