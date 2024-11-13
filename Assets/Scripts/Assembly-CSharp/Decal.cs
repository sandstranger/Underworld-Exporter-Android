using UnityEngine;

public class Decal : object_base
{
	protected override void Start()
	{
		base.Start();
		int objectTileX = base.ObjectTileX;
		int objectTileY = base.ObjectTileY;
		int num = base.xpos;
		int num2 = base.ypos;
		int num3 = base.heading * 45;
		Vector3 position = base.transform.position;
		Vector3 zero = Vector3.zero;
		if (!TileMap.ValidTile(objectTileX, objectTileY))
		{
			return;
		}
		switch (UWEBase.CurrentTileMap().Tiles[objectTileX, objectTileY].tileType)
		{
		case 1:
			if (num == 0 && (num3 == 0 || num3 == 180))
			{
				zero += new Vector3(0.2f, 0f, 0f);
			}
			if (num == 7 && (num3 == 0 || num3 == 180))
			{
				zero += new Vector3(-0.2f, 0f, 0f);
			}
			if (num2 == 0 && (num3 == 270 || num3 == 90))
			{
				zero += new Vector3(0f, 0f, 0.2f);
			}
			if (num2 == 7 && (num3 == 270 || num3 == 90))
			{
				zero += new Vector3(0f, 0f, -0.2f);
			}
			break;
		case 5:
			if (num3 == 135)
			{
				zero += new Vector3(-0.02f, 0f, 0.02f);
			}
			break;
		case 4:
			if (num3 == 225)
			{
				zero += new Vector3(0.02f, 0f, 0.02f);
			}
			break;
		case 2:
			if (num3 == 315)
			{
				zero += new Vector3(-0.08f, 0f, 0.08f);
			}
			break;
		case 3:
			if (num3 == 45)
			{
				zero += new Vector3(0.08f, 0f, 0.08f);
			}
			break;
		}
		base.transform.position = position + zero;
	}
}
