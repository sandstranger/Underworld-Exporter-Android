public class a_hack_trap_visibility : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
		if (objectIntAt != null)
		{
			objectIntAt.setInvis(0);
		}
	}
}
