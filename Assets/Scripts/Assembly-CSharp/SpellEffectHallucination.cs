using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class SpellEffectHallucination : SpellEffect
{
	private Grayscale gs;

	public override void ApplyEffect()
	{
		gs = Camera.main.gameObject.AddComponent<Grayscale>();
		gs.shader = GameWorldController.instance.greyScale;
		gs.textureRamp = GameWorldController.instance.palLoader.PaletteToImage(Random.Range(0, 7));
		base.ApplyEffect();
	}

	public override void EffectOverTime()
	{
		gs.textureRamp = GameWorldController.instance.palLoader.PaletteToImage(Random.Range(0, 7));
		base.EffectOverTime();
	}

	public override void CancelEffect()
	{
		if (gs != null)
		{
			Object.Destroy(gs);
		}
		base.CancelEffect();
	}
}
