using UnityEngine;

public class a_hack_trap_scintplatformreset : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		UpdateTile(25, 32, 0);
		UpdateTile(25, 35, 2);
		UpdateTile(25, 38, 1);
		UpdateTile(25, 41, 2);
		UpdateTile(25, 44, 2);
		UpdateTile(28, 32, 3);
		UpdateTile(28, 35, 1);
		UpdateTile(28, 38, 0);
		UpdateTile(28, 41, 1);
		UpdateTile(28, 44, 3);
		UpdateTile(31, 32, 0);
		UpdateTile(31, 35, 3);
		UpdateTile(31, 38, 1);
		UpdateTile(31, 41, 0);
		UpdateTile(31, 44, 2);
		UpdateTile(34, 32, 3);
		UpdateTile(34, 35, 0);
		UpdateTile(34, 38, 0);
		UpdateTile(34, 41, 0);
		UpdateTile(34, 44, 3);
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}

	private void UpdateTile(int tileXToChange, int tileYToChange, short newFloorTexture)
	{
		TileInfo tileInfo = UWEBase.CurrentTileMap().Tiles[tileXToChange, tileYToChange];
		if (tileInfo.floorTexture != newFloorTexture)
		{
			GameObject obj = GameWorldController.FindTile(tileXToChange, tileYToChange, 1);
			tileInfo.TileNeedsUpdate();
			TileMapRenderer.UpdateTile(tileXToChange, tileYToChange, tileInfo.tileType, 18, newFloorTexture, tileInfo.wallTexture, false);
			Object.Destroy(obj);
		}
	}
}
