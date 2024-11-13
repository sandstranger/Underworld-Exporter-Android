using UnityEngine;

public class SpellProp_Mind : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.psychic;
		switch (effectId)
		{
		case 117:
		case 289:
			counter = 4;
			break;
		case 115:
		case 293:
			counter = 5;
			break;
		case 99:
		case 296:
			counter = 3;
			break;
		case 113:
		case 266:
			counter = 4;
			break;
		case 184:
		case 291:
		case 398:
			counter = 2;
			break;
		case 212:
			counter = 4;
			break;
		case 213:
			counter = 4;
			break;
		case 183:
		case 300:
		case 396:
			counter = 2;
			break;
		case 187:
		case 302:
		case 395:
			counter = 4;
			break;
		case 177:
		case 265:
			BaseDamage = 5;
			break;
		default:
			Debug.Log("Default values used in mind spell");
			BaseDamage = 5;
			break;
		}
		impactFrameStart = 40;
		impactFrameEnd = 44;
	}
}
