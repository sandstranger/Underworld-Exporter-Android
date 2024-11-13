using System;
using UnityEngine;

public class ObjectInteraction : UWEBase
{
	public enum IdentificationFlags
	{
		Unidentified = 0,
		PartiallyIdentified = 1,
		Identified = 2
	}

	public static bool PlaySoundEffects = true;

	public const int NPC_TYPE = 0;

	public const int WEAPON = 1;

	public const int ARMOUR = 2;

	public const int AMMO = 3;

	public const int DOOR = 4;

	public const int KEY = 5;

	public const int RUNE = 6;

	public const int BRIDGE = 7;

	public const int BUTTON = 8;

	public const int LIGHT = 9;

	public const int SIGN = 10;

	public const int BOOK = 11;

	public const int WAND = 12;

	public const int SCROLL = 13;

	public const int POTIONS = 14;

	public const int INSERTABLE = 15;

	public const int INVENTORY = 16;

	public const int ACTIVATOR = 17;

	public const int TREASURE = 18;

	public const int CONTAINER = 19;

	public const int LOCK = 21;

	public const int TORCH = 22;

	public const int CLUTTER = 23;

	public const int FOOD = 24;

	public const int SCENERY = 25;

	public const int INSTRUMENT = 26;

	public const int FIRE = 27;

	public const int MAP = 28;

	public const int HIDDENDOOR = 29;

	public const int PORTCULLIS = 30;

	public const int PILLAR = 31;

	public const int SOUND = 32;

	public const int CORPSE = 33;

	public const int TMAP_SOLID = 34;

	public const int TMAP_CLIP = 35;

	public const int MAGICSCROLL = 36;

	public const int A_DAMAGE_TRAP = 37;

	public const int A_TELEPORT_TRAP = 38;

	public const int A_ARROW_TRAP = 39;

	public const int A_DO_TRAP = 40;

	public const int A_PIT_TRAP = 41;

	public const int A_CHANGE_TERRAIN_TRAP = 42;

	public const int A_SPELLTRAP = 43;

	public const int A_CREATE_OBJECT_TRAP = 44;

	public const int A_DOOR_TRAP = 45;

	public const int A_WARD_TRAP = 46;

	public const int A_TELL_TRAP = 47;

	public const int A_DELETE_OBJECT_TRAP = 48;

	public const int AN_INVENTORY_TRAP = 49;

	public const int A_SET_VARIABLE_TRAP = 50;

	public const int A_CHECK_VARIABLE_TRAP = 51;

	public const int A_COMBINATION_TRAP = 52;

	public const int A_TEXT_STRING_TRAP = 53;

	public const int A_MOVE_TRIGGER = 54;

	public const int A_PICK_UP_TRIGGER = 55;

	public const int A_USE_TRIGGER = 56;

	public const int A_LOOK_TRIGGER = 57;

	public const int A_STEP_ON_TRIGGER = 58;

	public const int AN_OPEN_TRIGGER = 59;

	public const int AN_UNLOCK_TRIGGER = 60;

	public const int A_FOUNTAIN = 61;

	public const int SHOCK_DECAL = 62;

	public const int COMPUTER_SCREEN = 63;

	public const int SHOCK_WORDS = 64;

	public const int SHOCK_GRATING = 65;

	public const int SHOCK_DOOR = 66;

	public const int SHOCK_DOOR_TRANSPARENT = 67;

	public const int UW_PAINTING = 68;

	public const int PARTICLE = 69;

	public const int RUNEBAG = 70;

	public const int SHOCK_BRIDGE = 71;

	public const int FORCE_DOOR = 72;

	public const int HIDDENPLACEHOLDER = 999;

	public const int HELM = 73;

	public const int RING = 74;

	public const int BOOT = 75;

	public const int GLOVES = 76;

	public const int LEGGINGS = 77;

	public const int SHIELD = 78;

	public const int LOCKPICK = 79;

	public const int ANIMATION = 80;

	public const int SILVERSEED = 81;

	public const int FOUNTAIN = 82;

	public const int SHRINE = 83;

	public const int GRAVE = 84;

	public const int ANVIL = 85;

	public const int POLE = 86;

	public const int SPIKE = 87;

	public const int REFILLABLE_LANTERN = 88;

	public const int OIL = 89;

	public const int MOONSTONE = 90;

	public const int LEECH = 91;

	public const int FISHING_POLE = 92;

	public const int ZANIUM = 93;

	public const int BEDROLL = 94;

	public const int FORCEFIELD = 95;

	public const int MOONGATE = 96;

	public const int BOULDER = 97;

	public const int ORB = 98;

	public const int SPELL = 99;

	public const int AN_OSCILLATOR = 100;

	public const int A_TIMER_TRIGGER = 101;

	public const int A_SCHEDULED_TRIGGER = 102;

	public const int A_CHANGE_FROM_TRAP = 103;

	public const int A_CHANGE_TO_TRAP = 104;

	public const int AN_EXPERIENCE_TRAP = 105;

	public const int A_POCKETWATCH = 106;

	public const int A_3D_MODEL = 107;

	public const int A_LARGE_BLACKROCK_GEM = 108;

	public const int A_NULL_TRAP = 109;

	public const int AN_ORB_ROCK = 110;

	public const int AN_EXPLODING_BOOK = 111;

	public const int A_MAGIC_PROJECTILE = 112;

	public const int A_MOVING_DOOR = 113;

	public const int A_PRESSURE_TRIGGER = 114;

	public const int A_CLOSE_TRIGGER = 115;

	public const int A_BLACKROCK_GEM = 116;

	public const int AN_ENTER_TRIGGER = 117;

	public const int A_JUMP_TRAP = 118;

	public const int A_SKILL_TRAP = 119;

	public const int AN_EXIT_TRIGGER = 120;

	public const int UNIMPLEMENTED_TRAP = 121;

	public const int A_STORAGECRYSTAL = 122;

	public const int NPC_WISP = 123;

	public const int NPC_VOID = 124;

	public const int DREAM_PLANT = 125;

	public const int BED = 126;

	public const int ARROW = 127;

	public const int A_PROXIMITY_TRAP = 128;

	public const int BOUNCING_PROJECTILE = 129;

	public const int MAPPIECE = 130;

	public const int SPECIAL_EFFECT = 131;

	public const int DRINK = 132;

	public const int A_BRIDGE_TRAP = 133;

	public const int A_DJINN_BOTTLE = 134;

	public const int SHOCK_TRIGGER_ENTRY = 1000;

	public const int SHOCK_TRIGGER_NULL = 1001;

	public const int SHOCK_TRIGGER_FLOOR = 1002;

	public const int SHOCK_TRIGGER_PLAYER_DEATH = 1003;

	public const int SHOCK_TRIGGER_DEATHWATCH = 1004;

	public const int SHOCK_TRIGGER_AOE_ENTRY = 1005;

	public const int SHOCK_TRIGGER_AOE_CONTINOUS = 1006;

	public const int SHOCK_TRIGGER_AI_HINT = 1007;

	public const int SHOCK_TRIGGER_LEVEL = 1008;

	public const int SHOCK_TRIGGER_CONTINUOUS = 1009;

	public const int SHOCK_TRIGGER_REPULSOR = 1010;

	public const int SHOCK_TRIGGER_ECOLOGY = 1011;

	public const int SHOCK_TRIGGER_SHODAN = 1012;

	public const int SHOCK_TRIGGER_TRIPBEAM = 1013;

	public const int SHOCK_TRIGGER_BIOHAZARD = 1014;

	public const int SHOCK_TRIGGER_RADHAZARD = 1015;

	public const int SHOCK_TRIGGER_CHEMHAZARD = 1016;

	public const int SHOCK_TRIGGER_MAPNOTE = 1017;

