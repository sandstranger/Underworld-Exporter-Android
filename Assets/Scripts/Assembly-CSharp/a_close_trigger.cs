using UnityEngine;

public class a_close_trigger : trigger_base
{
	public override bool Activate(GameObject src)
	{
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (gameObjectAt != null && gameObjectAt.GetComponent<trap_base>() != null)
		{
			gameObjectAt.GetComponent<trap_base>().Activate(this, base.quality, base.owner, base.flags);
		}
		if (ObjectLoader.GetItemTypeAt(base.next) != 59)
		{
			gameObjectAt = ObjectLoader.getGameObjectAt(base.next);
			if (gameObjectAt != null && gameObjectAt.GetComponent<trigger_base>() != null)
			{
				gameObjectAt.GetComponent<trigger_base>().Activate(base.gameObject);
			}
		}
		PostActivate(src);
		return true;
	}

	public override void PostActivate(GameObject src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
