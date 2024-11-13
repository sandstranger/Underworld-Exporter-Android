using UnityEngine;

public class SpellEffectPetrified : SpellEffect
{
	private NPC npc;

	public override void ApplyEffect()
	{
		base.ApplyEffect();
		npc = GetComponent<NPC>();
		TickTime = 1;
		npc.npc_goal = 15;
		npc.npc_gtarg = counter;
		npc.newAnim.FreezeAnimFrame = true;
		npc.newAnim.output.color = Color.gray;
	}

	public override void EffectOverTime()
	{
		base.EffectOverTime();
		npc.npc_gtarg = counter;
	}

	public override void CancelEffect()
	{
		npc.npc_goal = 5;
		npc.npc_gtarg = 0;
		npc.newAnim.FreezeAnimFrame = false;
		npc.newAnim.output.color = Color.white;
		base.CancelEffect();
	}
}
