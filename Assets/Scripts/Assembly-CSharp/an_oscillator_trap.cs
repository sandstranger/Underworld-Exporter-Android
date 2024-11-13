using System.Collections;
using UnityEngine;

public class an_oscillator_trap : trap_base
{
	private GameObject platformTile;

	public Vector3 TileVector;

	public int TileXToWatch;

	public int TileYToWatch;

	public Vector3 ContactArea = new Vector3(0.59f, 0.15f, 0.59f);

	public Collider[] colliders;

	protected override void Start()
	{
		base.Start();
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		TileXToWatch = triggerX;
		TileYToWatch = triggerY;
		TileVector = UWEBase.CurrentTileMap().getTileVector(TileXToWatch, TileYToWatch);
		if (platformTile == null)
		{
			platformTile = GameWorldController.FindTile(triggerX, triggerY, 1);
		}
		if (!(platformTile == null))
		{
			if (UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight / 2 >= base.owner)
			{
				base.xpos = 0;
			}
			else if (UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight / 2 <= base.quality)
			{
				base.xpos = 1;
			}
			if (base.xpos == 1)
			{
				MoveTileUp(triggerX, triggerY);
				return;
			}
			UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight -= 2;
			StartCoroutine(MoveTile(platformTile.transform, new Vector3(0f, -0.3f, 0f), 0.1f));
		}
	}

	private void MoveTileUp(int triggerX, int triggerY)
	{
		UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight += 2;
		StartCoroutine(MoveTile(platformTile.transform, new Vector3(0f, 0.3f, 0f), 0.1f));
		if (UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight >= 30 && TileMap.visitTileX == triggerX && TileMap.visitTileY == triggerY)
		{
			UWCharacter.Instance.CurVIT -= 1000;
		}
	}

	protected IEnumerator MoveTile(Transform platform, Vector3 dist, float traveltime)
	{
		float rate = 1f / traveltime;
		float index = 0f;
		Vector3 StartPos = platform.position;
		Vector3 EndPos = StartPos + dist;
		base.transform.position = StartPos;
		TileVector = UWEBase.CurrentTileMap().getTileVector(TileXToWatch, TileYToWatch);
		colliders = Physics.OverlapBox(TileVector, ContactArea);
		while (index < 1f)
		{
			Vector3 OldPosition = platform.position;
			platform.position = Vector3.Lerp(StartPos, EndPos, index);
			float height = TileVector.y;
			MoveObjectsInContact(height);
			base.transform.position = platform.position;
			index += rate * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		platform.position = EndPos;
		base.transform.position = EndPos;
	}

	public void MoveObjectsInContact(float Height)
	{
		for (int i = 0; i <= colliders.GetUpperBound(0); i++)
		{
			if (colliders[i].gameObject.GetComponent<ObjectInteraction>() != null && colliders[i].gameObject.GetComponent<ObjectInteraction>().isMoveable())
			{
				Vector3 position = colliders[i].gameObject.transform.position;
				UWEBase.UnFreezeMovement(colliders[i].gameObject);
				colliders[i].gameObject.transform.position = new Vector3(position.x, Height, position.z);
			}
		}
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
