using System;
using System.Collections;
using UnityEngine;

public class MagicProjectile : MobileObject
{
	private enum ProjectileHeadings
	{
		NORTH = 0,
		NORTHEAST = 1,
		EAST = 2,
		SOUTHEAST = 3,
		SOUTH = 4,
		SOUTHWEST = 5,
		WEST = 6,
		NORTHWEST = 7
	}

	public float x;

	public float y;

	public float z;

	public bool HasHit;

	public GameObject caster;

	public SpellProp spellprop;

	public Rigidbody rgd;

	public bool DetonateNow;

	public NPC target;

	private short[,] turningHeading = new short[8, 8]
	{
		{ 0, 1, 1, 1, 1, -1, -1, -1 },
		{ -1, 0, 1, 1, 1, 1, -1, -1 },
		{ -1, -1, 0, 1, 1, 1, 1, -1 },
		{ -1, -1, -1, 0, 1, 1, 1, 1 },
		{ 1, -1, -1, -1, 0, 1, 1, 1 },
		{ 1, 1, -1, -1, -1, 0, 1, 1 },
		{ 1, 1, 1, -1, -1, -1, 0, 1 },
		{ 1, 1, 1, 1, -1, -1, -1, 0 }
	};

	private void OnCollisionEnter(Collision collision)
	{
		if (caster == null)
		{
			caster = base.gameObject;
		}
		if (!(collision.gameObject.name == caster.name) && !collision.gameObject.GetComponent<AnimationOverlay>() && !HasHit)
		{
			Detonate(collision);
		}
	}

	public override void Update()
	{
		base.npc_xhome = (short)(base.transform.position.x / 1.2f);
		base.npc_yhome = (short)(base.transform.position.z / 1.2f);
		base.transform.Translate(object_base.ProjectilePropsToVector(this) * Time.deltaTime);
		if (DetonateNow)
		{
			DestroyProjectile();
		}
	}

	protected virtual void Detonate(Collision collision)
	{
		HasHit = true;
		spellprop.onImpact(base.transform);
		ObjectInteraction component = collision.gameObject.GetComponent<ObjectInteraction>();
		if (component != null)
		{
			spellprop.onHit(component);
			component.Attack(spellprop.BaseDamage, caster);
			if (component.GetHitFrameStart() >= 0)
			{
				Impact.SpawnHitImpact(Impact.ImpactMagic(), component.GetImpactPoint(), component.GetHitFrameStart(), component.GetHitFrameEnd());
			}
		}
		else if (collision.gameObject.GetComponent<UWCharacter>() != null)
		{
			int spellResistance = UWCharacter.Instance.getSpellResistance(spellprop);
			collision.gameObject.GetComponent<UWCharacter>().ApplyDamage(spellprop.BaseDamage / spellResistance);
			spellprop.onHitPlayer();
		}
		else
		{
			Impact.SpawnHitImpact(Impact.ImpactDamage(), base.transform.position, spellprop.impactFrameStart, spellprop.impactFrameEnd);
		}
		DestroyProjectile();
	}

	protected virtual void DestroyProjectile()
	{
		ObjectInteraction component = GetComponent<ObjectInteraction>();
		if (component != null)
		{
			component.objectloaderinfo.InUseFlag = 0;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void BeginHoming()
	{
		StartCoroutine(Homing());
	}

	private IEnumerator Homing()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f);
			if (target == null)
			{
				NPC nPCTargetRandom = Magic.GetNPCTargetRandom(base.gameObject, 5f);
				if (nPCTargetRandom != null)
				{
					target = nPCTargetRandom;
				}
			}
			if (target != null)
			{
				Vector3 vector = new Vector3(target.transform.position.x, 0f, target.transform.position.z);
				Vector3 vector2 = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
				float num = Mathf.Atan2(vector.z - vector2.z, vector.x - vector2.x) * 180f / (float)Math.PI;
				num += 180f;
				int num2 = (((double)num >= 337.5 || (double)num < 22.5) ? 6 : (((double)num >= 22.5 && (double)num < 67.5) ? 5 : (((double)num >= 67.5 && (double)num < 112.5) ? 4 : (((double)num >= 112.5 && (double)num < 157.5) ? 3 : (((double)num >= 157.5 && (double)num < 202.5) ? 2 : (((double)num >= 202.5 && (double)num < 247.5) ? 1 : ((!((double)num >= 247.5) || !((double)num < 292.5)) ? ((!((double)num >= 292.5) || !((double)num < 337.5)) ? base.ProjectileHeadingMajor : 7) : 0)))))));
				base.ProjectileHeadingMajor += turningHeading[base.ProjectileHeadingMajor, num2];
				if (base.ProjectileHeadingMajor < 0)
				{
					base.ProjectileHeadingMajor = 7;
				}
				if (base.ProjectileHeadingMajor > 7)
				{
					base.ProjectileHeadingMajor = 0;
				}
				if (base.transform.position.y > target.transform.position.y)
				{
					base.Projectile_Pitch = 1;
					base.Projectile_Sign = 0;
				}
				else
				{
					base.Projectile_Pitch = 1;
					base.Projectile_Sign = 1;
				}
			}
		}
	}

	public void BeginVapourTrail()
	{
		StartCoroutine(VapourTrail());
	}

	private IEnumerator VapourTrail()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			Impact.SpawnHitImpact(462, base.transform.position, 56, 62);
		}
	}
}
