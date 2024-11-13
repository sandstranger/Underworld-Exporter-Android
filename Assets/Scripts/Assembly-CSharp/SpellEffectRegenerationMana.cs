public class SpellEffectRegenerationMana : SpellEffectRegeneration
{
	public override void EffectOverTime()
	{
		base.EffectOverTime();
		UWCharacter.Instance.PlayerMagic.CurMana = UWCharacter.Instance.PlayerMagic.CurMana + DOT;
		if (UWCharacter.Instance.PlayerMagic.CurMana >= UWCharacter.Instance.PlayerMagic.MaxMana)
		{
			UWCharacter.Instance.PlayerMagic.CurMana = UWCharacter.Instance.PlayerMagic.MaxMana;
		}
	}
}
