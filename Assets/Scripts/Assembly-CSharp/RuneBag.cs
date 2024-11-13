using UnityEngine;

public class RuneBag : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			return Activate(base.gameObject);
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	private void OpenRuneBag()
	{
		UWHUD.instance.RefreshPanels(2);
	}

	public override bool Activate(GameObject src)
	{
		OpenRuneBag();
		return true;
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed.GetComponent<RuneStone>() != null)
		{
			UWCharacter.Instance.PlayerMagic.PlayerRunes[ObjectUsed.item_id - 232] = true;
			ObjectUsed.consumeObject();
			UWEBase.CurrentObjectInHand = null;
			return true;
		}
		if (Character.InteractionMode == 5)
		{
			return ObjectUsed.FailMessage();
		}
		return false;
	}

	public override string UseVerb()
	{
		return "open";
	}

	public override string UseObjectOnVerb_Inv()
	{
		if (UWEBase.CurrentObjectInHand != null)
		{
			int itemType = UWEBase.CurrentObjectInHand.GetItemType();
			if (itemType == 6)
			{
				return "place rune in bag";
			}
		}
		return base.UseObjectOnVerb_Inv();
	}
}
