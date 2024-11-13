public class SpellEffectSlowFall : SpellEffect
{
	public override void ApplyEffect()
	{
		base.ApplyEffect();
		UWCharacter.Instance.isFloating = true;
	}

	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.isFloating = true;
		}
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isFloating = false;
		base.CancelEffect();
	}
}
