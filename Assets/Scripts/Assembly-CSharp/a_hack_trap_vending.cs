using UnityEngine;

public class a_hack_trap_vending : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Vector3 tileVector = UWEBase.CurrentTileMap().getTileVector(base.ObjectTileX, base.ObjectTileY);
		tileVector = new Vector3(tileVector.x, 4.4f, tileVector.z);
		int num = 0;
		int num2 = 0;
		switch (Quest.instance.variables[base.owner])
		{
		default:
			return;
		case 0:
			num = 182;
			num2 = 3;
			break;
		case 1:
			num = 176;
			num2 = 3;
			break;
		case 2:
			num = 187;
			num2 = 4;
			break;
		case 3:
			num = 293;
			num2 = 4;
			break;
		case 4:
			num = 188;
			num2 = 3;
			break;
		case 5:
			num = 3;
			num2 = 11;
			break;
		case 6:
			num = 257;
			num2 = 6;
			break;
		case 7:
			num = 145;
			num2 = 4;
			break;
		}
		if (CheckPrice(num2, base.ObjectTileX, base.ObjectTileY))
		{
			ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(num, 40, 0, 0, 256);
			objectLoaderInfo.InUseFlag = 1;
			UWEBase.UnFreezeMovement(GameWorldController.MoveToWorld(ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, tileVector)).gameObject);
		}
	}

	private bool CheckPrice(int TargetPrice, int triggerX, int triggerY)
	{
		Collider[] array = Physics.OverlapBox(halfExtents: new Vector3(0.59f, 0.15f, 0.59f), center: UWEBase.CurrentTileMap().getTileVector(triggerX, triggerY));
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if (array[i].gameObject.GetComponent<ObjectInteraction>() != null && array[i].gameObject.GetComponent<ObjectInteraction>().item_id == 160 && array[i].gameObject.GetComponent<ObjectInteraction>().GetQty() >= TargetPrice)
			{
				for (int j = 0; j < TargetPrice; j++)
				{
					array[i].gameObject.GetComponent<ObjectInteraction>().consumeObject();
				}
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
