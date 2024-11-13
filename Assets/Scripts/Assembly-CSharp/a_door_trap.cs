using UnityEngine;

public class a_door_trap : trap_base
{
	public bool TriggerInstantly;

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		GameObject gameObject = DoorControl.findDoor(triggerX, triggerY);
		if (gameObject != null)
		{
			DoorControl component = gameObject.GetComponent<DoorControl>();
			switch (base.quality)
			{
			case 0:
				if (base.link != 0)
				{
					if (ObjectLoader.GetItemTypeAt(base.link) == 21)
					{
						ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
						ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(objectIntAt.item_id, objectIntAt.quality, objectIntAt.owner, objectIntAt.link, 256);
						objectLoaderInfo.flags = objectIntAt.flags;
						objectLoaderInfo.doordir = objectIntAt.doordir;
						objectLoaderInfo.invis = objectIntAt.invis;
						objectLoaderInfo.enchantment = objectIntAt.enchantment;
						objectLoaderInfo.zpos = objectIntAt.zpos;
						objectLoaderInfo.xpos = objectIntAt.xpos;
						objectLoaderInfo.ypos = objectIntAt.ypos;
						objectLoaderInfo.next = component.link;
						objectLoaderInfo.InUseFlag = 1;
						GameObject gameObject2 = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.InventoryMarker.gameObject, GameWorldController.instance.InventoryMarker.transform.position).gameObject;
						component.link = objectLoaderInfo.index;
					}
				}
				else
				{
					ObjectInteraction objectIntAt2 = ObjectLoader.getObjectIntAt(component.link);
					if (objectIntAt2 != null)
					{
						component.link = objectIntAt2.next;
						objectIntAt2.objectloaderinfo.InUseFlag = 0;
						Object.Destroy(objectIntAt2);
					}
				}
				break;
			case 1:
				if (TriggerInstantly)
				{
					component.UnlockDoor(false);
					component.OpenDoor(0f);
				}
				else
				{
					component.UnlockDoor(false);
					component.OpenDoor(1.3f);
				}
				break;
			case 2:
				if (TriggerInstantly)
				{
					component.CloseDoor(0f);
					component.LockDoor();
				}
				else
				{
					component.CloseDoor(1.3f);
					component.LockDoor();
				}
				break;
			case 3:
				if (TriggerInstantly)
				{
					component.ToggleDoor(0f, false);
				}
				else
				{
					component.ToggleDoor(1.3f, false);
				}
				break;
			}
		}
		else
		{
			Debug.Log("Door not found!");
		}
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
