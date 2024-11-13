using UnityEngine;

public class a_scheduled_trigger : trigger_base
{
	public override bool Activate(GameObject src)
	{
		Debug.Log("scheduled trigger " + base.name);
		return base.Activate(base.gameObject);
	}
}
