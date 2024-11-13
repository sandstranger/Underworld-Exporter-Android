using System;
using TMPro;
using UnityEngine.UI;

public class ScrollController : GuiBase
{
	public int LineWidth = 65;

	public string[] txtToDisplay = new string[5];

	public Text NewUIOUt;

	public TextMeshProUGUI TextOutput;

	public int ptr;

	public int MaxEntries;

	public bool useDragon;

	public void Add(string WhatToSay, string ColourToUse)
	{
		string[] array = ParseParagraphs(ref WhatToSay);
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			ListAdd(ColourToUse + array[i] + "</color>");
		}
		PrintList();
	}

	public void Add(string WhatToSay)
	{
		string[] array = ParseParagraphs(ref WhatToSay);
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			ListAdd(array[i]);
		}
		PrintList();
	}

	private string[] ParseParagraphs(ref string WhatToSay)
	{
		if (WhatToSay == null)
		{
			WhatToSay = "";
		}
		WhatToSay = SplitText(WhatToSay);
		WhatToSay = WhatToSay.Replace("\\n", "\n");
		WhatToSay = WhatToSay.TrimEnd();
		return WhatToSay.Split(new string[1] { "\n" }, StringSplitOptions.None);
	}

	public void Set(string text)
	{
		Clear();
		Add(text);
		PrintList();
	}

	public void DirectSet(string text)
	{
		if (NewUIOUt != null)
		{
			NewUIOUt.text = text;
		}
		if (TextOutput != null)
		{
			TextOutput.text = text;
		}
	}

	public void Clear()
	{
		for (int i = 0; i <= txtToDisplay.GetUpperBound(0); i++)
		{
			txtToDisplay[i] = "";
		}
		ptr = 0;
		DirectSet("");
	}

	public void ListAdd(string text)
	{
		if (ptr == MaxEntries)
		{
			for (int i = 0; i < txtToDisplay.GetUpperBound(0); i++)
			{
				txtToDisplay[i] = txtToDisplay[i + 1];
			}
			txtToDisplay[ptr - 1] = text;
		}
		else
		{
			txtToDisplay[ptr++] = text;
		}
	}

	public void PrintList()
	{
		string text = "";
		for (int i = 0; i < ptr; i++)
		{
			text = text + txtToDisplay[i] + "\n";
		}
		DirectSet(text);
		if (useDragon)
		{
			Dragons.MoveScroll();
		}
	}

	private string SplitText(string text)
	{
		if (text.Length <= LineWidth)
		{
			return text;
		}
		int num = text.Substring(0, LineWidth).LastIndexOf(" ");
		char[] array = text.ToCharArray();
		array[num] = '\n';
		text = new string(array);
		if (text.Length - LineWidth > LineWidth)
		{
			return text.Substring(0, LineWidth) + SplitText(text.Substring(LineWidth));
		}
		return text;
	}
}
