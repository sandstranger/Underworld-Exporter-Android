using UnityEngine;

public class trigger_base : traptrigger_base
{
	public bool TriggerMeNow = false;

	protected override void Start()
	{
		base.Start();
		base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	}

	public override bool Activate(GameObject src)
	{
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (gameObjectAt != null)
		{
			if (WillFire())
			{
				if (gameObjectAt.GetComponent<trap_base>() != null)
				{
					gameObjectAt.GetComponent<trap_base>().Activate(this, base.quality, base.owner, base.flags);
				}
			}
			else
			{
				Debug.Log(base.name + " will not trigger due to flag.");
			}
		}
		PostActivate(src);
		return true;
	}

	public virtual void PostActivate(GameObject src)
	{
		if (!WillFireRepeatedly())
		{
			if (src != null && src.GetComponent<ObjectInteraction>() != null && src.GetComponent<ObjectInteraction>().link == base.gameObject.GetComponent<ObjectInteraction>().objectloaderinfo.index)
			{
				src.GetComponent<ObjectInteraction>().link = 0;
			}
			objInt().objectloaderinfo.InUseFlag = 0;
			Object.Destroy(base.gameObject);
		}
	}

	public override void Update()
	{
		if (TriggerMeNow)
		{
			TriggerMeNow = false;
			Activate(base.gameObject);
		}
	}
}
