public class a_hack_trap_change_goal : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		NPC[] componentsInChildren = GameWorldController.instance.DynamicObjectMarker().GetComponentsInChildren<NPC>();
		for (int i = 0; i < componentsInChildren.GetUpperBound(0); i++)
		{
			if (componentsInChildren[i].npc_whoami == base.zpos)
			{
				componentsInChildren[i].npc_goal = base.owner;
			}
		}
	}
}
