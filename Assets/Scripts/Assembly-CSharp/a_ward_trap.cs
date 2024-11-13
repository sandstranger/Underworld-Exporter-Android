using UnityEngine;

public class a_ward_trap : trap_base
{
	public SpellProp spellprop;

	protected override void Start()
	{
		base.Start();
		base.gameObject.layer = LayerMask.NameToLayer("Ward");
		BoxCollider boxCollider = base.gameObject.GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = base.gameObject.AddComponent<BoxCollider>();
		}
		boxCollider.size = new Vector3(0.35f, 0.35f, 0.35f);
		boxCollider.center = new Vector3(0f, 0.1f, 0f);
		boxCollider.isTrigger = true;
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (base.item_id)
			{
			case 414:
			{
				SpellProp_Fireball spellProp_Fireball = new SpellProp_Fireball();
				spellProp_Fireball.init(83, UWCharacter.Instance.gameObject);
				spellprop = spellProp_Fireball;
				break;
			}
			case 415:
			{
				SpellProp_Tym spellProp_Tym = new SpellProp_Tym();
				spellProp_Tym.init(117, UWCharacter.Instance.gameObject);
				spellprop = spellProp_Tym;
				break;
			}
			default:
				Debug.Log("unimplemented ward trap type " + base.item_id);
				break;
			}
		}
		else
		{
			SpellProp_RuneOfWarding spellProp_RuneOfWarding = new SpellProp_RuneOfWarding();
			spellProp_RuneOfWarding.init(131, UWCharacter.Instance.gameObject);
			spellprop = spellProp_RuneOfWarding;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		NPC component = other.gameObject.GetComponent<NPC>();
		if (component != null)
		{
			if (spellprop.BaseDamage != 0)
			{
				component.ApplyAttack(spellprop.BaseDamage);
			}
			spellprop.onImpact(component.transform);
			spellprop.onHit(component.gameObject.GetComponent<ObjectInteraction>());
			objInt().consumeObject();
		}
		else if ((bool)other.gameObject.GetComponent<UWCharacter>())
		{
			if (spellprop.BaseDamage != 0)
			{
				UWCharacter.Instance.ApplyDamage(spellprop.BaseDamage);
			}
			spellprop.onHitPlayer();
			objInt().consumeObject();
		}
		else if (UWEBase._RES == "UW2")
		{
			objInt().consumeObject();
		}
	}
}
