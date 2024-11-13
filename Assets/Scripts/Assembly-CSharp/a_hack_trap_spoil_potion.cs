using UnityEngine;

public class a_hack_trap_spoil_potion : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		foreach (Transform item in GameWorldController.instance.InventoryMarker.transform)
		{
			if (item.gameObject.GetComponent<ObjectInteraction>() != null)
			{
				ObjectInteraction component = item.gameObject.GetComponent<ObjectInteraction>();
				if (component.GetItemType() == 14 && component.link == 529 && component.enchantment == 1 && component.GetComponent<Potion>().linked == null)
				{
					SpoilPotion(component);
				}
			}
		}
	}

	private void SpoilPotion(ObjectInteraction obj)
	{
		obj.ChangeType(228);
		obj.isquant = 0;
		ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(384, 40, 1, 0, 256);
		objectLoaderInfo.InUseFlag = 1;
		ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, GameWorldController.instance.InventoryMarker.transform.position);
		GameWorldController.MoveToInventory(objectInteraction);
		objectInteraction.transform.parent = GameWorldController.instance.InventoryMarker.transform;
		obj.GetComponent<Potion>().linked = objectInteraction;
		obj.GetComponent<Potion>().SetDisplayEnchantment();
	}
}
