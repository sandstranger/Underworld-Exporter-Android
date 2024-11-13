using UnityEngine;

public class a_hack_trap_platformwave : a_hack_trap
{
	public Vector3 TileVector;

	public Vector3 ContactArea = new Vector3(0.59f, 0.15f, 0.59f);

	private const int baseHeight = 18;

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		base.owner++;
		if (base.owner > 15)
		{
			base.owner = 1;
		}
		switch (base.owner)
		{
		case 1:
			ChangeTileHeight(10, 49, 16);
			ChangeTileHeight(10, 50, 18);
			ChangeTileHeight(10, 51, 16);
			ChangeTileHeight(10, 45, 20);
			ChangeTileHeight(10, 46, 18);
			ChangeTileHeight(10, 47, 20);
			break;
		case 3:
			ChangeTileHeight(10, 49, 20);
			ChangeTileHeight(10, 50, 22);
			ChangeTileHeight(10, 51, 20);
			ChangeTileHeight(10, 45, 16);
			ChangeTileHeight(10, 46, 14);
			ChangeTileHeight(10, 47, 16);
			break;
		case 5:
			ChangeTileHeight(10, 49, 22);
			ChangeTileHeight(10, 50, 24);
			ChangeTileHeight(10, 51, 22);
			ChangeTileHeight(10, 45, 14);
			ChangeTileHeight(10, 46, 12);
			ChangeTileHeight(10, 47, 14);
			break;
		case 7:
			ChangeTileHeight(10, 49, 20);
			ChangeTileHeight(10, 50, 22);
			ChangeTileHeight(10, 51, 20);
			ChangeTileHeight(10, 45, 16);
			ChangeTileHeight(10, 46, 14);
			ChangeTileHeight(10, 47, 16);
			break;
		case 9:
			ChangeTileHeight(10, 49, 18);
			ChangeTileHeight(10, 50, 18);
			ChangeTileHeight(10, 51, 18);
			ChangeTileHeight(10, 45, 18);
			ChangeTileHeight(10, 46, 18);
			ChangeTileHeight(10, 47, 18);
			break;
		case 11:
			ChangeTileHeight(10, 49, 16);
			ChangeTileHeight(10, 50, 14);
			ChangeTileHeight(10, 51, 16);
			ChangeTileHeight(10, 45, 20);
			ChangeTileHeight(10, 46, 22);
			ChangeTileHeight(10, 47, 20);
			break;
		case 13:
			ChangeTileHeight(10, 49, 14);
			ChangeTileHeight(10, 50, 12);
			ChangeTileHeight(10, 51, 14);
			ChangeTileHeight(10, 45, 22);
			ChangeTileHeight(10, 46, 24);
			ChangeTileHeight(10, 47, 22);
			break;
		case 15:
			ChangeTileHeight(10, 49, 16);
			ChangeTileHeight(10, 50, 14);
			ChangeTileHeight(10, 51, 16);
			ChangeTileHeight(10, 45, 20);
			ChangeTileHeight(10, 46, 22);
			ChangeTileHeight(10, 47, 20);
			break;
		case 2:
		case 4:
		case 6:
		case 8:
		case 10:
		case 12:
		case 14:
			break;
		}
	}

	private void ChangeTileHeight(int tileX, int tileY, int newHeight)
	{
		TileInfo tileInfo = UWEBase.CurrentTileMap().Tiles[tileX, tileY];
		tileInfo.floorHeight = (short)newHeight;
		GameObject obj = GameWorldController.FindTile(tileX, tileY, 1);
		TileVector = UWEBase.CurrentTileMap().getTileVector(tileX, tileY);
		Collider[] colliders = Physics.OverlapBox(TileVector, ContactArea);
		MoveObjectsInContact((float)newHeight * 0.15f, colliders);
		Object.DestroyImmediate(obj);
		TileMapRenderer.RenderTile(GameWorldController.instance.LevelModel, tileInfo.tileX, tileInfo.tileY, tileInfo, false, false, false, true);
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}

	public void MoveObjectsInContact(float Height, Collider[] colliders)
	{
		for (int i = 0; i <= colliders.GetUpperBound(0); i++)
		{
			if (colliders[i].gameObject.GetComponent<ObjectInteraction>() != null && colliders[i].gameObject.GetComponent<ObjectInteraction>().isMoveable())
			{
				Vector3 position = colliders[i].gameObject.transform.position;
				UWEBase.UnFreezeMovement(colliders[i].gameObject);
				colliders[i].gameObject.transform.position = new Vector3(position.x, Height, position.z);
			}
		}
	}
}
