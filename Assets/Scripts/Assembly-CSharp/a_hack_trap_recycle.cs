using UnityEngine;

public class a_hack_trap_recycle : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Collider[] array = Physics.OverlapBox(halfExtents: new Vector3(0.59f, 0.15f, 0.59f), center: UWEBase.CurrentTileMap().getTileVector(base.ObjectTileX, base.ObjectTileY));
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if (array[i].gameObject.GetComponent<ObjectInteraction>() != null && array[i].gameObject.GetComponent<ObjectInteraction>().item_id == 317)
			{
				object_base component = array[i].gameObject.GetComponent<object_base>();
				Object.Destroy(component);
				array[i].gameObject.AddComponent<Coin>();
				array[i].gameObject.GetComponent<ObjectInteraction>().ChangeType(160);
			}
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
