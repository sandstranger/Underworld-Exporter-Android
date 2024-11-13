public class SpellEffectLight : SpellEffect
{
	public override void ApplyEffect()
	{
		if (LightSource.MagicBrightness < (float)Value)
		{
			LightSource.MagicBrightness = Value;
		}
		UWCharacter.Instance.playerInventory.UpdateLightSources();
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		LightSource.MagicBrightness = 0f;
		UWCharacter.Instance.playerInventory.UpdateLightSources();
		base.CancelEffect();
	}

	public virtual void Update()
	{
		if (Active && LightSource.MagicBrightness < (float)Value)
		{
			LightSource.MagicBrightness = Value;
			UWCharacter.Instance.playerInventory.UpdateLightSources();
		}
	}
}
