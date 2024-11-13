public class DoorKey : object_base
{
	public int KeyId;

	protected override void Start()
	{
		KeyId = base.owner;
	}

	public override bool use()
	{
		if (objInt().PickedUp)
		{
			if (UWEBase.CurrentObjectInHand == null)
			{
				BecomeObjectInHand();
				UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, 7));
				return true;
			}
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		objInt().FailMessage();
		return false;
	}

	public override bool LookAt()
	{
		if (objInt().PickedUp)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(5, base.owner + 100));
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
		}
		return true;
	}
}
