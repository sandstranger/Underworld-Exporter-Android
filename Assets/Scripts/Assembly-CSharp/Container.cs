using UnityEngine;
using UnityEngine.UI;

public class Container : UWEBase
{
	protected ObjectInteraction _objInt;

	public ObjectInteraction[] items = new ObjectInteraction[40];

	public int LockObject;

	public bool isOpenOnPanel;

	public string ContainerParent;

	private void Start()
	{
		if (objInt() != null)
		{
			PopulateContainer(this, objInt(), objInt().objectloaderinfo.parentList);
		}
	}

	public short MaxCapacity()
	{
		return (short)items.GetUpperBound(0);
	}

	public ObjectInteraction GetItemAt(short index)
	{
		if (index >= 0 && index <= MaxCapacity())
		{
			return items[index];
		}
		return null;
	}

	public bool AddItemMergedItemToContainer(ObjectInteraction itemToAdd)
	{
		for (int i = 0; i <= MaxCapacity(); i++)
		{
			if (items[i] != null && ObjectInteraction.CanMerge(items[i], itemToAdd))
			{
				ObjectInteraction.Merge(items[i], itemToAdd);
				return true;
			}
		}
		return AddItemToContainer(itemToAdd);
	}

	public bool AddItemToContainer(ObjectInteraction itemToAdd)
	{
		int i;
		for (i = 0; items[i] != null && i <= MaxCapacity(); i++)
		{
		}
		if (i <= MaxCapacity())
		{
			return AddItemToContainer(itemToAdd, i);
		}
		Debug.Log(base.name + " is full");
		return false;
	}

	public bool AddItemToContainer(ObjectInteraction item, int index)
	{
		if (item == null)
		{
			return false;
		}
		if (index <= MaxCapacity())
		{
			items[index] = item;
			return true;
		}
		return false;
	}

	public bool RemoveItemFromContainer(int index)
	{
		if (items[index] != null)
		{
			items[index] = null;
			return true;
		}
		return false;
	}

	public bool RemoveItemFromContainer(ObjectInteraction objectToRemove)
	{
		if (objectToRemove == null)
		{
			return false;
		}
		for (int i = 0; i <= MaxCapacity(); i++)
		{
			if (items[i] == objectToRemove)
			{
				RemoveItemFromContainer(i);
				return true;
			}
		}
		return false;
	}

	public void OpenContainer()
	{
		UWCharacter.Instance.playerInventory.ContainerOffset = 0;
		ScrollButtonStatsDisplay.ScrollValue = 0;
		ObjectInteraction component = base.gameObject.GetComponent<ObjectInteraction>();
		if (!component.PickedUp)
		{
			SpillContents();
			return;
		}
		SortContainer(this);
		UWHUD.instance.ContainerOpened.GetComponent<RawImage>().texture = GetContainerEquipDisplay().texture;
		if (!isOpenOnPanel)
		{
			isOpenOnPanel = true;
			ContainerParent = UWCharacter.Instance.playerInventory.currentContainer;
		}
		UWCharacter.Instance.playerInventory.currentContainer = base.name;
		if (UWCharacter.Instance.playerInventory.currentContainer == "")
		{
			UWCharacter.Instance.playerInventory.currentContainer = UWCharacter.Instance.name;
			ContainerParent = UWCharacter.Instance.name;
		}
		for (short num = 0; num < 8; num++)
		{
			UWCharacter.Instance.playerInventory.SetObjectAtSlot((short)(num + 11), GetItemAt(num));
		}
		UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().BackpackBg.SetActive(true);
		if (CountItems() >= 8 && this != UWCharacter.Instance.playerInventory.playerContainer)
		{
			UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().InvUp.SetActive(true);
			UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().InvDown.SetActive(true);
		}
		else
		{
			UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().InvUp.SetActive(false);
			UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().InvDown.SetActive(false);
		}
	}

