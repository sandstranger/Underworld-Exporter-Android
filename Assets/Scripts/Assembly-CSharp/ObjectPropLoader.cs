public class ObjectPropLoader : Loader
{
	public struct ShockCommonObjectProperties
	{
		public int Mass;

		public int hp;

		public int armour;

		public int Render;

		public int Unk1;

		public int Unk2;

		public int Unk3;

		public int Offset;

		public int Unk5;

		public int Unk6;

		public int Vulner;

		public int spevul;

		public int defence;

		public int flags;

		public int ThreeDMod;

		public int frames;
	}

	public ShockCommonObjectProperties[] properties;

	public ObjectPropLoader()
	{
		int num = 5099;
		char[] buffer;
		if (DataLoader.ReadStreamFile(Loader.BasePath + UWClass.sep + "RES" + UWClass.sep + "DATA" + UWClass.sep + "OBJPROP.DAT", out buffer))
		{
			properties = new ShockCommonObjectProperties[476];
			for (int i = 0; i <= properties.GetUpperBound(0); i++)
			{
				properties[i].Mass = (int)DataLoader.getValAtAddress(buffer, num, 32);
				properties[i].hp = (int)DataLoader.getValAtAddress(buffer, num + 4, 16);
				properties[i].armour = (int)DataLoader.getValAtAddress(buffer, num + 6, 8);
				properties[i].Render = (int)DataLoader.getValAtAddress(buffer, num + 7, 8);
				properties[i].Unk1 = (int)DataLoader.getValAtAddress(buffer, num + 8, 8);
				properties[i].Unk2 = (int)DataLoader.getValAtAddress(buffer, num + 9, 8);
				properties[i].Unk3 = (int)DataLoader.getValAtAddress(buffer, num + 10, 8);
				properties[i].Offset = (int)DataLoader.getValAtAddress(buffer, num + 11, 8);
				properties[i].Unk5 = (int)DataLoader.getValAtAddress(buffer, num + 12, 8);
				properties[i].Unk6 = (int)DataLoader.getValAtAddress(buffer, num + 13, 8);
				properties[i].Vulner = (int)DataLoader.getValAtAddress(buffer, num + 14, 8);
				properties[i].spevul = (int)DataLoader.getValAtAddress(buffer, num + 15, 8);
				properties[i].defence = (int)DataLoader.getValAtAddress(buffer, num + 18, 8);
				properties[i].flags = (int)DataLoader.getValAtAddress(buffer, num + 20, 16);
				properties[i].ThreeDMod = (int)DataLoader.getValAtAddress(buffer, num + 22, 16);
				properties[i].frames = (int)DataLoader.getValAtAddress(buffer, num + 25, 8);
				num += 27;
			}
		}
	}
}
