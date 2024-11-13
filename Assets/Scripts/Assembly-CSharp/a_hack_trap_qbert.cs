using UnityEngine;

public class a_hack_trap_qbert : a_hack_trap
{
	private const int QbertColourRed = 0;

	private const int QbertColourGreen = 1;

	private const int QbertColourBlue = 2;

	private const int QbertColourPurple = 3;

	private const int QbertColourYellow = 4;

	private const int QbertColourOrange = 5;

	private const int QbertColourWhite = 6;

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		switch (base.owner)
		{
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
			ExitToPyramid(base.owner);
			break;
		case 16:
			LeaveArea();
			break;
		case 12:
		case 32:
			RandomTeleport(src);
			break;
		case 62:
			BeginQbertPyramid();
			break;
		case 63:
			StepOnPyramid(TileMap.visitTileX, TileMap.visitTileY);
			break;
		default:
			UWHUD.instance.MessageScroll.Add("Unimplemented qbert destination " + base.owner);
			break;
		}
	}

	private void ExitToPyramid(int colourToSet)
	{
		AddColourToQbert(colourToSet);
		TeleportToLocation(68, 49, 51);
		UWCharacter.Instance.transform.position = new Vector3(UWCharacter.Instance.transform.position.x, 4.2f, UWCharacter.Instance.transform.position.z);
	}

	private void AddColourToQbert(int colourToAdd)
	{
		int[] colourSequence = getColourSequence();
		bool flag = false;
		for (int i = 0; i < colourSequence.GetUpperBound(0); i++)
		{
			if (colourSequence[i] == colourToAdd)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			Quest.instance.variables[101 + colourSequence.GetUpperBound(0)] = colourToAdd;
		}
	}

	private void RandomTeleport(object_base src)
	{
		ObjectInteraction obj = null;
		if (FindMoongateInTile(src.ObjectTileX, src.ObjectTileY, out obj))
		{
			switch (obj.link)
			{
			case 545:
			case 557:
				TeleportRed();
				break;
			case 591:
				TeleportBlue();
				break;
			case 603:
				TeleportPurple();
				break;
			case 528:
				TeleportYellow();
				break;
			}
		}
	}

	private void TeleportRed()
	{
		switch (Random.Range(0, 7))
		{
		case 0:
			TeleportToLocation(68, 19, 3);
			return;
		case 1:
			TeleportToLocation(68, 17, 31);
			return;
		case 2:
			TeleportToLocation(68, 22, 50);
			return;
		case 3:
			TeleportToLocation(68, 46, 23);
			return;
		}
		if (GameWorldController.instance.LevelNo != 64)
		{
			TeleportToLocation(64, 44, 56);
		}
		else
		{
			TeleportToLocation(68, 19, 3);
		}
	}

	private void TeleportBlue()
	{
		switch (Random.Range(0, 7))
		{
		case 0:
			TeleportToLocation(68, 33, 38);
			return;
		case 1:
			TeleportToLocation(68, 47, 14);
			return;
		case 2:
			TeleportToLocation(68, 22, 50);
			return;
		case 3:
			TeleportToLocation(68, 12, 6);
			return;
		}
		if (GameWorldController.instance.LevelNo != 64)
		{
			TeleportToLocation(64, 10, 60);
		}
		else
		{
			TeleportToLocation(68, 33, 38);
		}
	}

	private void TeleportYellow()
	{
		switch (Random.Range(0, 7))
		{
		case 0:
			TeleportToLocation(68, 44, 19);
			return;
		case 1:
			TeleportToLocation(68, 21, 17);
			return;
		case 2:
			TeleportToLocation(68, 22, 50);
			return;
		case 3:
			TeleportToLocation(68, 24, 38);
			return;
		}
		if (GameWorldController.instance.LevelNo != 67)
		{
			TeleportToLocation(67, 2, 20);
		}
		else
		{
			TeleportToLocation(68, 44, 19);
		}
	}

	private void TeleportPurple()
	{
		switch (Random.Range(0, 7))
		{
		case 0:
			TeleportToLocation(68, 49, 19);
			return;
		case 1:
			TeleportToLocation(68, 45, 38);
			return;
		case 2:
			TeleportToLocation(68, 22, 50);
			return;
		case 3:
			TeleportToLocation(68, 21, 34);
			return;
		}
		if (GameWorldController.instance.LevelNo != 64)
		{
			TeleportToLocation(64, 26, 13);
		}
		else
		{
			TeleportToLocation(68, 49, 19);
		}
	}

	private void TeleportWhite()
	{
		switch (Random.Range(0, 4))
		{
		case 0:
			TeleportToLocation(68, 13, 34);
			break;
		case 1:
			TeleportToLocation(68, 20, 28);
			break;
		case 2:
			TeleportToLocation(68, 22, 50);
			break;
		case 3:
			TeleportToLocation(68, 41, 4);
			break;
		}
	}

	private void LeaveArea()
	{
		TeleportWhite();
		Quest.instance.variables[107] = 0;
	}

	private void BeginQbertPyramid()
	{
		Quest.instance.variables[107] = 1;
	}

	private void StepOnPyramid(int tileX, int tileY)
	{
		int tileX2 = 0;
		int tileY2 = 0;
		getPreviousTileXY(out tileX2, out tileY2);
		if (tileX == tileX2 && tileY == tileY2)
		{
			return;
		}
		setPreviousTileXY(tileX, tileY);
		int nextColour = getNextColour(tileX, tileY);
		UWEBase.CurrentTileMap().Tiles[tileX, tileY].floorTexture = (short)nextColour;
		UWEBase.CurrentTileMap().Tiles[tileX, tileY].TileNeedsUpdate();
		GameObject obj = GameWorldController.FindTile(tileX, tileY, 1);
		Object.Destroy(obj);
		int[] colourSequence = getColourSequence();
		int num = -1;
		for (int i = 0; i <= colourSequence.GetUpperBound(0); i++)
		{
			if (CheckTileColours(colourSequence[i]))
			{
				num = colourSequence[i];
				break;
			}
		}
		if (num != -1)
		{
			SetPyramidWallColour(num);
			Debug.Log("Moongate spawned");
			UWEBase.CurrentObjectList().objInfo[974].instance.setInvis(0);
			switch (num)
			{
			case 2:
				UWEBase.CurrentObjectList().objInfo[973].instance.quality = 4;
				UWEBase.CurrentObjectList().objInfo[973].instance.owner = 16;
				break;
			case 4:
				UWEBase.CurrentObjectList().objInfo[973].instance.quality = 4;
				UWEBase.CurrentObjectList().objInfo[973].instance.owner = 28;
				break;
			case 3:
				UWEBase.CurrentObjectList().objInfo[973].instance.quality = 4;
				UWEBase.CurrentObjectList().objInfo[973].instance.owner = 22;
				break;
			case 0:
				UWEBase.CurrentObjectList().objInfo[973].instance.quality = 4;
				UWEBase.CurrentObjectList().objInfo[973].instance.owner = 4;
				break;
			case 6:
				UWEBase.CurrentObjectList().objInfo[973].instance.quality = 4;
				UWEBase.CurrentObjectList().objInfo[973].instance.owner = 40;
				break;
			case 5:
				UWEBase.CurrentObjectList().objInfo[973].instance.quality = 32;
				UWEBase.CurrentObjectList().objInfo[973].instance.owner = 25;
				break;
			}
			if (colourSequence.GetUpperBound(0) >= 4)
			{
				Quest.instance.variables[105] = 5;
				UWEBase.CurrentObjectList().objInfo[666].instance.setInvis(0);
			}
		}
		else
		{
			UWEBase.CurrentObjectList().objInfo[974].instance.setInvis(1);
			Debug.Log("Moongate despawned");
		}
	}

	private void SetPyramidWallColour(int colourToSet)
	{
		SetWallColour(44, 51, colourToSet);
		SetWallColour(45, 52, colourToSet);
		SetWallColour(46, 53, colourToSet);
		SetWallColour(47, 54, colourToSet);
		SetWallColour(48, 55, colourToSet);
		SetWallColour(49, 56, colourToSet);
		SetWallColour(45, 51, colourToSet);
		SetWallColour(46, 51, colourToSet);
		SetWallColour(47, 51, colourToSet);
		SetWallColour(48, 51, colourToSet);
		SetWallColour(49, 51, colourToSet);
		SetWallColour(46, 52, colourToSet);
		SetWallColour(47, 52, colourToSet);
		SetWallColour(48, 52, colourToSet);
		SetWallColour(49, 52, colourToSet);
		SetWallColour(47, 53, colourToSet);
		SetWallColour(48, 53, colourToSet);
		SetWallColour(49, 53, colourToSet);
		SetWallColour(48, 54, colourToSet);
		SetWallColour(49, 54, colourToSet);
		SetWallColour(49, 55, colourToSet);
		UWEBase.CurrentTileMap().SetTileMapWallFacesUW();
		DestroyTile(45, 51);
		DestroyTile(46, 51);
		DestroyTile(46, 52);
		DestroyTile(47, 51);
		DestroyTile(47, 52);
		DestroyTile(47, 53);
		DestroyTile(48, 51);
		DestroyTile(48, 52);
		DestroyTile(48, 53);
		DestroyTile(48, 54);
		DestroyTile(49, 51);
		DestroyTile(49, 52);
		DestroyTile(49, 53);
		DestroyTile(49, 54);
		DestroyTile(49, 55);
	}

	private void SetWallColour(int tileX, int tileY, int colourToSet)
	{
		UWEBase.CurrentTileMap().Tiles[tileX, tileY].wallTexture = (short)colourToSet;
	}

	private void DestroyTile(int tileX, int tileY)
	{
		UWEBase.CurrentTileMap().Tiles[tileX, tileY].TileNeedsUpdate();
		GameObject gameObject = GameWorldController.FindTile(tileX, tileY, 1);
		if (gameObject != null)
		{
			Object.Destroy(gameObject);
		}
	}

	private bool CheckTileColours(int ColourToTest)
	{
		int[] pyramidTileColours = getPyramidTileColours();
		for (int i = 0; i <= pyramidTileColours.GetUpperBound(0); i++)
		{
			if (pyramidTileColours[i] != ColourToTest)
			{
				return false;
			}
		}
		return true;
	}

	private int[] getPyramidTileColours()
	{
		return new int[15]
		{
			getFloorTexture(45, 51),
			getFloorTexture(46, 51),
			getFloorTexture(46, 52),
			getFloorTexture(47, 51),
			getFloorTexture(47, 52),
			getFloorTexture(47, 53),
			getFloorTexture(48, 51),
			getFloorTexture(48, 52),
			getFloorTexture(48, 53),
			getFloorTexture(48, 54),
			getFloorTexture(49, 51),
			getFloorTexture(49, 52),
			getFloorTexture(49, 53),
			getFloorTexture(49, 54),
			getFloorTexture(49, 55)
		};
	}

	private void getPreviousTileXY(out int tileX, out int tileY)
	{
		int num = Quest.instance.variables[108];
		tileX = num & 0x3F;
		tileY = (num >> 6) & 0x3F;
	}

	private void setPreviousTileXY(int tileX, int tileY)
	{
		Quest.instance.variables[108] = (tileY << 6) | tileX;
	}

	private int getTargetColour()
	{
		int result = 1;
		for (int i = 101; i <= 105 && Quest.instance.variables[i] != 255; i++)
		{
			result = Quest.instance.variables[i];
		}
		return result;
	}

	private int getNextColour(int tileX, int tileY)
	{
		int floorTexture = getFloorTexture(tileX, tileY);
		int[] colourSequence = getColourSequence();
		for (int i = 0; i <= colourSequence.GetUpperBound(0); i++)
		{
			if (colourSequence[i] == floorTexture)
			{
				if (i < colourSequence.GetUpperBound(0))
				{
					return colourSequence[i + 1];
				}
				return colourSequence[0];
			}
		}
		return 6;
	}

	private int getFloorTexture(int tileX, int tileY)
	{
		return UWEBase.CurrentTileMap().Tiles[tileX, tileY].floorTexture;
	}

	private int[] getColourSequence()
	{
		int num = 0;
		for (int i = 101; i <= 105; i++)
		{
			if (Quest.instance.variables[i] != 255)
			{
				num++;
			}
		}
		int[] array = new int[num + 1];
		array[num] = 6;
		for (int j = 101; j <= 105 && Quest.instance.variables[j] != 255; j++)
		{
			array[j - 101] = Quest.instance.variables[j];
		}
		return array;
	}

	private void TeleportToLocation(int levelNo, int tileX, int tileY)
	{
		if (GameWorldController.instance.LevelNo != levelNo)
		{
			UWCharacter.Instance.playerMotor.movement.velocity = Vector3.zero;
			GameWorldController.instance.SwitchLevel((short)levelNo, (short)tileX, (short)tileY);
			return;
		}
		float x = (float)tileX * 1.2f + 0.6f;
		float z = (float)tileY * 1.2f + 0.6f;
		float num = (float)UWEBase.CurrentTileMap().GetFloorHeight(tileX, tileY) * 0.15f;
		UWCharacter.Instance.transform.position = new Vector3(x, num + 0.3f, z);
		UWCharacter.Instance.TeleportPosition = UWCharacter.Instance.transform.position;
	}

	private bool FindMoongateInTile(int tileX, int tileY, out ObjectInteraction obj)
	{
		ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
		for (int i = 0; i < 1024; i++)
		{
			if (objInfo[i] != null && objInfo[i].GetItemType() == 96 && objInfo[i].ObjectTileX == tileX && objInfo[i].ObjectTileY == tileY)
			{
				obj = objInfo[i].instance;
				return true;
			}
		}
		obj = null;
		return false;
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
