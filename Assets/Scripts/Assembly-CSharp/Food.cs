using UnityEngine;

public class Food : object_base
{
	protected override void Start()
	{
		base.Start();
		if (base.isquant == 0 && base.link <= 1 && base.item_id >= 176 && base.item_id <= 192)
		{
			base.isquant = 1;
			base.link = 1;
		}
	}

	public int Nutrition()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			return 16;
		}
		switch (base.item_id)
		{
		case 176:
			return 16;
		case 177:
			return 16;
		case 178:
			return 5;
		case 179:
			return 6;
		case 180:
			return 5;
		case 181:
			return 16;
		case 182:
			return 16;
		case 183:
			return 5;
		case 184:
			return 5;
		case 185:
			return 5;
		default:
			return 16;
		}
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null || UWEBase.CurrentObjectInHand == objInt())
		{
			int num = base.item_id;
			if (num == 191)
			{
				if (UWEBase._RES == "UW1")
				{
					UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 127));
					return true;
				}
				return Eat();
			}
			return Eat();
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public bool Drink()
	{
		return true;
	}

	public override bool Eat()
	{
		if (Nutrition() + UWCharacter.Instance.FoodLevel >= 255)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_are_too_full_to_eat_that_now_));
			return false;
		}
		UWCharacter.Instance.FoodLevel = Nutrition() + UWCharacter.Instance.FoodLevel;
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			TasteUW2();
		}
		else
		{
			TasteUW1();
		}
		if (ObjectInteraction.PlaySoundEffects)
		{
			int itemType = objInt().GetItemType();
			if (itemType == 132)
			{
				DrinkSoundEffects();
			}
			else
			{
				EatSoundEffects();
			}
		}
		if (UWEBase._RES == "UW2")
		{
			LeftOvers();
		}
		objInt().consumeObject();
		return true;
	}

	private static void EatSoundEffects()
	{
		switch (Random.Range(1, 3))
		{
		case 1:
			UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[31];
			break;
		case 2:
			UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[33];
			break;
		default:
			UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[37];
			break;
		}
		UWCharacter.Instance.aud.Play();
	}

	private static void DrinkSoundEffects()
	{
		Debug.Log("Glug glug");
	}

	private void TasteUW1()
	{
		switch (base.item_id)
		{
		case 184:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_mushroom_causes_your_head_to_spin_and_your_vision_to_blur_));
			UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, 213, 1, 2);
			break;
		case 185:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_toadstool_tastes_odd_and_you_begin_to_feel_ill_));
			UWCharacter.Instance.play_poison += 2;
			break;
		case 186:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_drink_the_dark_ale_));
			break;
		case 192:
		case 207:
		case 212:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_plant_is_plain_tasting_but_nourishing_));
			break;
		case 217:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 234));
			break;
		case 283:
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 235));
			break;
		default:
			UWHUD.instance.MessageScroll.Add("That " + StringController.instance.GetObjectNounUW(objInt()) + foodFlavourText());
			break;
		}
	}

	private void TasteUW2()
	{
		int num = base.item_id;
		if (num == 185)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_the_mushroom_causes_your_head_to_spin_and_your_vision_to_blur_));
			UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, 212, 1, 2);
		}
		else
		{
			UWHUD.instance.MessageScroll.Add("That " + StringController.instance.GetObjectNounUW(objInt()) + foodFlavourText());
		}
	}

	private void LeftOvers()
	{
		int num = -1;
		switch (base.item_id)
		{
		case 176:
		case 177:
			num = 197;
			break;
		case 186:
			num = 210;
			break;
		case 187:
		case 188:
		case 189:
			num = 317;
			break;
		}
		if (num != -1)
		{
			ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(num, 40, 0, 0, 256);
			objectLoaderInfo.InUseFlag = 1;
			ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, GameWorldController.instance.InventoryMarker.transform.position);
			GameWorldController.MoveToWorld(objectInteraction.gameObject);
			UWEBase.CurrentObjectInHand = objectInteraction;
			Character.InteractionMode = 2;
		}
	}

	public override bool LookAt()
	{
		if (objInt().GetItemType() == 132)
		{
			return base.LookAt();
		}
		switch (UWEBase._RES)
		{
		case "UW2":
		{
			int num = base.item_id;
			if (num == 192)
			{
				return base.LookAt();
			}
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), foodSmellText()) + OwnershipString());
			break;
		}
		default:
			switch (base.item_id)
			{
			case 191:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_see_) + " " + StringController.instance.GetString(1, 264));
				break;
			case 192:
			case 207:
			case 212:
			case 217:
				return base.LookAt();
			default:
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt(), foodSmellText()) + OwnershipString());
				break;
			}
			break;
		}
		return true;
	}

	private string foodFlavourText()
	{
		int str__tasted_putrid_ = StringController.str__tasted_putrid_;
		if (base.quality == 0)
		{
			return StringController.instance.GetString(1, str__tasted_putrid_);
		}
		if (base.quality >= 1 && base.quality < 15)
		{
			return StringController.instance.GetString(1, str__tasted_putrid_ + 1);
		}
		if (base.quality >= 15 && base.quality < 32)
		{
			return StringController.instance.GetString(1, str__tasted_putrid_ + 2);
		}
		if (base.quality >= 32 && base.quality < 40)
		{
			return StringController.instance.GetString(1, str__tasted_putrid_ + 3);
		}
		if (base.quality >= 40 && base.quality < 48)
		{
			return StringController.instance.GetString(1, str__tasted_putrid_ + 4);
		}
		return StringController.instance.GetString(1, str__tasted_putrid_ + 5);
	}

	private string foodSmellText()
	{
		if (base.quality == 0)
		{
			return StringController.instance.GetString(5, 18);
		}
		if (base.quality >= 1 && base.quality < 15)
		{
			return StringController.instance.GetString(5, 19);
		}
		if (base.quality >= 15 && base.quality < 32)
		{
			return StringController.instance.GetString(5, 20);
		}
		if (base.quality >= 32 && base.quality < 40)
		{
			return StringController.instance.GetString(5, 21);
		}
		if (base.quality >= 40 && base.quality < 48)
		{
			return StringController.instance.GetString(5, 22);
		}
		return StringController.instance.GetString(5, 23);
	}

	public override bool ApplyAttack(short damage)
	{
		base.quality -= damage;
		if (base.quality <= 0)
		{
			ChangeType(213);
			base.gameObject.AddComponent<object_base>();
			objInt().objectloaderinfo.InUseFlag = 0;
			Object.Destroy(this);
		}
		return true;
	}

	public override string UseVerb()
	{
		int itemType = objInt().GetItemType();
		if (itemType == 132)
		{
			return "drink";
		}
		return "eat";
	}
}
