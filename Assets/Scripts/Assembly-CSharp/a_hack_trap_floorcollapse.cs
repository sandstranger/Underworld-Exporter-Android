using UnityEngine;

public class a_hack_trap_floorcollapse : a_hack_trap
{
	private const int range = 10;

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (TileMap.visitedTileX < triggerX - 10 || TileMap.visitedTileY < triggerY - 10 || TileMap.visitedTileX > triggerX + 10 || TileMap.visitedTileY > triggerY + 10 || !UWCharacter.Instance.Grounded)
		{
			return;
		}
		int floorTexture = UWEBase.CurrentTileMap().Tiles[TileMap.visitTileX, TileMap.visitTileY].floorTexture;
		if (floorTexture != base.owner)
		{
			return;
		}
		TileInfo tileInfo = UWEBase.CurrentTileMap().Tiles[TileMap.visitTileX, TileMap.visitTileY];
		if (tileInfo.floorHeight >= 2)
		{
			tileInfo.floorTexture = (short)((base.ypos << 3) | base.xpos);
			tileInfo.floorHeight -= 2;
			tileInfo.Render = true;
			for (int i = 0; i < 6; i++)
			{
				tileInfo.VisibleFaces[i] = true;
			}
			GameObject gameObject = GameWorldController.FindTile(TileMap.visitTileX, TileMap.visitTileY, 1);
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
			tileInfo.TileNeedsUpdate();
		}
	}
}
