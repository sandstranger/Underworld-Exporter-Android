using UnityEngine;

public class Leech : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, 182, 1, 2);
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_leeches_remove_the_poison_as_well_as_some_of_your_skin_and_blood_));
			UWCharacter.Instance.ApplyDamage(Random.Range(1, 6));
			objInt().consumeObject();
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}
}
