using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoader : Loader
{
	public struct Chunk
	{
		public long chunkUnpackedLength;

		public int chunkCompressionType;

		public long chunkPackedLength;

		public int chunkContentType;

		public char[] data;
	}

	public struct UWBlock
	{
		public char[] Data;

		public long Address;

		public long DataLen;

		public int CompressionFlag;

		public long ReservedSpace;
	}

	public const int UW2_NOCOMPRESSION = 0;

	public const int UW2_SHOULDCOMPRESS = 1;

	public const int UW2_ISCOMPRESS = 2;

	public const int UW2_HASEXTRASPACE = 4;

	public static bool ReadStreamFile(string Path, out char[] buffer)
	{
		Path = Path.Replace("--", UWClass.sep.ToString());
		if (!File.Exists(Path))
		{
			UWHUD.instance.ErrorText.text = "File not found " + Path + "\nCheck your paths in config.ini and ensure game files have been extracted. See readme.txt";
			Debug.Log("DataLoader.ReadStreamFile : File not found : " + Path);
			buffer = null;
			return false;
		}
		FileStream fileStream = File.OpenRead(Path);
		if (fileStream.CanRead)
		{
			buffer = new char[fileStream.Length];
			for (int i = 0; i < fileStream.Length; i++)
			{
				buffer[i] = (char)fileStream.ReadByte();
			}
			fileStream.Close();
			return true;
		}
		fileStream.Close();
		buffer = new char[0];
		return false;
	}

	public static long ConvertInt16(char Byte1, char Byte2)
	{
		return (int)(((uint)Byte2 << 8) | Byte1);
	}

	public static long ConvertInt24(char Byte1, char Byte2, char Byte3)
	{
		return (int)(((uint)Byte3 << 16) | ((uint)Byte2 << 8) | Byte1);
	}

	public static long ConvertInt32(char Byte1, char Byte2, char Byte3, char Byte4)
	{
		return (int)(((uint)Byte4 << 24) | ((uint)Byte3 << 16) | ((uint)Byte2 << 8) | Byte1);
	}

	public static long getValAtAddress(UWBlock buffer, long Address, int size)
	{
		return getValAtAddress(buffer.Data, Address, size);
	}

	public static long getValAtAddress(char[] buffer, long Address, int size)
	{
		switch (size)
		{
		case 8:
			return (int)buffer[Address];
		case 16:
			return ConvertInt16(buffer[Address], buffer[Address + 1]);
		case 24:
			return ConvertInt24(buffer[Address], buffer[Address + 1], buffer[Address + 2]);
		case 32:
			return ConvertInt32(buffer[Address], buffer[Address + 1], buffer[Address + 2], buffer[Address + 3]);
		default:
			Debug.Log("Invalid data size in getValAtAddress");
			return -1L;
		}
	}

	public static char[] unpackUW2(char[] tmp, long address_pointer, ref long datalen)
	{
		long num = (int)getValAtAddress(tmp, address_pointer, 32);
		long val = (num / 4096 + 1) * 4096;
		char[] array = new char[Math.Max(val, num + 100)];
		long num2 = 0L;
		datalen = 0L;
		address_pointer += 4;
		while (num2 < num)
		{
			byte b = (byte)tmp[address_pointer++];
			for (int i = 0; i < 8; i++)
			{
				if (address_pointer > tmp.GetUpperBound(0))
				{
					return array;
				}
				if ((b & 1) == 1)
				{
					array[num2++] = tmp[address_pointer++];
					datalen++;
				}
				else
				{
					if (address_pointer >= tmp.GetUpperBound(0))
					{
						return array;
					}
					int num3 = tmp[address_pointer++];
					int num4 = tmp[address_pointer++];
					num3 |= (num4 & 0xF0) << 4;
					num4 = (num4 & 0xF) + 3;
					num3 += 18;
					if (num3 > num2)
					{
						num3 -= 4096;
					}
					for (; num3 < num2 - 4096; num3 += 4096)
					{
					}
					while (num4-- > 0)
					{
						if (num3 < 0)
						{
							array[num2++] = '\0';
							num3++;
						}
						else
						{
							array[num2++] = array[num3++];
						}
						datalen++;
					}
				}
				b >>= 1;
			}
		}
		return array;
	}

	public static char[] RepackUW2(char[] srcData)
	{
		List<char> searchfor = new List<char>();
		List<char> records = new List<char>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int headerIndex = 0;
		while (num <= srcData.GetUpperBound(0))
		{
			searchfor.Add(srcData[num++]);
			int MatchingOffset;
			if (searchfor.Count > 3 && !FindMatchingSequence(ref records, ref searchfor, out MatchingOffset))
			{
				for (int i = num3; i < searchfor.Count; i++)
				{
					if (num2 == 0)
					{
						headerIndex = CreateHeader(ref records);
					}
					CreateTransferRecord(ref records, searchfor[i], headerIndex, num2++);
				}
				searchfor.Clear();
			}
			if (num2 == 8)
			{
				num2 = 0;
			}
		}
		WriteListToBytes(records, Loader.BasePath + "DATA" + UWClass.sep + "recodetest.dat");
		char[] array = new char[records.Count];
		for (int j = 0; j < records.Count; j++)
		{
			array[j] = records[j];
		}
		return array;
	}

	private static int CreateHeader(ref List<char> Output)
	{
		Output.Add('\0');
		return Output.Count - 1;
	}

	private static void WriteListToBytes(List<char> Output, string path)
	{
		FileStream output = File.Open(path, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(output);
		WriteInt32(binaryWriter, Output.Count);
		for (int i = 0; i < Output.Count; i++)
		{
			WriteInt8(binaryWriter, (int)Output[i]);
		}
		binaryWriter.Close();
	}

	private static void CreateTransferRecord(ref List<char> Output, char TransferData, int headerIndex, int bit)
	{
		Output[headerIndex] = (char)(Output[headerIndex] | (1 << bit));
		Output.Add(TransferData);
	}

	private static int CreateCopyRecord(ref List<char> Output, int Offset, int CopyCount, int headerIndex, int bit)
	{
		Output[headerIndex] = (char)(Output[headerIndex] | (0 << bit));
		int num = Offset & 0xF;
		int num2 = (CopyCount & 0xF) | (Offset >> 7 << 4);
		Output.Add((char)num);
		Output.Add((char)num2);
		return Output.Count - 1;
	}

	private static void IncrementCopyRecord(ref List<char> Output, int CopyRecordOffset)
	{
		int copyCountAtOffset = getCopyCountAtOffset(ref Output, CopyRecordOffset);
		copyCountAtOffset++;
		copyCountAtOffset &= 0xF;
		char c = (char)(Output[CopyRecordOffset] & 0xF8u);
		c = (char)(c | copyCountAtOffset);
		Output[CopyRecordOffset] = c;
	}

	private static int getCopyCountAtOffset(ref List<char> Output, int CopyRecordOffset)
	{
		if (CopyRecordOffset < 0)
		{
			return 0;
		}
		int num = Output[CopyRecordOffset];
		return num & 0xF;
	}

	private static bool FindMatchingSequence(ref List<char> records, ref List<char> searchfor, out int MatchingOffset)
	{
		int num = records.Count / 4096 * 4096;
		string text = charListToVal(searchfor, 0, searchfor.Count);
		MatchingOffset = -1;
		for (int num2 = records.Count - searchfor.Count; num2 >= num; num2--)
		{
			string text2 = charListToVal(records, num2, searchfor.Count - 1);
			if (text2 == text)
			{
				MatchingOffset = num2;
				return true;
			}
		}
		return false;
	}

	private static string charListToVal(List<char> input, int start, int len)
	{
		string text = "";
		for (int i = start; i <= start + len; i++)
		{
			if (i < input.Count)
			{
				text += input[i];
			}
		}
		return text;
	}

	private static int getTrueOffset(int offset)
	{
		while (offset > 4096)
		{
			offset -= 4096;
		}
		offset -= 18;
		return offset;
	}

	public static void unpack_data(char[] pack, ref char[] unpack, long unpacksize)
	{
		long num = 0L;
		long num2 = 0L;
		int num3 = 0;
		int num4 = 0;
		long[] array = new long[16384];
		int[] array2 = new int[16384];
		int[] array3 = new int[16384];
		for (int i = 0; i < 16384; i++)
		{
			array2[i] = 1;
			array3[i] = -1;
		}
		num = 0L;
		num2 = 0L;
		int j = 0;
		while (num2 < unpacksize)
		{
			for (; j < 14; j += 8)
			{
				num3 = (num3 << 8) + pack[num++];
			}
			j -= 14;
			int num5 = (num3 >> j) & 0x3FFF;
			switch (num5)
			{
			case 16383:
				return;
			case 16382:
			{
				for (int i = 0; i < 16384; i++)
				{
					array2[i] = 1;
					array3[i] = -1;
				}
				num4 = 0;
				continue;
			}
			}
			if (num4 < 16384)
			{
				array[num4] = num2;
				if (num5 >= 256)
				{
					array3[num4] = num5 - 256;
				}
			}
			num4++;
			if (num5 < 256)
			{
				unpack[num2++] = (char)num5;
				continue;
			}
			num5 -= 256;
			if (array2[num5] == 1)
			{
				if (array3[num5] != -1)
				{
					array2[num5] += array2[array3[num5]];
				}
				else
				{
					array2[num5]++;
				}
			}
			for (int i = 0; i < array2[num5]; i++)
			{
				if (i + array[num5] < unpacksize)
				{
					unpack[num2++] = unpack[i + array[num5]];
				}
				else
				{
					Debug.Log("Oh shit");
				}
			}
		}
	}

	public static bool LoadChunk(char[] archive_ark, int chunkNo, out Chunk data_ark)
	{
		long num = 0L;
		data_ark.chunkPackedLength = 0L;
		data_ark.chunkUnpackedLength = 0L;
		data_ark.chunkContentType = 0;
		data_ark.chunkCompressionType = 0;
		num = getShockBlockAddress(chunkNo, archive_ark, ref data_ark.chunkPackedLength, ref data_ark.chunkUnpackedLength, ref data_ark.chunkCompressionType, ref data_ark.chunkContentType);
		if (num == -1)
		{
			data_ark.data = new char[1];
			return false;
		}
		data_ark.data = new char[data_ark.chunkUnpackedLength];
		LoadShockChunk(num, data_ark.chunkCompressionType, archive_ark, ref data_ark.data, data_ark.chunkPackedLength, data_ark.chunkUnpackedLength);
		return true;
	}

	private static long getShockBlockAddress(long BlockNo, char[] tmp_ark, ref long chunkPackedLength, ref long chunkUnpackedLength, ref int chunkCompressionType, ref int chunkContentType)
	{
		int num = 0;
		long valAtAddress = getValAtAddress(tmp_ark, 124L, 32);
		int num2 = (int)getValAtAddress(tmp_ark, valAtAddress, 16);
		long valAtAddress2 = getValAtAddress(tmp_ark, valAtAddress + 2, 32);
		long num3 = valAtAddress + 6;
		long num4 = valAtAddress2;
		for (int i = 0; i < num2; i++)
		{
			int num5 = (int)getValAtAddress(tmp_ark, num3, 16);
			chunkUnpackedLength = getValAtAddress(tmp_ark, num3 + 2, 24);
			chunkCompressionType = (int)getValAtAddress(tmp_ark, num3 + 5, 8);
			chunkPackedLength = getValAtAddress(tmp_ark, num3 + 6, 24);
			chunkContentType = (short)getValAtAddress(tmp_ark, num3 + 9, 8);
			if (num5 == BlockNo)
			{
				num = 1;
				num3 = 0L;
				break;
			}
			num4 += chunkPackedLength;
			if (num4 % 4 != 0)
			{
				num4 = num4 + 4 - num4 % 4;
			}
			num3 += 10;
		}
		if (num == 0)
		{
			return -1L;
		}
		return num4;
	}

	private static long LoadShockChunk(long AddressOfBlockStart, int chunkType, char[] archive_ark, ref char[] OutputChunk, long chunkPackedLength, long chunkUnpackedLength)
	{
		if (AddressOfBlockStart == -1)
		{
			return -1L;
		}
		switch (chunkType)
		{
		case 0:
		{
			for (long num3 = 0L; num3 < chunkUnpackedLength; num3++)
			{
				OutputChunk[num3] = archive_ark[AddressOfBlockStart + num3];
			}
			return chunkUnpackedLength;
		}
		case 1:
		{
			char[] array = new char[chunkPackedLength];
			for (long num2 = 0L; num2 < chunkPackedLength; num2++)
			{
				array[num2] = archive_ark[AddressOfBlockStart + num2];
			}
			unpack_data(array, ref OutputChunk, chunkUnpackedLength);
			return chunkUnpackedLength;
		}
		case 3:
		{
			int num4 = (int)getValAtAddress(archive_ark, AddressOfBlockStart, 16);
			int num5 = (num4 + 1) * 4 + 2;
			char[] array2 = new char[chunkPackedLength];
			char[] unpack = new char[chunkUnpackedLength];
			for (long num6 = 0L; num6 < chunkPackedLength; num6++)
			{
				array2[num6] = archive_ark[AddressOfBlockStart + num6 + num5];
			}
			unpack_data(array2, ref unpack, chunkUnpackedLength);
			for (long num7 = 0L; num7 < num5; num7++)
			{
				OutputChunk[num7] = archive_ark[AddressOfBlockStart + num7];
			}
			for (long num8 = num5; num8 < chunkUnpackedLength; num8++)
			{
				OutputChunk[num8] = unpack[num8 - num5];
			}
			return chunkUnpackedLength;
		}
		default:
		{
			for (long num = 0L; num < chunkUnpackedLength; num++)
			{
				OutputChunk[num] = archive_ark[AddressOfBlockStart + num];
			}
			return chunkUnpackedLength;
		}
		}
	}

	public static long WriteInt8(BinaryWriter writer, long val)
	{
		byte value = (byte)(val & 0xFF);
		writer.Write(value);
		return 1L;
	}

	public static long WriteInt16(BinaryWriter writer, long val)
	{
		byte value = (byte)(val & 0xFF);
		writer.Write(value);
		value = (byte)((val >> 8) & 0xFF);
		writer.Write(value);
		return 2L;
	}

	public static long WriteInt32(BinaryWriter writer, long val)
	{
		byte value = (byte)(val & 0xFF);
		writer.Write(value);
		value = (byte)((val >> 8) & 0xFF);
		writer.Write(value);
		value = (byte)((val >> 16) & 0xFF);
		writer.Write(value);
		value = (byte)((val >> 24) & 0xFF);
		writer.Write(value);
		return 4L;
	}

	public static bool LoadUWBlock(char[] arkData, int blockNo, long targetDataLen, out UWBlock uwb)
	{
		uwb = default(UWBlock);
		int num = (int)getValAtAddress(arkData, 0L, 32);
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			uwb.Address = (int)getValAtAddress(arkData, 6 + blockNo * 4, 32);
			uwb.CompressionFlag = (int)getValAtAddress(arkData, 6 + blockNo * 4 + num * 4, 32);
			uwb.DataLen = getValAtAddress(arkData, 6 + blockNo * 4 + num * 8, 32);
			uwb.ReservedSpace = getValAtAddress(arkData, 6 + blockNo * 4 + num * 12, 32);
			if (uwb.Address != 0)
			{
				if (((uwb.CompressionFlag >> 1) & 1) == 1)
				{
					uwb.Data = unpackUW2(arkData, uwb.Address, ref uwb.DataLen);
					return true;
				}
				uwb.Data = new char[uwb.DataLen];
				int num2 = 0;
				for (long num3 = uwb.Address; num3 < uwb.Address + uwb.DataLen; num3++)
				{
					uwb.Data[num2++] = arkData[num3];
				}
				return true;
			}
			uwb.DataLen = 0L;
			return false;
		}
		uwb.Address = getValAtAddress(arkData, blockNo * 4 + 2, 32);
		if (uwb.Address != 0)
		{
			uwb.Data = new char[targetDataLen];
			uwb.DataLen = targetDataLen;
			int num4 = 0;
			for (long num5 = uwb.Address; num5 < uwb.Address + uwb.DataLen; num5++)
			{
				uwb.Data[num4++] = arkData[num5];
			}
			return true;
		}
		uwb.DataLen = 0L;
		return false;
	}

	public static int ExtractBits(int value, int From, int Length)
	{
		int mask = getMask(Length);
		return (value >> From) & mask;
	}

	private static int getMask(int Length)
	{
		int num = 0;
		for (int i = 0; i < Length; i++)
		{
			num = (num << 1) | 1;
		}
		return num;
	}

	public static int InsertBits(int valueToChange, int valueToInsert, int From, int Length)
	{
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			num = ((i < From || i >= From + Length) ? ((num << 1) | 0) : ((num << 1) | 1));
		}
		valueToChange &= num;
		valueToInsert &= getMask(Length);
		valueToChange = valueToInsert << From;
		return valueToChange;
	}
}
