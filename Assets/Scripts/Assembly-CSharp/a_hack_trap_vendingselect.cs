using UnityEngine;

public class a_hack_trap_vendingselect : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Quest.instance.variables[base.owner]++;
		if (Quest.instance.variables[base.owner] >= 8)
		{
			Quest.instance.variables[base.owner] = 0;
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
