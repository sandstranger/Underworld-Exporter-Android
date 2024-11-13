using UnityEngine;

public class a_proximity_trap : trap_base
{
	protected Vector3 boxDimensions = new Vector3(1.2f, 1.2f, 1.2f);

	protected Vector3 boxCenter = Vector3.zero;

	private BoxCollider box;

	protected override void Start()
	{
		boxDimensions = new Vector3((float)base.quality * 1.2f, 0.2f, (float)base.owner * 1.2f);
		boxCenter = new Vector3((float)base.quality * 0.6f, 0f, (float)base.owner * 0.6f);
		box = base.gameObject.GetComponent<BoxCollider>();
		if (box == null)
		{
			box = base.gameObject.AddComponent<BoxCollider>();
		}
		box.size = boxDimensions;
		box.center = boxCenter;
		box.isTrigger = true;
		base.Start();
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if ((other.name == UWCharacter.Instance.name || other.name == "Feet") && !UWEBase.EditorMode && !Quest.instance.InDreamWorld)
		{
			ExecuteTrap(this, base.owner, base.quality, base.flags);
		}
	}
}
