using UnityEngine;

public class SpellProp_Regen : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		damagetype = DamageTypes.aid;
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			BaseDamage = 20;
			counter = 3;
		}
		else
		{
			BaseDamage = 20;
			counter = 3;
		}
	}
}