	public const int SHOCK_TRIGGER_MUSIC = 1018;

	public const int ACTION_DO_NOTHING = 0;

	public const int ACTION_TRANSPORT_LEVEL = 1;

	public const int ACTION_RESURRECTION = 2;

	public const int ACTION_CLONE = 3;

	public const int ACTION_SET_VARIABLE = 4;

	public const int ACTION_ACTIVATE = 6;

	public const int ACTION_LIGHTING = 7;

	public const int ACTION_EFFECT = 8;

	public const int ACTION_MOVING_PLATFORM = 9;

	public const int ACTION_TIMER = 11;

	public const int ACTION_CHOICE = 12;

	public const int ACTION_EMAIL = 15;

	public const int ACTION_RADAWAY = 16;

	public const int ACTION_CHANGE_STATE = 19;

	public const int ACTION_AWAKEN = 21;

	public const int ACTION_MESSAGE = 22;

	public const int ACTION_SPAWN = 23;

	public const int ACTION_CHANGE_TYPE = 24;

	public const int HEADINGNORTH = 180;

	public const int HEADINGSOUTH = 0;

	public const int HEADINGEAST = 270;

	public const int HEADINGWEST = 90;

	public const int HEADINGNORTHEAST = 225;

	public const int HEADINGSOUTHEAST = 315;

	public const int HEADINGNORTHWEST = 135;

	public const int HEADINGSOUTHWEST = 45;

	[Header("UW Static Properties")]
	public int item_id;

	public short flags;

	public short enchantment;

	public short doordir;

	public short invis;

	public short isquant;

	public short zpos;

	public short heading;

	public short xpos;

	public short ypos;

	public short quality;

	public int next;

	public short owner;

	public int link;

	[Header("UW Mobile Properties")]
	public short npc_whoami;

	public short npc_voidanim;

	public short npc_xhome;

	public short npc_yhome;

	public short npc_hunger;

	public short npc_health;

	public short npc_hp;

	public short npc_arms;

	public short npc_power;

	public short npc_goal;

	public short npc_attitude;

	public short npc_gtarg;

	public short npc_heading;

	public short npc_talkedto;

	public short npc_level;

	public short npc_name;

	public short npc_height;

	public short MobileUnk01;

	public short MobileUnk02;

	public short MobileUnk03;

	public short MobileUnk04;

	public short MobileUnk05;

	public short MobileUnk06;

	public short MobileUnk07;

	public short MobileUnk08;

	public short MobileUnk09;

	public short MobileUnk11;

	public short MobileUnk12;

	public short MobileUnk13;

	public short MobileUnk14;

	[Header("Projectile")]
	public short ProjectileHeadingMajor;

	public short ProjectileHeadingMinor;

	public short Projectile_Speed;

	public short Projectile_Pitch;

	public short Projectile_Sign;

	[Header("Display Settings")]
	public int WorldDisplayIndex;

	public int InvDisplayIndex;

	public bool ignoreSprite;

	public SpriteRenderer ObjectSprite = null;

	public bool animationStarted;

	[Header("Interaction Options")]
	public short inventorySlot = -1;

	[Header("Positioning")]
	public short ObjectTileX;

	public short ObjectTileY;

	public Vector3 startPos;

	[Header("Links")]
	public AudioSource aud;

	public Rigidbody rg;

	public ObjectLoaderInfo objectloaderinfo;

	public bool PickedUp
	{
		get
		{
			return base.gameObject.transform.parent.gameObject == GameWorldController.instance.InventoryMarker;
		}
		set
		{
			Debug.Log("obsolete pickup");
		}
	}

	public bool isEnchanted
	{
		get
		{
			return enchantment == 1;
		}
	}

	public bool isQuant
	{
		get
		{
			return isquant == 1 && link < 512;
		}
	}

	public bool CanBePickedUp
	{
		get
		{
			return GameWorldController.instance.commonObject.properties[item_id].FlagCanBePickedUp == 1 || GetComponent<object_base>().CanBePickedUp();
		}
	}

	public bool isUsable
	{
		get
		{
			return GameWorldController.instance.objectMaster.objProp[item_id].isUseable == 1;
		}
	}

	private void Start()
	{
		animationStarted = false;
		startPos = base.transform.position;
		if (ObjectSprite != null)
		{
			ObjectSprite.gameObject.SetActive(invis == 0);
		}
	}

	private void Update()
	{
		if (!animationStarted && !UseSprite() && invis == 0)
		{
			UpdateAnimation();
		}
	}

