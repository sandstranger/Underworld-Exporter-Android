using UnityEngine;

public class a_change_to_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Debug.Log(base.name + "triggerX = " + triggerX + " triggerY = " + triggerY);
	}
}
