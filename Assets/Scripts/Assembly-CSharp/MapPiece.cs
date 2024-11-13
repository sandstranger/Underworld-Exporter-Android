using System;
using UnityEngine;

public class MapPiece : Map
{
	public override bool use()
	{
		int num = base.link & 0xFF;
		if (num >= 1)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 9));
			CopyMap(70 + base.quality, base.owner - 1);
			base.link &= 768;
		}
		else
		{
			OpenMap(base.owner - 1);
		}
		return true;
	}

	private void CopyMap(int SrcMap, int DstMap)
	{
		if (GameWorldController.instance.AutoMaps[SrcMap] == null || GameWorldController.instance.AutoMaps[DstMap] == null)
		{
			return;
		}
		AutoMap autoMap = GameWorldController.instance.AutoMaps[SrcMap];
		AutoMap autoMap2 = GameWorldController.instance.AutoMaps[DstMap];
		for (int i = 0; i <= 63; i++)
		{
			for (int j = 0; j <= 63; j++)
			{
				float angle = Mathf.Atan2(j - 31, i - 31) * 180f / (float)Math.PI;
				if ((TestAngleAgainstBitField(angle, base.link & 0xFF, i, j) || base.link >= 512) && autoMap.Tiles[i, j].DisplayType != 0)
				{
					autoMap2.Tiles[i, j].DisplayType = autoMap.Tiles[i, j].DisplayType;
					autoMap2.Tiles[i, j].tileType = autoMap.Tiles[i, j].tileType;
				}
			}
		}
		for (int k = 0; k < autoMap.MapNotes.Count; k++)
		{
			float angle2 = Mathf.Atan2(autoMap.MapNotes[k].PosY - 100, autoMap.MapNotes[k].PosX - 100) * 180f / (float)Math.PI;
			if (TestAngleAgainstBitField(angle2, base.link & 0xFF, (int)((float)autoMap.MapNotes[k].PosX * 0.32f), (int)((float)autoMap.MapNotes[k].PosY * 0.32f)) || base.link >= 512)
			{
				autoMap2.MapNotes.Add(new MapNote(autoMap.MapNotes[k].PosX, autoMap.MapNotes[k].PosY, autoMap.MapNotes[k].NoteText));
			}
		}
	}

	private bool TestAngleAgainstBitField(float angle, long bitfield, int x, int y)
	{
		if (angle < 0f)
		{
			angle += 360f;
		}
		float arcToTest = 45f;
		bool flag = false;
		if (x <= 40 && x >= 24 && y <= 40 && y >= 24)
		{
			flag = true;
		}
		if (bitfield >= 512)
		{
			return true;
		}
		if ((bitfield & 1) == 1)
		{
			return flag;
		}
		if (TestAngle(angle, 0f, 45f, bitfield, 1))
		{
			return !flag;
		}
		if (TestAngle(angle, 45f, 60f, bitfield, 2))
		{
			return !flag;
		}
		if (TestAngle(angle, 105f, 45f, bitfield, 3))
		{
			return !flag;
		}
		if (TestAngle(angle, 150f, 60f, bitfield, 4))
		{
			return !flag;
		}
		if (TestAngle(angle, 210f, 45f, bitfield, 5))
		{
			return !flag;
		}
		if (TestAngle(angle, 245f, 75f, bitfield, 6))
		{
			return !flag;
		}
		if (TestAngle(angle, 320f, 40f, bitfield, 7))
		{
			return !flag;
		}
		if (TestAngle(angle, 315f, arcToTest, bitfield, 8))
		{
			return !flag;
		}
		return false;
	}

	private bool TestAngle(float angleToTest, float toTestAgainst, float ArcToTest, long bitfield, int shiftBit)
	{
		if (angleToTest >= toTestAgainst && angleToTest < toTestAgainst + ArcToTest)
		{
			return ((bitfield >> shiftBit) & 1) == 1;
		}
		return false;
	}
}
