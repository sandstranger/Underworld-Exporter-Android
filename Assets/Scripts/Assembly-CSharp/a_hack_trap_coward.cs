using UnityEngine;

public class a_hack_trap_coward : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i <= Quest.instance.ArenaOpponents.GetUpperBound(0); i++)
		{
			if (Quest.instance.ArenaOpponents[i] != 0)
			{
				num++;
				num2 = Quest.instance.ArenaOpponents[i];
				ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(num2);
				if (objectIntAt != null && objectIntAt.GetComponent<NPC>() != null)
				{
					objectIntAt.GetComponent<NPC>().npc_attitude = 1;
					objectIntAt.GetComponent<NPC>().npc_goal = 8;
				}
			}
			Quest.instance.ArenaOpponents[i] = 0;
		}
		if (num <= 0)
		{
			return;
		}
		Quest.instance.QuestVariables[129] = Mathf.Max(Quest.instance.QuestVariables[129] - num, 0);
		Quest.instance.QuestVariables[133] = 0;
		if (num2 > 0)
		{
			ObjectInteraction objectIntAt2 = ObjectLoader.getObjectIntAt(num2);
			if (objectIntAt2 != null)
			{
				objectIntAt2.TalkTo();
			}
		}
		else
		{
			Quest.instance.FightingInArena = false;
		}
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
