using UnityEngine;

public class SpellProp_ResistanceAgainstType : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.protection;
		switch (effectId)
		{
		case 53:
		case 54:
		case 55:
		case 278:
		case 283:
		case 284:
		case 287:
		case 393:
		case 394:
		case 395:
			counter = 5;
			break;
		default:
			Debug.Log("Default values used in resistance spell");
			counter = 1;
			break;
		}
	}
}
