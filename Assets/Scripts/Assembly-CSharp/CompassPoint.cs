using UnityEngine.UI;

public class CompassPoint : GuiBase
{
	public int index;

	public override void Start()
	{
		GetComponent<RawImage>().texture = GameWorldController.instance.grCompass.LoadImageAt(index);
	}
}
