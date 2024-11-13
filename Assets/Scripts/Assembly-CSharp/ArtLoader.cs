using UnityEngine;

public class ArtLoader : Loader
{
	protected bool LoadMod;

	public bool xfer;

	public const byte BitMapHeaderSize = 28;

	protected char[] ImageFileData;

	public short PaletteNo = 0;

	public virtual bool LoadImageFile()
	{
		if (DataLoader.ReadStreamFile(Loader.BasePath + Path.Replace("--", UWClass.sep.ToString()), out ImageFileData))
		{
			DataLoaded = true;
		}
		else
		{
			DataLoaded = false;
		}
		return DataLoaded;
	}

	public virtual Texture2D LoadImageAt(int index)
	{
		return new Texture2D(1, 1);
	}

	public virtual Texture2D LoadImageAt(int index, bool Alpha)
	{
		return new Texture2D(1, 1);
	}

	public static Texture2D Image(char[] databuffer, long dataOffSet, int width, int height, string imageName, Palette pal, bool Alpha)
	{
		return Image(databuffer, dataOffSet, width, height, imageName, pal, Alpha, false);
	}

	public static Texture2D Image(char[] databuffer, long dataOffSet, int width, int height, string imageName, Palette pal, bool Alpha, bool useXFER)
	{
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
		Color32[] array = new Color32[width * height];
		long num = 0L;
		for (int num2 = height - 1; num2 >= 0; num2--)
		{
			for (int i = num2 * width; i < num2 * width + width; i++)
			{
				byte b = (byte)DataLoader.getValAtAddress(databuffer, dataOffSet + i, 8);
				if (useXFER)
				{
					switch (b)
					{
					case 240:
					case 249:
						array[num++] = new Color32(252, 56, 76, 40);
						break;
					case 244:
						array[num++] = new Color32(92, 92, 252, 40);
						break;
					case 248:
						array[num++] = new Color32(96, 172, 84, 40);
						break;
					case 251:
						array[num++] = new Color32(4, 4, 4, 40);
						break;
					case 252:
						array[num++] = new Color32(204, 204, 220, 40);
						break;
					case 253:
						array[num++] = new Color32(4, 4, 4, 40);
						break;
					case 0:
						array[num++] = pal.ColorAtPixel(b, Alpha);
						break;
					default:
						array[num++] = pal.ColorAtPixel(b, Alpha);
						break;
					}
				}
				else
				{
					array[num++] = pal.ColorAtPixel(b, Alpha);
				}
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixels32(array);
		texture2D.Apply();
		return texture2D;
	}

	public static void ua_image_decode_rle(char[] FileIn, char[] pixels, int bits, int datalen, int maxpix, int addr_ptr, char[] auxpal)
	{
		int num = 0;
		int num2 = 0;
		int num3 = (1 << bits) - 1 << 8 - bits;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		while (datalen > 0 && num4 < maxpix)
		{
			int num9;
			if (num < bits)
			{
				if (num > 0)
				{
					num9 = (num2 & num3) >> 8 - num;
					num9 <<= bits - num;
				}
				else
				{
					num9 = 0;
				}
				num2 = (int)DataLoader.getValAtAddress(FileIn, addr_ptr, 8);
				addr_ptr++;
				if (num2 == -1)
				{
					break;
				}
				int num10 = 8 - (bits - num);
				num9 |= num2 >> num10;
				num2 = (num2 << 8 - num10) & 0xFF;
				num = num10;
			}
			else
			{
				num9 = (num2 & num3) >> 8 - bits;
				num -= bits;
				num2 <<= bits;
			}
			datalen--;
			switch (num5)
			{
			case 0:
				if (num9 == 0)
				{
					num5++;
					break;
				}
				num6 = num9;
				num5 = 6;
				break;
			case 1:
				num6 = num9;
				num5++;
				break;
			case 2:
				num6 = (num6 << 4) | num9;
				num5 = ((num6 != 0) ? 6 : (num5 + 1));
				break;
			case 3:
			case 4:
			case 5:
				num6 = (num6 << 4) | num9;
				num5++;
				break;
			}
			if (num5 < 6)
			{
				continue;
			}
			switch (num7)
			{
			case 0:
				switch (num6)
				{
				case 1:
					num7 = 3;
					break;
				case 2:
					num7 = 2;
					break;
				default:
					num7 = 1;
					continue;
				}
				break;
			case 1:
			{
				for (int i = 0; i < num6; i++)
				{
					pixels[num4++] = auxpal[num9];
					if (num4 >= maxpix)
					{
						break;
					}
				}
				if (num8 == 0)
				{
					num7 = 3;
					break;
				}
				num8--;
				num7 = 0;
				break;
			}
			case 2:
				num8 = num6 - 1;
				num7 = 0;
				break;
			case 3:
				num7 = 4;
				continue;
			case 4:
				pixels[num4++] = auxpal[num9];
				if (--num6 == 0)
				{
					num7 = 0;
					break;
				}
				continue;
			}
			num5 = 0;
		}
	}

	private static char getActualAuxPalVal(char[] auxpal, int nibble)
	{
		switch (auxpal[nibble])
		{
		case 'ð':
			return (char)(384 + nibble);
		case 'ô':
			return (char)(640 + nibble);
		case 'ø':
			return (char)(768 + nibble);
		case 'ü':
			return (char)(896 + nibble);
		default:
			return auxpal[nibble];
		}
	}

	public void UncompressBitmap(char[] chunk_bits, long chunk_ptr, out char[] outbits, int numbits)
	{
		int num = 0;
		outbits = new char[numbits];
		while (num < numbits)
		{
			int num2 = chunk_bits[chunk_ptr++];
			if (num2 == 0)
			{
				num2 = chunk_bits[chunk_ptr++];
				for (int i = 0; i < num2; i++)
				{
					if (num >= numbits)
					{
						break;
					}
					outbits[num++] = chunk_bits[chunk_ptr];
				}
				chunk_ptr++;
			}
			else if (num2 < 129)
			{
				if (num2 == 128)
				{
					num2 = chunk_bits[chunk_ptr++];
					if (num2 == 0)
					{
						break;
					}
					if (chunk_bits[chunk_ptr] < '\u0080')
					{
						num += num2 + (int)((uint)chunk_bits[chunk_ptr] << 8);
						num2 = 0;
					}
					chunk_ptr++;
				}
				for (int i = 0; i < num2; i++)
				{
					if (num >= numbits)
					{
						break;
					}
					outbits[num++] = chunk_bits[chunk_ptr++];
				}
			}
			else
			{
				num += num2 & 0x7F;
			}
		}
	}

	public static Texture2D CreateBlankImage(int ImgWidth, int ImgHeight)
	{
		Texture2D texture2D = new Texture2D(ImgWidth, ImgHeight, TextureFormat.ARGB32, false);
		Color32[] array = new Color32[ImgWidth * ImgHeight];
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			array[i] = GameWorldController.instance.palLoader.Palettes[0].ColorAtPixel(0, true);
		}
		texture2D.SetPixels32(array);
		texture2D.Apply();
		return texture2D;
	}

	public static Texture2D InsertImage(Texture2D srcImg, Texture2D dstImg, int CornerX, int CornerY)
	{
		Texture2D texture2D = new Texture2D(dstImg.width, dstImg.height, TextureFormat.ARGB32, false);
		texture2D.SetPixels32(dstImg.GetPixels32());
		for (int i = 0; i < srcImg.width; i++)
		{
			for (int j = 0; j < srcImg.height; j++)
			{
				if (i + CornerX < dstImg.width && j + CornerY < dstImg.height && CornerX + i >= 0 && CornerY + j >= 0)
				{
					texture2D.SetPixel(CornerX + i, CornerY + j, srcImg.GetPixel(i, j));
				}
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}
}
