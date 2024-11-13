using UnityEngine;

public class ObjectVariables : MonoBehaviour
{
	public int triggerX;

	public int triggerY;

	public int Hidden;

	public int state;

	public string trigger;

	private void Start()
	{
	}

	public GameObject findDoor(int x, int y)
	{
		return GameObject.Find("door_" + x.ToString("D3") + "_" + y.ToString("D3"));
	}
}
