using UnityEngine;

public class a_hack_trap_light_recharge : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Collider[] array = Physics.OverlapBox(halfExtents: new Vector3(0.59f, 0.15f, 0.59f), center: UWEBase.CurrentTileMap().getTileVector(triggerX, triggerY));
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if (array[i].gameObject.GetComponent<ObjectInteraction>() != null && (array[i].gameObject.GetComponent<ObjectInteraction>().item_id == 147 || array[i].gameObject.GetComponent<ObjectInteraction>().item_id == 151))
			{
				array[i].gameObject.GetComponent<ObjectInteraction>().quality = 63;
				Debug.Log("sparkle sparkle");
			}
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
