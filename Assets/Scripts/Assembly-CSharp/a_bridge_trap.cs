using UnityEngine;

public class a_bridge_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (((base.owner >> 5) & 1) == 1)
		{
			DestroyBridges(triggerX, triggerY);
		}
		else
		{
			CreateBridges(triggerX, triggerY);
		}
	}

	private void CreateBridges(int triggerX, int triggerY)
	{
		int dirX = 0;
		int dirY = 0;
		GetDirectionsForBridgeTrap(ref dirX, ref dirY);
		for (int i = 0; i < base.quality; i++)
		{
			int num = triggerX + dirX * i;
			int num2 = triggerY + dirY * i;
			if (TileMap.ValidTile(num, num2) && ObjectLoader.findObjectByTypeInTile(UWEBase.CurrentObjectList().objInfo, (short)num, (short)num2, 7) == -1)
			{
				ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(356, 40, 0, 0, 256);
				objectLoaderInfo.xpos = 4;
				objectLoaderInfo.ypos = 4;
				objectLoaderInfo.zpos = base.zpos;
				objectLoaderInfo.flags = (short)(base.owner & 7);
				objectLoaderInfo.enchantment = (short)((base.owner >> 3) & 1);
				objectLoaderInfo.heading = base.heading;
				objectLoaderInfo.ObjectTileX = (short)num;
				objectLoaderInfo.ObjectTileY = (short)num2;
				Vector3 position = ObjectLoader.CalcObjectXYZ(objectLoaderInfo.index, 0);
				ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.LevelModel, position);
			}
		}
	}

	private void GetDirectionsForBridgeTrap(ref int dirX, ref int dirY)
	{
		switch (base.heading)
		{
		case 0:
		case 1:
			dirY = 1;
			break;
		case 2:
		case 3:
			dirX = 1;
			break;
		case 4:
		case 5:
			dirY = -1;
			break;
		case 6:
		case 7:
			dirX = -1;
			break;
		}
	}

	private void DestroyBridges(int triggerX, int triggerY)
	{
		int dirX = 0;
		int dirY = 0;
		GetDirectionsForBridgeTrap(ref dirX, ref dirY);
		for (int i = 0; i < base.quality; i++)
		{
			int num = triggerX + dirX * i;
			int num2 = triggerY + dirY * i;
			if (TileMap.ValidTile(num, num2))
			{
				int num3 = ObjectLoader.findObjectByTypeInTile(UWEBase.CurrentObjectList().objInfo, (short)num, (short)num2, 7);
				if (num3 != -1)
				{
					ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(num3);
					objectIntAt.consumeObject();
				}
			}
		}
	}
}
