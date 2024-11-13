public class a_hack_trap_unlink : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
		if (objectIntAt != null)
		{
			objectIntAt.link = 0;
		}
	}

	public override bool Activate(object_base src, int triggerX, int triggerY, int State)
	{
		ExecuteTrap(this, triggerX, triggerY, State);
		PostActivate(src);
		return true;
	}
}
