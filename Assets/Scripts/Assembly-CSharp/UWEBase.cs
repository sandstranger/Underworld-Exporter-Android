using UnityEngine;

public class UWEBase : MonoBehaviour
{
	public const string GAME_UWDEMO = "UW0";

	public const string GAME_UW1 = "UW1";

	public const string GAME_UW2 = "UW2";

	public const string GAME_SHOCK = "SHOCK";

	public const string GAME_TNOVA = "TNOVA";

	public static string _RES = "UW1";

	public static bool EditorMode = false;

	public static char sep
	{
		get
		{
			return UWClass.sep;
		}
	}

	public static ObjectInteraction CurrentObjectInHand
	{
		get
		{
			if (UWCharacter.Instance != null && UWCharacter.Instance.playerInventory != null)
			{
				return UWCharacter.Instance.playerInventory.ObjectInHand;
			}
			return null;
		}
		set
		{
			UWCharacter.Instance.playerInventory.ObjectInHand = value;
		}
	}

	public virtual Vector3 GetImpactPoint()
	{
		return base.transform.position;
	}

	public static void FreezeMovement(GameObject myObj)
	{
		Rigidbody component = myObj.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.useGravity = false;
			component.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	public static void FreezeMovement(ObjectInteraction myObj)
	{
		FreezeMovement(myObj.gameObject);
	}

	public static void UnFreezeMovement(GameObject myObj)
	{
		Rigidbody component = myObj.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.useGravity = true;
			component.constraints = RigidbodyConstraints.FreezeRotation;
		}
	}

	public static void UnFreezeMovement(ObjectInteraction myObj)
	{
		UnFreezeMovement(myObj.gameObject);
	}

	public static ObjectLoader CurrentObjectList()
	{
		if (GameWorldController.instance.LevelNo == -1)
		{
			return null;
		}
		return GameWorldController.instance.objectList[GameWorldController.instance.LevelNo];
	}

	public static TileMap CurrentTileMap()
	{
		if (GameWorldController.instance.LevelNo == -1)
		{
			return null;
		}
		return GameWorldController.instance.Tilemaps[GameWorldController.instance.LevelNo];
	}

	public static AutoMap CurrentAutoMap()
	{
		if (GameWorldController.instance.LevelNo == -1)
		{
			return null;
		}
		return GameWorldController.instance.AutoMaps[GameWorldController.instance.LevelNo];
	}
}
