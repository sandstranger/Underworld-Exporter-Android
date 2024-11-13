public class SpellEffectWaterWalk : SpellEffect
{
	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.isWaterWalking = true;
		}
	}

	public override void ApplyEffect()
	{
		if (UWCharacter.Instance == null)
		{
			UWCharacter.Instance = GetComponent<UWCharacter>();
		}
		UWCharacter.Instance.isWaterWalking = true;
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isWaterWalking = false;
		base.CancelEffect();
	}
}
