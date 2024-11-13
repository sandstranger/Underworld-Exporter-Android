using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UWCharacter : Character
{
	public const float baseJumpHeight = 0.2f;

	public const float extraJumpHeight = 0.8f;

	public const float extraJumpHeightLeap = 1.2f;

	public string[] LayersForRay = new string[4] { "Water", "MapMesh", "Lava", "Ice" };

	public Vector3 Rayposition;

	public Vector3 Raydirection = Vector3.down;

	public float Raydistance = 1f;

	private int mask;

	public const int CharClassFighter = 0;

	public const int CharClassMage = 1;

	public const int CharClassBard = 2;

	public const int CharClassTinker = 3;

	public const int CharClassDruid = 4;

	public const int CharClassPaladin = 5;

	public const int CharClassRanger = 6;

	public const int CharClassShepard = 7;

	public static UWCharacter Instance;

	[Header("Player Position Status")]
	public int CurrentTerrain;

	public TerrainDatLoader.TerrainTypes terrainType;

	public bool Grounded;

	public bool onIce;

	public bool onIcePrev;

	public bool onLava;

	public bool onBridge;

	public Vector3 IceCurrentVelocity = Vector3.zero;

	[Header("Player Movement Status")]
	public SpellEffect[] ActiveSpell = new SpellEffect[3];

	public SpellEffect[] PassiveSpell = new SpellEffect[10];

	private bool _isSwimming;

	public int StealthLevel;

	public int Resistance;

	public bool Paralyzed;

	public float ParalyzeTimer = 0f;

	public float currYVelocity;

	public float fallSpeed;

	public float braking;

	public float bounceMult = 1f;

	public Vector3 BounceMovement;

	public bool Fleeing;

	public bool WeaponDrawn;

	[Header("Player Magic Status")]
	public bool isFlying;

	public bool isLeaping;

	public bool isRoaming;

	public bool isFloating;

	public bool isSpeeding;

	public bool isWaterWalking;

	public bool isTelekinetic;

	public bool isTimeFrozen;

	public bool isLucky;

	public bool isBouncy;

	[Header("Player Health Status")]
	public int FoodLevel;

	public int Fatigue;

	[SerializeField]
	private short _play_poison;

	public float poison_timer = 30f;

	public float lavaDamageTimer;

	private bool InventoryReady = false;

	public bool Injured;

	public bool Death;

	[Header("Save game")]
	public int XorKey = 217;

	public bool decode = true;

	public bool recode = true;

	public bool recode_cheat = true;

	public int IndexToRecode = 0;

	public int ValueToRecode = 0;

	[Header("Character Details")]
	public int Body;

	public int CharClass;

	public int CharLevel;

	public int EXP;

	public int TrainingPoints;

	public bool isFemale;

	public bool isLefty;

	[Header("Speeds")]
	public float flySpeed;

	public float walkSpeed;

	public float speedMultiplier = 1f;

	public float swimSpeedMultiplier = 1f;

	public float SwimTimer = 0f;

	public float SwimDamageTimer;

	[Header("Character Modules")]
	public Skills PlayerSkills;

	public Magic PlayerMagic;

	public PlayerInventory playerInventory;

	public UWCombat PlayerCombat;

	[Header("Teleportation")]
	public short ResurrectLevel;

	public Vector3 ResurrectPosition = Vector3.zero;

	public Vector3 MoonGatePosition = Vector3.zero;

	public short MoonGateLevel = 2;

	public float teleportedTimer = 0f;

	public bool JustTeleported = false;

	public short DreamReturnTileX = 0;

	public short DreamReturnTileY = 0;

	public short DreamReturnLevel = 0;

	public float DreamWorldTimer = 30f;

	public Vector3 TeleportPosition;

	public Vector3 CameraLocalPos
	{
		get
		{
			if (isSwimming)
			{
				return new Vector3(0f, -0.8f + 0.05f * Mathf.Sin((float)Math.PI / 180f * (360f * (SwimTimer % 1f))), 0.38f);
			}
			return new Vector3(0f, 0.91f, 0.38f);
		}
	}

	public bool isSwimming
	{
		get
		{
			return _isSwimming;
		}
		set
		{
			if (!_isSwimming && value)
			{
				PlaySplashSound();
			}
			_isSwimming = value;
		}
	}

	public short play_poison
	{
		get
		{
			return _play_poison;
		}
		set
		{
			if (_play_poison == 0 && value != 0)
			{
				UWHUD.instance.FlaskHealth.UpdatePoisonDisplay(true);
			}
			else if (_play_poison != 0 && value == 0)
			{
				UWHUD.instance.FlaskHealth.UpdatePoisonDisplay(false);
			}
			_play_poison = value;
			UWHUD.instance.FlaskHealth.UpdateFlaskDisplay();
		}
	}

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		XAxis.enabled = false;
		YAxis.enabled = false;
		MouseLookEnabled = false;
		mask = LayerMask.GetMask(LayersForRay);
		StartCoroutine(playfootsteps());
	}

	public override void Begin()
	{
		base.Begin();
		if (!(UWEBase._RES == "SHOCK"))
		{
			InventoryReady = false;
			XAxis.enabled = false;
			YAxis.enabled = false;
			MouseLookEnabled = false;
			Cursor.SetCursor(UWHUD.instance.CursorIconBlank, Vector2.zero, CursorMode.ForceSoftware);
			Character.InteractionMode = Character.DefaultInteractionMode;
			UWHUD.instance.InputControl.text = "";
			UWHUD.instance.MessageScroll.Clear();
			switch (Instance.Body)
			{
			case 0:
			case 2:
			case 3:
			case 4:
				GameWorldController.instance.weapongr = new WeaponsLoader(0);
				break;
			default:
				GameWorldController.instance.weapongr = new WeaponsLoader(1);
				break;
			}
		}
	}

	private void PlayerDeath()
	{
		Death = true;
		Character.InteractionMode = 5;
		UWHUD.instance.wpa.SetAnimation = -1;
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			DeathHandlingUW2();
		}
		else
		{
			DeathHandlingUW1();
		}
		if (PlayerMagic.ReadiedSpell != "")
		{
			PlayerMagic.ReadiedSpell = "";
			UWHUD.instance.CursorIcon = UWHUD.instance.CursorIconDefault;
		}
	}

	private void DeathHandlingUW1()
	{
		if (UWHUD.instance.CutScenesSmall != null)
		{
			if (ResurrectLevel != 0 && (!(UWEBase._RES == "UW1") || GameWorldController.instance.LevelNo != 8))
			{
				UWHUD.instance.CutScenesSmall.anim.SetAnimation = "cs402.n01";
			}
			else
			{
				UWHUD.instance.CutScenesSmall.anim.SetAnimation = "cs403.n01";
			}
		}
	}

	private void DeathHandlingUW2()
	{
		switch (GameWorldController.instance.LevelNo)
		{
		case 0:
			if (WasIKilledByAFriend())
			{
				Quest.instance.QuestVariables[112] = 1;
			}
			if (Quest.instance.QuestVariables[112] == 1)
			{
				UWHUD.instance.CutScenesSmall.anim.SetAnimation = "uw2resurrecttransition";
			}
			else
			{
				UWHUD.instance.CutScenesSmall.anim.SetAnimation = "cs403.n01";
			}
			break;
		case 1:
		case 2:
		case 3:
		case 4:
			UWHUD.instance.CutScenesSmall.anim.SetAnimation = "cs403.n01";
			break;
		default:
			UWHUD.instance.CutScenesSmall.anim.SetAnimation = "uw2resurrecttransition";
			break;
		}
	}

	private bool WasIKilledByAFriend()
	{
		if (LastEnemyToHitMe != null && LastEnemyToHitMe.GetComponent<NPC>() != null)
		{
			switch (LastEnemyToHitMe.GetComponent<NPC>().npc_whoami)
			{
			case 129:
			case 130:
			case 131:
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 141:
			case 142:
			case 143:
			case 149:
			case 168:
				return true;
			case 140:
				return false;
			}
		}
		return false;
	}

	public static void ResurrectPlayerUW1()
	{
		ResurrectCommon();
		if (GameWorldController.instance.LevelNo != Instance.ResurrectLevel - 1)
		{
			if (UWEBase._RES == "UW1")
			{
				ResetTrueMana();
			}
			GameWorldController.instance.SwitchLevel((short)(Instance.ResurrectLevel - 1));
		}
		Instance.gameObject.transform.position = Instance.ResurrectPosition;
	}

	private static void ResurrectCommon()
	{
		Instance.Death = false;
		Instance.Fleeing = false;
		if (MusicController.instance != null)
		{
			MusicController.instance.Combat = false;
			MusicController.LastAttackCounter = 0f;
		}
		Instance.playerCam.cullingMask = -33;
		Instance.isSwimming = false;
		Instance.play_poison = 0;
		Instance.CurVIT = UnityEngine.Random.Range(Instance.MaxVIT / 2, Instance.MaxVIT);
	}

	public static void ResurrectPlayerUW2()
	{
		ResurrectCommon();
		if (GameWorldController.instance.LevelNo == 0)
		{
			float x = 51.000004f;
			float z = 46.2f;
			float num = (float)UWEBase.CurrentTileMap().GetFloorHeight(42, 38) * 0.15f;
			Instance.transform.position = new Vector3(x, num + 0.3f, z);
			a_hack_trap_castle_npcs.MakeEveryoneFriendly();
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(NPC.findNpcByWhoAmI(142));
			if (objectIntAt != null)
			{
				NPC component = objectIntAt.GetComponent<NPC>();
				if (component != null)
				{
					component.Agent.Warp(UWEBase.CurrentTileMap().getTileVector(42, 35));
				}
			}
			else
			{
				Debug.Log("Lord British is missing. This should not happen.");
			}
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(9, 4));
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 361));
			GameWorldController.instance.SwitchLevel(4, 30, 39);
		}
	}

	public override float GetUseRange()
	{
		if (UWEBase.EditorMode)
		{
			return 100f;
		}
		if (isTelekinetic)
		{
			return useRange * 8f;
		}
		if (playerInventory.ObjectInHand == null)
		{
			return useRange;
		}
		if (playerInventory.ObjectInHand != null)
		{
			int itemType = playerInventory.ObjectInHand.GetItemType();
			if (itemType == 86)
			{
				return useRange * 2f;
			}
		}
		return useRange;
	}

	public override float GetPickupRange()
	{
		if (isTelekinetic)
		{
			return pickupRange * 8f;
		}
		return pickupRange;
	}

	private void FlyingMode()
	{
		playerMotor.movement.maxFallSpeed = 0f;
		playerMotor.movement.maxForwardSpeed = flySpeed * speedMultiplier;
		playerMotor.movement.maxSidewaysSpeed = playerMotor.movement.maxForwardSpeed * 2f / 3f;
		playerMotor.movement.maxBackwardsSpeed = playerMotor.movement.maxForwardSpeed / 3f;
		if ((Input.GetKeyDown(KeyBindings.instance.FlyUp) || Input.GetKey(KeyBindings.instance.FlyUp)) && !WindowDetect.WaitingForInput)
		{
			GetComponent<CharacterController>().Move(new Vector3(0f, 0.2f * Time.deltaTime * speedMultiplier, 0f));
		}
		else if ((Input.GetKeyDown(KeyBindings.instance.FlyDown) || Input.GetKey(KeyBindings.instance.FlyDown)) && !WindowDetect.WaitingForInput)
		{
			GetComponent<CharacterController>().Move(new Vector3(0f, -0.2f * Time.deltaTime * speedMultiplier, 0f));
		}
	}

	private void SwimmingEffects()
	{
		if (Character.InteractionMode == 4)
		{
			Character.InteractionMode = 6;
		}
		playerCam.transform.localPosition = CameraLocalPos + CameraShake.CurrentShake;
		swimSpeedMultiplier = Mathf.Max((float)PlayerSkills.Swimming / 30f, 0.3f);
		SwimTimer += Time.deltaTime;
		if (SwimTimer >= 5f + (float)PlayerSkills.Swimming * 15f)
		{
			SwimDamageTimer += Time.deltaTime;
			if (SwimDamageTimer >= 10f)
			{
				ApplyDamage(1);
				SwimDamageTimer = 0f;
			}
		}
		else
		{
			SwimDamageTimer = 0f;
		}
		if (ObjectInteraction.PlaySoundEffects && !footsteps.isPlaying)
		{
			footsteps.clip = MusicController.instance.SoundEffects[24];
			footsteps.Play();
		}
	}

	private void PlaySplashSound()
	{
		if (ObjectInteraction.PlaySoundEffects && !footsteps.isPlaying)
		{
			footsteps.clip = MusicController.instance.SoundEffects[25];
			footsteps.Play();
		}
	}

	public override void Update()
	{
		if (UWEBase._RES == "SHOCK" || UWEBase._RES == "TNOVA")
		{
			if (isFlying)
			{
				flySpeed = 10f;
				FlyingMode();
			}
			return;
		}
		Grounded = IsGrounded();
		TerrainAndCurrentsUpdate();
		base.Update();
		FallDamageUpdate();
		if (UWEBase._RES == "UW2")
		{
			BounceUpdate();
		}
		if (UWEBase.EditorMode)
		{
			base.CurVIT = base.MaxVIT;
		}
		TeleportUpdate();
		InventoryUpdate();
		if (WindowDetect.WaitingForInput && !Instrument.PlayingInstrument)
		{
			UWHUD.instance.InputControl.Select();
		}
		if (base.CurVIT <= 0 && !Death)
		{
			PlayerDeath();
		}
		else
		{
			if (Death)
			{
				return;
			}
			if (UWEBase._RES == "UW2")
			{
				ParalyzeUpdate();
			}
			if (playerCam.enabled)
			{
				SwimUpdate();
			}
			if (play_poison > 0)
			{
				PoisonUpdate();
			}
			playerMotor.enabled = !Paralyzed && !GameWorldController.instance.AtMainMenu && !ConversationVM.InConversation;
			if (UWEBase._RES == "UW2")
			{
				DreamWorldUpdate();
			}
			if (isFlying && !Grounded)
			{
				FlyingMode();
			}
			else if (isFloating)
			{
				playerMotor.movement.maxFallSpeed = 0.1f;
			}
			else
			{
				playerMotor.movement.maxFallSpeed = 20f;
				playerMotor.movement.maxForwardSpeed = walkSpeed * speedMultiplier * swimSpeedMultiplier;
				playerMotor.movement.maxSidewaysSpeed = playerMotor.movement.maxForwardSpeed * 2f / 3f;
				playerMotor.movement.maxBackwardsSpeed = playerMotor.movement.maxForwardSpeed / 3f;
			}
			if (isLeaping)
			{
				playerMotor.jumping.baseHeight = 0.2f;
				playerMotor.jumping.extraHeight = 1.2f;
			}
			else
			{
				playerMotor.jumping.baseHeight = 0.2f;
				playerMotor.jumping.extraHeight = 0.8f;
			}
			if (isRoaming)
			{
				playerMotor.movement.maxFallSpeed = 0f;
			}
			if (!isSpeeding && !onIce)
			{
				speedMultiplier = 1f;
			}
			WeaponDrawn = Character.InteractionMode == 4;
			if (PlayerMagic.ReadiedSpell != "")
			{
				SpellMode();
				return;
			}
			if (!UWHUD.instance.window.JustClicked && !Paralyzed)
			{
				PlayerCombat.PlayerCombatIdle();
			}
			if (onLava)
			{
				OnLavaUpdate();
			}
			else
			{
				lavaDamageTimer = 0f;
			}
			if (LightActive)
			{
				DetectionRange = 6f;
			}
			else
			{
				DetectionRange = 0.2f + 5.8f * ((30f - (float)(GetBaseStealthLevel() + StealthLevel)) / 30f);
			}
		}
	}

	private void TerrainAndCurrentsUpdate()
	{
		switch (terrainType)
		{
		case TerrainDatLoader.TerrainTypes.Lava:
		case TerrainDatLoader.TerrainTypes.Lavafall:
			onLava = true;
			onIce = false;
			isSwimming = false;
			break;
		case TerrainDatLoader.TerrainTypes.Ice_wall:
		case TerrainDatLoader.TerrainTypes.Ice_walls:
			if (!onIcePrev)
			{
				IceCurrentVelocity = playerMotor.movement.velocity.normalized * 3f;
			}
			onIce = true;
			onLava = false;
			isSwimming = false;
			break;
		case TerrainDatLoader.TerrainTypes.WaterFlowEast:
			IceCurrentVelocity = new Vector3(0.5f, 0f, 0f);
			isSwimming = true;
			onIce = false;
			onLava = false;
			break;
		case TerrainDatLoader.TerrainTypes.WaterFlowWest:
			IceCurrentVelocity = new Vector3(-0.5f, 0f, 0f);
			isSwimming = true;
			onIce = false;
			onLava = false;
			break;
		case TerrainDatLoader.TerrainTypes.WaterFlowNorth:
			IceCurrentVelocity = new Vector3(0f, 0f, 0.5f);
			isSwimming = true;
			onIce = false;
			onLava = false;
			break;
		case TerrainDatLoader.TerrainTypes.WaterFlowSouth:
			IceCurrentVelocity = new Vector3(0f, 0f, -0.5f);
			isSwimming = true;
			onIce = false;
			onLava = false;
			break;
		case TerrainDatLoader.TerrainTypes.Water:
		case TerrainDatLoader.TerrainTypes.Waterfall:
			isSwimming = true;
			IceCurrentVelocity = Vector3.zero;
			onIce = false;
			onLava = false;
			break;
		default:
			if (IceCurrentVelocity != Vector3.zero)
			{
				braking += Time.deltaTime;
				IceCurrentVelocity = Vector3.Lerp(IceCurrentVelocity, Vector3.zero, braking);
			}
			else
			{
				braking = 0f;
			}
			isSwimming = false;
			onIce = false;
			onLava = false;
			break;
		}
		if (isWaterWalking || !Grounded || onBridge)
		{
			isSwimming = false;
			onIce = false;
			IceCurrentVelocity = Vector3.zero;
		}
		if (!Grounded || onBridge)
		{
			onLava = false;
		}
		if (IceCurrentVelocity != Vector3.zero)
		{
			if (onIce && !aud.isPlaying)
			{
				aud.clip = MusicController.instance.SoundEffects[29];
				aud.Play();
			}
			GetComponent<CharacterController>().Move(new Vector3(IceCurrentVelocity.x * Time.deltaTime * speedMultiplier, IceCurrentVelocity.y * Time.deltaTime, IceCurrentVelocity.z * Time.deltaTime * speedMultiplier));
		}
		onIcePrev = onIce;
	}

	private void OnLavaUpdate()
	{
		if (!isFireProof())
		{
			lavaDamageTimer += Time.deltaTime;
			if (lavaDamageTimer >= 1f)
			{
				ApplyDamage(10);
				lavaDamageTimer = 0f;
			}
		}
		if (UWEBase._RES == "UW2" && Quest.instance.x_clocks[3] == 3)
		{
			Quest.instance.x_clocks[3] = 4;
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 334));
		}
	}

	private void DreamWorldUpdate()
	{
		if (Quest.instance.InDreamWorld)
		{
			isFlying = true;
			DreamWorldTimer -= Time.deltaTime;
			if (DreamWorldTimer < 0f)
			{
				DreamTravelFromVoid();
			}
		}
	}

	private void PoisonUpdate()
	{
		poison_timer -= Time.deltaTime;
		if (poison_timer <= 0f)
		{
			poison_timer = 30f;
			base.CurVIT -= 3;
			play_poison--;
		}
	}

	private void SwimUpdate()
	{
		if (isSwimming)
		{
			playerMotor.jumping.enabled = false;
			SwimmingEffects();
			return;
		}
		playerMotor.jumping.enabled = !Paralyzed && !GameWorldController.instance.AtMainMenu && !ConversationVM.InConversation && !WindowDetect.InMap;
		playerCam.transform.localPosition = CameraLocalPos + CameraShake.CurrentShake;
		swimSpeedMultiplier = 1f;
		SwimTimer = 0f;
	}

	private void ParalyzeUpdate()
	{
		if (ParalyzeTimer > 0f)
		{
			ParalyzeTimer -= Time.deltaTime;
		}
		if (ParalyzeTimer < 0f)
		{
			ParalyzeTimer = 0f;
		}
		Paralyzed = ParalyzeTimer != 0f;
	}

	private void InventoryUpdate()
	{
		if (PlayerInventory.Ready && !InventoryReady && playerInventory != null && playerInventory.GetCurrentContainer() != null)
		{
			playerInventory.Refresh();
			InventoryReady = true;
		}
	}

	private void TeleportUpdate()
	{
		if (JustTeleported)
		{
			teleportedTimer += Time.deltaTime;
			if (teleportedTimer >= 0.1f)
			{
				JustTeleported = false;
			}
			else
			{
				base.transform.position = new Vector3(TeleportPosition.x, base.transform.position.y, TeleportPosition.z);
			}
		}
	}

	private void BounceUpdate()
	{
		if (isBouncy)
		{
			bounceMult = 2f;
		}
		else
		{
			bounceMult = 1f;
		}
		if (BounceMovement.magnitude > 0f)
		{
			playerController.Move(BounceMovement * Time.deltaTime);
			BounceMovement.y -= 20f * Time.deltaTime;
			if (BounceMovement.y < 0f)
			{
				BounceMovement = Vector3.zero;
			}
		}
	}

	public void SpellMode()
	{
		if (Input.GetMouseButtonDown(1) && (WindowDetect.CursorInMainWindow || MouseLookEnabled) && !UWHUD.instance.window.JustClicked && !PlayerCombat.AttackCharging && !PlayerCombat.AttackExecuting)
		{
			PlayerMagic.castSpell(base.gameObject, PlayerMagic.ReadiedSpell, false);
			PlayerMagic.SpellCost = 0;
			UWHUD.instance.window.UWWindowWait(1f);
		}
	}

	public void OnSubmitPickup(int quant)
	{
		InputField inputControl = UWHUD.instance.InputControl;
		Time.timeScale = 1f;
		inputControl.gameObject.SetActive(false);
		WindowDetect.WaitingForInput = false;
		inputControl.text = "";
		inputControl.text = "";
		UWHUD.instance.MessageScroll.Clear();
		if (quant == 0)
		{
			QuantityObj = null;
		}
		if (QuantityObj != null)
		{
			if (quant >= QuantityObj.link)
			{
				Pickup(QuantityObj, playerInventory);
				return;
			}
			ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(QuantityObj.item_id, QuantityObj.quality, QuantityObj.owner, quant, 256);
			objectLoaderInfo.is_quant = QuantityObj.isquant;
			objectLoaderInfo.flags = QuantityObj.flags;
			objectLoaderInfo.enchantment = QuantityObj.enchantment;
			objectLoaderInfo.doordir = QuantityObj.doordir;
			objectLoaderInfo.invis = QuantityObj.invis;
			ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, QuantityObj.transform.position);
			objectLoaderInfo.InUseFlag = 1;
			QuantityObj.link -= quant;
			Pickup(objectInteraction, playerInventory);
			ObjectInteraction.Split(objectInteraction, QuantityObj);
			QuantityObj = null;
		}
	}

	public void TalkMode()
	{
		Ray ray = ((!MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, talkRange))
		{
			if (hitInfo.transform.gameObject.GetComponent<ObjectInteraction>() != null)
			{
				hitInfo.transform.gameObject.GetComponent<ObjectInteraction>().TalkTo();
			}
		}
		else
		{
			UWHUD.instance.MessageScroll.Add("Talking to yourself?");
		}
	}

	public override void LookMode()
	{
		Ray ray = ((!MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
		RaycastHit hitInfo = default(RaycastHit);
		if (!Physics.Raycast(ray, out hitInfo, lookRange))
		{
			return;
		}
		ObjectInteraction component = hitInfo.transform.GetComponent<ObjectInteraction>();
		if (component != null)
		{
			if (UWEBase.EditorMode)
			{
				IngameEditor.instance.ObjectSelect.value = component.objectloaderinfo.index;
			}
			component.LookDescription();
			return;
		}
		int num = hitInfo.transform.name.Length;
		if (num > 4)
		{
			num = 4;
		}
		switch (hitInfo.transform.name.Substring(0, num).ToUpper())
		{
		case "CEIL":
			UWHUD.instance.MessageScroll.Add("You see the ceiling");
			return;
		case "PILL":
			UWHUD.instance.MessageScroll.Add("You see a pillar");
			return;
		case "BRID":
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_see_a_bridge_));
			return;
		}
		if (hitInfo.transform.GetComponent<PortcullisInteraction>() != null)
		{
			ObjectInteraction parentObjectInteraction = hitInfo.transform.GetComponent<PortcullisInteraction>().getParentObjectInteraction();
			if (parentObjectInteraction != null)
			{
				parentObjectInteraction.LookDescription();
			}
			return;
		}
		Renderer component2 = hitInfo.collider.GetComponent<Renderer>();
		if (component2 == null)
		{
			return;
		}
		MeshCollider meshCollider = (MeshCollider)hitInfo.collider;
		int num2 = -1;
		Mesh sharedMesh = meshCollider.sharedMesh;
		int triangleIndex = hitInfo.triangleIndex;
		int num3 = sharedMesh.triangles[triangleIndex * 3];
		int num4 = sharedMesh.triangles[triangleIndex * 3 + 1];
		int num5 = sharedMesh.triangles[triangleIndex * 3 + 2];
		int subMeshCount = sharedMesh.subMeshCount;
		for (int i = 0; i < subMeshCount; i++)
		{
			int[] triangles = sharedMesh.GetTriangles(i);
			for (int j = 0; j < triangles.Length; j += 3)
			{
				if (triangles[j] == num3 && triangles[j + 1] == num4 && triangles[j + 2] == num5)
				{
					num2 = i;
					break;
				}
			}
			if (num2 != -1)
			{
				break;
			}
		}
		if (num2 == -1 || component2.materials[num2].name.Length < 7)
		{
			return;
		}
		int result = 0;
		if (int.TryParse(component2.materials[num2].name.Substring(4, 3), out result))
		{
			if (result == 142 && UWEBase._RES != "UW2")
			{
				UWHUD.instance.CutScenesSmall.anim.SetAnimation = "VolcanoWindow_" + GameWorldController.instance.LevelNo;
			}
			UWHUD.instance.MessageScroll.Add("You see " + StringController.instance.GetTextureName(result));
		}
	}

	public override void PickupMode(int ptrId)
	{
		PlayerInventory component = GetComponent<PlayerInventory>();
		if (!(component.ObjectInHand == null))
		{
			return;
		}
		Ray ray = ((!MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
		RaycastHit hitInfo = default(RaycastHit);
		if (!Physics.Raycast(ray, out hitInfo, GetPickupRange()))
		{
			return;
		}
		ObjectInteraction component2 = hitInfo.transform.GetComponent<ObjectInteraction>();
		if (!(component2 != null))
		{
			return;
		}
		if (component2.CanBePickedUp)
		{
			if (component2.GetWeight() > playerInventory.getEncumberance())
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_that_is_too_heavy_for_you_to_pick_up_));
			}
			else if (ptrId == -2)
			{
				if (!component2.isQuant || (component2.isQuant && component2.link == 1) || component2.isEnchanted)
				{
					component2 = Pickup(component2, component);
					return;
				}
				UWHUD.instance.MessageScroll.Set("Move how many?");
				InputField inputControl = UWHUD.instance.InputControl;
				inputControl.gameObject.SetActive(true);
				inputControl.gameObject.GetComponent<InputHandler>().target = base.gameObject;
				inputControl.gameObject.GetComponent<InputHandler>().currentInputMode = 0;
				inputControl.text = component2.GetQty().ToString();
				inputControl.Select();
				QuantityObj = component2;
				Time.timeScale = 0f;
				WindowDetect.WaitingForInput = true;
			}
			else
			{
				component2 = Pickup(component2, component);
			}
		}
		else if (component2.isUsable)
		{
			UseMode();
			UWHUD.instance.window.UWWindowWait(1f);
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_cannot_pick_that_up_));
		}
	}

	public Quest quest()
	{
		return GetComponent<Quest>();
	}

	public void onLanding(float fallSpeed)
	{
		if (!isSwimming)
		{
			float num = fallSpeed - (float)PlayerSkills.GetSkill(18) * 0.13f;
			if (num >= 5f)
			{
				ApplyDamage(UnityEngine.Random.Range(1, 5));
			}
			if (ObjectInteraction.PlaySoundEffects)
			{
				aud.clip = MusicController.instance.SoundEffects[0];
				aud.Play();
			}
		}
	}

	public void UpdateHungerAndFatigue()
	{
		Fatigue--;
		if (Fatigue < 0)
		{
			Fatigue = 0;
		}
		FoodLevel--;
		if (FoodLevel < 0)
		{
			FoodLevel = 0;
		}
		if (FoodLevel < 3)
		{
			ApplyDamage(1);
		}
	}

	public string GetFedStatus()
	{
		int num = 0;
		num = ((FoodLevel >= 30) ? ((FoodLevel < 60) ? 1 : ((FoodLevel < 90) ? 2 : ((FoodLevel < 120) ? 3 : ((FoodLevel < 150) ? 4 : ((FoodLevel < 180) ? 5 : ((FoodLevel < 210) ? 6 : ((FoodLevel >= 240) ? 8 : 7))))))) : 0);
		return StringController.instance.GetString(1, StringController.str_starving + num);
	}

	public string GetFatiqueStatus()
	{
		return StringController.instance.GetString(1, StringController.str_fatigued + Fatigue / 5);
	}

	public void RegenMana()
	{
		PlayerMagic.CurMana += UnityEngine.Random.Range(1, 6);
		if (PlayerMagic.CurMana > PlayerMagic.MaxMana)
		{
			PlayerMagic.CurMana = PlayerMagic.MaxMana;
		}
	}

	public void SetCharLevel(int level)
	{
		if (Instance.CharLevel < level)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_have_attained_experience_level_));
			TrainingPoints += 3;
			Instance.MaxVIT = Instance.PlayerSkills.STR * 3;
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW1")
			{
				if (GameWorldController.instance.LevelNo == 6 && !Quest.instance.isOrbDestroyed)
				{
					Instance.PlayerMagic.TrueMaxMana = Instance.PlayerSkills.ManaSkill * 3;
				}
				else
				{
					Instance.PlayerMagic.MaxMana = Instance.PlayerSkills.ManaSkill * 3;
					Instance.PlayerMagic.CurMana = Instance.PlayerMagic.MaxMana;
					Instance.PlayerMagic.TrueMaxMana = Instance.PlayerMagic.MaxMana;
				}
			}
			else
			{
				Instance.PlayerMagic.MaxMana = Instance.PlayerSkills.ManaSkill * 3;
				Instance.PlayerMagic.CurMana = Instance.PlayerMagic.MaxMana;
				Instance.PlayerMagic.TrueMaxMana = Instance.PlayerMagic.MaxMana;
			}
		}
		Instance.CharLevel = level;
	}

	public void AddXP(int xp)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			int num = EXP % 150;
			int num2 = (EXP + xp) % 150;
			EXP += xp;
			if (EXP <= 50)
			{
				SetCharLevel(1);
			}
			else if (EXP <= 100)
			{
				SetCharLevel(2);
			}
			else if (EXP <= 150)
			{
				SetCharLevel(3);
			}
			else if (EXP <= 200)
			{
				SetCharLevel(4);
			}
			else if (EXP <= 300)
			{
				SetCharLevel(5);
			}
			else if (EXP <= 400)
			{
				SetCharLevel(6);
			}
			else if (EXP <= 600)
			{
				SetCharLevel(7);
			}
			else if (EXP <= 800)
			{
				SetCharLevel(8);
			}
			else if (EXP <= 1200)
			{
				SetCharLevel(9);
			}
			else if (EXP <= 1600)
			{
				SetCharLevel(10);
			}
			else if (EXP <= 2400)
			{
				SetCharLevel(11);
			}
			else if (EXP <= 3200)
			{
				SetCharLevel(12);
			}
			else if (EXP <= 4800)
			{
				SetCharLevel(13);
			}
			else if (EXP <= 6400)
			{
				SetCharLevel(14);
			}
			else if (EXP < 65535)
			{
				SetCharLevel(15);
				if (num2 > num)
				{
					TrainingPoints++;
				}
			}
			else
			{
				EXP = 65535;
			}
		}
		else
		{
			EXP += xp;
			if (EXP <= 600)
			{
				SetCharLevel(1);
				return;
			}
			if (EXP <= 1200)
			{
				SetCharLevel(2);
				return;
			}
			if (EXP <= 1800)
			{
				SetCharLevel(3);
				return;
			}
			if (EXP <= 2400)
			{
				SetCharLevel(4);
				return;
			}
			if (EXP <= 3000)
			{
				SetCharLevel(5);
				return;
			}
			if (EXP <= 3600)
			{
				SetCharLevel(6);
				return;
			}
			if (EXP <= 4200)
			{
				SetCharLevel(7);
				return;
			}
			if (EXP <= 4800)
			{
				SetCharLevel(8);
				return;
			}
			if (EXP <= 5400)
			{
				SetCharLevel(9);
				return;
			}
			if (EXP <= 6000)
			{
				SetCharLevel(10);
				return;
			}
			if (EXP <= 6600)
			{
				SetCharLevel(11);
				return;
			}
			if (EXP <= 7200)
			{
				SetCharLevel(12);
				return;
			}
			if (EXP <= 7800)
			{
				SetCharLevel(13);
				return;
			}
			if (EXP <= 8400)
			{
				SetCharLevel(14);
				return;
			}
			if (EXP <= 9000)
			{
				SetCharLevel(15);
				return;
			}
			if (EXP <= 9600)
			{
				SetCharLevel(16);
				return;
			}
			EXP = 9600;
			SetCharLevel(16);
		}
	}

	public int GetBaseStealthLevel()
	{
		return PlayerSkills.GetSkill(14);
	}

	public void Sleep()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			SleepUW2();
		}
		else
		{
			SleepUW1();
		}
	}

	public void SleepUW2()
	{
		if (!CheckForMonsters())
		{
			if (Quest.instance.DreamPlantEaten)
			{
				DreamTravelToVoid();
			}
			else if (Instance.FoodLevel >= 3)
			{
				if (IsGaramonTime())
				{
					UWHUD.instance.MessageScroll.Add("You dream of the guardian");
				}
				else
				{
					StartCoroutine(SleepDelay());
				}
			}
			SleepRegen();
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 14));
		}
	}

	public void SleepUW1()
	{
		if (!CheckForMonsters())
		{
			ObjectInteraction objectInteraction = Instance.playerInventory.findObjInteractionByID(277);
			if (objectInteraction != null)
			{
				IncenseDream(objectInteraction);
			}
			else if (Instance.FoodLevel >= 3)
			{
				if (IsGaramonTime())
				{
					PlayGaramonDream(Quest.instance.GaramonDream++);
				}
				else
				{
					StartCoroutine(SleepDelay());
				}
			}
			SleepRegen();
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 14));
		}
	}

	private void IncenseDream(ObjectInteraction incense)
	{
		UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel.gameObject, true);
		incense.consumeObject();
		Cutscene_Incense cs = UWHUD.instance.gameObject.AddComponent<Cutscene_Incense>();
		UWHUD.instance.CutScenesFull.cs = cs;
		UWHUD.instance.CutScenesFull.Begin();
	}

	private void DreamTravelToVoid()
	{
		Quest.instance.DreamPlantEaten = false;
		DreamReturnTileX = TileMap.visitTileX;
		DreamReturnTileY = TileMap.visitTileY;
		DreamReturnLevel = GameWorldController.instance.LevelNo;
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 24));
		GameWorldController.instance.SwitchLevel(68, 32, 27);
		Quest.instance.InDreamWorld = true;
		DreamWorldTimer = 30f;
		Quest.instance.QuestVariables[48] = 1;
	}

	private void DreamTravelFromVoid()
	{
		Quest.instance.InDreamWorld = false;
		isFlying = false;
		GameWorldController.instance.SwitchLevel(DreamReturnLevel, DreamReturnTileX, DreamReturnTileY);
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 25));
	}

	private void SleepRegen()
	{
		for (int i = Instance.Fatigue; i < 29; i += 3)
		{
			if (Instance.FoodLevel >= 3)
			{
				GameClock.Advance();
				continue;
			}
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 17));
			UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel, false);
			Instance.Fatigue += i;
			return;
		}
		Instance.Fatigue = 29;
		if (Instance.CurVIT < Instance.MaxVIT)
		{
			Instance.CurVIT += UnityEngine.Random.Range(1, Instance.MaxVIT - Instance.CurVIT + 1);
		}
		if (Instance.PlayerMagic.CurMana < Instance.PlayerMagic.MaxMana)
		{
			Instance.PlayerMagic.CurMana += UnityEngine.Random.Range(1, Instance.PlayerMagic.MaxMana - Instance.PlayerMagic.CurMana + 1);
		}
	}

	private bool CheckForMonsters()
	{
		return false;
	}

	private bool IsGaramonTime()
	{
		if (Quest.instance.GaramonDream == 6)
		{
			return true;
		}
		if (Quest.instance.GaramonDream == 7)
		{
			return true;
		}
		if (GameClock.day() >= Quest.instance.DayGaramonDream)
		{
			return true;
		}
		return false;
	}

	private void PlayGaramonDream(int dreamIndex)
	{
		int num = 0;
		switch (dreamIndex)
		{
		case 0:
		{
			Cutscene_Dream_1 cs10 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_1>();
			UWHUD.instance.CutScenesFull.cs = cs10;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 1:
		{
			Cutscene_Dream_2 cs9 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_2>();
			UWHUD.instance.CutScenesFull.cs = cs9;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 2:
		{
			Cutscene_Dream_3 cs8 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_3>();
			UWHUD.instance.CutScenesFull.cs = cs8;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 3:
		{
			Cutscene_Dream_4 cs7 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_4>();
			UWHUD.instance.CutScenesFull.cs = cs7;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 4:
		{
			Cutscene_Dream_5 cs6 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_5>();
			UWHUD.instance.CutScenesFull.cs = cs6;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 5:
		{
			Cutscene_Dream_6 cs5 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_6>();
			UWHUD.instance.CutScenesFull.cs = cs5;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 6:
		{
			Cutscene_Dream_7 cs4 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_7>();
			UWHUD.instance.CutScenesFull.cs = cs4;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 7:
		{
			Cutscene_Dream_7 cs3 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_7>();
			UWHUD.instance.CutScenesFull.cs = cs3;
			UWHUD.instance.CutScenesFull.Begin();
			Quest.instance.GaramonDream = 3;
			num = 1;
			break;
		}
		case 8:
		{
			Cutscene_Dream_9 cs2 = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_9>();
			UWHUD.instance.CutScenesFull.cs = cs2;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		case 9:
		{
			Cutscene_Dream_10 cs = UWHUD.instance.gameObject.AddComponent<Cutscene_Dream_10>();
			UWHUD.instance.CutScenesFull.cs = cs;
			UWHUD.instance.CutScenesFull.Begin();
			num = 1;
			break;
		}
		}
		Quest.instance.DayGaramonDream = GameClock.day() + num;
	}

	private static void RestoreHealthMana(UWCharacter sunshine)
	{
		sunshine.CurVIT += UnityEngine.Random.Range(1, 40);
		if (sunshine.CurVIT > sunshine.MaxVIT)
		{
			sunshine.CurVIT = sunshine.MaxVIT;
		}
		sunshine.PlayerMagic.CurMana += UnityEngine.Random.Range(1, 40);
		if (sunshine.PlayerMagic.CurMana > sunshine.PlayerMagic.MaxMana)
		{
			sunshine.PlayerMagic.CurMana = sunshine.PlayerMagic.MaxMana;
		}
	}

	public static void WakeUp(UWCharacter sunshine)
	{
		RestoreHealthMana(sunshine);
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 18));
	}

	private IEnumerator SleepDelay()
	{
		UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel.gameObject, true);
		UWHUD.instance.CutScenesFull.SetAnimationFile = "FadeToBlackSleep";
		yield return new WaitForSeconds(3f);
		UWHUD.instance.CutScenesFull.SetAnimationFile = "Anim_Base";
		UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel.gameObject, false);
	}

	public static void ResetTrueMana()
	{
		if (Instance.PlayerMagic.MaxMana < Instance.PlayerMagic.TrueMaxMana)
		{
			Instance.PlayerMagic.MaxMana = Instance.PlayerMagic.TrueMaxMana;
			if (Instance.PlayerMagic.CurMana == 0)
			{
				Instance.PlayerMagic.CurMana = Instance.PlayerMagic.MaxMana / 4;
			}
		}
	}

	public bool isPoisonResistant()
	{
		return base.gameObject.GetComponent<SpellEffectImmunityPoison>() != null;
	}

	public bool isFireProof()
	{
		return base.gameObject.GetComponent<SpellEffectFlameproof>() != null;
	}

	public bool isMagicResistant()
	{
		return base.gameObject.GetComponent<SpellEffectMagicResistant>() != null;
	}

	public int getSpellResistance(SpellProp sp)
	{
		int num = 1;
		switch (sp.damagetype)
		{
		case SpellProp.DamageTypes.fire:
			if (isFireProof())
			{
				num++;
			}
			if (isMagicResistant())
			{
				num++;
			}
			break;
		case SpellProp.DamageTypes.poison:
			if (isPoisonResistant())
			{
				num++;
			}
			if (isMagicResistant())
			{
				num++;
			}
			break;
		case SpellProp.DamageTypes.physcial:
			if (isMagicResistant())
			{
				num++;
			}
			if (Instance.Resistance > 0)
			{
				num++;
			}
			break;
		default:
			if (isMagicResistant())
			{
				num++;
			}
			break;
		}
		return num;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (onIce)
		{
			if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("Water") || hit.collider.gameObject.layer == LayerMask.NameToLayer("MapMesh") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Lava") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Ice")) && hit.normal.y < 1f)
			{
				Vector3 vector = Vector3.Reflect(Instance.IceCurrentVelocity, hit.normal);
				IceCurrentVelocity = new Vector3(vector.x, 0f, vector.z);
			}
		}
		else if (isBouncy)
		{
			if (hit.normal.y == 1f && BounceMovement == Vector3.zero && hit.controller.velocity.y < -0.3f)
			{
				BounceMovement = new Vector3(hit.controller.velocity.x, 0f - hit.controller.velocity.y, hit.controller.velocity.z) * bounceMult;
			}
			if (hit.collider.name == "Tile_00_00")
			{
				BounceMovement = Vector3.zero;
			}
		}
	}

	private bool IsGrounded()
	{
		onBridge = false;
		Rayposition = base.transform.position;
		RaycastHit hitInfo;
		Physics.Raycast(Rayposition, Raydirection, out hitInfo, Raydistance, mask);
		if (hitInfo.collider != null)
		{
			if (hitInfo.collider.name.Contains("bridge"))
			{
				onBridge = true;
			}
			return true;
		}
		return false;
	}

	private void FallDamageUpdate()
	{
		if (!Grounded)
		{
			if (playerMotor.movement.velocity.y < currYVelocity)
			{
				fallSpeed = Mathf.Max(0f - Instance.playerMotor.movement.velocity.y, fallSpeed);
			}
			else
			{
				fallSpeed = 0f;
			}
		}
		else if (fallSpeed > 0f)
		{
			onLanding(fallSpeed);
			fallSpeed = 0f;
		}
		currYVelocity = playerMotor.movement.velocity.y;
	}

	private IEnumerator playfootsteps()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.4f);
			if (Grounded && playerMotor.movement.velocity.magnitude != 0f && !footsteps.isPlaying)
			{
				if (step)
				{
					footsteps.clip = MusicController.instance.SoundEffects[1];
					step = false;
				}
				else
				{
					footsteps.clip = MusicController.instance.SoundEffects[2];
					step = true;
				}
				footsteps.Play();
			}
		}
	}
}
