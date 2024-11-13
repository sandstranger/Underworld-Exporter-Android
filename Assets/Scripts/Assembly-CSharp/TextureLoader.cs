using System.IO;
using UnityEngine;

public class TextureLoader : ArtLoader
{
	private string pathTexW_UW0 = "DATA--DW64.TR";

	private string pathTexF_UW0 = "DATA--DF32.TR";

	private string pathTexW_UW1 = "DATA--W64.TR";

	private string pathTexF_UW1 = "DATA--F32.TR";

	private string pathTex_UW2 = "DATA--T64.TR";

	private string pathTex_SS1 = "RES--DATA--Texture.res";

	private char[] texturebufferW;

	private char[] texturebufferF;

	private char[] texturebufferT;

	public bool texturesWLoaded;

	public bool texturesFLoaded;

	private int TextureSplit = 210;

	private int FloorDim = 32;

	private string ModPathW;

	private string ModPathF;

	public const float BumpMapStrength = 1f;

	public TextureLoader()
	{
		switch (UWClass._RES)
		{
		case "SHOCK":
			break;
		case "UW2":
			ModPathW = Loader.BasePath + pathTex_UW2.Replace(".", "_").Replace("--", UWClass.sep.ToString());
			if (Directory.Exists(ModPathW))
			{
				LoadMod = true;
			}
			break;
		case "UW0":
			ModPathW = Loader.BasePath + pathTexW_UW0.Replace(".", "_").Replace("--", UWClass.sep.ToString());
			if (Directory.Exists(ModPathW))
			{
				LoadMod = true;
			}
			ModPathF = Loader.BasePath + pathTexF_UW0.Replace(".", "_").Replace("--", UWClass.sep.ToString());
			if (Directory.Exists(ModPathF))
			{
				LoadMod = true;
			}
			break;
		case "UW1":
			ModPathW = Loader.BasePath + pathTexW_UW1.Replace(".", "_").Replace("--", UWClass.sep.ToString());
			if (Directory.Exists(ModPathW))
			{
				LoadMod = true;
			}
			ModPathF = Loader.BasePath + pathTexF_UW1.Replace(".", "_").Replace("--", UWClass.sep.ToString());
			if (Directory.Exists(ModPathF))
			{
				LoadMod = true;
			}
			break;
		}
	}

	public Texture2D LoadImageAt(int index, short TextureType)
	{
		switch (TextureType)
		{
		case 1:
			return LoadImageAt(index, GameWorldController.instance.palLoader.GreyScale);
		case 2:
			return TGALoader.LoadTGA(ModPathW + UWClass.sep + index.ToString("d3") + "_normal.tga");
		default:
			return LoadImageAt(index, GameWorldController.instance.palLoader.Palettes[0]);
		}
	}

	public override Texture2D LoadImageAt(int index)
	{
		return LoadImageAt(index, GameWorldController.instance.palLoader.Palettes[0]);
	}

