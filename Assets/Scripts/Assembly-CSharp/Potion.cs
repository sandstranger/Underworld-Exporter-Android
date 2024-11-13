using UnityEngine;

public class Potion : enchantment_base
{
	public ObjectInteraction linked;

	public override bool Eat()
	{
		return use();
	}

	public override bool use()
	{
		if (ConversationVM.InConversation)
		{
			return false;
		}
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (linked != null)
			{
				int num = linked.item_id;
				if (num == 384)
				{
					linked.gameObject.GetComponent<trap_base>().Activate(this, 0, 0, 0);
					objInt().consumeObject();
					return true;
				}
				UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, linked.gameObject.GetComponent<a_spell>().link - 256, 1, 2);
				objInt().consumeObject();
				return true;
			}
			int num2 = -1;
			num2 = StringController.str_you_quaff_the_potion_in_one_gulp_;
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, num2));
			UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, GetActualSpellIndex(), 1, 2);
			objInt().consumeObject();
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	protected override int GetActualSpellIndex()
	{
		if (linked != null)
		{
			int num = linked.item_id;
			if (num == 384)
			{
				return 116;
			}
			return linked.link - 256;
		}
		return base.GetActualSpellIndex();
	}

	public override bool ApplyAttack(short damage)
	{
		base.quality -= damage;
		if (base.quality <= 0)
		{
			ChangeType(213);
			base.gameObject.AddComponent<enchantment_base>();
			Object.Destroy(this);
		}
		return true;
	}

	public override string UseVerb()
	{
		return "quaff";
	}

	public override void MoveToWorldEvent()
	{
		if (base.isquant != 0 || base.link >= 256 || base.link <= 0 || !(linked != null))
		{
			return;
		}
		bool flag = false;
		ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
		for (int i = 0; i <= objInfo.GetUpperBound(0); i++)
		{
			if (objInfo[i].GetItemType() == linked.GetItemType() && objInfo[i].instance != null && objInfo[i].link == linked.link && objInfo[i].owner == linked.owner && objInfo[i].quality == linked.quality)
			{
				Object.Destroy(linked.gameObject);
				linked = objInfo[i].instance;
				base.link = i;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			GameWorldController.MoveToWorld(linked.gameObject);
		}
	}

	public override void MoveToInventoryEvent()
	{
		if (linked != null)
		{
			GameObject gameObject = Object.Instantiate(linked.gameObject);
			gameObject.name = ObjectInteraction.UniqueObjectName(gameObject.GetComponent<ObjectInteraction>());
			gameObject.gameObject.transform.parent = GameWorldController.instance.InventoryMarker.transform;
			linked = gameObject.GetComponent<ObjectInteraction>();
		}
	}

	private void MagicFood()
	{
	}
}
