public class LockPick : object_base
{
	public override bool use()
	{
		if (objInt().PickedUp)
		{
			if (UWEBase.CurrentObjectInHand == null)
			{
				BecomeObjectInHand();
				UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, 8));
				return true;
			}
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		objInt().FailMessage();
		return false;
	}
}
