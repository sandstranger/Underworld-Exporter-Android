using UnityEngine;
using UnityEngine.AI;

public class NPC : MobileObject
{
	public enum NPCCategory
	{
		ethereal = 0,
		humanoid = 1,
		flying = 2,
		swimming = 3,
		creeping = 4,
		crawling = 5,
		golem = 6,
		human = 81
	}

	public enum npc_goals
	{
		npc_goal_stand_still_0 = 0,
		npc_goal_stand_still_7 = 7,
		npc_goal_stand_still_11 = 11,
		npc_goal_stand_still_12 = 12,
		npc_goal_want_to_talk = 10,
		npc_goal_goto_1 = 1,
		npc_goal_wander_2 = 2,
		npc_goal_wander_4 = 4,
		npc_goal_wander_8 = 8,
		npc_goal_attack_5 = 5,
		npc_goal_attack_9 = 9,
		npc_goal_flee = 6,
		npc_goal_follow = 3,
		npc_goal_petrified = 15
	}

	public enum AttackStages
	{
		AttackPosition = 0,
		AttackAnimateRanged = 1,
		AttackAnimateMelee = 2,
		AttackExecute = 3,
		AttackWaitCycle = 4
	}

	private enum AgentMasks
	{
		LAND = 3,
		WATER = 4,
		LAVA = 5,
		AIR = 6
	}

	public string debugname;

	public CharacterController CharController;

	public const int AI_ATTITUDE_HOSTILE = 0;

	public const int AI_ATTITUDE_UPSET = 1;

	public const int AI_ATTITUDE_MELLOW = 2;

	public const int AI_ATTITUDE_FRIENDLY = 3;

	public const int AI_RANGE_IDLE = 1;

	public const int AI_RANGE_MOVE = 10;

	public const int AI_ANIM_IDLE_FRONT = 1;

	public const int AI_ANIM_IDLE_FRONT_RIGHT = 2;

	public const int AI_ANIM_IDLE_RIGHT = 3;

	public const int AI_ANIM_IDLE_REAR_RIGHT = 4;

	public const int AI_ANIM_IDLE_REAR = 5;

	public const int AI_ANIM_IDLE_REAR_LEFT = 6;

	public const int AI_ANIM_IDLE_LEFT = 7;

	public const int AI_ANIM_IDLE_FRONT_LEFT = 8;

	public const int AI_ANIM_WALKING_FRONT = 10;

	public const int AI_ANIM_WALKING_FRONT_RIGHT = 20;

	public const int AI_ANIM_WALKING_RIGHT = 30;

	public const int AI_ANIM_WALKING_REAR_RIGHT = 40;

	public const int AI_ANIM_WALKING_REAR = 50;

	public const int AI_ANIM_WALKING_REAR_LEFT = 60;

	public const int AI_ANIM_WALKING_LEFT = 70;

	public const int AI_ANIM_WALKING_FRONT_LEFT = 80;

	public const int AI_ANIM_DEATH = 100;

	public const int AI_ANIM_ATTACK_BASH = 1000;

	public const int AI_ANIM_ATTACK_SLASH = 2000;

	public const int AI_ANIM_ATTACK_THRUST = 3000;

	public const int AI_ANIM_COMBAT_IDLE = 4000;

	public const int AI_ANIM_ATTACK_SECONDARY = 5000;

	private static short[] CompassHeadings = new short[9] { 0, -1, -2, -3, 4, 3, 2, 1, 0 };

	[Header("AI Target")]
	public GameObject gtarg;

	public string gtargName;

	[Header("Combat")]
	public AttackStages AttackState;

	public int CurrentAttack;

	[Header("Animation")]
	public int AnimRange = 1;

	public int NPC_IDi;

	public NPC_Animation newAnim;

	private short facingIndex;

	private short PreviousFacing = -1;

	private int PreviousAnimRange = -1;

	private short CalcedFacing;

	private short currentHeading;

	private Vector3 direction;

	[Header("Status")]
	public bool NPC_DEAD;

	public bool Poisoned;

	public bool Paralyzed;

	public short FrozenUpdate = 0;

	public SpellEffect[] NPCStatusEffects = new SpellEffect[3];

	public GameObject NPC_Launcher;

	public float WaitTimer = 0f;

	[Header("NavMesh")]
	public NavMeshAgent Agent;

	private float targetBaseOffset = 0f;

	private float startBaseOffset = 0f;

	private float floatTime = 0f;

	public float DistanceToGtarg;

	public bool ArrivedAtDestination;

	private short StartingHP;

	[Header("Positioning")]
	public int CurTileX = 0;

	public int CurTileY = 0;

	public int prevTileX = -1;

	public int prevTileY = -1;

	public int DestTileX;

	public int DestTileY;

	public Vector3 destinationVector;

