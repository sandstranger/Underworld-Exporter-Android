using UnityEngine;

public class a_create_object_trap : trap_base
{
	public static string LastObjectCreated = "";

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		string text = "";
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
		if (!(objectIntAt != null))
		{
			return;
		}
		GameObject gameObject = CloneObject(objectIntAt, triggerX, triggerY, true);
		LastObjectCreated = gameObject.name;
		text = gameObject.name;
		if (!(objectIntAt.GetComponent<Container>() != null))
		{
			return;
		}
		for (short num = 0; num <= objectIntAt.GetComponent<Container>().MaxCapacity(); num++)
		{
			ObjectInteraction itemAt = objectIntAt.GetComponent<Container>().GetItemAt(num);
			if (itemAt != null)
			{
				GameObject gameObject2 = CloneObject(itemAt, triggerX, triggerY, false);
				gameObject.GetComponent<Container>().items[num] = gameObject2.GetComponent<ObjectInteraction>();
			}
		}
	}

	public GameObject CloneObject(ObjectInteraction objToClone, int triggerX, int triggerY, bool MoveItem)
	{
		GameObject gameObject = Object.Instantiate(objToClone.gameObject);
		ObjectLoaderInfo objectLoaderInfo = ((!(objToClone.GetComponent<NPC>() != null)) ? ObjectLoader.newObject(objToClone.item_id, objToClone.quality, objToClone.quality, objToClone.link, 256) : ObjectLoader.newObject(objToClone.item_id, objToClone.quality, objToClone.quality, objToClone.link, 2));
		objectLoaderInfo.instance = gameObject.GetComponent<ObjectInteraction>();
		gameObject.GetComponent<ObjectInteraction>().objectloaderinfo = objectLoaderInfo;
		objectLoaderInfo.InUseFlag = 1;
		if (MoveItem)
		{
			if (base.gameObject.transform.position.x >= 100f)
			{
				gameObject.transform.position = UWEBase.CurrentTileMap().getTileVector(triggerX, triggerY);
			}
			else
			{
				Vector3 tileVector = UWEBase.CurrentTileMap().getTileVector(triggerX, triggerY);
				gameObject.transform.position = new Vector3(tileVector.x, base.gameObject.transform.position.y, tileVector.z);
			}
		}
		gameObject.transform.parent = objToClone.transform.parent;
		objectLoaderInfo.instance.UpdatePosition();
		gameObject.name = ObjectLoader.UniqueObjectName(objectLoaderInfo);
		return gameObject;
	}

	public override bool Activate(object_base src, int triggerX, int triggerY, int State)
	{
		ExecuteTrap(this, triggerX, triggerY, State);
		PostActivate(src);
		return true;
	}
}
