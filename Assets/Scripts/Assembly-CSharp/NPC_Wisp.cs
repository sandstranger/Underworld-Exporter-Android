using UnityEngine;

public class NPC_Wisp : NPC
{
	protected override void Start()
	{
		base.npc_whoami = 48;
	}

	public override void Update()
	{
	}

	public override bool ApplyAttack(short damage, GameObject source)
	{
		return true;
	}

	public override bool ApplyAttack(short damage)
	{
		return true;
	}
}
