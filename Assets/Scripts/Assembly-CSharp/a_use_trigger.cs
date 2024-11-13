using UnityEngine;

public class a_use_trigger : trigger_base
{
	public bool Activate(GameObject src, bool mode)
	{
		GameObject gameObject = null;
		gameObject = (mode ? ObjectLoader.getGameObjectAt(base.link) : ((base.next == 0) ? ObjectLoader.getGameObjectAt(base.link) : ObjectLoader.getGameObjectAt(base.next)));
		if (gameObject != null)
		{
			if (gameObject.GetComponent<trap_base>() != null)
			{
				gameObject.GetComponent<trap_base>().Activate(this, base.quality, base.owner, base.flags);
			}
			if (gameObject.GetComponent<trigger_base>() != null)
			{
				gameObject.GetComponent<trigger_base>().Activate(base.gameObject);
			}
		}
		PostActivate(src);
		return true;
	}
}
