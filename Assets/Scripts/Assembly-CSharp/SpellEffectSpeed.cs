public class SpellEffectSpeed : SpellEffect
{
	public float speedMultiplier;

	public override void ApplyEffect()
	{
		UWCharacter.Instance.speedMultiplier = speedMultiplier;
		UWCharacter.Instance.isSpeeding = true;
		base.ApplyEffect();
	}

	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.speedMultiplier = speedMultiplier;
			UWCharacter.Instance.isSpeeding = true;
		}
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.speedMultiplier = 1f;
		UWCharacter.Instance.isSpeeding = false;
		base.CancelEffect();
	}
}
