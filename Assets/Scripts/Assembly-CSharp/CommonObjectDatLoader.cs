public class CommonObjectDatLoader : Loader
{
	public struct CommonObjectProperties
	{
		public int height;

		public int radius;

		public int animFlag;

		public int mass;

		public int Flag0;

		public int Flag1;

		public int Flag2;

		public int FlagMagicObject;

		public int FlagDecalObject;

		public int FlagCanBePickedUp;

		public int Flag6;

		public int FlagisContainer;

		public int Value;

		public int QualityClass;

		public int CanBelongTo;

		public int type;

		public int scaleValue;

		public int unk2;

		public int QualityType;

		public int LookAt;
	}

	public CommonObjectProperties[] properties;

	public CommonObjectDatLoader()
	{
		char[] buffer;
		if (DataLoader.ReadStreamFile(Loader.BasePath + UWClass.sep + "DATA" + UWClass.sep + "COMOBJ.DAT", out buffer))
		{
			int num = (buffer.GetUpperBound(0) - 2) / 11;
			properties = new CommonObjectProperties[num];
			int num2 = 2;
			for (int i = 0; i <= properties.GetUpperBound(0); i++)
			{
				properties[i].height = (int)DataLoader.getValAtAddress(buffer, num2, 8);
				properties[i].radius = (int)DataLoader.getValAtAddress(buffer, num2 + 1, 16) & 7;
				properties[i].animFlag = ((int)DataLoader.getValAtAddress(buffer, num2 + 1, 16) >> 3) & 1;
				properties[i].mass = (int)DataLoader.getValAtAddress(buffer, num2 + 1, 16) >> 4;
				properties[i].Flag0 = (int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) & 1;
				properties[i].Flag1 = ((int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) >> 1) & 1;
				properties[i].Flag2 = ((int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) >> 2) & 1;
				properties[i].FlagMagicObject = ((int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) >> 3) & 1;
				properties[i].FlagDecalObject = ((int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) >> 4) & 1;
				properties[i].FlagCanBePickedUp = ((int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) >> 5) & 1;
				properties[i].Flag6 = ((int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) >> 6) & 1;
				properties[i].FlagisContainer = ((int)DataLoader.getValAtAddress(buffer, num2 + 3, 8) >> 7) & 1;
				properties[i].Value = (int)DataLoader.getValAtAddress(buffer, num2 + 4, 16);
				properties[i].QualityClass = ((int)DataLoader.getValAtAddress(buffer, num2 + 6, 8) >> 2) & 3;
				properties[i].CanBelongTo = ((int)DataLoader.getValAtAddress(buffer, num2 + 7, 8) >> 7) & 1;
				properties[i].QualityType = (int)DataLoader.getValAtAddress(buffer, num2 + 10, 8) & 0xF;
				properties[i].LookAt = ((int)DataLoader.getValAtAddress(buffer, num2 + 10, 8) >> 3) & 1;
				num2 += 11;
			}
			properties[302].FlagCanBePickedUp = 0;
			string rES = UWClass._RES;
			if (rES != null && rES == "UW2")
			{
				properties[290].FlagCanBePickedUp = 0;
				return;
			}
			properties[279].FlagCanBePickedUp = 0;
			properties[172].CanBelongTo = 0;
		}
	}
}
