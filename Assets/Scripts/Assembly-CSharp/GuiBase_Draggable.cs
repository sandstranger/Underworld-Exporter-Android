using UnityEngine;
using UnityEngine.EventSystems;

public class GuiBase_Draggable : GuiBase
{
	public RectTransform[] rectT = new RectTransform[1];

	public bool Dragging;

	public override void Start()
	{
		base.Start();
	}

	public void DragStart()
	{
		if (UWHUD.instance.window.FullScreen)
		{
			Dragging = true;
		}
	}

	public void OnDrag(BaseEventData evnt)
	{
		if (!UWHUD.instance.window.FullScreen || ConversationVM.InConversation)
		{
			return;
		}
		PointerEventData pointerEventData = (PointerEventData)evnt;
		for (int i = 0; i <= rectT.GetUpperBound(0); i++)
		{
			if (rectT[i] != null)
			{
				rectT[i].position = new Vector3(rectT[i].position.x + pointerEventData.delta.x, rectT[i].position.y + pointerEventData.delta.y, rectT[i].position.z);
			}
		}
	}

	public void DragEnd()
	{
		if (UWHUD.instance.window.FullScreen)
		{
			UWHUD.instance.window.updateUIPositions();
		}
		Dragging = false;
	}
}
