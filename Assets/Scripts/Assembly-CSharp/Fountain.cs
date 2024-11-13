public class Fountain : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (objInt().isEnchanted && base.link >= 512)
			{
				UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, base.link - 512, 1, 2);
			}
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_water_refreshes_you_));
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override string UseVerb()
	{
		return "drink";
	}
}
