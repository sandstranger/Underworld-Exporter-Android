using UnityEngine;

public class object_base : UWEBase
{
	public bool testUse = false;

	protected ObjectInteraction _objInt;

	public static string ItemDesc;

	public static string UseableDesc;

	public static string PickableDesc;

	public static string UseObjectOnDesc;

	public static bool UseAvail;

	public static bool PickAvail;

	public static bool TalkAvail;

	public int item_id
	{
		get
		{
			return objInt().item_id;
		}
		set
		{
			objInt().item_id = value;
		}
	}

	public short xpos
	{
		get
		{
			return objInt().xpos;
		}
		set
		{
			objInt().xpos = value;
		}
	}

	public short ypos
	{
		get
		{
			return objInt().ypos;
		}
		set
		{
			objInt().ypos = value;
		}
	}

	public short zpos
	{
		get
		{
			return objInt().zpos;
		}
		set
		{
			objInt().zpos = value;
		}
	}

	public short heading
	{
		get
		{
			return objInt().heading;
		}
		set
		{
			objInt().heading = value;
		}
	}

	public short ObjectTileX
	{
		get
		{
			return objInt().ObjectTileX;
		}
		set
		{
			objInt().ObjectTileX = value;
		}
	}

	public short ObjectTileY
	{
		get
		{
			return objInt().ObjectTileY;
		}
		set
		{
			objInt().ObjectTileY = value;
		}
	}

	public short flags
	{
		get
		{
			return objInt().flags;
		}
		set
		{
			objInt().flags = value;
		}
	}

	public short enchantment
	{
		get
		{
			return objInt().enchantment;
		}
		set
		{
			objInt().enchantment = value;
		}
	}

	public short doordir
	{
		get
		{
			return objInt().doordir;
		}
		set
		{
			objInt().doordir = value;
		}
	}

	public short invis
	{
		get
		{
			return objInt().invis;
		}
		set
		{
			objInt().invis = value;
		}
	}

	public short isquant
	{
		get
		{
			return objInt().isquant;
		}
		set
		{
			objInt().isquant = value;
		}
	}

	public short quality
	{
		get
		{
			return objInt().quality;
		}
		set
		{
			objInt().quality = value;
		}
	}

	public int next
	{
		get
		{
			return objInt().next;
		}
		set
		{
			objInt().next = value;
		}
	}

	public short owner
	{
		get
		{
			return objInt().owner;
		}
		set
		{
			objInt().owner = value;
		}
	}

	public int link
	{
		get
		{
			return objInt().link;
		}
		set
		{
			objInt().link = value;
		}
	}

	public short npc_whoami
	{
		get
		{
			return objInt().npc_whoami;
		}
		set
		{
			objInt().npc_whoami = value;
		}
	}

	public short npc_voidanim
	{
		get
		{
			return objInt().npc_voidanim;
		}
		set
		{
			objInt().npc_voidanim = value;
		}
	}

	public short npc_xhome
	{
		get
		{
			return objInt().npc_xhome;
		}
		set
		{
			objInt().npc_xhome = value;
		}
	}

	public short npc_yhome
	{
		get
		{
			return objInt().npc_yhome;
		}
		set
		{
			objInt().npc_yhome = value;
		}
	}

	public short npc_hunger
	{
		get
		{
			return objInt().npc_hunger;
		}
		set
		{
			objInt().npc_hunger = value;
		}
	}

	public short npc_health
	{
		get
		{
			return objInt().npc_health;
		}
		set
		{
			objInt().npc_health = value;
		}
	}

	public short npc_hp
	{
		get
		{
			return objInt().npc_hp;
		}
		set
		{
			objInt().npc_hp = value;
		}
	}

	public short npc_arms
	{
		get
		{
			return objInt().npc_arms;
		}
		set
		{
			objInt().npc_arms = value;
		}
	}

	public short npc_power
	{
		get
		{
			return objInt().npc_power;
		}
		set
		{
			objInt().npc_power = value;
		}
	}

	public short npc_goal
	{
		get
		{
			return objInt().npc_goal;
		}
		set
		{
			objInt().npc_goal = value;
		}
	}

	public short npc_attitude
	{
		get
		{
			return objInt().npc_attitude;
		}
		set
		{
			objInt().npc_attitude = value;
		}
	}

