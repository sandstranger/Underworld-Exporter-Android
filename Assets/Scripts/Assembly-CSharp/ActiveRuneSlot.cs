using UnityEngine;
using UnityEngine.UI;

public class ActiveRuneSlot : GuiBase_Draggable
{
	private RawImage thisRune;

	private static Texture2D[] runes = new Texture2D[24];

	private static Texture2D blank;

	public override void Start()
	{
		base.Start();
		thisRune = GetComponent<RawImage>();
		for (int i = 0; i < 24; i++)
		{
			if (runes[i] == null)
			{
				runes[i] = GameWorldController.instance.ObjectArt.LoadImageAt(232 + i);
			}
		}
		if (blank == null)
		{
			blank = Resources.Load<Texture2D>(UWEBase._RES + "/HUD/Runes/rune_blank");
		}
	}

	public static void UpdateRuneSlots()
	{
		for (int i = 0; i < 3; i++)
		{
			if (UWCharacter.Instance.PlayerMagic.ActiveRunes[i] != -1)
			{
				UWHUD.instance.activeRunes[i].thisRune.texture = runes[UWCharacter.Instance.PlayerMagic.ActiveRunes[i]];
			}
			else
			{
				UWHUD.instance.activeRunes[i].thisRune.texture = blank;
			}
		}
	}

	public void OnClick()
	{
		if (!UWHUD.instance.window.JustClicked && !Dragging && !WindowDetect.InMap && !WindowDetect.WaitingForInput && !ConversationVM.InConversation && Character.InteractionMode != 0 && UWCharacter.Instance.PlayerMagic.ReadiedSpell == "" && UWCharacter.Instance.PlayerMagic.TestSpellCast(UWCharacter.Instance, UWCharacter.Instance.PlayerMagic.ActiveRunes[0], UWCharacter.Instance.PlayerMagic.ActiveRunes[1], UWCharacter.Instance.PlayerMagic.ActiveRunes[2]))
		{
			UWCharacter.Instance.PlayerMagic.castSpell(UWCharacter.Instance.gameObject, UWCharacter.Instance.PlayerMagic.ActiveRunes[0], UWCharacter.Instance.PlayerMagic.ActiveRunes[1], UWCharacter.Instance.PlayerMagic.ActiveRunes[2], true);
			UWCharacter.Instance.PlayerMagic.ApplySpellCost();
		}
	}
}
