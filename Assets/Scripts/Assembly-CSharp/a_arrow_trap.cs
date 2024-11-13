using UnityEngine;

public class a_arrow_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		int num = (base.quality << 5) | base.owner;
		ObjectLoaderInfo currObj = ObjectLoader.newObject(num, 0, 0, 0, 256);
		GameObject gameObject = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, base.transform.position).gameObject;
		if (base.ObjectTileX == 99)
		{
			Vector3 position = UWEBase.CurrentTileMap().getTileVector(triggerX, triggerY);
			position = new Vector3(position.x, base.transform.position.y, position.z);
			gameObject.transform.position = position;
		}
		else
		{
			gameObject.transform.position = base.transform.position;
		}
		gameObject.transform.rotation = base.transform.rotation;
		if (gameObject.GetComponent<Rigidbody>() == null)
		{
			gameObject.AddComponent<Rigidbody>();
		}
		UWEBase.UnFreezeMovement(gameObject);
		gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
		gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 20f * base.owner);
		GameObject gameObject2 = new GameObject(gameObject.name + "_damage");
		gameObject2.transform.position = gameObject.transform.position;
		gameObject2.transform.parent = gameObject.transform;
		ProjectileDamage projectileDamage = gameObject2.AddComponent<ProjectileDamage>();
		projectileDamage.Source = base.gameObject;
		projectileDamage.Damage = 10;
		projectileDamage.AttackCharge = 100f;
		projectileDamage.AttackScore = 15;
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