	public bool isUndead
	{
		get
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				switch (base.item_id)
				{
				case 72:
				case 85:
				case 93:
				case 105:
				case 106:
				case 107:
					return true;
				default:
					return false;
				}
			}
			switch (base.item_id)
			{
			case 97:
			case 99:
			case 100:
			case 101:
			case 105:
			case 110:
			case 113:
				return true;
			default:
				return false;
			}
		}
	}

	public bool MagicAttack
	{
		get
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				int num = base.item_id;
				if (num == 104 || num == 105 || num == 75 || num == 88 || num == 96 || num == 111 || num == 117)
				{
					return true;
				}
				return false;
			}
			switch (base.item_id)
			{
			case 69:
			case 75:
			case 81:
			case 102:
			case 103:
			case 106:
			case 107:
			case 108:
			case 109:
			case 110:
			case 115:
			case 120:
			case 122:
			case 123:
				return true;
			default:
				return false;
			}
		}
	}

	public bool RangeAttack
	{
		get
		{
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				int num = base.item_id;
				if (num == 73 || num == 74 || num == 82 || num == 110)
				{
					return true;
				}
				return false;
			}
			switch (base.item_id)
			{
			case 70:
			case 71:
			case 76:
			case 77:
			case 78:
				return true;
			default:
				return false;
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		if (base.npc_whoami != 0)
		{
			debugname = StringController.instance.GetString(7, base.npc_whoami + 16);
		}
		else
		{
			debugname = StringController.instance.GetSimpleObjectNameUW(base.item_id);
		}
		NPC_IDi = base.item_id;
		StartingHP = base.npc_hp;
		newAnim = base.gameObject.AddComponent<NPC_Animation>();
		if (GameWorldController.instance.critsLoader[NPC_IDi - 64] == null)
		{
			GameWorldController.instance.critsLoader[NPC_IDi - 64] = new CritLoader(NPC_IDi - 64);
		}
		newAnim.critAnim = GameWorldController.instance.critsLoader[NPC_IDi - 64].critter.AnimInfo;
		newAnim.output = GetComponentInChildren<SpriteRenderer>();
		DestTileX = base.ObjectTileX;
		DestTileY = base.ObjectTileY;
		if (base.npc_goal == 15)
		{
			SpellEffectPetrified spellEffectPetrified = base.gameObject.AddComponent<SpellEffectPetrified>();
			spellEffectPetrified.counter = base.npc_gtarg;
			spellEffectPetrified.Go();
		}
	}

	private void AI_INIT()
	{
		if (GameWorldController.NavMeshReady && Agent == null)
		{
			Vector3 tileVector = UWEBase.CurrentTileMap().getTileVector(CurTileX, CurTileY);
			int num = (int)AgentMask();
			num = 1 << num;
			NavMeshHit hit;
			NavMesh.SamplePosition(tileVector, out hit, 0.2f, num);
			if (hit.hit)
			{
				AddAgent(num);
				Agent.Warp(objInt().startPos);
			}
		}
	}

	private void AddAgent(int mask)
	{
		int agentTypeID = GameWorldController.instance.NavMeshLand.agentTypeID;
		Agent = base.gameObject.AddComponent<NavMeshAgent>();
		Agent.autoTraverseOffMeshLink = false;
		Agent.speed = 2f * ((float)GameWorldController.instance.objDat.critterStats[base.item_id - 64].Speed / 12f);
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (base.item_id)
			{
			case 65:
			case 66:
			case 71:
			case 75:
			case 98:
			case 102:
			case 105:
			case 106:
			case 107:
			case 111:
				agentTypeID = GameWorldController.instance.NavMeshAir.agentTypeID;
				break;
			case 77:
			case 89:
				agentTypeID = GameWorldController.instance.NavMeshWater.agentTypeID;
				break;
			case 96:
				agentTypeID = GameWorldController.instance.NavMeshLava.agentTypeID;
				break;
			}
		}
		else
		{
			switch (base.item_id)
			{
			case 66:
			case 73:
			case 75:
			case 81:
			case 102:
			case 122:
			case 123:
				agentTypeID = GameWorldController.instance.NavMeshAir.agentTypeID;
				break;
			case 87:
			case 116:
				agentTypeID = GameWorldController.instance.NavMeshWater.agentTypeID;
				break;
			case 120:
				agentTypeID = GameWorldController.instance.NavMeshLava.agentTypeID;
				break;
			}
		}
		Agent.agentTypeID = agentTypeID;
		Agent.areaMask = mask;
	}

	private AgentMasks AgentMask()
	{
		AgentMasks result = AgentMasks.LAND;
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (base.item_id)
			{
			case 65:
			case 66:
			case 71:
			case 75:
			case 98:
			case 102:
			case 105:
			case 106:
			case 107:
			case 111:
				return AgentMasks.AIR;
			case 77:
			case 89:
				return AgentMasks.WATER;
			case 96:
				return AgentMasks.LAVA;
			}
		}
		else
		{
			switch (base.item_id)
			{
			case 66:
			case 73:
			case 75:
			case 81:
			case 100:
			case 101:
			case 102:
			case 122:
			case 123:
				return AgentMasks.AIR;
			case 87:
			case 116:
				return AgentMasks.WATER;
			case 120:
				return AgentMasks.LAVA;
			}
		}
		return result;
	}

	private void OnDeath()
	{
		if (SpecialDeathCases())
		{
			return;
		}
		if (UWEBase._RES == "UW2" && Quest.instance.FightingInArena)
		{
			for (int i = 0; i <= Quest.instance.ArenaOpponents.GetUpperBound(0); i++)
			{
				if (Quest.instance.ArenaOpponents[i] == objInt().objectloaderinfo.index)
				{
					Quest.instance.ArenaOpponents[i] = 0;
					Quest.instance.QuestVariables[129] = Mathf.Min(255, Quest.instance.QuestVariables[129] + 1);
					Quest.instance.x_clocks[14] = Mathf.Min(255, Quest.instance.x_clocks[14] + 1);
					Quest.instance.QuestVariables[24] = 1;
				}
			}
		}
		objInt().objectloaderinfo.InUseFlag = 0;
		objInt().objectloaderinfo.npc_hp = 0;
		NPC_DEAD = true;
		PerformDeathAnim();
		Container component = GetComponent<Container>();
		if (component != null)
		{
			SetupNPCInventory();
			component.SpillContents();
		}
		UWCharacter.Instance.AddXP(GameWorldController.instance.objDat.critterStats[base.item_id - 64].Exp);
		switch ((NPCCategory)GameWorldController.instance.objDat.critterStats[base.item_id - 64].Category)
		{
		case NPCCategory.ethereal:
		case NPCCategory.flying:
			objInt().aud.clip = MusicController.instance.SoundEffects[35];
			break;
		case NPCCategory.swimming:
			objInt().aud.clip = MusicController.instance.SoundEffects[24];
			break;
		case NPCCategory.creeping:
		case NPCCategory.crawling:
			objInt().aud.clip = MusicController.instance.SoundEffects[34];
			break;
		case NPCCategory.golem:
			objInt().aud.clip = MusicController.instance.SoundEffects[18];
			break;
		default:
			objInt().aud.clip = MusicController.instance.SoundEffects[6];
			break;
		}
		if (ObjectInteraction.PlaySoundEffects)
		{
			objInt().aud.Play();
		}
	}

	public void SetupNPCInventory()
	{
		if (UWEBase._RES != "UW2" && base.item_id == 64)
		{
			return;
		}
		Container component = GetComponent<Container>();
		if (!(component != null) || component.CountItems() != 0)
		{
			return;
		}
		for (int i = 0; i <= GameWorldController.instance.objDat.critterStats[base.item_id - 64].Loot.GetUpperBound(0); i++)
		{
			if (GameWorldController.instance.objDat.critterStats[base.item_id - 64].Loot[i] != -1)
			{
				int num = GameWorldController.instance.objDat.critterStats[base.item_id - 64].Loot[i];
				ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(num, Random.Range(1, 41), 0, 0, 256);
				if (num == 16)
				{
					objectLoaderInfo.is_quant = 1;
					objectLoaderInfo.link = Random.Range(1, 10);
					objectLoaderInfo.quality = 40;
				}
				else
				{
					objectLoaderInfo.is_quant = 0;
				}
				objectLoaderInfo.instance = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance._ObjectMarker, GameWorldController.instance.InventoryMarker.transform.position);
				component.AddItemToContainer(objectLoaderInfo.instance);
			}
		}
	}

	private bool SpecialDeathCases()
	{
		switch (UWEBase._RES)
		{
		case "UW1":
			switch (base.npc_whoami)
			{
			case 22:
				if (!ConversationVM.InConversation)
				{
					NPC_DEAD = false;
					TalkTo();
				}
				return true;
			case 110:
				Quest.instance.QuestVariables[4] = 1;
				return false;
			case 142:
				Quest.instance.QuestVariables[11] = 1;
				return false;
			case 231:
				Quest.instance.GaramonDream = 7;
				Quest.instance.DayGaramonDream = GameClock.day();
				UWCharacter.Instance.PlayerMagic.CastEnchantment(base.gameObject, null, 226, 1, -1);
				return false;
			}
			break;
		case "UW2":
			if (GameWorldController.instance.LevelNo == 3 && base.item_id == 78)
			{
				Quest.instance.QuestVariables[135]++;
			}
			switch (base.npc_whoami)
			{
			case 32:
			{
				Quest.instance.QuestVariables[7] = 1;
				ObjectInteraction instance = UWEBase.CurrentObjectList().objInfo[961].instance;
				if (instance != null && instance.GetComponent<trigger_base>() != null)
				{
					instance.GetComponent<trigger_base>().Activate(null);
				}
				break;
			}
			case 47:
				if (Quest.instance.QuestVariables[117] == 0)
				{
					Quest.instance.QuestVariables[117] = 1;
					base.npc_hp = 50;
					base.npc_goal = 0;
					base.npc_attitude = 1;
					TalkTo();
					return true;
				}
				break;
			case 58:
				Quest.instance.QuestVariables[50] = 1;
				return false;
			case 75:
				if (NPC_IDi == 108)
				{
					NPC_IDi = 94;
					base.item_id = 94;
					base.npc_hp = 92;
					NPC_DEAD = false;
					if (GameWorldController.instance.critsLoader[NPC_IDi - 64] == null)
					{
						GameWorldController.instance.critsLoader[NPC_IDi - 64] = new CritLoader(NPC_IDi - 64);
					}
					newAnim.critAnim = GameWorldController.instance.critsLoader[NPC_IDi - 64].critter.AnimInfo;
					return true;
				}
				return false;
			case 98:
				Quest.instance.QuestVariables[25] = 1;
				return false;
			case 99:
				Quest.instance.QuestVariables[121] = 1;
				return false;
			case 145:
				Quest.instance.QuestVariables[11] = 1;
				Quest.instance.x_clocks[1]++;
				return false;
			case 152:
			{
				Quest.instance.QuestVariables[122] = 1;
				ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(638);
				if (objectIntAt != null && objectIntAt.GetComponent<trigger_base>() != null)
				{
					objectIntAt.GetComponent<trigger_base>().Activate(UWCharacter.Instance.gameObject);
				}
				objectIntAt = ObjectLoader.getObjectIntAt(649);
				if (objectIntAt != null && objectIntAt.GetComponent<trigger_base>() != null)
				{
					objectIntAt.GetComponent<trigger_base>().Activate(UWCharacter.Instance.gameObject);
				}
				return false;
			}
			}
			break;
		}
		return false;
	}

	public override void Update()
	{
		if (UWEBase.EditorMode || ConversationVM.InConversation)
		{
			return;
		}
		bool flag = isNPCFrozen();
		newAnim.FreezeAnimFrame = flag;
		if (WaitTimer > 0f && !flag)
		{
			WaitTimer -= Time.deltaTime;
			if (WaitTimer < 0f)
			{
				WaitTimer = 0f;
			}
		}
		if (flag && Agent != null)
		{
			AgentStand();
		}
		if (NPC_DEAD)
		{
			UpdateSprite();
			if (WaitTimer <= 0f && !flag)
			{
				DumpRemains();
			}
			return;
		}
		CurTileX = (int)(base.transform.position.x / 1.2f);
		CurTileY = (int)(base.transform.position.z / 1.2f);
		UpdateNPCAWake();
		UpdateSpecialNPCBehaviours();
		UpdateSprite();
		UpdateGTarg();
		if (base.npc_hp <= 0)
		{
			OnDeath();
		}
		else
		{
			UpdateGoals();
		}
	}

	private void UpdateSpecialNPCBehaviours()
	{
		if (UWEBase._RES == "UW2" && base.npc_whoami == 142 && GameWorldController.instance.LevelNo == 0 && Quest.instance.QuestVariables[112] == 1)
		{
			base.npc_xhome = 40;
			base.npc_yhome = 38;
		}
	}

	private void UpdateNPCAWake()
	{
		if (Vector3.Distance(base.transform.position, UWCharacter.Instance.CameraPos) <= 10f)
		{
			if (objInt() != null)
			{
				if (TileMap.ValidTile(CurTileX, CurTileY))
				{
					AI_INIT();
				}
				newAnim.enabled = true;
			}
		}
		else
		{
			newAnim.enabled = false;
		}
	}

	private void NPCFollow()
	{
		if (!(gtarg != null))
		{
			return;
		}
		Vector3 vector = base.transform.position - gtarg.transform.position;
		Vector3 destination = gtarg.transform.position + 0.9f * vector.normalized;
		Agent.destination = destination;
		Agent.isStopped = false;
		if (!(gtarg.name == "_Gronk"))
		{
			return;
		}
		if (UWCharacter.Instance.HelpMeMyFriends)
		{
			UWCharacter.Instance.HelpMeMyFriends = false;
			if (UWCharacter.Instance.LastEnemyToHitMe != null)
			{
				gtarg = UWCharacter.Instance.LastEnemyToHitMe;
				base.npc_goal = 5;
				base.npc_gtarg = (short)UWCharacter.Instance.LastEnemyToHitMe.GetComponent<ObjectInteraction>().objectloaderinfo.index;
				gtargName = UWCharacter.Instance.LastEnemyToHitMe.name;
			}
		}
		if (UWEBase._RES == "UW1" && GameWorldController.instance.LevelNo == 8 && base.item_id == 124)
		{
			base.npc_goal = 5;
			gtarg = UWCharacter.Instance.gameObject;
		}
	}

	private void UpdateGoalsForNonAgents()
	{
		npc_goals npc_goals = (npc_goals)base.npc_goal;
		if (npc_goals == npc_goals.npc_goal_want_to_talk)
		{
			AnimRange = 1;
			if (!UWCharacter.Instance.isRoaming && !ConversationVM.InConversation && (double)Vector3.Distance(base.transform.position, UWCharacter.Instance.CameraPos) <= 1.5)
			{
				TalkTo();
			}
		}
	}

	private void UpdateGoals()
	{
		if (Agent == null)
		{
			if (GameWorldController.NavMeshReady)
			{
				UpdateGoalsForNonAgents();
			}
		}
		else
		{
			if (!Agent.isOnNavMesh || isNPCFrozen() || !GameWorldController.NavMeshReady)
			{
				return;
			}
			DistanceToGtarg = getDistanceToGtarg();
			if (base.npc_attitude == 0 && base.npc_goal != 5 && base.npc_goal != 9 && DistanceToGtarg <= UWCharacter.Instance.DetectionRange && base.npc_gtarg <= 3)
			{
				base.npc_goal = 5;
				base.npc_gtarg = 1;
			}
			if (targetBaseOffset != Agent.baseOffset)
			{
				floatTime += Time.deltaTime;
				Agent.baseOffset = Mathf.Lerp(startBaseOffset, targetBaseOffset, floatTime);
			}
			switch ((npc_goals)base.npc_goal)
			{
			case npc_goals.npc_goal_want_to_talk:
				AnimRange = 1;
				if (!UWCharacter.Instance.isRoaming && !ConversationVM.InConversation && (double)Vector3.Distance(base.transform.position, UWCharacter.Instance.CameraPos) <= 1.5)
				{
					TalkTo();
				}
				break;
			case npc_goals.npc_goal_stand_still_0:
			case npc_goals.npc_goal_goto_1:
			case npc_goals.npc_goal_stand_still_7:
			case npc_goals.npc_goal_stand_still_11:
			case npc_goals.npc_goal_stand_still_12:
				DestTileX = base.npc_xhome;
				DestTileY = base.npc_yhome;
				if (CurTileX != base.npc_xhome || CurTileY != base.npc_yhome)
				{
					AnimRange = 10;
					AgentGotoDestTileXY(ref DestTileX, ref DestTileY, ref CurTileX, ref CurTileY);
				}
				else
				{
					AnimRange = 1;
					AgentStand();
				}
				break;
			case npc_goals.npc_goal_wander_2:
			case npc_goals.npc_goal_wander_4:
			case npc_goals.npc_goal_flee:
			case npc_goals.npc_goal_wander_8:
				NPCWanderUpdate();
				break;
			case npc_goals.npc_goal_attack_5:
			case npc_goals.npc_goal_attack_9:
				NPCCombatUpdate();
				break;
			case npc_goals.npc_goal_follow:
				NPCFollow();
				break;
			case npc_goals.npc_goal_petrified:
				AgentStand();
				break;
			}
			if (CurTileX != prevTileX || CurTileY != prevTileY)
			{
				switch ((npc_goals)base.npc_goal)
				{
				case npc_goals.npc_goal_stand_still_0:
				case npc_goals.npc_goal_stand_still_7:
				case npc_goals.npc_goal_want_to_talk:
				case npc_goals.npc_goal_stand_still_11:
				case npc_goals.npc_goal_stand_still_12:
					if (CurTileX != base.npc_xhome && CurTileY != base.npc_yhome)
					{
						NPCDoorUse();
					}
					break;
				default:
					NPCDoorUse();
					break;
				}
			}
			prevTileX = CurTileX;
			prevTileY = CurTileY;
		}
	}

	private void NPCDoorUse()
	{
		NPCCategory category = (NPCCategory)GameWorldController.instance.objDat.critterStats[base.item_id - 64].Category;
		if (category != NPCCategory.human && category != NPCCategory.humanoid)
		{
			return;
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (((i != 0 || (j != -1 && j != 1)) && (j != 0 || (i != -1 && i != 1)) && (i != 0 || j != 0)) || !TileMap.ValidTile(CurTileX + i, CurTileY + j) || !UWEBase.CurrentTileMap().Tiles[CurTileX + i, CurTileY + j].isDoor)
				{
					continue;
				}
				GameObject gameObject = DoorControl.findDoor(CurTileX + i, CurTileY + j);
				if (gameObject != null)
				{
					DoorControl component = gameObject.GetComponent<DoorControl>();
					if (component != null && !component.state() && !component.locked() && !component.Spiked() && !component.DoorBusy)
					{
						component.PlayerUse = false;
						component.Activate(base.gameObject);
					}
				}
			}
		}
	}

	private void NPCWanderUpdate()
	{
		bool flag = DestTileX == CurTileX && DestTileY == CurTileY;
		if (!flag)
		{
			AnimRange = 10;
			AgentGotoDestTileXY(ref DestTileX, ref DestTileY, ref CurTileX, ref CurTileY);
			ArrivedAtDestination = false;
		}
		else
		{
			AnimRange = 1;
			if (!ArrivedAtDestination)
			{
				ArrivedAtDestination = true;
				if (WaitTimer == 0f)
				{
					WaitTimer = Random.Range(1f, 10f);
				}
			}
		}
		if (WaitTimer <= 0f && flag && AnimRange == 1)
		{
			SetRandomDestination();
			ArrivedAtDestination = false;
		}
	}

	private void NPCCombatUpdate()
	{
		switch (AttackState)
		{
		case AttackStages.AttackPosition:
			if (gtarg != null)
			{
				Vector3 vector = base.transform.position - gtarg.transform.position;
				if (DistanceToGtarg > 1f)
				{
					if (DistanceToGtarg < 6f)
					{
						if (MagicAttack || RangeAttack)
						{
							AgentStand();
							base.transform.LookAt(gtarg.transform.position);
							if (AgentCanAttack(NPC_Launcher.transform.position, gtarg.GetComponent<UWEBase>().GetImpactPoint(), gtarg, vector.magnitude))
							{
								AnimRange = 5000;
								AttackState = AttackStages.AttackAnimateRanged;
								WaitTimer = 0.8f;
							}
							else
							{
								AgentMoveToGtarg();
							}
						}
						else
						{
							AgentMoveToGtarg();
						}
					}
					else if (DistanceToGtarg < UWCharacter.Instance.BaseEngagementRange + UWCharacter.Instance.DetectionRange || (UWEBase._RES == "UW1" && base.item_id == 124))
					{
						AgentMoveToGtarg();
					}
					else
					{
						base.npc_goal = 8;
						AgentStand();
					}
				}
				else
				{
					AgentStand();
					base.transform.LookAt(gtarg.transform.position);
					if (AgentCanAttack(NPC_Launcher.transform.position, gtarg.GetComponent<UWEBase>().GetImpactPoint(), gtarg, vector.magnitude))
					{
						SetRandomAttack();
						AttackState = AttackStages.AttackAnimateMelee;
						WaitTimer = 0.8f;
					}
					else
					{
						AgentMoveToGtarg();
					}
				}
			}
			else
			{
				AgentStand();
				AttackState = AttackStages.AttackWaitCycle;
				WaitTimer = 0.8f;
			}
			break;
		case AttackStages.AttackAnimateMelee:
			if (WaitTimer <= 0.2f)
			{
				ExecuteAttack();
				AttackState = AttackStages.AttackExecute;
				WaitTimer = 0.8f;
			}
			break;
		case AttackStages.AttackAnimateRanged:
			if (WaitTimer <= 0.2f)
			{
				if (MagicAttack)
				{
					ExecuteMagicAttack();
				}
				else if (RangeAttack)
				{
					ExecuteRangedAttack();
				}
				AttackState = AttackStages.AttackExecute;
				WaitTimer = 0.8f;
			}
			break;
		case AttackStages.AttackExecute:
			if (WaitTimer <= 0.2f)
			{
				AttackState = AttackStages.AttackWaitCycle;
				WaitTimer = 0.8f;
			}
			break;
		case AttackStages.AttackWaitCycle:
			AnimRange = 4000;
			if (WaitTimer <= 0.2f)
			{
				AttackState = AttackStages.AttackPosition;
			}
			break;
		}
	}

	private void AgentMoveToGtarg()
	{
		AgentMoveToPosition(gtarg.transform.position);
		AnimRange = 10;
	}

	private void AgentGotoDestTileXY(ref int DestinationX, ref int DestinationY, ref int tileX, ref int tileY)
	{
		if (Agent.agentTypeID == GameWorldController.instance.NavMeshAir.agentTypeID)
		{
			targetBaseOffset = 0.5f;
			startBaseOffset = Agent.baseOffset;
			floatTime = 1f;
		}
		AgentMoveToPosition(UWEBase.CurrentTileMap().getTileVector(DestTileX, DestTileY));
	}

	private void AgentStand()
	{
		if (Agent.isOnNavMesh)
		{
			destinationVector = base.transform.position;
			Agent.destination = base.transform.position;
			Agent.isStopped = true;
		}
	}

	private void AgentMoveToPosition(Vector3 dest)
	{
		if (Agent.isOnNavMesh)
		{
			destinationVector = dest;
			Agent.destination = dest;
			Agent.isStopped = false;
		}
	}

	private void UpdateGTarg()
	{
		if (base.npc_gtarg <= 5)
		{
			gtargName = "_Gronk";
			if (gtarg == null)
			{
				gtarg = UWCharacter.Instance.transform.gameObject;
			}
			else if (gtarg.name != "_Gronk")
			{
				gtarg = UWCharacter.Instance.transform.gameObject;
			}
			return;
		}
		if (gtarg == null)
		{
			if (gtargName != "")
			{
				gtarg = GameObject.Find(gtargName);
			}
		}
		else if (gtarg.name != gtargName)
		{
			gtarg = GameObject.Find(gtargName);
		}
		if (gtarg == null)
		{
			if (base.npc_attitude > 0 && gtargName != "" && (base.npc_goal == 5 || base.npc_goal == 9))
			{
				base.npc_goal = 3;
				base.npc_gtarg = 1;
				gtarg = UWCharacter.Instance.transform.gameObject;
			}
			if (base.npc_attitude == 0 && gtargName != "" && (base.npc_goal == 5 || base.npc_goal == 9))
			{
				base.npc_goal = 5;
				base.npc_gtarg = 1;
				gtarg = UWCharacter.Instance.transform.gameObject;
			}
		}
	}

	private bool AgentCanAttack(Vector3 src, Vector3 target, GameObject targetGtarg, float range)
	{
		return !Physics.Linecast(src, target, GameWorldController.instance.MapMeshLayerMask | GameWorldController.instance.DoorLayerMask);
	}

	public override bool ApplyAttack(short damage)
	{
		if (!(UWEBase._RES == "UW1") || base.item_id != 124)
		{
			base.npc_hp -= damage;
			UWHUD.instance.MonsterEyes.SetTargetFrame(base.npc_hp, StartingHP);
		}
		if (base.npc_hp < 0)
		{
			base.npc_hp = 0;
		}
		return true;
	}

	public override bool ApplyAttack(short damage, GameObject source)
	{
		if (source != null && source.name == "_Gronk")
		{
			if (base.npc_goal != 15)
			{
				base.npc_attitude = 0;
				base.npc_gtarg = 1;
				gtarg = UWCharacter.Instance.gameObject;
				gtargName = gtarg.name;
				base.npc_goal = 5;
			}
			if (base.npc_hp < 5 && Random.Range(0, 5) >= 4)
			{
				UWCharacter.Instance.PlayerMagic.CastEnchantment(source, base.gameObject, 113, 0, -1);
			}
			Collider[] array = Physics.OverlapSphere(base.transform.position, 8f);
			foreach (Collider collider in array)
			{
				if (collider.gameObject.GetComponent<NPC>() != null && AreNPCSAllied(this, collider.gameObject.GetComponent<NPC>()) && collider.gameObject.GetComponent<NPC>().CanGetAngry())
				{
					collider.gameObject.GetComponent<NPC>().npc_attitude = 0;
					collider.gameObject.GetComponent<NPC>().npc_gtarg = 1;
					collider.gameObject.GetComponent<NPC>().npc_goal = 5;
				}
			}
		}
		ApplyAttack(damage);
		return true;
	}

	private bool CanGetAngry()
	{
		if (base.npc_goal == 15)
		{
			return false;
		}
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW1" && base.npc_whoami == 27)
		{
			return false;
		}
		return true;
	}

	private static bool AreNPCSAllied(NPC srcNPC, NPC dstNPC)
	{
		return srcNPC.GetRace() == dstNPC.GetRace();
	}

	public override bool TalkTo()
	{
		GameWorldController.instance.convVM.RunConversation(this);
		return true;
	}

	public override bool LookAt()
	{
		string text = "";
		if (base.item_id != 123)
		{
			text = StringController.instance.GetFormattedObjectNameUW(objInt(), NPCMoodDesc());
		}
		if (base.npc_whoami >= 1 && base.npc_whoami < 255)
		{
			if (base.npc_whoami == 231)
			{
				text = "You see Tybal";
			}
			else if (base.npc_whoami == 207)
			{
				text = "You see an " + NPCMoodDesc() + " " + StringController.instance.GetString(7, base.npc_whoami + 16);
			}
			else if (base.npc_talkedto != 0)
			{
				text = text + " named " + StringController.instance.GetString(7, base.npc_whoami + 16);
			}
		}
		UWHUD.instance.MessageScroll.Add(text);
		return true;
	}

	private string NPCMoodDesc()
	{
		switch (base.npc_attitude)
		{
		case 0:
			return StringController.instance.GetString(5, 96);
		case 1:
			return StringController.instance.GetString(5, 97);
		case 2:
			return StringController.instance.GetString(5, 98);
		default:
			return StringController.instance.GetString(5, 99);
		}
	}

	private void UpdateSprite()
	{
		direction = UWCharacter.Instance.gameObject.transform.position - base.gameObject.transform.position;
		float angle = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
		currentHeading = CompassHeadings[(int)Mathf.Round(base.gameObject.transform.eulerAngles.y % 360f / 45f)];
		facingIndex = facing(angle);
		if (PreviousFacing != facingIndex && AnimRange != PreviousAnimRange)
		{
			PreviousFacing = facingIndex;
			PreviousAnimRange = AnimRange;
		}
		CalcedFacing = (short)(facingIndex + currentHeading);
		if (CalcedFacing >= 8)
		{
			CalcedFacing -= 8;
		}
		if (CalcedFacing <= -8)
		{
			CalcedFacing += 8;
		}
		if (CalcedFacing < 0)
		{
			CalcedFacing = (short)(8 + CalcedFacing);
		}
		else if (CalcedFacing > 7)
		{
			CalcedFacing = (short)(8 - CalcedFacing);
		}
		if ((AnimRange == 1000 && CalcedFacing != 0 && CalcedFacing != 1 && CalcedFacing != 7) || (AnimRange == 2000 && CalcedFacing != 0 && CalcedFacing != 1 && CalcedFacing != 7) || (AnimRange == 3000 && CalcedFacing != 0 && CalcedFacing != 1 && CalcedFacing != 7) || (AnimRange == 4000 && CalcedFacing != 0 && CalcedFacing != 1 && CalcedFacing != 7))
		{
			CalcedFacing++;
		}
		else
		{
			CalcedFacing = (short)((CalcedFacing + 1) * AnimRange);
		}
		switch (CalcedFacing)
		{
		case 1:
			playAnimation(14, false);
			return;
		case 2:
			playAnimation(13, false);
			return;
		case 3:
			playAnimation(12, false);
			return;
		case 4:
			playAnimation(11, false);
			return;
		case 5:
			playAnimation(10, false);
			return;
		case 6:
			playAnimation(17, false);
			return;
		case 7:
			playAnimation(16, false);
			return;
		case 8:
			playAnimation(15, false);
			return;
		case 10:
			playAnimation(22, false);
			return;
		case 20:
			playAnimation(21, false);
			return;
		case 30:
			playAnimation(20, false);
			return;
		case 40:
			playAnimation(19, false);
			return;
		case 50:
			playAnimation(18, false);
			return;
		case 60:
			playAnimation(25, false);
			return;
		case 70:
			playAnimation(24, false);
			return;
		case 80:
			playAnimation(23, false);
			return;
		}
		switch (AnimRange)
		{
		case 100:
			playAnimation(8, true);
			break;
		case 1000:
			playAnimation(1, true);
			break;
		case 2000:
			playAnimation(2, true);
			break;
		case 3000:
			playAnimation(3, true);
			break;
		case 4000:
			playAnimation(0, true);
			break;
		case 5000:
			playAnimation(5, true);
			break;
		}
	}

	private short facing(float angle)
	{
		if ((double)angle >= -22.5 && (double)angle <= 22.5)
		{
			return 0;
		}
		if ((double)angle > 22.5 && (double)angle <= 67.5)
		{
			return 1;
		}
		if ((double)angle > 67.5 && (double)angle <= 112.5)
		{
			return 2;
		}
		if ((double)angle > 112.5 && (double)angle <= 157.5)
		{
			return 3;
		}
		if (((double)angle > 157.5 && (double)angle <= 180.0) || (angle >= -180f && (double)angle <= -157.5))
		{
			return 4;
		}
		if ((double)angle >= -157.5 && (double)angle < -112.5)
		{
			return 5;
		}
		if ((double)angle > -112.5 && (double)angle < -67.5)
		{
			return 6;
		}
		if ((double)angle > -67.5 && (double)angle < -22.5)
		{
			return 7;
		}
		return 0;
	}

	protected void playAnimation(int index, bool isConstantAnim)
	{
		newAnim.Play(index, isConstantAnim);
	}

	public void ExecuteAttack()
	{
		if (ConversationVM.InConversation || gtarg == null)
		{
			return;
		}
		float maxDistance = 1.5f;
		Vector3 impactPoint = gtarg.GetComponent<UWEBase>().GetImpactPoint();
		Ray ray = new Ray(NPC_Launcher.transform.position, impactPoint - NPC_Launcher.transform.position);
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, maxDistance))
		{
			if (hitInfo.transform.Equals(base.transform))
			{
				Debug.Log("you've hit yourself ? " + hitInfo.transform.name);
			}
			else if (hitInfo.transform.name == UWCharacter.Instance.name)
			{
				UWCombat.NPC_Hits_PC(UWCharacter.Instance, this);
			}
			else if (hitInfo.transform.GetComponent<NPC>() != null)
			{
				UWCombat.NPC_Hits_NPC(hitInfo.transform.GetComponent<NPC>(), this);
			}
			else if (hitInfo.transform.GetComponent<ObjectInteraction>() != null)
			{
				short damage = (short)Random.Range(1, GetDamage() + 1);
				hitInfo.transform.GetComponent<ObjectInteraction>().Attack(damage, base.gameObject);
			}
			else
			{
				Impact.SpawnHitImpact(Impact.ImpactDamage(), GetImpactPoint(), 46, 50);
			}
		}
	}

	public void ExecuteMagicAttack()
	{
		if (!(Vector3.Distance(base.transform.position, UWCharacter.Instance.CameraPos) > 8f))
		{
			UWCharacter.Instance.PlayerMagic.CastEnchantmentImmediate(NPC_Launcher, gtarg, SpellIndex(), 2, 0);
		}
	}

	public void ExecuteRangedAttack()
	{
		if (Vector3.Distance(base.transform.position, UWCharacter.Instance.CameraPos) > 8f)
		{
			return;
		}
		Vector3 impactPoint = gtarg.GetComponent<UWEBase>().GetImpactPoint();
		Vector3 normalized = (impactPoint - NPC_Launcher.transform.position).normalized;
		normalized += new Vector3(0f, 0.3f, 0f);
		Ray ray = new Ray(NPC_Launcher.transform.position, normalized);
		RaycastHit hitInfo = default(RaycastHit);
		float num = 0.5f;
		if (!Physics.Raycast(ray, out hitInfo, num))
		{
			float num2 = 100f * Vector3.Distance(impactPoint, NPC_Launcher.transform.position);
			int num3 = RangedAttackProjectile();
			ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(num3, 0, 0, 1, 256);
			if (objectLoaderInfo != null)
			{
				objectLoaderInfo.is_quant = 1;
				GameObject gameObject = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, ray.GetPoint(num - 0.1f)).gameObject;
				UWEBase.UnFreezeMovement(gameObject);
				Vector3 vector = normalized;
				gameObject.GetComponent<Rigidbody>().AddForce(vector * num2);
				GameObject gameObject2 = new GameObject(gameObject.name + "_damage");
				gameObject2.transform.position = gameObject.transform.position;
				gameObject2.transform.parent = gameObject.transform;
				ProjectileDamage projectileDamage = gameObject2.AddComponent<ProjectileDamage>();
				projectileDamage.Source = base.gameObject;
				projectileDamage.Damage = (short)GameWorldController.instance.objDat.rangedStats[num3 - 16].damage;
				projectileDamage.AttackCharge = 100f;
				projectileDamage.AttackScore = GetAttack();
				projectileDamage.ArmourDamage = GetArmourDamage();
			}
		}
	}

	private int RangedAttackProjectile()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (base.item_id)
			{
			case 110:
				return 21;
			case 20:
				return 28;
			default:
				return 16;
			}
		}
		return 16;
	}

	public override string ContextMenuDesc(int item_id)
	{
		if (base.npc_talkedto != 0 && base.npc_whoami != 0)
		{
			return StringController.instance.GetString(7, base.npc_whoami + 16);
		}
		return base.ContextMenuDesc(item_id);
	}

	public override string ContextMenuUsedDesc()
	{
		object_base.TalkAvail = true;
		return base.ContextMenuUsedDesc();
	}

	public override string UseVerb()
	{
		return "talk";
	}

	public override Vector3 GetImpactPoint()
	{
		return NPC_Launcher.transform.position;
	}

	public override GameObject GetImpactGameObject()
	{
		return NPC_Launcher;
	}

	public GameObject getGtarg()
	{
		return gtarg;
	}

	public int GetAttack()
	{
		return GameWorldController.instance.objDat.critterStats[base.item_id - 64].AttackPower;
	}

	public int GetDamage()
	{
		return GameWorldController.instance.objDat.critterStats[base.item_id - 64].AttackDamage[CurrentAttack];
	}

	public int GetDefence()
	{
		return GameWorldController.instance.objDat.critterStats[base.item_id - 64].Defence;
	}

	public int GetArmourDamage()
	{
		return GameWorldController.instance.objDat.critterStats[base.item_id - 64].EquipDamage;
	}

	public int GetRace()
	{
		return GameWorldController.instance.objDat.critterStats[base.item_id - 64].Race;
	}

	public int Room()
	{
		int num = (int)(base.transform.position.x / 1.2f);
		int num2 = (int)(base.transform.position.z / 1.2f);
		if (TileMap.ValidTile(num, num2))
		{
			return UWEBase.CurrentTileMap().Tiles[num, num2].roomRegion;
		}
		return 0;
	}

	public int SpellIndex()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			switch (base.item_id)
			{
			case 88:
				return 210;
			case 96:
			case 104:
			case 105:
				return 289;
			default:
				return 258;
			}
		}
		switch (base.item_id)
		{
		case 69:
			return 305;
		case 120:
			return 280;
		case 123:
			return 287;
		default:
			return 258;
		}
	}

	private void SetRandomAttack()
	{
		int num = Random.Range(1, 100);
		int num2 = 0;
		int num3 = 0;
		for (num3 = 0; num3 <= GameWorldController.instance.objDat.critterStats[NPC_IDi - 64].AttackProbability.GetUpperBound(0); num3++)
		{
			num2 += GameWorldController.instance.objDat.critterStats[NPC_IDi - 64].AttackProbability[num3];
			if (num <= num2)
			{
				CurrentAttack = num3;
				break;
			}
		}
		switch (num3)
		{
		case 1:
			AnimRange = 2000;
			break;
		case 2:
			AnimRange = 3000;
			break;
		default:
			AnimRange = 1000;
			break;
		}
	}

	private void SetRandomDestination()
	{
		int num = 1;
		if (base.npc_gtarg == 6)
		{
			num = 9;
		}
		bool flag = false;
		Vector3 position = base.transform.position;
		for (int i = 0; i < 6; i++)
		{
			int num2 = base.npc_xhome + Random.Range(-2 * num, 3 * num);
			int num3 = base.npc_yhome + Random.Range(-2 * num, 3 * num);
			if (TileMap.ValidTile(DestTileX, DestTileY))
			{
				if (Room() == UWEBase.CurrentTileMap().GetRoom(num2, num3))
				{
					DestTileX = num2;
					DestTileY = num3;
					flag = true;
					break;
				}
				if (!flag)
				{
					DestTileX = CurTileX;
					DestTileY = CurTileY;
				}
			}
		}
		float num4 = Mathf.Pow(DestTileX - base.npc_xhome, 5f);
		float num5 = Mathf.Pow(DestTileY - base.npc_yhome, 5f);
		if (num5 + num4 >= 10f)
		{
			DestTileX = base.npc_xhome;
			DestTileY = base.npc_yhome;
		}
	}

	private void PerformDeathAnim()
	{
		AnimRange = 100;
		MusicController.LastAttackCounter = 0f;
		MusicController.instance.PlaySpecialClip(MusicController.instance.VictoryTracks);
		WaitTimer = 0.8f;
	}

	private void DumpRemains()
	{
		int num = -1;
		switch (GameWorldController.instance.objDat.critterStats[base.item_id - 64].Remains)
		{
		case 2:
			num = 217;
			break;
		case 4:
			num = 218;
			break;
		case 6:
			num = 219;
			break;
		case 8:
			num = 220;
			break;
		case 10:
			num = 221;
			break;
		case 12:
			num = 222;
			break;
		case 14:
			num = 223;
			break;
		case 0:
			num = -1;
			break;
		default:
			Debug.Log(base.name + " should drop " + GameWorldController.instance.objDat.critterStats[base.item_id - 64].Remains);
			num = -1;
			break;
		}
		if (num != -1)
		{
			ObjectLoaderInfo currObj = ObjectLoader.newObject(num, 0, 0, 0, 256);
			ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, base.transform.position);
		}
		Object.Destroy(base.gameObject);
	}

	public static int findNpcByWhoAmI(int whoami)
	{
		ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
		for (int i = 1; i < 256; i++)
		{
			if (objInfo[i].npc_whoami == whoami)
			{
				return i;
			}
		}
		return -1;
	}

	public static void SetNPCLocation(int index, int xhome, int yhome, npc_goals NewGoal)
	{
		if (index < 0)
		{
			return;
		}
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(index);
		if (!(objectIntAt != null))
		{
			return;
		}
		NPC component = objectIntAt.GetComponent<NPC>();
		if (component != null)
		{
			component.npc_xhome = (short)xhome;
			component.npc_yhome = (short)yhome;
			if ((short)NewGoal >= 0)
			{
				component.npc_goal = (short)NewGoal;
			}
		}
	}

	public static void SetNPCAttitudeGoal(int index, npc_goals NewGoal, short NewAttitude)
	{
		if (index < 0)
		{
			return;
		}
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(index);
		if (!(objectIntAt != null))
		{
			return;
		}
		NPC component = objectIntAt.GetComponent<NPC>();
		if (component != null)
		{
			if ((short)NewGoal >= 0)
			{
				component.npc_goal = (short)NewGoal;
			}
			component.npc_attitude = NewAttitude;
		}
	}

	public short PoisonLevel()
	{
		return (short)GameWorldController.instance.objDat.critterStats[base.item_id - 64].Poison;
	}

	public float getDistanceToGtarg()
	{
		if (gtarg != null)
		{
			if (!(Agent != null))
			{
				return (base.transform.position - gtarg.transform.position).magnitude;
			}
			if (Agent.agentTypeID == GameWorldController.instance.NavMeshAir.agentTypeID)
			{
				return (base.transform.position - gtarg.transform.position).magnitude;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (NavMesh.CalculatePath(base.transform.position, gtarg.transform.position, Agent.areaMask, navMeshPath))
			{
				float num = 0f;
				if (navMeshPath.status != NavMeshPathStatus.PathInvalid)
				{
					if (navMeshPath.status == NavMeshPathStatus.PathPartial)
					{
						num = 8f;
					}
					for (int i = 1; i < navMeshPath.corners.Length; i++)
					{
						num += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
					}
				}
				return num;
			}
		}
		return 1000f;
	}

	private bool isNPCFrozen()
	{
		return UWCharacter.Instance.isTimeFrozen || Paralyzed;
	}

	public Texture2D UW2NPCPortrait()
	{
		switch (base.npc_whoami)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 34:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 56:
		case 57:
		case 72:
		case 73:
		case 74:
		case 75:
		case 81:
		case 82:
		case 88:
		case 96:
		case 97:
		case 98:
		case 99:
		case 100:
		case 101:
		case 105:
		case 128:
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
		case 140:
		case 141:
		case 142:
		case 143:
		case 145:
		case 146:
		case 149:
		case 152:
		case 153:
		case 156:
		case 168:
		{
			GRLoader gRLoader2 = new GRLoader(7);
			return gRLoader2.LoadImageAt(base.npc_whoami - 1);
		}
		default:
		{
			int num = base.item_id - 64;
			if (num > 59)
			{
				num = 0;
			}
			GRLoader gRLoader = new GRLoader(33);
			return gRLoader.LoadImageAt(num);
		}
		}
	}
}
