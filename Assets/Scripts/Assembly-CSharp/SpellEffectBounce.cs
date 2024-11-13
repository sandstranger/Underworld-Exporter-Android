public class SpellEffectBounce : SpellEffect
{
	public override void ApplyEffect()
	{
		if (UWCharacter.Instance == null)
		{
			UWCharacter.Instance = GetComponent<UWCharacter>();
		}
		UWCharacter.Instance.isBouncy = true;
		base.ApplyEffect();
	}

	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.isBouncy = true;
		}
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isBouncy = false;
		base.CancelEffect();
	}
}
