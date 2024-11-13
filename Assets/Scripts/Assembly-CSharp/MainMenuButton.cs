using UnityEngine.UI;

public class MainMenuButton : GuiBase
{
	public int ButtonOffIndex;

	public int ButtonOnIndex;

	public RawImage ButtonBG;

	public int Value;

	public MainMenuHud SubmitTarget;

	private static GRLoader ButtonLoader;

	public override void Start()
	{
		string rES = UWEBase._RES;
		if (rES == null || !(rES == "UW0"))
		{
			if (ButtonLoader == null)
			{
				ButtonLoader = new GRLoader(21);
				ButtonLoader.PaletteNo = 6;
			}
			ButtonBG.texture = ButtonLoader.LoadImageAt(ButtonOffIndex, false);
		}
	}

	public void OnHoverEnter()
	{
		ButtonBG.texture = ButtonLoader.LoadImageAt(ButtonOnIndex, false);
	}

	public void OnHoverExit()
	{
		ButtonBG.texture = ButtonLoader.LoadImageAt(ButtonOffIndex, false);
	}

	public virtual void OnClick()
	{
		SubmitTarget.ButtonClickMainMenu(Value);
	}
}