	public void SpillContents()
	{
		TileMap tileMap = UWEBase.CurrentTileMap();
		UWEBase.FreezeMovement(base.gameObject);
		ObjectInteraction component = base.gameObject.GetComponent<ObjectInteraction>();
		component.UpdatePosition();
		component.SetWorldDisplay(component.GetEquipDisplay());
		component.link = 0;
		for (short num = 0; num <= MaxCapacity(); num++)
		{
			ObjectInteraction itemAt = GetItemAt(num);
			if (itemAt != null)
			{
				if (itemAt.GetComponent<trigger_base>() != null)
				{
					itemAt.GetComponent<trigger_base>().Activate(base.gameObject);
				}
				else
				{
					itemAt.transform.position = base.transform.position;
					itemAt.UpdatePosition();
					switch (tileMap.Tiles[component.ObjectTileX, component.ObjectTileY].tileType)
					{
					case 1:
					case 6:
					case 7:
					case 8:
					case 9:
						itemAt.xpos = (short)Random.Range(1, 7);
						itemAt.ypos = (short)Random.Range(1, 7);
						break;
					case 2:
						itemAt.xpos = (short)Random.Range(1, 7);
						itemAt.ypos = (short)Random.Range(0, itemAt.xpos);
						break;
					case 3:
						itemAt.xpos = (short)Random.Range(1, 7);
						itemAt.ypos = (short)Random.Range(1, 7 - itemAt.xpos);
						break;
					case 4:
						itemAt.xpos = (short)Random.Range(1, 7);
						itemAt.ypos = (short)Random.Range(8 - itemAt.xpos, 8);
						break;
					case 5:
						itemAt.xpos = (short)Random.Range(1, 7);
						itemAt.ypos = (short)Random.Range(itemAt.xpos, 8);
						break;
					}
					itemAt.zpos = (short)(tileMap.Tiles[component.ObjectTileX, component.ObjectTileY].floorHeight * 4);
					itemAt.objectloaderinfo.xpos = itemAt.xpos;
					itemAt.objectloaderinfo.ypos = itemAt.ypos;
					itemAt.objectloaderinfo.zpos = itemAt.zpos;
					itemAt.transform.position = ObjectLoader.CalcObjectXYZ(itemAt.objectloaderinfo.index, 0);
					RemoveItemFromContainer(num);
					UWEBase.UnFreezeMovement(itemAt);
				}
			}
		}
	}

	public void SpillContentsX()
	{
		TileMap tileMap = UWEBase.CurrentTileMap();
		UWEBase.FreezeMovement(base.gameObject);
		ObjectInteraction component = base.gameObject.GetComponent<ObjectInteraction>();
		component.SetWorldDisplay(component.GetEquipDisplay());
		for (short num = 0; num <= MaxCapacity(); num++)
		{
			ObjectInteraction itemAt = GetItemAt(num);
			if (itemAt != null)
			{
				if (itemAt.GetComponent<trigger_base>() != null)
				{
					itemAt.GetComponent<trigger_base>().Activate(base.gameObject);
				}
				bool flag = false;
				Vector3 vector = base.transform.position;
				int num2 = 0;
				while (!flag && num2 < 25)
				{
					vector = base.transform.position + Random.insideUnitSphere;
					if (vector.y < base.transform.position.y)
					{
						vector.y = base.transform.position.y + 0.1f;
					}
					flag = !Physics.CheckSphere(vector, 0.5f) && tileMap.ValidTile(vector);
					num2++;
				}
				if (flag)
				{
					RemoveItemFromContainer(num);
					itemAt.transform.position = vector;
					UWEBase.UnFreezeMovement(itemAt);
				}
				else
				{
					RemoveItemFromContainer(num);
					itemAt.transform.position = base.transform.position;
					UWEBase.UnFreezeMovement(itemAt);
				}
			}
		}
		UWEBase.UnFreezeMovement(base.gameObject);
	}

	public static void SetPickedUpFlag(Container cn, bool NewValue)
	{
		for (short num = 0; num <= cn.MaxCapacity(); num++)
		{
			ObjectInteraction itemAt = cn.GetItemAt(num);
			if (itemAt != null)
			{
				if (itemAt.GetItemType() == 55)
				{
					itemAt.GetComponent<a_pick_up_trigger>().Activate(cn.gameObject);
					cn.RemoveItemFromContainer(num);
				}
				else if (itemAt.GetComponent<Container>() != null)
				{
					SetPickedUpFlag(itemAt.GetComponent<Container>(), NewValue);
				}
			}
		}
	}

	public static void SetItemsPosition(Container cn, Vector3 Position)
	{
		for (short num = 0; num <= cn.MaxCapacity(); num++)
		{
			ObjectInteraction itemAt = cn.GetItemAt(num);
			if (itemAt != null)
			{
				itemAt.transform.position = Position;
				if (itemAt.GetComponent<Container>() != null)
				{
					SetItemsPosition(itemAt.GetComponent<Container>(), Position);
				}
			}
		}
	}

