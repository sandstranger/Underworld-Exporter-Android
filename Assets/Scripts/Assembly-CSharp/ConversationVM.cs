using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ConversationVM : UWEBase
{
	private struct ImportedFunctions
	{
		public string functionName;

		public int ID_or_Address;

		public int import_type;

		public int return_type;
	}

	private struct cnvHeader
	{
		public int CodeSize;

		public int StringBlock;

		public int NoOfMemorySlots;

		public int NoOfImportedGlobals;

		public ImportedFunctions[] functions;

		public short[] instuctions;
	}

	public static bool EnteringQty;

	public static bool InConversation;

	public static int CurrentConversation;

	public const int NPC_SAY = 0;

	public const int PC_SAY = 1;

	public const int PRINT_SAY = 2;

	private const short cnv_NOP = 0;

	private const short cnv_OPADD = 1;

	private const short cnv_OPMUL = 2;

	private const short cnv_OPSUB = 3;

	private const short cnv_OPDIV = 4;

	private const short cnv_OPMOD = 5;

	private const short cnv_OPOR = 6;

	private const short cnv_OPAND = 7;

	private const short cnv_OPNOT = 8;

	private const short cnv_TSTGT = 9;

	private const short cnv_TSTGE = 10;

	private const short cnv_TSTLT = 11;

	private const short cnv_TSTLE = 12;

	private const short cnv_TSTEQ = 13;

	private const short cnv_TSTNE = 14;

	private const short cnv_JMP = 15;

	private const short cnv_BEQ = 16;

	private const short cnv_BNE = 17;

	private const short cnv_BRA = 18;

	private const short cnv_CALL = 19;

	private const short cnv_CALLI = 20;

	private const short cnv_RET = 21;

	private const short cnv_PUSHI = 22;

	private const short cnv_PUSHI_EFF = 23;

	private const short cnv_POP = 24;

	private const short cnv_SWAP = 25;

	private const short cnv_PUSHBP = 26;

	private const short cnv_POPBP = 27;

	private const short cnv_SPTOBP = 28;

	private const short cnv_BPTOSP = 29;

	private const short cnv_ADDSP = 30;

	private const short cnv_FETCHM = 31;

	private const short cnv_STO = 32;

	private const short cnv_OFFSET = 33;

	private const short cnv_START = 34;

	private const short cnv_SAVE_REG = 35;

	private const short cnv_PUSH_REG = 36;

	private const short cnv_STRCMP = 37;

	private const short cnv_EXIT_OP = 38;

	private const short cnv_SAY_OP = 39;

	private const short cnv_RESPOND_OP = 40;

	private const short cnv_OPNEG = 41;

	private const int import_function = 273;

	private const int import_variable = 271;

	private const int return_void = 0;

	private const int return_int = 297;

	private const int return_string = 299;

	private static string[] ObjectMasterList;

	private static Text PlayerInput;

	private int currConv = 0;

	public int MaxAnswer;

	private int NPCTalkedToIndex = 0;

	private bool Teleport = false;

	private int TeleportLevel = -1;

	private int TeleportTileX = -1;

	private int TeleportTileY = -1;

	private bool SettingUpFight = false;

	public static bool VMLoaded = false;

	private cnvHeader[] conv;

	private CnvStack stack;

	public static int PlayerAnswer;

	public static string PlayerTypedAnswer;

	public static bool WaitingForInput;

	public static bool WaitingForMore;

	public static bool WaitingForTyping;

	public static int[] bablf_array = new int[10];

	public static bool usingBablF;

	public static int bablf_ans = 0;

	public static ConversationVM instance;

	private void Awake()
	{
		instance = this;
	}

	public void InitConvVM()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			LoadCnvArkUW2(Loader.BasePath + "DATA" + UWEBase.sep + "CNV.ARK");
		}
		else
		{
			LoadCnvArk(Loader.BasePath + "DATA" + UWEBase.sep + "CNV.ARK");
		}
		VMLoaded = true;
	}

	private void Update()
	{
		if (WaitingForTyping)
		{
			UWHUD.instance.InputControl.Select();
		}
	}

	public void LoadCnvArk(string cnv_ark_path)
	{
		char[] buffer;
		if (!DataLoader.ReadStreamFile(cnv_ark_path, out buffer))
		{
			return;
		}
		int num = (int)DataLoader.getValAtAddress(buffer, 0L, 16);
		conv = new cnvHeader[num];
		for (int i = 0; i < num; i++)
		{
			int num2 = (int)DataLoader.getValAtAddress(buffer, 2 + i * 4, 32);
			if (num2 == 0)
			{
				continue;
			}
			conv[i].CodeSize = (int)DataLoader.getValAtAddress(buffer, num2 + 4, 16);
			conv[i].StringBlock = (int)DataLoader.getValAtAddress(buffer, num2 + 10, 16);
			conv[i].NoOfMemorySlots = (int)DataLoader.getValAtAddress(buffer, num2 + 12, 16);
			conv[i].NoOfImportedGlobals = (int)DataLoader.getValAtAddress(buffer, num2 + 14, 16);
			conv[i].functions = new ImportedFunctions[conv[i].NoOfImportedGlobals];
			int num3 = num2 + 16;
			for (int j = 0; j < conv[i].NoOfImportedGlobals; j++)
			{
				int num4 = (int)DataLoader.getValAtAddress(buffer, num3, 16);
				for (int k = 0; k < num4; k++)
				{
					conv[i].functions[j].functionName += (char)DataLoader.getValAtAddress(buffer, num3 + 2 + k, 8);
				}
				conv[i].functions[j].ID_or_Address = (int)DataLoader.getValAtAddress(buffer, num3 + num4 + 2, 16);
				conv[i].functions[j].import_type = (int)DataLoader.getValAtAddress(buffer, num3 + num4 + 6, 16);
				conv[i].functions[j].return_type = (int)DataLoader.getValAtAddress(buffer, num3 + num4 + 8, 16);
				num3 += num4 + 10;
			}
			conv[i].instuctions = new short[conv[i].CodeSize];
			int num5 = 0;
			for (int l = 0; l < conv[i].CodeSize * 2; l += 2)
			{
				conv[i].instuctions[num5++] = (short)DataLoader.getValAtAddress(buffer, num3 + l, 16);
			}
		}
	}

	public void LoadCnvArkUW2(string cnv_ark_path)
	{
		int num = 2;
		char[] buffer;
		if (!DataLoader.ReadStreamFile(cnv_ark_path, out buffer))
		{
			Debug.Log("unable to load uw2 conv ark");
			return;
		}
		int num2 = (int)DataLoader.getValAtAddress(buffer, 0L, 32);
		conv = new cnvHeader[num2];
		for (int i = 0; i < num2; i++)
		{
			int num3 = (int)DataLoader.getValAtAddress(buffer, num + num2 * 4, 32);
			int num4 = (num3 >> 1) & 1;
			long valAtAddress = DataLoader.getValAtAddress(buffer, num, 32);
			if (valAtAddress != 0)
			{
				if (num4 == 1)
				{
					long datalen = 0L;
					char[] buffer2 = DataLoader.unpackUW2(buffer, valAtAddress, ref datalen);
					valAtAddress = 0L;
					conv[i].CodeSize = (int)DataLoader.getValAtAddress(buffer2, valAtAddress + 4, 16);
					conv[i].StringBlock = (int)DataLoader.getValAtAddress(buffer2, valAtAddress + 10, 16);
					conv[i].NoOfMemorySlots = (int)DataLoader.getValAtAddress(buffer2, valAtAddress + 12, 16);
					conv[i].NoOfImportedGlobals = (int)DataLoader.getValAtAddress(buffer2, valAtAddress + 14, 16);
					conv[i].functions = new ImportedFunctions[conv[i].NoOfImportedGlobals];
					long num5 = valAtAddress + 16;
					for (int j = 0; j < conv[i].NoOfImportedGlobals; j++)
					{
						int num6 = (int)DataLoader.getValAtAddress(buffer2, num5, 16);
						for (int k = 0; k < num6; k++)
						{
							conv[i].functions[j].functionName += (char)DataLoader.getValAtAddress(buffer2, num5 + 2 + k, 8);
						}
						conv[i].functions[j].ID_or_Address = (int)DataLoader.getValAtAddress(buffer2, num5 + num6 + 2, 16);
						conv[i].functions[j].import_type = (int)DataLoader.getValAtAddress(buffer2, num5 + num6 + 6, 16);
						conv[i].functions[j].return_type = (int)DataLoader.getValAtAddress(buffer2, num5 + num6 + 8, 16);
						num5 += num6 + 10;
					}
					conv[i].instuctions = new short[conv[i].CodeSize];
					int num7 = 0;
					for (int l = 0; l < conv[i].CodeSize * 2; l += 2)
					{
						conv[i].instuctions[num7++] = (short)DataLoader.getValAtAddress(buffer2, num5 + l, 16);
					}
				}
				else
				{
					Debug.Log("uncompressed flag in cnv.ark");
				}
			}
			num += 4;
		}
	}

	public void DisplayInstructionSet()
	{
		string text = "";
		text = "String Block = " + conv[currConv].StringBlock + "\n";
		string text2 = text;
		text = text2 + "Code Size = " + conv[currConv].CodeSize + "\n";
		for (int i = 0; i <= conv[currConv].functions.GetUpperBound(0); i++)
		{
			if (conv[currConv].functions[i].import_type == 273)
			{
				text2 = text;
				text = text2 + "Function : " + conv[currConv].functions[i].ID_or_Address + " " + conv[currConv].functions[i].functionName + "\n";
			}
			else
			{
				text2 = text;
				text = text2 + "Variable : " + conv[currConv].functions[i].ID_or_Address + " " + conv[currConv].functions[i].functionName + "\n";
			}
		}
		for (int j = 0; j < conv[currConv].CodeSize; j++)
		{
			switch (conv[currConv].instuctions[j])
			{
			case 0:
				text = text + j + ":NOP\n";
				break;
			case 1:
				text = text + j + ":OPADD\n";
				break;
			case 2:
				text = text + j + ":OPMUL\n";
				break;
			case 3:
				text = text + j + ":OPSUB\n";
				break;
			case 4:
				text = text + j + ":OPDIV\n";
				break;
			case 5:
				text = text + j + ":OPMOD\n";
				break;
			case 6:
				text = text + j + ":OPOR\n";
				break;
			case 7:
				text = text + j + ":OPAND\n";
				break;
			case 8:
				text = text + j + ":OPNOT\n";
				break;
			case 9:
				text = text + j + ":TSTGT\n";
				break;
			case 10:
				text = text + j + ":TSTGE\n";
				break;
			case 11:
				text = text + j + ":TSTLT\n";
				break;
			case 12:
				text = text + j + ":TSTLE\n";
				break;
			case 13:
				text = text + j + ":TSTEQ\n";
				break;
			case 14:
				text = text + j + ":TSTNE\n";
				break;
			case 15:
				text = text + j + ":JMP ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + "\n";
				break;
			case 16:
				text = text + j + ":BEQ ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + " // ";
				text = text + " to " + (conv[currConv].instuctions[j] + j);
				text += "\n";
				break;
			case 17:
				text = text + j + ":BNE ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + "\n";
				break;
			case 18:
				text = text + j + ":BRA ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + "\n";
				break;
			case 19:
				text = text + j + ":CALL ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + "\n";
				break;
			case 20:
			{
				text = text + j + ":CALLI ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + " // ";
				int num = conv[currConv].instuctions[j];
				for (int k = 0; k <= conv[currConv].functions.GetUpperBound(0); k++)
				{
					if (conv[currConv].functions[k].ID_or_Address == num && conv[currConv].functions[k].import_type == 273)
					{
						text = text + conv[currConv].functions[k].functionName + "\n";
						break;
					}
				}
				break;
			}
			case 21:
				text = text + j + ":RET\n";
				break;
			case 22:
				text = text + j + ":PUSHI ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + "\n";
				break;
			case 23:
				text = text + j + ":PUSHI_EFF ";
				j++;
				text2 = text;
				text = text2 + " " + conv[currConv].instuctions[j] + "\n";
				break;
			case 24:
				text = text + j + ":POP\n";
				break;
			case 25:
				text = text + j + ":SWAP\n";
				break;
			case 26:
				text = text + j + ":PUSHBP\n";
				break;
			case 27:
				text = text + j + ":POPBP\n";
				break;
			case 28:
				text = text + j + ":SPTOBP\n";
				break;
			case 29:
				text = text + j + ":BPTOSP\n";
				break;
			case 30:
				text = text + j + ":ADDSP\n";
				break;
			case 31:
				text = text + j + ":FETCHM\n";
				break;
			case 32:
				text = text + j + ":STO\n";
				break;
			case 33:
				text = text + j + ":OFFSET\n";
				break;
			case 34:
				text = text + j + ":START\n";
				break;
			case 35:
				text = text + j + ":SAVE_REG\n";
				break;
			case 36:
				text = text + j + ":PUSH_REG\n";
				break;
			case 37:
				text = text + j + ":STRCMP\n";
				break;
			case 38:
				text = text + j + ":EXIT_OP\n";
				break;
			case 39:
			{
				text = text + j + ":SAY_OP  //";
				int stringNo = conv[currConv].instuctions[j - 1];
				string @string = StringController.instance.GetString(conv[currConv].StringBlock, stringNo);
				text = text + @string + "\n";
				break;
			}
			case 40:
				text = text + j + ":RESPOND_OP\n";
				break;
			case 41:
				text = text + j + ":OPNEG\n";
				break;
			}
		}
		TextWriter textWriter = new StreamWriter(Loader.BasePath + UWEBase.sep + "conversation_debug.txt");
		textWriter.Write(text);
		textWriter.Close();
	}

	public void RunConversation(NPC npc)
	{
		string text = "";
		if (!VMLoaded)
		{
			InitConvVM();
			if (UWEBase._RES == "UW2")
			{
				UWHUD.instance.UW2ConversationBG.texture = GameWorldController.instance.bytloader.LoadImageAt(2, false);
			}
		}
		if (npc.npc_whoami == 0)
		{
			currConv = 256 + (npc.item_id - 64);
			text = StringController.instance.GetSimpleObjectNameUW(npc.objInt());
		}
		else
		{
			currConv = npc.npc_whoami;
			if (UWEBase._RES == "UW2")
			{
				currConv++;
			}
		}
		if (npc.npc_whoami > 255)
		{
			text = StringController.instance.GetSimpleObjectNameUW(npc.objInt());
		}
		if (conv[currConv].CodeSize == 0 || npc.npc_whoami == 255)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(7, 1));
			return;
		}
		if (UWEBase._RES == "UW2" && (npc.npc_goal == 5 || npc.npc_goal == 9))
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(7, 1));
			return;
		}
		if (text == "")
		{
			UWHUD.instance.NPCName.text = StringController.instance.GetString(7, npc.npc_whoami + 16);
		}
		else
		{
			UWHUD.instance.NPCName.text = text;
		}
		UWHUD.instance.PCName.text = UWCharacter.Instance.CharName;
		Character.InteractionMode = 7;
		CurrentConversation = npc.npc_whoami;
		InConversation = true;
		UWHUD.instance.RefreshPanels(3);
		UWHUD.instance.Conversation_tl.Clear();
		UWHUD.instance.MessageScroll.Clear();
		PlayerInput = UWHUD.instance.MessageScroll.NewUIOUt;
		UWHUD.instance.RefreshPanels(3);
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			UWHUD.instance.npcTrade[i++].clear();
		}
		RawImage rawImage = UWHUD.instance.ConversationPortraits[0];
		RawImage rawImage2 = UWHUD.instance.ConversationPortraits[1];
		GRLoader gRLoader = new GRLoader(17);
		if (UWCharacter.Instance.isFemale)
		{
			rawImage.texture = gRLoader.LoadImageAt(UWCharacter.Instance.Body + 5);
		}
		else
		{
			rawImage.texture = gRLoader.LoadImageAt(UWCharacter.Instance.Body);
		}
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			rawImage2.texture = npc.UW2NPCPortrait();
			npc.SetupNPCInventory();
		}
		else if (npc.npc_whoami != 0 && npc.npc_whoami <= 28)
		{
			GRLoader gRLoader2 = new GRLoader(7);
			rawImage2.texture = gRLoader2.LoadImageAt(npc.npc_whoami - 1);
		}
		else
		{
			int num = npc.item_id - 64;
			if (num > 59)
			{
				num = 0;
			}
			GRLoader gRLoader3 = new GRLoader(16);
			rawImage2.texture = gRLoader3.LoadImageAt(num);
		}
		UWHUD.instance.MessageScroll.Clear();
		ClearConversationOptions();
		UWCharacter.Instance.playerMotor.enabled = false;
		if (MusicController.instance != null)
		{
			MusicController.instance.InMap = true;
		}
		BuildObjectList();
		DisplayInstructionSet();
		Time.timeScale = 0f;
		Teleport = false;
		SettingUpFight = false;
		StopAllCoroutines();
		StartCoroutine(RunConversationVM(npc));
	}

	private IEnumerator RunConversationVM(NPC npc)
	{
		bool finished = false;
		stack = new CnvStack(4096);
		stack.set_stackp(200);
		stack.basep = 0;
		WaitingForInput = false;
		WaitingForTyping = false;
		WaitingForMore = false;
		ImportVariableMemory(npc);
		while (!finished)
		{
			switch (conv[currConv].instuctions[stack.instrp])
			{
			case 1:
				stack.Push(stack.Pop() + stack.Pop());
				break;
			case 2:
				stack.Push(stack.Pop() * stack.Pop());
				break;
			case 3:
			{
				int num11 = stack.Pop();
				int num12 = stack.Pop();
				stack.Push(num12 - num11);
				break;
			}
			case 4:
			{
				int num15 = stack.Pop();
				int num16 = stack.Pop();
				stack.Push(num16 / num15);
				break;
			}
			case 5:
			{
				int num7 = stack.Pop();
				int num8 = stack.Pop();
				stack.Push(num8 % num7);
				break;
			}
			case 6:
				stack.Push(stack.Pop() | stack.Pop());
				break;
			case 7:
				stack.Push(stack.Pop() & stack.Pop());
				break;
			case 8:
				if (stack.Pop() == 0)
				{
					stack.Push(1);
				}
				else
				{
					stack.Push(0);
				}
				break;
			case 9:
			{
				int num5 = stack.Pop();
				int num6 = stack.Pop();
				if (num6 > num5)
				{
					stack.Push(1);
				}
				else
				{
					stack.Push(0);
				}
				break;
			}
			case 10:
			{
				int num19 = stack.Pop();
				int num20 = stack.Pop();
				if (num20 >= num19)
				{
					stack.Push(1);
				}
				else
				{
					stack.Push(0);
				}
				break;
			}
			case 11:
			{
				int num17 = stack.Pop();
				int num18 = stack.Pop();
				if (num18 < num17)
				{
					stack.Push(1);
				}
				else
				{
					stack.Push(0);
				}
				break;
			}
			case 12:
			{
				int num13 = stack.Pop();
				int num14 = stack.Pop();
				if (num14 <= num13)
				{
					stack.Push(1);
				}
				else
				{
					stack.Push(0);
				}
				break;
			}
			case 13:
				if (stack.Pop() == stack.Pop())
				{
					stack.Push(1);
				}
				else
				{
					stack.Push(0);
				}
				break;
			case 14:
				if (stack.Pop() != stack.Pop())
				{
					stack.Push(1);
				}
				else
				{
					stack.Push(0);
				}
				break;
			case 15:
				stack.instrp = conv[currConv].instuctions[stack.instrp + 1] - 1;
				break;
			case 16:
				if (stack.Pop() == 0)
				{
					stack.instrp += conv[currConv].instuctions[stack.instrp + 1];
				}
				else
				{
					stack.instrp++;
				}
				break;
			case 17:
				if (stack.Pop() != 0)
				{
					stack.instrp += conv[currConv].instuctions[stack.instrp + 1];
				}
				else
				{
					stack.instrp++;
				}
				break;
			case 18:
				stack.instrp += conv[currConv].instuctions[stack.instrp + 1];
				break;
			case 19:
				stack.Push(stack.instrp + 1);
				stack.instrp = conv[currConv].instuctions[stack.instrp + 1] - 1;
				stack.call_level++;
				break;
			case 20:
			{
				int arg2 = conv[currConv].instuctions[++stack.instrp];
				for (int i = 0; i <= conv[currConv].functions.GetUpperBound(0); i++)
				{
					if (conv[currConv].functions[i].ID_or_Address == arg2 && conv[currConv].functions[i].import_type == 273)
					{
						yield return StartCoroutine(run_imported_function(conv[currConv].functions[i], npc));
						break;
					}
				}
				break;
			}
			case 21:
				if (--stack.call_level < 0)
				{
					finished = true;
				}
				else
				{
					stack.instrp = stack.Pop();
				}
				break;
			case 22:
				stack.Push(conv[currConv].instuctions[++stack.instrp]);
				break;
			case 23:
			{
				int num10 = conv[currConv].instuctions[stack.instrp + 1];
				if (num10 >= 0)
				{
					stack.Push(stack.basep + num10);
				}
				else
				{
					num10--;
					stack.Push(stack.basep + num10);
				}
				stack.instrp++;
				break;
			}
			case 24:
				stack.Pop();
				break;
			case 25:
			{
				int newValue = stack.Pop();
				int newValue2 = stack.Pop();
				stack.Push(newValue);
				stack.Push(newValue2);
				break;
			}
			case 26:
				stack.Push(stack.basep);
				break;
			case 27:
			{
				int basep = stack.Pop();
				stack.basep = basep;
				break;
			}
			case 28:
				stack.basep = stack.stackptr;
				break;
			case 29:
				stack.set_stackp(stack.basep);
				break;
			case 30:
			{
				int num9 = stack.Pop();
				for (int j = 0; j <= num9; j++)
				{
					stack.Push(0);
				}
				break;
			}
			case 31:
				stack.Push(stack.at(stack.Pop()));
				break;
			case 32:
			{
				int num3 = stack.Pop();
				int num4 = stack.Pop();
				if (num4 < conv[currConv].NoOfImportedGlobals)
				{
					PrintImportedVariable(num4, num3);
				}
				stack.Set(num4, num3);
				break;
			}
			case 33:
			{
				int num = stack.Pop();
				int num2 = stack.Pop();
				num += num2 - 1;
				stack.Push(num);
				break;
			}
			case 35:
				stack.result_register = stack.Pop();
				break;
			case 36:
				stack.Push(stack.result_register);
				break;
			case 38:
				finished = true;
				break;
			case 39:
			{
				int arg1 = stack.Pop();
				yield return StartCoroutine(say_op(arg1));
				break;
			}
			case 40:
				Debug.Log("Respond_Op");
				break;
			case 41:
				stack.Push(-stack.Pop());
				break;
			}
			stack.instrp++;
			if (stack.instrp > conv[currConv].instuctions.GetUpperBound(0))
			{
				finished = true;
			}
		}
		yield return StartCoroutine(EndConversation(npc));
	}

	public IEnumerator EndConversation(NPC npc)
	{
		for (int i = 0; i <= GameWorldController.instance.bGlobals.GetUpperBound(0); i++)
		{
			if (CurrentConversation == GameWorldController.instance.bGlobals[i].ConversationNo)
			{
				GameWorldController.instance.bGlobals[i].Globals[NPCTalkedToIndex] = 1;
				for (int j = 0; j <= GameWorldController.instance.bGlobals[i].Globals.GetUpperBound(0); j++)
				{
					GameWorldController.instance.bGlobals[i].Globals[j] = stack.at(j);
				}
				break;
			}
		}
		int maxAddress = 0;
		for (int k = 0; k <= conv[currConv].functions.GetUpperBound(0); k++)
		{
			if (conv[currConv].functions[k].import_type == 271)
			{
				int iD_or_Address = conv[currConv].functions[k].ID_or_Address;
				if (iD_or_Address > maxAddress)
				{
					maxAddress = iD_or_Address;
				}
				switch (conv[currConv].functions[k].functionName.ToLower())
				{
				case "npc_talkedto":
					npc.npc_talkedto = 1;
					break;
				case "npc_gtarg":
					npc.npc_gtarg = (short)stack.at(iD_or_Address);
					break;
				case "npc_attitude":
					npc.npc_attitude = (short)stack.at(iD_or_Address);
					break;
				case "npc_goal":
					npc.npc_goal = (short)stack.at(iD_or_Address);
					break;
				case "npc_power":
					npc.npc_power = (short)stack.at(iD_or_Address);
					break;
				case "npc_arms":
					npc.npc_arms = (short)stack.at(iD_or_Address);
					break;
				case "npc_hp":
					npc.npc_hp = (short)stack.at(iD_or_Address);
					break;
				case "npc_health":
					npc.npc_health = (short)stack.at(iD_or_Address);
					break;
				case "npc_hunger":
					npc.npc_hunger = (short)stack.at(iD_or_Address);
					break;
				case "npc_whoami":
					npc.npc_whoami = (short)stack.at(iD_or_Address);
					break;
				case "npc_yhome":
					npc.npc_yhome = (short)stack.at(iD_or_Address);
					break;
				case "npc_xhome":
					npc.npc_xhome = (short)stack.at(iD_or_Address);
					break;
				}
			}
		}
		if (UWEBase._RES == "UW2" && currConv == 5)
		{
			Quest.instance.QuestVariables[0] = stack.at(31);
		}
		ClearConversationOptions();
		UWCharacter.Instance.playerMotor.enabled = true;
		Container cn = UWCharacter.Instance.playerInventory.GetCurrentContainer();
		for (int l = 0; l <= TradeSlot.TradeSlotUBound; l++)
		{
			TradeSlot tradeSlot = UWHUD.instance.playerTrade[l];
			if (!(tradeSlot.objectInSlot != null))
			{
				continue;
			}
			if (Container.GetFreeSlot(cn) != -1)
			{
				npc.GetComponent<Container>().RemoveItemFromContainer(tradeSlot.objectInSlot);
				cn.AddItemToContainer(tradeSlot.objectInSlot);
				GameObject gameObject = tradeSlot.GetGameObjectInteraction().gameObject;
				gameObject.transform.parent = GameWorldController.instance.InventoryMarker.transform;
				GameWorldController.MoveToInventory(gameObject);
				tradeSlot.clear();
				UWCharacter.Instance.GetComponent<PlayerInventory>().Refresh();
			}
			else
			{
				npc.GetComponent<Container>().RemoveItemFromContainer(tradeSlot.objectInSlot);
				tradeSlot.clear();
				if (tradeSlot.objectInSlot.transform.parent != GameWorldController.instance.DynamicObjectMarker())
				{
					GameWorldController.MoveToWorld(tradeSlot.objectInSlot);
				}
				tradeSlot.objectInSlot.transform.position = npc.NPC_Launcher.transform.position;
			}
		}
		for (int m = 0; m <= TradeSlot.TradeSlotUBound; m++)
		{
			UWHUD.instance.npcTrade[m].clear();
		}
		Time.timeScale = 1f;
		yield return StartCoroutine(WaitForMore());
		yield return new WaitForSeconds(0.5f);
		InConversation = false;
		UWHUD.instance.Conversation_tl.Clear();
		UWHUD.instance.MessageScroll.Clear();
		Character.InteractionMode = 1;
		if (MusicController.instance != null)
		{
			MusicController.instance.InMap = false;
		}
		if (UWEBase.CurrentObjectInHand != null)
		{
			Character.InteractionMode = 2;
		}
		StopAllCoroutines();
		UWHUD.instance.RefreshPanels(0);
		if (UWEBase._RES == "UW2" && !UWEBase.EditorMode && GameWorldController.instance.events != null)
		{
			GameWorldController.instance.events.ProcessEvents();
		}
		if (!SettingUpFight)
		{
			Quest.instance.FightingInArena = false;
		}
		if (Teleport)
		{
			if (TeleportLevel == GameWorldController.instance.LevelNo)
			{
				float x = (float)TeleportTileX * 1.2f + 0.6f;
				float z = (float)TeleportTileY * 1.2f + 0.6f;
				float num = (float)UWEBase.CurrentTileMap().GetFloorHeight(TeleportTileX, TeleportTileY) * 0.15f;
				UWCharacter.Instance.transform.position = new Vector3(x, num + 0.1f, z);
				UWCharacter.Instance.TeleportPosition = UWCharacter.Instance.transform.position;
			}
			else
			{
				UWCharacter.Instance.JustTeleported = true;
				UWCharacter.Instance.teleportedTimer = 0f;
				UWCharacter.Instance.playerMotor.movement.velocity = Vector3.zero;
				GameWorldController.instance.SwitchLevel((short)TeleportLevel, (short)TeleportTileX, (short)TeleportTileY);
			}
		}
	}

	private void ImportVariableMemory(NPC npc)
	{
		for (int i = 0; i <= GameWorldController.instance.bGlobals.GetUpperBound(0); i++)
		{
			if (npc.npc_whoami == GameWorldController.instance.bGlobals[i].ConversationNo)
			{
				for (int j = 0; j <= GameWorldController.instance.bGlobals[i].Globals.GetUpperBound(0); j++)
				{
					stack.Set(j, GameWorldController.instance.bGlobals[i].Globals[j]);
				}
				break;
			}
		}
		for (int k = 0; k <= conv[currConv].functions.GetUpperBound(0); k++)
		{
			if (conv[currConv].functions[k].import_type != 271)
			{
				continue;
			}
			int iD_or_Address = conv[currConv].functions[k].ID_or_Address;
			switch (conv[currConv].functions[k].functionName.ToLower())
			{
			case "game_mins":
				stack.Set(iD_or_Address, GameClock.game_min());
				break;
			case "game_days":
				stack.Set(iD_or_Address, GameClock.day());
				break;
			case "game_time":
				stack.Set(iD_or_Address, GameClock.second());
				break;
			case "riddlecounter":
				stack.Set(iD_or_Address, 0);
				break;
			case "dungeon_level":
				stack.Set(iD_or_Address, GameWorldController.instance.LevelNo + 1);
				break;
			case "npc_name":
				stack.Set(iD_or_Address, StringController.instance.AddString(conv[currConv].StringBlock, StringController.instance.GetString(7, npc.npc_whoami + 16)));
				break;
			case "npc_level":
				stack.Set(iD_or_Address, npc.npc_level);
				break;
			case "npc_talkedto":
				NPCTalkedToIndex = iD_or_Address;
				stack.Set(iD_or_Address, npc.npc_talkedto);
				break;
			case "npc_gtarg":
				stack.Set(iD_or_Address, npc.npc_gtarg);
				break;
			case "npc_attitude":
				stack.Set(iD_or_Address, npc.npc_attitude);
				break;
			case "npc_goal":
				stack.Set(iD_or_Address, npc.npc_goal);
				break;
			case "npc_power":
				stack.Set(iD_or_Address, npc.npc_power);
				break;
			case "npc_arms":
				stack.Set(iD_or_Address, npc.npc_arms);
				break;
			case "npc_hp":
				stack.Set(iD_or_Address, npc.npc_hp);
				break;
			case "npc_health":
				stack.Set(iD_or_Address, npc.npc_health);
				break;
			case "npc_hunger":
				stack.Set(iD_or_Address, npc.npc_hunger);
				break;
			case "npc_whoami":
				stack.Set(iD_or_Address, npc.npc_whoami);
				break;
			case "npc_yhome":
				stack.Set(iD_or_Address, npc.npc_yhome);
				break;
			case "npc_xhome":
				stack.Set(iD_or_Address, npc.npc_xhome);
				break;
			case "play_sex":
				if (UWCharacter.Instance.isFemale)
				{
					stack.Set(iD_or_Address, 1);
				}
				else
				{
					stack.Set(iD_or_Address, 0);
				}
				break;
			case "play_poison":
				stack.Set(iD_or_Address, UWCharacter.Instance.play_poison);
				break;
			case "play_name":
				stack.Set(iD_or_Address, StringController.instance.AddString(conv[currConv].StringBlock, UWCharacter.Instance.CharName));
				break;
			case "play_level":
				stack.Set(iD_or_Address, UWCharacter.Instance.CharLevel);
				break;
			case "play_mana":
				stack.Set(iD_or_Address, UWCharacter.Instance.PlayerMagic.CurMana);
				break;
			case "play_hp":
				stack.Set(iD_or_Address, UWCharacter.Instance.CurVIT);
				break;
			case "play_hunger":
				stack.Set(iD_or_Address, UWCharacter.Instance.FoodLevel);
				break;
			}
		}
	}

	private IEnumerator say_op(int arg1)
	{
		yield return StartCoroutine(say_op(arg1, 0));
		yield return 0;
	}

	private IEnumerator say_op(int arg1, int PrintType)
	{
		yield return StartCoroutine(say_op(StringController.instance.GetString(conv[currConv].StringBlock, arg1), PrintType));
		yield return 0;
	}

	private IEnumerator say_op(string text, int PrintType)
	{
		yield return new WaitForSecondsRealtime(0.2f);
		if (text.Trim() == "")
		{
			yield return 0;
		}
		if (text.Contains("@"))
		{
			text = TextSubstitute(text);
		}
		string[] Lines = text.Split(new string[1] { "\n" }, StringSplitOptions.None);
		for (int s = 0; s <= Lines.GetUpperBound(0); s++)
		{
			if (!(Lines[s].Trim() != ""))
			{
				continue;
			}
			string[] Paragraphs = Lines[s].Split(new string[1] { "\\m" }, StringSplitOptions.None);
			for (int i = 0; i <= Paragraphs.GetUpperBound(0); i++)
			{
				string Markup2 = "";
				switch (PrintType)
				{
				case 1:
					Markup2 = "<color=red>";
					break;
				case 2:
					Markup2 = "<color=purple>";
					break;
				default:
					Markup2 = "<color=black>";
					break;
				}
				UWHUD.instance.Conversation_tl.Add(Paragraphs[i], Markup2);
				if (i < Paragraphs.GetUpperBound(0))
				{
					UWHUD.instance.Conversation_tl.Add("MORE", "<color=white>");
					yield return StartCoroutine(WaitForMore());
				}
			}
		}
		yield return new WaitForSecondsRealtime(0.2f);
		yield return 0;
	}

	private string TextSubstitute(string input)
	{
		string pattern = "([@][GSP][SI])([0-9]*)([S][I])?([0-9]*)?([C][0-9]*)?";
		MatchCollection matchCollection = Regex.Matches(input, pattern);
		for (int i = 0; i < matchCollection.Count; i++)
		{
			string value = matchCollection[i].Value;
			if (!matchCollection[i].Success)
			{
				continue;
			}
			string text = "";
			int num = 0;
			string text2 = "";
			int num2 = 0;
			string text3 = "";
			string text4 = "";
			for (int j = 0; j < matchCollection[i].Groups.Count; j++)
			{
				if (matchCollection[i].Groups[j].Success)
				{
					switch (j)
					{
					case 1:
						text = matchCollection[i].Groups[j].Value;
						break;
					case 2:
					{
						int result2 = 0;
						num = (int.TryParse(matchCollection[i].Groups[j].Value, out result2) ? result2 : 0);
						break;
					}
					case 3:
						text2 = matchCollection[i].Groups[j].Value;
						break;
					case 4:
					{
						int result = 0;
						num2 = (int.TryParse(matchCollection[i].Groups[j].Value, out result) ? result : 0);
						break;
					}
					case 5:
						text3 = matchCollection[i].Groups[j].Value;
						break;
					}
				}
			}
			switch (text)
			{
			case "@GS":
				text4 = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(num));
				break;
			case "@GI":
				Debug.Log("@GI String replacement (" + num + ")");
				text4 = stack.at(num).ToString();
				break;
			case "@SS":
				if (num2 != 0)
				{
					int num3 = stack.at(stack.basep + num2);
					text4 = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(stack.basep + num3));
				}
				else
				{
					text4 = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(stack.basep + num));
				}
				break;
			case "@SI":
				text4 = ((!(UWEBase._RES == "UW2")) ? stack.at(stack.basep + num + 1).ToString() : stack.at(stack.basep + num).ToString());
				break;
			case "@PS":
				text4 = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(stack.at(stack.basep + num)));
				break;
			case "@PI":
				text4 = ((num >= 0) ? stack.at(stack.at(stack.basep + num)).ToString() : stack.at(stack.at(stack.basep + num - 1)).ToString());
				break;
			}
			if (text4 != "")
			{
				input = input.Replace(value, text4);
			}
		}
		return input;
	}

	private IEnumerator run_imported_function(ImportedFunctions func, NPC npc)
	{
		switch (func.functionName.ToLower())
		{
		case "babl_menu":
			yield return StartCoroutine(babl_menu((new int[1] { stack.at(stack.stackptr - 2) })[0]));
			break;
		case "babl_fmenu":
		{
			int start = stack.at(stack.stackptr - 2);
			int flagstart = stack.at(stack.stackptr - 3);
			yield return StartCoroutine(babl_fmenu(start, flagstart));
			break;
		}
		case "babl_ask":
			yield return StartCoroutine(babl_ask());
			break;
		case "get_quest":
		{
			int[] array36 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = get_quest(stack.at(array36[0]));
			break;
		}
		case "set_quest":
		{
			int[] array35 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			set_quest(stack.at(array35[0]), stack.at(array35[1]));
			break;
		}
		case "print":
		{
			int[] args5 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				0
			};
			yield return StartCoroutine(say_op(stack.at(args5[0]), 2));
			break;
		}
		case "x_skills":
		{
			int[] array34 = new int[4]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4),
				stack.at(stack.stackptr - 5)
			};
			stack.result_register = x_skills(stack.at(array34[0]), stack.at(array34[1]), stack.at(array34[2]), stack.at(array34[3]));
			break;
		}
		case "set_likes_dislikes":
		{
			int[] array33 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			set_likes_dislikes(stack.at(array33[0]), stack.at(array33[1]));
			break;
		}
		case "sex":
		{
			int[] array32 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			stack.result_register = sex(stack.at(array32[0]), stack.at(array32[1]));
			break;
		}
		case "random":
		{
			int[] array31 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = UnityEngine.Random.Range(1, stack.at(array31[0]) + 1);
			break;
		}
		case "show_inv":
		{
			int startObjectPos = stack.at(stack.stackptr - 2);
			int startObjectIDs = stack.at(stack.stackptr - 3);
			stack.result_register = show_inv(startObjectPos, startObjectIDs);
			break;
		}
		case "give_to_npc":
		{
			int[] array30 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			stack.result_register = give_to_npc(npc, array30[0], stack.at(array30[1]));
			break;
		}
		case "take_from_npc":
		{
			int index = stack.at(stack.stackptr - 2);
			stack.result_register = take_from_npc(npc, stack.at(index));
			break;
		}
		case "setup_to_barter":
			setup_to_barter(npc);
			break;
		case "do_offer":
			switch (stack.TopValue)
			{
			case 7:
			{
				int[] args4 = new int[7]
				{
					stack.at(stack.stackptr - 2),
					stack.at(stack.stackptr - 3),
					stack.at(stack.stackptr - 4),
					stack.at(stack.stackptr - 5),
					stack.at(stack.stackptr - 6),
					stack.at(stack.stackptr - 7),
					stack.at(stack.stackptr - 8)
				};
				yield return StartCoroutine(do_offer(npc, stack.at(args4[0]), stack.at(args4[1]), stack.at(args4[2]), stack.at(args4[3]), stack.at(args4[4]), stack.at(args4[5]), stack.at(args4[6])));
				break;
			}
			case 5:
			{
				int[] args3 = new int[5]
				{
					stack.at(stack.stackptr - 2),
					stack.at(stack.stackptr - 3),
					stack.at(stack.stackptr - 4),
					stack.at(stack.stackptr - 5),
					stack.at(stack.stackptr - 5)
				};
				yield return StartCoroutine(do_offer(npc, stack.at(args3[0]), stack.at(args3[1]), stack.at(args3[2]), stack.at(args3[3]), stack.at(args3[4]), -1, -1));
				break;
			}
			default:
				Debug.Log("uniplemented version of do_offer " + stack.TopValue);
				break;
			}
			break;
		case "do_demand":
		{
			int[] args2 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			yield return StartCoroutine(do_demand(npc, stack.at(args2[0]), stack.at(args2[1])));
			break;
		}
		case "do_judgement":
			yield return StartCoroutine(do_judgement(npc));
			break;
		case "do_decline":
			do_decline(npc);
			break;
		case "gronk_door":
		{
			int[] array29 = new int[3]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4)
			};
			stack.result_register = gronk_door(stack.at(array29[0]), stack.at(array29[1]), stack.at(array29[2]));
			break;
		}
		case "x_obj_stuff":
		{
			int[] array28 = new int[9]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4),
				stack.at(stack.stackptr - 5),
				stack.at(stack.stackptr - 6),
				stack.at(stack.stackptr - 7),
				stack.at(stack.stackptr - 8),
				stack.at(stack.stackptr - 9),
				stack.at(stack.stackptr - 10)
			};
			x_obj_stuff(array28[0], array28[1], array28[2], array28[3], array28[4], array28[5], array28[6], array28[7], array28[8]);
			break;
		}
		case "find_inv":
		{
			int[] array27 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			stack.result_register = find_inv(npc, stack.at(array27[0]), stack.at(array27[1]));
			break;
		}
		case "identify_inv":
		{
			int[] array26 = new int[4]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4),
				stack.at(stack.stackptr - 5)
			};
			stack.result_register = identify_inv(array26[0], array26[1], array26[2], array26[3]);
			break;
		}
		case "contains":
		{
			int[] array25 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			stack.result_register = contains(array25[0], array25[1]);
			break;
		}
		case "set_race_attitude":
		{
			int[] array24 = new int[3]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4)
			};
			set_race_attitude(npc, stack.at(array24[0]), stack.at(array24[1]), stack.at(array24[2]));
			break;
		}
		case "set_attitude":
		{
			int[] array23 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			set_attitude(stack.at(array23[0]), stack.at(array23[1]));
			break;
		}
		case "compare":
		{
			int[] array22 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			stack.result_register = compare(stack.at(array22[0]), stack.at(array22[1]));
			break;
		}
		case "count_inv":
		{
			int[] array21 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = count_inv(stack.at(array21[0]));
			break;
		}
		case "remove_talker":
			remove_talker(npc);
			break;
		case "give_ptr_npc":
		{
			int[] array20 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			give_ptr_npc(npc, stack.at(array20[0]), array20[1]);
			break;
		}
		case "take_from_npc_inv":
		{
			int[] array19 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = take_from_npc_inv(npc, stack.at(array19[0]));
			break;
		}
		case "take_id_from_npc":
		{
			int[] array18 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = take_id_from_npc(npc, stack.at(array18[0]));
			break;
		}
		case "find_barter":
		{
			int[] array17 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = find_barter(stack.at(array17[0]));
			break;
		}
		case "length":
		{
			int[] array16 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = length(stack.at(array16[0]));
			break;
		}
		case "find_barter_total":
		{
			int[] array15 = new int[4]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4),
				stack.at(stack.stackptr - 5)
			};
			stack.result_register = find_barter_total(array15[0], array15[1], array15[2], stack.at(array15[3]));
			break;
		}
		case "do_inv_create":
		{
			int[] array14 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = do_inv_create(npc, stack.at(array14[0]));
			break;
		}
		case "place_object":
		{
			int[] array13 = new int[3]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4)
			};
			place_object(stack.at(array13[0]), stack.at(array13[1]), stack.at(array13[2]));
			break;
		}
		case "do_inv_delete":
		{
			int[] array12 = new int[1] { stack.at(stack.stackptr - 2) };
			do_inv_delete(npc, stack.at(array12[0]));
			break;
		}
		case "x_traps":
		{
			int[] array11 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			if (stack.at(array11[0]) == 10001)
			{
				stack.result_register = x_traps(stack.at(array11[0]), stack.at(array11[1]));
			}
			else
			{
				x_traps(stack.at(array11[0]), stack.at(array11[1]));
			}
			break;
		}
		case "switch_pic":
		{
			int[] array10 = new int[1] { stack.at(stack.stackptr - 2) };
			switch_pic(stack.at(array10[0]));
			break;
		}
		case "x_clock":
		{
			int[] array9 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			x_clock(stack.at(array9[0]), stack.at(array9[1]));
			break;
		}
		case "x_exp":
		{
			int[] array8 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				0
			};
			x_exp(stack.at(array8[0]));
			break;
		}
		case "check_inv_quality":
		{
			int[] array7 = new int[1] { stack.at(stack.stackptr - 2) };
			stack.result_register = check_inv_quality(stack.at(array7[0]));
			break;
		}
		case "set_inv_quality":
		{
			int[] array6 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			set_inv_quality(stack.at(array6[0]), stack.at(array6[1]));
			break;
		}
		case "x_obj_pos":
		{
			int[] array5 = new int[5]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4),
				stack.at(stack.stackptr - 5),
				stack.at(stack.stackptr - 5)
			};
			x_obj_pos(stack.at(array5[0]), stack.at(array5[1]), stack.at(array5[2]), stack.at(array5[3]), stack.at(array5[4]));
			break;
		}
		case "teleport_talker":
		{
			int[] array4 = new int[2]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3)
			};
			teleport_talker(npc, stack.at(array4[0]), stack.at(array4[1]));
			break;
		}
		case "babl_hack":
		{
			int num = stack.at(stack.at(stack.stackptr - 2));
			switch (num)
			{
			case 0:
				SettingUpFight = true;
				Quest.instance.ArenaOpponents[0] = npc.objInt().objectloaderinfo.index;
				Quest.instance.FightingInArena = true;
				break;
			case 1:
				if (Quest.instance.FightingInArena)
				{
					stack.result_register = 1;
				}
				else
				{
					stack.result_register = 0;
				}
				break;
			case 2:
			{
				int[] array3 = new int[4]
				{
					stack.at(stack.stackptr - 2),
					stack.at(stack.stackptr - 3),
					stack.at(stack.stackptr - 4),
					stack.at(stack.stackptr - 5)
				};
				babl_hackSetUpFight(stack.at(array3[0]), stack.at(array3[1]), stack.at(array3[2]), stack.at(array3[3]));
				break;
			}
			case 3:
			{
				int[] array2 = new int[1] { stack.at(stack.stackptr - 2) };
				babl_hackJospurDebt(stack.at(array2[0]));
				break;
			}
			default:
				Debug.Log("Unimplemented babl hack " + num);
				break;
			}
			break;
		}
		case "teleport_player":
		{
			int[] array = new int[3]
			{
				stack.at(stack.stackptr - 2),
				stack.at(stack.stackptr - 3),
				stack.at(stack.stackptr - 4)
			};
			teleport_player(stack.at(array[0]), stack.at(array[1]), stack.at(array[2]));
			break;
		}
		case "pause":
		{
			int[] args = new int[1] { stack.at(stack.stackptr - 2) };
			yield return StartCoroutine(pause(stack.at(args[0])));
			break;
		}
		default:
			Debug.Log("Conversation : " + npc.npc_whoami + "unimplemented function " + func.functionName + " instr at " + stack.instrp);
			break;
		}
		yield return 0;
	}

	public IEnumerator pause(int waittime)
	{
		yield return new WaitForSecondsRealtime(waittime);
	}

	public IEnumerator babl_menu(int Start)
	{
		UWHUD.instance.MessageScroll.Clear();
		yield return new WaitForSecondsRealtime(0.2f);
		usingBablF = false;
		MaxAnswer = 0;
		int i = 1;
		ClearConversationOptions();
		for (int j = Start; j <= stack.Upperbound() && stack.at(j) > 0; j++)
		{
			string text = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(j));
			if (text.Contains("@"))
			{
				text = TextSubstitute(text);
			}
			UWHUD.instance.ConversationOptions[i - 1].SetText(i + "." + text + "");
			UWHUD.instance.EnableDisableControl(UWHUD.instance.ConversationOptions[i - 1], true);
			i++;
			MaxAnswer++;
		}
		yield return StartCoroutine(WaitForInput());
		int AnswerIndex = stack.at(Start + PlayerAnswer - 1);
		yield return StartCoroutine(say_op(AnswerIndex, 1));
		stack.result_register = PlayerAnswer;
		yield return 0;
	}

	private static void ClearConversationOptions()
	{
		for (int i = 0; i <= UWHUD.instance.ConversationOptions.GetUpperBound(0); i++)
		{
			UWHUD.instance.ConversationOptions[i].SetText("");
			UWHUD.instance.EnableDisableControl(UWHUD.instance.ConversationOptions[i], false);
		}
	}

	public IEnumerator babl_fmenu(int Start, int flagIndex)
	{
		ClearConversationOptions();
		UWHUD.instance.MessageScroll.Clear();
		yield return new WaitForSecondsRealtime(0.2f);
		usingBablF = true;
		for (int j = 0; j <= bablf_array.GetUpperBound(0); j++)
		{
			bablf_array[j] = 0;
		}
		int i = 1;
		MaxAnswer = 0;
		for (int k = Start; k <= stack.Upperbound() && stack.at(k) != 0; k++)
		{
			if (stack.at(flagIndex++) != 0)
			{
				string text = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(k));
				if (text.Contains("@"))
				{
					text = TextSubstitute(text);
				}
				bablf_array[i - 1] = stack.at(k);
				UWHUD.instance.ConversationOptions[i - 1].SetText(i + "." + text + "");
				UWHUD.instance.EnableDisableControl(UWHUD.instance.ConversationOptions[i - 1], true);
				i++;
				MaxAnswer++;
			}
		}
		yield return StartCoroutine(WaitForInput());
		yield return StartCoroutine(say_op(bablf_array[bablf_ans - 1], 1));
		stack.result_register = PlayerAnswer;
		yield return 0;
	}

	public IEnumerator babl_ask()
	{
		PlayerTypedAnswer = "";
		PlayerInput.text = ">";
		InputField inputctrl = UWHUD.instance.InputControl;
		inputctrl.gameObject.SetActive(true);
		inputctrl.gameObject.GetComponent<InputHandler>().target = base.gameObject;
		inputctrl.gameObject.GetComponent<InputHandler>().currentInputMode = 2;
		inputctrl.contentType = InputField.ContentType.Standard;
		inputctrl.text = "";
		inputctrl.Select();
		yield return StartCoroutine(WaitForTypedInput());
		yield return StartCoroutine(say_op(PlayerTypedAnswer, 1));
		inputctrl.text = "";
		UWHUD.instance.MessageScroll.Clear();
		stack.result_register = StringController.instance.AddString(conv[currConv].StringBlock, PlayerTypedAnswer);
	}

	public static void OnSubmitPickup(string PlayerTypedAnswerIN)
	{
		InputField inputControl = UWHUD.instance.InputControl;
		WaitingForTyping = false;
		inputControl.gameObject.SetActive(false);
		PlayerTypedAnswer = PlayerTypedAnswerIN;
	}

	private IEnumerator WaitForInput()
	{
		WaitingForInput = true;
		while (WaitingForInput)
		{
			yield return null;
		}
	}

	private IEnumerator WaitForTypedInput()
	{
		WaitingForTyping = true;
		while (WaitingForTyping)
		{
			yield return null;
		}
	}

	private void OnGUI()
	{
		if (EnteringQty)
		{
			return;
		}
		if (WaitingForInput)
		{
			if (!(UWEBase.CurrentObjectInHand != null))
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					CheckAnswer(1);
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					CheckAnswer(2);
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					CheckAnswer(3);
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					CheckAnswer(4);
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					CheckAnswer(5);
				}
				else if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					CheckAnswer(6);
				}
			}
		}
		else if (WaitingForMore && Input.GetKeyDown(KeyCode.Space))
		{
			WaitingForMore = false;
		}
	}

	public void CheckAnswer(int AnswerNo)
	{
		if (!usingBablF)
		{
			if (AnswerNo > 0 && AnswerNo <= MaxAnswer)
			{
				PlayerAnswer = AnswerNo;
				WaitingForInput = false;
			}
		}
		else if (AnswerNo > 0 && AnswerNo <= MaxAnswer)
		{
			bablf_ans = AnswerNo;
			PlayerAnswer = bablf_array[AnswerNo - 1];
			WaitingForInput = false;
			usingBablF = false;
		}
	}

	public int get_quest(int QuestNo)
	{
		if (UWEBase._RES == "UW2")
		{
			Debug.Log("Checking Quest no " + QuestNo + " it's value is " + Quest.instance.QuestVariables[QuestNo]);
		}
		if (QuestNo > Quest.instance.QuestVariables.GetUpperBound(0))
		{
			Debug.Log("invalid quest no " + QuestNo);
			return 0;
		}
		return Quest.instance.QuestVariables[QuestNo];
	}

	public void set_quest(int value, int QuestNo)
	{
		if (UWEBase._RES == "UW2")
		{
			Debug.Log("Setting Quest no " + QuestNo + " to " + value);
		}
		if (QuestNo > Quest.instance.QuestVariables.GetUpperBound(0))
		{
			Debug.Log("Setting invalid quest no " + QuestNo);
		}
		else
		{
			Quest.instance.QuestVariables[QuestNo] = value;
		}
	}

	public int x_skills(int mode, int skillToChange, int val3, int val4)
	{
		Debug.Log("X_skills (" + mode + "," + skillToChange + "," + val3 + "," + val4 + ")");
		if (UWEBase._RES != "UW2")
		{
			switch (mode)
			{
			case 10001:
				Debug.Log("Returning skill " + UWCharacter.Instance.PlayerSkills.GetSkillName(skillToChange + 1));
				return UWCharacter.Instance.PlayerSkills.GetSkill(skillToChange);
			case 10000:
				Debug.Log("Possibly setting skill to " + UWCharacter.Instance.PlayerSkills.GetSkillName(skillToChange + 1) + " " + 6);
				UWCharacter.Instance.PlayerSkills.AdvanceSkill(skillToChange, 6);
				return UWCharacter.Instance.PlayerSkills.GetSkill(skillToChange);
			default:
				Debug.Log("Possibly setting skill to " + UWCharacter.Instance.PlayerSkills.GetSkillName(skillToChange + 1) + " " + mode);
				UWCharacter.Instance.PlayerSkills.AdvanceSkill(skillToChange, mode);
				return UWCharacter.Instance.PlayerSkills.GetSkill(skillToChange);
			}
		}
		skillToChange++;
		switch (mode)
		{
		case 9999:
			return UWCharacter.Instance.PlayerSkills.GetSkill(skillToChange);
		case 10001:
			if (UWCharacter.Instance.TrainingPoints > 0)
			{
				UWCharacter.Instance.PlayerSkills.AdvanceSkill(skillToChange, 1);
				UWCharacter.Instance.TrainingPoints--;
				return 1;
			}
			return 0;
		default:
			UWCharacter.Instance.PlayerSkills.SetSkill(skillToChange, mode);
			return UWCharacter.Instance.PlayerSkills.GetSkill(skillToChange);
		}
	}

	public void set_likes_dislikes(int index1, int index2)
	{
		Debug.Log("set_likes_dislikes(" + index1 + "," + index2 + ")");
	}

	public int show_inv(int startObjectPos, int startObjectIDs)
	{
		int num = 0;
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			TradeSlot tradeSlot = UWHUD.instance.playerTrade[i];
			if (tradeSlot.isSelected())
			{
				stack.Set(startObjectPos + num, FindObjectIndexInObjectList(tradeSlot.objectInSlot.name));
				stack.Set(startObjectIDs + num, tradeSlot.GetObjectID());
				num++;
			}
		}
		return num;
	}

	public int give_to_npc(NPC npc, int start, int NoOfItems)
	{
		Container component = npc.gameObject.GetComponent<Container>();
		bool flag = false;
		ObjectInteraction[] array = new ObjectInteraction[4];
		for (int i = 0; i < NoOfItems; i++)
		{
			int index = stack.at(start + i);
			ObjectInteraction objectInteraction = FindGameObjectInObjectList(index);
			if (Container.GetFreeSlot(component) != -1)
			{
				if (objectInteraction != null)
				{
					ClearTradeSlotWithObject(index);
					array[i] = objectInteraction;
					objectInteraction.transform.position = GameWorldController.instance.InventoryMarker.transform.position;
					flag = true;
					component.AddItemToContainer(objectInteraction);
				}
				flag = true;
			}
			else if (objectInteraction != null)
			{
				ClearTradeSlotWithObject(index);
				array[i] = objectInteraction;
				objectInteraction.transform.position = npc.NPC_Launcher.transform.position;
				flag = false;
			}
		}
		for (int j = 0; j <= array.GetUpperBound(0); j++)
		{
			if (array[j] != null)
			{
				GameWorldController.MoveToWorld(array[j]);
				stack.Set(start + j, array[j].GetComponent<ObjectInteraction>().objectloaderinfo.index);
			}
		}
		if (flag)
		{
			return 1;
		}
		return 0;
	}

	public int take_from_npc(NPC npc, int arg1)
	{
		int result = 1;
		Container component = npc.gameObject.GetComponent<Container>();
		Container component2 = UWCharacter.Instance.gameObject.GetComponent<Container>();
		if (arg1 < 1000)
		{
			for (short num = 0; num <= component.MaxCapacity(); num++)
			{
				if (component.GetItemAt(num) != null)
				{
					ObjectInteraction itemAt = component.GetItemAt(num);
					if (itemAt != null && itemAt.item_id == arg1)
					{
						return TakeItemFromNPCCOntainer(npc, component2, num);
					}
				}
			}
		}
		else
		{
			int num2 = (arg1 - 1000) * 16;
			int num3 = num2 + 16;
			for (short num4 = 0; num4 <= component.MaxCapacity(); num4++)
			{
				if (component.GetItemAt(num4) != null)
				{
					ObjectInteraction itemAt2 = component.GetItemAt(num4);
					if ((arg1 >= 1000 && itemAt2.item_id >= num2 && itemAt2.item_id <= num3) || (arg1 < 1000 && itemAt2.item_id == arg1))
					{
						result = TakeItemFromNPCCOntainer(npc, component2, num4);
					}
				}
			}
		}
		return result;
	}

	private static int TakeItemFromNPCCOntainer(NPC npc, Container PlayerContainer, int index)
	{
		ObjectInteraction itemAt = npc.GetComponent<Container>().GetItemAt((short)index);
		if (Container.GetFreeSlot(PlayerContainer) != -1)
		{
			npc.GetComponent<Container>().RemoveItemFromContainer(itemAt);
			UWCharacter.Instance.Pickup(itemAt.GetComponent<ObjectInteraction>(), UWCharacter.Instance.playerInventory);
			return 1;
		}
		npc.GetComponent<Container>().RemoveItemFromContainer(itemAt);
		if (itemAt.transform.parent != GameWorldController.instance.DynamicObjectMarker())
		{
			GameWorldController.MoveToWorld(itemAt);
		}
		itemAt.transform.position = npc.NPC_Launcher.transform.position;
		if ((bool)itemAt.GetComponent<Container>())
		{
			Container component = itemAt.GetComponent<Container>();
			for (short num = 0; num <= component.MaxCapacity(); num++)
			{
				ObjectInteraction itemAt2 = component.GetItemAt(num);
				if (itemAt2 != null)
				{
					npc.GetComponent<Container>().RemoveItemFromContainer(itemAt2);
				}
			}
		}
		UWEBase.CurrentObjectInHand = itemAt;
		GameWorldController.MoveToInventory(itemAt);
		return 2;
	}

	private IEnumerator WaitForMore()
	{
		WaitingForMore = true;
		while (WaitingForMore)
		{
			yield return null;
		}
	}

	public void setup_to_barter(NPC npc)
	{
		Container component = npc.gameObject.GetComponent<Container>();
		int num = 0;
		npc.SetupNPCInventory();
		for (short num2 = 0; num2 <= component.MaxCapacity(); num2++)
		{
			ObjectInteraction itemAt = component.GetItemAt(num2);
			if (itemAt != null && num <= TradeSlot.TradeSlotUBound)
			{
				TradeSlot tradeSlot = UWHUD.instance.npcTrade[num++];
				tradeSlot.objectInSlot = itemAt;
				tradeSlot.SlotImage.texture = itemAt.GetInventoryDisplay().texture;
				int qty = itemAt.GetQty();
				if (qty <= 1)
				{
					tradeSlot.Quantity.text = "";
				}
				else
				{
					tradeSlot.Quantity.text = qty.ToString();
				}
			}
		}
	}

	public IEnumerator do_judgement(NPC npc)
	{
		int playerObjectCount = 0;
		int npcObjectCount = 0;
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			TradeSlot tradeSlot = UWHUD.instance.npcTrade[i];
			TradeSlot tradeSlot2 = UWHUD.instance.playerTrade[i];
			if (tradeSlot.isSelected())
			{
				npcObjectCount++;
			}
			if (tradeSlot2.isSelected())
			{
				playerObjectCount++;
			}
		}
		if (playerObjectCount < npcObjectCount)
		{
			yield return StartCoroutine(say_op("Player has the better deal", 2));
		}
		else if (playerObjectCount == npcObjectCount)
		{
			yield return StartCoroutine(say_op("It is an even deal", 2));
		}
		else
		{
			yield return StartCoroutine(say_op("NPC has the better deal", 2));
		}
	}

	public void do_decline(NPC npc)
	{
		Container currentContainer = UWCharacter.Instance.playerInventory.GetCurrentContainer();
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			TradeSlot tradeSlot = UWHUD.instance.playerTrade[i];
			if (tradeSlot.objectInSlot != null)
			{
				if (Container.GetFreeSlot(currentContainer) != -1)
				{
					currentContainer.AddItemToContainer(tradeSlot.objectInSlot);
					tradeSlot.clear();
					UWCharacter.Instance.GetComponent<PlayerInventory>().Refresh();
				}
				else
				{
					GameWorldController.MoveToWorld(tradeSlot.objectInSlot);
					tradeSlot.objectInSlot.transform.position = npc.NPC_Launcher.transform.position;
					tradeSlot.clear();
				}
			}
		}
		for (int j = 0; j <= TradeSlot.TradeSlotUBound; j++)
		{
			UWHUD.instance.npcTrade[j].clear();
		}
	}

	public IEnumerator do_offer(NPC npc, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7)
	{
		int NpcAnswer = UnityEngine.Random.Range(0, 2);
		stack.result_register = NpcAnswer;
		if (NpcAnswer == 1)
		{
			yield return StartCoroutine(say_op(arg5));
			for (int i = 0; i <= 3; i++)
			{
				TakeFromNPC(npc, i);
			}
			for (int j = 0; j <= 3; j++)
			{
				TakeFromPC(npc, j);
			}
			yield break;
		}
		switch (UnityEngine.Random.Range(1, 5))
		{
		case 1:
			yield return StartCoroutine(say_op(arg1));
			break;
		case 2:
			yield return StartCoroutine(say_op(arg2));
			break;
		case 3:
			yield return StartCoroutine(say_op(arg3));
			break;
		case 4:
			yield return StartCoroutine(say_op(arg4));
			break;
		}
	}

	public IEnumerator do_offer(NPC npc, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6)
	{
		yield return StartCoroutine(do_offer(npc, arg1, arg2, arg3, arg4, arg5, arg6, -1));
	}

	public IEnumerator do_demand(NPC npc, int arg1, int arg2)
	{
		int DemandResult = UnityEngine.Random.Range(0, 2);
		if (DemandResult == 1)
		{
			for (int i = 0; i <= 3; i++)
			{
				TakeFromNPC(npc, i);
			}
			yield return StartCoroutine(say_op(arg2));
		}
		else
		{
			yield return StartCoroutine(say_op(arg1));
		}
		RestorePCsInventory(npc);
		stack.result_register = DemandResult;
	}

	private void TakeFromNPC(NPC npc, int SlotNo)
	{
		Container currentContainer = UWCharacter.Instance.playerInventory.GetCurrentContainer();
		TradeSlot tradeSlot = UWHUD.instance.npcTrade[SlotNo];
		if (tradeSlot.isSelected())
		{
			if (Container.GetFreeSlot(currentContainer) != -1)
			{
				npc.GetComponent<Container>().RemoveItemFromContainer(tradeSlot.objectInSlot);
				currentContainer.AddItemToContainer(tradeSlot.objectInSlot);
				tradeSlot.objectInSlot.transform.parent = GameWorldController.instance.InventoryMarker.transform;
				GameWorldController.MoveToInventory(tradeSlot.objectInSlot);
				tradeSlot.objectInSlot.transform.position = Vector3.zero;
				tradeSlot.clear();
				UWCharacter.Instance.GetComponent<PlayerInventory>().Refresh();
			}
			else
			{
				tradeSlot.objectInSlot.transform.position = npc.NPC_Launcher.transform.position;
				npc.GetComponent<Container>().RemoveItemFromContainer(tradeSlot.objectInSlot);
				tradeSlot.clear();
				GameWorldController.MoveToWorld(tradeSlot.objectInSlot);
			}
		}
	}

	private void TakeFromPC(NPC npc, int slotNo)
	{
		Container component = npc.GetComponent<Container>();
		TradeSlot tradeSlot = UWHUD.instance.playerTrade[slotNo];
		if (tradeSlot.isSelected())
		{
			if (Container.GetFreeSlot(component) != -1)
			{
				GameWorldController.MoveToWorld(tradeSlot.objectInSlot);
				component.AddItemToContainer(tradeSlot.objectInSlot);
				tradeSlot.objectInSlot.transform.position = new Vector3(119f, 2.1f, 119f);
				tradeSlot.clear();
			}
			else
			{
				tradeSlot.objectInSlot.transform.position = npc.NPC_Launcher.transform.position;
				GameWorldController.MoveToWorld(tradeSlot.objectInSlot);
				tradeSlot.clear();
			}
		}
	}

	private void RestorePCsInventory(NPC npc)
	{
		Container currentContainer = UWCharacter.Instance.playerInventory.GetCurrentContainer();
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			TradeSlot tradeSlot = UWHUD.instance.playerTrade[i];
			if (tradeSlot.objectInSlot != null)
			{
				if (Container.GetFreeSlot(currentContainer) != -1)
				{
					currentContainer.AddItemToContainer(tradeSlot.objectInSlot);
					tradeSlot.clear();
					UWCharacter.Instance.GetComponent<PlayerInventory>().Refresh();
				}
				else
				{
					tradeSlot.clear();
					tradeSlot.objectInSlot.transform.position = npc.NPC_Launcher.transform.position;
					GameWorldController.MoveToWorld(tradeSlot.objectInSlot);
				}
			}
		}
	}

	public int gronk_door(int Action, int tileY, int tileX)
	{
		GameObject gameObject = GameObject.Find("door_" + tileX.ToString("D3") + "_" + tileY.ToString("D3"));
		if (gameObject != null)
		{
			DoorControl component = gameObject.GetComponent<DoorControl>();
			if (component != null)
			{
				if (Action == 0)
				{
					component.UnlockDoor(false);
					component.OpenDoor(1.3f);
				}
				else
				{
					component.CloseDoor(1.3f);
					component.LockDoor();
				}
				if (component.quality == 0)
				{
					return 0;
				}
				return 1;
			}
			Debug.Log(stack.instrp + " Unable to find doorcontrol to gronk  at " + tileX + " " + tileY);
			return 0;
		}
		Debug.Log("Unable to find door to gronk  at " + tileX + " " + tileY);
		return 0;
	}

	public void x_obj_stuff(int arg1, int arg2, int arg3, int link, int arg5, int owner, int quality, int item_id, int pos)
	{
		ObjectInteraction objectInteraction = null;
		pos = stack.at(pos);
		objectInteraction = FindObjectInteractionInObjectList(pos);
		if (objectInteraction == null)
		{
			Debug.Log("Obj not found in x_obj_stuff. Trying the last traded object");
			if (objectInteraction == null)
			{
				return;
			}
		}
		if (stack.at(link) <= 0)
		{
			if (UWEBase._RES == "UW2")
			{
				stack.Set(link, objectInteraction.link);
			}
			else
			{
				stack.Set(link, objectInteraction.link - 512);
			}
		}
		else if (objectInteraction.isQuant)
		{
			objectInteraction.link = stack.at(link);
		}
		else
		{
			objectInteraction.link = stack.at(link) + 512;
		}
		if (stack.at(owner) <= 0)
		{
			stack.Set(owner, objectInteraction.owner);
		}
		else
		{
			objectInteraction.owner = (short)stack.at(owner);
		}
		if (stack.at(quality) <= 0)
		{
			stack.Set(quality, objectInteraction.quality);
		}
		else
		{
			objectInteraction.quality = (short)stack.at(quality);
		}
		if (stack.at(item_id) <= 0)
		{
			stack.Set(item_id, objectInteraction.item_id);
		}
	}

	public int find_inv(NPC npc, int targetInventory, int item_id)
	{
		switch (targetInventory)
		{
		case 0:
		{
			Container component = npc.gameObject.GetComponent<Container>();
			for (short num3 = 0; num3 <= component.items.GetUpperBound(0); num3++)
			{
				ObjectInteraction itemAt = component.GetItemAt(num3);
				if (itemAt != null && itemAt.item_id == item_id)
				{
					return FindObjectIndexInObjectList(itemAt.name);
				}
			}
			break;
		}
		case 1:
		{
			int num = (item_id - 1000) * 16;
			int num2 = num + 16;
			ObjectInteraction objectInteraction = null;
			if (item_id >= 1000)
			{
				for (int i = num; i <= num2; i++)
				{
					objectInteraction = UWCharacter.Instance.playerInventory.findObjInteractionByID(i);
					if (objectInteraction != null)
					{
						break;
					}
				}
			}
			else
			{
				objectInteraction = UWCharacter.Instance.playerInventory.findObjInteractionByID(item_id);
			}
			Debug.Log("PC version of find_inv.");
			if (objectInteraction != null)
			{
				return 1;
			}
			return 0;
		}
		}
		return 0;
	}

	public int identify_inv(int pUNK1, int pStrPtr, int pUNK3, int pTradeSlot)
	{
		int index = stack.at(pTradeSlot);
		ObjectInteraction objectInteraction = FindObjectInteractionInObjectList(index);
		if (objectInteraction != null)
		{
			int value = GameWorldController.instance.commonObject.properties[objectInteraction.item_id].Value;
			int qty = objectInteraction.GetQty();
			if (pStrPtr >= 0)
			{
				string text = "";
				text = ((!objectInteraction.GetComponent<enchantment_base>()) ? StringController.instance.GetSimpleObjectNameUW(objectInteraction) : objectInteraction.GetComponent<enchantment_base>().DisplayEnchantment);
				stack.Set(pStrPtr, StringController.instance.AddString(conv[currConv].StringBlock, text));
			}
			return value * qty;
		}
		return 0;
	}

	public int contains(int pString1, int pString2)
	{
		string @string = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(pString2));
		string string2 = StringController.instance.GetString(conv[currConv].StringBlock, stack.at(pString1));
		Debug.Log("checking to see if " + @string + " contains " + string2);
		if (string2.Trim() == "")
		{
			return 0;
		}
		if (@string.ToUpper().Contains(string2.ToUpper()))
		{
			return 1;
		}
		return 0;
	}

	public void set_race_attitude(NPC npc, int unk1, int attitude, int Race)
	{
		Debug.Log(npc.name + " Set Race attitude " + unk1 + " " + attitude + " " + Race);
		Collider[] array = Physics.OverlapSphere(npc.transform.position, 4f);
		foreach (Collider collider in array)
		{
			if (collider.gameObject.GetComponent<NPC>() != null && collider.gameObject.GetComponent<NPC>().GetRace() == Race)
			{
				collider.gameObject.GetComponent<NPC>().npc_attitude = (short)attitude;
				if (attitude == 0)
				{
					collider.gameObject.GetComponent<NPC>().npc_gtarg = 5;
					collider.gameObject.GetComponent<NPC>().gtarg = UWCharacter.Instance.gameObject;
					collider.gameObject.GetComponent<NPC>().gtargName = UWCharacter.Instance.gameObject.name;
					collider.gameObject.GetComponent<NPC>().npc_goal = 5;
				}
			}
		}
	}

	public void set_attitude(int attitude, int target_whoami)
	{
		NPC[] componentsInChildren = GameWorldController.instance.DynamicObjectMarker().GetComponentsInChildren<NPC>();
		for (int i = 0; i < componentsInChildren.GetUpperBound(0); i++)
		{
			if (componentsInChildren[i].npc_whoami == target_whoami)
			{
				componentsInChildren[i].npc_attitude = (short)attitude;
			}
		}
	}

	public int compare(int StringIndex1, int StringIndex2)
	{
		if (StringController.instance.GetString(conv[currConv].StringBlock, StringIndex1).ToUpper() == StringController.instance.GetString(conv[currConv].StringBlock, StringIndex2).ToUpper())
		{
			return 1;
		}
		return 0;
	}

	public int count_inv(int ItemPos)
	{
		ObjectInteraction objectInteraction = FindObjectInteractionInObjectList(ItemPos);
		if (objectInteraction != null)
		{
			return objectInteraction.GetQty();
		}
		return 0;
	}

	public void remove_talker(NPC npc)
	{
		if (npc.Agent != null)
		{
			npc.Agent.enabled = false;
		}
		npc.gameObject.transform.position = UWCharacter.Instance.playerInventory.InventoryMarker.transform.position;
	}

	public void give_ptr_npc(NPC npc, int Quantity, int ptrSlotNo)
	{
		int index = stack.at(ptrSlotNo);
		int[] array = new int[4];
		for (int i = 1; i <= array.GetUpperBound(0); i++)
		{
			if (stack.at(ptrSlotNo + i) >= 1024)
			{
				array[i] = ptrSlotNo + i;
			}
		}
		Container component = npc.gameObject.GetComponent<Container>();
		ObjectInteraction objectInteraction = FindGameObjectInObjectList(index);
		if (objectInteraction != null)
		{
			if (Quantity == -1)
			{
				ClearTradeSlotWithObject(index);
				objectInteraction.transform.parent = GameWorldController.instance.DynamicObjectMarker().transform;
				component.AddItemToContainer(objectInteraction);
				GameWorldController.MoveToWorld(objectInteraction);
				UWCharacter.Instance.playerInventory.Refresh();
			}
			else if (objectInteraction != null)
			{
				if (objectInteraction.isQuant && objectInteraction.link > 1 && !objectInteraction.isEnchanted && objectInteraction.link != Quantity)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(objectInteraction.gameObject);
					gameObject.GetComponent<ObjectInteraction>().link = Quantity;
					objectInteraction.GetComponent<ObjectInteraction>().link = objectInteraction.GetComponent<ObjectInteraction>().link - Quantity;
					GameWorldController.MoveToWorld(objectInteraction);
					gameObject.name = ObjectLoader.UniqueObjectName(gameObject.GetComponent<ObjectInteraction>().objectloaderinfo);
					component.AddItemToContainer(objectInteraction);
				}
				else
				{
					ClearTradeSlotWithObject(index);
					component.AddItemToContainer(objectInteraction);
					objectInteraction.transform.parent = GameWorldController.instance.DynamicObjectMarker().transform;
					GameWorldController.MoveToWorld(objectInteraction.GetComponent<ObjectInteraction>());
					UWCharacter.Instance.playerInventory.Refresh();
				}
			}
		}
		stack.Set(ptrSlotNo, FindObjectIndexInObjectList(objectInteraction.name));
	}

	public int take_id_from_npc(NPC npc, int index)
	{
		if (index > UWEBase.CurrentObjectList().objInfo.GetUpperBound(0))
		{
			Debug.Log("Index out of range in take_id_from_npc");
			return 2;
		}
		ObjectInteraction objectInteraction = UWEBase.CurrentObjectList().objInfo[index].instance;
		int num = 1;
		Container component = UWCharacter.Instance.gameObject.GetComponent<Container>();
		if (objectInteraction == null)
		{
			return 1;
		}
		if (Container.GetFreeSlot(component) != -1)
		{
			npc.GetComponent<Container>().RemoveItemFromContainer(objectInteraction);
			component.AddItemToContainer(objectInteraction);
			GameWorldController.MoveToInventory(objectInteraction);
			objectInteraction.transform.parent = GameWorldController.instance.InventoryMarker.transform;
			if ((bool)objectInteraction.GetComponent<Container>())
			{
				Container component2 = objectInteraction.GetComponent<Container>();
				for (short num2 = 0; num2 <= component2.MaxCapacity(); num2++)
				{
					ObjectInteraction itemAt = component2.GetItemAt(num2);
					if (itemAt != null)
					{
						npc.GetComponent<Container>().RemoveItemFromContainer(itemAt);
						itemAt.transform.parent = UWCharacter.Instance.playerInventory.InventoryMarker.transform;
						GameWorldController.MoveToInventory(itemAt);
					}
				}
			}
			UWCharacter.Instance.GetComponent<PlayerInventory>().Refresh();
			num = 1;
		}
		else
		{
			num = 2;
			objectInteraction.transform.parent = GameWorldController.instance.DynamicObjectMarker();
			objectInteraction.transform.position = npc.NPC_Launcher.transform.position;
			npc.GetComponent<Container>().RemoveItemFromContainer(objectInteraction);
			if (objectInteraction.GetComponent<Container>() != null)
			{
				Container component3 = objectInteraction.GetComponent<Container>();
				for (short num3 = 0; num3 <= component3.MaxCapacity(); num3++)
				{
					ObjectInteraction itemAt2 = component3.GetItemAt(num3);
					if (itemAt2 != null && itemAt2 != null)
					{
						npc.GetComponent<Container>().RemoveItemFromContainer(itemAt2);
					}
				}
			}
		}
		return num;
	}

	public int take_from_npc_inv(NPC npc, int pos)
	{
		pos--;
		ObjectInteraction itemAt = npc.GetComponent<Container>().GetItemAt((short)pos);
		if (itemAt != null)
		{
			return itemAt.objectloaderinfo.index;
		}
		return 0;
	}

	public int find_barter(int itemID)
	{
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			if (!UWHUD.instance.playerTrade[i].isSelected())
			{
				continue;
			}
			ObjectInteraction gameObjectInteraction = UWHUD.instance.playerTrade[i].GetGameObjectInteraction();
			if (!(gameObjectInteraction != null))
			{
				continue;
			}
			if (itemID < 1000)
			{
				if (gameObjectInteraction.item_id == itemID)
				{
					return FindObjectIndexInObjectList(gameObjectInteraction.name);
				}
			}
			else if (gameObjectInteraction.item_id >= (itemID - 1000) * 16 && gameObjectInteraction.item_id < (itemID + 1 - 1000) * 16)
			{
				return FindObjectIndexInObjectList(gameObjectInteraction.name);
			}
		}
		return 0;
	}

	public int length(int str)
	{
		return StringController.instance.GetString(conv[currConv].StringBlock, str).Length;
	}

	public int sex(int ParamFemale, int ParamMale)
	{
		if (UWCharacter.Instance.isFemale)
		{
			return ParamFemale;
		}
		return ParamMale;
	}

	public int find_barter_total(int ptrCount, int ptrSlot, int ptrNoOfSlots, int item_id)
	{
		ObjectInteraction objectInteraction = null;
		int num = 0;
		stack.Set(ptrNoOfSlots, 0);
		stack.Set(ptrCount, 0);
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			if (!UWHUD.instance.playerTrade[i].isSelected())
			{
				continue;
			}
			objectInteraction = UWHUD.instance.playerTrade[i].GetGameObjectInteraction();
			if (objectInteraction != null)
			{
				int item_id2 = objectInteraction.item_id;
				int qty = objectInteraction.GetQty();
				if (item_id == item_id2)
				{
					stack.Set(ptrCount, stack.at(ptrCount) + qty);
					stack.Set(ptrSlot + num++, FindObjectIndexInObjectList(objectInteraction.name));
					stack.Set(ptrNoOfSlots, stack.at(ptrNoOfSlots) + 1);
				}
			}
		}
		return stack.StackValues[ptrCount];
	}

	public int do_inv_create(NPC npc, int item_id)
	{
		ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(item_id, 0, 0, 1, 256);
		objectLoaderInfo.is_quant = 1;
		ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, GameWorldController.instance.InventoryMarker.transform.position);
		BuildObjectList();
		npc.GetComponent<Container>().AddItemToContainer(objectInteraction);
		return objectInteraction.GetComponent<ObjectInteraction>().objectloaderinfo.index;
	}

	public void place_object(int tileY, int tileX, int index)
	{
		ObjectInteraction objectInteraction = UWEBase.CurrentObjectList().objInfo[index].instance;
		if (objectInteraction != null)
		{
			GameObject gameObject = objectInteraction.gameObject;
			gameObject.transform.position = UWEBase.CurrentTileMap().getTileVector(tileX, tileY);
		}
	}

	public void do_inv_delete(NPC npc, int item_id)
	{
		Container component = npc.GetComponent<Container>();
		if (component != null)
		{
			ObjectInteraction objectInteraction = component.findItemOfType(item_id);
			if (objectInteraction != null)
			{
				component.RemoveItemFromContainer(objectInteraction);
				objectInteraction.consumeObject();
			}
		}
	}

	public int x_traps(int VariableValue, int VariableIndex)
	{
		Debug.Log("x_traps :" + VariableValue + " " + VariableIndex);
		if (VariableValue <= Quest.instance.variables.GetUpperBound(0))
		{
			Quest.instance.variables[VariableIndex] = VariableValue;
		}
		return Quest.instance.variables[VariableIndex];
	}

	private void switch_pic(int PortraitNo)
	{
		GRLoader gRLoader = new GRLoader(7);
		RawImage rawImage = UWHUD.instance.ConversationPortraits[1];
		rawImage.texture = gRLoader.LoadImageAt(PortraitNo - 1);
		UWHUD.instance.NPCName.text = StringController.instance.GetString(7, PortraitNo + 16);
	}

	private void x_clock(int unk1, int unk2)
	{
		if (unk1 == 10001)
		{
			Debug.Log("x_clock returning: " + Quest.instance.x_clocks[unk2] + " from " + unk2);
			stack.result_register = Quest.instance.x_clocks[unk2];
		}
		else
		{
			Debug.Log("x_clock setting: " + unk2 + " to " + unk1);
			Quest.instance.x_clocks[unk2] = unk1;
		}
	}

	private void x_exp(int xpToAdd)
	{
		UWCharacter.Instance.AddXP(xpToAdd);
	}

	private void PrintImportedVariable(int index, int newValue)
	{
		for (int i = 0; i <= conv[currConv].functions.GetUpperBound(0) && (conv[currConv].functions[i].ID_or_Address != index || conv[currConv].functions[i].import_type != 271); i++)
		{
		}
	}

	public int check_inv_quality(int itemPos)
	{
		ObjectInteraction objectInteraction = FindObjectInteractionInObjectList(itemPos);
		if (objectInteraction != null)
		{
			return objectInteraction.quality;
		}
		return 0;
	}

	private void x_obj_pos(int arg1, int arg2, int arg3, int arg4, int arg5)
	{
		Debug.Log("x_obj_pos (" + arg1 + "," + arg2 + "," + arg3 + "," + arg4 + "," + arg5);
	}

	private void teleport_talker(NPC npc, int tileY, int tileX)
	{
		Debug.Log("moving " + npc.name + " to " + tileX + " " + tileY);
		npc.Agent.Warp(UWEBase.CurrentTileMap().getTileVector(tileX, tileY));
	}

	private void babl_hackJospurDebt(int arg)
	{
		stack.result_register = Quest.instance.QuestVariables[133];
	}

	private void babl_hackSetUpFight(int arg, int NoOfFighters, int arena, int unk)
	{
		Debug.Log("Setting up a fight with " + NoOfFighters + " in arena " + arena);
		SettingUpFight = true;
		Quest.instance.QuestVariables[133] = NoOfFighters * 5;
		Quest.instance.FightingInArena = true;
		int[] array = new int[5];
		int[] array2 = new int[5];
		switch (arena)
		{
		case 0:
			array[0] = 37;
			array2[0] = 25;
			array[1] = 36;
			array2[1] = 35;
			array[2] = 35;
			array2[2] = 35;
			array[3] = 35;
			array2[3] = 36;
			array[4] = 35;
			array[4] = 37;
			break;
		case 1:
			array[0] = 27;
			array2[0] = 37;
			array[1] = 27;
			array2[1] = 36;
			array[2] = 27;
			array2[2] = 35;
			array[3] = 26;
			array2[3] = 35;
			array[4] = 25;
			array[4] = 35;
			break;
		case 2:
			array[0] = 25;
			array2[0] = 27;
			array[1] = 26;
			array2[1] = 27;
			array[2] = 27;
			array2[2] = 27;
			array[3] = 27;
			array2[3] = 26;
			array[4] = 27;
			array[4] = 25;
			break;
		default:
			array[0] = 36;
			array2[0] = 27;
			array[1] = 35;
			array2[1] = 26;
			array[2] = 36;
			array2[2] = 27;
			array[3] = 35;
			array2[3] = 26;
			array[4] = 37;
			array[4] = 27;
			break;
		}
		for (int i = 0; i < NoOfFighters; i++)
		{
			ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(UnityEngine.Random.Range(120, 122), 36, 27, 0, 1);
			Vector3 tileVector = UWEBase.CurrentTileMap().getTileVector(array[i], array2[i]);
			ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, tileVector);
			objectInteraction.GetComponent<NPC>().npc_attitude = 0;
			objectInteraction.GetComponent<NPC>().npc_goal = 5;
			objectInteraction.GetComponent<NPC>().npc_hp = 49;
			objectInteraction.GetComponent<NPC>().npc_xhome = (short)array[i];
			objectInteraction.GetComponent<NPC>().npc_yhome = (short)array2[i];
			objectInteraction.GetComponent<NPC>().npc_whoami = 102;
			Quest.instance.ArenaOpponents[i] = objectLoaderInfo.index;
		}
	}

	private void teleport_player(int level, int tileY, int tileX)
	{
		Debug.Log("teleporting to " + level + " " + tileX + " " + tileY);
		Teleport = true;
		TeleportLevel = level - 1;
		TeleportTileX = tileX;
		TeleportTileY = tileY;
	}

	public void set_inv_quality(int NewQuality, int itemIndex)
	{
		if (itemIndex <= UWEBase.CurrentObjectList().objInfo.GetUpperBound(0))
		{
			if (UWEBase.CurrentObjectList().objInfo[itemIndex].instance != null)
			{
				UWEBase.CurrentObjectList().objInfo[itemIndex].instance.quality = (short)NewQuality;
			}
		}
		else
		{
			Debug.Log("itemIndex out of range in set_inv_quality");
		}
	}

	public static void BuildObjectList()
	{
		ObjectLoader.UpdateObjectList(UWEBase.CurrentTileMap(), UWEBase.CurrentObjectList());
		int childCount = GameWorldController.instance.InventoryMarker.transform.childCount;
		ObjectMasterList = new string[1024 + childCount + 1];
		ObjectLoaderInfo[] objInfo = UWEBase.CurrentObjectList().objInfo;
		for (int i = 0; i < 1024; i++)
		{
			if (objInfo[i].instance != null)
			{
				ObjectMasterList[i] = objInfo[i].instance.name;
			}
			else
			{
				ObjectMasterList[i] = "";
			}
		}
		for (int j = 1024; j < 1024 + childCount; j++)
		{
			ObjectMasterList[j] = GameWorldController.instance.InventoryMarker.transform.GetChild(j - 1024).gameObject.name;
		}
	}

	private static int FindObjectIndexInObjectList(string objectName)
	{
		for (int i = 0; i <= ObjectMasterList.GetUpperBound(0); i++)
		{
			if (ObjectMasterList[i] == objectName)
			{
				return i;
			}
		}
		return 0;
	}

	private static ObjectInteraction FindObjectInteractionInObjectList(int index)
	{
		return FindGameObjectInObjectList(index);
	}

	private static ObjectInteraction FindGameObjectInObjectList(int index)
	{
		string text = ObjectMasterList[index];
		if (text != "")
		{
			GameObject gameObject = GameObject.Find(text);
			return gameObject.GetComponent<ObjectInteraction>();
		}
		return null;
	}

	private static void ClearTradeSlotWithObject(int index)
	{
		TradeSlot tradeSlot = FindTradeSlotWithItem(index);
		if (tradeSlot != null)
		{
			tradeSlot.clear();
		}
	}

	private static TradeSlot FindTradeSlotWithItem(int index)
	{
		string text = ObjectMasterList[index];
		if (text == "")
		{
			return null;
		}
		for (int i = 0; i <= TradeSlot.TradeSlotUBound; i++)
		{
			if (UWHUD.instance.playerTrade[i].objectInSlot != null && UWHUD.instance.playerTrade[i].objectInSlot.name == text)
			{
				return UWHUD.instance.playerTrade[i];
			}
		}
		for (int j = 0; j <= UWHUD.instance.npcTrade.GetUpperBound(0); j++)
		{
			if (UWHUD.instance.npcTrade[j].objectInSlot != null && UWHUD.instance.npcTrade[j].objectInSlot.name == text)
			{
				return UWHUD.instance.npcTrade[j];
			}
		}
		return null;
	}
}
