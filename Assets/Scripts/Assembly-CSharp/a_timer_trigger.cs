using UnityEngine;

public class a_timer_trigger : trigger_base
{
	private float traptime = 0f;

	private float interval = 1f;

	protected override void Start()
	{
		base.Start();
		if (base.zpos != 0)
		{
			interval = base.zpos;
		}
		else
		{
			interval = 1f;
		}
	}

	public override void Update()
	{
		if (GameWorldController.instance.EnableTimerTriggers)
		{
			base.Update();
			traptime += Time.deltaTime;
			if (traptime > interval * GameWorldController.instance.TimerRate)
			{
				Activate(base.gameObject);
				traptime = 0f;
			}
		}
		else if (TriggerMeNow)
		{
			base.Update();
		}
	}

	public override bool Activate(GameObject src)
	{
		if (UWEBase.EditorMode)
		{
			return true;
		}
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (gameObjectAt != null && gameObjectAt.GetComponent<trap_base>() != null)
		{
			gameObjectAt.GetComponent<trap_base>().Activate(this, base.quality, base.owner, base.flags);
		}
		PostActivate(src);
		return true;
	}
}
