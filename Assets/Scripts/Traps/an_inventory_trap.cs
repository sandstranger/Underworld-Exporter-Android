﻿using UnityEngine;
using System.Collections;

public class an_inventory_trap : trap_base {
	/*
   018c  an_inventory trap
         the trap searches for an item in the inventory; when it is found, the
         sp_link'ed trigger is set off. the item_id is given by
         ("quality" << 5) | "owner". additionally, the zpos value must be != 0
         to enable the trap.
	*/

	private bool ObjectFound;

	public override void ExecuteTrap (object_base src, int triggerX, int triggerY, int State)
	{
		//Debug.Log (this.name);
		int itemToFind = quality <<5 | owner;
		ObjectFound=false;
		ObjectInteraction foundObjInt = UWCharacter.Instance.playerInventory.findObjInteractionByID(itemToFind);
		if (foundObjInt!=null)
		{
			Debug.Log("Inventory trap " + this.name + " found " + foundObjInt.name);
			ObjectFound=true;
		}
        else
        {
           // Debug.Log("Inventory trap did not find item of id =" + itemToFind);
        }
	}

	public override void TriggerNext (int triggerX, int triggerY, int State)
	{
		if (ObjectFound==true)
		{
			base.TriggerNext (triggerX, triggerY, State);
		}
	}

	//public override void PostActivate (object_base src)
	//{//Do not destroy.
 //       Debug.Log("Overridden PostActivate to test " + this.name);
 //       base.PostActivate(src);
	//}

}
