using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class SpellEffectNightVision : SpellEffectLight
{
	public static bool inUse;

	public Grayscale gs;

	public override void ApplyEffect()
	{
		if (!inUse)
		{
			gs = Camera.main.gameObject.AddComponent<Grayscale>();
			gs.shader = GameWorldController.instance.greyScale;
		}
		base.ApplyEffect();
	}

	public override void Update()
	{
		if (Active)
		{
			inUse = true;
		}
		base.Update();
	}

	public override void CancelEffect()
	{
		if (gs != null)
		{
			Object.Destroy(gs);
		}
		inUse = false;
		base.CancelEffect();
	}
}
