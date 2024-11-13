using UnityEngine;

public class PaletteLoader : ArtLoader
{
	public Palette[] Palettes = new Palette[22];

	public int NoOfPals = 22;

	public Palette GreyScale = null;

	public PaletteLoader(string PathToResource, short chunkID)
	{
		Path = Loader.BasePath + PathToResource;
		if (UWClass._RES == "UW2")
		{
			PaletteNo = chunkID;
		}
		LoadPalettes();
	}

	private void LoadPalettes()
	{
		GreyScale = new Palette();
		for (int i = 0; i <= GreyScale.blue.GetUpperBound(0); i++)
		{
			GreyScale.red[i] = (byte)i;
			GreyScale.blue[i] = (byte)i;
			GreyScale.green[i] = (byte)i;
		}
		string rES = UWClass._RES;
		if (rES != null && rES == "SHOCK")
		{
			Palettes = new Palette[1];
			Palettes[0] = new Palette();
			char[] buffer;
			DataLoader.Chunk data_ark;
			if (DataLoader.ReadStreamFile(Path, out buffer) && DataLoader.LoadChunk(buffer, PaletteNo, out data_ark))
			{
				int num = 0;
				int num2 = 0;
				for (int j = 0; j < 256; j++)
				{
					Palettes[0].red[num] = (byte)data_ark.data[num2];
					Palettes[0].green[num] = (byte)data_ark.data[num2 + 1];
					Palettes[0].blue[num] = (byte)data_ark.data[num2 + 2];
					num2 += 3;
					num++;
				}
			}
			return;
		}
		Palettes = new Palette[NoOfPals];
		char[] buffer2;
		if (!DataLoader.ReadStreamFile(Path, out buffer2))
		{
			return;
		}
		for (int k = 0; k <= Palettes.GetUpperBound(0); k++)
		{
			Palettes[k] = new Palette();
			for (int l = 0; l < 256; l++)
			{
				Palettes[k].red[l] = (byte)(DataLoader.getValAtAddress(buffer2, k * 256 + l * 3, 8) << 2);
				Palettes[k].green[l] = (byte)(DataLoader.getValAtAddress(buffer2, k * 256 + l * 3 + 1, 8) << 2);
				Palettes[k].blue[l] = (byte)(DataLoader.getValAtAddress(buffer2, k * 256 + l * 3 + 2, 8) << 2);
			}
		}
	}

	public static int[] LoadAuxilaryPalIndices(string auxPalPath, int auxPalIndex)
	{
		int[] array = new int[16];
		char[] buffer;
		if (DataLoader.ReadStreamFile(auxPalPath, out buffer))
		{
			for (int i = 0; i < 16; i++)
			{
				array[i] = (int)DataLoader.getValAtAddress(buffer, auxPalIndex * 16 + i, 8);
			}
		}
		return array;
	}

	public static Palette LoadAuxilaryPal(string auxPalPath, Palette gamepal, int auxPalIndex)
	{
		Palette palette = new Palette();
		palette.red = new byte[16];
		palette.green = new byte[16];
		palette.blue = new byte[16];
		char[] buffer;
		if (DataLoader.ReadStreamFile(auxPalPath, out buffer))
		{
			for (int i = 0; i < 16; i++)
			{
				int num = (int)DataLoader.getValAtAddress(buffer, auxPalIndex * 16 + i, 8);
				palette.green[i] = gamepal.green[num];
				palette.blue[i] = gamepal.blue[num];
				palette.red[i] = gamepal.red[num];
			}
		}
		return palette;
	}

	public Texture2D PaletteToImage(int PalIndex)
	{
		int num = 1;
		int num2 = 256;
		char[] array = new char[num * num2];
		for (int i = 0; i < array.GetUpperBound(0); i++)
		{
			array[i] = (char)i;
		}
		return ArtLoader.Image(array, 0L, num2, num, "name here", Palettes[PalIndex], true);
	}
}
