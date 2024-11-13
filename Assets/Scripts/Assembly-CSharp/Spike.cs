public class Spike : object_base
{
	public override bool use()
	{
		if (objInt().PickedUp)
		{
			if (UWEBase.CurrentObjectInHand == null)
			{
				BecomeObjectInHand();
				UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, 130));
				return true;
			}
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		objInt().FailMessage();
		return false;
	}
}
