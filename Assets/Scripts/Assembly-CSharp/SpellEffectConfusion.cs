public class SpellEffectConfusion : SpellEffect
{
	public short OriginalAttitude;

	public short OriginalGtarg;

	public short OriginalGoal;

	private NPC npc;

	public bool WasActive;

	public override void ApplyEffect()
	{
		if (GetComponent<SpellEffectAlly>() == null && GetComponent<SpellEffectFear>() == null)
		{
			npc = GetComponent<NPC>();
			if (npc != null)
			{
				OriginalAttitude = npc.npc_attitude;
				OriginalGoal = npc.npc_goal;
				OriginalGtarg = npc.npc_gtarg;
				npc.npc_attitude = 1;
				npc.npc_goal = 8;
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
