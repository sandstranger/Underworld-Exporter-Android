using System.IO;
using UnityEngine;

public class BytLoader : ArtLoader
{
	public const int BLNKMAP_BYT = 0;

	public const int CHARGEN_BYT = 1;

	public const int CONV_BYT = 2;

	public const int MAIN_BYT = 3;

	public const int OPSCR_BYT = 4;

	public const int PRES1_BYT = 5;

	public const int PRES2_BYT = 6;

	public const int WIN1_BYT = 7;

	public const int WIN2_BYT = 8;

	public const int PRESD_BYT = 9;

	public const int UW2MAIN_BYT = 5;

	private int currentIndex = -1;

	private string[] FilePaths = new string[10] { "DATA--BLNKMAP.BYT", "DATA--CHARGEN.BYT", "DATA--CONV.BYT", "DATA--MAIN.BYT", "DATA--OPSCR.BYT", "DATA--PRES1.BYT", "DATA--PRES2.BYT", "DATA--WIN1.BYT", "DATA--WIN2.BYT", "DATA--PRESD.BYT" };

	private int[] PaletteIndices = new int[10] { 3, 9, 0, 0, 6, 15, 15, 0, 22, 0 };

	private int[] PaletteIndicesUW2 = new int[11]
	{
		3, 0, 0, 0, 0, 0, 15, 15, 0, 0,
		0
	};

	public override Texture2D LoadImageAt(int index)
	{
		return LoadImageAt(index, false);
	}

	public override Texture2D LoadImageAt(int index, bool Alpha)
	{
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			return extractUW2Bitmap("DATA" + UWClass.sep + "BYT.ARK", index, Alpha);
		}
		if (File.Exists(Loader.BasePath + FilePaths[index].Replace("--", UWClass.sep.ToString()).Replace(".", "_") + UWClass.sep + "001.tga"))
		{
			return TGALoader.LoadTGA(Loader.BasePath + FilePaths[index].Replace("--", UWClass.sep.ToString()).Replace(".", "_") + UWClass.sep + "001.tga");
		}
		if (currentIndex != index)
		{
			DataLoaded = false;
			Path = FilePaths[index];
			LoadImageFile();
		}
		return ArtLoader.Image(ImageFileData, 0L, 320, 200, "name_goes_here", GameWorldController.instance.palLoader.Palettes[PaletteIndices[index]], Alpha);
	}

	public Texture2D extractUW2Bitmap(string path, int index, bool Alpha)
	{
		if (File.Exists(Loader.BasePath + path.Replace(".", "_") + UWClass.sep + index.ToString("d3") + ".tga"))
		{
			return TGALoader.LoadTGA(Loader.BasePath + path.Replace(".", "_") + UWClass.sep + index.ToString("d3") + ".tga");
		}
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + path, out buffer))
		{
			return null;
		}
		long valAtAddress = DataLoader.getValAtAddress(buffer, 0L, 8);
		long num = (int)DataLoader.getValAtAddress(buffer, index * 4 + 6, 32);
		if (num != 0)
		{
			int num2 = (int)DataLoader.getValAtAddress(buffer, index * 4 + 6 + valAtAddress * 4, 32);
			int num3 = (num2 >> 1) & 1;
			if (num3 == 1)
			{
				long datalen = 0L;
				return ArtLoader.Image(DataLoader.unpackUW2(buffer, num, ref datalen), 0L, 320, 200, "namehere", GameWorldController.instance.palLoader.Palettes[PaletteIndicesUW2[index]], Alpha);
			}
			return ArtLoader.Image(buffer, num, 320, 200, "name_goes_here", GameWorldController.instance.palLoader.Palettes[PaletteIndicesUW2[index]], Alpha);
		}
		return null;
	}
}
