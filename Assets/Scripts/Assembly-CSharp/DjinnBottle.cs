using UnityEngine;

public class DjinnBottle : object_base
{
	public override bool use()
	{
		if (objInt().PickedUp)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 141));
			return true;
		}
		return base.use();
	}

	public override bool ApplyAttack(short damage)
	{
		switch (Quest.instance.x_clocks[3])
		{
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
			DjinnKillsPlayer();
			break;
		case 5:
			if (IsAtSigil())
			{
				BindDjinn();
			}
			else
			{
				Debug.Log("Not at sigil");
			}
			break;
		default:
			Debug.Log("You broke another bottle!. I've yet to test this!");
			break;
		}
		objInt().consumeObject();
		return true;
	}

	private void BindDjinn()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 336));
		Quest.instance.x_clocks[3] = 6;
	}

	private void DjinnKillsPlayer()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 369));
		UWCharacter.Instance.ApplyDamage(UWCharacter.Instance.MaxVIT + 1);
	}

	private bool IsAtSigil()
	{
		if (GameWorldController.instance.LevelNo == 68)
		{
			int visitTileX = TileMap.visitTileX;
			int visitTileY = TileMap.visitTileY;
			if (visitTileX >= 18 && visitTileX <= 25 && visitTileY >= 49 && visitTileY <= 56)
			{
				return true;
			}
		}
		return false;
	}
}
