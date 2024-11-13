using UnityEngine;

public class a_hack_trap_tmap_range_change : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		int num = Mathf.Min(triggerX, src.ObjectTileX);
		int num2 = Mathf.Min(triggerY, src.ObjectTileY);
		int num3 = Mathf.Max(triggerX, src.ObjectTileX);
		int num4 = Mathf.Max(triggerY, src.ObjectTileY);
		num = 0;
		num2 = 0;
		num3 = 63;
		num4 = 63;
		ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
		for (int i = 0; i <= objInfo.GetUpperBound(0); i++)
		{
			if ((objInfo[i].item_id == 366 || objInfo[i].item_id == 367) && objInfo[i].instance != null && objInfo[i].instance.ObjectTileX >= num && objInfo[i].instance.ObjectTileX <= num3 && objInfo[i].instance.ObjectTileY >= num2 && objInfo[i].instance.ObjectTileY <= num4 && objInfo[i].instance.owner == 63)
			{
				TMAP component = objInfo[i].instance.GetComponent<TMAP>();
				if (component != null)
				{
					component.owner = (short)(40 + base.owner);
					component.TextureIndex = UWEBase.CurrentTileMap().texture_map[40 + base.owner];
					TMAP.CreateTMAP(component.gameObject, component.TextureIndex);
				}
			}
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
