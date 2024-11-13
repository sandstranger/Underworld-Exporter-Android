using UnityEngine;

public class ShockButtonHandler : MonoBehaviour
{
	public int TriggerAction;

	private void OnMouseDown()
	{
		Debug.Log(base.name + " clicked");
		Activate();
	}

	private void Activate()
	{
		switch (TriggerAction)
		{
		case 0:
			base.transform.parent.GetComponent<Action_Do_Nothing>().PerformAction();
			break;
		case 1:
			base.transform.parent.GetComponent<Action_Transport_Level>().PerformAction();
			break;
		case 2:
			base.transform.parent.GetComponent<Action_Resurrection>().PerformAction();
			break;
		case 3:
			base.transform.parent.GetComponent<Action_Clone>().PerformAction();
			break;
		case 4:
			base.transform.parent.GetComponent<Action_Set_Variable>().PerformAction();
			break;
		case 6:
			base.transform.parent.GetComponent<Action_Activate>().PerformAction();
			break;
		case 7:
			base.transform.parent.GetComponent<Action_Lighting>().PerformAction();
			break;
		case 8:
			base.transform.parent.GetComponent<Action_Effect>().PerformAction();
			break;
		case 9:
			base.transform.parent.GetComponent<Action_Moving_Platform>().PerformAction();
			break;
		case 11:
			base.transform.parent.GetComponent<Action_Timer>().PerformAction();
			break;
		case 12:
			base.transform.parent.GetComponent<Action_Choice>().PerformAction();
			break;
		case 15:
			base.transform.parent.GetComponent<Action_Email>().PerformAction();
			break;
		case 16:
			base.transform.parent.GetComponent<Action_Radaway>().PerformAction();
			break;
		case 19:
			base.transform.parent.GetComponent<Action_Change_State>().PerformAction();
			break;
		case 21:
			base.transform.parent.GetComponent<Action_Awaken>().PerformAction();
			break;
		case 22:
			base.transform.parent.GetComponent<Action_Message>().PerformAction();
			break;
		case 23:
			base.transform.parent.GetComponent<Action_Spawn>().PerformAction();
			break;
		case 24:
			base.transform.parent.GetComponent<Action_Change_Type>().PerformAction();
			break;
		default:
			base.transform.parent.GetComponent<Action_Default>().PerformAction();
			break;
		}
	}
}
