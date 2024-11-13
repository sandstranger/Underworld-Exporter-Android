using UnityEngine;

public class Palette : UWClass
{
	public byte[] red = new byte[256];

	public byte[] blue = new byte[256];

	public byte[] green = new byte[256];

	public Color32 ColorAtPixel(byte pixel, bool Alpha)
	{
		byte a = (byte)(Alpha ? ((pixel != 0) ? byte.MaxValue : 0) : 0);
		return new Color32(red[pixel], green[pixel], blue[pixel], a);
	}

	public Color32 ColorAtPixelAlpha(byte pixel, byte alpha)
	{
		return new Color32(red[pixel], green[pixel], blue[pixel], alpha);
	}

	public static Texture2D toImage(Palette pal)
	{
		if (pal == null)
		{
			Debug.Log("Null Palette in cyclePalette");
			return null;
		}
		int num = 1;
		int num2 = 256;
		Texture2D texture2D = new Texture2D(num2, num, TextureFormat.ARGB32, false);
		Color32[] array = new Color32[256];
		long num3 = 0L;
		byte a = 0;
		for (int num4 = num - 1; num4 >= 0; num4--)
		{
			for (int i = num4 * num2; i < num4 * num2 + num2; i++)
			{
				if (num3 != 0)
				{
					a = byte.MaxValue;
				}
				array[num3] = new Color32(pal.red[num3], pal.green[num3], pal.blue[num3], a);
				num3++;
			}
		}
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixels32(array);
		texture2D.Apply();
		return texture2D;
	}

	public static void cyclePalette(Palette pal, int Start, int length)
	{
		byte b = pal.red[Start];
		byte b2 = pal.green[Start];
		byte b3 = pal.blue[Start];
		for (int i = Start; i < Start + length - 1; i++)
		{
			pal.red[i] = pal.red[i + 1];
			pal.green[i] = pal.green[i + 1];
			pal.blue[i] = pal.blue[i + 1];
		}
		pal.red[Start + length - 1] = b;
		pal.green[Start + length - 1] = b2;
		pal.blue[Start + length - 1] = b3;
	}

	public static void cyclePaletteReverse(Palette pal, int Start, int length)
	{
		byte b = pal.red[Start];
		byte b2 = pal.green[Start];
		byte b3 = pal.blue[Start];
		for (int i = Start; i < Start + length; i++)
		{
			byte b4 = pal.red[i + 1];
			byte b5 = pal.green[i + 1];
			byte b6 = pal.blue[i + 1];
			pal.red[i + 1] = b;
			pal.green[i + 1] = b2;
			pal.blue[i + 1] = b3;
			b = b4;
			b2 = b5;
			b3 = b6;
		}
		pal.red[Start] = b;
		pal.green[Start] = b2;
		pal.blue[Start] = b3;
	}
}
