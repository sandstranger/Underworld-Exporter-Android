using System.Collections;
using UnityEngine;

public class Impact : object_base
{
	protected override void Start()
	{
		base.Start();
		if (base.gameObject.GetComponent<Billboard>() == null)
		{
			base.gameObject.AddComponent<Billboard>();
		}
	}

	public void go(int StartFrame, int EndFrame)
	{
		StartCoroutine(Animate(StartFrame, EndFrame));
	}

	private void LoadAnimo(int index)
	{
		SpriteRenderer spriteRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		if (spriteRenderer == null)
		{
			spriteRenderer = base.gameObject.AddComponent<SpriteRenderer>();
		}
		spriteRenderer.sprite = GameWorldController.instance.TmAnimo.RequestSprite(index);
	}

	public IEnumerator Animate(int StartFrame, int EndFrame)
	{
		bool Active = true;
		LoadAnimo(StartFrame);
		while (Active)
		{
			yield return new WaitForSeconds(0.2f);
			StartFrame++;
			if (StartFrame >= EndFrame)
			{
				Active = false;
				Object.Destroy(base.gameObject);
			}
			else
			{
				LoadAnimo(StartFrame);
			}
		}
	}

	public static GameObject SpawnHitImpact(int Item_ID, Vector3 ImpactPosition, int StartFrame, int EndFrame)
	{
		ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(Item_ID, 40, StartFrame, 1, 256);
		if (objectLoaderInfo != null)
		{
			ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, ImpactPosition);
			objectInteraction.GetComponent<AnimationOverlay>().StartFrame = StartFrame;
			objectInteraction.GetComponent<AnimationOverlay>().NoOfFrames = EndFrame - StartFrame;
			objectInteraction.GetComponent<AnimationOverlay>().StartingDuration = objectInteraction.GetComponent<AnimationOverlay>().NoOfFrames;
			objectInteraction.gameObject.layer = LayerMask.NameToLayer("Animation");
			return objectInteraction.gameObject;
		}
		return null;
	}

	public static int ImpactBlood()
	{
		return 448;
	}

	public static int ImpactDamage()
	{
		return 459;
	}

	public static int ImpactMagic()
	{
		return 459;
	}
}
