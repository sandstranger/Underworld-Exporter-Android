public class SpellEffectResistance : SpellEffect
{
	public override void ApplyEffect()
	{
		UWCharacter.Instance.Resistance = Value;
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.Resistance = 0;
		base.CancelEffect();
	}

	private void Update()
	{
		if (Active && UWCharacter.Instance.Resistance < Value)
		{
			UWCharacter.Instance.Resistance = Value;
		}
	}
}
