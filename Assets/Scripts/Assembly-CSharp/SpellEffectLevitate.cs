public class SpellEffectLevitate : SpellEffect
{
	public float flySpeed = 1f;

	public override void ApplyEffect()
	{
		if (UWCharacter.Instance == null)
		{
			UWCharacter.Instance = GetComponent<UWCharacter>();
		}
		UWCharacter.Instance.isFlying = true;
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isFlying = false;
		base.CancelEffect();
		if (!Permanent)
		{
			UWCharacter.Instance.isFloating = true;
			if (UWEBase._RES == "UW2")
			{
				UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, 18, 1, 2);
			}
			else
			{
				UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, 18, 1, 2);
			}
		}
		UWCharacter.Instance.flySpeed = 0f;
	}

	public virtual void Update()
	{
		if (Active)
		{
			if (UWCharacter.Instance.flySpeed <= flySpeed)
			{
				UWCharacter.Instance.flySpeed = flySpeed;
			}
			UWCharacter.Instance.isFlying = true;
		}
	}
}
