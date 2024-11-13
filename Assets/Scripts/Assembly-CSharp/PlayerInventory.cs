using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : UWEBase
{
	private ObjectInteraction _ObjectInHand;

	public bool JustPickedup;

	public ObjectInteraction _sHelm;

	public ObjectInteraction _sChest;

	public ObjectInteraction _sLegs;

	public ObjectInteraction _sBoots;

	public ObjectInteraction _sGloves;

	public ObjectInteraction _sLeftHand;

	public ObjectInteraction _sRightHand;

	public ObjectInteraction _sLeftRing;

	public ObjectInteraction _sRightRing;

	public ObjectInteraction _sLeftShoulder;

	public ObjectInteraction _sRightShoulder;

	public ObjectInteraction[] _sBackPack = new ObjectInteraction[8];

	public GameObject InventoryMarker;

	private ObjectInteraction[] LightGameObjects = new ObjectInteraction[4];

	public string currentContainer;

	private UWCharacter playerUW;

	public Texture2D Blank;

	public Light lt;

	private LightSource ls;

	public Container playerContainer;

	public short ContainerOffset = 0;

	public static bool Ready;

	public ObjectInteraction ObjectInHand
	{
		get
		{
			return _ObjectInHand;
		}
		set
		{
			_ObjectInHand = value;
			if (_ObjectInHand == null)
			{
				UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
				return;
			}
			_ObjectInHand.UpdateAnimation();
			UWHUD.instance.CursorIcon = _ObjectInHand.GetInventoryDisplay().texture;
		}
	}

	public ObjectInteraction sHelm
	{
		get
		{
			return _sHelm;
		}
		set
		{
			_sHelm = value;
			if (playerUW.isFemale)
			{
				DisplayGameObject(_sHelm, UWHUD.instance.Helm_f_Slot, null, true);
			}
			else
			{
				DisplayGameObject(_sHelm, UWHUD.instance.Helm_m_Slot, null, true);
			}
		}
	}

	public ObjectInteraction sChest
	{
		get
		{
			return _sChest;
		}
		set
		{
			_sChest = value;
			if (playerUW.isFemale)
			{
				DisplayGameObject(_sChest, UWHUD.instance.Chest_f_Slot, null, true);
			}
			else
			{
				DisplayGameObject(_sChest, UWHUD.instance.Chest_m_Slot, null, true);
			}
		}
	}

	public ObjectInteraction sLegs
	{
		get
		{
			return _sLegs;
		}
		set
		{
			_sLegs = value;
			if (playerUW.isFemale)
			{
				DisplayGameObject(_sLegs, UWHUD.instance.Legs_f_Slot, null, true);
			}
			else
			{
				DisplayGameObject(_sLegs, UWHUD.instance.Legs_m_Slot, null, true);
			}
		}
	}

	public ObjectInteraction sBoots
	{
		get
		{
			return _sBoots;
		}
		set
		{
			_sBoots = value;
			if (playerUW.isFemale)
			{
				DisplayGameObject(_sBoots, UWHUD.instance.Boots_f_Slot, null, true);
			}
			else
			{
				DisplayGameObject(_sBoots, UWHUD.instance.Boots_m_Slot, null, true);
			}
		}
	}

	public ObjectInteraction sGloves
	{
		get
		{
			return _sGloves;
		}
		set
		{
			_sHelm = value;
			if (playerUW.isFemale)
			{
				DisplayGameObject(_sHelm, UWHUD.instance.Gloves_f_Slot, null, true);
			}
			else
			{
				DisplayGameObject(_sHelm, UWHUD.instance.Gloves_m_Slot, null, true);
			}
		}
	}

	public ObjectInteraction sLeftHand
	{
		get
		{
			return _sLeftHand;
		}
		set
		{
			_sLeftHand = value;
			DisplayGameObject(_sLeftHand, UWHUD.instance.LeftHand_Slot, UWHUD.instance.LeftHand_Qty, false);
		}
	}

	public ObjectInteraction sRightHand
	{
		get
		{
			return _sRightHand;
		}
		set
		{
			_sRightHand = value;
			DisplayGameObject(_sRightHand, UWHUD.instance.RightHand_Slot, UWHUD.instance.RightHand_Qty, false);
		}
	}

	public ObjectInteraction sLeftRing
	{
		get
		{
			return _sLeftRing;
		}
		set
		{
			_sLeftRing = value;
			DisplayGameObject(_sLeftRing, UWHUD.instance.LeftRing_Slot, null, false);
		}
	}

	public ObjectInteraction sRightRing
	{
		get
		{
			return _sRightRing;
		}
		set
		{
			_sRightRing = value;
			DisplayGameObject(_sRightRing, UWHUD.instance.RightRing_Slot, null, false);
		}
	}

	public ObjectInteraction sLeftShoulder
	{
		get
		{
			return _sLeftShoulder;
		}
		set
		{
			_sLeftShoulder = value;
			DisplayGameObject(_sLeftShoulder, UWHUD.instance.LeftShoulder_Slot, UWHUD.instance.LeftShoulder_Qty, false);
		}
	}

	public ObjectInteraction sRightShoulder
	{
		get
		{
			return _sRightShoulder;
		}
		set
		{
			_sRightShoulder = value;
			DisplayGameObject(_sRightShoulder, UWHUD.instance.RightShoulder_Slot, UWHUD.instance.RightShoulder_Qty, false);
		}
	}

	public void setBackPack(int index, ObjectInteraction value)
	{
		_sBackPack[index] = value;
		DisplayGameObject(_sBackPack[index], UWHUD.instance.BackPack_Slot[index], UWHUD.instance.Backpack_Slot_Qty[index], false);
	}

	public ObjectInteraction getBackPack(int index)
	{
		return _sBackPack[index];
	}

	public void Begin()
	{
		if (!(UWEBase._RES == "SHOCK"))
		{
			GRLoader gRLoader = new GRLoader(4);
			Blank = Resources.Load<Texture2D>(UWEBase._RES + "/Sprites/Texture_Blank");
			playerUW = GetComponent<UWCharacter>();
			playerContainer = GetComponent<Container>();
			for (int i = 0; i < 8; i++)
			{
				setBackPack(i, null);
			}
			UWHUD.instance.Encumberance.text = Mathf.Round(getEncumberance()).ToString();
			if (playerUW.isFemale)
			{
				UWHUD.instance.playerBody.texture = gRLoader.LoadImageAt(5 + playerUW.Body);
			}
			else
			{
				UWHUD.instance.playerBody.texture = gRLoader.LoadImageAt(playerUW.Body);
			}
			Ready = true;
			Refresh();
		}
	}

	private void Update()
	{
		if (Ready)
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "SHOCK")
			{
				UpdateShock();
			}
		}
	}

	private void UpdateShock()
	{
	}

	private void UpdateUW()
	{
	}

	public void UpdateLightSources()
	{
		if (lt == null)
		{
			lt = base.gameObject.GetComponentInChildren<Light>();
		}
		ls = null;
		float num = LightSource.MagicBrightness;
		for (int i = 5; i <= 8; i++)
		{
			ls = null;
			if (LightGameObjects[i - 5] != null)
			{
				if (GetObjectAtSlot(i) != LightGameObjects[i - 5])
				{
					LightGameObjects[i - 5] = GetObjectIntAtSlot(i);
				}
			}
			else if (GetObjectAtSlot(i) != null)
			{
				LightGameObjects[i - 5] = GetObjectIntAtSlot(i);
			}
			if (GetObjectAtSlot(i) != null && LightGameObjects[i - 5] != null)
			{
				ls = LightGameObjects[i - 5].GetComponent<LightSource>();
				if (ls != null && ls.IsOn() && num < ls.Brightness())
				{
					num = ls.Brightness();
				}
			}
		}
		lt.range = LightSource.BaseBrightness + num;
		if (num > 0f)
		{
			playerUW.LightActive = true;
		}
		else
		{
			playerUW.LightActive = false;
		}
	}

	private void DisplayGameObject(ObjectInteraction obj, RawImage Label, Text qtyDisplay, bool isEquipped)
	{
		if (obj == null)
		{
			Label.texture = Blank;
			if (qtyDisplay != null)
			{
				qtyDisplay.text = "";
			}
		}
		else if (obj != null)
		{
			if (isEquipped)
			{
				Label.texture = obj.GetEquipDisplay().texture;
				return;
			}
			Label.texture = obj.GetInventoryDisplay().texture;
			if (qtyDisplay != null)
			{
				int qty = obj.GetQty();
				if (qty <= 1)
				{
					qtyDisplay.text = "";
				}
				else
				{
					qtyDisplay.text = qty.ToString();
				}
			}
		}
		else
		{
			Label.texture = Blank;
			if (qtyDisplay != null)
			{
				qtyDisplay.text = "";
			}
		}
	}

	private void DisplayGameObject(ObjectInteraction obj, RawImage Label, Text qtyDisplay, bool isEquipped, ref bool hasChanged)
	{
		if (!hasChanged)
		{
			return;
		}
		if (obj == null)
		{
			Label.texture = Blank;
			if (qtyDisplay != null)
			{
				qtyDisplay.text = "";
			}
			hasChanged = false;
			return;
		}
		hasChanged = false;
		if (obj != null)
		{
			if (isEquipped)
			{
				Label.texture = obj.GetEquipDisplay().texture;
				return;
			}
			Label.texture = obj.GetInventoryDisplay().texture;
			if (qtyDisplay != null)
			{
				int qty = obj.GetQty();
				if (qty <= 1)
				{
					qtyDisplay.text = "";
				}
				else
				{
					qtyDisplay.text = qty.ToString();
				}
			}
		}
		else
		{
			Label.texture = Blank;
			if (qtyDisplay != null)
			{
				qtyDisplay.text = "";
			}
		}
	}

	public bool GetObjectDescAtSlot(int SlotIndex)
	{
		ObjectInteraction objectAtSlot = GetObjectAtSlot(SlotIndex);
		if (objectAtSlot != null)
		{
			return objectAtSlot.LookDescription();
		}
		return false;
	}

	public ObjectInteraction GetObjectIntAtSlot(int slotIndex)
	{
		return GetObjectAtSlot(slotIndex);
	}

	public ObjectInteraction GetObjectAtSlot(int slotIndex)
	{
		switch (slotIndex)
		{
		case 0:
			return sHelm;
		case 1:
			return sChest;
		case 2:
			return sLegs;
		case 3:
			return sBoots;
		case 4:
			return sGloves;
		case 5:
			return sRightShoulder;
		case 6:
			return sLeftShoulder;
		case 7:
			return sRightHand;
		case 8:
			return sLeftHand;
		case 9:
			return sRightRing;
		case 10:
			return sLeftRing;
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
			return getBackPack(slotIndex - 11);
		default:
			return null;
		}
	}

	public void SetObjectAtSlot(short slotIndex, ObjectInteraction sObject)
	{
		switch (slotIndex)
		{
		case 0:
			sHelm = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 1:
			sChest = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 2:
			sLegs = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 3:
			sBoots = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 4:
			sGloves = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 5:
			sRightShoulder = sObject;
			break;
		case 6:
			sLeftShoulder = sObject;
			break;
		case 7:
			sRightHand = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 8:
			sLeftHand = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 9:
			sRightRing = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 10:
			sLeftRing = sObject;
			EquipItemEvent(slotIndex);
			break;
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		{
			setBackPack(slotIndex - 11, sObject);
			Container component = GameObject.Find(currentContainer).GetComponent<Container>();
			component.items[ContainerOffset + slotIndex - 11] = sObject;
			PutItemAwayEvent(slotIndex);
			break;
		}
		}
	}

	public void ClearSlot(short slotIndex)
	{
		switch (slotIndex)
		{
		case 0:
			UnEquipItemEvent(slotIndex);
			sHelm = null;
			break;
		case 1:
			UnEquipItemEvent(slotIndex);
			sChest = null;
			break;
		case 2:
			UnEquipItemEvent(slotIndex);
			sLegs = null;
			break;
		case 3:
			UnEquipItemEvent(slotIndex);
			sBoots = null;
			break;
		case 4:
			UnEquipItemEvent(slotIndex);
			sGloves = null;
			break;
		case 5:
			sRightShoulder = null;
			break;
		case 6:
			sLeftShoulder = null;
			break;
		case 7:
			UnEquipItemEvent(slotIndex);
			sRightHand = null;
			break;
		case 8:
			UnEquipItemEvent(slotIndex);
			sLeftHand = null;
			break;
		case 9:
			UnEquipItemEvent(slotIndex);
			sRightRing = null;
			break;
		case 10:
			UnEquipItemEvent(slotIndex);
			sLeftRing = null;
			break;
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
			setBackPack(slotIndex - 11, null);
			break;
		}
	}

	public void Refresh()
	{
		Container component = GameObject.Find(currentContainer).GetComponent<Container>();
		sHelm = sHelm;
		sChest = sChest;
		sLegs = sLegs;
		sBoots = sBoots;
		sGloves = sGloves;
		sRightShoulder = sRightShoulder;
		sLeftShoulder = sLeftShoulder;
		sRightHand = sRightHand;
		sLeftHand = sLeftHand;
		sRightRing = sRightRing;
		sLeftRing = sLeftRing;
		for (short num = 11; num <= 18; num++)
		{
			setBackPack(num - 11, component.GetItemAt((short)(ContainerOffset + num - 11)));
		}
		if (UWHUD.instance.Encumberance.enabled)
		{
			UWHUD.instance.Encumberance.text = Mathf.Round(getEncumberance()).ToString();
		}
		UpdateLightSources();
	}

	public void SwapObjects(ObjectInteraction ObjInSlot, short slotIndex, ObjectInteraction cObjectInHand)
	{
		Container component = GameObject.Find(currentContainer).GetComponent<Container>();
		RemoveItem(ObjInSlot);
		SetObjectAtSlot(slotIndex, cObjectInHand);
		if (slotIndex >= 11)
		{
			component.AddItemToContainer(cObjectInHand, ContainerOffset + slotIndex - 11);
		}
		ObjectInHand = ObjInSlot;
		Refresh();
	}

	public bool RemoveItem(ObjectInteraction ObjectToRemove)
	{
		if (sHelm == ObjectToRemove)
		{
			UnEquipItemEvent(0);
			sHelm = null;
			return true;
		}
		if (sChest == ObjectToRemove)
		{
			UnEquipItemEvent(1);
			sChest = null;
			return true;
		}
		if (sLegs == ObjectToRemove)
		{
			UnEquipItemEvent(2);
			sLegs = null;
			return true;
		}
		if (sBoots == ObjectToRemove)
		{
			UnEquipItemEvent(3);
			sBoots = null;
			return true;
		}
		if (sGloves == ObjectToRemove)
		{
			UnEquipItemEvent(4);
			sGloves = null;
			return true;
		}
		if (sRightShoulder == ObjectToRemove)
		{
			sRightShoulder = null;
			return true;
		}
		if (sLeftShoulder == ObjectToRemove)
		{
			sLeftShoulder = null;
			return true;
		}
		if (sRightHand == ObjectToRemove)
		{
			UnEquipItemEvent(7);
			sRightHand = null;
			return true;
		}
		if (sLeftHand == ObjectToRemove)
		{
			UnEquipItemEvent(8);
			sLeftHand = null;
			return true;
		}
		if (sRightRing == ObjectToRemove)
		{
			UnEquipItemEvent(9);
			sRightRing = null;
			return true;
		}
		if (sLeftRing == ObjectToRemove)
		{
			UnEquipItemEvent(10);
			sLeftRing = null;
			return true;
		}
		Container container = playerContainer;
		for (int i = 0; i < 8; i++)
		{
			if (container.items[i] != null && ObjectToRemove == container.items[i])
			{
				container.items[i] = null;
				setBackPack(i, null);
				return true;
			}
		}
		foreach (Transform item in GameWorldController.instance.InventoryMarker.transform)
		{
			if (!(item.GetComponent<Container>() != null))
			{
				continue;
			}
			Container component = item.GetComponent<Container>();
			if (!(component != null))
			{
				continue;
			}
			for (int j = 0; j <= component.items.GetUpperBound(0); j++)
			{
				if (component.items[j] == ObjectToRemove)
				{
					component.items[j] = null;
					return true;
				}
			}
		}
		return false;
	}

	public bool RemoveItemFromEquipment(ObjectInteraction ObjectToRemove)
	{
		if (sHelm == ObjectToRemove)
		{
			UnEquipItemEvent(0);
			sHelm = null;
			return true;
		}
		if (sChest == ObjectToRemove)
		{
			UnEquipItemEvent(1);
			sChest = null;
			return true;
		}
		if (sLegs == ObjectToRemove)
		{
			UnEquipItemEvent(2);
			sLegs = null;
			return true;
		}
		if (sBoots == ObjectToRemove)
		{
			UnEquipItemEvent(3);
			sBoots = null;
			return true;
		}
		if (sGloves == ObjectToRemove)
		{
			UnEquipItemEvent(4);
			sGloves = null;
			return true;
		}
		if (sRightShoulder == ObjectToRemove)
		{
			sRightShoulder = null;
			return true;
		}
		if (sLeftShoulder == ObjectToRemove)
		{
			sLeftShoulder = null;
			return true;
		}
		if (sRightHand == ObjectToRemove)
		{
			UnEquipItemEvent(7);
			sRightHand = null;
			return true;
		}
		if (sLeftHand == ObjectToRemove)
		{
			UnEquipItemEvent(8);
			sLeftHand = null;
			return true;
		}
		if (sRightRing == ObjectToRemove)
		{
			UnEquipItemEvent(9);
			sRightRing = null;
			return true;
		}
		if (sLeftRing == ObjectToRemove)
		{
			UnEquipItemEvent(10);
			sLeftRing = null;
			return true;
		}
		return false;
	}

	public Container GetCurrentContainer()
	{
		return GameObject.Find(currentContainer).GetComponent<Container>();
	}

	public GameObject GetGameObject(string name)
	{
		return GameObject.Find(name);
	}

	public void PutItemAwayEvent(short slotNo)
	{
		ObjectInteraction objectIntAtSlot = GetObjectIntAtSlot(slotNo);
		if (objectIntAtSlot != null)
		{
			objectIntAtSlot.PutItemAway(slotNo);
		}
	}

	public void EquipItemEvent(short slotNo)
	{
		ObjectInteraction objectIntAtSlot = GetObjectIntAtSlot(slotNo);
		if (objectIntAtSlot != null)
		{
			objectIntAtSlot.Equip(slotNo);
		}
	}

	public void UnEquipItemEvent(short slotNo)
	{
		ObjectInteraction objectIntAtSlot = GetObjectIntAtSlot(slotNo);
		if (objectIntAtSlot != null)
		{
			objectIntAtSlot.UnEquip(slotNo);
		}
	}

	public float getInventoryWeight()
	{
		float num = 0f;
		for (int i = 0; i <= 10; i++)
		{
			ObjectInteraction objectIntAtSlot = GetObjectIntAtSlot(i);
			if (objectIntAtSlot != null)
			{
				num += objectIntAtSlot.GetWeight();
			}
		}
		for (short num2 = 0; num2 <= playerContainer.MaxCapacity(); num2++)
		{
			ObjectInteraction itemAt = playerContainer.GetItemAt(num2);
			num = ((!(itemAt != null)) ? num : (num + itemAt.GetWeight()));
		}
		return num;
	}

	public float getEncumberance()
	{
		float inventoryWeight = getInventoryWeight();
		float num = 0f;
		string rES = UWEBase._RES;
		num = ((rES == null || !(rES == "UW2")) ? ((float)playerUW.PlayerSkills.STR * 2f) : ((float)playerUW.PlayerSkills.STR * 2.5f));
		return num - inventoryWeight;
	}

	public ObjectInteraction findObjInteractionByID(int item_id)
	{
		for (int i = 0; i <= 10; i++)
		{
			ObjectInteraction objectIntAtSlot = GetObjectIntAtSlot(i);
			if (!(objectIntAtSlot != null))
			{
				continue;
			}
			if (objectIntAtSlot.item_id == item_id)
			{
				return objectIntAtSlot;
			}
			if (objectIntAtSlot.GetItemType() == 19)
			{
				ObjectInteraction objectInteraction = objectIntAtSlot.GetComponent<Container>().findItemOfType(item_id);
				if (objectInteraction != null && objectInteraction.item_id == item_id)
				{
					return objectInteraction;
				}
			}
		}
		ObjectInteraction objectInteraction2 = playerContainer.findItemOfType(item_id);
		if (objectInteraction2 != null && objectInteraction2.item_id == item_id)
		{
			return objectInteraction2;
		}
		return null;
	}

	public short getArmourScore()
	{
		short num = 0;
		num += getDefenceAtSlot(0);
		num += getDefenceAtSlot(1);
		num += getDefenceAtSlot(2);
		num += getDefenceAtSlot(3);
		num += getDefenceAtSlot(4);
		return (short)(num + UWCharacter.Instance.Resistance);
	}

	public short getArmourDurability()
	{
		short num = 0;
		num += getDefenceAtSlot(0);
		num += getDefenceAtSlot(1);
		num += getDefenceAtSlot(2);
		num += getDefenceAtSlot(3);
		return (short)(num + getDefenceAtSlot(4));
	}

	public void ApplyArmourDamage(short armourDamage)
	{
		int[] array = new int[7] { 0, 1, 2, 3, 4, 7, 8 };
		int num = array[Random.Range(0, array.GetUpperBound(0))];
		num = 2;
		ObjectInteraction objectIntAtSlot = GetObjectIntAtSlot(num);
		if (!(objectIntAtSlot != null))
		{
			return;
		}
		switch (num)
		{
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		{
			if (!(objectIntAtSlot.GetComponent<Armour>() != null))
			{
				break;
			}
			short durability3 = objectIntAtSlot.GetComponent<Armour>().getDurability();
			if (durability3 <= 30)
			{
				objectIntAtSlot.GetComponent<Armour>().SelfDamage((short)Mathf.Max(0, armourDamage - durability3));
				if (objectIntAtSlot.quality <= 0)
				{
					playerUW.playerInventory.ClearSlot((short)num);
					objectIntAtSlot.transform.parent = GameWorldController.instance.DynamicObjectMarker().transform;
					objectIntAtSlot.transform.position = playerUW.transform.position;
					GameWorldController.MoveToWorld(objectIntAtSlot);
					UWEBase.UnFreezeMovement(objectIntAtSlot);
					playerUW.playerInventory.Refresh();
				}
			}
			break;
		}
		case 7:
		{
			if (UWCharacter.Instance.isLefty || !(objectIntAtSlot.gameObject.GetComponent<Shield>() != null))
			{
				break;
			}
			short durability2 = objectIntAtSlot.GetComponent<Shield>().getDurability();
			if (durability2 <= 30)
			{
				objectIntAtSlot.GetComponent<Shield>().SelfDamage((short)Mathf.Max(0, armourDamage - durability2));
				if (objectIntAtSlot.quality <= 0)
				{
					playerUW.playerInventory.Refresh();
				}
			}
			break;
		}
		case 8:
		{
			if (!UWCharacter.Instance.isLefty || !(objectIntAtSlot.gameObject.GetComponent<Shield>() != null))
			{
				break;
			}
			short durability = objectIntAtSlot.GetComponent<Shield>().getDurability();
			if (durability <= 30)
			{
				objectIntAtSlot.GetComponent<Shield>().SelfDamage((short)Mathf.Max(0, armourDamage - durability));
				if (objectIntAtSlot.quality <= 0)
				{
					playerUW.playerInventory.Refresh();
				}
			}
			break;
		}
		case 5:
		case 6:
			break;
		}
	}

	private short getDefenceAtSlot(int slot)
	{
		ObjectInteraction objectIntAtSlot = GetObjectIntAtSlot(slot);
		if (objectIntAtSlot != null)
		{
			switch (slot)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
				if (objectIntAtSlot.GetComponent<Armour>() != null)
				{
					return objectIntAtSlot.GetComponent<Armour>().getDefence();
				}
				break;
			case 5:
				if (objectIntAtSlot.GetComponent<Ring>() != null)
				{
					return objectIntAtSlot.GetComponent<Ring>().getDefence();
				}
				break;
			case 7:
				if (UWCharacter.Instance.isLefty)
				{
					return 0;
				}
				if (objectIntAtSlot.GetComponent<Shield>() != null)
				{
					return objectIntAtSlot.GetComponent<Shield>().getDefence();
				}
				break;
			case 8:
				if (!UWCharacter.Instance.isLefty)
				{
					return 0;
				}
				if (objectIntAtSlot.GetComponent<Shield>() != null)
				{
					return objectIntAtSlot.GetComponent<Shield>().getDefence();
				}
				break;
			}
		}
		return 0;
	}
}
