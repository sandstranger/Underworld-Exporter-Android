using UnityEngine;

public class map_object : object_base
{
	public GameObject ModelInstance;

	protected override void Start()
	{
		base.Start();
		if (base.invis == 0)
		{
			foreach (Transform item in GameWorldController.instance.SceneryModel.transform)
			{
				if (item.name == base.name)
				{
					ModelInstance = item.gameObject;
					break;
				}
			}
			return;
		}
		base.gameObject.layer = LayerMask.NameToLayer("MapMesh");
	}

	public override void DestroyEvent()
	{
		base.DestroyEvent();
		Object.Destroy(ModelInstance);
	}
}
