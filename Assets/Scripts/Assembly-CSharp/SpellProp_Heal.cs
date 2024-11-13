using UnityEngine;

public class SpellProp_Heal : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.aid;
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			switch (effectId)
			{
			case 64:
			case 65:
			case 66:
			case 67:
				BaseDamage = (short)Random.Range(1, 10);
				break;
			case 68:
			case 69:
			case 70:
			case 71:
			case 281:
				BaseDamage = (short)Random.Range(10, 20);
				break;
			case 72:
			case 73:
			case 74:
				BaseDamage = (short)Random.Range(30, 40);
				break;
			case 76:
			case 77:
			case 78:
			case 79:
			case 300:
				BaseDamage = (short)Random.Range(50, 60);
				break;
			default:
				Debug.Log("Default values used in heal spell");
				BaseDamage = 20;
				break;
			}
		}
		else
		{
			switch (effectId)
			{
			case 64:
			case 65:
			case 66:
			case 67:
			case 264:
				BaseDamage = (short)Random.Range(1, 10);
				break;
			case 68:
			case 69:
			case 70:
			case 71:
			case 275:
				BaseDamage = (short)Random.Range(10, 20);
				break;
			case 72:
			case 73:
			case 74:
			case 75:
				BaseDamage = (short)Random.Range(30, 40);
				break;
			case 76:
			case 77:
			case 78:
			case 79:
			case 286:
				BaseDamage = (short)Random.Range(50, 60);
				break;
			default:
				Debug.Log("Default values used in heal spell");
				BaseDamage = 20;
				break;
			}
		}
	}
}
