using UnityEngine;

public class a_do_trap_EndGame : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Debug.Log(base.name);
		Cutscene_EndGame cs = UWHUD.instance.gameObject.AddComponent<Cutscene_EndGame>();
		UWHUD.instance.CutScenesFull.cs = cs;
		UWHUD.instance.CutScenesFull.Begin();
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
