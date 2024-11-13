using UnityEngine;

public class a_delete_object_trap : trap_base
{
	public override bool Activate(object_base src, int triggerX, int triggerY, int State)
	{
		ExecuteTrap(this, triggerX, triggerY, State);
		PostActivate(src);
		return true;
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (gameObjectAt != null)
		{
			gameObjectAt.GetComponent<ObjectInteraction>().objectloaderinfo.InUseFlag = 0;
			if (gameObjectAt.GetComponent<map_object>() != null)
			{
				Object.Destroy(gameObjectAt.GetComponent<map_object>().ModelInstance);
			}
			Debug.Log(base.name + " deleting " + gameObjectAt.name);
			Object.Destroy(gameObjectAt);
		}
	}
}
