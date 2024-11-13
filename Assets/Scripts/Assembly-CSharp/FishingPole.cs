using UnityEngine;

public class FishingPole : object_base
{
	public override bool use()
	{
		if (objInt().PickedUp)
		{
			if (UWEBase.CurrentObjectInHand == null)
			{
				GoFish();
				return true;
			}
			return FailMessage();
		}
		return false;
	}

	private void GoFish()
	{
		int num = (int)(UWCharacter.Instance.transform.position.x / 1.2f);
		int num2 = (int)(UWCharacter.Instance.transform.position.z / 1.2f);
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (!UWEBase.CurrentTileMap().Tiles[num + i, num2 + j].isWater)
				{
					continue;
				}
				if (Random.Range(0, 10) >= 7)
				{
					if ((float)GameWorldController.instance.commonObject.properties[182].mass * 0.1f <= UWCharacter.Instance.playerInventory.getEncumberance())
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_catch_a_lovely_fish_));
						ObjectInteraction currentObjectInHand = CreateFish();
						UWEBase.CurrentObjectInHand = currentObjectInHand;
						Character.InteractionMode = 2;
						InteractionModeControl.UpdateNow = true;
					}
					else
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_feel_a_nibble_but_the_fish_gets_away_));
					}
				}
				else
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_no_luck_this_time_));
				}
				return;
			}
		}
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_cannot_fish_there__perhaps_somewhere_else_));
	}

	private ObjectInteraction CreateFish()
	{
		ObjectLoaderInfo currObj = ObjectLoader.newObject(182, 40, 0, 1, 256);
		ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.InventoryMarker.gameObject, GameWorldController.instance.InventoryMarker.transform.position);
		objectInteraction.gameObject.name = ObjectLoader.UniqueObjectName(currObj);
		objectInteraction.isquant = 1;
		GameWorldController.MoveToInventory(objectInteraction.gameObject);
		return objectInteraction;
	}
}
