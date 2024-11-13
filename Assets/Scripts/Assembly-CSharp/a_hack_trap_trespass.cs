public class a_hack_trap_trespass : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (((uint)base.owner & 0x1Fu) != 0)
		{
			object_base.SignalTheft(UWCharacter.Instance.transform.position, base.owner, 7f);
		}
	}
}
