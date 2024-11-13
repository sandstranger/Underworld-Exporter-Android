using UnityEngine;

public class a_do_trap_emeraldpuzzle : a_hack_trap
{
	public bool hasExecuted;

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Debug.Log(base.name);
		if (!hasExecuted && CheckPlinths())
		{
			CreateRuneStone(253);
			hasExecuted = true;
		}
	}

	private void CreateRuneStone(int ItemID)
	{
		ObjectLoaderInfo currObj = ObjectLoader.newObject(ItemID, 0, 0, 1, 256);
		GameObject myObj = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, new Vector3(64.5f, 4f, 24.5f)).gameObject;
		UWEBase.UnFreezeMovement(myObj);
	}

	private bool CheckPlinths()
	{
		if (CheckArea(new Vector3(59.4f, 3.9f, 27f), 1.2f, 167) && CheckArea(new Vector3(59.4f, 3.9f, 17.5f), 1.2f, 167) && CheckArea(new Vector3(69f, 3.9f, 17.5f), 1.2f, 167) && CheckArea(new Vector3(69f, 3.9f, 27f), 1.2f, 167))
		{
			return true;
		}
		return false;
	}

	private bool CheckArea(Vector3 centre, float radius, int item_id)
	{
		Collider[] array = Physics.OverlapSphere(centre, radius);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.GetComponent<ObjectInteraction>() != null && array[i].gameObject.GetComponent<ObjectInteraction>().item_id == item_id)
			{
				Debug.Log("found " + array[i].gameObject.name);
				return true;
			}
		}
		return false;
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
