public class Pole : object_base
{
	public override bool use()
	{
		if (objInt().PickedUp)
		{
			if (UWEBase.CurrentObjectInHand == null)
			{
				BecomeObjectInHand();
				return true;
			}
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		objInt().FailMessage();
		return false;
	}

	public override bool FailMessage()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_pole_cannot_be_used_on_that_));
		return false;
	}
}
