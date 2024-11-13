public class ObjectDatLoader : Loader
{
	public struct MeleeData
	{
		public short Slash;

		public short Bash;

		public short Stab;

		public short Skill;

		public short Durability;
	}

	public struct RangedData
	{
		public int ammo;

		public int damage;
	}

	public struct ArmourData
	{
		public short protection;

		public short durability;

		public short category;
	}

	public struct ContainerData
	{
		public int capacity;

		public int objectsMask;

		public int slots;
	}

	public struct LightSourceData
	{
		public int brightness;

		public int duration;
	}

	public struct CritterData
	{
		public int Level;

		public short AvgHit;

		public int AttackPower;

		public int Remains;

		public int Blood;

		public int Race;

		public int Passive;

		public int Defence;

		public int Speed;

		public int Poison;

		public int Category;

		public int EquipDamage;

		public int[] AttackChanceToHit;

		public int[] AttackDamage;

		public int[] AttackProbability;

		public int[] Loot;

		public int Exp;
	}

	public MeleeData[] weaponStats = new MeleeData[16];

	public RangedData[] rangedStats = new RangedData[8];

	public ArmourData[] armourStats = new ArmourData[32];

	public ContainerData[] containerStats = new ContainerData[16];

	public LightSourceData[] lightSourceStats = new LightSourceData[8];

	public CritterData[] critterStats = new CritterData[64];

	public ObjectDatLoader()
	{
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + UWClass.sep + "DATA" + UWClass.sep + "OBJECTS.DAT", out buffer))
		{
			return;
		}
		int num = 2;
		int num2 = 0;
		for (int i = 0; i < 16; i++)
		{
			weaponStats[num2].Slash = (short)DataLoader.getValAtAddress(buffer, num, 8);
			weaponStats[num2].Bash = (short)DataLoader.getValAtAddress(buffer, num + 1, 8);
			weaponStats[num2].Stab = (short)DataLoader.getValAtAddress(buffer, num + 2, 8);
			weaponStats[num2].Skill = (short)DataLoader.getValAtAddress(buffer, num + 6, 8);
			weaponStats[num2].Durability = (short)DataLoader.getValAtAddress(buffer, num + 7, 8);
			num += 8;
			num2++;
		}
		num = 130;
		num2 = 0;
		for (int j = 0; j < 8; j++)
		{
			rangedStats[num2].damage = (int)((DataLoader.getValAtAddress(buffer, num, 16) >> 9) & 0x7F);
			num += 3;
			num2++;
		}
		num2 = 0;
		for (int k = 0; k < 8; k++)
		{
			rangedStats[num2].ammo = (int)DataLoader.getValAtAddress(buffer, num + 2, 8) + 16;
			num += 3;
			num2++;
		}
		num = 178;
		num2 = 0;
		for (int l = 0; l < 32; l++)
		{
			armourStats[num2].protection = (short)DataLoader.getValAtAddress(buffer, num, 8);
			armourStats[num2].durability = (short)DataLoader.getValAtAddress(buffer, num + 1, 8);
			armourStats[num2].category = (short)DataLoader.getValAtAddress(buffer, num + 3, 8);
			num += 4;
			num2++;
		}
		num = 3378;
		num2 = 0;
		for (int m = 0; m < 16; m++)
		{
			containerStats[m].capacity = (int)DataLoader.getValAtAddress(buffer, num, 8);
			containerStats[m].objectsMask = (int)DataLoader.getValAtAddress(buffer, num + 1, 8);
			containerStats[m].slots = (int)DataLoader.getValAtAddress(buffer, num + 2, 8);
			num += 3;
			num2++;
		}
		num = 3426;
		num2 = 0;
		for (int n = 0; n < 8; n++)
		{
			lightSourceStats[num2].duration = (int)DataLoader.getValAtAddress(buffer, num, 8);
			lightSourceStats[num2].brightness = (int)DataLoader.getValAtAddress(buffer, num + 1, 8);
			num += 2;
			num2++;
		}
		num = 306;
		num2 = 0;
		for (int num3 = 0; num3 < 64; num3++)
		{
			critterStats[num2].Level = (int)DataLoader.getValAtAddress(buffer, num, 8);
			critterStats[num2].AvgHit = (short)DataLoader.getValAtAddress(buffer, num + 4, 16);
			critterStats[num2].AttackPower = (int)DataLoader.getValAtAddress(buffer, num + 6, 8);
			critterStats[num2].Remains = (int)((DataLoader.getValAtAddress(buffer, num + 8, 8) & 0xF0) >> 4);
			critterStats[num2].Blood = (int)DataLoader.getValAtAddress(buffer, num + 8, 8) & 0xF;
			critterStats[num2].Race = (int)DataLoader.getValAtAddress(buffer, num + 9, 8);
			critterStats[num2].Passive = (int)DataLoader.getValAtAddress(buffer, num + 10, 8);
			critterStats[num2].Defence = (int)DataLoader.getValAtAddress(buffer, num + 11, 8);
			critterStats[num2].Speed = (int)DataLoader.getValAtAddress(buffer, num + 12, 8);
			critterStats[num2].Poison = (int)DataLoader.getValAtAddress(buffer, num + 15, 8);
			critterStats[num2].Category = (int)DataLoader.getValAtAddress(buffer, num + 16, 8);
			critterStats[num2].EquipDamage = (int)DataLoader.getValAtAddress(buffer, num + 17, 8);
			critterStats[num2].AttackChanceToHit = new int[3];
			critterStats[num2].AttackDamage = new int[3];
			critterStats[num2].AttackProbability = new int[3];
			critterStats[num2].AttackChanceToHit[0] = (int)DataLoader.getValAtAddress(buffer, num + 19, 8);
			critterStats[num2].AttackDamage[0] = (int)DataLoader.getValAtAddress(buffer, num + 20, 8);
			critterStats[num2].AttackProbability[0] = (int)DataLoader.getValAtAddress(buffer, num + 21, 8);
			critterStats[num2].AttackChanceToHit[1] = (int)DataLoader.getValAtAddress(buffer, num + 22, 8);
			critterStats[num2].AttackDamage[1] = (int)DataLoader.getValAtAddress(buffer, num + 23, 8);
			critterStats[num2].AttackProbability[1] = (int)DataLoader.getValAtAddress(buffer, num + 24, 8);
			critterStats[num2].AttackChanceToHit[2] = (int)DataLoader.getValAtAddress(buffer, num + 25, 8);
			critterStats[num2].AttackDamage[2] = (int)DataLoader.getValAtAddress(buffer, num + 26, 8);
			critterStats[num2].AttackProbability[2] = (int)DataLoader.getValAtAddress(buffer, num + 27, 8);
			critterStats[num2].Exp = (int)DataLoader.getValAtAddress(buffer, num + 40, 16);
			critterStats[num2].Loot = new int[4];
			critterStats[num2].Loot[0] = -1;
			critterStats[num2].Loot[1] = -1;
			critterStats[num2].Loot[2] = -1;
			critterStats[num2].Loot[3] = -1;
			int num4 = (int)DataLoader.getValAtAddress(buffer, num + 32, 8);
			if ((num4 & 1) == 1)
			{
				critterStats[num2].Loot[0] = num4 >> 1;
			}
			num4 = (int)DataLoader.getValAtAddress(buffer, num + 32 + 1, 8);
			if ((num4 & 1) == 1)
			{
				critterStats[num2].Loot[1] = num4 >> 1;
			}
			num4 = (int)DataLoader.getValAtAddress(buffer, num + 32 + 2, 16);
			if (num4 != 0)
			{
				critterStats[num2].Loot[2] = num4 >> 4;
			}
			num4 = (int)DataLoader.getValAtAddress(buffer, num + 32 + 4, 16);
			if (num4 != 0)
			{
				critterStats[num2].Loot[3] = num4 >> 4;
			}
			num += 48;
			num2++;
		}
	}
}
