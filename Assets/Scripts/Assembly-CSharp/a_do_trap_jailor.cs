public class a_do_trap_jailor : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(251);
		if (objectIntAt != null && objectIntAt.GetComponent<NPC>() != null && objectIntAt.GetComponent<NPC>().npc_whoami == 216)
		{
			objectIntAt.TalkTo();
		}
	}
}
