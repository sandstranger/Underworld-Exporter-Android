using UnityEngine;

public class Action_Do_Nothing : MonoBehaviour
{
	public string ObjectToActivate;

	public void PerformAction()
	{
		GameObject gameObject = GameObject.Find(ObjectToActivate);
		Debug.Log(base.name + " Action Do Nothing");
		gameObject.SendMessage("Activate");
	}
}
