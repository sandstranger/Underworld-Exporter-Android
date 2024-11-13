using UnityEngine;
using UnityEngine.UI;

public class ChargenButton : GuiBase
{
	public Text ButtonText;

	public RawImage ButtonImage;

	public Texture2D ButtonOff;

	public Texture2D ButtonOn;

	public RawImage ButtonBG;

	public int Value;

	public MainMenuHud SubmitTarget;

	private static GRLoader ButtonLoader;

	public override void Start()
	{
		base.Start();
		if (UWEBase._RES == "UW2" && ButtonText != null)
		{
			ButtonText.color = Color.white;
		}
	}

	public void OnHoverEnter()
	{
		ButtonBG.texture = ButtonOn;
	}

	public void OnHoverExit()
	{
		ButtonBG.texture = ButtonOff;
	}

	public virtual void OnClick()
	{
		SubmitTarget.ButtonClickMainMenu(Value);
	}
}
