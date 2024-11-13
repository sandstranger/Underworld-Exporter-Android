using UnityEngine;

public class a_hack_trap_terraform_puzzle : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (base.xpos != 0)
		{
			ChangeColumn(triggerX, triggerY, 0, base.xpos);
		}
		if (base.ypos != 0)
		{
			ChangeColumn(triggerX, triggerY, 1, base.ypos);
		}
		if (base.heading != 0)
		{
			ChangeColumn(triggerX, triggerY, 2, base.heading);
		}
		if (((uint)base.zpos & 7u) != 0)
		{
			ChangeColumn(triggerX, triggerY, 3, base.zpos & 7);
		}
		if (((uint)(base.zpos >> 3) & 7u) != 0)
		{
			ChangeColumn(triggerX, triggerY, 4, (base.zpos >> 3) & 7);
		}
	}

	public void ChangeColumn(int baseX, int baseY, int column, int bitfield)
	{
		int num = baseX + column * 3;
		for (int i = 0; i < 3; i++)
		{
			int num2 = (bitfield >> i) & 1;
			if (num2 != 1)
			{
				continue;
			}
			int num3 = baseY + i * 3;
			if (UWEBase.CurrentTileMap().Tiles[num, num3].floorHeight / 2 == base.owner - 2)
			{
				UWEBase.CurrentTileMap().Tiles[num, num3].floorHeight = 4;
			}
			else
			{
				UWEBase.CurrentTileMap().Tiles[num, num3].floorHeight = (short)((base.owner - 2) * 2);
				if (TileMap.visitTileX == num && TileMap.visitTileY == num3)
				{
					UWCharacter.Instance.transform.position = UWEBase.CurrentTileMap().getTileVector(num, num3);
				}
			}
			UWEBase.CurrentTileMap().Tiles[num, num3].TileNeedsUpdate();
			GameObject gameObject = GameWorldController.FindTile(num, num3, 1);
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
	}
}
