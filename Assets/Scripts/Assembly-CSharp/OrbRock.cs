using UnityEngine;

public class OrbRock : object_base
{
	public static void DestroyOrb(ObjectInteraction orbToDestroy)
	{
		Impact.SpawnHitImpact(Impact.ImpactDamage(), orbToDestroy.GetImpactPoint(), 46, 50);
		Quest.instance.isOrbDestroyed = true;
		UWCharacter.Instance.PlayerMagic.MaxMana = UWCharacter.Instance.PlayerMagic.TrueMaxMana;
		UWCharacter.Instance.PlayerMagic.CurMana = UWCharacter.Instance.PlayerMagic.MaxMana;
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 133));
		orbToDestroy.consumeObject();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!objInt().PickedUp && GameWorldController.instance.LevelNo == 6 && UWEBase._RES == "UW1" && collision.gameObject.name.Contains("orb"))
		{
			ObjectInteraction component = collision.gameObject.GetComponent<ObjectInteraction>();
			if (component.GetItemType() == 98)
			{
				DestroyOrb(component);
			}
		}
	}
}
