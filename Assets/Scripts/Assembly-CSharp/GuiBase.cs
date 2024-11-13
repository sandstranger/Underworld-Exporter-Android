using UnityEngine;

public class GuiBase : UWEBase
{
	public Vector3 anchorPos;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		if (GetComponent<RectTransform>() != null)
		{
			anchorPos = GetComponent<RectTransform>().anchoredPosition;
		}
	}
}
