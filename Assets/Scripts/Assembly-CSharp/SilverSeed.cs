public class SilverSeed : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (objInt().PickedUp)
			{
				ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(458, 40, 16, 1, 256);
				objectLoaderInfo.is_quant = 1;
				ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, UWEBase.CurrentTileMap().getTileVector(TileMap.visitTileX, TileMap.visitTileY));
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 12));
				UWCharacter.Instance.ResurrectPosition = UWCharacter.Instance.transform.position;
				UWCharacter.Instance.ResurrectLevel = (short)(GameWorldController.instance.LevelNo + 1);
				objInt().consumeObject();
				return true;
			}
			return false;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override string UseVerb()
	{
		return "plant";
	}

	public override bool CanBePickedUp()
	{
		return true;
	}
}
