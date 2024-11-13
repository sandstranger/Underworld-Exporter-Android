public class Chargen : Props
{
	public const int STAGE_GENDER = 0;

	public const int STAGE_HANDENESS = 1;

	public const int STAGE_CLASS = 2;

	public const int STAGE_SKILLS_1 = 3;

	public const int STAGE_SKILLS_2 = 4;

	public const int STAGE_SKILLS_3 = 5;

	public const int STAGE_SKILLS_4 = 6;

	public const int STAGE_SKILLS_5 = 7;

	public const int STAGE_PORTRAIT = 8;

	public const int STAGE_DIFFICULTY = 9;

	public const int STAGE_NAME = 10;

	public const int STAGE_CONFIRM = 11;

	private static int[] GenderChoice = new int[2] { 9, 10 };

	private static int[] PortraitsChoice = new int[5] { 0, 1, 2, 3, 4 };

	private static int[] HandednessChoice = new int[2] { 11, 12 };

	private static int[] ClassesChoice = new int[8] { 23, 24, 25, 26, 27, 28, 29, 30 };

	private static int[] BaseStr = new int[8] { 20, 12, 14, 18, 18, 12, 16, 12 };

	private static int[] BaseDex = new int[8] { 16, 16, 20, 18, 12, 18, 16, 12 };

	private static int[] BaseInt = new int[8] { 12, 20, 14, 12, 18, 18, 16, 12 };

	private static int[] SkillSeed = new int[8] { 12, 12, 12, 12, 12, 12, 12, 20 };

	private static int[][] FighterSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[2] { 31, 32 },
		new int[5] { 33, 34, 35, 36, 37 },
		new int[6] { 50, 41, 42, 46, 48, 49 }
	};

	private static int[][] MageSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[1] { 38 },
		new int[1] { 40 },
		new int[2] { 38, 40 }
	};

	private static int[][] BardSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[2] { 39, 46 },
		new int[6] { 49, 48, 44, 47, 42, 50 },
		new int[7] { 38, 40, 34, 35, 36, 33, 37 }
	};

	private static int[][] TinkerSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[1] { 45 },
		new int[5] { 33, 34, 35, 36, 37 },
		new int[5] { 47, 41, 42, 49, 45 }
	};

	private static int[][] DruidSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[1] { 40 },
		new int[1] { 38 },
		new int[3] { 43, 39, 42 }
	};

	private static int[][] PaladinSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[1] { 46 },
		new int[4] { 49, 46, 48, 45 },
		new int[5] { 33, 34, 35, 36, 37 }
	};

	private static int[][] RangerSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[1] { 43 },
		new int[6] { 41, 48, 44, 42, 50, 45 },
		new int[8] { 33, 34, 35, 36, 37, 31, 32, 43 }
	};

	private static int[][] ShepherdSkills = new int[5][]
	{
		new int[1] { 31 },
		new int[1] { 32 },
		new int[6] { 33, 34, 35, 36, 37, 32 },
		new int[10] { 41, 42, 44, 48, 49, 50, 43, 40, 39, 38 },
		new int[10] { 41, 42, 44, 48, 49, 50, 43, 40, 39, 38 }
	};

	private static int[] YesNoChoice = new int[2] { 15, 16 };

	private static int[] DifficultyChoice = new int[2] { 14, 13 };

	public static int[] GetChoices(int stage, int charclass)
	{
		switch (stage)
		{
		case 0:
			return GenderChoice;
		case 1:
			return HandednessChoice;
		case 2:
			return ClassesChoice;
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
			return GetClassChoices(stage, charclass);
		case 8:
			return PortraitsChoice;
		case 9:
			return DifficultyChoice;
		case 10:
			return YesNoChoice;
		case 11:
			return YesNoChoice;
		default:
			return new int[1] { 1 };
		}
	}

	private static int[] GetClassChoices(int stage, int charclass)
	{
		switch (charclass)
		{
		case 0:
			return FighterSkills[stage - 3];
		case 1:
			return MageSkills[stage - 3];
		case 2:
			return BardSkills[stage - 3];
		case 3:
			return TinkerSkills[stage - 3];
		case 4:
			return DruidSkills[stage - 3];
		case 5:
			return PaladinSkills[stage - 3];
		case 6:
			return RangerSkills[stage - 3];
		case 7:
			return ShepherdSkills[stage - 3];
		default:
			return new int[1] { 1 };
		}
	}

	public static int getSeed(int charClass)
	{
		return SkillSeed[charClass];
	}

	public static int getBaseSTR(int charClass)
	{
		return BaseStr[charClass];
	}

	public static int getBaseINT(int charClass)
	{
		return BaseInt[charClass];
	}

	public static int getBaseDEX(int charClass)
	{
		return BaseDex[charClass];
	}
}
