using System;
using System.IO;
using UnityEngine;

public static class TGALoader
{
	public static Texture2D LoadTGA(string fileName)
	{
		if (File.Exists(fileName))
		{
			using (FileStream tGAStream = File.OpenRead(fileName))
			{
				return LoadTGA(tGAStream);
			}
		}
		return null;
	}

	public static Texture2D LoadTGA(Stream TGAStream)
	{
		using (BinaryReader binaryReader = new BinaryReader(TGAStream))
		{
			binaryReader.BaseStream.Seek(12L, SeekOrigin.Begin);
			short num = binaryReader.ReadInt16();
			short num2 = binaryReader.ReadInt16();
			int num3 = binaryReader.ReadByte();
			binaryReader.BaseStream.Seek(1L, SeekOrigin.Current);
			Texture2D texture2D = new Texture2D(num, num2);
			Color32[] array = new Color32[num * num2];
			switch (num3)
			{
			case 32:
			{
				for (int j = 0; j < num * num2; j++)
				{
					byte b2 = binaryReader.ReadByte();
					byte g2 = binaryReader.ReadByte();
					byte r2 = binaryReader.ReadByte();
					byte a = binaryReader.ReadByte();
					array[j] = new Color32(r2, g2, b2, a);
				}
				break;
			}
			case 24:
			{
				for (int i = 0; i < num * num2; i++)
				{
					byte b = binaryReader.ReadByte();
					byte g = binaryReader.ReadByte();
					byte r = binaryReader.ReadByte();
					array[i] = new Color32(r, g, b, 1);
				}
				break;
			}
			default:
				throw new Exception("TGA texture has non 32/24 bit depth.");
			}
			texture2D.filterMode = FilterMode.Point;
			texture2D.SetPixels32(array);
			texture2D.Apply();
			return texture2D;
		}
	}
}
