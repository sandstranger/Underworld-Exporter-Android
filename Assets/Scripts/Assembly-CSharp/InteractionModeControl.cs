using UnityEngine;
using UnityEngine.UI;

public class InteractionModeControl : GuiBase_Draggable
{
	public RawImage[] Controls = new RawImage[6];

	public InteractionModeControlItem[] ControlItems = new InteractionModeControlItem[6];

	public static bool UpdateNow = true;

	public OptionsMenuControl OptionsMenu;

	private GRLoader grlfti;

	public Texture2D[] UW2Buttons;

	public override void Start()
	{
		base.Start();
		grlfti = new GRLoader(19);
		if (UWEBase._RES == "UW2")
		{
			SetUpUW2OptBtns();
		}
		SetImage(ref Controls[0], 0);
	}

	public override void Update()
	{
		base.Update();
		if (!UpdateNow)
		{
			return;
		}
		UpdateNow = false;
		WindowDetect.ContextUIUse = Character.InteractionMode == 1 || Character.InteractionMode == 2 || 5 == Character.InteractionMode;
		for (int i = 1; i <= 5; i++)
		{
			if (i != Character.InteractionMode)
			{
				SetImage(ref Controls[i], i * 2);
			}
			else
			{
				SetImage(ref Controls[i], i * 2 + 1);
			}
		}
		if (Character.InteractionMode == 0)
		{
			if (UWCharacter.Instance.MouseLookEnabled)
			{
				WindowDetectUW.SwitchFromMouseLook();
			}
			OptionsMenu.gameObject.SetActive(true);
			OptionsMenu.initMenu();
			base.gameObject.SetActive(false);
		}
	}

	public void TurnOffOthers(int LeaveOn)
	{
		for (int i = 0; i <= 5; i++)
		{
			if (i != LeaveOn)
			{
				Controls[i].gameObject.GetComponent<InteractionModeControlItem>().isOn = false;
			}
		}
	}

	private void SetImage(ref RawImage img, int index)
	{
		if (UWEBase._RES == "UW2")
		{
			img.texture = UW2Buttons[index];
		}
		else
		{
			img.texture = grlfti.LoadImageAt(index);
		}
	}

	private void SetUpUW2OptBtns()
	{
		GRLoader gRLoader = new GRLoader(23);
		Texture2D dstImg = ArtLoader.CreateBlankImage(25, 14);
		UWHUD.instance.InteractionControlUW2BG.texture = gRLoader.LoadImageAt(2);
		UW2Buttons = new Texture2D[12];
		UW2Buttons[0] = ArtLoader.InsertImage(gRLoader.LoadImageAt(0), dstImg, -52, 0);
		UW2Buttons[1] = ArtLoader.InsertImage(gRLoader.LoadImageAt(1), dstImg, -52, 0);
		UW2Buttons[2] = ArtLoader.InsertImage(gRLoader.LoadImageAt(0), dstImg, 0, 0);
		UW2Buttons[3] = ArtLoader.InsertImage(gRLoader.LoadImageAt(1), dstImg, 0, 0);
		UW2Buttons[4] = ArtLoader.InsertImage(gRLoader.LoadImageAt(0), dstImg, -52, -15);
		UW2Buttons[5] = ArtLoader.InsertImage(gRLoader.LoadImageAt(1), dstImg, -52, -15);
		UW2Buttons[6] = ArtLoader.InsertImage(gRLoader.LoadImageAt(0), dstImg, -26, 0);
		UW2Buttons[7] = ArtLoader.InsertImage(gRLoader.LoadImageAt(1), dstImg, -26, 0);
		UW2Buttons[8] = ArtLoader.InsertImage(gRLoader.LoadImageAt(0), dstImg, 0, -15);
		UW2Buttons[9] = ArtLoader.InsertImage(gRLoader.LoadImageAt(1), dstImg, 0, -15);
		UW2Buttons[10] = ArtLoader.InsertImage(gRLoader.LoadImageAt(0), dstImg, -26, -15);
		UW2Buttons[11] = ArtLoader.InsertImage(gRLoader.LoadImageAt(1), dstImg, -26, -15);
	}
}
