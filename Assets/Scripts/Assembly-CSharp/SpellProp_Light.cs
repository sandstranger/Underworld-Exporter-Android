using UnityEngine;

public class SpellProp_Light : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.light;
		counter = 5;
		switch (effectId)
		{
		case 0:
			BaseDamage = 1;
			break;
		case 1:
			BaseDamage = 2;
			break;
		case 2:
			BaseDamage = 3;
			break;
		case 3:
			BaseDamage = 4;
			break;
		case 4:
			BaseDamage = 5;
			break;
		case 6:
			BaseDamage = 6;
			break;
		case 7:
			BaseDamage = 7;
			break;
		case 256:
			BaseDamage = 4;
			break;
		case 290:
			BaseDamage = 7;
			break;
		case 401:
			BaseDamage = 4;
			break;
		case 404:
			BaseDamage = 7;
			break;
		case 5:
		case 270:
		case 403:
			BaseDamage = 12;
			break;
		default:
			Debug.Log("Default values used in light spell");
			BaseDamage = 12;
			break;
		}
	}
}
