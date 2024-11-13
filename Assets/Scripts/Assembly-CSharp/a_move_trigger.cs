using UnityEngine;

public class a_move_trigger : trigger_base
{
	protected Vector3 boxDimensions = new Vector3(1.2f, 1.2f, 1.2f);

	protected Vector3 boxCenter = Vector3.zero;

	public bool playerStartedInTrigger = false;

	private BoxCollider box;

	protected override void Start()
	{
		base.Start();
		box = base.gameObject.GetComponent<BoxCollider>();
		if (box == null)
		{
			box = base.gameObject.AddComponent<BoxCollider>();
		}
		box.size = boxDimensions;
		box.center = boxCenter;
		box.isTrigger = true;
		CheckPlayerStart();
		if (base.ObjectTileX >= 63 || base.ObjectTileY >= 63 || base.link == 0)
		{
			return;
		}
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (gameObjectAt != null && gameObjectAt.GetComponent<a_teleport_trap>() != null && gameObjectAt.GetComponent<a_teleport_trap>().zpos != 0)
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				UWEBase.CurrentAutoMap().MarkTileDisplayType(base.ObjectTileX, base.ObjectTileY, 3);
			}
			else
			{
				UWEBase.CurrentAutoMap().MarkTileDisplayType(base.ObjectTileX, base.ObjectTileY, 12);
			}
		}
	}

	private void CheckPlayerStart()
	{
		Collider[] array = Physics.OverlapBox(base.transform.position, box.size / 2f);
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if (array[i].gameObject.GetComponent<UWCharacter>() != null || array[i].gameObject.GetComponent<Feet>() != null)
			{
				playerStartedInTrigger = true;
				break;
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (!playerStartedInTrigger && (other.name == UWCharacter.Instance.name || other.name == "Feet") && !UWEBase.EditorMode && !Quest.instance.InDreamWorld)
		{
			Activate(other.gameObject);
		}
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if ((other.name == UWCharacter.Instance.name || other.name == "Feet") && !UWEBase.EditorMode && !Quest.instance.InDreamWorld)
		{
			playerStartedInTrigger = false;
		}
	}

	public override bool Activate(GameObject src)
	{
		if (!WillFire())
		{
			Debug.Log(base.name + " will not fire due to flags");
			return false;
		}
		if (UWEBase._RES == "UW2" && GameWorldController.instance.LevelNo == 68)
		{
			ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
			for (int i = 0; i < 1024; i++)
			{
				if (objInfo[i] != null && objInfo[i].GetItemType() == 96 && objInfo[i].ObjectTileX == base.ObjectTileX && objInfo[i].ObjectTileY == base.ObjectTileY)
				{
					if (objInfo[i].instance.invis == 1)
					{
						return false;
					}
					break;
				}
			}
		}
		return base.Activate(base.gameObject);
	}
}
