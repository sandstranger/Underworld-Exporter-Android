using UnityEngine;

public class SpellProp_Fireball : SpellProp
{
	protected short splashDamage;

	protected float splashDistance;

	protected int SecondaryStartFrame;

	protected int SecondaryEndFrame;

	public override void init(int effectId, GameObject SpellCaster)
	{
		base.init(effectId, SpellCaster);
		ProjectileItemId = 20;
		Force = 200f;
		BaseDamage = 16;
		splashDamage = 4;
		splashDistance = 1f;
		impactFrameStart = 21;
		impactFrameEnd = 25;
		SecondaryStartFrame = 31;
		SecondaryEndFrame = 35;
		damagetype = DamageTypes.fire;
	}

	public override void onImpact(Transform tf)
	{
		base.onImpact(tf);
		for (int i = 0; i < 3; i++)
		{
			Impact.SpawnHitImpact(Impact.ImpactMagic(), tf.position + Random.insideUnitSphere * 0.5f, SecondaryStartFrame, SecondaryEndFrame);
		}
		Collider[] array = Physics.OverlapSphere(tf.position, splashDistance);
		foreach (Collider collider in array)
		{
			if (collider.gameObject != tf.gameObject)
			{
				if (collider.gameObject.GetComponent<ObjectInteraction>() != null)
				{
					collider.gameObject.GetComponent<ObjectInteraction>().Attack(splashDamage, caster);
				}
				if (collider.gameObject.GetComponent<UWCharacter>() != null)
				{
					collider.gameObject.GetComponent<UWCharacter>().ApplyDamage(splashDamage, caster);
				}
			}
		}
	}
}
