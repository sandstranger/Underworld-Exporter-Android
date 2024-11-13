using UnityEngine;

public class SpellProp_Stealth : SpellProp
{
	public int StealthLevel;

	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.aid;
		switch (effectId)
		{
		case 50:
		case 260:
		case 390:
			StealthLevel = 5;
			counter = 2;
			break;
		case 51:
		case 269:
		case 391:
			StealthLevel = 15;
			counter = 3;
			break;
		case 52:
		case 295:
		case 392:
			StealthLevel = 30;
			counter = 5;
			break;
		default:
			Debug.Log("Default values used in stealth spell");
			StealthLevel = 30;
			counter = 5;
			break;
		}
	}
}
