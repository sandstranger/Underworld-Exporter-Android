using UnityEngine;

public class a_hack_trap_skup : a_hack_trap
{
	private bool skupSpawned = false;

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (TestObjectAtTile(59, 4, 161, 2) == -1 || TestObjectAtTile(57, 4, 161, 6) == -1 || TestObjectAtTile(58, 4, 174, -1) == -1)
		{
			return;
		}
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(642);
		if (!(objectIntAt != null) || !(objectIntAt.GetComponent<trigger_base>() != null))
		{
			return;
		}
		skupSpawned = true;
		Quest.instance.QuestVariables[38] = 1;
		Quest.instance.QuestVariables[122] = 0;
		objectIntAt.GetComponent<trigger_base>().Activate(base.gameObject);
		int num = TestObjectAtTile(59, 4, 161, 2);
		if (num != -1)
		{
			ObjectInteraction objectIntAt2 = ObjectLoader.getObjectIntAt(num);
			if (objectIntAt2 != null)
			{
				objectIntAt2.consumeObject();
			}
		}
		num = TestObjectAtTile(57, 4, 161, 6);
		if (num != -1)
		{
			ObjectInteraction objectIntAt3 = ObjectLoader.getObjectIntAt(num);
			if (objectIntAt3 != null)
			{
				objectIntAt3.consumeObject();
			}
		}
	}

	private int TestObjectAtTile(int tileX, int tileY, int ObjectToFind, int QualityToFind)
	{
		Collider[] array = Physics.OverlapBox(halfExtents: new Vector3(0.59f, 0.15f, 0.59f), center: UWEBase.CurrentTileMap().getTileVector(tileX, tileY));
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if (!(array[i].gameObject.GetComponent<ObjectInteraction>() != null) || array[i].gameObject.GetComponent<ObjectInteraction>().item_id != ObjectToFind)
			{
				continue;
			}
			if (QualityToFind != -1)
			{
				if (array[i].gameObject.GetComponent<ObjectInteraction>().quality == QualityToFind)
				{
					return array[i].gameObject.GetComponent<ObjectInteraction>().objectloaderinfo.index;
				}
				return -1;
			}
			return array[i].gameObject.GetComponent<ObjectInteraction>().objectloaderinfo.index;
		}
		return -1;
	}

	public override void PostActivate(object_base src)
	{
		if (skupSpawned)
		{
			Debug.Log("Overridden PostActivate to test " + base.name);
			base.PostActivate(src);
		}
	}
}
