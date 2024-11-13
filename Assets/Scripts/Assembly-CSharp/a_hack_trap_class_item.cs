using UnityEngine;

public class a_hack_trap_class_item : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(995);
		if (!(objectIntAt == null))
		{
			Object.Destroy(objectIntAt.GetComponent<object_base>());
			switch (UWCharacter.Instance.CharClass)
			{
			case 0:
				objectIntAt.item_id = 38;
				objectIntAt.WorldDisplayIndex = 38;
				objectIntAt.InvDisplayIndex = 38;
				objectIntAt.gameObject.AddComponent<Gloves>();
				break;
			case 1:
			case 2:
			case 4:
			case 7:
				objectIntAt.item_id = 244;
				objectIntAt.WorldDisplayIndex = 224;
				objectIntAt.InvDisplayIndex = 244;
				objectIntAt.gameObject.AddComponent<RuneStone>();
				break;
			case 3:
			case 5:
				objectIntAt.item_id = 4;
				objectIntAt.WorldDisplayIndex = 4;
				objectIntAt.InvDisplayIndex = 4;
				objectIntAt.gameObject.AddComponent<WeaponMelee>();
				break;
			case 6:
				objectIntAt.item_id = 60;
				objectIntAt.WorldDisplayIndex = 60;
				objectIntAt.InvDisplayIndex = 60;
				objectIntAt.gameObject.AddComponent<Shield>();
				break;
			}
			objectIntAt.UpdateAnimation();
		}
	}
}
