using UnityEngine;

public class a_pit_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		GameObject gameObject = GameWorldController.FindTile(triggerX, triggerY, 1);
		if (gameObject != null)
		{
			TileInfo tileInfo = UWEBase.CurrentTileMap().Tiles[triggerX, triggerY];
			if (tileInfo.floorHeight == 0)
			{
				tileInfo.floorHeight = (short)(base.zpos >> 2);
				tileInfo.floorTexture = (short)(base.owner & 0xF);
			}
			else
			{
				tileInfo.floorHeight = 0;
				tileInfo.floorTexture = (short)(base.quality & 0xF);
			}
			tileInfo.TileNeedsUpdate();
			Object.Destroy(gameObject);
		}
	}
}
