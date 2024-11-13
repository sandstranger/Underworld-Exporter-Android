using UnityEngine;

public class SilverTree : object_base
{
	public override bool PickupEvent()
	{
		base.PickupEvent();
		return PickUpSeed();
	}

	public override bool use()
	{
		return PickUpSeed();
	}

	private bool PickUpSeed()
	{
		ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(290, 40, 16, 1, 256);
		objectLoaderInfo.is_quant = 1;
		ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.InventoryMarker.gameObject, UWEBase.CurrentTileMap().getTileVector(99, 99));
		GameWorldController.MoveToInventory(objectInteraction.gameObject);
		UWCharacter.Instance.ResurrectPosition = Vector3.zero;
		UWCharacter.Instance.ResurrectLevel = 0;
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 9));
		UWEBase.CurrentObjectInHand = objectInteraction;
		Character.InteractionMode = 2;
		InteractionModeControl.UpdateNow = true;
		objInt().consumeObject();
		return true;
	}
}
