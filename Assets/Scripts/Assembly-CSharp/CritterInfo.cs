using System.IO;
using UnityEngine;

public class CritterInfo : Loader
{
	public const int idle_combat = 0;

	public const int attack_bash = 1;

	public const int attack_slash = 2;

	public const int attack_thrust = 3;

	public const int attack_unk4 = 4;

	public const int attack_secondary = 5;

	public const int attack_unk6 = 6;

	public const int walking_towards = 7;

	public const int death = 12;

	public const int begin_combat = 13;

	public const int idle_rear = 32;

	public const int idle_rear_right = 33;

	public const int idle_right = 34;

	public const int idle_front_right = 35;

	public const int idle_front = 36;

	public const int idle_front_left = 37;

	public const int idle_left = 38;

	public const int idle_rear_left = 39;

	public const int walking_rear = 128;

	public const int walking_rear_right = 129;

	public const int walking_right = 130;

	public const int walking_front_right = 131;

	public const int walking_front = 132;

	public const int walking_front_left = 133;

	public const int walking_left = 134;

	public const int walking_rear_left = 135;

	private char[] FilePage0;

	private char[] FilePage1;

	public Palette pal;

	public CritterAnimInfo AnimInfo;

	public CritterInfo(int critter_id, Palette paletteToUse, int AuxPalNo)
	{
		string text = DecimalToOct(critter_id.ToString());
		pal = paletteToUse;
		AnimInfo = new CritterAnimInfo();
		int spriteIndex = 0;
		for (int i = 0; i < 2; i++)
		{
			if (i == 0)
			{
				DataLoader.ReadStreamFile(Loader.BasePath + "CRIT" + UWClass.sep + "CR" + text + "PAGE.N0" + i, out FilePage0);
				bool loadMod = Directory.Exists(Loader.BasePath + "CRIT" + UWClass.sep + "CR" + text + "PAGE_N0" + i);
				spriteIndex = ReadPageFile(FilePage0, critter_id, i, spriteIndex, AuxPalNo, loadMod, Loader.BasePath + "CRIT" + UWClass.sep + "CR" + text + "PAGE_N0" + i);
			}
			else
			{
				DataLoader.ReadStreamFile(Loader.BasePath + "CRIT" + UWClass.sep + "CR" + text + "PAGE.N0" + i, out FilePage1);
				bool loadMod2 = Directory.Exists(Loader.BasePath + "CRIT" + UWClass.sep + "CR" + text + "PAGE.N0" + i);
				ReadPageFile(FilePage1, critter_id, i, spriteIndex, AuxPalNo, loadMod2, Loader.BasePath + "CRIT" + UWClass.sep + "CR" + text + "PAGE.N0" + i);
			}
		}
	}

	public CritterInfo(int critter_id, Palette paletteToUse, int palno, char[] assocData, char[] PGMP, char[] cran)
	{
		int num = 0;
		string text = DecimalToOct(critter_id.ToString());
		AnimInfo = new CritterAnimInfo();
		int spriteIndex = 0;
		for (int i = 0; i < 8; i++)
		{
			if ((int)DataLoader.getValAtAddress(PGMP, critter_id * 8 + i, 8) != 255)
			{
				string text2 = DecimalToOct(num.ToString());
				string fileCrit = Loader.BasePath + UWClass.sep + "CRIT" + UWClass.sep + "CR" + text + "." + text2;
				spriteIndex = ReadUW2PageFileData(assocData, palno, fileCrit, AnimInfo, spriteIndex, paletteToUse);
				num++;
			}
		}
		int num2 = critter_id * 512;
		for (int j = 0; j < 8; j++)
		{
			bool flag = isAnimUnAngled(j);
			for (int k = 0; k < 8; k++)
			{
				if (flag && k != 4)
				{
					continue;
				}
				int uW2Anim = GetUW2Anim(j, k);
				int num3 = TranslateAnimToIndex(uW2Anim);
				AnimInfo.animName[num3] = PrintAnimName(uW2Anim);
				int num4 = (int)DataLoader.getValAtAddress(cran, num2 + j * 64 + k * 8 + 7, 8);
				for (int l = 0; l < 8; l++)
				{
					int num5 = (int)DataLoader.getValAtAddress(cran, num2 + j * 64 + k * 8 + l, 8);
					if (l < num4)
					{
						AnimInfo.animIndices[num3, l] = num5;
					}
					else
					{
						AnimInfo.animIndices[num3, l] = -1;
					}
				}
			}
		}
	}

