public class a_change_from_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		ObjectInteraction objectInteraction = null;
		if (base.link != 0)
		{
			objectInteraction = UWEBase.CurrentObjectList().objInfo[base.link].instance;
		}
		if (objectInteraction == null)
		{
			return;
		}
		short floorTexture = (short)(objectInteraction.heading | (((objectInteraction.zpos >> 4) & 1) << 3));
		short num = (short)(base.heading | (((base.zpos >> 4) & 1) << 3));
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				if (base.quality == UWEBase.CurrentTileMap().Tiles[i, j].wallTexture)
				{
					UWEBase.CurrentTileMap().Tiles[i, j].wallTexture = objectInteraction.quality;
				}
				if (UWEBase.CurrentTileMap().Tiles[i, j].floorTexture != num)
				{
					continue;
				}
				if (UWEBase.CurrentTileMap().Tiles[i, j].Render)
				{
					for (int k = 0; k < 6; k++)
					{
						UWEBase.CurrentTileMap().Tiles[i, j].VisibleFaces[k] = true;
						UWEBase.CurrentTileMap().Tiles[i, j].VisibleFaces[k] = true;
					}
				}
				UWEBase.CurrentTileMap().Tiles[i, j].floorTexture = floorTexture;
				if (objectInteraction.owner < 10)
				{
					UWEBase.CurrentTileMap().Tiles[i, j].tileType = objectInteraction.owner;
				}
				if (objectInteraction.zpos < 15)
				{
					UWEBase.CurrentTileMap().Tiles[i, j].floorHeight = objectInteraction.zpos;
				}
			}
		}
		UWEBase.CurrentTileMap().SetTileMapWallFacesUW();
		GameWorldController.WorldReRenderPending = true;
		GameWorldController.FullReRender = true;
	}
}
