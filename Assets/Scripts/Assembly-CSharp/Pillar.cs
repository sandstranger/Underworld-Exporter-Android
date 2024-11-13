using UnityEngine;

public class Pillar : map_object
{
	protected override void Start()
	{
		base.Start();
		BoxCollider component = GetComponent<BoxCollider>();
		if (component != null)
		{
			component.center = new Vector3(0f, 0f, 0f);
			component.size = new Vector3(0.1f, 12f, 0.1f);
		}
	}
}
