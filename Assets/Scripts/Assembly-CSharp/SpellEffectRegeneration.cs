public class SpellEffectRegeneration : SpellEffect
{
	public int DOT;

	public override void ApplyEffect()
	{
		DOT = Value / counter;
		base.ApplyEffect();
	}
}
