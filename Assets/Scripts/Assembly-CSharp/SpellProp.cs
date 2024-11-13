using UnityEngine;

public class SpellProp : UWClass
{
	public enum DamageTypes
	{
		fire = 0,
		acid = 1,
		magic = 2,
		physcial = 3,
		electric = 4,
		poison = 5,
		aid = 6,
		psychic = 7,
		holy = 8,
		light = 9,
		protection = 10,
		mobility = 11
	}

	public bool CastRaySource;

	public short BaseDamage;

	public short counter;

	public int DOT;

	public int noOfCasts = 1;

	public float spread;

	public float Force;

	public int ProjectileItemId;

	public int impactFrameStart;

	public int impactFrameEnd;

	public static UWCharacter playerUW;

	public bool homing = false;

	public bool hasTrail = false;

	public bool silent;

	public GameObject caster;

	public DamageTypes damagetype = DamageTypes.magic;

	public virtual void init(int effectId, GameObject SpellCaster)
	{
		caster = SpellCaster;
	}

	public virtual void onImpact(Transform tf)
	{
	}

	public virtual void onHit(ObjectInteraction objInt)
	{
	}

	public virtual void onHitPlayer()
	{
	}
}
