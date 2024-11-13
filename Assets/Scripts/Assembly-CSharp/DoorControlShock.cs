using System.Collections;
using UnityEngine;

public class DoorControlShock : MonoBehaviour
{
	public bool locked;

	public int KeyIndex;

	private bool state;

	public bool DoorBusy;

	public int DoorSpriteIndex;

	public int NoOfFrames;

	private SpriteRenderer sc;

	public BoxCollider bc;

	private void Start()
	{
		sc = GetComponentInChildren<SpriteRenderer>();
		if (sc != null)
		{
			if (!state)
			{
				setSprite(0);
				AddDoorCollision();
			}
			else
			{
				setSprite(NoOfFrames - 1);
			}
		}
		locked = false;
	}

	private void setSprite(int index)
	{
	}

	public void Activate()
	{
		if (!state)
		{
			OpenDoor();
		}
		else
		{
			CloseDoor();
		}
	}

	private void OnMouseDown()
	{
		if (!locked)
		{
			if (!state)
			{
				OpenDoor();
			}
			else
			{
				CloseDoor();
			}
		}
		else
		{
			Debug.Log(base.name + " is locked");
		}
	}

	public void OpenDoor()
	{
		if (!DoorBusy)
		{
			Debug.Log("Move door to open position");
			RemoveDoorCollision();
			StartCoroutine(AnimateDoorOpen(1f));
			state = true;
		}
	}

	public void CloseDoor()
	{
		if (!DoorBusy)
		{
			Debug.Log("Move door to closed position");
			AddDoorCollision();
			StartCoroutine(AnimateDoorClose(1f));
			state = false;
		}
	}

	public void LockDoor()
	{
		Debug.Log("Locking door");
		locked = false;
	}

	public void UnlockDoor()
	{
		Debug.Log("Locking door");
		locked = true;
	}

	public void ToggleLock()
	{
		if (!locked)
		{
			locked = true;
		}
		else
		{
			locked = false;
		}
	}

	public void ToggleDoor()
	{
		if (!state)
		{
			UnlockDoor();
			OpenDoor();
		}
		else
		{
			CloseDoor();
			LockDoor();
		}
	}

	private IEnumerator AnimateDoorOpen(float traveltime)
	{
		float animationFrameTime = traveltime / (float)NoOfFrames;
		int currentFrame = 0;
		float nextFrameTime = animationFrameTime;
		setSprite(currentFrame++);
		for (float t = 0f; t < traveltime; t += Time.deltaTime / traveltime)
		{
			if (t >= nextFrameTime)
			{
				setSprite(currentFrame++);
				nextFrameTime += animationFrameTime;
			}
			yield return null;
		}
	}

	private IEnumerator AnimateDoorClose(float traveltime)
	{
		float animationFrameTime = traveltime / (float)NoOfFrames;
		int currentFrame = NoOfFrames - 1;
		float nextFrameTime = animationFrameTime;
		setSprite(currentFrame--);
		for (float t = 0f; t < traveltime; t += Time.deltaTime / traveltime)
		{
			if (t >= nextFrameTime)
			{
				setSprite(currentFrame--);
				nextFrameTime += animationFrameTime;
			}
			yield return null;
		}
	}

	private void AddDoorCollision()
	{
		if (bc == null)
		{
			bc = base.gameObject.AddComponent<BoxCollider>();
			bc.center = new Vector3(0f, 0.317f, 0f);
			bc.size = new Vector3(0.64f, 0.64f, 0.01f);
		}
	}

	private void RemoveDoorCollision()
	{
		if (bc != null)
		{
			Object.Destroy(bc);
		}
	}
}
