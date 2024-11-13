using UnityEngine;

public class SpellEffectParalyze : SpellEffect
{
	public bool isNPC;

	public NPC npc;

	public Animator anim;

	public override void ApplyEffect()
	{
		if (!isNPC)
		{
			UWCharacter.Instance.Paralyzed = true;
		}
		else if (npc == null)
		{
			npc = GetComponent<NPC>();
			npc.Paralyzed = true;
		}
		else
		{
			npc.Paralyzed = true;
		}
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		if (!isNPC)
		{
			UWCharacter.Instance.Paralyzed = false;
			UWCharacter.Instance.walkSpeed = 3f;
		}
		else
		{
			npc.Paralyzed = false;
		}
		base.CancelEffect();
	}

	public void Update()
	{
		if (isNPC)
		{
			if (npc != null)
			{
				npc.Paralyzed = true;
				return;
			}
			npc = GetComponent<NPC>();
			npc.Paralyzed = true;
		}
		else
		{
			UWCharacter.Instance.Paralyzed = true;
		}
	}
}