	public static void SetItemsParent(Container cn, Transform Parent)
	{
		for (short num = 0; num <= cn.MaxCapacity(); num++)
		{
			ObjectInteraction itemAt = cn.GetItemAt(num);
			if (itemAt != null)
			{
				itemAt.transform.parent = Parent;
				if (Parent == GameWorldController.instance.DynamicObjectMarker())
				{
					GameWorldController.MoveToWorld(itemAt);
				}
				else
				{
					GameWorldController.MoveToInventory(itemAt);
				}
				if (itemAt.GetComponent<Container>() != null)
				{
					SetItemsParent(itemAt.GetComponent<Container>(), Parent);
				}
			}
		}
	}

	public static int GetFreeSlot(Container cn)
	{
		for (short num = 0; num <= cn.MaxCapacity(); num++)
		{
			if (cn.GetItemAt(num) == null)
			{
				return num;
			}
		}
		return -1;
	}

	public static void SortContainer(Container cn)
	{
		int num = -1;
		bool flag = true;
		for (short num2 = 0; num2 <= cn.MaxCapacity(); num2++)
		{
			if (flag)
			{
				for (short num3 = 0; num3 <= cn.MaxCapacity(); num3++)
				{
					if (cn.GetItemAt(num3) == null)
					{
						num = num3;
						flag = false;
						break;
					}
				}
			}
			if (num2 > num && num != -1 && cn.GetItemAt(num2) != null)
			{
				cn.AddItemToContainer(cn.GetItemAt(num2), num);
				cn.RemoveItemFromContainer(num2);
				flag = true;
				num = -1;
			}
		}
	}

