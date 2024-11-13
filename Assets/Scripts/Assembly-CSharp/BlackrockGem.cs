public class BlackrockGem : object_base
{
	public override bool LookAt()
	{
		string whatToSay = ((base.owner != 1) ? (StringController.instance.GetString(1, StringController.str_you_see_) + " a " + StringController.instance.GetString(1, 356) + StringController.instance.GetSimpleObjectNameUW(objInt())) : (StringController.instance.GetString(1, StringController.str_you_see_) + " a " + StringController.instance.GetString(1, 357) + StringController.instance.GetSimpleObjectNameUW(objInt())));
		UWHUD.instance.MessageScroll.Add(whatToSay);
		return true;
	}
}
