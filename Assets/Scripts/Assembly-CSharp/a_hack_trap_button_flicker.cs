using UnityEngine;

public class a_hack_trap_button_flicker : a_hack_trap
{
	private ObjectLoaderInfo[] objList;

	protected override void Start()
	{
		base.Start();
		objList = UWEBase.CurrentObjectList().objInfo;
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (objList == null)
		{
			return;
		}
		for (int i = 256; i <= objList.GetUpperBound(0); i++)
		{
			if (objList[i].InUseFlag != 0 && (objList[i].item_id == 376 || objList[i].item_id == 368 || objList[i].item_id == 380 || objList[i].item_id == 372) && objList[i].link == 0 && objList[i].instance != null && Random.Range(0, 2) == 1 && objList[i].instance.GetComponent<ButtonHandler>() != null)
			{
				objList[i].instance.GetComponent<ButtonHandler>().Activate(base.gameObject);
			}
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
