using System.IO;
using UnityEngine;

public class GRLoader : ArtLoader
{
	private const int repeat_record_start = 0;

	private const int repeat_record = 1;

	private const int run_record = 2;

	public const int ThreeDWIN_GR = 0;

	public const int ANIMO_GR = 1;

	public const int ARMOR_F_GR = 2;

	public const int ARMOR_M_GR = 3;

	public const int BODIES_GR = 4;

	public const int BUTTONS_GR = 5;

	public const int CHAINS_GR = 6;

	public const int CHARHEAD_GR = 7;

	public const int CHRBTNS_GR = 8;

	public const int COMPASS_GR = 9;

	public const int CONVERSE_GR = 10;

	public const int CURSORS_GR = 11;

	public const int DOORS_GR = 12;

	public const int DRAGONS_GR = 13;

	public const int EYES_GR = 14;

	public const int FLASKS_GR = 15;

	public const int GENHEAD_GR = 16;

	public const int HEADS_GR = 17;

	public const int INV_GR = 18;

	public const int LFTI_GR = 19;

	public const int OBJECTS_GR = 20;

	public const int OPBTN_GR = 21;

	public const int OPTB_GR = 22;

	public const int OPTBTNS_GR = 23;

	public const int PANELS_GR = 24;

	public const int POWER_GR = 25;

	public const int QUEST_GR = 26;

	public const int SCRLEDGE_GR = 27;

	public const int SPELLS_GR = 28;

	public const int TMFLAT_GR = 29;

	public const int TMOBJ_GR = 30;

	public const int WEAPONS_GR = 31;

	public const int GEMPT_GR = 32;

	public const int GHED_GR = 33;

	private string[] pathGR = new string[34]
	{
		"DATA--3DWIN.GR", "DATA--ANIMO.GR", "DATA--ARMOR_F.GR", "DATA--ARMOR_M.GR", "DATA--BODIES.GR", "DATA--BUTTONS.GR", "DATA--CHAINS.GR", "DATA--CHARHEAD.GR", "DATA--CHRBTNS.GR", "DATA--COMPASS.GR",
		"DATA--CONVERSE.GR", "DATA--CURSORS.GR", "DATA--DOORS.GR", "DATA--DRAGONS.GR", "DATA--EYES.GR", "DATA--FLASKS.GR", "DATA--GENHEAD.GR", "DATA--HEADS.GR", "DATA--INV.GR", "DATA--LFTI.GR",
		"DATA--OBJECTS.GR", "DATA--OPBTN.GR", "DATA--OPTB.GR", "DATA--OPTBTNS.GR", "DATA--PANELS.GR", "DATA--POWER.GR", "DATA--QUEST.GR", "DATA--SCRLEDGE.GR", "DATA--SPELLS.GR", "DATA--TMFLAT.GR",
		"DATA--TMOBJ.GR", "DATA--WEAPONS.GR", "DATA--GEMPT.GR", "DATA--GHED.GR"
	};

	private string AuxPalPath = "DATA--ALLPALS.DAT";

	private bool useOverrideAuxPalIndex = false;

	private int OverrideAuxPalIndex = 0;

	public int FileToLoad;

	private bool ImageFileDataLoaded;

	private int NoOfImages;

	protected Texture2D[] ImageCache = new Texture2D[1];

	public GRLoader(int File, int PalToUse)
	{
		AuxPalPath = AuxPalPath.Replace("--", UWClass.sep.ToString());
		useOverrideAuxPalIndex = false;
		OverrideAuxPalIndex = 0;
		FileToLoad = File;
		PaletteNo = (short)PalToUse;
		LoadImageFile();
	}

	public GRLoader(int File)
	{
		AuxPalPath = AuxPalPath.Replace("--", UWClass.sep.ToString());
		useOverrideAuxPalIndex = false;
		OverrideAuxPalIndex = 0;
		FileToLoad = File;
		PaletteNo = 0;
		LoadImageFile();
	}

