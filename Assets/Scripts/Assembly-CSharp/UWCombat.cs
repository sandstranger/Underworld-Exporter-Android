using System.Collections;
using UnityEngine;

public class UWCombat : Combat
{
	public static UWCombat instance;

	public WeaponMelee currWeapon;

	public WeaponRanged currWeaponRanged;

	private ObjectInteraction currentAmmo;

	private string CurrentStrike;

	private short CurrentStrikeAnimation;

	private int DamageImpactStart
	{
		get
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				return 43;
			}
			return 46;
		}
	}

	private int DamageImpactEnd
	{
		get
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				return 47;
			}
			return 50;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public override void PlayerCombatIdle()
	{
		base.PlayerCombatIdle();
		if (AttackCharging || AttackExecuting)
		{
			return;
		}
		if (Character.InteractionMode == 4)
		{
			if (IsMelee())
			{
				UWHUD.instance.wpa.SetAnimation = GetWeaponOffset() + GetHandOffset() + 6;
			}
		}
		else
		{
			UWHUD.instance.wpa.SetAnimation = -1;
		}
	}

	public override void CombatBegin()
	{
		chargeRate = 33f + 66f * ((float)UWCharacter.Instance.PlayerSkills.Attack / 30f);
		if (IsMelee())
		{
			CurrentStrike = GetStrikeType();
			CurrentStrikeAnimation = GetStrikeOffset();
			UWHUD.instance.wpa.SetAnimation = GetWeaponOffset() + CurrentStrikeAnimation + GetHandOffset();
		}
		else
		{
			currentAmmo = UWCharacter.Instance.playerInventory.findObjInteractionByID(currWeaponRanged.AmmoType());
			if (currentAmmo == null && ObjectInteraction.Alias(currWeaponRanged.AmmoType()) != currWeaponRanged.AmmoType())
			{
				currentAmmo = UWCharacter.Instance.playerInventory.findObjInteractionByID(ObjectInteraction.Alias(currWeaponRanged.AmmoType()));
			}
			if (currentAmmo == null)
			{
				UWHUD.instance.MessageScroll.Add("Sorry, you have no " + StringController.instance.GetObjectNounUW(currWeaponRanged.AmmoType()));
				return;
			}
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconTarget;
		}
		AttackCharging = true;
		Charge = 0f;
	}

	public override void CombatCharging()
	{
		Charge += chargeRate * Time.deltaTime;
		if (Charge > 100f)
		{
			Charge = 100f;
		}
	}

	public override IEnumerator ExecuteMelee(string StrikeType, float StrikeCharge)
	{
		yield return new WaitForSeconds(0.4f);
		Ray ray = ((!UWCharacter.Instance.MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
		RaycastHit hit = default(RaycastHit);
		if (Physics.Raycast(ray, out hit, weaponRange))
		{
			if (!hit.transform.Equals(base.transform))
			{
				ObjectInteraction component = hit.transform.gameObject.GetComponent<ObjectInteraction>();
				if (component != null)
				{
					if (component.GetItemType() == 0)
					{
						PC_Hits_NPC(UWCharacter.Instance, currWeapon, CurrentStrike, StrikeCharge, component.GetComponent<NPC>(), hit);
					}
					else
					{
						Impact.SpawnHitImpact(Impact.ImpactDamage(), (ray.origin + hit.point) / 2f, DamageImpactStart, DamageImpactEnd);
						component.Attack((short)GetPlayerBaseDamage(currWeapon, CurrentStrike), UWCharacter.Instance.gameObject);
					}
				}
				else
				{
					Impact.SpawnHitImpact(Impact.ImpactDamage(), (ray.origin + hit.point) / 2f, DamageImpactStart, DamageImpactEnd);
					if (currWeapon != null)
					{
						short durability = currWeapon.getDurability();
						if (durability <= 30)
						{
							currWeapon.SelfDamage((short)Mathf.Max(0, Random.Range(1, durability + 1) - durability));
						}
					}
					if (ObjectInteraction.PlaySoundEffects)
					{
						UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[10];
						UWCharacter.Instance.aud.Play();
					}
				}
			}
		}
		else
		{
			if (ObjectInteraction.PlaySoundEffects)
			{
				UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[10];
				UWCharacter.Instance.aud.Play();
			}
			if (currWeapon != null)
			{
				currWeapon.onHit(null);
			}
		}
		AttackExecuting = false;
		UWHUD.instance.window.UWWindowWait(1f);
	}

	public override void ExecuteRanged(float charge)
	{
		base.ExecuteRanged(charge);
		UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
		if (currentAmmo != null)
		{
			LaunchAmmo(charge);
		}
	}

	public override void ReleaseAttack()
	{
		if (UWHUD.instance.window.JustClicked)
		{
			return;
		}
		if (IsMelee())
		{
			if (CurrentStrike == "")
			{
				CurrentStrike = GetStrikeType();
			}
			UWHUD.instance.wpa.SetAnimation = GetWeaponOffset() + CurrentStrikeAnimation + GetHandOffset() + 1;
			AttackExecuting = true;
			StartCoroutine(ExecuteMelee(CurrentStrike, Charge));
		}
		else
		{
			ExecuteRanged(Charge);
		}
		Charge = 0f;
		AttackCharging = false;
	}

	public bool IsMelee()
	{
		if (GetWeapon() == "Ranged")
		{
			return false;
		}
		return true;
	}

	public string GetWeapon()
	{
		if (currWeapon != null)
		{
			switch (currWeapon.GetSkill())
			{
			case 3:
				return "Sword";
			case 4:
				return "Axe";
			case 5:
				return "Mace";
			default:
				return "Fist";
			}
		}
		if (currWeaponRanged != null)
		{
			return "Ranged";
		}
		return "Fist";
	}

	public short GetWeaponOffset()
	{
		if (currWeapon != null)
		{
			switch (currWeapon.GetSkill())
			{
			case 3:
				return 0;
			case 4:
				return 7;
			case 5:
				return 14;
			default:
				return 21;
			}
		}
		if (currWeaponRanged != null)
		{
			return -1;
		}
		return 21;
	}

	public string GetRace()
	{
		switch (UWCharacter.Instance.Body)
		{
		case 0:
		case 2:
		case 3:
		case 4:
			return "White";
		default:
			return "Black";
		}
	}

	public string GetHand()
	{
		if (UWCharacter.Instance.isLefty)
		{
			return "Left";
		}
		return "Right";
	}

	public short GetHandOffset()
	{
		if (UWCharacter.Instance.isLefty)
		{
			return 28;
		}
		return 0;
	}

	public string GetStrikeType()
	{
		if (!UWCharacter.Instance.MouseLookEnabled)
		{
			if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.666f)
			{
				return "Bash";
			}
			if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.333f)
			{
				return "Slash";
			}
			return "Stab";
		}
		switch (Random.Range(1, 4))
		{
		case 1:
			return "Bash";
		case 2:
			return "Slash";
		default:
			return "Stab";
		}
	}

	public short GetStrikeOffset()
	{
		if (!UWCharacter.Instance.MouseLookEnabled)
		{
			if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.666f)
			{
				return 2;
			}
			if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.333f)
			{
				return 0;
			}
			return 4;
		}
		switch (Random.Range(1, 4))
		{
		case 1:
			return 2;
		case 2:
			return 4;
		default:
			return 0;
		}
	}

	private bool LaunchAmmo(float charge)
	{
		if (currentAmmo != null)
		{
			Ray ray = ((!UWCharacter.Instance.MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
			RaycastHit hitInfo = default(RaycastHit);
			float num = 0.5f;
			if (!Physics.Raycast(ray, out hitInfo, num))
			{
				float num2 = 1000f * (charge / 100f);
				GameObject gameObject;
				if (currentAmmo.GetQty() == 1)
				{
					gameObject = currentAmmo.gameObject;
					UWCharacter.Instance.playerInventory.RemoveItem(currentAmmo);
					GameWorldController.MoveToWorld(gameObject);
					gameObject.transform.position = ray.GetPoint(num - 0.1f);
				}
				else
				{
					ObjectLoaderInfo currObj = ObjectLoader.newObject(currWeaponRanged.AmmoType(), 40, 0, 1, 256);
					gameObject = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, ray.GetPoint(num - 0.1f)).gameObject;
					currentAmmo.consumeObject();
				}
				gameObject.GetComponent<ObjectInteraction>().isquant = 1;
				UWEBase.UnFreezeMovement(gameObject);
				Vector3 vector = ray.GetPoint(num) - ray.origin;
				gameObject.GetComponent<Rigidbody>().AddForce(vector * num2);
				GameObject gameObject2 = new GameObject(gameObject.name + "_damage");
				gameObject2.transform.position = gameObject.transform.position;
				gameObject2.transform.parent = gameObject.transform;
				ProjectileDamage projectileDamage = gameObject2.AddComponent<ProjectileDamage>();
				projectileDamage.Source = UWCharacter.Instance.gameObject;
				projectileDamage.Damage = (short)currWeaponRanged.Damage();
				projectileDamage.AttackCharge = charge;
				projectileDamage.AttackScore = UWCharacter.Instance.PlayerSkills.GetSkill(1) / 2 + UWCharacter.Instance.PlayerSkills.GetSkill(7);
				return true;
			}
			return false;
		}
		return false;
	}

	public static void PC_Hits_NPC(UWCharacter playerUW, WeaponMelee currentWeapon, string StrikeName, float StrikeCharge, NPC npc, RaycastHit hit)
	{
		int num = 0;
		int playerBaseDamage = GetPlayerBaseDamage(currentWeapon, StrikeName);
		if (currentWeapon != null)
		{
			num = playerUW.PlayerSkills.GetSkill(1) / 2 + playerUW.PlayerSkills.GetSkill(currentWeapon.GetSkill() + 1);
			num += currentWeapon.AccuracyBonus();
		}
		else
		{
			num = playerUW.PlayerSkills.GetSkill(1) / 2 + playerUW.PlayerSkills.GetSkill(3);
		}
		int num2 = Mathf.Max(npc.GetDefence() - num, 0);
		int num3 = Mathf.Min(30, Skills.DiceRoll(-1, 31) + 5 * (1 - GameWorldController.instance.difficulty));
		int num4 = 0;
		if ((num3 >= num2 || num3 >= 30) && num3 > -1)
		{
			short damage = (short)Mathf.Max((float)playerBaseDamage * (StrikeCharge / 100f), 1f);
			npc.ApplyAttack(damage, playerUW.gameObject);
			num4 = 1;
			if (num3 == 30)
			{
				num4 = 2;
				npc.ApplyAttack(damage, playerUW.gameObject);
			}
			if (num3 >= 27)
			{
				num4 = 2;
			}
		}
		else
		{
			num4 = 0;
			npc.ApplyAttack(0, playerUW.gameObject);
			if (currentWeapon != null && (float)npc.GetDefence() > (float)num * 1.5f)
			{
				short durability = currentWeapon.getDurability();
				if (durability <= 30)
				{
					currentWeapon.SelfDamage((short)Mathf.Max(0, Random.Range(0, npc.GetArmourDamage() + 1) - durability));
				}
			}
		}
		switch (num4)
		{
		case 0:
			if (ObjectInteraction.PlaySoundEffects)
			{
				npc.objInt().aud.clip = MusicController.instance.SoundEffects[28];
				npc.objInt().aud.Play();
			}
			break;
		case 1:
			Impact.SpawnHitImpact(Impact.ImpactBlood(), npc.GetImpactPoint(), npc.objInt().GetHitFrameStart(), npc.objInt().GetHitFrameEnd());
			if (ObjectInteraction.PlaySoundEffects)
			{
				npc.objInt().aud.clip = MusicController.instance.SoundEffects[GetHitSound()];
				npc.objInt().aud.Play();
			}
			break;
		case 2:
			Impact.SpawnHitImpact(Impact.ImpactBlood(), npc.GetImpactPoint(), npc.objInt().GetHitFrameStart(), npc.objInt().GetHitFrameEnd());
			Impact.SpawnHitImpact(Impact.ImpactBlood(), npc.GetImpactPoint() + Vector3.up * 0.1f, npc.objInt().GetHitFrameStart(), npc.objInt().GetHitFrameEnd());
			if (ObjectInteraction.PlaySoundEffects)
			{
				npc.objInt().aud.clip = MusicController.instance.SoundEffects[GetHitSound()];
				npc.objInt().aud.Play();
			}
			break;
		}
		if (currentWeapon != null)
		{
			currentWeapon.onHit(hit.transform.gameObject);
		}
	}

	public static void NPC_Hits_PC(UWCharacter playerUW, NPC npc)
	{
		int num = 0;
		num = ((!(playerUW.PlayerCombat.currWeapon != null)) ? (playerUW.PlayerSkills.GetSkill(2) + playerUW.PlayerSkills.GetSkill(3) / 2) : (playerUW.PlayerSkills.GetSkill(2) + playerUW.PlayerSkills.GetSkill(playerUW.PlayerCombat.currWeapon.GetSkill() + 1) / 2));
		int num2 = Mathf.Max(num - npc.GetAttack(), 0);
		int num3 = Random.Range(-1, 31);
		if (UWEBase._RES == "UW1" && npc.item_id == 124)
		{
			num3 = 30;
		}
		int damage = npc.GetDamage();
		if ((num3 < num2 && num3 < 30) || num3 <= -1)
		{
			return;
		}
		int armourScore = playerUW.playerInventory.getArmourScore();
		int num4 = Mathf.Max(1, damage - armourScore);
		playerUW.ApplyDamage(Random.Range(1, num4 + 1), npc.gameObject);
		if (damage > armourScore)
		{
			playerUW.playerInventory.ApplyArmourDamage((short)Random.Range(0, npc.GetArmourDamage() + 1));
		}
		if (npc.PoisonLevel() > 0 && !UWCharacter.Instance.isPoisonResistant())
		{
			int num5 = Random.Range(1, 30);
			if (num5 < npc.PoisonLevel())
			{
				int num6 = Random.Range(1, npc.PoisonLevel() + 1);
				int num7 = (short)Mathf.Min(playerUW.play_poison + num6, 15);
				UWCharacter.Instance.play_poison = (short)num7;
				if (UWCharacter.Instance.poison_timer == 0f)
				{
					UWCharacter.Instance.poison_timer = 30f;
				}
			}
		}
		MusicController.LastAttackCounter = 10f;
		if (ObjectInteraction.PlaySoundEffects)
		{
			UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[7];
			UWCharacter.Instance.aud.Play();
		}
	}

	public static void NPC_Hits_NPC(NPC targetNPC, NPC originNPC)
	{
		int defence = targetNPC.GetDefence();
		int attack = originNPC.GetAttack();
		int num = Mathf.Max(defence - attack, 1);
		int num2 = Random.Range(-1, 31);
		int num3 = Random.Range(1, originNPC.GetDamage() + 1);
		if ((num2 >= num || num2 >= 30) && num2 > -1)
		{
			targetNPC.ApplyAttack((short)num3, originNPC.gameObject);
			Impact.SpawnHitImpact(Impact.ImpactBlood(), targetNPC.GetImpactPoint(), targetNPC.objInt().GetHitFrameStart(), targetNPC.objInt().GetHitFrameEnd());
			if (ObjectInteraction.PlaySoundEffects)
			{
				originNPC.objInt().aud.clip = MusicController.instance.SoundEffects[7];
				originNPC.objInt().aud.Play();
			}
		}
	}

	public static int GetPlayerBaseDamage(WeaponMelee currentWeapon, string StrikeName)
	{
		int num = 1;
		if (currentWeapon != null)
		{
			switch (StrikeName)
			{
			case "SLASH":
				return currentWeapon.GetSlash();
			case "BASH":
				return currentWeapon.GetBash();
			default:
				return currentWeapon.GetStab();
			}
		}
		switch (StrikeName)
		{
		case "SLASH":
			return WeaponMelee.getMeleeSlash();
		case "BASH":
			return WeaponMelee.getMeleeBash();
		default:
			return WeaponMelee.getMeleeStab();
		}
	}

	private static int GetHitSound()
	{
		if (instance.currWeapon != null)
		{
			return Random.Range(7, 9);
		}
		return Random.Range(3, 5);
	}
}
