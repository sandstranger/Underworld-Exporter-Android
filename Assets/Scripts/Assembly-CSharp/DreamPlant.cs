public class DreamPlant : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null || UWEBase.CurrentObjectInHand == this)
		{
			return Eat();
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool Eat()
	{
		if (23 + UWCharacter.Instance.FoodLevel >= 255)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_are_too_full_to_eat_that_now_));
			return false;
		}
		UWCharacter.Instance.FoodLevel = 23 + UWCharacter.Instance.FoodLevel;
		Quest.instance.DreamPlantEaten = true;
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 248));
		objInt().consumeObject();
		return true;
	}
}
