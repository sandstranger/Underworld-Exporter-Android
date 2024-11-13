public class SpellEffectLucky : SpellEffect
{
	public override void ApplyEffect()
	{
		if (UWCharacter.Instance == null)
		{
			UWCharacter.Instance = GetComponent<UWCharacter>();
		}
		UWCharacter.Instance.isLucky = true;
		base.ApplyEffect();
	}

	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.isLucky = true;
		}
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isLucky = false;
		base.CancelEffect();
	}
}
