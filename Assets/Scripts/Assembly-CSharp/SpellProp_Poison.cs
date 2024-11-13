using UnityEngine;

public class SpellProp_Poison : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.poison;
		impactFrameStart = 40;
		impactFrameEnd = 44;
		switch (effectId)
		{
		case 116:
			BaseDamage = 50;
			counter = 5;
			break;
		case 277:
			BaseDamage = 100;
			counter = 6;
			break;
		case 491:
			BaseDamage = 40;
			counter = 5;
			break;
		}
	}
}
