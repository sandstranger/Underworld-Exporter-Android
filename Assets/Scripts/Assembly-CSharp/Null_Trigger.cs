using UnityEngine;

public class Null_Trigger : MonoBehaviour
{
	public int TriggerAction;

	public int[] conditions = new int[4];

	public bool TriggerOnce;

	public bool AlreadyTriggered;

	private void Activate()
	{
		switch (TriggerAction)
		{
		case 0:
			GetComponent<Action_Do_Nothing>().PerformAction();
			break;
		case 1:
			GetComponent<Action_Transport_Level>().PerformAction();
			break;
		case 2:
			GetComponent<Action_Resurrection>().PerformAction();
			break;
		case 3:
			GetComponent<Action_Clone>().PerformAction();
			break;
		case 4:
			GetComponent<Action_Set_Variable>().PerformAction();
			break;
		case 6:
			GetComponent<Action_Activate>().PerformAction();
			break;
		case 7:
			GetComponent<Action_Lighting>().PerformAction();
			break;
		case 8:
			GetComponent<Action_Effect>().PerformAction();
			break;
		case 9:
			GetComponent<Action_Moving_Platform>().PerformAction();
			break;
		case 11:
			GetComponent<Action_Timer>().PerformAction();
			break;
		case 12:
			GetComponent<Action_Choice>().PerformAction();
			break;
		case 15:
			GetComponent<Action_Email>().PerformAction();
			break;
		case 16:
			GetComponent<Action_Radaway>().PerformAction();
			break;
		case 19:
			GetComponent<Action_Change_State>().PerformAction();
			break;
		case 21:
			GetComponent<Action_Awaken>().PerformAction();
			break;
		case 22:
			GetComponent<Action_Message>().PerformAction();
			break;
		case 23:
			GetComponent<Action_Spawn>().PerformAction();
			break;
		case 24:
			GetComponent<Action_Change_Type>().PerformAction();
			break;
		default:
			GetComponent<Action_Default>().PerformAction();
			break;
		}
	}
}
