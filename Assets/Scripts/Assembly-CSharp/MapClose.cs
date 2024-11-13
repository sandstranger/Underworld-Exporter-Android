using UnityEngine;

public class MapClose : MonoBehaviour
{
	public void OnClick()
	{
		Time.timeScale = 1f;
		WindowDetect.InMap = false;
		if (MusicController.instance != null)
		{
			MusicController.instance.InMap = false;
		}
		UWHUD.instance.RefreshPanels(0);
	}
}
