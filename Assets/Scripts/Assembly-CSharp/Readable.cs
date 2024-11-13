using UnityEngine;

public class Readable : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (UWEBase._RES == "UW1" && base.link == 769)
			{
				return MixRotwormStew();
			}
			return Read();
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool LookAt()
	{
		return Read();
	}

	public virtual bool Read()
	{
		int itemType = objInt().GetItemType();
		if (itemType == 11 || itemType == 13)
		{
			if (objInt().PickedUp)
			{
				if (UWEBase._RES == "UW1" && base.link == 520)
				{
					UWHUD.instance.CutScenesSmall.anim.SetAnimation = "cs410.n01";
					UWHUD.instance.CutScenesSmall.anim.looping = true;
				}
				else
				{
					UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(3, base.link - 512));
				}
				return true;
			}
			return base.LookAt();
		}
		UWHUD.instance.MessageScroll.Add("READABLE TYPE NOT FOUND! (" + base.item_id + ")");
		return false;
	}

	private bool MixRotwormStew()
	{
		bool flag = false;
		ObjectInteraction objectInteraction = null;
		bool flag2 = false;
		ObjectInteraction objectInteraction2 = null;
		bool flag3 = false;
		ObjectInteraction objectInteraction3 = null;
		bool flag4 = false;
		Container playerContainer = UWCharacter.Instance.playerInventory.playerContainer;
		if (playerContainer != null)
		{
			ObjectInteraction objectInteraction4 = playerContainer.findItemOfType(142);
			if (objectInteraction4 != null)
			{
				Container component = objectInteraction4.GetComponent<Container>();
				if (component != null)
				{
					short num = 0;
					while ((float)num <= component.GetCapacity())
					{
						ObjectInteraction itemAt = component.GetItemAt(num);
						if (itemAt != null)
						{
							switch (itemAt.item_id)
							{
							case 184:
								objectInteraction2 = itemAt;
								flag2 = true;
								break;
							case 190:
								objectInteraction = itemAt;
								flag = true;
								break;
							case 217:
								objectInteraction3 = itemAt;
								flag3 = true;
								break;
							default:
								flag4 = true;
								break;
							}
						}
						num++;
					}
					if (flag3 && flag2 && flag && !flag4)
					{
						UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 149));
						objectInteraction.consumeObject();
						objectInteraction3.consumeObject();
						objectInteraction2.consumeObject();
						ObjectInteraction component2 = objectInteraction4.GetComponent<ObjectInteraction>();
						component2.ChangeType(283);
						Object.Destroy(component);
						objectInteraction4.gameObject.AddComponent<Food>();
						component2.isquant = 1;
						component2.link = 1;
						return true;
					}
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 148));
					return true;
				}
			}
		}
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 150));
		return true;
	}

	public override string UseVerb()
	{
		return "read";
	}

	public override string ExamineVerb()
	{
		return "read";
	}

	public override float GetWeight()
	{
		return (float)GameWorldController.instance.commonObject.properties[base.item_id].mass * 0.1f;
	}
}
