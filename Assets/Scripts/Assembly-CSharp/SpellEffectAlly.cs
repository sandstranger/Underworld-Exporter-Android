public class SpellEffectAlly : SpellEffect
{
	public short OriginalAttitude;

	public short OriginalGtarg;

	public short OriginalGoal;

	private NPC npc;

	public bool WasActive;

	public override void ApplyEffect()
	{
		if (GetComponent<SpellEffectConfusion>() == null && GetComponent<SpellEffectFear>() == null)
		{
			npc = GetComponent<NPC>();
			if (npc != null)
			{
				OriginalAttitude = npc.npc_attitude;
				OriginalGoal = npc.npc_goal;
				OriginalGtarg = npc.npc_gtarg;
				npc.npc_attitude = 2;
				npc.npc_goal = 3;
				npc.npc_gtarg = 1;
				WasActive = true;
			}
		}
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		if (WasActive)
		{
			npc.npc_attitude = OriginalAttitude;
			npc.npc_goal = OriginalGoal;
			npc.npc_gtarg = OriginalGtarg;
		}
		base.CancelEffect();
	}
}
