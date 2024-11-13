using UnityEngine;

public class ScrollButtonInventory : Scrollbutton
{
	public short stepSize;

	public static short ScrollValue = 0;

	public short MaxScrollValue;

	public short MinScrollValue;

	private int previousScrollValue = -1;

	public void OnClick()
	{
		ScrollValue += stepSize;
		int num = UWCharacter.Instance.playerInventory.GetCurrentContainer().CountItems();
		MaxScrollValue = (short)Mathf.Max((num / 4 - 1) * 4, 8);
		if (ScrollValue > MaxScrollValue)
		{
			ScrollValue = MaxScrollValue;
		}
		if (ScrollValue < MinScrollValue)
		{
			ScrollValue = MinScrollValue;
		}
	}

	public override void Update()
	{
		base.Update();
		if (ScrollValue != previousScrollValue)
		{
			previousScrollValue = ScrollValue;
			UWCharacter.Instance.playerInventory.ContainerOffset = ScrollValue;
			UWCharacter.Instance.playerInventory.Refresh();
		}
	}
}
