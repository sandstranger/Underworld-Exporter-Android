using UnityEngine;

public class SpellEffectFreezeTime : SpellEffect
{
	public Animator anim;

	public long Key;

	public bool isNPC;

	public override void ApplyEffect()
	{
		UWCharacter.Instance.isTimeFrozen = true;
		base.ApplyEffect();
	}

	private void Update()
	{
		UWCharacter.Instance.isTimeFrozen = true;
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isTimeFrozen = false;
		base.CancelEffect();
	}
}
