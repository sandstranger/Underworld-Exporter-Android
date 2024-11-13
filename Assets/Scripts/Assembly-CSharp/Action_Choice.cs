using UnityEngine;

public class Action_Choice : MonoBehaviour
{
	public bool state = true;

	public string ActivateTrue;

	public string ActivateFalse;

	private GameObject ActivateObjectTrue;

	private GameObject ActivateObjectFalse;

	private void Start()
	{
		ActivateObjectTrue = GameObject.Find(ActivateTrue);
		ActivateObjectFalse = GameObject.Find(ActivateFalse);
	}

	public void PerformAction()
	{
		Debug.Log(base.name + " Action Choice");
		if (state)
		{
			state = false;
			ActivateObjectTrue.SendMessage("Activate");
		}
		else
		{
			state = true;
			ActivateObjectFalse.SendMessage("Activate");
		}
	}
}
