using UnityEngine;

public class TMAP : object_base
{
	public int TextureIndex;

	protected override void Start()
	{
		base.Start();
		TextureIndex = UWEBase.CurrentTileMap().texture_map[base.owner];
		CreateTMAP(base.gameObject, TextureIndex);
	}

	public override bool LookAt()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.TextureDescription(TextureIndex));
		if (TextureIndex == 142 && (UWEBase._RES == "UW1" || UWEBase._RES == "UW0"))
		{
			UWHUD.instance.CutScenesSmall.anim.SetAnimation = "VolcanoWindow_" + GameWorldController.instance.LevelNo;
			UWHUD.instance.CutScenesSmall.anim.looping = true;
		}
		if (base.link != 0)
		{
			GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
			if (gameObjectAt != null)
			{
				ObjectInteraction component = gameObjectAt.GetComponent<ObjectInteraction>();
				if (component.GetItemType() == 57 || component.GetItemType() == 56)
				{
					component.GetComponent<trigger_base>().Activate(base.gameObject);
					return true;
				}
			}
		}
		return true;
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (base.link != 0)
			{
				GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
				if (gameObjectAt != null)
				{
					ObjectInteraction component = gameObjectAt.GetComponent<ObjectInteraction>();
					if (component.GetItemType() == 56)
					{
						component.GetComponent<trigger_base>().Activate(base.gameObject);
						return true;
					}
				}
			}
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (UWEBase._RES == "UW1" && TextureIndex == 47 && ObjectUsed.item_id == 231 && base.link != 0)
		{
			GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
			if (gameObjectAt != null)
			{
				ObjectInteraction component = gameObjectAt.GetComponent<ObjectInteraction>();
				if (component.GetItemType() == 59)
				{
					component.GetComponent<trigger_base>().Activate(base.gameObject);
					UWEBase.CurrentObjectInHand = null;
					return true;
				}
			}
		}
		return base.ActivateByObject(ObjectUsed);
	}

	public override string ContextMenuDesc(int item_id)
	{
		return "";
	}

	public override string ContextMenuUsedDesc()
	{
		return "";
	}

	public override string ContextMenuUsedPickup()
	{
		return "";
	}

	public override string UseObjectOnVerb_World()
	{
		if (TextureIndex == 47 && UWEBase.CurrentObjectInHand != null)
		{
			int num = UWEBase.CurrentObjectInHand.item_id;
			if (num == 231)
			{
				return "unlock the shrine";
			}
		}
		return base.UseObjectOnVerb_Inv();
	}

	public static void CreateTMAP(GameObject myObj, int textureIndex)
	{
		if (myObj.transform.childCount > 0)
		{
			Object.Destroy(myObj.transform.GetChild(0).gameObject);
		}
		ObjectInteraction component = myObj.GetComponent<ObjectInteraction>();
		float x = 0f;
		float z = 0f;
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		gameObject.name = "_quad";
		gameObject.transform.position = myObj.transform.position;
		gameObject.layer = LayerMask.NameToLayer("UWObjects");
		gameObject.transform.parent = myObj.transform;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
		gameObject.transform.localPosition = new Vector3(x, 0.6f, z);
		MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
		component2.material = GameWorldController.instance.MaterialMasterList[textureIndex];
		BoxCollider boxCollider = myObj.GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = myObj.AddComponent<BoxCollider>();
		}
		boxCollider.size = new Vector3(1.25f, 1.25f, 0.1f);
		boxCollider.center = new Vector3(0f, 0.65f, 0f);
		if (component.GetItemType() == 35)
		{
			boxCollider.isTrigger = true;
		}
	}
}
