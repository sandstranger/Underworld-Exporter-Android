public class Lantern : LightSource
{
	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed != null)
		{
			int itemType = ObjectUsed.GetItemType();
			if (itemType == 89)
			{
				if (IsOn())
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_think_it_is_a_bad_idea_to_add_oil_to_the_lit_lantern_));
				}
				else if (base.quality == 64)
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_lantern_is_already_full_));
				}
				else
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_adding_oil_you_refuel_the_lantern_));
					base.quality = 64;
					ObjectUsed.consumeObject();
				}
				UWEBase.CurrentObjectInHand = null;
				return true;
			}
			return base.ActivateByObject(ObjectUsed);
		}
		return false;
	}

	public override string UseObjectOnVerb_Inv()
	{
		ObjectInteraction currentObjectInHand = UWEBase.CurrentObjectInHand;
		if (currentObjectInHand != null)
		{
			int itemType = currentObjectInHand.GetItemType();
			if (itemType == 89)
			{
				return "refill lantern";
			}
		}
		return base.UseObjectOnVerb_Inv();
	}
}
