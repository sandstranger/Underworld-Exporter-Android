public class SpellEffectTelekinesis : SpellEffect
{
	public override void ApplyEffect()
	{
		base.ApplyEffect();
		UWCharacter.Instance.isTelekinetic = true;
	}

	public override void CancelEffect()
	{
		base.CancelEffect();
		UWCharacter.Instance.isTelekinetic = false;
	}

	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.isTelekinetic = true;
		}
	}
}
