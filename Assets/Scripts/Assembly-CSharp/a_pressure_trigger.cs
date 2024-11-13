using UnityEngine;

public class a_pressure_trigger : trigger_base
{
	public int TileXToWatch;

	public int TileYToWatch;

	public Collider[] colliders;

	public bool IsReleaseTrigger;

	public a_door_trap door;

	public float WeightOnTrigger;

	public float PreviousWeightOnTrigger;

	private Vector3 TileVector;

	public Vector3 ContactArea = new Vector3(0.4f, 0.1f, 0.4f);

	protected override void Start()
	{
		base.Start();
		IsReleaseTrigger = base.item_id == 437 || base.item_id == 421;
		TileXToWatch = base.ObjectTileX;
		TileYToWatch = base.ObjectTileY;
		TileVector = UWEBase.CurrentTileMap().getTileVector(TileXToWatch, TileYToWatch);
		TileVector = new Vector3(TileVector.x, base.transform.position.y, TileVector.z);
		UWEBase.CurrentTileMap().Tiles[TileXToWatch, TileYToWatch].PressureTriggerIndex = (short)objInt().objectloaderinfo.index;
		colliders = Physics.OverlapBox(TileVector, new Vector3(0.4f, 0.1f, 0.4f));
		WeightOnTrigger = 0f;
		for (int i = 0; i <= colliders.GetUpperBound(0); i++)
		{
			if (colliders[i].gameObject.GetComponent<ObjectInteraction>() != null)
			{
				WeightOnTrigger += colliders[i].gameObject.GetComponent<ObjectInteraction>().GetWeight();
			}
			else if (colliders[i].gameObject.GetComponent<UWCharacter>() != null)
			{
				WeightOnTrigger += 5000f;
			}
		}
		PreviousWeightOnTrigger = WeightOnTrigger;
		if (UWEBase.CurrentObjectList().objInfo[base.link].GetItemType() == 45)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
			if (objectIntAt != null)
			{
				door = objectIntAt.GetComponent<a_door_trap>();
			}
		}
	}

	public override void Update()
	{
		base.Update();
		colliders = Physics.OverlapBox(TileVector, ContactArea);
		WeightOnTrigger = 0f;
		for (int i = 0; i <= colliders.GetUpperBound(0); i++)
		{
			if (colliders[i].gameObject.GetComponent<ObjectInteraction>() != null)
			{
				WeightOnTrigger += colliders[i].gameObject.GetComponent<ObjectInteraction>().GetWeight();
			}
			else if (colliders[i].gameObject.GetComponent<UWCharacter>() != null || colliders[i].gameObject.GetComponent<Feet>() != null)
			{
				WeightOnTrigger += 5000f;
			}
		}
		if (IsReleaseTrigger)
		{
			if (WeightOnTrigger < 1f && PreviousWeightOnTrigger >= 1f)
			{
				ReleaseWeightFrom();
			}
			else if (WeightOnTrigger >= 1f && PreviousWeightOnTrigger < 1f && !GameWorldController.WorldReRenderPending)
			{
				UpdateTileTexture(8);
			}
		}
		else if (WeightOnTrigger >= 1f && PreviousWeightOnTrigger < 1f)
		{
			PutWeightOn();
		}
		else if (WeightOnTrigger <= 1f && PreviousWeightOnTrigger > 1f && !GameWorldController.WorldReRenderPending)
		{
			UpdateTileTexture(7);
		}
		PreviousWeightOnTrigger = WeightOnTrigger;
	}

	public void PutWeightOn()
	{
		UpdateTileTexture(8);
		if (door != null)
		{
			door.TriggerInstantly = true;
		}
		Activate(base.gameObject);
		if (door != null)
		{
			door.TriggerInstantly = false;
		}
	}

	public void ReleaseWeightFrom()
	{
		UpdateTileTexture(7);
		if (door != null)
		{
			door.TriggerInstantly = true;
		}
		Activate(base.gameObject);
		if (door != null)
		{
			door.TriggerInstantly = false;
		}
	}

	public void UpdateTileTexture(int newTexture)
	{
		if (base.xpos != 3)
		{
			UWEBase.CurrentTileMap().Tiles[TileXToWatch, TileYToWatch].floorTexture = (short)newTexture;
			UWEBase.CurrentTileMap().Tiles[TileXToWatch, TileYToWatch].TileNeedsUpdate();
			GameObject gameObject = GameWorldController.FindTile(TileXToWatch, TileYToWatch, 1);
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
	}
}
