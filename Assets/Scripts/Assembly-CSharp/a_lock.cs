using UnityEngine;

public class a_lock : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if ((base.flags & 1) == 1)
		{
			base.flags &= 14;
		}
		else
		{
			base.flags |= 1;
		}
	}

	public override void TriggerNext(int triggerX, int triggerY, int State)
	{
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
