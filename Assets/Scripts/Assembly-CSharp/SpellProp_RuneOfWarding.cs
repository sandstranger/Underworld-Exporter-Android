using UnityEngine;

public class SpellProp_RuneOfWarding : SpellProp
{
	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		BaseDamage = 15;
	}

	public override void onImpact(Transform tf)
	{
		base.onImpact(tf);
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_your_rune_of_warding_has_been_set_off_) + Compass.getCompassHeading(SpellProp.playerUW.gameObject, tf.gameObject));
	}
}
