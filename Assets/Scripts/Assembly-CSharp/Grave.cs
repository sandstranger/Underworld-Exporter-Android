using System.Linq;
using UnityEngine;

public class Grave : Model3D
{
	private bool LookingAt;

	private float timeOut = 0f;

	public override void Update()
	{
		if (LookingAt)
		{
			timeOut += Time.deltaTime;
			if (timeOut >= 5f)
			{
				LookingAt = false;
				UWHUD.instance.CutScenesSmall.anim.SetAnimation = "Anim_Base";
			}
		}
	}

	public override bool LookAt()
	{
		UWHUD.instance.CutScenesSmall.anim.SetAnimation = "Grave_" + GraveID();
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(8, base.link - 512));
		return true;
	}

	public int GraveID()
	{
		if (UWEBase._RES != "UW2")
		{
			char[] buffer;
			DataLoader.ReadStreamFile(Loader.BasePath + "DATA" + UWEBase.sep + "GRAVE.DAT", out buffer);
			if (base.link >= 512)
			{
				return (short)DataLoader.getValAtAddress(buffer, base.link - 512, 8) - 1;
			}
		}
		return 0;
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			return LookAt();
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (GraveID() == 5)
		{
			if (ObjectUsed.item_id == 198)
			{
				if (ObjectUsed.quality == 63)
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_thoughtfully_give_the_bones_a_final_resting_place_));
					ObjectInteraction instance = UWEBase.CurrentObjectList().objInfo[495].instance;
					if (instance != null)
					{
						base.link++;
						ObjectUsed.consumeObject();
						instance.GetComponent<trigger_base>().Activate(base.gameObject);
						Quest.instance.isGaramonBuried = true;
						UWEBase.CurrentObjectInHand = null;
						GameObject gameObject = GameObject.Find(a_create_object_trap.LastObjectCreated);
						if (gameObject != null && gameObject.GetComponent<NPC>() != null)
						{
							gameObject.GetComponent<NPC>().TalkTo();
						}
					}
					UWEBase.CurrentObjectInHand = null;
					return true;
				}
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 259));
				UWEBase.CurrentObjectInHand = null;
				return true;
			}
			return ObjectUsed.FailMessage();
		}
		if (ObjectUsed.item_id == 198 && ObjectUsed.quality == 63)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 259));
			UWEBase.CurrentObjectInHand = null;
			return true;
		}
		UWEBase.CurrentObjectInHand = null;
		return ObjectUsed.FailMessage();
	}

	public override string UseObjectOnVerb_World()
	{
		if (UWEBase.CurrentObjectInHand != null)
		{
			int num = UWEBase.CurrentObjectInHand.item_id;
			if (num == 198)
			{
				return "bury remains";
			}
		}
		return base.UseObjectOnVerb_Inv();
	}

	public override int NoOfMeshes()
	{
		return 2;
	}

	public override Vector3[] ModelVertices()
	{
		return new Vector3[8]
		{
			new Vector3(-0.125f, 0f, -1f / 64f),
			new Vector3(-0.125f, 0.375f, -1f / 64f),
			new Vector3(0.125f, 0.375f, -1f / 64f),
			new Vector3(0.125f, 0f, -1f / 64f),
			new Vector3(-0.125f, 0f, 1f / 64f),
			new Vector3(-0.125f, 0.375f, 1f / 64f),
			new Vector3(0.125f, 0.375f, 1f / 64f),
			new Vector3(0.125f, 0f, 1f / 64f)
		};
	}

	public override int[] ModelTriangles(int meshNo)
	{
		switch (meshNo)
		{
		case 0:
			return new int[12]
			{
				0, 2, 1, 0, 3, 2, 4, 5, 6, 4,
				6, 7
			}.Reverse().ToArray();
		case 1:
			return new int[24]
			{
				0, 1, 4, 4, 1, 5, 3, 7, 6, 3,
				6, 2, 5, 1, 2, 5, 2, 6, 0, 4,
				7, 0, 7, 3
			}.Reverse().ToArray();
		default:
			return base.ModelTriangles(meshNo);
		}
	}

	public override Material ModelMaterials(int meshNo)
	{
		switch (meshNo)
		{
		case 0:
			return GameWorldController.instance.MaterialObj[base.flags + 28];
		case 1:
			return GameWorldController.instance.MaterialObj[base.flags + 28];
		default:
			return base.ModelMaterials(meshNo);
		}
	}

	public override Vector2[] ModelUVs(Vector3[] verts)
	{
		return new Vector2[8]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
	}
}