	private int ReadPageFile(char[] PageFile, int XX, int YY, int spriteIndex, int AuxPalNo, bool LoadMod, string ModPath)
	{
		int num = 0;
		int num2 = (int)DataLoader.getValAtAddress(PageFile, num++, 8);
		int num3 = (int)DataLoader.getValAtAddress(PageFile, num++, 8);
		int[] array = new int[num3];
		int num4 = 0;
		int num5 = 0;
		string text = DecimalToOct(XX.ToString());
		string text2 = DecimalToOct(YY.ToString());
		for (int i = 0; i < num3; i++)
		{
			int num6 = (int)DataLoader.getValAtAddress(PageFile, num++, 8);
			if (num6 != 255)
			{
				array[num5++] = i;
			}
		}
		int num7 = (int)DataLoader.getValAtAddress(PageFile, num++, 8);
		for (int j = 0; j < num7; j++)
		{
			string text3 = PrintAnimName(num2 + array[j]);
			int num8 = TranslateAnimToIndex(num2 + array[j]);
			AnimInfo.animName[num8] = text3;
			int num9 = 0;
			for (int k = 0; k < 8; k++)
			{
				int num10 = (int)DataLoader.getValAtAddress(PageFile, num++, 8);
				if (num10 != 255)
				{
					AnimInfo.animSequence[num8, k] = "CR" + text + "PAGE_N" + text2 + "_" + AuxPalNo + "_" + num10.ToString("d4");
					AnimInfo.animIndices[num8, k] = num10 + spriteIndex;
					num9++;
				}
				else
				{
					AnimInfo.animIndices[num8, k] = -1;
				}
			}
		}
		int num11 = (int)DataLoader.getValAtAddress(PageFile, num, 8);
		num++;
		char[] array2 = new char[32];
		for (int l = 0; l < 32; l++)
		{
			array2[l] = (char)DataLoader.getValAtAddress(PageFile, num + AuxPalNo * 32 + l, 8);
		}
		num += num11 * 32;
		int num12 = (int)DataLoader.getValAtAddress(PageFile, num, 8);
		num += 2;
		int num13 = num;
		int num14 = 0;
		int num15 = 0;
		int num16 = 0;
		int num17 = 0;
		for (int m = 0; m <= 1; m++)
		{
			num = num13;
			if (m == 0)
			{
				for (int n = 0; n < num12; n++)
				{
					int num18 = (int)DataLoader.getValAtAddress(PageFile, num + n * 2, 16);
					int num19 = (int)DataLoader.getValAtAddress(PageFile, num18, 8);
					int num20 = (int)DataLoader.getValAtAddress(PageFile, num18 + 1, 8);
					int num21 = (int)DataLoader.getValAtAddress(PageFile, num18 + 2, 8);
					int num22 = (int)DataLoader.getValAtAddress(PageFile, num18 + 3, 8);
					if (num21 > num19)
					{
						num21 = num19;
					}
					if (num22 > num20)
					{
						num22 = num20;
					}
					if (num19 > num14)
					{
						num14 = num19;
					}
					if (num20 > num15)
					{
						num15 = num20;
					}
					if (num21 > num16)
					{
						num16 = num21;
					}
					if (num22 > num17)
					{
						num17 = num22;
					}
				}
				continue;
			}
			if (num16 * 2 > num14)
			{
				num14 = num16 * 2;
			}
			char[] array3 = new char[num14 * num15 * 2];
			for (int num23 = 0; num23 < num12; num23++)
			{
				int num24 = (int)DataLoader.getValAtAddress(PageFile, num + num23 * 2, 16);
				int num25 = (int)DataLoader.getValAtAddress(PageFile, num24, 8);
				int num26 = (int)DataLoader.getValAtAddress(PageFile, num24 + 1, 8);
				int num27 = (int)DataLoader.getValAtAddress(PageFile, num24 + 2, 8);
				int num28 = (int)DataLoader.getValAtAddress(PageFile, num24 + 3, 8);
				int num29 = (int)DataLoader.getValAtAddress(PageFile, num24 + 4, 8);
				int datalen = (int)DataLoader.getValAtAddress(PageFile, num24 + 5, 16);
				int num30 = num16 - num27;
				int num31 = num17 - num28;
				num30 = ((num30 > 0) ? (num30 - 1) : 0);
				if (num31 <= 0)
				{
					num31 = 0;
				}
				bool flag = false;
				if (LoadMod && File.Exists(ModPath + UWClass.sep + AuxPalNo + UWClass.sep + num23.ToString("d3") + ".tga"))
				{
					Texture2D texture2D = TGALoader.LoadTGA(ModPath + UWClass.sep + AuxPalNo + UWClass.sep + num23.ToString("d3") + ".tga");
					flag = true;
					AnimInfo.animSprites[spriteIndex + num23] = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0f));
				}
				if (flag)
				{
					continue;
				}
				char[] array4 = new char[num25 * num26 * 2];
				array3 = new char[num14 * num15 * 2];
				ArtLoader.ua_image_decode_rle(PageFile, array4, (num29 != 6) ? 4 : 5, datalen, num25 * num26, num24 + 7, array2);
				num31 = num15 - num31;
				int num32 = 0;
				int num33 = 0;
				bool flag2 = false;
				for (int num34 = 0; num34 < num15; num34++)
				{
					for (int num35 = 0; num35 < num14; num35++)
					{
						if (num30 + num32 == num35 && num15 - num31 + num33 == num34 && num32 < num25 && num33 < num26)
						{
							flag2 = true;
							array3[num35 + num34 * num14] = array4[num32 + num33 * num25];
							num32++;
						}
						else
						{
							array3[num35 + num34 * num14] = '\0';
						}
					}
					if (flag2)
					{
						num33++;
						num32 = 0;
					}
				}
				num25 = num14;
				num26 = num15;
				Texture2D imgData = ArtLoader.Image(array3, 0L, num25, num26, "namehere", pal, true, true);
				CropImageData(ref imgData, pal);
				AnimInfo.animSprites[spriteIndex + num23] = Sprite.Create(imgData, new Rect(0f, 0f, imgData.width, imgData.height), new Vector2(0.5f, 0f));
				AnimInfo.animSprites[spriteIndex + num23].texture.filterMode = FilterMode.Point;
				num4++;
			}
		}
		return num4;
	}

	public static string PrintAnimName(int animNo)
	{
		switch (animNo)
		{
		case 0:
			return "idle_combat";
		case 1:
			return "attack_bash";
		case 2:
			return "attack_slash";
		case 3:
			return "attack_thrust";
		case 4:
			return "attack_unk4";
		case 5:
			return "attack_secondary";
		case 6:
			return "attack_unk6";
		case 7:
			return "walking_towards";
		case 12:
			return "death";
		case 13:
			return "begin_combat";
		case 32:
			return "idle_rear";
		case 33:
			return "idle_rear_right";
		case 34:
			return "idle_right";
		case 35:
			return "idle_front_right";
		case 36:
			return "idle_front";
		case 37:
			return "idle_front_left";
		case 38:
			return "idle_left";
		case 39:
			return "idle_rear_left";
		case 40:
			return "unknown_anim_40";
		case 41:
			return "unknown_anim_41";
		case 42:
			return "unknown_anim_42";
		case 43:
			return "unknown_anim_43";
		case 44:
			return "unknown_anim_44";
		case 45:
			return "unknown_anim_45";
		case 46:
			return "unknown_anim_46";
		case 47:
			return "unknown_anim_47";
		case 80:
			return "unknown_anim_80";
		case 81:
			return "unknown_anim_81";
		case 82:
			return "unknown_anim_82";
		case 83:
			return "unknown_anim_83";
		case 84:
			return "unknown_anim_84";
		case 85:
			return "unknown_anim_85";
		case 86:
			return "unknown_anim_86";
		case 87:
			return "unknown_anim_87";
		case 128:
			return "walking_rear";
		case 129:
			return "walking_rear_right";
		case 130:
			return "walking_right";
		case 131:
			return "walking_front_right";
		case 132:
			return "walking_front";
		case 133:
			return "walking_front_left";
		case 134:
			return "walking_left";
		case 135:
			return "walking_rear_left";
		default:
			Debug.Log("unknown animation" + animNo);
			return "unknown_anim";
		}
	}

	public static int TranslateIndexToAnim(int animIndex)
	{
		switch (animIndex)
		{
		case 0:
			return 0;
		case 1:
			return 1;
		case 2:
			return 2;
		case 3:
			return 3;
		case 4:
			return 4;
		case 5:
			return 5;
		case 6:
			return 6;
		case 7:
			return 7;
		case 8:
			return 12;
		case 9:
			return 13;
		case 10:
			return 32;
		case 11:
			return 33;
		case 12:
			return 34;
		case 13:
			return 35;
		case 14:
			return 36;
		case 15:
			return 37;
		case 16:
			return 38;
		case 17:
			return 39;
		case 18:
			return 128;
		case 19:
			return 129;
		case 20:
			return 130;
		case 21:
			return 131;
		case 22:
			return 132;
		case 23:
			return 133;
		case 24:
			return 134;
		case 25:
			return 135;
		case 26:
			return 80;
		case 27:
			return 81;
		case 28:
			return 82;
		case 29:
			return 83;
		case 30:
			return 84;
		case 31:
			return 85;
		case 32:
			return 86;
		case 33:
			return 87;
		case 34:
			return 40;
		case 35:
			return 41;
		case 36:
			return 42;
		case 37:
			return 43;
		case 38:
			return 44;
		case 39:
			return 45;
		case 40:
			return 46;
		case 41:
			return 47;
		default:
			return 0;
		}
	}

	public static int TranslateAnimToIndex(int animNo)
	{
		switch (animNo)
		{
		case 0:
			return 0;
		case 1:
			return 1;
		case 2:
			return 2;
		case 3:
			return 3;
		case 4:
			return 4;
		case 5:
			return 5;
		case 6:
			return 6;
		case 7:
			return 7;
		case 12:
			return 8;
		case 13:
			return 9;
		case 32:
			return 10;
		case 33:
			return 11;
		case 34:
			return 12;
		case 35:
			return 13;
		case 36:
			return 14;
		case 37:
			return 15;
		case 38:
			return 16;
		case 39:
			return 17;
		case 128:
			return 18;
		case 129:
			return 19;
		case 130:
			return 20;
		case 131:
			return 21;
		case 132:
			return 22;
		case 133:
			return 23;
		case 134:
			return 24;
		case 135:
			return 25;
		case 80:
			return 26;
		case 81:
			return 27;
		case 82:
			return 28;
		case 83:
			return 29;
		case 84:
			return 30;
		case 85:
			return 31;
		case 86:
			return 32;
		case 87:
			return 33;
		case 40:
			return 34;
		case 41:
			return 35;
		case 42:
			return 36;
		case 43:
			return 37;
		case 44:
			return 38;
		case 45:
			return 39;
		case 46:
			return 40;
		case 47:
			return 41;
		default:
			return 0;
		}
	}

	public string DecimalToOct(string data)
	{
		if (data == "0")
		{
			return "00";
		}
		string text = string.Empty;
		int num = 0;
		int num2 = int.Parse(data);
		while (num2 > 0)
		{
			num = num2 % 8;
			num2 /= 8;
			text = num + text;
		}
		if (text.Length == 1)
		{
			text = "0" + text;
		}
		return text;
	}

	private int ReadUW2PageFileData(char[] assocFile, int AuxPalNo, string fileCrit, CritterAnimInfo critanim, int spriteIndex, Palette paletteToUse)
	{
		char[] buffer;
		DataLoader.ReadStreamFile(fileCrit, out buffer);
		int num = 0;
		char[] array = new char[32];
		for (int i = 0; i < 32; i++)
		{
			array[i] = (char)DataLoader.getValAtAddress(buffer, num + AuxPalNo * 32 + i, 8);
		}
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int j = 0; j <= 1; j++)
		{
			if (j == 0)
			{
				for (int k = 128; k < 640; k += 2)
				{
					int num6 = (int)DataLoader.getValAtAddress(buffer, k, 16);
					if (num6 != 0)
					{
						int num7 = (int)DataLoader.getValAtAddress(buffer, num6, 8);
						int num8 = (int)DataLoader.getValAtAddress(buffer, num6 + 1, 8);
						int num9 = (int)DataLoader.getValAtAddress(buffer, num6 + 2, 8);
						int num10 = (int)DataLoader.getValAtAddress(buffer, num6 + 3, 8);
						if (num9 > num7)
						{
							num9 = num7;
						}
						if (num10 > num8)
						{
							num10 = num8;
						}
						if (num7 > num2)
						{
							num2 = num7;
						}
						if (num8 > num3)
						{
							num3 = num8;
						}
						if (num9 > num4)
						{
							num4 = num9;
						}
						if (num10 > num5)
						{
							num5 = num10;
						}
					}
				}
				string text = fileCrit.Substring(fileCrit.Length - 7, 4).ToUpper();
				if (text != null && text == "CR02")
				{
					num3 = 100;
				}
				continue;
			}
			if (num4 * 2 > num2)
			{
				num2 = num4 * 2;
			}
			char[] array2 = new char[num2 * num3 * 2];
			for (int l = 128; l < 640; l += 2)
			{
				int num11 = (int)DataLoader.getValAtAddress(buffer, l, 16);
				if (num11 == 0)
				{
					continue;
				}
				int num12 = (int)DataLoader.getValAtAddress(buffer, num11, 8);
				int num13 = (int)DataLoader.getValAtAddress(buffer, num11 + 1, 8);
				int num14 = (int)DataLoader.getValAtAddress(buffer, num11 + 2, 8);
				int num15 = (int)DataLoader.getValAtAddress(buffer, num11 + 3, 8);
				int num16 = (int)DataLoader.getValAtAddress(buffer, num11 + 4, 8);
				int datalen = (int)DataLoader.getValAtAddress(buffer, num11 + 5, 16);
				int num17 = num4 - num14;
				int num18 = num5 - num15;
				num17 = ((num17 > 0) ? (num17 - 1) : 0);
				if (num18 <= 0)
				{
					num18 = 0;
				}
				char[] array3 = new char[num12 * num13 * 2];
				ArtLoader.ua_image_decode_rle(buffer, array3, (num16 != 6) ? 4 : 5, datalen, num12 * num13, num11 + 7, array);
				num18 = num3 - num18;
				int num19 = 0;
				int num20 = 0;
				bool flag = false;
				for (int m = 0; m < num3; m++)
				{
					for (int n = 0; n < num2; n++)
					{
						if (num17 + num19 == n && num3 - num18 + num20 == m && num19 < num12 && num20 < num13)
						{
							flag = true;
							array2[n + m * num2] = array3[num19 + num20 * num12];
							num19++;
						}
						else
						{
							array2[n + m * num2] = '\0';
						}
					}
					if (flag)
					{
						num20++;
						num19 = 0;
					}
				}
				num12 = num2;
				num13 = num3;
				Texture2D imgData = ArtLoader.Image(array2, 0L, num12, num13, "namehere", paletteToUse, true, true);
				CropImageData(ref imgData, paletteToUse);
				AnimInfo.animSprites[spriteIndex++] = Sprite.Create(imgData, new Rect(0f, 0f, imgData.width, imgData.height), new Vector2(0.5f, 0f));
			}
		}
		return spriteIndex;
	}

	private bool isAnimUnAngled(int animationNo)
	{
		switch (animationNo)
		{
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 13:
			return true;
		default:
			return false;
		}
	}

	private int GetUW2Anim(int animation, int angle)
	{
		switch (animation)
		{
		case 0:
			return 32 + angle;
		case 1:
			return 128 + angle;
		case 2:
		case 3:
		case 4:
			return animation - 1;
		case 5:
		case 6:
			return animation;
		case 7:
			return 12;
		default:
			return animation;
		}
	}

	private static void CropImageData(ref Texture2D imgData, Palette PalUsed)
	{
		Color color = PalUsed.ColorAtPixel(0, true);
		int num = 0;
		for (int i = 0; i < imgData.height; i++)
		{
			bool flag = true;
			for (int j = 0; j <= imgData.width; j++)
			{
				if (imgData.GetPixel(j, i) != color)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				num++;
				for (int k = 0; k <= imgData.width; k++)
				{
					imgData.SetPixel(k, i, Color.white);
				}
				continue;
			}
			break;
		}
		if (num < imgData.height)
		{
			Texture2D texture2D = new Texture2D(imgData.width, imgData.height - num, TextureFormat.ARGB32, false);
			texture2D.SetPixels(imgData.GetPixels(0, num, imgData.width, imgData.height - num));
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			imgData = texture2D;
		}
	}
}
