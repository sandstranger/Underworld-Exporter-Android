using UnityEngine;

public class a_hack_trap_castle_npcs : a_hack_trap
{
	public enum BritanniaNPCS
	{
		MaleGuard = 129,
		Nystrul = 130,
		Charles = 131,
		Dupre = 132,
		Geoffrey = 133,
		Iolo = 134,
		Julia = 135,
		Miranda = 136,
		Nanna = 137,
		Nell = 138,
		Nelson = 139,
		Patterson = 140,
		Tory = 141,
		LordBritish = 142,
		Feridwyn = 143,
		FemaleGuard = 149,
		Syria = 168
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (Quest.instance.x_clocks[0] == 0)
		{
			EventsAtXClock0();
		}
		else
		{
			Debug.Log("what happens now???");
		}
	}

	private void EventsAtXClock0()
	{
		Cutscene_TauntBritish cs = UWHUD.instance.gameObject.AddComponent<Cutscene_TauntBritish>();
		UWHUD.instance.CutScenesFull.cs = cs;
		UWHUD.instance.CutScenesFull.Begin();
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Nell), 43, 49, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(227, 42, 35, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Charles), 36, 51, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(230, 29, 36, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(231, 30, 52, NPC.npc_goals.npc_goal_goto_1);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Patterson), 21, 34, NPC.npc_goals.npc_goal_goto_1);
		NPC.SetNPCLocation(233, 34, 35, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.LordBritish), 31, 52, NPC.npc_goals.npc_goal_goto_1);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Nelson), 22, 37, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Tory), 24, 39, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Nanna), 44, 48, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Feridwyn), 25, 34, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(248, 33, 52, NPC.npc_goals.npc_goal_goto_1);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Miranda), 27, 36, NPC.npc_goals.npc_goal_goto_1);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Geoffrey), 37, 35, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Julia), 25, 43, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Dupre), 21, 42, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Iolo), 22, 51, NPC.npc_goals.npc_goal_wander_2);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Nystrul), 42, 43, NPC.npc_goals.npc_goal_goto_1);
		NPC.SetNPCLocation(getNPC(BritanniaNPCS.Syria), 42, 36, NPC.npc_goals.npc_goal_wander_2);
	}

	private static int getNPC(BritanniaNPCS who)
	{
		return NPC.findNpcByWhoAmI((int)who);
	}

	public static void MakeEveryoneFriendly()
	{
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Nell), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Charles), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Patterson), NPC.npc_goals.npc_goal_goto_1, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.LordBritish), NPC.npc_goals.npc_goal_goto_1, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Nelson), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Tory), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Nanna), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Feridwyn), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Miranda), NPC.npc_goals.npc_goal_goto_1, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Geoffrey), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Julia), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Dupre), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Iolo), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Nystrul), NPC.npc_goals.npc_goal_goto_1, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.Syria), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.FemaleGuard), NPC.npc_goals.npc_goal_wander_2, 3);
		NPC.SetNPCAttitudeGoal(getNPC(BritanniaNPCS.MaleGuard), NPC.npc_goals.npc_goal_wander_2, 3);
	}
}
