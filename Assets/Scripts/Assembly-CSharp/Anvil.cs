public class Anvil : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			BecomeObjectInHand();
			UWHUD.instance.MessageScroll.Set("Use Anvil on what?");
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override string UseVerb()
	{
		return "repair";
	}
}
