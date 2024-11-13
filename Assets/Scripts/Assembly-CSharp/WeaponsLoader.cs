using UnityEngine;

public class WeaponsLoader : ArtLoader
{
	private const int repeat_record_start = 0;

	private const int repeat_record = 1;

	private const int run_record = 2;

	protected Texture2D[] ImageCache = new Texture2D[1];

	public WeaponsLoader(int AuxPal)
	{
		switch (UWClass._RES)
		{
		case "UW1":
		case "UW2":
			ReadAnimData(AuxPal);
			break;
		}
	}

	public void ReadAnimData(int auxPalIndex)
	{
		int[] array = new int[390];
		int[] array2 = new int[390];
		int[] array3 = new int[776];
		int[] array4 = new int[230]
		{
			35, 36, 37, -1, 39, 40, 41, 42, -1, 44,
			45, 46, -1, 48, 49, 50, 51, -1, -1, -1,
			-1, -1, 57, 58, 59, -1, -1, 62, 63, 64,
			-1, 132, 133, 134, -1, 136, 137, 138, 139, -1,
			141, 142, 143, -1, 145, 146, 147, 148, -1, -1,
			-1, -1, -1, 154, 155, 156, -1, -1, 159, 160,
			161, -1, 229, 230, 231, -1, 233, 234, 235, 236,
			-1, 238, 239, 240, -1, 242, 243, 244, 245, -1,
			-1, -1, -1, -1, 251, 252, 253, -1, -1, 256,
			257, 258, -1, -1, -1, -1, -1, 330, 331, 332,
			-1, -1, 335, -1, 337, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, 423, 424, 425, -1, 427, 428,
			429, 430, -1, 432, 433, 434, -1, 436, 437, 438,
			439, -1, -1, -1, -1, -1, 446, 447, 448, -1,
			-1, -1, 450, 451, 452, 520, 521, 522, -1, 524,
			525, 526, 527, -1, 529, 530, 531, -1, 533, 534,
			535, 536, -1, -1, -1, -1, -1, 542, 543, 544,
			-1, -1, 547, 548, 549, -1, 617, 618, 619, -1,
			621, 622, 623, 624, -1, 626, 627, 628, -1, 630,
			631, 632, 633, -1, -1, -1, -1, -1, 639, 640,
			641, -1, -1, 644, 645, 646, -1, -1, -1, -1,
			-1, 718, 719, 720, -1, 723, -1, 725, -1, -1
		};
		int[] array5 = new int[230]
		{
			66, 67, 68, -1, 70, 71, 72, 73, -1, 75,
			76, 77, -1, 79, 80, 81, 82, -1, -1, -1,
			-1, -1, 88, 89, 90, -1, -1, 93, 94, 95,
			-1, 163, 164, 165, -1, 167, 168, 169, 170, -1,
			172, 173, 174, -1, 176, 177, 178, 179, -1, -1,
			-1, -1, -1, 185, 186, 187, -1, -1, 190, 191,
			192, -1, 260, 261, 262, -1, 264, 265, 266, 267,
			-1, 269, 270, 271, -1, 273, 274, 275, 276, -1,
			-1, -1, -1, -1, 282, 283, 284, -1, -1, 287,
			288, 289, -1, -1, -1, -1, -1, 361, 362, 363,
			-1, -1, 366, -1, 368, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, 454, 455, 456, -1, 458, 459,
			460, 461, -1, 463, 464, 465, -1, 467, 468, 469,
			470, -1, -1, -1, -1, -1, 476, 477, 478, -1,
			-1, -1, 482, 482, 483, 551, 552, 553, -1, 555,
			556, 557, 558, -1, 560, 561, 562, -1, 564, 565,
			566, 567, -1, -1, -1, -1, -1, 573, 574, 575,
			-1, -1, 578, 579, 580, -1, 648, 649, 650, -1,
			652, 653, 654, 655, -1, 657, 658, 659, -1, 661,
			662, 663, 664, -1, -1, -1, -1, -1, 670, 671,
			672, -1, -1, 675, 676, 677, -1, -1, -1, -1,
			-1, 749, 750, 751, -1, 754, -1, 756, -1, -1
		};
		string text = "DATA" + UWClass.sep + "WEAPONS.DAT";
		string text2 = "DATA" + UWClass.sep + "WEAPONS.CM";
		string text3 = "DATA" + UWClass.sep + "WEAPONS.GR";
		if (UWClass._RES == "UW2")
		{
			text = "DATA" + UWClass.sep + "WEAP.DAT";
			text2 = "DATA" + UWClass.sep + "WEAP.CM";
			text3 = "DATA" + UWClass.sep + "WEAP.GR";
		}
		int num = 0;
		int num2 = 28;
		int num3 = 112;
		int num4 = 172;
		if (UWClass._RES == "UW2")
		{
			num3 = 128;
			num4 = 208;
		}
		int num5 = 0;
		int num6 = 0;
		char[] buffer;
		DataLoader.ReadStreamFile(Loader.BasePath + text, out buffer);
		char[] buffer2;
		DataLoader.ReadStreamFile(Loader.BasePath + text3, out buffer2);
		if (UWClass._RES != "UW2")
		{
			num2 = 28;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array[j + num] = (int)DataLoader.getValAtAddress(buffer, num5++, 8);
				}
				for (int k = 0; k < num2; k++)
				{
					array2[k + num] = (int)DataLoader.getValAtAddress(buffer, num5++, 8);
				}
				num += num2;
			}
		}
		else
		{
			for (int l = 0; l <= buffer.GetUpperBound(0); l++)
			{
				array3[l] = (int)DataLoader.getValAtAddress(buffer, num5++, 8);
			}
		}
		num5 = 0;
		int num7 = (int)(((uint)buffer2[2] << 8) | buffer2[1]);
		if (UWClass._RES == "UW2")
		{
			num7 = 230;
		}
		ImageCache = new Texture2D[num7 + 1];
		for (int m = 0; m < num7; m++)
		{
			long valAtAddress = DataLoader.getValAtAddress(buffer2, m * 4 + 3, 32);
			int num8 = (int)DataLoader.getValAtAddress(buffer2, valAtAddress + 1, 8);
			int num9 = (int)DataLoader.getValAtAddress(buffer2, valAtAddress + 2, 8);
			Palette pal = PaletteLoader.LoadAuxilaryPal(Loader.BasePath + text2, GameWorldController.instance.palLoader.Palettes[PaletteNo], auxPalIndex);
			int num10 = (int)DataLoader.getValAtAddress(buffer2, valAtAddress + 4, 16);
			char[] OutputData = new char[Mathf.Max(num8 * num9 * 2, num10 * 2)];
			valAtAddress += 6;
			copyNibbles(buffer2, ref OutputData, num10, valAtAddress);
			char[] array6 = new char[num8 * num9];
			char[] array7 = new char[num4 * num3];
			if (num10 >= 6)
			{
				array6 = DecodeRLEBitmap(OutputData, num10, num8, num9, 4);
			}
			int num11 = 0;
			int num12 = 0;
			int num13;
			int num14;
			if (UWClass._RES != "UW2")
			{
				num13 = array[m];
				num14 = array2[m];
			}
			else if (array4[m] != -1)
			{
				num13 = array3[array4[m]];
				num14 = array3[array5[m]];
			}
			else
			{
				num13 = 0;
				num14 = num9;
			}
			if (!(UWClass._RES == "UW1") && (!(UWClass._RES == "UW2") || array4[m] == -1))
			{
				continue;
			}
			bool flag = false;
			for (int n = 0; n < num3; n++)
			{
				for (int num15 = 0; num15 < num4; num15++)
				{
					if (num13 + num11 == num15 && num3 - num14 + num12 == n && num11 < num8 && num12 < num9)
					{
						flag = true;
						array7[num15 + n * num4] = array6[num11 + num12 * num8];
						num11++;
					}
					else
					{
						num6 = 0;
						array7[num15 + n * num4] = (char)num6;
					}
				}
				if (flag)
				{
					num12++;
					num11 = 0;
				}
			}
			ImageCache[m] = ArtLoader.Image(array7, 0L, num4, num3, "name_goes_here", pal, true);
		}
	}

	protected void copyNibbles(char[] InputData, ref char[] OutputData, int NoOfNibbles, long add_ptr)
	{
		int num = 0;
		while (NoOfNibbles > 1)
		{
			OutputData[num] = (char)((DataLoader.getValAtAddress(InputData, add_ptr, 8) >> 4) & 0xF);
			OutputData[num + 1] = (char)(DataLoader.getValAtAddress(InputData, add_ptr, 8) & 0xF);
			num += 2;
			add_ptr++;
			NoOfNibbles -= 2;
		}
		if (NoOfNibbles == 1)
		{
			OutputData[num] = (char)((DataLoader.getValAtAddress(InputData, add_ptr, 8) >> 4) & 0xF);
		}
	}

	private char[] DecodeRLEBitmap(char[] imageData, int datalen, int imageWidth, int imageHeight, int BitSize)
	{
		char[] array = new char[imageWidth * imageHeight];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int addr_ptr = 0;
		while (num2 < imageWidth * imageHeight || addr_ptr < datalen)
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
					array[num2++] = nibble;
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
					array[num2++] = nibble;
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

	public override Texture2D LoadImageAt(int index)
	{
		if (ImageCache[index] != null)
		{
			return ImageCache[index];
		}
		return base.LoadImageAt(index);
	}
}