	public short npc_gtarg
	{
		get
		{
			return objInt().npc_gtarg;
		}
		set
		{
			objInt().npc_gtarg = value;
		}
	}

	public short npc_heading
	{
		get
		{
			return objInt().npc_heading;
		}
		set
		{
			objInt().npc_heading = value;
		}
	}

	public short npc_talkedto
	{
		get
		{
			return objInt().npc_talkedto;
		}
		set
		{
			objInt().npc_talkedto = value;
		}
	}

	public short npc_level
	{
		get
		{
			return objInt().npc_level;
		}
		set
		{
			objInt().npc_level = value;
		}
	}

	public short npc_name
	{
		get
		{
			return objInt().npc_name;
		}
		set
		{
			objInt().npc_name = value;
		}
	}

	public short npc_height
	{
		get
		{
			return objInt().npc_height;
		}
		set
		{
			objInt().npc_height = value;
		}
	}

	public short ProjectileHeadingMajor
	{
		get
		{
			return objInt().ProjectileHeadingMajor;
		}
		set
		{
			objInt().ProjectileHeadingMajor = value;
		}
	}

	public short ProjectileHeadingMinor
	{
		get
		{
			return objInt().ProjectileHeadingMinor;
		}
		set
		{
			objInt().ProjectileHeadingMinor = value;
		}
	}

	public short Projectile_Speed
	{
		get
		{
			return objInt().Projectile_Speed;
		}
		set
		{
			objInt().Projectile_Speed = value;
		}
	}

	public short Projectile_Pitch
	{
		get
		{
			return objInt().Projectile_Pitch;
		}
		set
		{
			objInt().Projectile_Pitch = value;
		}
	}

	public short Projectile_Sign
	{
		get
		{
			return objInt().Projectile_Sign;
		}
		set
		{
			objInt().Projectile_Sign = value;
		}
	}

	public ObjectInteraction objInt()
	{
		if (_objInt == null)
		{
			_objInt = GetComponent<ObjectInteraction>();
		}
		return _objInt;
	}

	protected virtual void Start()
	{
	}

	public virtual bool Activate(GameObject src)
	{
		return false;
	}

	public virtual bool ApplyAttack(short damage)
	{
		return false;
	}

	public virtual bool ApplyAttack(short damage, GameObject source)
	{
		return ApplyAttack(damage);
	}

