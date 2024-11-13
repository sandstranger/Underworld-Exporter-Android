using UnityEngine;

public class Action_Change_State : MonoBehaviour
{
	public string ObjectToActivate;

	public int NewState;

	public void PerformAction()
	{
		Debug.Log("Action Change State");
		GameObject gameObject = GameObject.Find(ObjectToActivate);
		if (gameObject != null)
		{
			gameObject.SendMessage("Activate");
		}
		else
		{
			Debug.Log(base.name + " could not find " + ObjectToActivate);
		}
	}
}
