public class SpellEffectLeap : SpellEffect
{
	public override void ApplyEffect()
	{
		if (UWCharacter.Instance == null)
		{
			UWCharacter.Instance = GetComponent<UWCharacter>();
		}
		UWCharacter.Instance.isLeaping = true;
		base.ApplyEffect();
	}

	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.isLeaping = true;
		}
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isLeaping = false;
		base.CancelEffect();
	}
}
