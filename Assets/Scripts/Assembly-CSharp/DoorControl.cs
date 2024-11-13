using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DoorControl : object_base
{
	public int KeyIndex;

	public bool DoorBusy;

	public bool Pickable = true;

	public bool PlayerUse = false;

	private const int vTOP = 0;

	private const int vEAST = 1;

	private const int vBOTTOM = 2;

	private const int vWEST = 3;

	private const int vNORTH = 4;

	private const int vSOUTH = 5;

	private const int NORTH = 180;

	private const int SOUTH = 0;

	private const int EAST = 270;

	private const int WEST = 90;

	private const int OpenRotation = -90;

	private const int CloseRotation = 90;

	public const float DefaultDoorTravelTime = 1.3f;

	protected override void Start()
	{
		ObjectInteraction lockObjInt = getLockObjInt();
		if (lockObjInt != null)
		{
			KeyIndex = lockObjInt.link & 0x3F;
		}
		if (!state())
		{
			return;
		}
		if (!isPortcullis())
		{
			StartCoroutine(RotateDoor(base.transform, Vector3.up * doordirection() * -90f, 0.01f));
			return;
		}
		StartCoroutine(RaiseDoor(base.transform, new Vector3(0f, 1.1f, 0f), 0.1f));
		NavMeshObstacle component = GetComponent<NavMeshObstacle>();
		if (component != null)
		{
			component.enabled = !state();
		}
	}

	public bool isPortcullis()
	{
		int num = base.item_id;
		if (num == 326 || num == 334)
		{
			return true;
		}
		return false;
	}

	private ObjectInteraction getLockObjInt()
	{
		if (base.link == 0)
		{
			return null;
		}
		if (ObjectLoader.GetItemTypeAt(base.link) == 21)
		{
			return ObjectLoader.getObjectIntAt(base.link);
		}
		return null;
	}

	public bool state()
	{
		switch (base.item_id)
		{
		case 320:
		case 321:
		case 322:
		case 323:
		case 324:
		case 325:
		case 326:
		case 327:
			return false;
		default:
			return true;
		}
	}

	public bool locked()
	{
		ObjectInteraction lockObjInt = getLockObjInt();
		if (lockObjInt != null)
		{
			return (lockObjInt.flags & 1) == 1;
		}
		return false;
	}

	public override bool use()
	{
		trigger_base trigger_base2 = null;
		if (base.link != 0)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
			if (objectIntAt != null && objectIntAt.GetItemType() != 21)
			{
				trigger_base2 = objectIntAt.GetComponent<trigger_base>();
			}
		}
		if (UWEBase.CurrentObjectInHand != null)
		{
			ActivateByObject(UWEBase.CurrentObjectInHand);
			UWEBase.CurrentObjectInHand = null;
			if (trigger_base2 != null)
			{
				trigger_base2.Activate(base.gameObject);
			}
			return true;
		}
		PlayerUse = true;
		if (Character.AutoKeyUse && locked())
		{
			foreach (Transform item in GameWorldController.instance.InventoryMarker.transform)
			{
				if (!(item.gameObject.GetComponent<DoorKey>() != null))
				{
					continue;
				}
				DoorKey component = item.gameObject.GetComponent<DoorKey>();
				if (component.KeyId == KeyIndex)
				{
					ActivateByObject(component.objInt());
					if (trigger_base2 != null)
					{
						trigger_base2.Activate(base.gameObject);
					}
					PlayerUse = false;
					return true;
				}
			}
		}
		Activate(base.gameObject);
		PlayerUse = false;
		return true;
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed != null)
		{
			switch (ObjectUsed.GetItemType())
			{
			case 5:
			{
				DoorKey component = ObjectUsed.GetComponent<DoorKey>();
				if (!(component != null))
				{
					break;
				}
				if (state())
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 6));
					return true;
				}
				ObjectInteraction lockObjInt = getLockObjInt();
				if (lockObjInt == null)
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 3));
					return false;
				}
				if ((lockObjInt.link & 0x3F) == component.owner)
				{
					ToggleLock(true);
					if (locked())
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 4));
					}
					else
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 5));
					}
					return true;
				}
				if (base.link == 53)
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 3));
				}
				else
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 2));
				}
				return true;
			}
			case 79:
				if (Pickable)
				{
					if (UWCharacter.Instance.PlayerSkills.TrySkill(17, Skills.DiceRoll(1, 25)))
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_succeed_in_picking_the_lock_));
						UnlockDoor(true);
					}
					else
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_your_lockpicking_attempt_failed_));
					}
				}
				else
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_your_lockpicking_attempt_failed_));
				}
				break;
			case 87:
				if (Spike())
				{
					ObjectUsed.consumeObject();
				}
				break;
			default:
				return false;
			}
			return true;
		}
		return false;
	}

	public bool Spiked()
	{
		return base.owner == 63;
	}

	public bool Spike()
	{
		if (!Spiked())
		{
			if (!state())
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 129));
				base.owner = 63;
				return true;
			}
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 128));
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 131));
		}
		return false;
	}

	public override bool Activate(GameObject src)
	{
		if (!locked())
		{
			if (!state())
			{
				OpenDoor(1.3f);
			}
			else
			{
				CloseDoor(1.3f);
			}
		}
		else if (PlayerUse)
		{
			UWHUD.instance.MessageScroll.Add("The " + StringController.instance.GetObjectNounUW(objInt()) + " is locked.");
		}
		return true;
	}

	public void OpenDoor(float DoorTravelTime)
	{
		if (state() || DoorBusy)
		{
			return;
		}
		if (!isPortcullis())
		{
			if (ObjectInteraction.PlaySoundEffects)
			{
				objInt().aud.clip = MusicController.instance.SoundEffects[11];
				objInt().aud.Play();
			}
			StartCoroutine(RotateDoor(base.transform, Vector3.up * doordirection() * -90f, DoorTravelTime));
		}
		else
		{
			if (ObjectInteraction.PlaySoundEffects)
			{
				objInt().aud.clip = MusicController.instance.SoundEffects[20];
				objInt().aud.Play();
			}
			StartCoroutine(RaiseDoor(base.transform, new Vector3(0f, 1.1f, 0f), DoorTravelTime));
		}
		base.owner = 0;
		base.item_id += 8;
		base.zpos += 24;
		base.enchantment = 1;
		if (isPortcullis())
		{
			base.flags = 4;
			NavMeshObstacle component = GetComponent<NavMeshObstacle>();
			if (component != null)
			{
				component.enabled = !state();
			}
		}
		else
		{
			base.flags = 5;
		}
		if (base.link == 0)
		{
			return;
		}
		if (ObjectLoader.GetItemTypeAt(base.link) != 21 && ObjectLoader.GetItemTypeAt(base.link) != 115)
		{
			trigger_base component2 = ObjectLoader.getObjectIntAt(base.link).GetComponent<trigger_base>();
			if (component2 != null)
			{
				component2.Activate(base.gameObject);
			}
			return;
		}
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
		if (!(objectIntAt != null))
		{
			return;
		}
		int num = objectIntAt.next;
		while (num != 0)
		{
			ObjectInteraction objectIntAt2 = ObjectLoader.getObjectIntAt(num);
			if (objectIntAt2 != null)
			{
				num = 0;
				trigger_base component3 = objectIntAt2.GetComponent<trigger_base>();
				if (component3 != null)
				{
					if (component3.objInt().GetItemType() != 115 && component3.objInt().GetItemType() != 60)
					{
						component3.Activate(base.gameObject);
					}
					num = component3.next;
				}
			}
			else
			{
				num = 0;
			}
		}
	}

	public void CloseDoor(float DoorTravelTime)
	{
		if (!state() || DoorBusy)
		{
			return;
		}
		if (!isPortcullis())
		{
			if (ObjectInteraction.PlaySoundEffects)
			{
				objInt().aud.clip = MusicController.instance.SoundEffects[11];
				objInt().aud.Play();
			}
			StartCoroutine(RotateDoor(base.transform, Vector3.up * doordirection() * 90f, DoorTravelTime));
		}
		else
		{
			if (ObjectInteraction.PlaySoundEffects)
			{
				objInt().aud.clip = MusicController.instance.SoundEffects[20];
				objInt().aud.Play();
			}
			StartCoroutine(RaiseDoor(base.transform, new Vector3(0f, -1.1f, 0f), DoorTravelTime));
		}
		base.item_id -= 8;
		base.zpos -= 24;
		base.flags = 0;
		base.enchantment = 0;
		if (isPortcullis())
		{
			NavMeshObstacle component = GetComponent<NavMeshObstacle>();
			if (component != null)
			{
				component.enabled = !state();
			}
		}
		if (base.link == 0)
		{
			return;
		}
		if (ObjectLoader.GetItemTypeAt(base.link) != 21 && ObjectLoader.GetItemTypeAt(base.link) != 59)
		{
			trigger_base component2 = ObjectLoader.getObjectIntAt(base.link).GetComponent<trigger_base>();
			if (component2 != null)
			{
				component2.Activate(base.gameObject);
			}
			return;
		}
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
		if (!(objectIntAt != null))
		{
			return;
		}
		int num = objectIntAt.next;
		while (num != 0)
		{
			ObjectInteraction objectIntAt2 = ObjectLoader.getObjectIntAt(num);
			if (objectIntAt2 != null)
			{
				num = 0;
				trigger_base component3 = objectIntAt2.GetComponent<trigger_base>();
				if (component3 != null)
				{
					if (component3.objInt().GetItemType() != 59)
					{
						component3.Activate(base.gameObject);
					}
					num = component3.next;
				}
			}
			else
			{
				num = 0;
			}
		}
	}

	private int doordirection()
	{
		if (base.doordir == 0)
		{
			return 1;
		}
		return -1;
	}

	public void LockDoor()
	{
		ObjectInteraction lockObjInt = getLockObjInt();
		if (lockObjInt != null)
		{
			lockObjInt.flags |= 1;
		}
	}

	public void UnlockDoor(bool PlayerUse)
	{
		ObjectInteraction lockObjInt = getLockObjInt();
		if (!(lockObjInt != null))
		{
			return;
		}
		lockObjInt.flags &= 14;
		if (PlayerUse && lockObjInt.next != 0 && ObjectLoader.GetItemTypeAt(lockObjInt.next) == 60)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(lockObjInt.next);
			if (objectIntAt != null)
			{
				objectIntAt.GetComponent<object_base>().Activate(base.gameObject);
			}
		}
	}

	public void ToggleLock(bool PlayerUse)
	{
		if (!locked())
		{
			LockDoor();
		}
		else
		{
			UnlockDoor(PlayerUse);
		}
	}

	public void ToggleDoor(float doorTravelTime, bool PlayerUse)
	{
		if (!state())
		{
			UnlockDoor(PlayerUse);
			OpenDoor(doorTravelTime);
		}
		else
		{
			CloseDoor(doorTravelTime);
			LockDoor();
		}
	}

	private IEnumerator RotateDoor(Transform door, Vector3 turningAngle, float traveltime)
	{
		Quaternion StartAngle = door.rotation;
		Quaternion EndAngle = Quaternion.Euler(door.eulerAngles + turningAngle);
		DoorBusy = true;
		for (float t = 0f; t <= traveltime; t += Time.deltaTime / traveltime)
		{
			door.rotation = Quaternion.Lerp(StartAngle, EndAngle, t);
			yield return null;
		}
		DoorBusy = false;
		door.rotation = EndAngle;
		if (traveltime > 1f && ObjectInteraction.PlaySoundEffects)
		{
			objInt().aud.clip = MusicController.instance.SoundEffects[12];
			objInt().aud.Play();
		}
	}

	private IEnumerator RaiseDoor(Transform door, Vector3 TransformDir, float traveltime)
	{
		float rate = 1f / traveltime;
		float index = 0f;
		Vector3 StartPos = door.position;
		Vector3 EndPos = StartPos + TransformDir;
		DoorBusy = true;
		while (index <= traveltime)
		{
			door.position = Vector3.Lerp(StartPos, EndPos, index);
			index += rate * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		DoorBusy = false;
		door.position = EndPos;
	}

	public override bool ApplyAttack(short damage, GameObject source)
	{
		return ApplyAttack(damage);
	}

	public override bool ApplyAttack(short damage)
	{
		if (DR() < 3)
		{
			if (DR() != 0)
			{
				damage /= DR();
			}
			base.quality -= damage;
			if (base.quality <= 0)
			{
				UnlockDoor(true);
				OpenDoor(1.3f);
			}
		}
		return true;
	}

	private short DR()
	{
		switch (base.item_id)
		{
		case 320:
		case 327:
			return 0;
		case 321:
		case 322:
		case 328:
		case 329:
			return 1;
		case 323:
		case 324:
		case 330:
		case 331:
			return 2;
		default:
			return 3;
		}
	}

	public override bool LookAt()
	{
		if (!isPortcullis())
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), DoorQuality()));
			if (Spiked() && UWEBase._RES != "UW2")
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 131));
			}
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
		}
		return true;
	}

	private string DoorQuality()
	{
		if (base.quality == 0)
		{
			return StringController.instance.GetString(5, 0);
		}
		if (base.quality >= 1 && base.quality < 15)
		{
			return StringController.instance.GetString(5, 1);
		}
		if (base.quality >= 15 && base.quality < 32)
		{
			return StringController.instance.GetString(5, 2);
		}
		if (base.quality >= 32 && base.quality <= 40)
		{
			return StringController.instance.GetString(5, 3);
		}
		if (base.quality > 40 && base.quality < 48)
		{
			return StringController.instance.GetString(5, 4);
		}
		return StringController.instance.GetString(5, 5);
	}

	public override string UseVerb()
	{
		if (!state())
		{
			return "open";
		}
		return "close";
	}

	public override string UseObjectOnVerb_World()
	{
		if (UWEBase.CurrentObjectInHand != null)
		{
			switch (UWEBase.CurrentObjectInHand.GetItemType())
			{
			case 5:
				return "turn key in lock";
			case 87:
				return "spike door";
			case 79:
				return "attempt lockpicking";
			}
		}
		return base.UseObjectOnVerb_Inv();
	}

	public static void CreateDoor(GameObject myObj, ObjectInteraction objInt)
	{
		int num = 0;
		int num2 = 0;
		NavMeshObstacle navMeshObstacle = myObj.AddComponent<NavMeshObstacle>();
		navMeshObstacle.center = new Vector3(-0.4f, 0f, 0.5f);
		navMeshObstacle.size = new Vector3(0.8f, 0.1f, 1.1f);
		switch (objInt.GetItemType())
		{
		case 29:
			num2 = ((objInt.ObjectTileX <= 63) ? UWEBase.CurrentTileMap().Tiles[objInt.ObjectTileX, objInt.ObjectTileY].wallTexture : 0);
			break;
		default:
			num = ((objInt.item_id < 320 || objInt.item_id > 325) ? (objInt.item_id - 328) : (objInt.item_id - 320));
			num2 = ((!(UWEBase._RES == "UW2")) ? UWEBase.CurrentTileMap().texture_map[58 + num] : UWEBase.CurrentTileMap().texture_map[64 + num]);
			break;
		}
		myObj.layer = LayerMask.NameToLayer("Doors");
		GameObject gameObject;
		switch (objInt.GetItemType())
		{
		case 30:
			gameObject = Object.Instantiate((GameObject)Resources.Load("Models/Portcullis"));
			gameObject.name = myObj.name + "_Model";
			gameObject.transform.parent = myObj.transform;
			gameObject.transform.position = myObj.transform.position;
			return;
		case 29:
			RenderHiddenDoor(myObj.GetComponent<DoorControl>(), num2);
			return;
		}
		GameObject original = Resources.Load("Models/uw1_door") as GameObject;
		gameObject = Object.Instantiate(original);
		gameObject.name = myObj.name + "_Model";
		gameObject.transform.parent = myObj.transform;
		gameObject.transform.position = myObj.transform.position;
		gameObject.GetComponent<Renderer>().material = GameWorldController.instance.MaterialDoors[num2];
		gameObject.GetComponent<MeshCollider>().enabled = false;
		MeshCollider meshCollider = myObj.AddComponent<MeshCollider>();
		meshCollider.isTrigger = false;
		meshCollider.sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
	}

	private static void RenderHiddenDoor(DoorControl dc, int textureIndex)
	{
		dc.transform.position = new Vector3(dc.transform.position.x, 0f, dc.transform.position.z);
		textureIndex = UWEBase.CurrentTileMap().texture_map[textureIndex];
		int num = 6;
		Vector3[] array = new Vector3[24];
		Vector2[] array2 = new Vector2[24];
		int objectTileX = dc.ObjectTileX;
		int objectTileY = dc.ObjectTileY;
		if (objectTileX == 99)
		{
			return;
		}
		float num2 = UWEBase.CurrentTileMap().Tiles[objectTileX, objectTileY].floorHeight + 7;
		float num3 = UWEBase.CurrentTileMap().Tiles[objectTileX, objectTileY].floorHeight;
		float z = num2 * 0.15f;
		float z2 = num3 * 0.15f;
		float num4 = 1f;
		float num5 = 1f;
		float num6 = 0.8f;
		float num7 = 1.2f;
		float num8 = (num7 - num6) / 2f;
		float num9 = 0f;
		float num10 = num9 + num8 / 1.2f;
		float x = num10 + num6 / 1.2f;
		GameObject gameObject = new GameObject(dc.name + "_Model");
		gameObject.layer = LayerMask.NameToLayer("MapMesh");
		gameObject.transform.parent = dc.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.subMeshCount = num;
		Material[] array3 = new Material[num];
		int num11 = 0;
		float num12 = num2 - num3;
		float num13 = num3 * 0.125f;
		float y = num12 / 8f + num13;
		for (int i = 0; i < 6; i++)
		{
			switch (i)
			{
			case 0:
				array3[num11] = GameWorldController.instance.MaterialMasterList[textureIndex];
				array[4 * num11] = new Vector3(0f, -0.02f, z);
				array[1 + 4 * num11] = new Vector3(0f, 0.02f, z);
				array[2 + 4 * num11] = new Vector3(0f - num6, 0.02f, z);
				array[3 + 4 * num11] = new Vector3(0f - num6, -0.02f, z);
				array2[4 * num11] = new Vector2(0f, 1f * num5);
				array2[1 + 4 * num11] = new Vector2(0f, 0f);
				array2[2 + 4 * num11] = new Vector2(1f * num4, 0f);
				array2[3 + 4 * num11] = new Vector2(1f * num4, 1f * num5);
				break;
			case 4:
				array3[num11] = GameWorldController.instance.MaterialMasterList[textureIndex];
				array[4 * num11] = new Vector3(0f - num6, 0.02f, z2);
				array[1 + 4 * num11] = new Vector3(0f - num6, 0.02f, z);
				array[2 + 4 * num11] = new Vector3(0f, 0.02f, z);
				array[3 + 4 * num11] = new Vector3(0f, 0.02f, z2);
				array2[4 * num11] = new Vector2(num10, num13);
				array2[1 + 4 * num11] = new Vector2(num10, y);
				array2[2 + 4 * num11] = new Vector2(x, y);
				array2[3 + 4 * num11] = new Vector2(x, num13);
				break;
			case 3:
				array3[num11] = GameWorldController.instance.MaterialMasterList[textureIndex];
				array[4 * num11] = new Vector3(0f, 0.02f, z2);
				array[1 + 4 * num11] = new Vector3(0f, 0.02f, z);
				array[2 + 4 * num11] = new Vector3(0f, -0.02f, z);
				array[3 + 4 * num11] = new Vector3(0f, -0.02f, z2);
				array2[4 * num11] = new Vector2(num10, num13);
				array2[1 + 4 * num11] = new Vector2(num10, y);
				array2[2 + 4 * num11] = new Vector2(x, y);
				array2[3 + 4 * num11] = new Vector2(x, num13);
				break;
			case 1:
				array3[num11] = GameWorldController.instance.MaterialMasterList[textureIndex];
				array[4 * num11] = new Vector3(0f - num6, -0.02f, z2);
				array[1 + 4 * num11] = new Vector3(0f - num6, -0.02f, z);
				array[2 + 4 * num11] = new Vector3(0f - num6, 0.02f * num5, z);
				array[3 + 4 * num11] = new Vector3(0f - num6, 0.02f * num5, z2);
				array2[4 * num11] = new Vector2(num10, num13);
				array2[1 + 4 * num11] = new Vector2(num10, y);
				array2[2 + 4 * num11] = new Vector2(x, y);
				array2[3 + 4 * num11] = new Vector2(x, num13);
				break;
			case 5:
				array3[num11] = GameWorldController.instance.MaterialMasterList[textureIndex];
				array[4 * num11] = new Vector3(0f, -0.02f, z2);
				array[1 + 4 * num11] = new Vector3(0f, -0.02f, z);
				array[2 + 4 * num11] = new Vector3(0f - num6, -0.02f, z);
				array[3 + 4 * num11] = new Vector3(0f - num6, -0.02f, z2);
				array2[4 * num11] = new Vector2(num10, num13);
				array2[1 + 4 * num11] = new Vector2(num10, y);
				array2[2 + 4 * num11] = new Vector2(x, y);
				array2[3 + 4 * num11] = new Vector2(x, num13);
				break;
			case 2:
				array3[num11] = GameWorldController.instance.MaterialMasterList[textureIndex];
				array[4 * num11] = new Vector3(0f, 1.2f * num5, z2);
				array[1 + 4 * num11] = new Vector3(0f, 0f, z2);
				array[2 + 4 * num11] = new Vector3(-1.2f * num4, 0f, z2);
				array[3 + 4 * num11] = new Vector3(-1.2f * num4, 1.2f * num5, z2);
				array2[4 * num11] = new Vector2(0f, 0f);
				array2[1 + 4 * num11] = new Vector2(0f, 1f * num5);
				array2[2 + 4 * num11] = new Vector2(num4, 1f * num5);
				array2[3 + 4 * num11] = new Vector2(num4, 0f);
				break;
			}
			num11++;
		}
		mesh.vertices = array;
		mesh.uv = array2;
		num11 = 0;
		int[] array4 = new int[6];
		for (int j = 0; j < 6; j++)
		{
			array4[0] = 4 * num11;
			array4[1] = 1 + 4 * num11;
			array4[2] = 2 + 4 * num11;
			array4[3] = 4 * num11;
			array4[4] = 2 + 4 * num11;
			array4[5] = 3 + 4 * num11;
			mesh.SetTriangles(array4, num11);
			num11++;
		}
		meshRenderer.materials = array3;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshFilter.mesh = mesh;
		float num14 = 128f;
		float num15 = UWEBase.CurrentTileMap().CEILING_HEIGHT;
		int num16 = UWEBase.CurrentTileMap().Tiles[dc.ObjectTileX, dc.ObjectTileY].floorHeight * 4;
		float num17 = 15f;
		float num18 = (float)num16 / num14 * num15 * num17;
		num18 /= 100f;
		BoxCollider component = dc.GetComponent<BoxCollider>();
		component.center = new Vector3(-0.4f, 0f, 0.525f + num18);
		component.size = new Vector3(0.8f, 0.04f, 1.05f);
	}

	public override string ContextMenuDesc(int item_id)
	{
		int itemType = objInt().GetItemType();
		if (itemType == 29)
		{
			if (!state())
			{
				return "";
			}
			return base.ContextMenuDesc(item_id);
		}
		return base.ContextMenuDesc(item_id);
	}

	public override string ContextMenuUsedDesc()
	{
		int itemType = objInt().GetItemType();
		if (itemType == 29)
		{
			if (!state())
			{
				return "";
			}
			return base.ContextMenuUsedDesc();
		}
		return base.ContextMenuUsedDesc();
	}

	public static GameObject findDoor(int x, int y)
	{
		return GameObject.Find("door_" + x.ToString("D3") + "_" + y.ToString("D3"));
	}
}
