using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : Loader
{
	public enum InventorySlotsOffsets
	{
		UW1Helm = 248,
		UW1Chest = 250,
		UW1Gloves = 252,
		UW1Leggings = 254,
		UW1Boots = 256,
		UW1RightShoulder = 258,
		UW1LeftShoulder = 260,
		UW1RightHand = 262,
		UW1LeftHand = 264,
		UW1RightRing = 266,
		UW1LeftRing = 268,
		UW1Backpack0 = 270,
		UW1Backpack1 = 272,
		UW1Backpack2 = 274,
		UW1Backpack3 = 276,
		UW1Backpack4 = 278,
		UW1Backpack5 = 280,
		UW1Backpack6 = 282,
		UW1Backpack7 = 284,
		UW2Helm = 931,
		UW2Chest = 933,
		UW2Gloves = 935,
		UW2Leggings = 937,
		UW2Boots = 939,
		UW2RightShoulder = 941,
		UW2LeftShoulder = 943,
		UW2RightHand = 945,
		UW2LeftHand = 947,
		UW2RightRing = 949,
		UW2LeftRing = 951,
		UW2Backpack0 = 953,
		UW2Backpack1 = 955,
		UW2Backpack2 = 957,
		UW2Backpack3 = 959,
		UW2Backpack4 = 961,
		UW2Backpack5 = 963,
		UW2Backpack6 = 965,
		UW2Backpack7 = 967
	}

	private const float Ratio = 213f;

	private const float VertAdjust = 0.3543672f;

	private const int NoOfEncryptedBytes = 210;

	public static void LoadPlayerDatUW1(int slotNo)
	{
		int[] ActiveEffectIds = new int[3];
		short[] ActiveEffectStability = new short[3];
		int num = 0;
		ResetUI();
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "PLAYER.DAT", out buffer))
		{
			return;
		}
		int num2 = buffer[0];
		UWCharacter.Instance.XorKey = num2;
		int num3 = 3;
		for (int i = 1; i <= 210; i++)
		{
			if (i == 81 || i == 161)
			{
				num3 = 3;
			}
			buffer[i] ^= (char)(ushort)((num2 + num3) & 0xFF);
			num3 += 3;
		}
		LoadName(buffer);
		LoadStats(buffer);
		num = LoadSpellEffects(buffer, ref ActiveEffectIds, ref ActiveEffectStability);
		LoadRunes(buffer);
		LoadPlayerClass(buffer, 101);
		LoadGameOptions(buffer, 182);
		for (int j = 75; j <= 221; j++)
		{
			switch (j)
			{
			case 79:
				UWCharacter.Instance.EXP = (int)DataLoader.getValAtAddress(buffer, j, 32);
				break;
			case 83:
				UWCharacter.Instance.TrainingPoints = buffer[j];
				break;
			case 85:
				LoadPosition(buffer);
				break;
			case 95:
				UWCharacter.Instance.ResurrectLevel = (short)(((int)buffer[j] >> 4) & 0xF);
				UWCharacter.Instance.MoonGateLevel = (short)(buffer[j] & 0xF);
				break;
			case 96:
				Quest.instance.IncenseDream = buffer[j] & 3;
				UWCharacter.Instance.play_poison = (short)(((int)buffer[j] >> 2) & 0xF);
				UWCharacter.Instance.poison_timer = 30f;
				num = ((int)buffer[j] >> 6) & 3;
				break;
			case 97:
				Quest.instance.isOrbDestroyed = (((int)DataLoader.getValAtAddress(buffer, j, 8) >> 5) & 1) == 1;
				Quest.instance.isCupFound = (((int)DataLoader.getValAtAddress(buffer, j, 8) >> 6) & 1) == 1;
				break;
			case 99:
				Quest.instance.isGaramonBuried = buffer[j] == '\u001c';
				break;
			case 102:
			{
				int num4 = (int)DataLoader.getValAtAddress(buffer, j, 32);
				for (int k = 0; k <= 31; k++)
				{
					if (((num4 >> k) & 1) == 1)
					{
						Quest.instance.QuestVariables[k] = 1;
					}
					else
					{
						Quest.instance.QuestVariables[k] = 0;
					}
				}
				break;
			}
			case 106:
				Quest.instance.QuestVariables[32] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 107:
				Quest.instance.QuestVariables[33] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 108:
				Quest.instance.QuestVariables[34] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 109:
				Quest.instance.QuestVariables[35] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 110:
				Quest.instance.TalismansRemaining = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 111:
				Quest.instance.GaramonDream = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 124:
			case 125:
			case 126:
			case 127:
			case 128:
			case 129:
			case 130:
			case 131:
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 140:
			case 141:
			case 142:
			case 143:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 170:
			case 171:
			case 172:
			case 173:
			case 174:
			case 175:
			case 176:
				Quest.instance.variables[j - 113] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 177:
				UWCharacter.Instance.PlayerMagic.TrueMaxMana = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 207:
				GameClock.instance.game_time = (int)DataLoader.getValAtAddress(buffer, j, 32);
				break;
			case 208:
				GameClock.instance.gametimevals[0] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 209:
				GameClock.instance.gametimevals[1] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 210:
				GameClock.instance.gametimevals[2] = (int)DataLoader.getValAtAddress(buffer, j, 8);
				break;
			case 221:
				UWCharacter.Instance.CurVIT = buffer[j];
				break;
			}
		}
		ApplySpellEffects(ActiveEffectIds, ActiveEffectStability, num);
		GameClock.setUWTime(GameClock.instance.gametimevals[0] + GameClock.instance.gametimevals[1] * 255 + GameClock.instance.gametimevals[2] * 255 * 255);
		ResetInventory();
		LoadInventory(buffer, 312, 248, 286);
		if (UWCharacter.Instance.decode)
		{
			byte[] array = new byte[buffer.GetUpperBound(0) + 1];
			for (long num5 = 0L; num5 <= buffer.GetUpperBound(0); num5++)
			{
				array[num5] = (byte)buffer[num5];
			}
			File.WriteAllBytes(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "decode_" + slotNo + ".dat", array);
		}
	}

	public static int GetActiveSpellID(int val)
	{
		int num = ((val & 0xF) << 4) | ((val & 0xF0) >> 4);
		switch (num)
		{
		case 178:
			return 176;
		case 179:
			return 184;
		case 176:
			return 187;
		case 183:
			return 183;
		default:
			return num;
		}
	}

	private static void SetActiveRuneSlots(int slotNo, int rune)
	{
		if (rune < 24)
		{
			UWCharacter.Instance.PlayerMagic.ActiveRunes[slotNo] = rune;
		}
		else
		{
			UWCharacter.Instance.PlayerMagic.ActiveRunes[slotNo] = -1;
		}
		ActiveRuneSlot.UpdateRuneSlots();
	}

	public static void WritePlayerDatUW1(int slotNo)
	{
		FileStream output = File.Open(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "playertmp.dat", FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(output);
		int num = 0;
		int NoOfInventoryItems = 0;
		string[] array = ObjectLoader.UpdateInventoryObjectList(out NoOfInventoryItems);
		DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.XorKey);
		WriteName(binaryWriter);
		WriteSpace(binaryWriter, 17);
		WriteSkills(binaryWriter);
		WriteSpellEffects(binaryWriter);
		WriteRunes(binaryWriter);
		for (int i = 75; i < 312; i++)
		{
			switch (i)
			{
			case 75:
				DataLoader.WriteInt8(binaryWriter, array.GetUpperBound(0) + 1 << 2);
				break;
			case 77:
				DataLoader.WriteInt16(binaryWriter, UWCharacter.Instance.PlayerSkills.STR * 2 * 10);
				break;
			case 79:
				DataLoader.WriteInt32(binaryWriter, UWCharacter.Instance.EXP);
				break;
			case 83:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.TrainingPoints);
				break;
			case 85:
				WritePosition(binaryWriter);
				break;
			case 95:
			{
				int num2 = ((UWCharacter.Instance.ResurrectLevel & 0xF) << 4) | (UWCharacter.Instance.MoonGateLevel & 0xF);
				DataLoader.WriteInt8(binaryWriter, num2);
				break;
			}
			case 96:
				DataLoader.WriteInt8(binaryWriter, ((num & 3) << 6) | (UWCharacter.Instance.play_poison << 2) | (Quest.instance.IncenseDream & 3));
				break;
			case 97:
			{
				int num3 = 0;
				if (Quest.instance.isOrbDestroyed)
				{
					num3 = 32;
				}
				if (Quest.instance.isCupFound)
				{
					num3 |= 0x40;
				}
				DataLoader.WriteInt8(binaryWriter, num3);
				break;
			}
			case 99:
				if (Quest.instance.isGaramonBuried)
				{
					DataLoader.WriteInt8(binaryWriter, 28L);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, 16L);
				}
				break;
			case 101:
				WritePlayerClass(binaryWriter);
				break;
			case 102:
				WriteUW1QuestFlags(binaryWriter);
				break;
			case 106:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[32]);
				break;
			case 107:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[33]);
				break;
			case 108:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[34]);
				break;
			case 109:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[35]);
				break;
			case 110:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.TalismansRemaining);
				break;
			case 111:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.GaramonDream);
				break;
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 124:
			case 125:
			case 126:
			case 127:
			case 128:
			case 129:
			case 130:
			case 131:
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 140:
			case 141:
			case 142:
			case 143:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 170:
			case 171:
			case 172:
			case 173:
			case 174:
			case 175:
			case 176:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.variables[i - 113]);
				break;
			case 177:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerMagic.TrueMaxMana);
				break;
			case 188:
				DataLoader.WriteInt8(binaryWriter, 255L);
				break;
			case 181:
				DataLoader.WriteInt8(binaryWriter, GameWorldController.instance.difficulty);
				break;
			case 182:
				WriteGameOptions(binaryWriter);
				break;
			case 183:
				DataLoader.WriteInt8(binaryWriter, 8L);
				break;
			case 207:
				DataLoader.WriteInt8(binaryWriter, 0L);
				break;
			case 208:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[0]);
				break;
			case 209:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[1]);
				break;
			case 210:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[2]);
				break;
			case 211:
				DataLoader.WriteInt16(binaryWriter, array.GetUpperBound(0) + 1 + 1);
				break;
			case 213:
				DataLoader.WriteInt8(binaryWriter, 127L);
				break;
			case 214:
				DataLoader.WriteInt8(binaryWriter, 32L);
				break;
			case 219:
				if (GameWorldController.instance.InventoryMarker.transform.childCount > 0)
				{
					DataLoader.WriteInt8(binaryWriter, 64L);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, 0L);
				}
				break;
			case 221:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.CurVIT);
				break;
			case 248:
				WriteInventoryIndex(binaryWriter, array, 0);
				break;
			case 250:
				WriteInventoryIndex(binaryWriter, array, 1);
				break;
			case 252:
				WriteInventoryIndex(binaryWriter, array, 4);
				break;
			case 254:
				WriteInventoryIndex(binaryWriter, array, 2);
				break;
			case 256:
				WriteInventoryIndex(binaryWriter, array, 3);
				break;
			case 258:
				WriteInventoryIndex(binaryWriter, array, 5);
				break;
			case 260:
				WriteInventoryIndex(binaryWriter, array, 6);
				break;
			case 262:
				WriteInventoryIndex(binaryWriter, array, 7);
				break;
			case 264:
				WriteInventoryIndex(binaryWriter, array, 8);
				break;
			case 266:
				WriteInventoryIndex(binaryWriter, array, 9);
				break;
			case 268:
				WriteInventoryIndex(binaryWriter, array, 10);
				break;
			case 270:
				WriteInventoryIndex(binaryWriter, array, 11);
				break;
			case 272:
				WriteInventoryIndex(binaryWriter, array, 12);
				break;
			case 274:
				WriteInventoryIndex(binaryWriter, array, 13);
				break;
			case 276:
				WriteInventoryIndex(binaryWriter, array, 14);
				break;
			case 278:
				WriteInventoryIndex(binaryWriter, array, 15);
				break;
			case 280:
				WriteInventoryIndex(binaryWriter, array, 16);
				break;
			case 282:
				WriteInventoryIndex(binaryWriter, array, 17);
				break;
			case 284:
				WriteInventoryIndex(binaryWriter, array, 18);
				break;
			default:
				DataLoader.WriteInt8(binaryWriter, 0L);
				break;
			case 78:
			case 80:
			case 81:
			case 82:
			case 86:
			case 87:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
			case 103:
			case 104:
			case 105:
			case 212:
			case 249:
			case 251:
			case 253:
			case 255:
			case 257:
			case 259:
			case 261:
			case 263:
			case 265:
			case 267:
			case 269:
			case 271:
			case 273:
			case 275:
			case 277:
			case 279:
			case 281:
			case 283:
			case 285:
				break;
			}
		}
		WriteInventory(binaryWriter, array);
		binaryWriter.Close();
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "playertmp.dat", out buffer))
		{
			return;
		}
		int num4 = buffer[0];
		int num5 = 3;
		for (int j = 1; j <= 210; j++)
		{
			if (j == 81 || j == 161)
			{
				num5 = 3;
			}
			buffer[j] ^= (char)(ushort)((num4 + num5) & 0xFF);
			num5 += 3;
		}
		byte[] array2 = new byte[buffer.GetUpperBound(0) + 1];
		for (long num6 = 0L; num6 <= buffer.GetUpperBound(0); num6++)
		{
			array2[num6] = (byte)buffer[num6];
		}
		File.WriteAllBytes(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "PLAYER.DAT", array2);
	}

	public static void WritePlayerDatUW2(int slotNo)
	{
		FileStream output = File.Open(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "playertmp.dat", FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(output);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int NoOfInventoryItems = 0;
		string[] array = ObjectLoader.UpdateInventoryObjectList(out NoOfInventoryItems);
		Vector3 tileVector = UWClass.CurrentTileMap().getTileVector(UWCharacter.Instance.DreamReturnTileX, UWCharacter.Instance.DreamReturnTileY);
		DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.XorKey);
		WriteName(binaryWriter);
		WriteSpace(binaryWriter, 17);
		WriteSkills(binaryWriter);
		WriteSpellEffects(binaryWriter);
		WriteRunes(binaryWriter);
		for (int i = 75; i < 995; i++)
		{
			switch (i)
			{
			case 231:
			case 232:
			case 233:
			case 234:
			case 235:
			case 236:
			case 237:
			case 238:
			case 239:
			case 240:
			case 241:
			case 242:
			case 243:
			case 244:
			case 245:
			case 246:
			case 247:
			case 248:
			case 249:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[128 + (i - 231)]);
				continue;
			case 250:
			case 252:
			case 254:
			case 256:
			case 258:
			case 260:
			case 262:
			case 264:
			case 266:
			case 268:
			case 270:
			case 272:
			case 274:
			case 276:
			case 278:
			case 280:
			case 282:
			case 284:
			case 286:
			case 288:
			case 290:
			case 292:
			case 294:
			case 296:
			case 298:
			case 300:
			case 302:
			case 304:
			case 306:
			case 308:
			case 310:
			case 312:
			case 314:
			case 316:
			case 318:
			case 320:
			case 322:
			case 324:
			case 326:
			case 328:
			case 330:
			case 332:
			case 334:
			case 336:
			case 338:
			case 340:
			case 342:
			case 344:
			case 346:
			case 348:
			case 350:
			case 352:
			case 354:
			case 356:
			case 358:
			case 360:
			case 362:
			case 364:
			case 366:
			case 368:
			case 370:
			case 372:
			case 374:
			case 376:
			case 378:
			case 380:
			case 382:
			case 384:
			case 386:
			case 388:
			case 390:
			case 392:
			case 394:
			case 396:
			case 398:
			case 400:
			case 402:
			case 404:
			case 406:
			case 408:
			case 410:
			case 412:
			case 414:
			case 416:
			case 418:
			case 420:
			case 422:
			case 424:
			case 426:
			case 428:
			case 430:
			case 432:
			case 434:
			case 436:
			case 438:
			case 440:
			case 442:
			case 444:
			case 446:
			case 448:
			case 450:
			case 452:
			case 454:
			case 456:
			case 458:
			case 460:
			case 462:
			case 464:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.variables[num3++]);
				continue;
			case 468:
			case 470:
			case 472:
			case 474:
			case 476:
			case 478:
			case 480:
			case 482:
			case 484:
			case 486:
			case 488:
			case 490:
			case 492:
			case 494:
			case 496:
			case 498:
			case 500:
			case 502:
			case 504:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.variables[num3++]);
				continue;
			case 466:
				DataLoader.WriteInt16(binaryWriter, Quest.instance.variables[num3++]);
				continue;
			case 506:
			case 508:
			case 510:
			case 512:
			case 514:
			case 516:
			case 518:
			case 520:
			case 522:
			case 524:
			case 526:
			case 528:
			case 530:
			case 532:
			case 534:
			case 536:
			case 538:
			case 540:
			case 542:
			case 544:
			case 546:
			case 548:
			case 550:
			case 552:
			case 554:
			case 556:
			case 558:
			case 560:
			case 562:
			case 564:
			case 566:
			case 568:
			case 570:
			case 572:
			case 574:
			case 576:
			case 578:
			case 580:
			case 582:
			case 584:
			case 586:
			case 588:
			case 590:
			case 592:
			case 594:
			case 596:
			case 598:
			case 600:
			case 602:
			case 604:
			case 606:
			case 608:
			case 610:
			case 612:
			case 614:
			case 616:
			case 618:
			case 620:
			case 622:
			case 624:
			case 626:
			case 628:
			case 630:
			case 632:
			case 634:
			case 636:
			case 638:
			case 640:
			case 642:
			case 644:
			case 646:
			case 648:
			case 650:
			case 652:
			case 654:
			case 656:
			case 658:
			case 660:
			case 662:
			case 664:
			case 666:
			case 668:
			case 670:
			case 672:
			case 674:
			case 676:
			case 678:
			case 680:
			case 682:
			case 684:
			case 686:
			case 688:
			case 690:
			case 692:
			case 694:
			case 696:
			case 698:
			case 700:
			case 702:
			case 704:
			case 706:
			case 708:
			case 710:
			case 712:
			case 714:
			case 716:
			case 718:
			case 720:
			case 722:
			case 724:
			case 726:
			case 728:
			case 730:
			case 732:
			case 734:
			case 736:
			case 738:
			case 740:
			case 742:
			case 744:
			case 746:
			case 748:
			case 750:
			case 752:
			case 754:
			case 756:
			case 758:
			case 760:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.BitVariables[num4++]);
				continue;
			case 763:
				DataLoader.WriteInt16(binaryWriter, (int)(tileVector.x * 213f));
				continue;
			case 765:
				DataLoader.WriteInt16(binaryWriter, (int)(tileVector.z * 213f));
				continue;
			case 769:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.DreamReturnLevel);
				continue;
			case 771:
				WriteGameOptions(binaryWriter);
				continue;
			case 774:
				DataLoader.WriteInt8(binaryWriter, (int)UWCharacter.Instance.ParalyzeTimer);
				continue;
			case 865:
			case 866:
			case 867:
			case 868:
			case 869:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.ArenaOpponents[i - 865]);
				continue;
			case 874:
				DataLoader.WriteInt8(binaryWriter, 0L);
				continue;
			case 875:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[0]);
				continue;
			case 876:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[1]);
				continue;
			case 877:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[2]);
				continue;
			case 879:
			case 880:
			case 881:
			case 882:
			case 883:
			case 884:
			case 885:
			case 886:
			case 887:
			case 888:
			case 889:
			case 890:
			case 891:
			case 892:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.x_clocks[1 + i - 879]);
				continue;
			case 931:
				WriteInventoryIndex(binaryWriter, array, 0);
				continue;
			case 933:
				WriteInventoryIndex(binaryWriter, array, 1);
				continue;
			case 935:
				WriteInventoryIndex(binaryWriter, array, 4);
				continue;
			case 937:
				WriteInventoryIndex(binaryWriter, array, 2);
				continue;
			case 939:
				WriteInventoryIndex(binaryWriter, array, 3);
				continue;
			case 941:
				WriteInventoryIndex(binaryWriter, array, 5);
				continue;
			case 943:
				WriteInventoryIndex(binaryWriter, array, 6);
				continue;
			case 945:
				WriteInventoryIndex(binaryWriter, array, 7);
				continue;
			case 947:
				WriteInventoryIndex(binaryWriter, array, 8);
				continue;
			case 949:
				WriteInventoryIndex(binaryWriter, array, 9);
				continue;
			case 951:
				WriteInventoryIndex(binaryWriter, array, 10);
				continue;
			case 953:
				WriteInventoryIndex(binaryWriter, array, 11);
				continue;
			case 955:
				WriteInventoryIndex(binaryWriter, array, 12);
				continue;
			case 957:
				WriteInventoryIndex(binaryWriter, array, 13);
				continue;
			case 959:
				WriteInventoryIndex(binaryWriter, array, 14);
				continue;
			case 961:
				WriteInventoryIndex(binaryWriter, array, 15);
				continue;
			case 963:
				WriteInventoryIndex(binaryWriter, array, 16);
				continue;
			case 965:
				WriteInventoryIndex(binaryWriter, array, 17);
				continue;
			case 967:
				WriteInventoryIndex(binaryWriter, array, 18);
				continue;
			case 904:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.CurVIT);
				continue;
			case 893:
				DataLoader.WriteInt8(binaryWriter, 243L);
				continue;
			case 894:
				DataLoader.WriteInt8(binaryWriter, NoOfInventoryItems + 1);
				continue;
			case 896:
				DataLoader.WriteInt8(binaryWriter, 127L);
				continue;
			case 897:
				DataLoader.WriteInt8(binaryWriter, 32L);
				continue;
			case 898:
				DataLoader.WriteInt8(binaryWriter, 96L);
				continue;
			case 899:
				DataLoader.WriteInt8(binaryWriter, 243L);
				continue;
			case 902:
				DataLoader.WriteInt8(binaryWriter, 64L);
				continue;
			case 467:
			case 507:
			case 509:
			case 511:
			case 513:
			case 515:
			case 517:
			case 519:
			case 521:
			case 523:
			case 525:
			case 527:
			case 529:
			case 531:
			case 533:
			case 535:
			case 537:
			case 539:
			case 541:
			case 543:
			case 545:
			case 547:
			case 549:
			case 551:
			case 553:
			case 555:
			case 557:
			case 559:
			case 561:
			case 563:
			case 565:
			case 567:
			case 569:
			case 571:
			case 573:
			case 575:
			case 577:
			case 579:
			case 581:
			case 583:
			case 585:
			case 587:
			case 589:
			case 591:
			case 593:
			case 595:
			case 597:
			case 599:
			case 601:
			case 603:
			case 605:
			case 607:
			case 609:
			case 611:
			case 613:
			case 615:
			case 617:
			case 619:
			case 621:
			case 623:
			case 625:
			case 627:
			case 629:
			case 631:
			case 633:
			case 635:
			case 637:
			case 639:
			case 641:
			case 643:
			case 645:
			case 647:
			case 649:
			case 651:
			case 653:
			case 655:
			case 657:
			case 659:
			case 661:
			case 663:
			case 665:
			case 667:
			case 669:
			case 671:
			case 673:
			case 675:
			case 677:
			case 679:
			case 681:
			case 683:
			case 685:
			case 687:
			case 689:
			case 691:
			case 693:
			case 695:
			case 697:
			case 699:
			case 701:
			case 703:
			case 705:
			case 707:
			case 709:
			case 711:
			case 713:
			case 715:
			case 717:
			case 719:
			case 721:
			case 723:
			case 725:
			case 727:
			case 729:
			case 731:
			case 733:
			case 735:
			case 737:
			case 739:
			case 741:
			case 743:
			case 745:
			case 747:
			case 749:
			case 751:
			case 753:
			case 755:
			case 757:
			case 759:
			case 761:
			case 764:
			case 766:
			case 932:
			case 934:
			case 936:
			case 938:
			case 940:
			case 942:
			case 944:
			case 946:
			case 948:
			case 950:
			case 952:
			case 954:
			case 956:
			case 958:
			case 960:
			case 962:
			case 964:
			case 966:
			case 968:
				continue;
			}
			switch (i)
			{
			case 75:
				if (NoOfInventoryItems > 0)
				{
					DataLoader.WriteInt8(binaryWriter, array.GetUpperBound(0) + 3 << 2);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, 0L);
				}
				break;
			case 77:
				DataLoader.WriteInt16(binaryWriter, UWCharacter.Instance.PlayerSkills.STR * 2 * 10);
				break;
			case 79:
				DataLoader.WriteInt32(binaryWriter, UWCharacter.Instance.EXP * 10);
				break;
			case 83:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.TrainingPoints);
				break;
			case 85:
				WritePosition(binaryWriter);
				break;
			case 95:
			{
				int num7 = UWCharacter.Instance.MoonGateLevel & 0xF;
				DataLoader.WriteInt8(binaryWriter, num7);
				break;
			}
			case 97:
				DataLoader.WriteInt8(binaryWriter, ((num & 3) << 5) | (UWCharacter.Instance.play_poison << 1));
				break;
			case 100:
			{
				int num6 = 0;
				if (Quest.instance.DreamPlantEaten)
				{
					num6 |= 1;
				}
				if (Quest.instance.InDreamWorld)
				{
					num6 |= 2;
				}
				if (Quest.instance.FightingInArena)
				{
					num6 |= 4;
				}
				DataLoader.WriteInt8(binaryWriter, num6);
				break;
			}
			case 102:
				WritePlayerClass(binaryWriter);
				break;
			case 103:
			case 107:
			case 111:
			case 115:
			case 119:
			case 123:
			case 127:
			case 131:
			case 135:
			case 139:
			case 143:
			case 147:
			case 151:
			case 155:
			case 159:
			case 163:
			case 167:
			case 171:
			case 175:
			case 179:
			case 183:
			case 187:
			case 191:
			case 195:
			case 199:
			case 203:
			case 207:
			case 211:
			case 215:
			case 219:
			case 223:
			case 227:
			{
				int num5 = 0;
				for (int j = 0; j < 4; j++)
				{
					num5 |= (Quest.instance.QuestVariables[num2 + j] & 1) << j;
				}
				num2 += 4;
				DataLoader.WriteInt8(binaryWriter, num5);
				break;
			}
			default:
				DataLoader.WriteInt8(binaryWriter, 0L);
				break;
			case 78:
			case 80:
			case 81:
			case 82:
			case 86:
			case 87:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
				break;
			}
		}
		WriteInventory(binaryWriter, array);
		binaryWriter.Close();
		char[] buffer;
		if (DataLoader.ReadStreamFile(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "playertmp.dat", out buffer))
		{
			char[] array2 = DecodeEncodeUW2PlayerDat(buffer, (byte)UWCharacter.Instance.XorKey);
			byte[] array3 = new byte[array2.GetUpperBound(0) + 1];
			for (long num8 = 0L; num8 <= array2.GetUpperBound(0); num8++)
			{
				array3[num8] = (byte)array2[num8];
			}
			File.WriteAllBytes(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "PLAYER.DAT", array3);
		}
	}

	private static char[] DecodeEncodeUW2PlayerDat(char[] pDat, byte MS)
	{
		int[] array = new int[80];
		MS += 7;
		for (int i = 0; i < 80; i++)
		{
			MS += 6;
			array[i] = MS;
		}
		for (int j = 0; j < 16; j++)
		{
			MS += 7;
			array[j * 5] = MS;
		}
		for (int k = 0; k < 4; k++)
		{
			MS += 41;
			array[k * 12] = MS;
		}
		for (int l = 0; l < 11; l++)
		{
			MS += 73;
			array[l * 7] = MS;
		}
		char[] array2 = new char[pDat.GetUpperBound(0) + 1];
		int num = 1;
		int m = 0;
		for (int n = 0; n <= 11; n++)
		{
			array2[num] = (char)(pDat[num] ^ array[0]);
			m++;
			for (int num2 = 1; num2 < 80; num2++)
			{
				if (m < 893)
				{
					array2[num2 + num] = (char)(((pDat[num2 + num] & 0xFFu) ^ (uint)((array2[num2 - 1 + num] & 0xFF) + (pDat[num2 - 1 + num] & 0xFF) + (array[num2] & 0xFF))) & 0xFFu);
					m++;
				}
			}
			num += 80;
		}
		for (; m <= pDat.GetUpperBound(0); m++)
		{
			array2[m] = pDat[m];
		}
		array2[0] = pDat[0];
		return array2;
	}

	private static void WriteInventoryIndex(BinaryWriter writer, string[] InventoryObjects, short slotIndex)
	{
		ObjectInteraction objectInteraction = null;
		objectInteraction = ((slotIndex > 10) ? UWCharacter.Instance.playerInventory.playerContainer.GetItemAt((short)(slotIndex - 11)) : UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(slotIndex));
		if (objectInteraction != null)
		{
			int num = Array.IndexOf(InventoryObjects, objectInteraction.name) + 1 << 6;
			DataLoader.WriteInt16(writer, num);
		}
		else
		{
			DataLoader.WriteInt16(writer, 0L);
		}
	}

	public static void LoadPlayerDatUW2(int slotNo)
	{
		UWCharacter.Instance.CharName = "";
		int num = 0;
		int num2 = 0;
		int num3 = 1;
		int[] ActiveEffectIds = new int[3];
		short[] ActiveEffectStability = new short[3];
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		ResetUI();
		UWCharacter.Instance.JustTeleported = true;
		UWCharacter.Instance.teleportedTimer = 0f;
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "PLAYER.DAT", out buffer))
		{
			return;
		}
		byte b = (byte)DataLoader.getValAtAddress(buffer, 0L, 8);
		UWCharacter.Instance.XorKey = b;
		char[] array = DecodeEncodeUW2PlayerDat(buffer, b);
		if (UWCharacter.Instance.decode)
		{
			byte[] array2 = new byte[array.GetUpperBound(0) + 1];
			for (long num9 = 0L; num9 <= array.GetUpperBound(0); num9++)
			{
				array2[num9] = (byte)array[num9];
			}
			File.WriteAllBytes(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "decode_" + slotNo + ".dat", array2);
		}
		if (UWCharacter.Instance.recode)
		{
			if (UWCharacter.Instance.recode_cheat)
			{
				for (int i = 31; i <= 53; i++)
				{
					array[i] = '\0';
				}
			}
			else
			{
				array[UWCharacter.Instance.IndexToRecode] = (char)UWCharacter.Instance.ValueToRecode;
			}
			char[] array3 = DecodeEncodeUW2PlayerDat(array, b);
			byte[] array4 = new byte[array3.GetUpperBound(0) + 1];
			for (long num10 = 0L; num10 <= array3.GetUpperBound(0); num10++)
			{
				array4[num10] = (byte)array3[num10];
			}
			File.WriteAllBytes(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "playerrecoded.dat", array4);
		}
		LoadName(array);
		LoadStats(array);
		num4 = LoadSpellEffects(array, ref ActiveEffectIds, ref ActiveEffectStability);
		LoadRunes(array);
		LoadPlayerClass(array, 102);
		LoadGameOptions(array, 771);
		for (int j = 77; j <= 930; j++)
		{
			switch (j)
			{
			case 231:
			case 232:
			case 233:
			case 234:
			case 235:
			case 236:
			case 237:
			case 238:
			case 239:
			case 240:
			case 241:
			case 242:
			case 243:
			case 244:
			case 245:
			case 246:
			case 247:
			case 248:
			case 249:
				Quest.instance.QuestVariables[j - 103] = (int)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 250:
			case 252:
			case 254:
			case 256:
			case 258:
			case 260:
			case 262:
			case 264:
			case 266:
			case 268:
			case 270:
			case 272:
			case 274:
			case 276:
			case 278:
			case 280:
			case 282:
			case 284:
			case 286:
			case 288:
			case 290:
			case 292:
			case 294:
			case 296:
			case 298:
			case 300:
			case 302:
			case 304:
			case 306:
			case 308:
			case 310:
			case 312:
			case 314:
			case 316:
			case 318:
			case 320:
			case 322:
			case 324:
			case 326:
			case 328:
			case 330:
			case 332:
			case 334:
			case 336:
			case 338:
			case 340:
			case 342:
			case 344:
			case 346:
			case 348:
			case 350:
			case 352:
			case 354:
			case 356:
			case 358:
			case 360:
			case 362:
			case 364:
			case 366:
			case 368:
			case 370:
			case 372:
			case 374:
			case 376:
			case 378:
			case 380:
			case 382:
			case 384:
			case 386:
			case 388:
			case 390:
			case 392:
			case 394:
			case 396:
			case 398:
			case 400:
			case 402:
			case 404:
			case 406:
			case 408:
			case 410:
			case 412:
			case 414:
			case 416:
			case 418:
			case 420:
			case 422:
			case 424:
			case 426:
			case 428:
			case 430:
			case 432:
			case 434:
			case 436:
			case 438:
			case 440:
			case 442:
			case 444:
			case 446:
			case 448:
			case 450:
			case 452:
			case 454:
			case 456:
			case 458:
			case 460:
			case 462:
			case 464:
			case 468:
			case 470:
			case 472:
			case 474:
			case 476:
			case 478:
			case 480:
			case 482:
			case 484:
			case 486:
			case 488:
			case 490:
			case 492:
			case 494:
			case 496:
			case 498:
			case 500:
			case 502:
			case 504:
				Quest.instance.variables[num6++] = (int)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 466:
				Quest.instance.variables[num6++] = (int)DataLoader.getValAtAddress(array, j, 16);
				continue;
			case 506:
			case 508:
			case 510:
			case 512:
			case 514:
			case 516:
			case 518:
			case 520:
			case 522:
			case 524:
			case 526:
			case 528:
			case 530:
			case 532:
			case 534:
			case 536:
			case 538:
			case 540:
			case 542:
			case 544:
			case 546:
			case 548:
			case 550:
			case 552:
			case 554:
			case 556:
			case 558:
			case 560:
			case 562:
			case 564:
			case 566:
			case 568:
			case 570:
			case 572:
			case 574:
			case 576:
			case 578:
			case 580:
			case 582:
			case 584:
			case 586:
			case 588:
			case 590:
			case 592:
			case 594:
			case 596:
			case 598:
			case 600:
			case 602:
			case 604:
			case 606:
			case 608:
			case 610:
			case 612:
			case 614:
			case 616:
			case 618:
			case 620:
			case 622:
			case 624:
			case 626:
			case 628:
			case 630:
			case 632:
			case 634:
			case 636:
			case 638:
			case 640:
			case 642:
			case 644:
			case 646:
			case 648:
			case 650:
			case 652:
			case 654:
			case 656:
			case 658:
			case 660:
			case 662:
			case 664:
			case 666:
			case 668:
			case 670:
			case 672:
			case 674:
			case 676:
			case 678:
			case 680:
			case 682:
			case 684:
			case 686:
			case 688:
			case 690:
			case 692:
			case 694:
			case 696:
			case 698:
			case 700:
			case 702:
			case 704:
			case 706:
			case 708:
			case 710:
			case 712:
			case 714:
			case 716:
			case 718:
			case 720:
			case 722:
			case 724:
			case 726:
			case 728:
			case 730:
			case 732:
			case 734:
			case 736:
			case 738:
			case 740:
			case 742:
			case 744:
			case 746:
			case 748:
			case 750:
			case 752:
			case 754:
			case 756:
			case 758:
			case 760:
				Quest.instance.BitVariables[num7++] = (int)DataLoader.getValAtAddress(array, j, 16);
				continue;
			case 763:
				num = (int)DataLoader.getValAtAddress(array, j, 16);
				continue;
			case 765:
				num2 = (int)DataLoader.getValAtAddress(array, j, 16);
				continue;
			case 769:
				UWCharacter.Instance.DreamReturnLevel = (short)(DataLoader.getValAtAddress(array, j, 8) - 1);
				continue;
			case 774:
				UWCharacter.Instance.ParalyzeTimer = (short)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 865:
			case 866:
			case 867:
			case 868:
			case 869:
				Quest.instance.ArenaOpponents[num8++] = (int)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 874:
				GameClock.instance.game_time = (int)DataLoader.getValAtAddress(array, j, 32);
				continue;
			case 875:
				GameClock.instance.gametimevals[0] = (int)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 876:
				GameClock.instance.gametimevals[1] = (int)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 877:
				GameClock.instance.gametimevals[2] = (int)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 879:
			case 880:
			case 881:
			case 882:
			case 883:
			case 884:
			case 885:
			case 886:
			case 887:
			case 888:
			case 889:
			case 890:
			case 891:
			case 892:
				Quest.instance.x_clocks[num3++] = (int)DataLoader.getValAtAddress(array, j, 8);
				continue;
			case 507:
			case 509:
			case 511:
			case 513:
			case 515:
			case 517:
			case 519:
			case 521:
			case 523:
			case 525:
			case 527:
			case 529:
			case 531:
			case 533:
			case 535:
			case 537:
			case 539:
			case 541:
			case 543:
			case 545:
			case 547:
			case 549:
			case 551:
			case 553:
			case 555:
			case 557:
			case 559:
			case 561:
			case 563:
			case 565:
			case 567:
			case 569:
			case 571:
			case 573:
			case 575:
			case 577:
			case 579:
			case 581:
			case 583:
			case 585:
			case 587:
			case 589:
			case 591:
			case 593:
			case 595:
			case 597:
			case 599:
			case 601:
			case 603:
			case 605:
			case 607:
			case 609:
			case 611:
			case 613:
			case 615:
			case 617:
			case 619:
			case 621:
			case 623:
			case 625:
			case 627:
			case 629:
			case 631:
			case 633:
			case 635:
			case 637:
			case 639:
			case 641:
			case 643:
			case 645:
			case 647:
			case 649:
			case 651:
			case 653:
			case 655:
			case 657:
			case 659:
			case 661:
			case 663:
			case 665:
			case 667:
			case 669:
			case 671:
			case 673:
			case 675:
			case 677:
			case 679:
			case 681:
			case 683:
			case 685:
			case 687:
			case 689:
			case 691:
			case 693:
			case 695:
			case 697:
			case 699:
			case 701:
			case 703:
			case 705:
			case 707:
			case 709:
			case 711:
			case 713:
			case 715:
			case 717:
			case 719:
			case 721:
			case 723:
			case 725:
			case 727:
			case 729:
			case 731:
			case 733:
			case 735:
			case 737:
			case 739:
			case 741:
			case 743:
			case 745:
			case 747:
			case 749:
			case 751:
			case 753:
			case 755:
			case 757:
			case 759:
			case 761:
			case 771:
				continue;
			}
			switch (j)
			{
			case 79:
				UWCharacter.Instance.EXP = (int)((float)DataLoader.getValAtAddress(array, j, 32) * 0.1f);
				break;
			case 83:
				UWCharacter.Instance.TrainingPoints = array[j];
				break;
			case 85:
				LoadPosition(array);
				break;
			case 95:
				UWCharacter.Instance.ResurrectLevel = (short)(((int)array[j] >> 4) & 0xF);
				UWCharacter.Instance.MoonGateLevel = (short)(array[j] & 0xF);
				break;
			case 97:
				UWCharacter.Instance.play_poison = (short)(((int)array[j] >> 1) & 0xF);
				UWCharacter.Instance.poison_timer = 30f;
				num4 = ((int)array[j] >> 6) & 3;
				break;
			case 100:
				Quest.instance.DreamPlantEaten = 1 == (array[j] & 1);
				Quest.instance.InDreamWorld = 1 == (((int)array[j] >> 1) & 1);
				Quest.instance.FightingInArena = 1 == (((int)array[j] >> 2) & 1);
				UWCharacter.Instance.DreamWorldTimer = 30f;
				break;
			case 103:
			case 107:
			case 111:
			case 115:
			case 119:
			case 123:
			case 127:
			case 131:
			case 135:
			case 139:
			case 143:
			case 147:
			case 151:
			case 155:
			case 159:
			case 163:
			case 167:
			case 171:
			case 175:
			case 179:
			case 183:
			case 187:
			case 191:
			case 195:
			case 199:
			case 203:
			case 207:
			case 211:
			case 215:
			case 219:
			case 223:
			case 227:
			{
				int num11 = (int)DataLoader.getValAtAddress(array, j, 8);
				for (int k = 0; k < 4; k++)
				{
					Quest.instance.QuestVariables[num5++] = (num11 >> k) & 1;
				}
				break;
			}
			}
		}
		ApplySpellEffects(ActiveEffectIds, ActiveEffectStability, num4);
		GameClock.setUWTime(GameClock.instance.gametimevals[0] + GameClock.instance.gametimevals[1] * 255 + GameClock.instance.gametimevals[2] * 255 * 255);
		ResetInventory();
		LoadInventory(array, 995, 931, 969);
		Vector3 vector = new Vector3((float)num / 213f, 0f, (float)num2 / 213f);
		UWCharacter.Instance.DreamReturnTileX = (short)(vector.x / 1.2f);
		UWCharacter.Instance.DreamReturnTileY = (short)(vector.z / 1.2f);
		UWCharacter.Instance.TeleportPosition = GameWorldController.instance.StartPos;
	}

	public static string SaveGameName(int slotNo)
	{
		if (UWClass._RES == "UW2")
		{
			return "Level " + GameWorldController.instance.LevelNo + " " + DateTime.Now;
		}
		return UW1LevelName(GameWorldController.instance.LevelNo) + " " + DateTime.Now;
	}

	private static string UW1LevelName(int levelNo)
	{
		return GameWorldController.UW1_LevelNames[levelNo];
	}

	private static void LoadName(char[] buffer)
	{
		UWCharacter.Instance.CharName = "";
		for (int i = 1; i < 14; i++)
		{
			if (buffer[i].ToString() != "\0")
			{
				UWCharacter.Instance.CharName += buffer[i];
			}
		}
	}

	private static void LoadStats(char[] buffer)
	{
		for (int i = 1; i <= 62; i++)
		{
			switch (i)
			{
			case 31:
				UWCharacter.Instance.PlayerSkills.STR = buffer[i];
				break;
			case 32:
				UWCharacter.Instance.PlayerSkills.DEX = buffer[i];
				break;
			case 33:
				UWCharacter.Instance.PlayerSkills.INT = buffer[i];
				break;
			case 34:
				UWCharacter.Instance.PlayerSkills.Attack = buffer[i];
				break;
			case 35:
				UWCharacter.Instance.PlayerSkills.Defense = buffer[i];
				break;
			case 36:
				UWCharacter.Instance.PlayerSkills.Unarmed = buffer[i];
				break;
			case 37:
				UWCharacter.Instance.PlayerSkills.Sword = buffer[i];
				break;
			case 38:
				UWCharacter.Instance.PlayerSkills.Axe = buffer[i];
				break;
			case 39:
				UWCharacter.Instance.PlayerSkills.Mace = buffer[i];
				break;
			case 40:
				UWCharacter.Instance.PlayerSkills.Missile = buffer[i];
				break;
			case 41:
				UWCharacter.Instance.PlayerSkills.ManaSkill = buffer[i];
				break;
			case 42:
				UWCharacter.Instance.PlayerSkills.Lore = buffer[i];
				break;
			case 43:
				UWCharacter.Instance.PlayerSkills.Casting = buffer[i];
				break;
			case 44:
				UWCharacter.Instance.PlayerSkills.Traps = buffer[i];
				break;
			case 45:
				UWCharacter.Instance.PlayerSkills.Search = buffer[i];
				break;
			case 46:
				UWCharacter.Instance.PlayerSkills.Track = buffer[i];
				break;
			case 47:
				UWCharacter.Instance.PlayerSkills.Sneak = buffer[i];
				break;
			case 48:
				UWCharacter.Instance.PlayerSkills.Repair = buffer[i];
				break;
			case 49:
				UWCharacter.Instance.PlayerSkills.Charm = buffer[i];
				break;
			case 50:
				UWCharacter.Instance.PlayerSkills.PickLock = buffer[i];
				break;
			case 51:
				UWCharacter.Instance.PlayerSkills.Acrobat = buffer[i];
				break;
			case 52:
				UWCharacter.Instance.PlayerSkills.Appraise = buffer[i];
				break;
			case 53:
				UWCharacter.Instance.PlayerSkills.Swimming = buffer[i];
				break;
			case 54:
				UWCharacter.Instance.CurVIT = buffer[i];
				break;
			case 55:
				UWCharacter.Instance.MaxVIT = buffer[i];
				break;
			case 56:
				UWCharacter.Instance.PlayerMagic.CurMana = buffer[i];
				break;
			case 57:
				UWCharacter.Instance.PlayerMagic.MaxMana = buffer[i];
				break;
			case 58:
				UWCharacter.Instance.FoodLevel = buffer[i];
				break;
			case 62:
				UWCharacter.Instance.CharLevel = buffer[i];
				break;
			}
		}
	}

	private static void InitPlayerPosition(int x_position, int y_position, int z_position)
	{
		GameWorldController.instance.StartPos = new Vector3((float)x_position / 213f, (float)z_position / 213f + 0.3543672f, (float)y_position / 213f);
	}

	private static int LoadSpellEffects(char[] buffer, ref int[] ActiveEffectIds, ref short[] ActiveEffectStability)
	{
		int num = 0;
		for (int i = 63; i <= 68; i++)
		{
			switch (i)
			{
			case 63:
			case 65:
			case 67:
				ActiveEffectIds[num] = GetActiveSpellID(buffer[i]);
				break;
			case 64:
			case 66:
			case 68:
				ActiveEffectStability[num++] = (short)buffer[i];
				break;
			}
		}
		return num;
	}

	private static void LoadRunes(char[] buffer)
	{
		int num = 69;
		int num2 = 0;
		for (int i = 0; i < 3; i++)
		{
			for (int num3 = 7; num3 >= 0; num3--)
			{
				if ((((int)buffer[num + i] >> num3) & 1) == 1)
				{
					UWCharacter.Instance.PlayerMagic.PlayerRunes[7 - num3 + num2] = true;
				}
				else
				{
					UWCharacter.Instance.PlayerMagic.PlayerRunes[7 - num3 + num2] = false;
				}
			}
			num2 += 8;
		}
		SetActiveRuneSlots(0, buffer[72]);
		SetActiveRuneSlots(1, buffer[73]);
		SetActiveRuneSlots(2, buffer[74]);
	}

	private static void LoadPlayerClass(char[] buffer, int i)
	{
		GRLoader gRLoader = new GRLoader(4);
		UWCharacter.Instance.isLefty = (buffer[i] & 1) == 0;
		int num = ((int)buffer[i] >> 1) & 0xF;
		if (num % 2 == 0)
		{
			UWCharacter.Instance.isFemale = false;
			UWCharacter.Instance.Body = num / 2;
			UWHUD.instance.playerBody.texture = gRLoader.LoadImageAt(num / 2);
		}
		else
		{
			UWCharacter.Instance.isFemale = true;
			UWCharacter.Instance.Body = (num - 1) / 2;
			UWHUD.instance.playerBody.texture = gRLoader.LoadImageAt(5 + (num - 1) / 2);
		}
		UWCharacter.Instance.CharClass = (int)buffer[i] >> 5;
	}

	private static void LoadGameOptions(char[] buffer, int i)
	{
		int num = (int)DataLoader.getValAtAddress(buffer, i, 8);
		ObjectInteraction.PlaySoundEffects = (num & 1) == 1;
		MusicController.PlayMusic = ((num >> 2) & 1) == 1;
		GameWorldController.instance.difficulty = (int)DataLoader.getValAtAddress(buffer, i - 1, 8) & 1;
	}

	private static void ApplySpellEffects(int[] ActiveEffectIds, short[] ActiveEffectStability, int effectCounter)
	{
		for (int i = 0; i <= UWCharacter.Instance.ActiveSpell.GetUpperBound(0); i++)
		{
			if (UWCharacter.Instance.ActiveSpell[i] != null)
			{
				UWCharacter.Instance.ActiveSpell[i].CancelEffect();
				if (UWCharacter.Instance.ActiveSpell[i] != null)
				{
					UWCharacter.Instance.ActiveSpell[i].CancelEffect();
				}
			}
		}
		for (int j = 0; j <= UWCharacter.Instance.PassiveSpell.GetUpperBound(0); j++)
		{
			if (UWCharacter.Instance.PassiveSpell[j] != null)
			{
				UWCharacter.Instance.PassiveSpell[j].CancelEffect();
				if (UWCharacter.Instance.PassiveSpell[j] != null)
				{
					UWCharacter.Instance.PassiveSpell[j].CancelEffect();
				}
			}
		}
		for (int k = 0; k < effectCounter; k++)
		{
			UWCharacter.Instance.ActiveSpell[k] = UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, ActiveEffectIds[k], 1, 2);
			if (UWCharacter.Instance.ActiveSpell[k] != null)
			{
				UWCharacter.Instance.ActiveSpell[k].counter = ActiveEffectStability[k];
			}
		}
	}

	private static void ResetInventory()
	{
		UWCharacter.Instance.playerInventory.sHelm = null;
		UWCharacter.Instance.playerInventory.sChest = null;
		UWCharacter.Instance.playerInventory.sGloves = null;
		UWCharacter.Instance.playerInventory.sLegs = null;
		UWCharacter.Instance.playerInventory.sBoots = null;
		UWCharacter.Instance.playerInventory.sRightShoulder = null;
		UWCharacter.Instance.playerInventory.sLeftShoulder = null;
		UWCharacter.Instance.playerInventory.sRightHand = null;
		UWCharacter.Instance.playerInventory.sLeftHand = null;
		UWCharacter.Instance.playerInventory.sRightRing = null;
		UWCharacter.Instance.playerInventory.sLeftRing = null;
		for (int i = 0; i <= UWCharacter.Instance.playerInventory.playerContainer.items.GetUpperBound(0); i++)
		{
			UWCharacter.Instance.playerInventory.playerContainer.items[i] = null;
		}
		foreach (Transform item in GameWorldController.instance.InventoryMarker.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	private static void LoadInventory(char[] buffer, int StartOffset, int lBoundSlots, int uBoundSlots)
	{
		int num = (buffer.GetUpperBound(0) - StartOffset) / 8;
		GameWorldController.instance.inventoryLoader.objInfo = new ObjectLoaderInfo[num + 2];
		int num2 = 1;
		if (buffer.GetUpperBound(0) < StartOffset)
		{
			return;
		}
		int num3 = StartOffset;
		while (num3 < buffer.GetUpperBound(0))
		{
			GameWorldController.instance.inventoryLoader.objInfo[num2] = new ObjectLoaderInfo();
			GameWorldController.instance.inventoryLoader.objInfo[num2].index = num2;
			GameWorldController.instance.inventoryLoader.objInfo[num2].guid = Guid.NewGuid();
			GameWorldController.instance.inventoryLoader.objInfo[num2].parentList = GameWorldController.instance.inventoryLoader;
			GameWorldController.instance.inventoryLoader.objInfo[num2].ObjectTileX = 99;
			GameWorldController.instance.inventoryLoader.objInfo[num2].ObjectTileY = 99;
			GameWorldController.instance.inventoryLoader.objInfo[num2].InUseFlag = 1;
			GameWorldController.instance.inventoryLoader.objInfo[num2].item_id = (int)DataLoader.getValAtAddress(buffer, num3, 16) & 0x1FF;
			GameWorldController.instance.inventoryLoader.objInfo[num2].flags = (short)((DataLoader.getValAtAddress(buffer, num3, 16) >> 9) & 7);
			GameWorldController.instance.inventoryLoader.objInfo[num2].enchantment = (short)((DataLoader.getValAtAddress(buffer, num3, 16) >> 12) & 1);
			GameWorldController.instance.inventoryLoader.objInfo[num2].doordir = (short)((DataLoader.getValAtAddress(buffer, num3, 16) >> 13) & 1);
			GameWorldController.instance.inventoryLoader.objInfo[num2].invis = (short)((DataLoader.getValAtAddress(buffer, num3, 16) >> 14) & 1);
			GameWorldController.instance.inventoryLoader.objInfo[num2].is_quant = (short)((DataLoader.getValAtAddress(buffer, num3, 16) >> 15) & 1);
			GameWorldController.instance.inventoryLoader.objInfo[num2].zpos = (short)(DataLoader.getValAtAddress(buffer, num3 + 2, 16) & 0x7F);
			GameWorldController.instance.inventoryLoader.objInfo[num2].heading = (short)((DataLoader.getValAtAddress(buffer, num3 + 2, 16) >> 7) & 7);
			GameWorldController.instance.inventoryLoader.objInfo[num2].ypos = (short)((DataLoader.getValAtAddress(buffer, num3 + 2, 16) >> 10) & 7);
			GameWorldController.instance.inventoryLoader.objInfo[num2].xpos = (short)((DataLoader.getValAtAddress(buffer, num3 + 2, 16) >> 13) & 7);
			GameWorldController.instance.inventoryLoader.objInfo[num2].quality = (short)(DataLoader.getValAtAddress(buffer, num3 + 4, 16) & 0x3F);
			GameWorldController.instance.inventoryLoader.objInfo[num2].next = (int)((DataLoader.getValAtAddress(buffer, num3 + 4, 16) >> 6) & 0x3FF);
			GameWorldController.instance.inventoryLoader.objInfo[num2].owner = (short)(DataLoader.getValAtAddress(buffer, num3 + 6, 16) & 0x3F);
			GameWorldController.instance.inventoryLoader.objInfo[num2].link = (int)((DataLoader.getValAtAddress(buffer, num3 + 6, 16) >> 6) & 0x3FF);
			num3 += 8;
			num2++;
		}
		ObjectLoader.RenderObjectList(GameWorldController.instance.inventoryLoader, UWClass.CurrentTileMap(), GameWorldController.instance.InventoryMarker);
		ObjectLoader.LinkObjectListWands(GameWorldController.instance.inventoryLoader);
		ObjectLoader.LinkObjectListPotions(GameWorldController.instance.inventoryLoader);
		for (int i = lBoundSlots; i < uBoundSlots; i += 2)
		{
			int num4 = (int)DataLoader.getValAtAddress(buffer, i, 16) >> 6;
			ObjectInteraction objectInteraction = ((num4 == 0) ? null : GameWorldController.instance.inventoryLoader.objInfo[num4].instance);
			switch ((InventorySlotsOffsets)i)
			{
			case InventorySlotsOffsets.UW1Helm:
			case InventorySlotsOffsets.UW2Helm:
				UWCharacter.Instance.playerInventory.sHelm = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Chest:
			case InventorySlotsOffsets.UW2Chest:
				UWCharacter.Instance.playerInventory.sChest = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Gloves:
			case InventorySlotsOffsets.UW2Gloves:
				UWCharacter.Instance.playerInventory.sGloves = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Leggings:
			case InventorySlotsOffsets.UW2Leggings:
				UWCharacter.Instance.playerInventory.sLegs = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Boots:
			case InventorySlotsOffsets.UW2Boots:
				UWCharacter.Instance.playerInventory.sBoots = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1RightShoulder:
			case InventorySlotsOffsets.UW2RightShoulder:
				UWCharacter.Instance.playerInventory.sRightShoulder = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1LeftShoulder:
			case InventorySlotsOffsets.UW2LeftShoulder:
				UWCharacter.Instance.playerInventory.sLeftShoulder = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1RightHand:
			case InventorySlotsOffsets.UW2RightHand:
				UWCharacter.Instance.playerInventory.sRightHand = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1LeftHand:
			case InventorySlotsOffsets.UW2LeftHand:
				UWCharacter.Instance.playerInventory.sLeftHand = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1RightRing:
			case InventorySlotsOffsets.UW2RightRing:
				UWCharacter.Instance.playerInventory.sRightRing = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1LeftRing:
			case InventorySlotsOffsets.UW2LeftRing:
				UWCharacter.Instance.playerInventory.sLeftRing = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack0:
			case InventorySlotsOffsets.UW2Backpack0:
				UWCharacter.Instance.playerInventory.playerContainer.items[0] = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack1:
			case InventorySlotsOffsets.UW2Backpack1:
				UWCharacter.Instance.playerInventory.playerContainer.items[1] = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack2:
			case InventorySlotsOffsets.UW2Backpack2:
				UWCharacter.Instance.playerInventory.playerContainer.items[2] = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack3:
			case InventorySlotsOffsets.UW2Backpack3:
				UWCharacter.Instance.playerInventory.playerContainer.items[3] = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack4:
			case InventorySlotsOffsets.UW2Backpack4:
				UWCharacter.Instance.playerInventory.playerContainer.items[4] = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack5:
			case InventorySlotsOffsets.UW2Backpack5:
				UWCharacter.Instance.playerInventory.playerContainer.items[5] = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack6:
			case InventorySlotsOffsets.UW2Backpack6:
				UWCharacter.Instance.playerInventory.playerContainer.items[6] = objectInteraction;
				break;
			case InventorySlotsOffsets.UW1Backpack7:
			case InventorySlotsOffsets.UW2Backpack7:
				UWCharacter.Instance.playerInventory.playerContainer.items[7] = objectInteraction;
				break;
			}
		}
		UWCharacter.Instance.playerInventory.Refresh();
		for (short num5 = 0; num5 <= 10; num5++)
		{
			ObjectInteraction objectIntAtSlot = UWCharacter.Instance.playerInventory.GetObjectIntAtSlot(num5);
			if (objectIntAtSlot != null)
			{
				objectIntAtSlot.Equip(num5);
			}
		}
	}

	private static void ResetUI()
	{
		UWCharacter.Instance.playerInventory.currentContainer = "_Gronk";
		UWHUD.instance.ContainerOpened.GetComponent<RawImage>().texture = UWCharacter.Instance.playerInventory.Blank;
		UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().BackpackBg.SetActive(false);
		UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().InvUp.SetActive(false);
		UWHUD.instance.ContainerOpened.GetComponent<ContainerOpened>().InvDown.SetActive(false);
		UWCharacter.Instance.playerInventory.ContainerOffset = 0;
	}

	private static void WriteInventory(BinaryWriter writer, string[] inventoryObjects)
	{
		for (int i = 0; i <= inventoryObjects.GetUpperBound(0); i++)
		{
			GameObject gameObject = GameObject.Find(inventoryObjects[i]);
			if (!(gameObject != null))
			{
				continue;
			}
			ObjectInteraction component = gameObject.GetComponent<ObjectInteraction>();
			if (component.GetItemType() == 12)
			{
				if (component.GetComponent<Wand>() != null && component.GetComponent<Wand>().linkedspell != null)
				{
					string name = component.GetComponent<Wand>().linkedspell.name;
					for (int j = 0; j <= inventoryObjects.GetUpperBound(0); j++)
					{
						if (name == inventoryObjects[j])
						{
							component.link = j + 1;
							break;
						}
					}
				}
				if (component.GetComponent<Potion>() != null && component.GetComponent<Potion>().linked != null)
				{
					string name2 = component.GetComponent<Potion>().linked.name;
					for (int k = 0; k <= inventoryObjects.GetUpperBound(0); k++)
					{
						if (name2 == inventoryObjects[k])
						{
							component.link = k + 1;
							break;
						}
					}
				}
			}
			int num = (component.isquant << 15) | (component.invis << 14) | (component.doordir << 13) | (component.enchantment << 12) | ((component.flags & 7) << 9) | (component.item_id & 0x1FF);
			DataLoader.WriteInt8(writer, num & 0xFF);
			DataLoader.WriteInt8(writer, (num >> 8) & 0xFF);
			num = ((component.xpos & 7) << 13) | ((component.ypos & 7) << 10) | ((component.heading & 7) << 7) | (component.zpos & 0x7F);
			DataLoader.WriteInt8(writer, num & 0xFF);
			DataLoader.WriteInt8(writer, (num >> 8) & 0xFF);
			num = ((component.next & 0x3FF) << 6) | (component.quality & 0x3F);
			DataLoader.WriteInt8(writer, num & 0xFF);
			DataLoader.WriteInt8(writer, (num >> 8) & 0xFF);
			num = ((component.link & 0x3FF) << 6) | (component.owner & 0x3F);
			DataLoader.WriteInt8(writer, num & 0xFF);
			DataLoader.WriteInt8(writer, (num >> 8) & 0xFF);
		}
	}

	private static void WriteName(BinaryWriter writer)
	{
		for (int i = 1; i < 14; i++)
		{
			if (i - 1 < UWCharacter.Instance.CharName.Length)
			{
				char c = UWCharacter.Instance.CharName.ToCharArray()[i - 1];
				DataLoader.WriteInt8(writer, (int)c);
			}
			else
			{
				DataLoader.WriteInt8(writer, 0L);
			}
		}
	}

	private static void WriteSkills(BinaryWriter writer)
	{
		for (int i = 31; i <= 62; i++)
		{
			switch (i)
			{
			case 31:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.STR);
				break;
			case 32:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.DEX);
				break;
			case 33:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.INT);
				break;
			case 34:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Attack);
				break;
			case 35:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Defense);
				break;
			case 36:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Unarmed);
				break;
			case 37:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Sword);
				break;
			case 38:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Axe);
				break;
			case 39:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Mace);
				break;
			case 40:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Missile);
				break;
			case 41:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.ManaSkill);
				break;
			case 42:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Lore);
				break;
			case 43:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Casting);
				break;
			case 44:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Traps);
				break;
			case 45:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Search);
				break;
			case 46:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Track);
				break;
			case 47:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Sneak);
				break;
			case 48:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Repair);
				break;
			case 49:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Charm);
				break;
			case 50:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.PickLock);
				break;
			case 51:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Acrobat);
				break;
			case 52:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Appraise);
				break;
			case 53:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerSkills.Swimming);
				break;
			case 54:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.CurVIT);
				break;
			case 55:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.MaxVIT);
				break;
			case 56:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerMagic.CurMana);
				break;
			case 57:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerMagic.MaxMana);
				break;
			case 58:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.FoodLevel);
				break;
			case 59:
			case 60:
				DataLoader.WriteInt8(writer, 64L);
				break;
			case 61:
				DataLoader.WriteInt8(writer, 0L);
				break;
			case 62:
				DataLoader.WriteInt8(writer, UWCharacter.Instance.CharLevel);
				break;
			default:
				DataLoader.WriteInt8(writer, 0L);
				Debug.Log("THis should not happen. Writeskills  " + i);
				break;
			}
		}
	}

	private static void WriteSpace(BinaryWriter writer, int noOfSpaces)
	{
		for (int i = 0; i < noOfSpaces; i++)
		{
			DataLoader.WriteInt8(writer, 0L);
		}
	}

	private static void WriteRunes(BinaryWriter writer)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			int num2 = 0;
			for (int num3 = 7; num3 >= 0; num3--)
			{
				if (UWCharacter.Instance.PlayerMagic.PlayerRunes[7 - num3 + num])
				{
					num2 |= 1 << num3;
				}
			}
			DataLoader.WriteInt8(writer, num2);
			num += 8;
		}
		for (int j = 0; j < 3; j++)
		{
			if (UWCharacter.Instance.PlayerMagic.ActiveRunes[j] == -1)
			{
				DataLoader.WriteInt8(writer, 24L);
			}
			else
			{
				DataLoader.WriteInt8(writer, UWCharacter.Instance.PlayerMagic.ActiveRunes[j]);
			}
		}
	}

	private static int WriteSpellEffects(BinaryWriter writer)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (UWCharacter.Instance.ActiveSpell[i] != null)
			{
				int num2 = 0;
				int num3 = 0;
				switch (UWCharacter.Instance.ActiveSpell[i].EffectID)
				{
				case 176:
					num2 = 178;
					break;
				case 184:
					num2 = 179;
					break;
				case 187:
					num2 = 176;
					break;
				case 183:
					num2 = 183;
					break;
				default:
					num2 = UWCharacter.Instance.ActiveSpell[i].EffectID;
					break;
				}
				int counter = UWCharacter.Instance.ActiveSpell[i].counter;
				num3 = (counter << 8) | ((num2 & 0xF0) >> 4) | ((num2 & 0xF) << 4);
				DataLoader.WriteInt16(writer, num3);
				num++;
			}
			else
			{
				DataLoader.WriteInt16(writer, 0L);
			}
		}
		return num;
	}

	private static void WritePlayerClass(BinaryWriter writer)
	{
		int num = 0;
		if (!UWCharacter.Instance.isLefty)
		{
			num |= 1;
		}
		num = ((!UWCharacter.Instance.isFemale) ? (num | (UWCharacter.Instance.Body * 2 << 1)) : (num | (UWCharacter.Instance.Body * 2 + 1 << 1)));
		num |= UWCharacter.Instance.CharClass << 5;
		DataLoader.WriteInt8(writer, num);
	}

	private static void WriteUW1QuestFlags(BinaryWriter writer)
	{
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			num |= (Quest.instance.QuestVariables[i] & 1) << i;
		}
		DataLoader.WriteInt32(writer, num);
	}

	private static void WriteGameOptions(BinaryWriter writer)
	{
		int num = 48;
		if (ObjectInteraction.PlaySoundEffects)
		{
			num |= 1;
		}
		if (MusicController.PlayMusic)
		{
			num |= 4;
		}
		DataLoader.WriteInt8(writer, num);
	}

	private static void LoadPosition(char[] buffer)
	{
		int x_position = (int)DataLoader.getValAtAddress(buffer, 85L, 16);
		int y_position = (int)DataLoader.getValAtAddress(buffer, 87L, 16);
		int z_position = (int)DataLoader.getValAtAddress(buffer, 89L, 16);
		float num = DataLoader.getValAtAddress(buffer, 92L, 8);
		UWCharacter.Instance.transform.eulerAngles = new Vector3(0f, num * 1.4117647f, 0f);
		UWCharacter.Instance.playerCam.transform.localRotation = Quaternion.identity;
		GameWorldController.instance.startLevel = (short)(DataLoader.getValAtAddress(buffer, 93L, 16) - 1);
		InitPlayerPosition(x_position, y_position, z_position);
	}

	private static void WritePosition(BinaryWriter writer)
	{
		DataLoader.WriteInt16(writer, (int)(UWCharacter.Instance.transform.position.x * 213f));
		DataLoader.WriteInt16(writer, (int)(UWCharacter.Instance.transform.position.z * 213f));
		DataLoader.WriteInt16(writer, (int)((UWCharacter.Instance.transform.position.y - 0.3543672f) * 213f));
		DataLoader.WriteInt8(writer, 0L);
		DataLoader.WriteInt8(writer, (int)(UWCharacter.Instance.transform.eulerAngles.y * (17f / 24f)));
		DataLoader.WriteInt8(writer, GameWorldController.instance.LevelNo + 1);
	}

	public static void WritePlayerDatOriginal(int slotNo)
	{
		FileStream output = File.Open(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "playertmp.dat", FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(output);
		int num = 0;
		int num2 = 0;
		int NoOfInventoryItems = 0;
		string[] array = ObjectLoader.UpdateInventoryObjectList(out NoOfInventoryItems);
		DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.XorKey);
		for (int i = 1; i < 312; i++)
		{
			if (i < 14)
			{
				if (i - 1 < UWCharacter.Instance.CharName.Length)
				{
					char c = UWCharacter.Instance.CharName.ToCharArray()[i - 1];
					DataLoader.WriteInt8(binaryWriter, (int)c);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, 0L);
				}
				continue;
			}
			switch (i)
			{
			case 31:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.STR);
				break;
			case 32:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.DEX);
				break;
			case 33:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.INT);
				break;
			case 34:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Attack);
				break;
			case 35:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Defense);
				break;
			case 36:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Unarmed);
				break;
			case 37:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Sword);
				break;
			case 38:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Axe);
				break;
			case 39:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Mace);
				break;
			case 40:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Missile);
				break;
			case 41:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.ManaSkill);
				break;
			case 42:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Lore);
				break;
			case 43:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Casting);
				break;
			case 44:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Traps);
				break;
			case 45:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Search);
				break;
			case 46:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Track);
				break;
			case 47:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Sneak);
				break;
			case 48:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Repair);
				break;
			case 49:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Charm);
				break;
			case 50:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.PickLock);
				break;
			case 51:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Acrobat);
				break;
			case 52:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Appraise);
				break;
			case 53:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerSkills.Swimming);
				break;
			case 54:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.CurVIT);
				break;
			case 55:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.MaxVIT);
				break;
			case 56:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerMagic.CurMana);
				break;
			case 57:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerMagic.MaxMana);
				break;
			case 58:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.FoodLevel);
				break;
			case 59:
			case 60:
				DataLoader.WriteInt8(binaryWriter, 64L);
				break;
			case 62:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.CharLevel);
				break;
			case 63:
			{
				for (int k = 0; k < 3; k++)
				{
					if (UWCharacter.Instance.ActiveSpell[k] != null)
					{
						int num14 = 0;
						int num15 = 0;
						switch (UWCharacter.Instance.ActiveSpell[k].EffectID)
						{
						case 176:
							num14 = 178;
							break;
						case 184:
							num14 = 179;
							break;
						case 187:
							num14 = 176;
							break;
						case 183:
							num14 = 183;
							break;
						default:
							num14 = UWCharacter.Instance.ActiveSpell[k].EffectID;
							break;
						}
						int counter = UWCharacter.Instance.ActiveSpell[k].counter;
						num15 = (counter << 8) | ((num14 & 0xF0) >> 4) | ((num14 & 0xF) << 4);
						DataLoader.WriteInt16(binaryWriter, num15);
						num++;
					}
					else
					{
						DataLoader.WriteInt16(binaryWriter, 0L);
					}
				}
				break;
			}
			case 69:
			case 70:
			case 71:
			{
				int num12 = 0;
				for (int num13 = 7; num13 >= 0; num13--)
				{
					if (UWCharacter.Instance.PlayerMagic.PlayerRunes[7 - num13 + num2])
					{
						num12 |= 1 << num13;
					}
				}
				DataLoader.WriteInt8(binaryWriter, num12);
				num2 += 8;
				break;
			}
			case 72:
				if (UWCharacter.Instance.PlayerMagic.ActiveRunes[0] == -1)
				{
					DataLoader.WriteInt8(binaryWriter, 24L);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerMagic.ActiveRunes[0]);
				}
				break;
			case 73:
				if (UWCharacter.Instance.PlayerMagic.ActiveRunes[1] == -1)
				{
					DataLoader.WriteInt8(binaryWriter, 24L);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerMagic.ActiveRunes[1]);
				}
				break;
			case 74:
				if (UWCharacter.Instance.PlayerMagic.ActiveRunes[2] == -1)
				{
					DataLoader.WriteInt8(binaryWriter, 24L);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerMagic.ActiveRunes[2]);
				}
				break;
			case 75:
				DataLoader.WriteInt8(binaryWriter, array.GetUpperBound(0) + 1 << 2);
				break;
			case 77:
				DataLoader.WriteInt16(binaryWriter, UWCharacter.Instance.PlayerSkills.STR * 2 * 10);
				break;
			case 79:
				DataLoader.WriteInt32(binaryWriter, UWCharacter.Instance.EXP);
				break;
			case 83:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.TrainingPoints);
				break;
			case 85:
			{
				int num11 = (int)(UWCharacter.Instance.transform.position.x * 213f);
				DataLoader.WriteInt16(binaryWriter, num11);
				break;
			}
			case 87:
			{
				int num10 = (int)(UWCharacter.Instance.transform.position.z * 213f);
				DataLoader.WriteInt16(binaryWriter, num10);
				break;
			}
			case 89:
			{
				int num9 = (int)((UWCharacter.Instance.transform.position.y - 0.3543672f) * 213f);
				DataLoader.WriteInt16(binaryWriter, num9);
				break;
			}
			case 92:
			{
				float num8 = UWCharacter.Instance.transform.eulerAngles.y * (17f / 24f);
				DataLoader.WriteInt8(binaryWriter, (int)num8);
				break;
			}
			case 93:
				DataLoader.WriteInt8(binaryWriter, GameWorldController.instance.LevelNo + 1);
				break;
			case 95:
			{
				int num7 = ((UWCharacter.Instance.ResurrectLevel & 0xF) << 4) | (UWCharacter.Instance.MoonGateLevel & 0xF);
				DataLoader.WriteInt8(binaryWriter, num7);
				break;
			}
			case 96:
				DataLoader.WriteInt8(binaryWriter, ((num & 3) << 6) | (UWCharacter.Instance.play_poison << 2) | (Quest.instance.IncenseDream & 3));
				break;
			case 97:
			{
				int num6 = 0;
				if (Quest.instance.isOrbDestroyed)
				{
					num6 = 32;
				}
				if (Quest.instance.isCupFound)
				{
					num6 |= 0x40;
				}
				DataLoader.WriteInt8(binaryWriter, num6);
				break;
			}
			case 99:
				if (Quest.instance.isGaramonBuried)
				{
					DataLoader.WriteInt8(binaryWriter, 28L);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, 16L);
				}
				break;
			case 101:
			{
				int num5 = 0;
				if (!UWCharacter.Instance.isLefty)
				{
					num5 |= 1;
				}
				num5 = ((!UWCharacter.Instance.isFemale) ? (num5 | (UWCharacter.Instance.Body * 2 << 1)) : (num5 | (UWCharacter.Instance.Body * 2 + 1 << 1)));
				num5 |= UWCharacter.Instance.CharClass << 5;
				DataLoader.WriteInt8(binaryWriter, num5);
				break;
			}
			case 102:
			{
				int num4 = 0;
				for (int j = 0; j < 32; j++)
				{
					num4 |= (Quest.instance.QuestVariables[j] & 1) << j;
				}
				DataLoader.WriteInt32(binaryWriter, num4);
				break;
			}
			case 106:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[32]);
				break;
			case 107:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[33]);
				break;
			case 108:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[34]);
				break;
			case 109:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.QuestVariables[35]);
				break;
			case 110:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.TalismansRemaining);
				break;
			case 111:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.GaramonDream);
				break;
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 124:
			case 125:
			case 126:
			case 127:
			case 128:
			case 129:
			case 130:
			case 131:
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 140:
			case 141:
			case 142:
			case 143:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 170:
			case 171:
			case 172:
			case 173:
			case 174:
			case 175:
			case 176:
				DataLoader.WriteInt8(binaryWriter, Quest.instance.variables[i - 113]);
				break;
			case 177:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.PlayerMagic.TrueMaxMana);
				break;
			case 188:
				DataLoader.WriteInt8(binaryWriter, 255L);
				break;
			case 181:
				DataLoader.WriteInt8(binaryWriter, GameWorldController.instance.difficulty);
				break;
			case 182:
			{
				int num3 = 48;
				if (ObjectInteraction.PlaySoundEffects)
				{
					num3 |= 1;
				}
				if (MusicController.PlayMusic)
				{
					num3 |= 4;
				}
				DataLoader.WriteInt8(binaryWriter, num3);
				break;
			}
			case 183:
				DataLoader.WriteInt8(binaryWriter, 8L);
				break;
			case 207:
				DataLoader.WriteInt8(binaryWriter, 0L);
				break;
			case 208:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[0]);
				break;
			case 209:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[1]);
				break;
			case 210:
				DataLoader.WriteInt8(binaryWriter, GameClock.instance.gametimevals[2]);
				break;
			case 211:
				DataLoader.WriteInt16(binaryWriter, array.GetUpperBound(0) + 1 + 1);
				break;
			case 213:
				DataLoader.WriteInt8(binaryWriter, 127L);
				break;
			case 214:
				DataLoader.WriteInt8(binaryWriter, 32L);
				break;
			case 219:
				if (GameWorldController.instance.InventoryMarker.transform.childCount > 0)
				{
					DataLoader.WriteInt8(binaryWriter, 64L);
				}
				else
				{
					DataLoader.WriteInt8(binaryWriter, 0L);
				}
				break;
			case 221:
				DataLoader.WriteInt8(binaryWriter, UWCharacter.Instance.CurVIT);
				break;
			case 248:
				WriteInventoryIndex(binaryWriter, array, 0);
				break;
			case 250:
				WriteInventoryIndex(binaryWriter, array, 1);
				break;
			case 252:
				WriteInventoryIndex(binaryWriter, array, 4);
				break;
			case 254:
				WriteInventoryIndex(binaryWriter, array, 2);
				break;
			case 256:
				WriteInventoryIndex(binaryWriter, array, 3);
				break;
			case 258:
				WriteInventoryIndex(binaryWriter, array, 5);
				break;
			case 260:
				WriteInventoryIndex(binaryWriter, array, 6);
				break;
			case 262:
				WriteInventoryIndex(binaryWriter, array, 7);
				break;
			case 264:
				WriteInventoryIndex(binaryWriter, array, 8);
				break;
			case 266:
				WriteInventoryIndex(binaryWriter, array, 9);
				break;
			case 268:
				WriteInventoryIndex(binaryWriter, array, 10);
				break;
			case 270:
				WriteInventoryIndex(binaryWriter, array, 11);
				break;
			case 272:
				WriteInventoryIndex(binaryWriter, array, 12);
				break;
			case 274:
				WriteInventoryIndex(binaryWriter, array, 13);
				break;
			case 276:
				WriteInventoryIndex(binaryWriter, array, 14);
				break;
			case 278:
				WriteInventoryIndex(binaryWriter, array, 15);
				break;
			case 280:
				WriteInventoryIndex(binaryWriter, array, 16);
				break;
			case 282:
				WriteInventoryIndex(binaryWriter, array, 17);
				break;
			case 284:
				WriteInventoryIndex(binaryWriter, array, 18);
				break;
			default:
				DataLoader.WriteInt8(binaryWriter, 0L);
				break;
			case 64:
			case 65:
			case 66:
			case 67:
			case 68:
			case 78:
			case 80:
			case 81:
			case 82:
			case 86:
			case 88:
			case 90:
			case 103:
			case 104:
			case 105:
			case 212:
			case 249:
			case 251:
			case 253:
			case 255:
			case 257:
			case 259:
			case 261:
			case 263:
			case 265:
			case 267:
			case 269:
			case 271:
			case 273:
			case 275:
			case 277:
			case 279:
			case 281:
			case 283:
			case 285:
				break;
			}
		}
		for (int l = 0; l <= array.GetUpperBound(0); l++)
		{
			GameObject gameObject = GameObject.Find(array[l]);
			if (!(gameObject != null))
			{
				continue;
			}
			ObjectInteraction component = gameObject.GetComponent<ObjectInteraction>();
			if (component.GetItemType() == 12 && component.GetComponent<Wand>() != null && component.GetComponent<Wand>().linkedspell != null)
			{
				string name = component.GetComponent<Wand>().linkedspell.name;
				for (int m = 0; m <= array.GetUpperBound(0); m++)
				{
					if (name == array[m])
					{
						component.link = m + 1;
						break;
					}
				}
			}
			int num16 = (component.isquant << 15) | (component.invis << 14) | (component.doordir << 13) | (component.enchantment << 12) | ((component.flags & 7) << 9) | (component.item_id & 0x1FF);
			DataLoader.WriteInt8(binaryWriter, num16 & 0xFF);
			DataLoader.WriteInt8(binaryWriter, (num16 >> 8) & 0xFF);
			num16 = ((component.xpos & 7) << 13) | ((component.ypos & 7) << 10) | ((component.heading & 7) << 7) | (component.zpos & 0x7F);
			DataLoader.WriteInt8(binaryWriter, num16 & 0xFF);
			DataLoader.WriteInt8(binaryWriter, (num16 >> 8) & 0xFF);
			num16 = ((component.next & 0x3FF) << 6) | (component.quality & 0x3F);
			DataLoader.WriteInt8(binaryWriter, num16 & 0xFF);
			DataLoader.WriteInt8(binaryWriter, (num16 >> 8) & 0xFF);
			num16 = ((component.link & 0x3FF) << 6) | (component.owner & 0x3F);
			DataLoader.WriteInt8(binaryWriter, num16 & 0xFF);
			DataLoader.WriteInt8(binaryWriter, (num16 >> 8) & 0xFF);
		}
		binaryWriter.Close();
		char[] buffer;
		if (!DataLoader.ReadStreamFile(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "playertmp.dat", out buffer))
		{
			return;
		}
		int num17 = buffer[0];
		int num18 = 3;
		for (int n = 1; n <= 210; n++)
		{
			if (n == 81 || n == 161)
			{
				num18 = 3;
			}
			buffer[n] ^= (char)(ushort)((num17 + num18) & 0xFF);
			num18 += 3;
		}
		byte[] array2 = new byte[buffer.GetUpperBound(0) + 1];
		for (long num19 = 0L; num19 <= buffer.GetUpperBound(0); num19++)
		{
			array2[num19] = (byte)buffer[num19];
		}
		File.WriteAllBytes(Loader.BasePath + "SAVE" + slotNo + UWClass.sep + "PLAYER.DAT", array2);
	}
}
