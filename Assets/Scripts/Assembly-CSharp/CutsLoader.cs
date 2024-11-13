using UnityEngine;

public class CutsLoader : ArtLoader
{
	private struct lpHeader
	{
		public int NoOfPages;

		public int NoOfRecords;

		public int width;

		public int height;

		public int nFrames;
	}

	private struct lp_descriptor
	{
		public int baseRecord;

		public int nRecords;

		public int nBytes;
	}

	public Texture2D[] ImageCache = new Texture2D[1];

	private char[] dstImage;

	public CutsLoader(string File)
	{
		Path = "CUTS" + UWClass.sep + File.ToUpper();
		if (LoadImageFile())
		{
			ReadCutsFile(ref ImageFileData, UseAlpha(File), UseErrorHandling(File));
		}
	}

	private bool UseAlpha(string File)
	{
		switch (File)
		{
		case "cs401.n01":
		case "cs402.n01":
		case "cs403.n01":
		case "cs403.n02":
		case "cs404.n01":
		case "cs410.n01":
			return true;
		default:
			return false;
		}
	}

	private bool UseErrorHandling(string File)
	{
		if (File != null && File == "cs000.n23")
		{
			return true;
		}
		return false;
	}

	public void ReadCutsFile(ref char[] cutsFile, bool Alpha, bool ErrorHandling)
	{
		long num = 0L;
		int num2 = 0;
		Palette palette = new Palette();
		lpHeader lpHeader = default(lpHeader);
		lpHeader.NoOfPages = (int)DataLoader.getValAtAddress(cutsFile, 6L, 16);
		lpHeader.NoOfRecords = (int)DataLoader.getValAtAddress(cutsFile, 8L, 32);
		lpHeader.width = (int)DataLoader.getValAtAddress(cutsFile, 20L, 16);
		lpHeader.height = (int)DataLoader.getValAtAddress(cutsFile, 22L, 16);
		lpHeader.nFrames = (int)DataLoader.getValAtAddress(cutsFile, 64L, 16);
		num += 128;
		num += 128;
		dstImage = new char[lpHeader.height * lpHeader.width + 4000];
		for (int i = 0; i < 256; i++)
		{
			palette.blue[i] = (byte)DataLoader.getValAtAddress(cutsFile, num++, 8);
			palette.green[i] = (byte)DataLoader.getValAtAddress(cutsFile, num++, 8);
			palette.red[i] = (byte)DataLoader.getValAtAddress(cutsFile, num++, 8);
			num++;
		}
		lp_descriptor[] array = new lp_descriptor[256];
		for (int j = 0; j < array.GetUpperBound(0); j++)
		{
			array[j].baseRecord = (int)DataLoader.getValAtAddress(cutsFile, num, 16);
			array[j].nRecords = (int)DataLoader.getValAtAddress(cutsFile, num + 2, 16);
			array[j].nBytes = (int)DataLoader.getValAtAddress(cutsFile, num + 4, 16);
			num += 6;
		}
		char[] array2 = new char[cutsFile.GetUpperBound(0) - 2816 + 1];
		for (int k = 0; k <= array2.GetUpperBound(0); k++)
		{
			array2[k] = cutsFile[k + 2816];
		}
		ImageCache = new Texture2D[lpHeader.nFrames];
		lp_descriptor lp_descriptor = default(lp_descriptor);
		for (int l = 0; l < lpHeader.nFrames && (!ErrorHandling || l != 10); l++)
		{
			int m;
			for (m = 0; m < lpHeader.NoOfPages && (array[m].baseRecord > l || array[m].baseRecord + array[m].nRecords <= l); m++)
			{
			}
			num = 65536 * m;
			long num3 = num;
			lp_descriptor.baseRecord = (int)DataLoader.getValAtAddress(array2, num3, 16);
			lp_descriptor.nRecords = (int)DataLoader.getValAtAddress(array2, num3 + 2, 16);
			lp_descriptor.nBytes = (int)DataLoader.getValAtAddress(array2, num3 + 4, 16);
			long num4 = num3 + 6 + 2;
			int num5 = l - lp_descriptor.baseRecord;
			int num6 = 0;
			long num7 = num4;
			for (int n = 0; n < num5; n++)
			{
				num6 += (int)DataLoader.getValAtAddress(array2, num7 + n * 2, 16);
			}
			long num8 = num4 + lp_descriptor.nRecords * 2 + num6;
			num8 = ((cutsFile[num8 + 1] != 0) ? (num8 + 4) : (num8 + (4 + (cutsFile[num8 + 1] + (cutsFile[num8 + 1] & 1)))));
			myPlayRunSkipDump(num8, array2);
			ImageCache[num2++] = ArtLoader.Image(dstImage, 0L, lpHeader.width, lpHeader.height, "name here", palette, Alpha);
		}
	}

	private void myPlayRunSkipDump(long inptr, char[] srcData)
	{
		long num = 0L;
		while (true)
		{
			int num2 = (srcData[inptr] & 0x80) >> 7;
			num2 = ((num2 != 1) ? 1 : (-1));
			int num3 = srcData[inptr++];
			if (num3 * num2 > 0)
			{
				while (num3 > 0)
				{
					dstImage[num++] = srcData[inptr++];
					num3--;
				}
				continue;
			}
			if (num3 == 0)
			{
				int num4 = srcData[inptr++];
				char c = srcData[inptr++];
				while (num4 > 0)
				{
					dstImage[num++] = c;
					num4--;
				}
				continue;
			}
			num3 &= 0x7F;
			if (num3 != 0)
			{
				num += num3;
				continue;
			}
			int num5 = (int)DataLoader.getValAtAddress(srcData, inptr, 16);
			inptr += 2;
			int num6 = (num5 & 0x8000) >> 15;
			num6 = ((num6 != 1) ? 1 : (-1));
			if (num5 * num6 <= 0)
			{
				if (num5 == 0)
				{
					break;
				}
				num5 &= 0x7FFF;
				if (num5 >= 16384)
				{
					num5 -= 16384;
					char c2 = srcData[inptr++];
					while (num5 > 0)
					{
						dstImage[num++] = c2;
						num5--;
					}
				}
				else
				{
					while (num5 > 0)
					{
						dstImage[num++] = srcData[inptr++];
						num5--;
					}
				}
			}
			else
			{
				num += num5;
			}
		}
	}

	public Sprite RequestSprite(int index)
	{
		if (ImageCache[index] == null)
		{
			LoadImageAt(index);
		}
		return Sprite.Create(ImageCache[index], new Rect(0f, 0f, ImageCache[index].width, ImageCache[index].height), new Vector2(0.5f, 0f));
	}
}
