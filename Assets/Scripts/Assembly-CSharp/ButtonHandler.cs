using UnityEngine;

public class ButtonHandler : Decal
{
	public bool isOn;

	public int itemdIDOn;

	public int itemdIDOff;

	public int[] RotaryImageIDs = new int[8];

	public bool SpriteSet;

	private int currentItemID;

	protected override void Start()
	{
		BoxCollider component = GetComponent<BoxCollider>();
		component.size = new Vector3(0.3f, 0.3f, 0.1f);
		component.center = new Vector3(0f, 0.16f, 0f);
		base.flags = base.flags;
		if (!isRotarySwitch())
		{
			if (base.item_id >= 368 && base.item_id <= 375)
			{
				itemdIDOff = base.item_id - 368;
				itemdIDOn = base.item_id - 368 + 8;
				isOn = false;
			}
			else
			{
				itemdIDOff = base.item_id - 368 - 8;
				itemdIDOn = base.item_id - 368;
				isOn = true;
			}
			if (isOn)
			{
				setSprite(itemdIDOn);
			}
			else
			{
				setSprite(itemdIDOff);
			}
		}
		else
		{
			int num = ((base.item_id != 353) ? 12 : 4);
			for (int i = 0; i < 8; i++)
			{
				RotaryImageIDs[i] = num + i;
			}
			setRotarySprite(base.flags);
		}
	}

	public override void Update()
	{
		if (!isRotarySwitch())
		{
			if (isOn && currentItemID != itemdIDOn)
			{
				setSprite(itemdIDOn);
			}
			if (!isOn && currentItemID != itemdIDOff)
			{
				setSprite(itemdIDOff);
			}
		}
		else if (currentItemID != base.flags)
		{
			setRotarySprite(base.flags);
		}
	}

	private bool isRotarySwitch()
	{
		int num = base.item_id;
		if (num == 353 || num == 354)
		{
			return true;
		}
		return false;
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (base.invis == 0)
			{
				return Activate(base.gameObject);
			}
			return false;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool LookAt()
	{
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (gameObjectAt != null)
		{
			ObjectInteraction component = gameObjectAt.GetComponent<ObjectInteraction>();
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
			if (component.GetItemType() == 57)
			{
				base.LookAt();
				Activate(base.gameObject);
			}
			else
			{
				base.LookAt();
			}
		}
		else
		{
			base.LookAt();
		}
		return true;
	}

	public override bool Activate(GameObject src)
	{
		if (base.link != 0)
		{
			if (ObjectLoader.getGameObjectAt(base.link) == null)
			{
				return false;
			}
			GameObject gameObject = ObjectLoader.getObjectIntAt(base.link).gameObject;
			if (gameObject == null)
			{
				return true;
			}
			if (gameObject.GetComponent<trigger_base>() == null)
			{
				return false;
			}
			if (gameObject.GetComponent<a_use_trigger>() != null)
			{
				gameObject.GetComponent<a_use_trigger>().Activate(base.gameObject, isOn);
			}
			else
			{
				gameObject.GetComponent<trigger_base>().Activate(base.gameObject);
			}
		}
		if (isRotarySwitch())
		{
			if (base.flags == 7)
			{
				base.flags = 0;
			}
			else
			{
				base.flags++;
			}
		}
		if (!isRotarySwitch())
		{
			if (!isOn)
			{
				isOn = true;
				setSprite(itemdIDOn);
				base.item_id += 8;
			}
			else
			{
				isOn = false;
				setSprite(itemdIDOff);
				base.item_id -= 8;
			}
		}
		else
		{
			setRotarySprite(base.flags);
		}
		return true;
	}

	public void setSprite(int SpriteID)
	{
		if (base.invis == 0)
		{
			setSpriteTMFLAT(GetComponentInChildren<SpriteRenderer>(), SpriteID);
			currentItemID = SpriteID;
		}
	}

	public void setRotarySprite(int spriteId)
	{
		if (base.invis == 0)
		{
			setSpriteTMOBJ(GetComponentInChildren<SpriteRenderer>(), RotaryImageIDs[spriteId]);
			currentItemID = spriteId;
		}
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed != null)
		{
			int itemType = ObjectUsed.GetItemType();
			if (itemType == 86)
			{
				UWEBase.CurrentObjectInHand = null;
				UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, StringController.str_using_the_pole_you_trigger_the_switch_));
				return Activate(base.gameObject);
			}
			UWEBase.CurrentObjectInHand = null;
			ObjectUsed.FailMessage();
			return false;
		}
		return false;
	}

	public override string UseObjectOnVerb_World()
	{
		if (UWEBase.CurrentObjectInHand != null)
		{
			int itemType = UWEBase.CurrentObjectInHand.GetItemType();
			if (itemType == 86)
			{
				return "trigger with pole";
			}
		}
		return base.UseObjectOnVerb_Inv();
	}
}
