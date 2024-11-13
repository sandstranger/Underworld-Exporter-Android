using UnityEngine;

public class ShockStartup : MonoBehaviour
{
	public StringController StringControl;

	private void Awake()
	{
		StringControl = new StringController();
		StringController.instance.InitStringController("c:\\shock_strings.txt");
		Words.sc = StringControl;
	}
}
