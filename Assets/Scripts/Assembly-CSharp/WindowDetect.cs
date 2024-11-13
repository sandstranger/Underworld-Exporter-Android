using UnityEngine;

public class WindowDetect : GuiBase
{
	protected int cursorSizeX = 64;

	protected int cursorSizeY = 64;

	public bool MouseHeldDown = false;

	public bool FullScreen;

	public static bool WaitingForInput = false;

	public static bool InMap = false;

	public static bool CursorInMainWindow;

	public Rect CursorPosition;

	public RectTransform[] UIsToStore = new RectTransform[1];

	public Vector3[] UIPositionsWindowed = new Vector3[1];

	public Vector3[] UIPositionsFullScreen = new Vector3[1];

	public bool JustClicked;

	public float WindowWaitCount = 0f;

	public static bool ContextUIEnabled = true;

	public static bool ContextUIUse = true;

	public override void Start()
	{
		CursorPosition = new Rect(0f, 0f, cursorSizeX, cursorSizeY);
		if (FullScreen)
		{
			return;
		}
		for (int i = 0; i <= UIsToStore.GetUpperBound(0); i++)
		{
			UIPositionsWindowed[i] = UIsToStore[i].position;
			if (UIPositionsFullScreen[i] == Vector3.zero)
			{
				UIPositionsFullScreen[i] = UIsToStore[i].position;
			}
		}
	}

	public void updateUIPositions()
	{
		for (int i = 0; i <= UIsToStore.GetUpperBound(0); i++)
		{
			UIPositionsFullScreen[i] = UIsToStore[i].position;
		}
	}

	public void setPositions()
	{
		if (FullScreen)
		{
			for (int i = 0; i <= UIsToStore.GetUpperBound(0); i++)
			{
				UIsToStore[i].position = UIPositionsFullScreen[i];
			}
		}
		else
		{
			for (int j = 0; j <= UIsToStore.GetUpperBound(0); j++)
			{
				UIsToStore[j].position = UIPositionsWindowed[j];
			}
		}
	}

	protected virtual void OnHover(bool isOver)
	{
		CursorInMainWindow = isOver;
	}

	protected virtual void OnPress(bool isDown, int ptrID)
	{
		MouseHeldDown = isDown;
	}

	protected virtual void ThrowObjectInHand()
	{
	}
}
