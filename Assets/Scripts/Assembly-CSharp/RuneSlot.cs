using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneSlot : GuiBase
{
	public int SlotNumber;

	private RawImage thisRune;

	public bool isSet;

	public override void Start()
	{
		base.Start();
		thisRune = GetComponent<RawImage>();
	}

	public static void UpdateRuneDisplay()
	{
		for (int i = 0; i < 24; i++)
		{
			if (UWHUD.instance.runes[i].thisRune == null)
			{
				UWHUD.instance.runes[i].thisRune = UWHUD.instance.runes[i].gameObject.GetComponent<RawImage>();
			}
			if (UWCharacter.Instance.PlayerMagic.PlayerRunes[i])
			{
				UWHUD.instance.runes[i].thisRune.texture = GameWorldController.instance.ObjectArt.LoadImageAt(232 + i);
				UWHUD.instance.runes[i].isSet = true;
			}
			else
			{
				UWHUD.instance.runes[i].thisRune.texture = Resources.Load<Texture2D>(UWEBase._RES + "/HUD/Runes/rune_blank");
				UWHUD.instance.runes[i].isSet = false;
			}
		}
	}

	public void OnClick(BaseEventData evnt)
	{
		PointerEventData pointerEventData = (PointerEventData)evnt;
		ClickEvent(pointerEventData.pointerId);
	}

	public void ClickEvent(int ptrID)
	{
		if (!UWCharacter.Instance.PlayerMagic.PlayerRunes[SlotNumber])
		{
			return;
		}
		if (ptrID == -1)
		{
			if (UWCharacter.Instance.PlayerMagic.ActiveRunes[0] == -1)
			{
				UWCharacter.Instance.PlayerMagic.ActiveRunes[0] = SlotNumber;
			}
			else if (UWCharacter.Instance.PlayerMagic.ActiveRunes[1] == -1)
			{
				UWCharacter.Instance.PlayerMagic.ActiveRunes[1] = SlotNumber;
			}
			else if (UWCharacter.Instance.PlayerMagic.ActiveRunes[2] == -1)
			{
				UWCharacter.Instance.PlayerMagic.ActiveRunes[2] = SlotNumber;
			}
			else
			{
				UWCharacter.Instance.PlayerMagic.ActiveRunes[0] = UWCharacter.Instance.PlayerMagic.ActiveRunes[1];
				UWCharacter.Instance.PlayerMagic.ActiveRunes[1] = UWCharacter.Instance.PlayerMagic.ActiveRunes[2];
				UWCharacter.Instance.PlayerMagic.ActiveRunes[2] = SlotNumber;
			}
			ActiveRuneSlot.UpdateRuneSlots();
		}
		else
		{
			UWHUD.instance.MessageScroll.Add("You see " + StringController.instance.GetSimpleObjectNameUW(232 + SlotNumber));
		}
	}
}
