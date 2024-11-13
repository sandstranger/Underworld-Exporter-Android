using UnityEngine;

public class Character : UWEBase
{
	public static int InteractionMode;

	public const int InteractionModeWalk = 6;

	public const int InteractionModeOptions = 0;

	public const int InteractionModeTalk = 1;

	public const int InteractionModePickup = 2;

	public const int InteractionModeLook = 3;

	public const int InteractionModeAttack = 4;

	public const int InteractionModeUse = 5;

	public const int InteractionModeInConversation = 7;

	public static int DefaultInteractionMode = 5;

	public const float BaseAIWakeRange = 8f;

	public const float BaseDetectionRange = 6f;

	public const float MinDetectionRange = 0.2f;

	[Header("Controllers")]
	public CharacterMotorC playerMotor;

	public CharacterController playerController;

	public AudioSource aud;

	public AudioSource footsteps;

	public bool step = true;

	[Header("Health")]
	[SerializeField]
	private int _MaxVit;

	[SerializeField]
	private int _CurVit;

	[Header("Interaction Ranges")]
	public float pickupRange = 2.5f;

	public float useRange = 2f;

	public float talkRange = 20f;

	public float lookRange = 25f;

	public float DetectionRange = 6f;

	public float BaseEngagementRange = 8f;

	public static bool Invincible;

	public static bool AutoKeyUse;

	[Header("Mouselook")]
	public MouseLook XAxis;

	public MouseLook YAxis;

	public bool MouseLookEnabled;

	[Header("Character")]
	public string CharName;

	[Header("Compass and Position")]
	public int currentHeading;

	protected int[] CompassHeadings = new int[17]
	{
		0, 15, 14, 13, 12, 11, 10, 9, 8, 7,
		6, 5, 4, 3, 2, 1, 0
	};

	public Vector3 dir;

	public Vector3 dirForNPC;

	[Header("Camera")]
	public Camera playerCam;

	public Vector3 CameraPos;

	protected ObjectInteraction QuantityObj = null;

	[Header("AI")]
	public GameObject LastEnemyToHitMe;

	public bool HelpMeMyFriends;

	public bool LightActive;

	public GameObject TargetPoint;

	public int MaxVIT
	{
		get
		{
			return _MaxVit;
		}
		set
		{
			_MaxVit = value;
			UWHUD.instance.FlaskHealth.UpdateFlaskDisplay();
		}
	}

	public int CurVIT
	{
		get
		{
			return _CurVit;
		}
		set
		{
			_CurVit = value;
			UWHUD.instance.FlaskHealth.UpdateFlaskDisplay();
		}
	}

	public void ApplyDamage(int damage)
	{
		if (!Invincible)
		{
			CurVIT -= damage;
		}
		CameraShake.instance.ShakeCombat(0.2f);
	}

	public void ApplyDamage(int damage, GameObject src)
	{
		ApplyDamage(damage);
		HelpMeMyFriends = true;
		LastEnemyToHitMe = src;
	}

	public virtual void Begin()
	{
		if (UWEBase._RES == "SHOCK")
		{
			InteractionMode = 2;
			UWCharacter.Instance.playerCam.rect = new Rect(0f, 0f, 1f, 1f);
		}
	}

	public virtual void Update()
	{
		currentHeading = CompassHeadings[(int)Mathf.Round(base.gameObject.transform.eulerAngles.y % 360f / 22.5f)];
		dir = Camera.main.transform.forward;
		dirForNPC = dir;
		dirForNPC.y = 0f;
		CameraPos = Camera.main.transform.position;
		if (base.transform.position.y < -10f && UWEBase.CurrentTileMap() != null)
		{
			base.transform.position = UWEBase.CurrentTileMap().getTileVector(TileMap.visitedTileX, TileMap.visitedTileY);
		}
	}

	public void UseMode()
	{
		Ray ray = ((!MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
		RaycastHit hitInfo = default(RaycastHit);
		if (!Physics.Raycast(ray, out hitInfo, GetUseRange()))
		{
			return;
		}
		ObjectInteraction component = hitInfo.transform.GetComponent<ObjectInteraction>();
		if (component != null)
		{
			component.Use();
		}
		else if (hitInfo.transform.GetComponent<PortcullisInteraction>() != null)
		{
			component = hitInfo.transform.GetComponent<PortcullisInteraction>().getParentObjectInteraction();
			if (component != null)
			{
				component.Use();
				UWHUD.instance.window.UWWindowWait(1f);
			}
		}
		else if (hitInfo.transform.parent == GameWorldController.instance.LevelModel.transform && UWEBase.EditorMode)
		{
			string[] array = hitInfo.transform.name.Split('_');
			int tileX = int.Parse(array[1]);
			int tileY = int.Parse(array[2]);
			if (IngameEditor.FollowMeMode)
			{
				IngameEditor.UpdateFollowMeMode(tileX, tileY);
			}
			else
			{
				IngameEditor.instance.SelectTile(tileX, tileY);
			}
		}
	}

	public virtual float GetUseRange()
	{
		return useRange;
	}

	public virtual float GetPickupRange()
	{
		return pickupRange;
	}

	public virtual void PickupMode(int ptrId)
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
			if (ptrId == -2)
			{
				component2 = Pickup(component2, component);
			}
		}
		else if (component2.isUsable)
		{
			UseMode();
		}
	}

	public virtual ObjectInteraction Pickup(ObjectInteraction objPicked, PlayerInventory pInv)
	{
		if (objPicked.GetComponent<Container>() != null)
		{
			Container.SetPickedUpFlag(objPicked.GetComponent<Container>(), true);
			Container.SetItemsParent(objPicked.GetComponent<Container>(), GameWorldController.instance.InventoryMarker.transform);
			Container.SetItemsPosition(objPicked.GetComponent<Container>(), GameWorldController.instance.InventoryMarker.transform.position);
		}
		pInv.ObjectInHand = objPicked;
		if (objPicked.GetComponent<Rigidbody>() != null)
		{
			UWEBase.FreezeMovement(objPicked.gameObject);
		}
		objPicked.transform.position = GameWorldController.instance.InventoryMarker.transform.position;
		objPicked.transform.parent = GameWorldController.instance.InventoryMarker.transform;
		GameWorldController.MoveToInventory(objPicked);
		pInv.ObjectInHand = objPicked;
		objPicked.Pickup();
		if (WindowDetect.ContextUIEnabled && MouseLookEnabled)
		{
			WindowDetectUW.SwitchFromMouseLook();
		}
		return objPicked;
	}

	public virtual void LookMode()
	{
		Ray ray = ((!MouseLookEnabled) ? Camera.main.ScreenPointToRay(Input.mousePosition) : Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)));
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, lookRange))
		{
			ObjectInteraction component = hitInfo.transform.GetComponent<ObjectInteraction>();
			if (component != null)
			{
				component.LookDescription();
			}
		}
	}

	public override Vector3 GetImpactPoint()
	{
		return TargetPoint.transform.position;
	}
}