	public bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			OpenContainer();
			return true;
		}
		bool flag = true;
		if (UWEBase.CurrentObjectInHand.GetComponent<Container>() != null && base.gameObject.name == UWEBase.CurrentObjectInHand.name)
		{
			flag = false;
			Debug.Log("Attempt to add a container to itself");
		}
		if (!TestContainerRules(this, 11, false))
		{
			flag = false;
			return true;
		}
		if (flag)
		{
			if (!UWEBase.CurrentObjectInHand.isQuant || UWEBase.CurrentObjectInHand.isEnchanted)
			{
				AddItemToContainer(UWEBase.CurrentObjectInHand);
			}
			else
			{
				AddItemMergedItemToContainer(UWEBase.CurrentObjectInHand);
			}
			if (isOpenOnPanel)
			{
				OpenContainer();
			}
			else
			{
				removeFromContainer(UWCharacter.Instance.playerInventory.GetCurrentContainer(), UWEBase.CurrentObjectInHand);
			}
			UWEBase.CurrentObjectInHand = null;
			return true;
		}
		return false;
	}

	public float GetWeight(float containerObjWeight)
	{
		float num = containerObjWeight;
		for (short num2 = 0; num2 <= MaxCapacity(); num2++)
		{
			ObjectInteraction itemAt = GetItemAt(num2);
			if (itemAt != null)
			{
				num += itemAt.GetWeight();
			}
		}
		return num;
	}

	public float GetBaseWeight()
	{
		return GetComponent<object_base>().GetWeight();
	}

	public float GetCapacity()
	{
		return items.GetUpperBound(0);
	}

	public float GetFreeCapacity()
	{
		if (base.gameObject.name == UWCharacter.Instance.name)
		{
			return 999f;
		}
		return GetCapacity() - GetComponent<object_base>().GetWeight() - GetBaseWeight();
	}

	public static bool TestContainerRules(Container cn, int SlotIndex, bool Swapping)
	{
		if (SlotIndex < 11)
		{
			return true;
		}
		if (UWEBase.CurrentObjectInHand == null)
		{
			return true;
		}
		ObjectInteraction currentObjectInHand = UWEBase.CurrentObjectInHand;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (UWEBase.EditorMode)
		{
			return true;
		}
		switch (cn.ObjectsAccepted())
		{
		case 0:
			flag = currentObjectInHand.GetItemType() == 6;
			break;
		case 1:
			flag = currentObjectInHand.GetItemType() == 3;
			break;
		case 2:
			flag = currentObjectInHand.GetItemType() == 13 || currentObjectInHand.GetItemType() == 36 || currentObjectInHand.GetItemType() == 28 || currentObjectInHand.GetItemType() == 11;
			break;
		case 3:
			flag = currentObjectInHand.GetItemType() == 24 || currentObjectInHand.GetItemType() == 14;
			break;
		default:
			flag = true;
			break;
		}
		if (flag)
		{
			if (currentObjectInHand.GetWeight() >= cn.GetFreeCapacity())
			{
				flag2 = false;
				UWHUD.instance.MessageScroll.Add("The " + StringController.instance.GetSimpleObjectNameUW(cn.objInt()) + " is too full.");
			}
			else
			{
				flag2 = true;
			}
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_that_item_does_not_fit_));
		}
		if (flag2)
		{
			if (cn.CountItems() <= cn.MaxCapacity())
			{
				flag3 = true;
			}
			else
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_that_item_does_not_fit_));
			}
		}
		return flag && flag2 && (flag3 || Swapping);
	}

	public ObjectInteraction findItemOfType(int itemid)
	{
		for (short num = 0; num <= MaxCapacity(); num++)
		{
			ObjectInteraction itemAt = GetItemAt(num);
			if (itemAt != null)
			{
				if (itemAt.item_id == itemid)
				{
					return itemAt;
				}
				if (itemAt.GetItemType() == 19)
				{
					ObjectInteraction objectInteraction = itemAt.GetComponent<Container>().findItemOfType(itemid);
					if (objectInteraction != null)
					{
						return objectInteraction;
					}
				}
			}
		}
		return null;
	}

	public int CountItems()
	{
		int num = 0;
		for (int i = 0; i <= MaxCapacity(); i++)
		{
			if (items[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	public static void removeFromContainer(Container cn, ObjectInteraction objectToRemove)
	{
		for (int i = 0; i <= cn.MaxCapacity(); i++)
		{
			if (cn.items[i] == objectToRemove)
			{
				cn.RemoveItemFromContainer(i);
				break;
			}
		}
	}

	public static void PopulateContainer(Container cn, ObjectInteraction objInt, ObjectLoader objList)
	{
		if (objInt.link != 0)
		{
			ObjectLoaderInfo objectInfoAt = ObjectLoader.getObjectInfoAt(objInt.link, objList);
			if (ObjectLoader.GetItemTypeAt(objInt.link, objList) != 21)
			{
				cn.AddItemToContainer(objectInfoAt.instance);
			}
			else
			{
				cn.LockObject = objInt.link;
			}
			while (objectInfoAt.next != 0)
			{
				objectInfoAt = ObjectLoader.getObjectInfoAt(objectInfoAt.next, objList);
				cn.AddItemToContainer(objectInfoAt.instance);
			}
		}
	}

	public static int FindItemInContainer(Container cn, ObjectInteraction ItemToFind)
	{
		for (int i = 0; i <= cn.MaxCapacity(); i++)
		{
			if (cn.items[i] == ItemToFind)
			{
				return i;
			}
		}
		return -1;
	}

	public Sprite GetContainerEquipDisplay()
	{
		switch (objInt().item_id)
		{
		case 128:
		case 130:
		case 132:
		case 134:
		case 136:
		case 138:
			return GameWorldController.instance.ObjectArt.RequestSprite(objInt().item_id + 1);
		default:
			return GameWorldController.instance.ObjectArt.RequestSprite(objInt().item_id);
		}
	}

	public int ObjectsAccepted()
	{
		if (this == UWCharacter.Instance.playerInventory.playerContainer)
		{
			return -1;
		}
		return GameWorldController.instance.objDat.containerStats[objInt().item_id - 128].objectsMask;
	}

	public bool DropEvent()
	{
		for (short num = 0; num <= items.GetUpperBound(0); num++)
		{
			if (items[num] != null && items[num].GetComponent<object_base>() != null)
			{
				items[num].GetComponent<object_base>().DropEvent();
			}
		}
		return true;
	}

	public bool PickupEvent()
	{
		for (short num = 0; num <= items.GetUpperBound(0); num++)
		{
			if (items[num] != null && items[num].GetComponent<object_base>() != null)
			{
				items[num].GetComponent<object_base>().PickupEvent();
			}
		}
		return true;
	}

	public ObjectInteraction objInt()
	{
		if (_objInt == null)
		{
			_objInt = GetComponent<ObjectInteraction>();
		}
		return _objInt;
	}
}
