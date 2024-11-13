using UnityEngine;

public class SpellProp_Mana : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.aid;
		switch (effectId)
		{
		case 164:
		case 165:
		case 166:
		case 167:
		case 307:
			BaseDamage = (short)Random.Range(5, 10);
			break;
		case 308:
			BaseDamage = (short)Random.Range(20, 50);
			break;
		case 160:
		case 161:
		case 162:
		case 163:
			BaseDamage = (short)Random.Range(30, 50);
			break;
		case 168:
		case 169:
		case 170:
		case 171:
			BaseDamage = (short)Random.Range(30, 50);
			break;
		case 172:
		case 173:
		case 174:
		case 175:
			BaseDamage = (short)Random.Range(3, 70);
			break;
		default:
			Debug.Log("Default values used in mana spell");
			BaseDamage = 20;
			break;
		}
	}
}
