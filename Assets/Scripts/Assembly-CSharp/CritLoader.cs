public class CritLoader : ArtLoader
{
	public CritterInfo critter;

	public CritLoader(int CritterToLoad)
	{
		switch (UWClass._RES)
		{
		case "UW2":
			ReadUW2AssocFile(CritterToLoad);
			break;
		case "UW0":
			ReadUw1AssocFile(CritterToLoad, "CRIT" + UWClass.sep + "DASSOC.ANM");
			break;
		default:
			ReadUw1AssocFile(CritterToLoad, "CRIT" + UWClass.sep + "ASSOC.ANM");
			break;
		}
	}

	private void ReadUw1AssocFile(int CritterToLoad, string assocpath)
	{
		long num = 256L;
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + assocpath, out buffer))
		{
			return;
		}
		for (int i = 0; i <= 63; i++)
		{
			int critter_id = (int)DataLoader.getValAtAddress(buffer, num++, 8);
			int auxPalNo = (int)DataLoader.getValAtAddress(buffer, num++, 8);
			if (i == CritterToLoad)
			{
				critter = new CritterInfo(critter_id, GameWorldController.instance.palLoader.Palettes[0], auxPalNo);
			}
		}
	}

	private void ReadUW2AssocFile(int CritterToLoad)
	{
		long num = 0L;
		char[] buffer;
		char[] buffer2;
		char[] buffer3;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + "CRIT" + UWClass.sep + "AS.AN", out buffer) || !DataLoader.ReadStreamFile(Loader.BasePath + "CRIT" + UWClass.sep + "PG.MP", out buffer2) || !DataLoader.ReadStreamFile(Loader.BasePath + "CRIT" + UWClass.sep + "CR.AN", out buffer3))
		{
			return;
		}
		for (int i = 0; i <= 63; i++)
		{
			int num2 = (int)DataLoader.getValAtAddress(buffer, num++, 8);
			int palno = (int)DataLoader.getValAtAddress(buffer, num++, 8);
			if (num2 != 255 && i == CritterToLoad)
			{
				critter = new CritterInfo(num2, GameWorldController.instance.palLoader.Palettes[0], palno, buffer, buffer2, buffer3);
			}
		}
	}
}
