public class a_hack_trap_teleport : a_hack_trap
{
	public bool[] availableWorlds = new bool[8];

	public static int NoOfWorlds = 0;

	private const int PrisonTower = 0;

	private const int Killorn = 1;

	private const int IceCaverns = 2;

	private const int Talorus = 3;

	private const int Scintillus = 4;

	private const int Pits = 5;

	private const int Tomb = 6;

	private const int EtherealVoid = 7;

	private short[] DestinationLevels = new short[8] { 8, 16, 24, 32, 40, 56, 48, 68 };

	private short[] DestinationTileX = new short[8] { 33, 27, 18, 32, 4, 59, 32, 33 };

	private short[] DestinationTileY = new short[8] { 32, 34, 39, 31, 38, 20, 32, 4 };

	private short[] DestinationFloorHeight = new short[8] { -1, -1, -1, 24, -1, -1, -1, -1 };

	public static a_hack_trap_teleport instance;

	protected override void Start()
	{
		base.Start();
		instance = this;
		if (Quest.instance.x_clocks[1] < 4)
		{
			availableWorlds[0] = true;
			NoOfWorlds = 0;
		}
		else if (Quest.instance.x_clocks[1] < 8)
		{
			availableWorlds[0] = true;
			availableWorlds[1] = true;
			availableWorlds[2] = true;
			NoOfWorlds = 2;
		}
		else if (Quest.instance.x_clocks[1] < 13)
		{
			availableWorlds[0] = true;
			availableWorlds[1] = true;
			availableWorlds[2] = true;
			availableWorlds[3] = true;
			availableWorlds[4] = true;
			availableWorlds[5] = true;
			NoOfWorlds = 5;
		}
		else
		{
			availableWorlds[0] = true;
			availableWorlds[1] = true;
			availableWorlds[2] = true;
			availableWorlds[3] = true;
			availableWorlds[4] = true;
			availableWorlds[5] = true;
			availableWorlds[6] = true;
			availableWorlds[7] = true;
			NoOfWorlds = 7;
		}
		availableWorlds[0] = true;
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
	}

	public void TravelToWorlds(int variable, int world1, int world2)
	{
		int num = -1;
		if (variable == 0)
		{
			if (CheckWorldAvailabilty(world1))
			{
				num = world1;
			}
			else if (CheckWorldAvailabilty(world2))
			{
				num = world2;
			}
		}
		else if (CheckWorldAvailabilty(world2))
		{
			num = world2;
		}
		else if (CheckWorldAvailabilty(world1))
		{
			num = world1;
		}
		if (num == -1)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 348));
			return;
		}
		UWCharacter.Instance.JustTeleported = true;
		UWCharacter.Instance.teleportedTimer = 0f;
		GameWorldController.instance.SwitchLevel(DestinationLevels[num], DestinationTileX[num], DestinationTileY[num]);
	}

	public bool CheckWorldAvailabilty(int world)
	{
		return availableWorlds[world];
	}

	public bool TestPosXY(int xToTest, int yToTest, int TargetX, int TargetY)
	{
		if (xToTest == TargetX && yToTest == TargetY)
		{
			return true;
		}
		return false;
	}

	public override void Update()
	{
		base.Update();
		if (TileMap.visitTileX >= 27 && TileMap.visitTileX <= 29 && TileMap.visitTileY >= 39 && TileMap.visitTileY <= 41)
		{
			for (int i = 1; i <= UWHUD.instance.InWorldGemSelect.GetUpperBound(0); i++)
			{
				if (availableWorlds[i - 1])
				{
					UWHUD.instance.InWorldGemSelect[i].SetOn();
				}
				else
				{
					UWHUD.instance.InWorldGemSelect[i].SetOff();
				}
			}
			UWHUD.instance.EnableDisableControl(UWHUD.instance.WorldSelect.gameObject, UWHUD.instance.CURRENT_HUD_MODE != 3 && UWHUD.instance.CURRENT_HUD_MODE != 6 && Character.InteractionMode != 0);
		}
		else
		{
			UWHUD.instance.EnableDisableControl(UWHUD.instance.WorldSelect.gameObject, false);
		}
	}

	public void TravelDirect(int World)
	{
		if (CheckWorldAvailabilty(World))
		{
			UWHUD.instance.EnableDisableControl(UWHUD.instance.WorldSelect.gameObject, false);
			UWCharacter.Instance.JustTeleported = true;
			UWCharacter.Instance.teleportedTimer = 0f;
			GameWorldController.instance.SwitchLevel(DestinationLevels[World], DestinationTileX[World], DestinationTileY[World], DestinationFloorHeight[World]);
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 348));
		}
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
