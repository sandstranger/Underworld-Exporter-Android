using UnityEngine;

public class a_hack_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Debug.Log("Hack Trap " + objInt().objectloaderinfo.index + " qual = " + base.quality + " triggers:" + triggerX + "," + triggerY);
	}

	protected override void Start()
	{
		if (GameWorldController.instance.LevelNo != 0 && GameWorldController.instance.LevelNo != 16 && base.quality == 62)
		{
			Debug.Log("oh hey another instance of that hack trap I'm trying to figure out");
		}
	}
}
