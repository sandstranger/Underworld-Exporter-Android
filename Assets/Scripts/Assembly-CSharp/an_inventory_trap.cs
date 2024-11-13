using UnityEngine;

public class an_inventory_trap : trap_base
{
	private bool ObjectFound;

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		int num = (base.quality << 5) | base.owner;
		ObjectFound = false;
		ObjectInteraction objectInteraction = UWCharacter.Instance.playerInventory.findObjInteractionByID(num);
		if (objectInteraction != null)
		{
			Debug.Log("Inventory trap " + base.name + " found " + objectInteraction.name);
			ObjectFound = true;
		}
	}

	public override void TriggerNext(int triggerX, int triggerY, int State)
	{
		if (ObjectFound)
		{
			base.TriggerNext(triggerX, triggerY, State);
		}
	}
}
