using System.Collections;
using UnityEngine;

public class ProjectileDamage : UWEBase
{
	private bool hasHit;

	public short Damage;

	public string LastTarget;

	public int AttackScore;

	public float AttackCharge;

	public int ArmourDamage;

	public GameObject Source;

	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("MagicProjectile");
		BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(0.3f, 0.3f, 0.3f);
		boxCollider.center = new Vector3(0f, 0.1f, 0f);
		boxCollider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		HandleImpact(other.gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		HandleImpact(collision.gameObject);
	}

	private void HandleImpact(GameObject other)
	{
		if (Source == null)
		{
			Source = base.gameObject;
		}
		if (!hasHit && other.name == Source.name)
		{
			return;
		}
		if (!hasHit)
		{
			hasHit = true;
			StartCoroutine(EndProjectile());
		}
		if (!(LastTarget != other.name))
		{
			return;
		}
		LastTarget = other.name;
		int num = 0;
		int num2 = 0;
		if (other.name == "_Gronk")
		{
			num = UWCharacter.Instance.PlayerSkills.GetSkill(2) + UWCharacter.Instance.PlayerSkills.GetSkill(7) / 2;
			num2 = UWCharacter.Instance.playerInventory.getArmourScore();
		}
		else
		{
			if (!(other.GetComponent<NPC>() != null))
			{
				return;
			}
			num = other.GetComponent<NPC>().GetDefence();
		}
		int num3 = Mathf.Max(num - AttackScore, 1);
		int num4 = Random.Range(1, 31);
		if (num4 < num3)
		{
			return;
		}
		int num5 = (int)Mathf.Max((float)Damage * (AttackCharge / 100f), 1f);
		num5 = Mathf.Max(1, num5 - num2);
		if (other.gameObject.GetComponent<ObjectInteraction>() != null)
		{
			other.gameObject.GetComponent<ObjectInteraction>().Attack((short)num5, Source);
		}
		else if ((bool)other.GetComponent<UWCharacter>())
		{
			other.GetComponent<UWCharacter>().ApplyDamage(num5, Source);
			if (num5 > num2)
			{
				UWCharacter.Instance.playerInventory.ApplyArmourDamage((short)Random.Range(0, ArmourDamage + 1));
			}
		}
		else
		{
			Damage /= 2;
		}
	}

	public IEnumerator EndProjectile()
	{
		yield return new WaitForSeconds(1f);
		Object.Destroy(base.gameObject);
	}
}
