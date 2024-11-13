using UnityEngine;

public class BouncingProjectile : MagicProjectile
{
	protected override void Start()
	{
		SpellProp_Fireball spellProp_Fireball = new SpellProp_Fireball();
		spellProp_Fireball.init(83, base.gameObject);
		spellprop = spellProp_Fireball;
		base.gameObject.layer = LayerMask.NameToLayer("MagicProjectile");
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		component.size = new Vector3(0.1f, 0.1f, 0.1f);
		component.center = new Vector3(0f, 0.1f, 0f);
		rgd = base.gameObject.GetComponent<Rigidbody>();
		rgd.freezeRotation = true;
		rgd.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}

	protected override void DestroyProjectile()
	{
		reflectprojectile();
	}

	private void reflectprojectile()
	{
		HasHit = false;
		switch (base.ProjectileHeadingMajor)
		{
		case 1:
			base.ProjectileHeadingMajor = 5;
			break;
		case 2:
			base.ProjectileHeadingMajor = 6;
			break;
		case 3:
			base.ProjectileHeadingMajor = 7;
			break;
		case 4:
			base.ProjectileHeadingMajor = 0;
			break;
		case 5:
			base.ProjectileHeadingMajor = 1;
			break;
		case 6:
			base.ProjectileHeadingMajor = 2;
			break;
		case 7:
			base.ProjectileHeadingMajor = 3;
			break;
		default:
			base.ProjectileHeadingMajor = 4;
			break;
		}
	}
}
