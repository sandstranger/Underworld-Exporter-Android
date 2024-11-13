using UnityEngine.UI;

public class HealthFlask : GuiBase_Draggable
{
	public RawImage[] LevelImages = new RawImage[13];

	public float Level;

	public float MaxLevel;

	public float FlaskLevel;

	private float PreviousLevel;

	private float PreviousMaxLevel;

	public bool isHealthDisplay;

	public override void Start()
	{
		for (int i = 0; i <= LevelImages.GetUpperBound(0); i++)
		{
			if (isHealthDisplay)
			{
				LevelImages[i].texture = GameWorldController.instance.grFlasks.LoadImageAt(i);
			}
			else
			{
				LevelImages[i].texture = GameWorldController.instance.grFlasks.LoadImageAt(25 + i);
			}
		}
		GetComponent<RawImage>().texture = GameWorldController.instance.grFlasks.LoadImageAt(75);
	}

	public void UpdateFlaskDisplay()
	{
		if (isHealthDisplay)
		{
			Level = UWCharacter.Instance.CurVIT;
			MaxLevel = UWCharacter.Instance.MaxVIT;
		}
		else
		{
			Level = UWCharacter.Instance.PlayerMagic.CurMana;
			MaxLevel = UWCharacter.Instance.PlayerMagic.MaxMana;
		}
		if (Level == PreviousLevel && MaxLevel == PreviousMaxLevel)
		{
			return;
		}
		PreviousLevel = Level;
		PreviousMaxLevel = MaxLevel;
		if (MaxLevel > 0f)
		{
			FlaskLevel = Level / MaxLevel * 13f;
			for (int i = 0; i < 13; i++)
			{
				LevelImages[i].enabled = (float)i < FlaskLevel;
			}
		}
		else
		{
			for (int j = 0; j < 13; j++)
			{
				LevelImages[j].enabled = false;
			}
		}
	}

	public void UpdatePoisonDisplay(bool newState)
	{
		for (int i = 0; i < 13; i++)
		{
			if (newState)
			{
				LevelImages[i].texture = GameWorldController.instance.grFlasks.LoadImageAt(i + 50);
			}
			else
			{
				LevelImages[i].texture = GameWorldController.instance.grFlasks.LoadImageAt(i);
			}
		}
	}

	public void OnClick()
	{
		if (Dragging || ConversationVM.InConversation)
		{
			return;
		}
		string text = "";
		if (isHealthDisplay)
		{
			switch (UWCharacter.Instance.play_poison)
			{
			case 1:
			case 2:
			case 3:
				text = "You are barely poisoned\n";
				break;
			case 4:
			case 5:
			case 6:
				text = "You are mildly poisoned\n";
				break;
			case 7:
			case 8:
			case 9:
				text = "You are badly poisoned\n";
				break;
			case 10:
			case 11:
			case 12:
				text = "You are seriously poisoned\n";
				break;
			case 13:
			case 14:
			case 15:
				text = "You are egregiously poisoned\n";
				break;
			}
			if (text != "")
			{
				UWHUD.instance.MessageScroll.Add(text);
				text = "";
			}
			text = "Your current vitality is " + UWCharacter.Instance.CurVIT + " out of " + UWCharacter.Instance.MaxVIT;
		}
		else
		{
			text = text + "Your current mana points are " + UWCharacter.Instance.PlayerMagic.CurMana + " out of " + UWCharacter.Instance.PlayerMagic.MaxMana;
		}
		UWHUD.instance.MessageScroll.Add(text);
	}
}
