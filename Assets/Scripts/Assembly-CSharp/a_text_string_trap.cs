using UnityEngine;

public class a_text_string_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		int num = 0;
		string rES = UWEBase._RES;
		num = ((rES == null || !(rES == "UW2")) ? (64 * GameWorldController.instance.LevelNo + base.owner) : (32 * base.quality + base.owner));
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(9, num));
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}

	public override bool WillFireRepeatedly()
	{
		if (((base.flags >> 2) & 1) == 1)
		{
			return false;
		}
		return true;
	}
}
