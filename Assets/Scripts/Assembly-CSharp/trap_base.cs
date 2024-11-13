using UnityEngine;

public class trap_base : traptrigger_base
{
	public bool ExecuteNow;

	public virtual void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Debug.Log("Base Execute Trap " + base.name);
	}

	public virtual bool Activate(object_base src, int triggerX, int triggerY, int State)
	{
		ExecuteTrap(src, triggerX, triggerY, State);
		TriggerNext(triggerX, triggerY, State);
		PostActivate(src);
		return true;
	}

	public virtual void TriggerNext(int triggerX, int triggerY, int State)
	{
		if (base.link == 0)
		{
			return;
		}
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (!(gameObjectAt != null))
		{
			return;
		}
		trigger_base component = gameObjectAt.GetComponent<trigger_base>();
		if (component != null)
		{
			component.Activate(base.gameObject);
			return;
		}
		trap_base component2 = gameObjectAt.GetComponent<trap_base>();
		if (component2 != null)
		{
			component2.Activate(this, triggerX, triggerY, State);
		}
	}

	public override bool WillFireRepeatedly()
	{
		return (base.flags & 1) == 1;
	}

	public override bool WillFire()
	{
		return true;
	}

	public virtual void PostActivate(object_base src)
	{
		if (!WillFireRepeatedly())
		{
			DestroyTrap(src);
		}
	}

	protected void DestroyTrap(object_base src)
	{
		if (src != null && src.GetComponent<ObjectInteraction>() != null && src.GetComponent<ObjectInteraction>().link == base.gameObject.GetComponent<ObjectInteraction>().objectloaderinfo.index)
		{
			src.GetComponent<ObjectInteraction>().link = 0;
		}
		objInt().objectloaderinfo.InUseFlag = 0;
		Debug.Log("Destroying Trap: " + base.name);
		Object.Destroy(base.gameObject);
	}

	public virtual bool FindTrapInChain(int link, int TrapType)
	{
		if (link != 0)
		{
			ObjectInteraction instance = UWEBase.CurrentObjectList().objInfo[link].instance;
			if (instance != null)
			{
				if (instance.GetItemType() == 48)
				{
					return TrapType == 48;
				}
				if (instance.GetItemType() == TrapType)
				{
					return true;
				}
				return FindTrapInChain(instance.link, TrapType);
			}
		}
		return false;
	}

	public override void Update()
	{
		if (ExecuteNow)
		{
			ExecuteNow = false;
			ExecuteTrap(this, 0, 0, 0);
		}
	}
}
