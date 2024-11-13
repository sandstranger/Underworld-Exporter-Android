using UnityEngine;

public class a_hack_trap_forcefield : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		ObjectInteraction objectInteraction = findForceField();
		if (objectInteraction != null)
		{
			if (objectInteraction.zpos == 127)
			{
				objectInteraction.zpos = 0;
				objectInteraction.objectloaderinfo.zpos = 0;
			}
			else
			{
				objectInteraction.zpos = 127;
				objectInteraction.objectloaderinfo.zpos = 127;
			}
			objectInteraction.transform.position = ObjectLoader.CalcObjectXYZ(objectInteraction.objectloaderinfo.index, 1);
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}

	private ObjectInteraction findForceField()
	{
		ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
		for (int i = 256; i <= objInfo.GetUpperBound(0); i++)
		{
			if (objInfo[i].InUseFlag == 1 && objInfo[i].instance != null && objInfo[i].item_id == 365 && objInfo[i].instance.ObjectTileX == base.ObjectTileX && objInfo[i].instance.ObjectTileY == base.ObjectTileY)
			{
				return objInfo[i].instance;
			}
		}
		return null;
	}
}
