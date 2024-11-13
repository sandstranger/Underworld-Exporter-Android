using UnityEngine;

public class SpellProp_Movement : SpellProp
{
	public float Speed;

	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		damagetype = DamageTypes.mobility;
		if (UWClass._RES == "UW2")
		{
			Debug.Log("Unimplemented spell prop for Uw2");
		}
		switch (effectId)
		{
		case 17:
			counter = 4;
			break;
		case 19:
		case 276:
		case 386:
			counter = 3;
			Speed = 1f;
			break;
		case 21:
		case 292:
		case 388:
			counter = 4;
			Speed = 2f;
			break;
		case 18:
		case 263:
		case 385:
			counter = 2;
			break;
		case 20:
		case 274:
		case 387:
			counter = 4;
			break;
		case 176:
		case 397:
			Speed = 1.2f;
			counter = 6;
			break;
		default:
			Debug.Log("Default values used in speed spell");
			Speed = 1.2f;
			counter = 6;
			break;
		}
	}
}
