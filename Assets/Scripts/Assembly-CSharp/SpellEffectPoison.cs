public class SpellEffectPoison : SpellEffect
{
	public short DOT;

	public bool isNPC;

	private NPC npc;

	public override void ApplyEffect()
	{
		if (isNPC)
		{
			if (npc == null)
			{
				npc = GetComponent<NPC>();
			}
			npc.Poisoned = true;
			DOT = (short)(Value / counter);
			DOT = Value;
		}
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		if (isNPC)
		{
			npc.Poisoned = false;
		}
		base.CancelEffect();
	}

	public override void EffectOverTime()
	{
		if (isNPC && npc.Poisoned)
		{
			npc.npc_hp -= DOT;
		}
		base.EffectOverTime();
	}

	private void Update()
	{
		if (isNPC && !npc.Poisoned && Active)
		{
			CancelEffect();
		}
	}
}
