using System.Collections.Generic;
using UnityEngine;

public class KeyBindings : GuiBase
{
	public Dictionary<string, KeyCode> chartoKeycode = new Dictionary<string, KeyCode>
	{
		{
			".",
			KeyCode.KeypadPeriod
		},
		{
			"/",
			KeyCode.KeypadDivide
		},
		{
			"*",
			KeyCode.KeypadMultiply
		},
		{
			"+",
			KeyCode.KeypadPlus
		},
		{
			"`",
			KeyCode.BackQuote
		},
		{
			"-",
			KeyCode.Minus
		},
		{
			"=",
			KeyCode.Equals
		},
		{
			"[",
			KeyCode.LeftBracket
		},
		{
			"]",
			KeyCode.RightBracket
		},
		{
			"\\'",
			KeyCode.Backslash
		},
		{
			";",
			KeyCode.Semicolon
		},
		{
			"'",
			KeyCode.Quote
		},
		{
			",",
			KeyCode.Comma
		},
		{
			"1",
			KeyCode.Keypad1
		},
		{
			"2",
			KeyCode.Keypad2
		},
		{
			"3",
			KeyCode.Keypad3
		},
		{
			"4",
			KeyCode.Keypad4
		},
		{
			"5",
			KeyCode.Keypad5
		},
		{
			"6",
			KeyCode.Keypad6
		},
		{
			"7",
			KeyCode.Keypad7
		},
		{
			"8",
			KeyCode.Keypad8
		},
		{
			"9",
			KeyCode.Keypad9
		},
		{
			"0",
			KeyCode.Keypad0
		},
		{
			"a",
			KeyCode.A
		},
		{
			"b",
			KeyCode.B
		},
		{
			"c",
			KeyCode.C
		},
		{
			"d",
			KeyCode.D
		},
		{
			"e",
			KeyCode.E
		},
		{
			"f",
			KeyCode.F
		},
		{
			"g",
			KeyCode.G
		},
		{
			"h",
			KeyCode.H
		},
		{
			"i",
			KeyCode.I
		},
		{
			"j",
			KeyCode.J
		},
		{
			"k",
			KeyCode.K
		},
		{
			"l",
			KeyCode.L
		},
		{
			"m",
			KeyCode.M
		},
		{
			"n",
			KeyCode.N
		},
		{
			"o",
			KeyCode.O
		},
		{
			"p",
			KeyCode.P
		},
		{
			"q",
			KeyCode.Q
		},
		{
			"r",
			KeyCode.R
		},
		{
			"s",
			KeyCode.S
		},
		{
			"t",
			KeyCode.T
		},
		{
			"u",
			KeyCode.U
		},
		{
			"v",
			KeyCode.V
		},
		{
			"w",
			KeyCode.W
		},
		{
			"x",
			KeyCode.X
		},
		{
			"y",
			KeyCode.Y
		},
		{
			"z",
			KeyCode.Z
		},
		{
			"f1",
			KeyCode.F1
		},
		{
			"f2",
			KeyCode.F2
		},
		{
			"f3",
			KeyCode.F3
		},
		{
			"f4",
			KeyCode.F4
		},
		{
			"f5",
			KeyCode.F5
		},
		{
			"f6",
			KeyCode.F6
		},
		{
			"f7",
			KeyCode.F7
		},
		{
			"f8",
			KeyCode.F8
		},
		{
			"f9",
			KeyCode.F9
		},
		{
			"f10",
			KeyCode.F10
		},
		{
			"f11",
			KeyCode.F11
		},
		{
			"f12",
			KeyCode.F12
		}
	};

	public static KeyBindings instance;

	public KeyCode FlyUp = KeyCode.R;

	public KeyCode FlyDown = KeyCode.V;

	public KeyCode ToggleMouseLook = KeyCode.E;

	public KeyCode ToggleFullScreen = KeyCode.F;

	public KeyCode InteractionOptions = KeyCode.F1;

	public KeyCode InteractionTalk = KeyCode.F2;

	public KeyCode InteractionPickup = KeyCode.F3;

	public KeyCode InteractionLook = KeyCode.F4;

	public KeyCode InteractionAttack = KeyCode.F5;

	public KeyCode InteractionUse = KeyCode.F6;

	public KeyCode CastSpell = KeyCode.Q;

	public KeyCode TrackSkill = KeyCode.T;

	private void Awake()
	{
		instance = this;
	}

	public void ApplyBindings()
	{
		UWHUD.instance.InteractionControlUW1.ControlItems[0].ShortCutKey = InteractionOptions;
		UWHUD.instance.InteractionControlUW1.ControlItems[1].ShortCutKey = InteractionTalk;
		UWHUD.instance.InteractionControlUW1.ControlItems[2].ShortCutKey = InteractionPickup;
		UWHUD.instance.InteractionControlUW1.ControlItems[3].ShortCutKey = InteractionLook;
		UWHUD.instance.InteractionControlUW1.ControlItems[4].ShortCutKey = InteractionAttack;
		UWHUD.instance.InteractionControlUW1.ControlItems[5].ShortCutKey = InteractionUse;
		UWHUD.instance.InteractionControlUW2.ControlItems[0].ShortCutKey = InteractionOptions;
		UWHUD.instance.InteractionControlUW2.ControlItems[1].ShortCutKey = InteractionTalk;
		UWHUD.instance.InteractionControlUW2.ControlItems[2].ShortCutKey = InteractionPickup;
		UWHUD.instance.InteractionControlUW2.ControlItems[3].ShortCutKey = InteractionLook;
		UWHUD.instance.InteractionControlUW2.ControlItems[4].ShortCutKey = InteractionAttack;
		UWHUD.instance.InteractionControlUW2.ControlItems[5].ShortCutKey = InteractionUse;
	}
}
