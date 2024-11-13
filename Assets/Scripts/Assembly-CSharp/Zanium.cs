using UnityEngine;

public class Zanium : object_base
{
	public void SuckUpZanium()
	{
		ObjectInteraction objectInteraction = UWCharacter.Instance.playerInventory.findObjInteractionByID(base.item_id);
		if (objectInteraction != null)
		{
			objectInteraction.link += base.link;
			objectInteraction.isquant = 1;
			objInt().consumeObject();
			UWCharacter.Instance.playerInventory.Refresh();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == UWCharacter.Instance.name)
		{
			SuckUpZanium();
		}
	}
}