	public static void MoveToLinkedListChain(ObjectInteraction obj, TileInfo tNew)
	{
		if (tNew.indexObjectList == 0)
		{
			tNew.indexObjectList = obj.objectloaderinfo.index;
			Debug.Log("Putting " + obj.name + " at head of tile (" + tNew.tileX + "," + tNew.tileY + ")");
			return;
		}
		int num = tNew.indexObjectList;
		int num2 = 0;
		while (num != 0 && num2 <= 1024)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(num);
			if (objectIntAt != null)
			{
				if (objectIntAt.objectloaderinfo.index != obj.objectloaderinfo.index)
				{
					if (objectIntAt.next == 0)
					{
						Debug.Log("Chaining " + obj.name + " to " + objectIntAt.name);
						objectIntAt.next = obj.objectloaderinfo.index;
						num = 0;
					}
					else
					{
						num = objectIntAt.next;
					}
				}
				else
				{
					Debug.Log("object already in chain");
					num = 0;
				}
			}
			else
			{
				Debug.Log("Null object in chain");
				num = 0;
			}
			num2++;
		}
		if (num2 >= 1024)
		{
			Debug.Log("This chain looped " + num2 + " times" + obj.name);
		}
	}

	private static void MoveFromLinkedListChain(ObjectInteraction obj, TileInfo tOld)
	{
		if (tOld.indexObjectList == obj.objectloaderinfo.index)
		{
			tOld.indexObjectList = obj.next;
			Debug.Log("Removing " + obj.name + " at head of tile (" + tOld.tileX + "," + tOld.tileY + ")");
			return;
		}
		int num = 0;
		int num2 = tOld.indexObjectList;
		while (num2 != 0 && num <= 1024)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(num2);
			if (objectIntAt.next == obj.objectloaderinfo.index)
			{
				Debug.Log("DeChaining " + obj.name + " from " + objectIntAt.name);
				objectIntAt.next = obj.next;
				num2 = 0;
			}
			else if (num2 != objectIntAt.next)
			{
				num2 = objectIntAt.next;
			}
			else
			{
				Debug.Log("possibly looping chaing");
				num2 = 0;
			}
			num++;
		}
		if (num >= 1024)
		{
			Debug.Log("This chain looped " + num + " times (MoveFromLinkedListChain)");
		}
	}

	public void UpdateAnimation()
	{
		if (ObjectSprite == null)
		{
			ObjectSprite = GetComponentInChildren<SpriteRenderer>();
		}
		if (ObjectSprite != null)
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "SHOCK")
			{
				ObjectSprite.sprite = GameWorldController.instance.ObjectArt.RequestSprite(WorldDisplayIndex, GameWorldController.instance.ShockObjProp.properties[item_id].Offset);
			}
			else
			{
				ObjectSprite.sprite = GameWorldController.instance.ObjectArt.RequestSprite(WorldDisplayIndex);
			}
			if (inventorySlot != -1)
			{
				UWCharacter.Instance.playerInventory.Refresh();
			}
			animationStarted = true;
		}
	}

	public Sprite GetInventoryDisplay()
	{
		return GameWorldController.instance.ObjectArt.RequestSprite(InvDisplayIndex);
	}

	public Sprite GetEquipDisplay()
	{
		return GetComponent<object_base>().GetEquipDisplay();
	}

	public Sprite GetWorldDisplay()
	{
		return ObjectSprite.sprite;
	}

	public void SetWorldDisplay(Sprite NewSprite)
	{
		if (ObjectSprite != null)
		{
			ObjectSprite.sprite = NewSprite;
		}
	}

	public void RefreshAnim()
	{
		animationStarted = false;
	}

	public int GetItemType()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].type;
	}

	public bool Attack(short damage, GameObject source)
	{
		GetComponent<object_base>().ApplyAttack(damage, source);
		return true;
	}

	public string LookDescriptionContext()
	{
		object_base component = GetComponent<object_base>();
		if (component != null)
		{
			return component.GetContextMenuText(item_id, isUsable && WindowDetect.ContextUIUse, CanBePickedUp && WindowDetect.ContextUIUse, UWEBase.CurrentObjectInHand != null && Character.InteractionMode != 2);
		}
		return "";
	}

	public string UseVerb()
	{
		return GetComponent<object_base>().UseVerb();
	}

	public string PickupVerb()
	{
		return GetComponent<object_base>().PickupVerb();
	}

	public string ExamineVerb()
	{
		return GetComponent<object_base>().ExamineVerb();
	}

	public string UseObjectOnVerb_World()
	{
		return GetComponent<object_base>().UseObjectOnVerb_World();
	}

	public string UseObjectOnVerb_Inv()
	{
		return GetComponent<object_base>().UseObjectOnVerb_Inv();
	}

	public bool LookDescription()
	{
		object_base component = GetComponent<object_base>();
		if (component != null)
		{
			return component.LookAt();
		}
		return false;
	}

	public bool Use()
	{
		if (UWEBase.CurrentObjectInHand != null)
		{
			ObjectInteraction objectInteraction = CombineObject(this, UWEBase.CurrentObjectInHand);
			if (objectInteraction != null)
			{
				UWEBase.CurrentObjectInHand = objectInteraction;
				return true;
			}
		}
		object_base component = GetComponent<object_base>();
		if (component != null)
		{
			return component.use();
		}
		return false;
	}

	public bool Pickup()
	{
		object_base object_base2 = null;
		object_base2 = GetComponent<object_base>();
		if (TileMap.ValidTile(ObjectTileX, ObjectTileY) && UWEBase.CurrentTileMap().Tiles[ObjectTileX, ObjectTileY].PressureTriggerIndex != 0)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(UWEBase.CurrentTileMap().Tiles[ObjectTileX, ObjectTileY].PressureTriggerIndex);
			if (objectIntAt.GetComponent<a_pressure_trigger>() != null)
			{
				objectIntAt.GetComponent<a_pressure_trigger>().ReleaseWeightFrom();
			}
		}
		if (object_base2 != null)
		{
			return object_base2.PickupEvent();
		}
		return false;
	}

	public bool Drop()
	{
		object_base object_base2 = null;
		object_base2 = GetComponent<object_base>();
		if (object_base2 != null)
		{
			return object_base2.DropEvent();
		}
		return false;
	}

	public bool PutItemAway(short SlotNo)
	{
		inventorySlot = SlotNo;
		object_base object_base2 = null;
		object_base2 = GetComponent<object_base>();
		if (object_base2 != null)
		{
			return object_base2.PutItemAwayEvent(SlotNo);
		}
		return false;
	}

	public bool Equip(short SlotNo)
	{
		object_base component = GetComponent<object_base>();
		inventorySlot = SlotNo;
		if (component != null)
		{
			return component.EquipEvent(SlotNo);
		}
		return false;
	}

	public bool UnEquip(short SlotNo)
	{
		object_base component = GetComponent<object_base>();
		inventorySlot = -1;
		if (component != null)
		{
			return component.UnEquipEvent(SlotNo);
		}
		return false;
	}

	public bool TalkTo()
	{
		object_base component = GetComponent<object_base>();
		return component.TalkTo();
	}

	public bool FailMessage()
	{
		object_base component = GetComponent<object_base>();
		return component.FailMessage();
	}

	public ObjectInteraction CombineObject(ObjectInteraction InputObject1, ObjectInteraction InputObject2)
	{
		int[] array = new int[8];
		int[] array2 = new int[8];
		int[] array3 = new int[8];
		int[] array4 = new int[8];
		int[] array5 = new int[8];
		int num = InputObject1.item_id;
		int num2 = InputObject2.item_id;
		bool flag = false;
		bool flag2 = false;
		switch (UWEBase._RES)
		{
		case "UW0":
		case "UW1":
			array = new int[9] { 149, 225, 225, 226, 225, 226, 227, 149, 284 };
			array4 = new int[9] { 0, 1, 1, 1, 1, 1, 1, 0, 1 };
			array2 = new int[9] { 278, 226, 227, 227, 229, 228, 230, 180, 216 };
			array5 = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
			array3 = new int[9] { 277, 230, 228, 229, 231, 231, 231, 183, 299 };
			break;
		case "UW2":
			array = new int[5] { 216, 300, 149, 149, 191 };
			array4 = new int[5] { 1, 1, 0, 0, 1 };
			array2 = new int[5] { 300, 300, 180, 186, 188 };
			array5 = new int[5] { 1, 1, 1, 1, 1 };
			array3 = new int[5] { 299, 146, 183, 210, 187 };
			break;
		}
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			if ((num == array[i] && num2 == array2[i]) || (num2 == array[i] && num == array2[i]))
			{
				Debug.Log("Creating a " + array3[i]);
				if (array[i] == num && array4[i] == 1 && !flag)
				{
					Debug.Log("Destroying " + InputObject1.name);
					flag = true;
				}
				if (array[i] == num2 && array4[i] == 1 && !flag2)
				{
					Debug.Log("Destroying " + InputObject2.name);
					flag2 = true;
				}
				if (array2[i] == num && array5[i] == 1 && !flag)
				{
					Debug.Log("Destroying " + InputObject1.name);
					flag = true;
				}
				if (array2[i] == num2 && array5[i] == 1 && !flag2)
				{
					Debug.Log("Destroying " + InputObject2.name);
					flag2 = true;
				}
				if (flag)
				{
					InputObject1.consumeObject();
				}
				if (flag2)
				{
					InputObject2.consumeObject();
				}
				ObjectLoaderInfo currObj = ObjectLoader.newObject(array3[i], 40, 0, 0, 256);
				ObjectInteraction objectInteraction = CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.InventoryMarker.gameObject, GameWorldController.instance.InventoryMarker.transform.position);
				GameWorldController.MoveToInventory(objectInteraction);
				Character.InteractionMode = 2;
				objectInteraction.UpdateAnimation();
				InteractionModeControl.UpdateNow = true;
				return objectInteraction;
			}
		}
		return null;
	}

	public void consumeObject()
	{
		if (!isQuant || (isQuant && link == 1) || isEnchanted)
		{
			GetComponent<object_base>().DestroyEvent();
			Container currentContainer = UWCharacter.Instance.playerInventory.GetCurrentContainer();
			if (!currentContainer.RemoveItemFromContainer(this))
			{
				UWCharacter.Instance.playerInventory.RemoveItemFromEquipment(this);
			}
			if (UWEBase.CurrentObjectInHand == this)
			{
				UWEBase.CurrentObjectInHand = null;
			}
			UWCharacter.Instance.playerInventory.Refresh();
			objectloaderinfo.InUseFlag = 0;
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			link--;
			Split(this);
			UWCharacter.Instance.playerInventory.Refresh();
		}
	}

	public int GetHitFrameStart()
	{
		if (GetComponent<NPC>() == null)
		{
			return 45;
		}
		switch (GameWorldController.instance.objDat.critterStats[item_id - 64].Blood)
		{
		case 0:
			return 45;
		default:
			return 0;
		}
	}

	public int GetHitFrameEnd()
	{
		if (GetComponent<NPC>() == null)
		{
			return 49;
		}
		switch (GameWorldController.instance.objDat.critterStats[item_id - 64].Blood)
		{
		case 0:
			return 49;
		default:
			return 5;
		}
	}

	public int GetQty()
	{
		if (isEnchanted || GetComponent<Readable>() != null)
		{
			return 1;
		}
		if (isQuant)
		{
			return link;
		}
		return 1;
	}

	public float GetWeight()
	{
		return GetComponent<object_base>().GetWeight();
	}

	public static SpriteRenderer CreateObjectGraphics(GameObject myObj, string AssetPath, bool BillBoard)
	{
		GameObject gameObject = new GameObject("_sprite");
		gameObject.transform.position = myObj.transform.position;
		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		Sprite sprite = Resources.Load<Sprite>(AssetPath);
		spriteRenderer.sprite = sprite;
		gameObject.transform.parent = myObj.transform;
		gameObject.transform.Rotate(0f, 0f, 0f);
		gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
		spriteRenderer.material = Resources.Load<Material>("Materials/SpriteShader");
		if (BillBoard)
		{
			gameObject.AddComponent<Billboard>();
		}
		return spriteRenderer;
	}

	private static ObjectInteraction CreateObjectInteraction(GameObject myObj, float DimX, float DimY, float DimZ, ObjectLoaderInfo currObj)
	{
		ObjectInteraction objectInteraction = myObj.AddComponent<ObjectInteraction>();
		BoxCollider component = myObj.GetComponent<BoxCollider>();
		if (component == null && objectInteraction.GetItemType() != 0 && objectInteraction.isUsable)
		{
			component = myObj.AddComponent<BoxCollider>();
			component.size = new Vector3(0.2f, 0.2f, 0.2f);
			component.center = new Vector3(0f, 0.08f, 0f);
			if (objectInteraction.isMoveable())
			{
				component.material = Resources.Load<PhysicMaterial>("Materials/objects_bounce");
			}
		}
		objectInteraction.WorldDisplayIndex = objectInteraction.WorldIndex();
		objectInteraction.InvDisplayIndex = objectInteraction.InventoryIndex();
		objectInteraction.item_id = currObj.item_id;
		objectInteraction.link = currObj.link;
		objectInteraction.quality = currObj.quality;
		objectInteraction.owner = currObj.owner;
		objectInteraction.flags = currObj.flags;
		objectInteraction.InvDisplayIndex = GameWorldController.instance.objectMaster.objProp[currObj.item_id].InventoryIndex;
		objectInteraction.WorldDisplayIndex = GameWorldController.instance.objectMaster.objProp[currObj.item_id].WorldIndex;
		if (objectInteraction.isMoveable())
		{
			objectInteraction.rg = myObj.AddComponent<Rigidbody>();
			objectInteraction.rg.angularDrag = 0f;
			UWEBase.FreezeMovement(myObj);
		}
		objectInteraction.isquant = currObj.is_quant;
		objectInteraction.enchantment = currObj.enchantment;
		if (PlaySoundEffects && !ObjectLoader.isTrap(currObj) && !ObjectLoader.isTrigger(currObj))
		{
			objectInteraction.aud = myObj.AddComponent<AudioSource>();
			objectInteraction.aud.maxDistance = 1f;
			objectInteraction.aud.spatialBlend = 1f;
		}
		return objectInteraction;
	}

	public int AliasItemId()
	{
		return GetComponent<object_base>().AliasItemId();
	}

	public static int Alias(int id)
	{
		switch (id)
		{
		case 160:
			return 161;
		case 161:
			return 160;
		default:
			return id;
		}
	}

	public bool IsStackable()
	{
		return isQuant && !isEnchanted;
	}

	public static bool CanMerge(ObjectInteraction mergingInto, ObjectInteraction mergingFrom)
	{
		return (mergingInto.item_id == mergingFrom.item_id || mergingInto.AliasItemId() == mergingFrom.item_id || mergingInto.item_id == mergingFrom.AliasItemId()) && mergingInto.quality == mergingFrom.quality && ((mergingInto.GetItemType() == 5 && mergingFrom.GetItemType() == 5 && mergingInto.owner == mergingFrom.owner) || mergingInto.GetItemType() != 5 || mergingFrom.GetItemType() != 5);
	}

	public static void Merge(ObjectInteraction mergingInto, ObjectInteraction mergingFrom)
	{
		mergingInto.link += mergingFrom.link;
		mergingInto.isquant = 1;
		mergingInto.GetComponent<object_base>().MergeEvent();
		mergingFrom.objectloaderinfo.InUseFlag = 0;
		UnityEngine.Object.Destroy(mergingFrom.gameObject);
	}

	public static void Split(ObjectInteraction splitFrom, ObjectInteraction splitTo)
	{
		splitFrom.GetComponent<object_base>().Split();
		splitTo.GetComponent<object_base>().Split();
	}

	public static void Split(ObjectInteraction splitFrom)
	{
		splitFrom.GetComponent<object_base>().Split();
	}

	public virtual bool ChangeType(int newID)
	{
		item_id = newID;
		WorldDisplayIndex = newID;
		InvDisplayIndex = newID;
		UpdateAnimation();
		return true;
	}

	public static NPC CreateNPC(GameObject myObj, ObjectInteraction objInt, ObjectLoaderInfo objI)
	{
		myObj.layer = LayerMask.NameToLayer("NPCs");
		myObj.tag = "NPCs";
		NPC nPC = myObj.AddComponent<NPC>();
		GameObject gameObject = new GameObject("_NPC_Launcher");
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.parent = myObj.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0.5f, 0.3f);
		nPC.NPC_Launcher = gameObject;
		GameObject gameObject2 = new GameObject("_Sprite");
		gameObject2.transform.parent = myObj.transform;
		gameObject2.transform.position = myObj.transform.position;
		gameObject2.AddComponent<BillboardNPC>();
		SpriteRenderer spriteRenderer = gameObject2.AddComponent<SpriteRenderer>();
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			spriteRenderer.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		}
		else
		{
			spriteRenderer.transform.localScale = new Vector3(2f, 2f, 2f);
		}
		spriteRenderer.material = Resources.Load<Material>("Materials/SpriteShader");
		nPC.CharController = myObj.AddComponent<CharacterController>();
		SetNPCSizes(objInt, nPC, gameObject);
		nPC.CharController.stepOffset = 0.1f;
		return nPC;
	}

	private static void SetNPCSizes(ObjectInteraction objInt, NPC npc, GameObject NpcLauncher)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (objInt.item_id)
			{
			case 72:
			case 73:
			case 74:
			case 79:
			case 82:
			case 83:
			case 85:
			case 91:
			case 92:
			case 93:
			case 94:
			case 95:
			case 96:
			case 97:
			case 98:
			case 99:
			case 100:
			case 101:
			case 102:
			case 103:
			case 104:
			case 105:
			case 106:
			case 107:
			case 108:
			case 110:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 124:
			case 125:
			case 126:
			case 127:
				SetBigNPC(npc, NpcLauncher);
				break;
			case 71:
			case 75:
			case 76:
			case 77:
			case 81:
			case 84:
			case 87:
			case 88:
			case 89:
			case 90:
			case 109:
			case 111:
				SetMediumNPC(npc, NpcLauncher);
				break;
			case 64:
			case 65:
			case 66:
			case 67:
			case 68:
			case 69:
			case 70:
			case 78:
			case 80:
			case 86:
				SetSmallNPC(npc, NpcLauncher);
				break;
			default:
				Debug.Log("unimplemented npc");
				break;
			}
		}
		else
		{
			switch (objInt.item_id)
			{
			case 70:
			case 71:
			case 74:
			case 76:
			case 77:
			case 78:
			case 79:
			case 80:
			case 84:
			case 85:
			case 86:
			case 88:
			case 89:
			case 90:
			case 91:
			case 93:
			case 94:
			case 95:
			case 96:
			case 97:
			case 98:
			case 99:
			case 100:
			case 101:
			case 103:
			case 104:
			case 105:
			case 106:
			case 107:
			case 108:
			case 109:
			case 110:
			case 111:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 123:
			case 124:
			case 125:
			case 126:
				SetBigNPC(npc, NpcLauncher);
				break;
			case 67:
			case 68:
			case 72:
			case 75:
			case 81:
			case 83:
			case 92:
			case 102:
				SetMediumNPC(npc, NpcLauncher);
				break;
			case 64:
			case 65:
			case 66:
			case 69:
			case 73:
			case 82:
			case 87:
			case 122:
				SetSmallNPC(npc, NpcLauncher);
				break;
			}
		}
	}

	private static void SetSmallNPC(NPC npc, GameObject NpcLauncher)
	{
		npc.CharController.isTrigger = false;
		npc.CharController.center = new Vector3(0f, 0.3f, 0f);
		NpcLauncher.transform.localPosition = new Vector3(0f, 0.15f, 0.2f);
		npc.CharController.radius = 0.3f;
		npc.CharController.height = 0.6f;
		npc.CharController.skinWidth = 0.02f;
	}

	private static void SetMediumNPC(NPC npc, GameObject NpcLauncher)
	{
		npc.CharController.isTrigger = false;
		npc.CharController.center = new Vector3(0f, 0.3f, 0f);
		npc.CharController.radius = 0.3f;
		npc.CharController.height = 0.7f;
		npc.CharController.skinWidth = 0.02f;
		NpcLauncher.transform.localPosition = new Vector3(0f, 0.3f, 0.2f);
	}

	private static void SetBigNPC(NPC npc, GameObject NpcLauncher)
	{
		npc.CharController.isTrigger = false;
		npc.CharController.center = new Vector3(0f, 0.55f, 0f);
		npc.CharController.radius = 0.3f;
		npc.CharController.height = 1f;
		npc.CharController.skinWidth = 0.02f;
		NpcLauncher.transform.localPosition = new Vector3(0f, 0.5f, 0.2f);
	}

	public static void SetMobileProps(GameObject myObj, ObjectInteraction objInt, ObjectLoaderInfo objI)
	{
		objInt.npc_whoami = objI.npc_whoami;
		objInt.npc_voidanim = objI.npc_voidanim;
		objInt.npc_xhome = objI.npc_xhome;
		objInt.npc_yhome = objI.npc_yhome;
		objInt.npc_hunger = objI.npc_hunger;
		objInt.npc_health = objI.npc_health;
		objInt.npc_hp = objI.npc_hp;
		objInt.npc_arms = objI.npc_arms;
		objInt.npc_power = objI.npc_power;
		objInt.npc_goal = objI.npc_goal;
		objInt.npc_attitude = objI.npc_attitude;
		objInt.npc_gtarg = objI.npc_gtarg;
		objInt.npc_talkedto = objI.npc_talkedto;
		objInt.npc_level = objI.npc_level;
		objInt.npc_name = objI.npc_name;
		objInt.npc_heading = objI.npc_heading;
		objInt.Projectile_Speed = objI.Projectile_Speed;
		objInt.Projectile_Pitch = objI.Projectile_Pitch;
		objInt.ProjectileHeadingMinor = objI.ProjectileHeadingMinor;
		objInt.npc_height = objI.npc_height;
		objInt.ProjectileHeadingMajor = objI.ProjectileHeadingMajor;
		objInt.MobileUnk01 = objI.MobileUnk01;
		objInt.MobileUnk02 = objI.MobileUnk02;
		objInt.MobileUnk03 = objI.MobileUnk03;
		objInt.MobileUnk04 = objI.MobileUnk04;
		objInt.MobileUnk05 = objI.MobileUnk05;
		objInt.MobileUnk06 = objI.MobileUnk06;
		objInt.MobileUnk07 = objI.MobileUnk07;
		objInt.MobileUnk08 = objI.MobileUnk08;
		objInt.MobileUnk09 = objI.MobileUnk09;
		objInt.Projectile_Sign = objI.Projectile_Sign;
		objInt.MobileUnk11 = objI.MobileUnk11;
		objInt.MobileUnk12 = objI.MobileUnk12;
		objInt.MobileUnk13 = objI.MobileUnk13;
		objInt.MobileUnk14 = objI.MobileUnk14;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (PlaySoundEffects && rg != null && rg.useGravity && aud != null)
		{
			aud.clip = MusicController.instance.SoundEffects[0];
			aud.Play();
		}
	}

	public override Vector3 GetImpactPoint()
	{
		object_base component = GetComponent<object_base>();
		return component.GetImpactPoint();
	}

	public virtual GameObject GetImpactGameObject()
	{
		object_base component = GetComponent<object_base>();
		return component.GetImpactGameObject();
	}

	public void UpdatePosition()
	{
		if (objectloaderinfo == null)
		{
			Debug.Log(base.name + " has no objectloaderinfo");
		}
		else
		{
			if (ObjectLoader.isStatic(objectloaderinfo))
			{
				return;
			}
			ObjectTileX = (short)Mathf.FloorToInt(base.transform.localPosition.x / 1.2f);
			ObjectTileY = (short)Mathf.FloorToInt(base.transform.localPosition.z / 1.2f);
			if (ObjectTileX != 99)
			{
				float num = UWEBase.CurrentTileMap().CEILING_HEIGHT;
				if ((ObjectTileX > 63) | (ObjectTileX < 0))
				{
					ObjectTileX = 99;
				}
				if ((ObjectTileY > 63) | (ObjectTileY < 0))
				{
					ObjectTileY = 99;
				}
				int itemType = GetItemType();
				if (itemType != 4 && itemType != 29)
				{
					short num2 = (short)(base.transform.localPosition.y * 100f / 15f / num * 128f);
					num2 = (short)Mathf.Min(Mathf.Ceil(num2), 128f);
					if (Mathf.Abs(num2 - zpos) > 2)
					{
						zpos = num2;
					}
				}
				if (ObjectTileX < 99 && ObjectTileY < 99)
				{
					float num3 = base.transform.position.x - (float)ObjectTileX * 1.2f;
					xpos = (short)(7f * (num3 / 1.2f));
					float num4 = base.transform.position.z - (float)ObjectTileY * 1.2f;
					ypos = (short)(7f * (num4 / 1.2f));
				}
				heading = (short)Mathf.RoundToInt(base.transform.rotation.eulerAngles.y / 45f);
			}
			objectloaderinfo.heading = heading;
			objectloaderinfo.xpos = xpos;
			objectloaderinfo.ypos = ypos;
			objectloaderinfo.zpos = zpos;
			objectloaderinfo.ObjectTileX = ObjectTileX;
			objectloaderinfo.ObjectTileY = ObjectTileY;
			startPos = base.transform.position;
		}
	}

	public static ObjectInteraction CreateNewObject(TileMap tm, ObjectLoaderInfo currObj, ObjectLoaderInfo[] objList, GameObject parent, Vector3 position)
	{
		GameObject gameObject = new GameObject(ObjectLoader.UniqueObjectName(currObj));
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		gameObject.transform.localPosition = position;
		gameObject.transform.Rotate(0f, 0f, 0f);
		gameObject.transform.parent = parent.transform;
		gameObject.layer = LayerMask.NameToLayer("UWObjects");
		ObjectInteraction objectInteraction = CreateObjectInteraction(gameObject, 0.5f, 0.5f, 0.5f, currObj);
		objectInteraction.objectloaderinfo = currObj;
		currObj.instance = objectInteraction;
		objectInteraction.link = currObj.link;
		objectInteraction.quality = currObj.quality;
		objectInteraction.enchantment = currObj.enchantment;
		objectInteraction.doordir = currObj.doordir;
		objectInteraction.invis = currObj.invis;
		objectInteraction.zpos = currObj.zpos;
		objectInteraction.xpos = currObj.xpos;
		objectInteraction.ypos = currObj.ypos;
		objectInteraction.heading = currObj.heading;
		objectInteraction.zpos = currObj.zpos;
		objectInteraction.owner = currObj.owner;
		objectInteraction.ObjectTileX = currObj.ObjectTileX;
		objectInteraction.ObjectTileY = currObj.ObjectTileY;
		objectInteraction.objectloaderinfo = currObj;
		objectInteraction.next = currObj.next;
		switch (currObj.GetItemType())
		{
		case 0:
			flag = false;
			CreateNPC(gameObject, objectInteraction, currObj);
			gameObject.AddComponent<Container>();
			break;
		case 123:
			gameObject.AddComponent<NPC_Wisp>();
			flag4 = true;
			break;
		case 124:
			gameObject.AddComponent<NPC_VoidCreature>();
			break;
		case 4:
		case 29:
		case 30:
			gameObject.AddComponent<DoorControl>();
			DoorControl.CreateDoor(gameObject, objectInteraction);
			gameObject.transform.Rotate(-90f, (float)objectInteraction.heading * 45f - 180f, 0f, Space.World);
			flag2 = true;
			flag = false;
			break;
		case 19:
			gameObject.AddComponent<container_obj>();
			switch (objectInteraction.item_id)
			{
			case 349:
				gameObject.AddComponent<Chest>();
				gameObject.GetComponent<Container>().items = new ObjectInteraction[40];
				gameObject.AddComponent<Container>();
				flag = false;
				break;
			case 347:
				gameObject.AddComponent<Barrel>();
				gameObject.GetComponent<Container>().items = new ObjectInteraction[40];
				gameObject.AddComponent<Container>();
				flag = false;
				break;
			default:
				gameObject.AddComponent<Container>();
				gameObject.GetComponent<Container>().items = new ObjectInteraction[GameWorldController.instance.objDat.containerStats[currObj.item_id - 128].capacity + 1];
				gameObject.AddComponent<Container>();
				break;
			}
			break;
		case 5:
			gameObject.AddComponent<DoorKey>();
			break;
		case 11:
		case 13:
			if (UWEBase._RES == "UW1" && objectInteraction.item_id == 276)
			{
				gameObject.AddComponent<ReadableTrap>();
			}
			else if (objectInteraction.isEnchanted && objectInteraction.link != 0)
			{
				gameObject.AddComponent<MagicScroll>();
			}
			else
			{
				gameObject.AddComponent<Readable>();
			}
			break;
		case 10:
			flag3 = true;
			gameObject.AddComponent<Sign>();
			break;
		case 6:
			gameObject.AddComponent<RuneStone>();
			break;
		case 70:
			gameObject.AddComponent<RuneBag>();
			break;
		case 24:
		case 132:
			gameObject.AddComponent<Food>();
			break;
		case 23:
			if (objectInteraction.isMagicallyEnchanted(currObj, objList) && objectInteraction.link > 1)
			{
				gameObject.AddComponent<Wand>();
			}
			else
			{
				gameObject.AddComponent<object_base>();
			}
			break;
		case 28:
			gameObject.AddComponent<Map>();
			break;
		case 73:
		{
			Helm helm = gameObject.AddComponent<Helm>();
			helm.UpdateQuality();
			break;
		}
		case 2:
		{
			Armour armour = gameObject.AddComponent<Armour>();
			armour.UpdateQuality();
			break;
		}
		case 76:
		{
			Gloves gloves = gameObject.AddComponent<Gloves>();
			gloves.UpdateQuality();
			break;
		}
		case 75:
		{
			Boots boots = gameObject.AddComponent<Boots>();
			boots.UpdateQuality();
			break;
		}
		case 77:
		{
			Leggings leggings = gameObject.AddComponent<Leggings>();
			leggings.UpdateQuality();
			break;
		}
		case 78:
			gameObject.AddComponent<Shield>();
			break;
		case 1:
			switch (objectInteraction.item_id)
			{
			case 24:
			case 25:
			case 26:
			case 31:
				gameObject.AddComponent<WeaponRanged>();
				break;
			default:
				gameObject.AddComponent<WeaponMelee>();
				break;
			}
			break;
		case 22:
			gameObject.AddComponent<LightSource>();
			break;
		case 88:
			gameObject.AddComponent<Lantern>();
			break;
		case 74:
			gameObject.AddComponent<Ring>();
			break;
		case 14:
			gameObject.AddComponent<Potion>();
			break;
		case 79:
			gameObject.AddComponent<LockPick>();
			break;
		case 81:
			if (currObj.item_id == 458)
			{
				gameObject.AddComponent<SilverTree>();
				UWCharacter.Instance.ResurrectPosition = gameObject.transform.position;
				flag4 = true;
			}
			else
			{
				gameObject.AddComponent<SilverSeed>();
			}
			break;
		case 84:
			gameObject.AddComponent<Grave>();
			flag = false;
			break;
		case 83:
			gameObject.AddComponent<Shrine>();
			flag = false;
			break;
		case 85:
			gameObject.AddComponent<Anvil>();
			break;
		case 86:
			gameObject.AddComponent<Pole>();
			break;
		case 87:
			gameObject.AddComponent<Spike>();
			break;
		case 89:
			gameObject.AddComponent<Oil>();
			break;
		case 12:
			gameObject.AddComponent<Wand>();
			break;
		case 90:
			gameObject.AddComponent<MoonStone>();
			UWCharacter.Instance.MoonGatePosition = gameObject.transform.position;
			break;
		case 96:
			gameObject.AddComponent<MoonGate>();
			flag = false;
			break;
		case 91:
			gameObject.AddComponent<Leech>();
			break;
		case 92:
			gameObject.AddComponent<FishingPole>();
			break;
		case 93:
		{
			gameObject.AddComponent<Zanium>();
			gameObject.layer = LayerMask.NameToLayer("Zanium");
			gameObject.GetComponent<BoxCollider>().isTrigger = true;
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(0.1f, 0.1f, 0.1f);
			boxCollider.center = new Vector3(0f, 0.05f, 0f);
			break;
		}
		case 26:
			gameObject.AddComponent<Instrument>();
			break;
		case 94:
			gameObject.AddComponent<Bedroll>();
			break;
		case 126:
			gameObject.AddComponent<Bed>();
			flag = false;
			break;
		case 18:
			gameObject.AddComponent<Coin>();
			break;
		case 97:
			gameObject.AddComponent<Boulder>();
			flag = false;
			break;
		case 98:
			gameObject.AddComponent<Orb>();
			break;
		case 106:
			gameObject.AddComponent<PocketWatch>();
			break;
		case 107:
			gameObject.AddComponent<GenericModel3D>();
			flag = false;
			break;
		case 108:
			gameObject.AddComponent<LargeBlackrockGem>();
			flag = false;
			break;
		case 116:
			gameObject.AddComponent<BlackrockGem>();
			break;
		case 110:
			gameObject.layer = LayerMask.NameToLayer("MagicProjectile");
			gameObject.AddComponent<OrbRock>();
			break;
		case 111:
			gameObject.AddComponent<ReadableTrap>();
			break;
		case 68:
			flag = false;
			gameObject.AddComponent<UWPainting>();
			break;
		case 31:
			gameObject.AddComponent<Pillar>();
			flag = false;
			break;
		case 122:
			gameObject.AddComponent<StorageCrystal>();
			break;
		case 125:
			gameObject.AddComponent<DreamPlant>();
			break;
		case 95:
			gameObject.AddComponent<forcefield>();
			break;
		case 130:
			gameObject.AddComponent<MapPiece>();
			break;
		case 134:
			gameObject.AddComponent<DjinnBottle>();
			break;
		case 127:
			gameObject.AddComponent<Arrow>();
			flag = false;
			break;
		case 112:
		{
			flag2 = true;
			MagicProjectile magicProjectile = gameObject.AddComponent<MagicProjectile>();
			if (!GameWorldController.LoadingObjects)
			{
				break;
			}
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			component.size = new Vector3(0.2f, 0.2f, 0.2f);
			component.center = new Vector3(0f, 0.1f, 0f);
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				switch (currObj.item_id)
				{
				case 20:
				{
					SpellProp_Fireball spellProp_Fireball2 = new SpellProp_Fireball();
					spellProp_Fireball2.init(83, gameObject);
					magicProjectile.spellprop = spellProp_Fireball2;
					break;
				}
				case 21:
				{
					SpellProp_Fireball spellProp_Fireball = new SpellProp_Fireball();
					spellProp_Fireball.init(82, gameObject);
					magicProjectile.spellprop = spellProp_Fireball;
					break;
				}
				case 22:
				{
					SpellProp_Acid spellProp_Acid = new SpellProp_Acid();
					spellProp_Acid.init(321, gameObject);
					magicProjectile.spellprop = spellProp_Acid;
					break;
				}
				case 27:
				{
					SpellProp_Homing spellProp_Homing = new SpellProp_Homing();
					spellProp_Homing.init(85, gameObject);
					magicProjectile.spellprop = spellProp_Homing;
					break;
				}
				default:
				{
					SpellProp_MagicArrow spellProp_MagicArrow = new SpellProp_MagicArrow();
					spellProp_MagicArrow.init(81, gameObject);
					magicProjectile.spellprop = spellProp_MagicArrow;
					break;
				}
				}
			}
			else
			{
				switch (currObj.item_id)
				{
				case 20:
				{
					SpellProp_Fireball spellProp_Fireball4 = new SpellProp_Fireball();
					spellProp_Fireball4.init(83, gameObject);
					magicProjectile.spellprop = spellProp_Fireball4;
					break;
				}
				case 21:
				{
					SpellProp_Fireball spellProp_Fireball3 = new SpellProp_Fireball();
					spellProp_Fireball3.init(82, gameObject);
					magicProjectile.spellprop = spellProp_Fireball3;
					break;
				}
				case 22:
				{
					SpellProp_Acid spellProp_Acid2 = new SpellProp_Acid();
					spellProp_Acid2.init(305, gameObject);
					magicProjectile.spellprop = spellProp_Acid2;
					break;
				}
				default:
				{
					SpellProp_MagicArrow spellProp_MagicArrow2 = new SpellProp_MagicArrow();
					spellProp_MagicArrow2.init(81, gameObject);
					magicProjectile.spellprop = spellProp_MagicArrow2;
					break;
				}
				}
			}
			if (magicProjectile.spellprop.homing)
			{
				magicProjectile.BeginHoming();
			}
			if (magicProjectile.spellprop.hasTrail)
			{
				magicProjectile.BeginVapourTrail();
			}
			break;
		}
		case 129:
			flag2 = true;
			gameObject.AddComponent<BouncingProjectile>();
			break;
		case 8:
			gameObject.AddComponent<ButtonHandler>();
			flag3 = true;
			break;
		case 54:
		case 58:
			gameObject.AddComponent<a_move_trigger>();
			flag = false;
			break;
		case 117:
			gameObject.AddComponent<an_enter_trigger>();
			flag = false;
			break;
		case 120:
			gameObject.AddComponent<an_exit_trigger>();
			flag = false;
			break;
		case 55:
			gameObject.AddComponent<a_pick_up_trigger>();
			flag = false;
			break;
		case 56:
			gameObject.AddComponent<a_use_trigger>();
			flag = false;
			break;
		case 59:
			gameObject.AddComponent<an_open_trigger>();
			flag = false;
			break;
		case 115:
			gameObject.AddComponent<a_close_trigger>();
			flag = false;
			break;
		case 60:
			gameObject.AddComponent<an_unlock_trigger>();
			flag = false;
			break;
		case 57:
			gameObject.AddComponent<a_look_trigger>();
			flag = false;
			break;
		case 101:
			gameObject.AddComponent<a_timer_trigger>();
			flag = false;
			break;
		case 102:
			gameObject.AddComponent<a_scheduled_trigger>();
			flag = false;
			break;
		case 114:
			gameObject.AddComponent<a_pressure_trigger>();
			flag = false;
			break;
		case 37:
			gameObject.AddComponent<a_damage_trap>();
			flag = false;
			break;
		case 38:
			gameObject.AddComponent<a_teleport_trap>();
			flag = false;
			break;
		case 39:
			gameObject.AddComponent<a_arrow_trap>();
			flag = false;
			break;
		case 41:
			gameObject.AddComponent<a_pit_trap>();
			flag = false;
			break;
		case 42:
			gameObject.AddComponent<a_change_terrain_trap>();
			flag = false;
			break;
		case 43:
			gameObject.AddComponent<a_spelltrap>();
			flag = false;
			break;
		case 44:
			gameObject.AddComponent<a_create_object_trap>();
			flag = false;
			break;
		case 45:
			gameObject.AddComponent<a_door_trap>();
			flag = false;
			break;
		case 46:
			gameObject.AddComponent<a_ward_trap>();
			if (UWEBase._RES != "UW2")
			{
				flag = false;
			}
			break;
		case 47:
			gameObject.AddComponent<a_tell_trap>();
			flag = false;
			break;
		case 48:
			gameObject.AddComponent<a_delete_object_trap>();
			flag = false;
			break;
		case 49:
			gameObject.AddComponent<an_inventory_trap>();
			flag = false;
			break;
		case 50:
			gameObject.AddComponent<a_set_variable_trap>();
			flag = false;
			break;
		case 51:
			gameObject.AddComponent<a_check_variable_trap>();
			flag = false;
			break;
		case 52:
			gameObject.AddComponent<a_combination_trap>();
			flag = false;
			break;
		case 53:
			gameObject.AddComponent<a_text_string_trap>();
			flag = false;
			break;
		case 100:
			gameObject.AddComponent<an_oscillator_trap>();
			flag = false;
			break;
		case 103:
			gameObject.AddComponent<a_change_from_trap>();
			flag = false;
			break;
		case 104:
			gameObject.AddComponent<a_change_to_trap>();
			flag = false;
			break;
		case 105:
			gameObject.AddComponent<an_experience_trap>();
			flag = false;
			break;
		case 109:
			gameObject.AddComponent<a_null_trap>();
			flag = false;
			break;
		case 118:
			gameObject.AddComponent<a_jump_trap>();
			flag = false;
			break;
		case 119:
			gameObject.AddComponent<a_skill_trap>();
			flag = false;
			break;
		case 131:
			gameObject.AddComponent<a_special_effect_trap>();
			flag = false;
			break;
		case 121:
			Debug.Log("Unimplemented trap " + gameObject.name);
			gameObject.AddComponent<trap_base>();
			flag = false;
			break;
		case 128:
			gameObject.AddComponent<a_proximity_trap>();
			flag = false;
			break;
		case 133:
			gameObject.AddComponent<a_bridge_trap>();
			flag = false;
			break;
		case 34:
		case 35:
			gameObject.AddComponent<TMAP>();
			flag = false;
			flag3 = true;
			break;
		case 61:
		case 82:
			gameObject.AddComponent<Fountain>();
			break;
		case 80:
			gameObject.AddComponent<object_base>();
			flag4 = true;
			break;
		case 7:
			gameObject.AddComponent<Bridge>();
			flag = false;
			break;
		case 99:
			gameObject.AddComponent<a_spell>();
			flag = false;
			break;
		case 40:
			switch (objectInteraction.quality)
			{
			case 2:
				gameObject.AddComponent<a_do_trap_camera>();
				break;
			case 3:
				gameObject.AddComponent<a_do_trap_platform>();
				break;
			case 5:
				gameObject.AddComponent<a_hack_trap_trespass>();
				break;
			case 17:
				gameObject.AddComponent<a_hack_trap_floorcollapse>();
				break;
			case 18:
				gameObject.AddComponent<a_hack_trap_scintpuzzlereset>();
				break;
			case 19:
				gameObject.AddComponent<a_hack_trap_scintplatformreset>();
				break;
			case 21:
				gameObject.AddComponent<a_hack_trap_button_mover>();
				break;
			case 10:
				gameObject.AddComponent<a_hack_trap_class_item>();
				break;
			case 12:
				gameObject.AddComponent<a_hack_trap_platformwave>();
				break;
			case 14:
				gameObject.AddComponent<a_hack_trap_colour_cycle>();
				break;
			case 26:
				gameObject.AddComponent<a_hack_trap_forcefield>();
				break;
			case 30:
				gameObject.AddComponent<a_hack_trap_coward>();
				break;
			case 20:
				gameObject.AddComponent<a_hack_trap_terraform_puzzle>();
				break;
			case 24:
				if (UWEBase._RES != "UW2")
				{
					gameObject.AddComponent<a_do_trapBullfrog>();
				}
				else
				{
					gameObject.AddComponent<a_hack_trap_tmap_range_change>();
				}
				break;
			case 25:
				gameObject.AddComponent<a_hack_trap_skup>();
				break;
			case 27:
				gameObject.AddComponent<a_hack_trap_unlink>();
				break;
			case 23:
			case 28:
				gameObject.AddComponent<a_hack_trap_texture>();
				break;
			case 29:
				gameObject.AddComponent<a_hack_trap_button_flicker>();
				break;
			case 32:
				gameObject.AddComponent<a_hack_trap_qbert>();
				break;
			case 33:
				gameObject.AddComponent<a_hack_trap_recycle>();
				break;
			case 35:
				gameObject.AddComponent<a_hack_trap_light_recharge>();
				break;
			case 36:
				gameObject.AddComponent<a_hack_trap_castle_npcs>();
				break;
			case 38:
				gameObject.AddComponent<a_hack_trap_spoil_potion>();
				break;
			case 39:
				gameObject.AddComponent<a_hack_trap_visibility>();
				break;
			case 40:
				if (UWEBase._RES == "UW2")
				{
					gameObject.AddComponent<a_hack_trap_vendingselect>();
				}
				else
				{
					gameObject.AddComponent<a_do_trap_emeraldpuzzle>();
				}
				break;
			case 41:
				gameObject.AddComponent<a_hack_trap_vending>();
				break;
			case 42:
				if (UWEBase._RES == "UW2")
				{
					gameObject.AddComponent<a_hack_trap_vendingsign>();
				}
				else
				{
					gameObject.AddComponent<a_do_trap_conversation>();
				}
				break;
			case 43:
				gameObject.AddComponent<a_hack_trap_change_goal>();
				break;
			case 44:
				gameObject.AddComponent<a_hack_trap_sleep>();
				break;
			case 50:
				gameObject.AddComponent<a_do_trap_jailor>();
				break;
			case 63:
				gameObject.AddComponent<a_do_trap_EndGame>();
				break;
			case 54:
				gameObject.AddComponent<a_hack_trap_gemrotate>();
				break;
			case 55:
				gameObject.AddComponent<a_hack_trap_teleport>();
				break;
			default:
				gameObject.AddComponent<a_hack_trap>();
				break;
			}
			flag = false;
			break;
		default:
			gameObject.AddComponent<object_base>();
			break;
		}
		if (parent.transform == GameWorldController.instance.DynamicObjectMarker() && currObj.index < 256)
		{
			if (currObj.GetItemType() != 0)
			{
				flag2 = true;
			}
			SetMobileProps(gameObject, objectInteraction, currObj);
			if (gameObject.GetComponent<Rigidbody>() != null)
			{
				int itemType = currObj.GetItemType();
				if (itemType != 0 && itemType != 112 && itemType != 129)
				{
					UWEBase.UnFreezeMovement(gameObject);
					gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
					gameObject.GetComponent<Rigidbody>().AddForce(150f * object_base.ProjectilePropsToVector(gameObject.GetComponent<object_base>()));
				}
			}
		}
		if (flag || UWEBase.EditorMode)
		{
			objectInteraction.ObjectSprite = CreateObjectGraphics(gameObject, UWEBase._RES + "/Sprites/Objects/Objects_" + currObj.item_id, !flag3);
		}
		if (!flag2)
		{
			gameObject.transform.Rotate(0f, (float)currObj.heading * 45f, 0f);
		}
		if (flag4)
		{
			bool flag5 = true;
			if (UWEBase._RES == "UW2")
			{
				switch (currObj.item_id)
				{
				case 448:
				case 450:
				case 451:
				case 452:
				case 453:
				case 459:
				case 462:
					flag5 = false;
					break;
				}
			}
			else
			{
				switch (currObj.item_id)
				{
				case 450:
				case 451:
				case 452:
				case 453:
				case 459:
					flag5 = false;
					break;
				}
			}
			AnimationOverlay animationOverlay = gameObject.AddComponent<AnimationOverlay>();
			animationOverlay.StartFrame = currObj.StartFrameValue();
			animationOverlay.NoOfFrames = currObj.useSpriteValue();
			if (flag5)
			{
				animationOverlay.StartingDuration = 65535;
			}
			else
			{
				animationOverlay.StartingDuration = animationOverlay.NoOfFrames;
			}
		}
		return objectInteraction;
	}

	public bool isMagicallyEnchanted(ObjectLoaderInfo currObj, ObjectLoaderInfo[] objList)
	{
		if (isquant == 1)
		{
			return false;
		}
		if (enchantment == 1)
		{
			return true;
		}
		if (currObj.link > 0 && currObj.link <= objList.GetUpperBound(0) && objList[currObj.link].GetItemType() == 99 && objList[currObj.link].InUseFlag == 1)
		{
			return true;
		}
		return true;
	}

	public IdentificationFlags identity()
	{
		switch (heading)
		{
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
			return IdentificationFlags.Identified;
		case 1:
			return IdentificationFlags.PartiallyIdentified;
		default:
			return IdentificationFlags.Unidentified;
		}
	}

	public void setInvis(short val)
	{
		invis = val;
		if (ObjectSprite != null)
		{
			ObjectSprite.gameObject.SetActive(val == 0);
		}
	}

	public bool isMoveable()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].isMoveable == 1;
	}

	public static string UniqueObjectName(ObjectInteraction currObj)
	{
		int itemType = currObj.GetItemType();
		if (itemType == 29 || itemType == 30 || itemType == 4)
		{
			return "door_" + currObj.ObjectTileX.ToString("d3") + "_" + currObj.ObjectTileY.ToString("d3");
		}
		return currObj.getDesc() + Guid.NewGuid();
	}

	public void OnSaveObjectEvent()
	{
		GetComponent<object_base>().OnSaveObjectEvent();
	}

	public int WorldIndex()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].WorldIndex;
	}

	public int InventoryIndex()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].InventoryIndex;
	}

	public bool IsAnimated()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].startFrame == 1;
	}

	public bool UseSprite()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].useSprite == 0;
	}

	public string getDesc()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].desc;
	}
}