	public virtual bool LookAt()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()) + OwnershipString());
		if (link != 0 && !objInt().isQuant && enchantment == 0 && ObjectLoader.GetItemTypeAt(link) == 57)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(link);
			if (objectIntAt != null)
			{
				objectIntAt.GetComponent<object_base>().Activate(base.gameObject);
			}
		}
		return true;
	}

	public virtual bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (Character.InteractionMode == 5)
		{
			FailMessage();
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
			UWEBase.CurrentObjectInHand = null;
			return true;
		}
		return false;
	}

	public virtual bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (objInt().isUsable && objInt().PickedUp)
			{
				BecomeObjectInHand();
				return true;
			}
			if (objInt().isUsable && ((link != 0 && !objInt().isQuant) || (link == 1 && objInt().isQuant)) && enchantment == 0 && ObjectLoader.GetItemTypeAt(link) == 56)
			{
				ObjectLoader.getGameObjectAt(link).GetComponent<trigger_base>().Activate(base.gameObject);
			}
			return false;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public void BecomeObjectInHand()
	{
		UWEBase.CurrentObjectInHand = objInt();
		Character.InteractionMode = 5;
		InteractionModeControl.UpdateNow = true;
	}

	public virtual bool TalkTo()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_cannot_talk_to_that_));
		return true;
	}

	public virtual bool Eat()
	{
		return false;
	}

	public virtual bool PickupEvent()
	{
		if (link != 0 && !objInt().isQuant && enchantment == 0 && ObjectLoader.GetItemTypeAt(link) == 55)
		{
			ObjectLoader.getGameObjectAt(link).GetComponent<trigger_base>().Activate(base.gameObject);
			link = 0;
		}
		if (CanBeOwned() && ((uint)owner & 0x1Fu) != 0)
		{
			if (UWEBase._RES == "UW1" && owner == 13 && Quest.instance.QuestVariables[32] == 4)
			{
				owner = 0;
				return false;
			}
			SignalTheft(UWCharacter.Instance.transform.position, owner, 4f);
			owner = 0;
		}
		return false;
	}

	public virtual bool DropEvent()
	{
		return false;
	}

	public virtual bool PutItemAwayEvent(short slotNo)
	{
		return false;
	}

	public virtual bool EquipEvent(short slotNo)
	{
		return false;
	}

	public virtual bool UnEquipEvent(short slotNo)
	{
		return false;
	}

	public virtual bool FailMessage()
	{
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_cannot_use_that_));
		return false;
	}

	public virtual float GetWeight()
	{
		if (objInt() == null)
		{
			return 0f;
		}
		return (float)objInt().GetQty() * (float)GameWorldController.instance.commonObject.properties[item_id].mass * 0.1f;
	}

	public virtual void MergeEvent()
	{
	}

	public virtual int AliasItemId()
	{
		return item_id;
	}

	public virtual void Split()
	{
	}

	public virtual bool ChangeType(int newID)
	{
		objInt().ChangeType(newID);
		return true;
	}

	public int getIdentificationLevels(int EffectID)
	{
		return Random.Range(1, 31);
	}

	public virtual Sprite GetEquipDisplay()
	{
		return objInt().GetInventoryDisplay();
	}

	public string GetContextMenuText(int item_id, bool isUseable, bool isPickable, bool ObjectInHand)
	{
		if (invis == 1)
		{
			UseAvail = false;
			TalkAvail = false;
			PickAvail = false;
			return "";
		}
		ItemDesc = ContextMenuDesc(item_id);
		TalkAvail = false;
		if (isUseable)
		{
			UseableDesc = ContextMenuUsedDesc();
			UseAvail = true;
		}
		else
		{
			UseableDesc = "";
			UseAvail = false;
		}
		if (ObjectInHand)
		{
			UseObjectOnDesc = ContextMenuUseObjectOn_World();
		}
		else if (isPickable)
		{
			PickableDesc = ContextMenuUsedPickup();
			PickAvail = true;
		}
		else
		{
			PickableDesc = "";
			PickAvail = false;
		}
		if (Character.InteractionMode == 2 && UWEBase.CurrentObjectInHand != null)
		{
			UseAvail = false;
			UseableDesc = "";
			PickableDesc = "";
		}
		if (ObjectInHand)
		{
			if (UseAvail)
			{
				return ItemDesc + "\n" + UseObjectOnDesc;
			}
			return ItemDesc;
		}
		if (UseableDesc != "" || PickableDesc != "")
		{
			return ItemDesc + "\n" + UseableDesc + " " + PickableDesc;
		}
		return "";
	}

	public virtual string ContextMenuDesc(int item_id)
	{
		return StringController.instance.GetSimpleObjectNameUW(item_id);
	}

	public virtual string ContextMenuUsedDesc()
	{
		return "L-Click to " + UseVerb();
	}

	public virtual string ContextMenuUsedPickup()
	{
		return "R-Click to " + PickupVerb();
	}

	public virtual string ContextMenuUseObjectOn_World()
	{
		return "L-Click to " + UseObjectOnVerb_World();
	}

	public virtual string ContextMenuUseObjectOn_Inv()
	{
		return "R-Click to " + UseObjectOnVerb_Inv();
	}

	public virtual string UseVerb()
	{
		return "use";
	}

	public virtual string PickupVerb()
	{
		return "pickup";
	}

	public virtual string ExamineVerb()
	{
		return "examine";
	}

	public virtual string UseObjectOnVerb_World()
	{
		return "use object on";
	}

	public virtual string UseObjectOnVerb_Inv()
	{
		return "swap/combine";
	}

	public override Vector3 GetImpactPoint()
	{
		return base.gameObject.transform.position;
	}

	public virtual GameObject GetImpactGameObject()
	{
		return base.gameObject;
	}

	public bool setSpriteTMOBJ(SpriteRenderer sprt, int SpriteIndex)
	{
		if (sprt == null)
		{
			return false;
		}
		if (SpriteIndex != -1)
		{
			sprt.sprite = GameWorldController.instance.TmObjArt.RequestSprite(SpriteIndex);
			objInt().animationStarted = true;
			return true;
		}
		return false;
	}

	public bool setSpriteTMFLAT(SpriteRenderer sprt, int SpriteIndex)
	{
		if (sprt == null)
		{
			return false;
		}
		if (SpriteIndex != -1)
		{
			sprt.sprite = GameWorldController.instance.TmFlatArt.RequestSprite(SpriteIndex);
			objInt().animationStarted = true;
			return true;
		}
		return false;
	}

	public virtual void InventoryEventOnLevelEnter()
	{
	}

	public virtual void InventoryEventOnLevelExit()
	{
	}

	public virtual void OnSaveObjectEvent()
	{
	}

	public virtual bool CanBePickedUp()
	{
		return false;
	}

	public bool CanBeOwned()
	{
		return GameWorldController.instance.commonObject.properties[item_id].CanBelongTo == 1;
	}

	public virtual string OwnershipString()
	{
		if (CanBeOwned() && ((uint)owner & 0x1Fu) != 0)
		{
			return " belonging to" + StringController.instance.GetString(1, 370 + (owner & 0x1F));
		}
		return "";
	}

	public static void SignalTheft(Vector3 position, int Owner, float range)
	{
		Collider[] array = Physics.OverlapSphere(position, 4f);
		foreach (Collider collider in array)
		{
			if (collider.gameObject.GetComponent<NPC>() != null && collider.gameObject.GetComponent<NPC>().GetRace() == (Owner & 0x1F))
			{
				string @string = StringController.instance.GetString(1, 370 + (Owner & 0x1F));
				string text = "";
				collider.gameObject.GetComponent<NPC>().npc_attitude--;
				if (collider.gameObject.GetComponent<NPC>().npc_attitude <= 0)
				{
					collider.gameObject.GetComponent<NPC>().npc_attitude = 0;
					collider.gameObject.GetComponent<NPC>().npc_gtarg = 1;
					collider.gameObject.GetComponent<NPC>().gtarg = UWCharacter.Instance.gameObject;
					collider.gameObject.GetComponent<NPC>().gtargName = UWCharacter.Instance.gameObject.name;
					collider.gameObject.GetComponent<NPC>().npc_goal = 5;
					text = StringController.instance.GetString(1, StringController.str__is_angered_by_your_action_);
				}
				else
				{
					text = StringController.instance.GetString(1, StringController.str__is_annoyed_by_your_action_);
				}
				UWHUD.instance.MessageScroll.Add(@string.Trim() + text);
			}
		}
	}

	public virtual ObjectInteraction FindObjectInChain(int link, int ItemType)
	{
		if (link != 0)
		{
			ObjectInteraction instance = UWEBase.CurrentObjectList().objInfo[link].instance;
			if (instance != null)
			{
				if (instance.GetItemType() == 48)
				{
					if (ItemType == 48)
					{
						return instance;
					}
					return null;
				}
				if (instance.GetItemType() == ItemType)
				{
					return instance;
				}
				return FindObjectInChain(instance.link, ItemType);
			}
		}
		return null;
	}

	public virtual void MoveToWorldEvent()
	{
	}

	public virtual void MoveToInventoryEvent()
	{
	}

	public virtual void Update()
	{
		if (testUse)
		{
			testUse = false;
			use();
		}
	}

	public static Vector3 ProjectilePropsToVector(object_base obj)
	{
		Quaternion quaternion = Quaternion.AngleAxis(45f * (float)obj.ProjectileHeadingMinor / 32f, Vector3.up);
		float y = ((obj.Projectile_Sign != 0) ? (1f * ((float)obj.Projectile_Pitch / 7f)) : (-1f * ((float)obj.Projectile_Pitch / 7f)));
		Vector3 vector;
		switch (obj.ProjectileHeadingMajor)
		{
		case 1:
			vector = new Vector3(1f, y, 1f);
			break;
		case 2:
			vector = new Vector3(1f, y, 0f);
			break;
		case 3:
			vector = new Vector3(1f, y, -1f);
			break;
		case 4:
			vector = new Vector3(0f, y, -1f);
			break;
		case 5:
			vector = new Vector3(-1f, y, -1f);
			break;
		case 6:
			vector = new Vector3(-1f, y, 0f);
			break;
		case 7:
			vector = new Vector3(-1f, y, 1f);
			break;
		default:
			vector = new Vector3(0f, y, 1f);
			break;
		}
		return quaternion * vector;
	}

	public virtual void DestroyEvent()
	{
	}
}
