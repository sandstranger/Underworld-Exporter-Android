using UnityEngine;

public class TileContact : UWEBase
{
	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<ObjectInteraction>() != null)
		{
			TileContactEvent(collision.gameObject.GetComponent<ObjectInteraction>(), collision.contacts[0].point);
		}
	}

	protected virtual void TileContactEvent(ObjectInteraction obj, Vector3 position)
	{
	}

	public bool IsObjectDestroyable(ObjectInteraction obj)
	{
		int[] array = new int[34]
		{
			453, 10, 50, 47, 54, 55, 151, 147, 280, 281,
			275, 272, 273, 276, 258, 259, 260, 261, 262, 263,
			265, 265, 266, 267, 268, 269, 270, 285, 191, 175,
			287, 290, 143, 315
		};
		if (ObjectLoader.isStatic(obj.objectloaderinfo))
		{
			return false;
		}
		if (obj.item_id >= 64 && obj.item_id <= 127)
		{
			return false;
		}
		if (obj.GetComponent<Rigidbody>() != null && !obj.GetComponent<Rigidbody>().useGravity)
		{
			return false;
		}
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if (obj.item_id == array[i])
			{
				return false;
			}
		}
		if (obj.GetComponent<Container>() != null)
		{
			Container component = obj.GetComponent<Container>();
			for (int j = 0; j <= array.GetUpperBound(0); j++)
			{
				if (component.findItemOfType(array[j]) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	protected virtual void DestroyObject(ObjectInteraction obj)
	{
		obj.objectloaderinfo.InUseFlag = 0;
		Object.Destroy(obj.gameObject);
	}
}
