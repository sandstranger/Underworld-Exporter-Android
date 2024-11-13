public class event_remove_npc : event_base
{
	public override void ExecuteEvent()
	{
		base.ExecuteEvent();
		int whoAmI = RawData[4];
		NPC[] array = findNPC(whoAmI);
		if (array != null)
		{
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				array[i].gameObject.transform.position = UWCharacter.Instance.playerInventory.InventoryMarker.transform.position;
			}
		}
	}

	public override string EventName()
	{
		return "Remove_NPC";
	}

	public override string summary()
	{
		return base.summary() + "\n\t\tWhoAmI=" + (int)RawData[4];
	}
}
