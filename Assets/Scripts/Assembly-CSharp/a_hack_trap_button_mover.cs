using UnityEngine;

public class a_hack_trap_button_mover : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		switch (src.owner)
		{
		case 1:
			MoveButton(950);
			MoveButton(951);
			break;
		case 2:
			MoveButton(951);
			MoveButton(952);
			break;
		case 4:
			MoveButton(952);
			MoveButton(953);
			break;
		case 8:
			MoveButton(953);
			MoveButton(954);
			break;
		case 16:
			MoveButton(954);
			MoveButton(950);
			break;
		default:
			Debug.Log("unknown switch to move " + src.owner);
			break;
		}
	}

	private void MoveButton(int index)
	{
		if (UWEBase.CurrentObjectList().objInfo[index].instance != null)
		{
			ObjectInteraction instance = UWEBase.CurrentObjectList().objInfo[index].instance;
			if (instance.zpos == base.zpos)
			{
				MoveButton((short)(base.zpos + base.owner), instance);
			}
			else
			{
				MoveButton(base.zpos, instance);
			}
		}
	}

	private void MoveButton(short NewZpos, ObjectInteraction buttonToMove)
	{
		buttonToMove.zpos = NewZpos;
		buttonToMove.objectloaderinfo.zpos = NewZpos;
		Vector3 position = ObjectLoader.CalcObjectXYZ(buttonToMove.objectloaderinfo.index, 0);
		buttonToMove.transform.position = position;
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
