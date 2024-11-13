using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellEffectsDisplay : GuiBase_Draggable
{
	public int SlotNumber;

	private int setSpell = -1;

	private RawImage thisSpell;

	private static Texture2D SpellBlank;

	public override void Start()
	{
		base.Start();
		thisSpell = GetComponent<RawImage>();
		if (SpellBlank == null)
		{
			SpellBlank = Resources.Load<Texture2D>(UWEBase._RES + "/HUD/Runes/rune_blank");
		}
	}

	public override void Update()
	{
		base.Update();
		if (UWCharacter.Instance.ActiveSpell[SlotNumber] != null)
		{
			if (UWCharacter.Instance.ActiveSpell[SlotNumber].EffectIcon() != setSpell)
			{
				setSpell = UWCharacter.Instance.ActiveSpell[SlotNumber].EffectIcon();
				if (setSpell > -1)
				{
					thisSpell.texture = GameWorldController.instance.SpellIcons.LoadImageAt(UWCharacter.Instance.ActiveSpell[SlotNumber].EffectIcon());
				}
				else
				{
					thisSpell.texture = SpellBlank;
				}
			}
		}
		else if (setSpell >= -1)
		{
			thisSpell.texture = SpellBlank;
			setSpell = -2;
		}
	}

	public void OnClick(BaseEventData evnt)
	{
		if (!Dragging)
		{
			PointerEventData pointerEventData = (PointerEventData)evnt;
			ClickEvent(pointerEventData.pointerId);
		}
	}

	private void ClickEvent(int ptrID)
	{
		if (!(UWCharacter.Instance.ActiveSpell[SlotNumber] == null))
		{
			switch (ptrID)
			{
			case -1:
				UWCharacter.Instance.ActiveSpell[SlotNumber].CancelEffect();
				break;
			case -2:
				UWHUD.instance.MessageScroll.Add(UWCharacter.Instance.ActiveSpell[SlotNumber].getSpellDescription());
				break;
			}
		}
	}
}
