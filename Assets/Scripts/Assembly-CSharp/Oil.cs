public class Oil : object_base
{
	public override bool use()
	{
		if (objInt().PickedUp)
		{
			if (UWEBase.CurrentObjectInHand == null)
			{
				BecomeObjectInHand();
				UWHUD.instance.MessageScroll.Set("Use oil on?");
				return true;
			}
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		objInt().FailMessage();
		return false;
	}

	public override bool FailMessage()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_cannot_use_oil_on_that_));
		return false;
	}
}
