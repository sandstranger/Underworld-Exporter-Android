using UnityEngine;

public class forcefield : object_base
{
	private BoxCollider bx;

	protected override void Start()
	{
		base.Start();
		bx = base.gameObject.GetComponent<BoxCollider>();
		if (bx == null)
		{
			bx = base.gameObject.AddComponent<BoxCollider>();
		}
		bx.size = new Vector3(1.2f, 5f, 1.2f);
		bx.center = new Vector3(0f, 2.5f, 0f);
		base.gameObject.layer = LayerMask.NameToLayer("MapMesh");
	}

	public override void Update()
	{
		bx.enabled = true;
		if (UWEBase._RES == "UW2")
		{
			ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(4);
			if (objectIntAtSlot != null && objectIntAtSlot.item_id == 51)
			{
				bx.enabled = false;
			}
		}
	}
}
