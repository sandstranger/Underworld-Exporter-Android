public class Quest : UWEBase
{
	public int[] QuestVariables = new int[256];

	public int[] variables = new int[128];

	public int[] BitVariables = new int[128];

	public int[] x_clocks = new int[16];

	public const int TalismanSword = 10;

	public const int TalismanShield = 55;

	public const int TalismanTaper = 147;

	public const int TalismanTaperLit = 151;

	public const int TalismanCup = 174;

	public const int TalismanBook = 310;

	public const int TalismanWine = 191;

	public const int TalismanRing = 54;

	public const int TalismanHonour = 287;

	public int TalismansRemaining;

	public int GaramonDream;

	public int IncenseDream;

	public int DayGaramonDream = -1;

	public bool isOrbDestroyed;

	public bool isGaramonBuried;

	public bool isCupFound;

	public bool FightingInArena = false;

	public int[] ArenaOpponents = new int[5];

	public bool DreamPlantEaten = false;

	public bool InDreamWorld = false;

	public static Quest instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			QuestVariables = new int[147];
		}
		else
		{
			QuestVariables = new int[36];
		}
	}

	public int getIncenseDream()
	{
		if (IncenseDream >= 3)
		{
			IncenseDream = 0;
		}
		return IncenseDream++;
	}
}
