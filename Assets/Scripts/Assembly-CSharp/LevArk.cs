using System;
using System.IO;
using UnityEngine;

public class LevArk : Loader
{
	public static char[] lev_ark_file_data;

	public static void WriteBackLevArkUW2(int slotNo)
	{
		int num = 320;
		DataLoader.UWBlock[] array = new DataLoader.UWBlock[num];
		ObjectLoader.UpdateObjectList(UWClass.CurrentTileMap(), UWClass.CurrentObjectList());
		long num2 = 0L;
		long num3 = 0L;
		for (int i = 0; i <= GameWorldController.instance.Tilemaps.GetUpperBound(0); i++)
		{
			array[i].CompressionFlag = (int)DataLoader.getValAtAddress(lev_ark_file_data, 6 + i * 4 + num * 4, 32);
			array[i].DataLen = DataLoader.getValAtAddress(lev_ark_file_data, 6 + i * 4 + num * 8, 32);
			array[i].ReservedSpace = DataLoader.getValAtAddress(lev_ark_file_data, 6 + i * 4 + num * 12, 32);
			if (GameWorldController.instance.Tilemaps[i] != null)
			{
				long datalen = 0L;
				array[i].CompressionFlag = 0;
				array[i].Data = GameWorldController.instance.Tilemaps[i].TileMapToBytes(lev_ark_file_data, out datalen);
				array[i].DataLen = datalen;
			}
			else
			{
				num2 = DataLoader.getValAtAddress(lev_ark_file_data, i * 4 + 6, 32);
				if (num2 != 0)
				{
					array[i].Data = CopyData(num2, array[i].ReservedSpace);
				}
				else
				{
					array[i].DataLen = 0L;
				}
			}
		}
		for (int j = 0; j <= GameWorldController.instance.Tilemaps.GetUpperBound(0); j++)
		{
			num2 = DataLoader.getValAtAddress(lev_ark_file_data, j * 4 + 6 + 320, 32);
			array[j + 80].CompressionFlag = (int)DataLoader.getValAtAddress(lev_ark_file_data, j * 4 + 6 + 320 + num * 4, 32);
			array[j + 80].ReservedSpace = DataLoader.getValAtAddress(lev_ark_file_data, j * 4 + 6 + 320 + num * 12, 32);
			num3 = DataLoader.getValAtAddress(lev_ark_file_data, j * 4 + 6 + 320 + num * 8, 32);
			if (num2 != 0)
			{
				array[j + 80].Data = CopyData(num2, num3);
				array[j + 80].DataLen = array[j + 80].Data.GetUpperBound(0) + 1;
			}
			else
			{
				array[j + 80].DataLen = 0L;
			}
		}
		for (int k = 0; k <= GameWorldController.instance.Tilemaps.GetUpperBound(0); k++)
		{
			if (GameWorldController.instance.AutoMaps[k] != null)
			{
				array[k + 160].CompressionFlag = 0;
				array[k + 160].Data = GameWorldController.instance.AutoMaps[k].AutoMapVisitedToBytes();
				array[k + 160].DataLen = array[k + 160].Data.GetUpperBound(0) + 1;
				array[k + 160].ReservedSpace = array[k + 160].DataLen;
				continue;
			}
			num2 = DataLoader.getValAtAddress(lev_ark_file_data, k * 4 + 6 + 640, 32);
			array[k + 160].CompressionFlag = (int)DataLoader.getValAtAddress(lev_ark_file_data, k * 4 + 6 + 640 + num * 4, 32);
			num3 = DataLoader.getValAtAddress(lev_ark_file_data, k * 4 + 6 + 640 + num * 8, 32);
			array[k + 160].ReservedSpace = DataLoader.getValAtAddress(lev_ark_file_data, k * 4 + 6 + 640 + num * 12, 32);
			if (num2 != 0)
			{
				array[k + 160].Data = CopyData(num2, num3);
				array[k + 160].DataLen = array[k + 160].Data.GetUpperBound(0) + 1;
			}
			else
			{
				array[k + 160].DataLen = 0L;
			}
		}
		for (int l = 0; l <= GameWorldController.instance.Tilemaps.GetUpperBound(0); l++)
		{
			if (GameWorldController.instance.AutoMaps[l] != null)
			{
				array[l + 240].Data = GameWorldController.instance.AutoMaps[l].AutoMapNotesToBytes();
				if (array[l + 240].Data != null)
				{
					array[l + 240].CompressionFlag = 0;
					array[l + 240].DataLen = array[l + 240].Data.GetUpperBound(0) + 1;
					array[l + 240].ReservedSpace = array[l + 240].DataLen;
					continue;
				}
				num2 = DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960, 32);
				array[l + 240].CompressionFlag = (int)DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960 + num * 4, 32);
				num3 = DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960 + num * 8, 32);
				array[l + 240].ReservedSpace = DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960 + num * 12, 32);
				if (num2 != 0)
				{
					array[l + 240].Data = CopyData(num2, num3);
					array[l + 240].DataLen = array[l + 240].Data.GetUpperBound(0) + 1;
				}
				else
				{
					array[l + 240].DataLen = 0L;
				}
			}
			else
			{
				num2 = DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960, 32);
				array[l + 240].CompressionFlag = (int)DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960 + num * 4, 32);
				num3 = DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960 + num * 8, 32);
				array[l + 240].ReservedSpace = DataLoader.getValAtAddress(lev_ark_file_data, l * 4 + 6 + 960 + num * 12, 32);
				if (num2 != 0)
				{
					array[l + 240].Data = CopyData(num2, num3);
					array[l + 240].DataLen = array[l + 240].Data.GetUpperBound(0) + 1;
				}
				else
				{
					array[l + 240].DataLen = 0L;
				}
			}
		}
		array[0].Address = 5126L;
		long address = array[0].Address;
		long num4 = Math.Max(array[0].ReservedSpace, array[0].DataLen);
		for (int m = 1; m < array.GetUpperBound(0); m++)
		{
			if (array[m].DataLen != 0)
			{
				array[m].Address = address + num4;
				address = array[m].Address;
				num4 = Math.Max(array[m].ReservedSpace, array[m].DataLen);
			}
			else
			{
				array[m].Address = 0L;
			}
		}
		FileStream fileStream = File.Open(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "LEV.ARK", FileMode.Create);
		BinaryWriter writer = new BinaryWriter(fileStream);
		long num5 = 0L;
		num5 += DataLoader.WriteInt8(writer, 64L);
		num5 += DataLoader.WriteInt8(writer, 1L);
		num5 += DataLoader.WriteInt8(writer, 0L);
		num5 += DataLoader.WriteInt8(writer, 0L);
		num5 += DataLoader.WriteInt8(writer, 0L);
		num5 += DataLoader.WriteInt8(writer, 0L);
		for (int n = 0; n < 320; n++)
		{
			num5 += DataLoader.WriteInt32(writer, array[n].Address);
		}
		for (int num6 = 320; num6 < 640; num6++)
		{
			num5 += DataLoader.WriteInt32(writer, array[num6 - 320].CompressionFlag);
		}
		for (int num7 = 960; num7 < 1280; num7++)
		{
			num5 += DataLoader.WriteInt32(writer, array[num7 - 960].DataLen);
		}
		for (int num8 = 1280; num8 < 1600; num8++)
		{
			num5 += DataLoader.WriteInt32(writer, array[num8 - 1280].ReservedSpace);
		}
		for (long num9 = num5; num9 < array[0].Address; num9++)
		{
			num5 += DataLoader.WriteInt8(writer, 0L);
		}
		for (int num10 = 0; num10 <= array.GetUpperBound(0); num10++)
		{
			if (array[num10].Data == null)
			{
				continue;
			}
			if (num5 < array[num10].Address)
			{
				for (; num5 < array[num10].Address; num5 += DataLoader.WriteInt8(writer, 0L))
				{
				}
			}
			else if (num5 > array[num10].Address)
			{
				Debug.Log("Writing block " + num10 + " at " + num5 + " should be " + array[num10].Address);
			}
			Debug.Log("Writing block " + num10 + " datalen " + array[num10].DataLen + " ubound=" + array[num10].Data.GetUpperBound(0));
			int upperBound = array[num10].Data.GetUpperBound(0);
			for (long num11 = 0L; num11 < array[num10].DataLen; num11++)
			{
				num5 = ((num11 > upperBound) ? (num5 + DataLoader.WriteInt8(writer, 0L)) : (num5 + DataLoader.WriteInt8(writer, (int)array[num10].Data[num11])));
			}
		}
		fileStream.Close();
	}

	public static void WriteBackLevArkUW1(int slotNo)
	{
		DataLoader.UWBlock[] array = new DataLoader.UWBlock[45];
		ObjectLoader.UpdateObjectList(UWClass.CurrentTileMap(), UWClass.CurrentObjectList());
		long num = 0L;
		for (int i = 0; i <= GameWorldController.instance.Tilemaps.GetUpperBound(0); i++)
		{
			if (GameWorldController.instance.Tilemaps[i] != null)
			{
				long datalen = 0L;
				array[i].Data = GameWorldController.instance.Tilemaps[i].TileMapToBytes(lev_ark_file_data, out datalen);
				array[i].DataLen = array[i].Data.GetUpperBound(0) + 1;
			}
			else
			{
				num = DataLoader.getValAtAddress(lev_ark_file_data, i * 4 + 2, 32);
				array[i].Data = CopyData(num, 31752L);
				array[i].DataLen = array[i].Data.GetUpperBound(0) + 1;
			}
		}
		for (int j = 0; j <= GameWorldController.instance.Tilemaps.GetUpperBound(0); j++)
		{
			num = DataLoader.getValAtAddress(lev_ark_file_data, (j + 9) * 4 + 2, 32);
			array[j + 9].Data = CopyData(num, 384L);
			array[j + 9].DataLen = array[j + 9].Data.GetUpperBound(0) + 1;
		}
		for (int k = 0; k <= GameWorldController.instance.Tilemaps.GetUpperBound(0); k++)
		{
			if (GameWorldController.instance.Tilemaps[k] != null)
			{
				array[k + 18].Data = GameWorldController.instance.Tilemaps[k].TextureMapToBytes();
				array[k + 18].DataLen = array[k + 18].Data.GetUpperBound(0) + 1;
			}
			else
			{
				num = DataLoader.getValAtAddress(lev_ark_file_data, (k + 18) * 4 + 2, 32);
				array[k + 18].Data = CopyData(num, 122L);
				array[k + 18].DataLen = array[k + 18].Data.GetUpperBound(0) + 1;
			}
		}
		for (int l = 0; l <= GameWorldController.instance.AutoMaps.GetUpperBound(0); l++)
		{
			array[l + 27].Data = GameWorldController.instance.AutoMaps[l].AutoMapVisitedToBytes();
			if (array[l + 27].Data != null)
			{
				array[l + 27].DataLen = array[l + 27].Data.GetUpperBound(0) + 1;
			}
			else
			{
				array[l + 27].DataLen = 0L;
			}
		}
		for (int m = 0; m <= GameWorldController.instance.AutoMaps.GetUpperBound(0); m++)
		{
			array[m + 36].Data = GameWorldController.instance.AutoMaps[m].AutoMapNotesToBytes();
			if (array[m + 36].Data != null)
			{
				array[m + 36].DataLen = array[m + 36].Data.GetUpperBound(0) + 1;
			}
			else
			{
				array[m + 36].DataLen = 0L;
			}
		}
		array[0].Address = 542L;
		long address = array[0].Address;
		for (int n = 1; n < array.GetUpperBound(0); n++)
		{
			if (array[n - 1].DataLen != 0)
			{
				array[n].Address = address + array[n - 1].DataLen;
				address = array[n].Address;
			}
			else
			{
				array[n].Address = 0L;
			}
		}
		FileStream fileStream = File.Open(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "LEV.ARK", FileMode.Create);
		BinaryWriter writer = new BinaryWriter(fileStream);
		long num2 = 0L;
		num2 += DataLoader.WriteInt8(writer, 135L);
		num2 += DataLoader.WriteInt8(writer, 0L);
		for (int num3 = 0; num3 <= array.GetUpperBound(0); num3++)
		{
			num2 += DataLoader.WriteInt32(writer, array[num3].Address);
		}
		for (long num4 = num2; num4 < array[0].Address; num4++)
		{
			num2 += DataLoader.WriteInt8(writer, 0L);
		}
		for (int num5 = 0; num5 <= array.GetUpperBound(0); num5++)
		{
			if (array[num5].Data != null)
			{
				for (long num6 = 0L; num6 <= array[num5].Data.GetUpperBound(0); num6++)
				{
					num2 += DataLoader.WriteInt8(writer, (int)array[num5].Data[num6]);
				}
			}
		}
		fileStream.Close();
	}

	public static char[] CopyData(long address, long length)
	{
		char[] array = new char[length];
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			array[i] = lev_ark_file_data[address + i];
		}
		return array;
	}
}
