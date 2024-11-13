using UnityEngine;

public class SpellProp_Resistance : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.protection;
		switch (effectId)
		{
		case 34:
		case 257:
		case 399:
			BaseDamage = 1;
			counter = 3;
			break;
		case 35:
		case 273:
		case 400:
			BaseDamage = 2;
			counter = 4;
			break;
		case 37:
		case 298:
		case 402:
			BaseDamage = 3;
			counter = 5;
			break;
		default:
			Debug.Log("Default values used in resistance spell");
			BaseDamage = 1;
			counter = 3;
			break;
		}
	}
}