	public GRLoader(string FileName, int ChunkNo)
	{
		useOverrideAuxPalIndex = false;
		OverrideAuxPalIndex = 0;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + FileName, out ImageFileData))
		{
			Debug.Log("Unable to load " + Loader.BasePath + pathGR[FileToLoad]);
			return;
		}
		DataLoader.Chunk data_ark;
		DataLoader.LoadChunk(ImageFileData, ChunkNo, out data_ark);
		int chunkContentType = data_ark.chunkContentType;
		if (chunkContentType == 3 || (chunkContentType != 2 && chunkContentType != 17))
		{
			return;
		}
		NoOfImages = (int)DataLoader.getValAtAddress(data_ark.data, 0L, 16);
		ImageCache = new Texture2D[NoOfImages];
		ImageFileDataLoaded = true;
		for (int i = 0; i < NoOfImages; i++)
		{
			long num = (int)DataLoader.getValAtAddress(data_ark.data, 2 + i * 4, 32);
			int num2 = (int)DataLoader.getValAtAddress(data_ark.data, num + 4, 16);
			int num3 = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 16);
			int num4 = (int)DataLoader.getValAtAddress(data_ark.data, num + 10, 16);
			if (num3 > 0 && num4 > 0)
			{
				if (num2 == 4)
				{
					char[] outbits;
					UncompressBitmap(data_ark.data, num + 28, out outbits, num4 * num3);
					ImageCache[i] = ArtLoader.Image(outbits, 0L, num3, num4, "namehere", GameWorldController.instance.palLoader.Palettes[PaletteNo], true, xfer);
				}
				else
				{
					ImageCache[i] = ArtLoader.Image(data_ark.data, num + 28, num3, num4, "namehere", GameWorldController.instance.palLoader.Palettes[PaletteNo], true, xfer);
				}
			}
		}
	}

	public override bool LoadImageFile()
	{
		string text = Loader.BasePath + pathGR[FileToLoad].Replace("--", UWClass.sep.ToString()).Replace(".", "_");
		if (Directory.Exists(text))
		{
			LoadMod = true;
		}
		if (!DataLoader.ReadStreamFile(Loader.BasePath + pathGR[FileToLoad].Replace("--", UWClass.sep.ToString()), out ImageFileData))
		{
			Debug.Log("Unable to load " + Loader.BasePath + pathGR[FileToLoad].Replace("--", UWClass.sep.ToString()));
			return false;
		}
		NoOfImages = (int)DataLoader.getValAtAddress(ImageFileData, 1L, 16);
		ImageCache = new Texture2D[NoOfImages];
		if (LoadMod)
		{
			for (int i = 0; i <= ImageCache.GetUpperBound(0); i++)
			{
				if (File.Exists(text + UWClass.sep + i.ToString("d3") + ".tga"))
				{
					ImageCache[i] = TGALoader.LoadTGA(text + UWClass.sep + i.ToString("d3") + ".tga");
				}
			}
		}
		ImageFileDataLoaded = true;
		return true;
	}

	public override Texture2D LoadImageAt(int index)
	{
		return LoadImageAt(index, true);
	}

	public override Texture2D LoadImageAt(int index, bool Alpha)
	{
		if (!ImageFileDataLoaded)
		{
			if (!LoadImageFile())
			{
				return base.LoadImageAt(index);
			}
		}
		else if (ImageCache[index] != null)
		{
			return ImageCache[index];
		}
		long valAtAddress = DataLoader.getValAtAddress(ImageFileData, index * 4 + 3, 32);
		if (valAtAddress >= ImageFileData.GetUpperBound(0))
		{
			return base.LoadImageAt(index);
		}
		int num = (int)DataLoader.getValAtAddress(ImageFileData, valAtAddress + 1, 8);
		int num2 = (int)DataLoader.getValAtAddress(ImageFileData, valAtAddress + 2, 8);
		long valAtAddress2 = DataLoader.getValAtAddress(ImageFileData, valAtAddress, 8);
		if (valAtAddress2 >= 8 && valAtAddress2 <= 10)
		{
			switch (valAtAddress2 - 8)
			{
			case 0L:
			{
				int auxPalIndex = (int)(useOverrideAuxPalIndex ? OverrideAuxPalIndex : DataLoader.getValAtAddress(ImageFileData, valAtAddress + 3, 8));
				int num3 = (int)DataLoader.getValAtAddress(ImageFileData, valAtAddress + 4, 16);
				char[] OutputData = new char[Mathf.Max(num * num2 * 2, (num3 + 5) * 2)];
				valAtAddress += 6;
				copyNibbles(ImageFileData, ref OutputData, num3, valAtAddress);
				int[] auxpal = PaletteLoader.LoadAuxilaryPalIndices(Loader.BasePath + AuxPalPath, auxPalIndex);
				char[] databuffer = DecodeRLEBitmap(OutputData, num3, num, num2, 4, auxpal);
				ImageCache[index] = ArtLoader.Image(databuffer, 0L, num, num2, "name_goes_here", GameWorldController.instance.palLoader.Palettes[PaletteNo], Alpha, xfer);
				return ImageCache[index];
			}
			case 2L:
			{
				int auxPalIndex = (int)(useOverrideAuxPalIndex ? OverrideAuxPalIndex : DataLoader.getValAtAddress(ImageFileData, valAtAddress + 3, 8));
				int num3 = (int)DataLoader.getValAtAddress(ImageFileData, valAtAddress + 4, 16);
				char[] OutputData = new char[Mathf.Max(num * num2 * 2, (5 + num3) * 2)];
				valAtAddress += 6;
				copyNibbles(ImageFileData, ref OutputData, num3, valAtAddress);
				Palette pal = PaletteLoader.LoadAuxilaryPal(Loader.BasePath + AuxPalPath, GameWorldController.instance.palLoader.Palettes[PaletteNo], auxPalIndex);
				ImageCache[index] = ArtLoader.Image(OutputData, 0L, num, num2, "name_goes_here", pal, Alpha, xfer);
				return ImageCache[index];
			}
			}
		}
		if (valAtAddress2 == 4)
		{
			valAtAddress += 5;
			ImageCache[index] = ArtLoader.Image(ImageFileData, valAtAddress, num, num2, "name_goes_here", GameWorldController.instance.palLoader.Palettes[PaletteNo], Alpha, xfer);
			return ImageCache[index];
		}
		if (pathGR[FileToLoad].ToUpper().EndsWith("PANELS.GR"))
		{
			num = 83;
			num2 = 114;
			if (UWClass._RES == "UW2")
			{
				num = 79;
				num2 = 112;
			}
			valAtAddress = DataLoader.getValAtAddress(ImageFileData, index * 4 + 3, 32);
			ImageCache[index] = ArtLoader.Image(ImageFileData, valAtAddress, num, num2, "name_goes_here", GameWorldController.instance.palLoader.Palettes[PaletteNo], Alpha, xfer);
			return ImageCache[index];
		}
		return new Texture2D(2, 2);
	}

	protected void copyNibbles(char[] InputData, ref char[] OutputData, int NoOfNibbles, long add_ptr)
	{
		int num = 0;
		for (NoOfNibbles *= 2; NoOfNibbles > 1; NoOfNibbles -= 2)
		{
			if (add_ptr <= InputData.GetUpperBound(0))
			{
				OutputData[num] = (char)((DataLoader.getValAtAddress(InputData, add_ptr, 8) >> 4) & 0xF);
				OutputData[num + 1] = (char)(DataLoader.getValAtAddress(InputData, add_ptr, 8) & 0xF);
			}
			num += 2;
			add_ptr++;
		}
		if (NoOfNibbles == 1)
		{
			OutputData[num] = (char)((DataLoader.getValAtAddress(InputData, add_ptr, 8) >> 4) & 0xF);
		}
	}

	private char[] DecodeRLEBitmap(char[] imageData, int datalen, int imageWidth, int imageHeight, int BitSize, int[] auxpal)
	{
		char[] array = new char[imageWidth * imageHeight];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int addr_ptr = 0;
		while (num2 < imageWidth * imageHeight || addr_ptr <= datalen)
		{
			switch (num)
			{
			case 0:
				num3 = getcount(imageData, ref addr_ptr, BitSize);
				switch (num3)
				{
				case 1:
					num = 2;
					break;
				case 2:
					num4 = getcount(imageData, ref addr_ptr, BitSize) - 1;
					num = 0;
					break;
				default:
					num = 1;
					break;
				}
				break;
			case 1:
			{
				char nibble = getNibble(imageData, ref addr_ptr);
				if (imageWidth * imageHeight - num2 < num3)
				{
					num3 = imageWidth * imageHeight - num2;
				}
				for (int j = 0; j < num3; j++)
				{
					array[num2++] = (char)auxpal[(uint)nibble];
				}
				if (num4 == 0)
				{
					num = 2;
					break;
				}
				num = 0;
				num4--;
				break;
			}
			case 2:
			{
				num3 = getcount(imageData, ref addr_ptr, BitSize);
				if (imageWidth * imageHeight - num2 < num3)
				{
					num3 = imageWidth * imageHeight - num2;
				}
				for (int i = 0; i < num3; i++)
				{
					char nibble = getNibble(imageData, ref addr_ptr);
					array[num2++] = (char)auxpal[(uint)nibble];
				}
				num = 0;
				break;
			}
			}
		}
		return array;
	}

	private int getcount(char[] nibbles, ref int addr_ptr, int size)
	{
		int num = 0;
		int nibble = getNibble(nibbles, ref addr_ptr);
		num = nibble;
		if (num == 0)
		{
			nibble = getNibble(nibbles, ref addr_ptr);
			int nibble2 = getNibble(nibbles, ref addr_ptr);
			num = (nibble << size) | nibble2;
		}
		if (num == 0)
		{
			nibble = getNibble(nibbles, ref addr_ptr);
			int nibble2 = getNibble(nibbles, ref addr_ptr);
			int nibble3 = getNibble(nibbles, ref addr_ptr);
			num = (((nibble << size) | nibble2) << size) | nibble3;
		}
		return num;
	}

	private char getNibble(char[] nibbles, ref int addr_ptr)
	{
		char result = nibbles[addr_ptr];
		addr_ptr++;
		return result;
	}

	public Sprite RequestSprite(int index)
	{
		if (ImageCache[index] == null)
		{
			LoadImageAt(index);
			if (ImageCache[index] == null)
			{
				return Resources.Load<Sprite>("Common/null");
			}
		}
		return Sprite.Create(ImageCache[index], new Rect(0f, 0f, ImageCache[index].width, ImageCache[index].height), new Vector2(0.5f, 0f));
	}

	public Sprite RequestSprite(int index, int offset)
	{
		if (ImageCache[index] == null)
		{
			LoadImageAt(index);
			if (ImageCache[index] == null)
			{
				return Resources.Load<Sprite>("Common/null");
			}
		}
		return Sprite.Create(ImageCache[index], new Rect(0f, 0f, ImageCache[index].width, ImageCache[index].height), new Vector2(0.5f, 0f));
	}

	public int NoOfFileImages()
	{
		return ImageCache.Length;
	}
}
