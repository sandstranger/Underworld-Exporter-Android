using UnityEngine.UI;

public class chains : GuiBase_Draggable
{
	public static int ActiveControl;

	public override void Start()
	{
		base.Start();
		GRLoader gRLoader = new GRLoader(6);
		GetComponent<RawImage>().texture = gRLoader.LoadImageAt(0);
	}

	public void OnClick()
	{
		if (!Dragging && !UWHUD.instance.isRotating)
		{
			switch (ActiveControl)
			{
			default:
				return;
			case 0:
				ActiveControl = 1;
				break;
			case 1:
				ActiveControl = 0;
				break;
			case 2:
				ActiveControl = 0;
				break;
			}
			Refresh();
		}
	}

	public void Refresh()
	{
		UWHUD.instance.RefreshPanels(ActiveControl);
	}
}