	public Texture2D LoadImageAt(int index, Palette palToUse)
	{
		if (UWClass._RES == "UW0")
		{
			TextureSplit = 48;
			pathTexW_UW1 = pathTexW_UW0;
			pathTexF_UW1 = pathTexF_UW0;
		}
		if (UWClass._RES == "UW2")
		{
			FloorDim = 64;
		}
		switch (UWClass._RES)
		{
		case "SHOCK":
		{
			if (!texturesFLoaded)
			{
				if (!DataLoader.ReadStreamFile(Loader.BasePath + pathTex_SS1, out texturebufferT))
				{
					return base.LoadImageAt(index);
				}
				texturesFLoaded = true;
			}
			DataLoader.Chunk data_ark;
			if (DataLoader.LoadChunk(texturebufferT, index + 1000, out data_ark))
			{
				int chunkContentType = data_ark.chunkContentType;
				if (chunkContentType == 2 || chunkContentType == 17)
				{
					long num = (int)DataLoader.getValAtAddress(data_ark.data, 2L, 32);
					int num2 = (int)DataLoader.getValAtAddress(data_ark.data, num + 4, 16);
					int num3 = (int)DataLoader.getValAtAddress(data_ark.data, num + 8, 16);
					int num4 = (int)DataLoader.getValAtAddress(data_ark.data, num + 10, 16);
					if (num3 > 0 && num4 > 0)
					{
						if (num2 == 4)
						{
							char[] outbits;
							UncompressBitmap(data_ark.data, num + 28, out outbits, num4 * num3);
							return ArtLoader.Image(outbits, 0L, num3, num4, "namehere", palToUse, true);
						}
						return ArtLoader.Image(data_ark.data, num + 28, num3, num4, "namehere", palToUse, true);
					}
					return base.LoadImageAt(index);
				}
			}
			return base.LoadImageAt(index);
		}
		case "UW2":
		{
			if (LoadMod && File.Exists(ModPathW + UWClass.sep + index.ToString("d3") + ".tga"))
			{
				return TGALoader.LoadTGA(ModPathW + UWClass.sep + index.ToString("d3") + ".tga");
			}
			if (!texturesFLoaded)
			{
				if (!DataLoader.ReadStreamFile(Loader.BasePath + pathTex_UW2, out texturebufferT))
				{
					return base.LoadImageAt(index);
				}
				texturesFLoaded = true;
			}
			long valAtAddress3 = DataLoader.getValAtAddress(texturebufferT, index * 4 + 4, 32);
			return ArtLoader.Image(texturebufferT, valAtAddress3, FloorDim, FloorDim, "name_goes_here", palToUse, false);
		}
		default:
		{
			if (index < TextureSplit)
			{
				if (!texturesWLoaded)
				{
					if (!DataLoader.ReadStreamFile(Loader.BasePath + pathTexW_UW1, out texturebufferW))
					{
						return base.LoadImageAt(index);
					}
					texturesWLoaded = true;
				}
				if (LoadMod && File.Exists(ModPathW + UWClass.sep + index.ToString("d3") + ".tga"))
				{
					return TGALoader.LoadTGA(ModPathW + UWClass.sep + index.ToString("d3") + ".tga");
				}
				long valAtAddress = DataLoader.getValAtAddress(texturebufferW, index * 4 + 4, 32);
				return ArtLoader.Image(texturebufferW, valAtAddress, 64, 64, "name_goes_here", palToUse, false);
			}
			if (!texturesFLoaded)
			{
				if (!DataLoader.ReadStreamFile(Loader.BasePath + pathTexF_UW1, out texturebufferF))
				{
					return base.LoadImageAt(index);
				}
				texturesFLoaded = true;
			}
			if (LoadMod && File.Exists(ModPathF + UWClass.sep + index.ToString("d3") + ".tga"))
			{
				return TGALoader.LoadTGA(ModPathF + UWClass.sep + index.ToString("d3") + ".tga");
			}
			long valAtAddress2 = DataLoader.getValAtAddress(texturebufferF, (index - TextureSplit) * 4 + 4, 32);
			return ArtLoader.Image(texturebufferF, valAtAddress2, FloorDim, FloorDim, "name_goes_here", palToUse, false);
		}
		}
	}

	public static Texture2D NormalMap(Texture2D source, float strength)
	{
		strength = Mathf.Clamp(strength, 0f, 10f);
		Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
		for (int i = 0; i < texture2D.height; i++)
		{
			for (int j = 0; j < texture2D.width; j++)
			{
				float num = source.GetPixel(j - 1, i).grayscale * strength;
				float num2 = source.GetPixel(j + 1, i).grayscale * strength;
				float num3 = source.GetPixel(j, i - 1).grayscale * strength;
				float num4 = source.GetPixel(j, i + 1).grayscale * strength;
				float r = (num - num2 + 1f) * 0.5f;
				float num5 = (num3 - num4 + 1f) * 0.5f;
				texture2D.SetPixel(j, i, new Color(r, num5, 1f, num5));
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	public string ModPath(int index)
	{
		switch (UWClass._RES)
		{
		case "SHOCK":
			return "";
		case "UW2":
			return ModPathW + UWClass.sep + index.ToString("d3") + ".tga";
		default:
			if (index < TextureSplit)
			{
				return ModPathW + UWClass.sep + index.ToString("d3") + ".tga";
			}
			return ModPathF + UWClass.sep + index.ToString("d3") + ".tga";
		}
	}
}
